using CommonServiceLocator;
using log4net;
using Prism.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    public class MQCenter
    {
        private static ILog log;
        private Dictionary<string, object> expires = new Dictionary<string, object>() { { "x-expires", 1000 * 60 * 60 * 24 } };
        static MQCenter _instance;
        ConnectionFactory _factory = new ConnectionFactory();
        bool isQueueAlived = true;
        Thread T_Receive;
        private static IEventAggregator eventAggregator;

        public static MQCenter Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MQCenter();
                    log = LogManager.GetLogger("MQCenter");
                    eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
                }
                return _instance;
            }
        }

        public bool InitListen(string hostName, int port, string name, string pwd)
        {
            try
            {
                _factory.HostName = hostName;
                _factory.Port = port;
                _factory.UserName = name;
                _factory.Password = pwd;
                Console.WriteLine("MQ消息队列参数：hostName;" + hostName + " port:" + port + " name:" + name + " pwd:" + pwd);
                var connection = _factory.CreateConnection();

                if (connection != null && connection.IsOpen)
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare("terminal.direct", "direct", true, false);
                    }
                }
                return connection.IsOpen;
            }
            catch (Exception ex)
            {
                //////Console.WriteLine(ex.ToString());
                //////Common.Trace("mqInitListen Error:" + ex.Message);
                Console.WriteLine("MQCenter.cs Error 001 :" + ex.ToString());
                return false;
            }


        }

        public void CreateQueue(string QueueName)
        {
            try
            {
                isQueueAlived = false;
                T_Receive?.Abort();
                T_Receive = new Thread(() =>
                {
                    isQueueAlived = true;
                    while (!QueueReceive(QueueName))
                    {
                        Thread.Sleep(100);
                    }
                });
                T_Receive.Start();


            }
            catch (Exception ex)
            {
                //////Console.WriteLine(ex.ToString());
                log.Error(ex);
                Console.WriteLine("MQCenter.cs Error 002 :" + ex.ToString());
            }

        }

        private void Send(string routingKey, string msg)
        {
            try
            {
                using (IConnection connection = _factory?.CreateConnection())
                {
                    using (IModel channel = connection?.CreateModel())
                    {
                        channel.QueueDeclare(queue: routingKey.ToString(),
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                        channel.QueueBind(routingKey.ToString(), "terminal.direct", routingKey.ToString(), null);
                        var props = channel.CreateBasicProperties();
                        props.Persistent = true;
                        channel.BasicPublish("terminal.direct", routingKey, props, Encoding.UTF8.GetBytes(msg)); //生产消息
                    }
                }

            }
            catch (Exception ex)
            {
                //////Console.WriteLine("SendMQ Error:" + ex.Message.ToString());
                //////Common.Trace("mqSend Error:" + msg);
                Console.WriteLine("MQCenter.cs Error 003 :" + ex.ToString());
            }
        }

        public void Send(StudentInfo student, MessageType type, object data)
        {
            Task.Run(() =>
            {
                try
                {
                    var msg = new Message { classId = Common.CurrentClassV2.ClassId, lessonId = Common.CurrentLesson.Id, classRoomId = Common.CurrentClassRoomV2.RoomId, sendUserId = Common.CurrentUser.Id, userType = UserType.teacher, data = data, type = type }.ToJson();
                    //System.Windows.Forms.MessageBox.Show(msg);
                    //Console.WriteLine("单独发送MQ消息：" + student.Name + "-->" + msg);
                    Console.WriteLine("单独发送MQ消息：第[" + student.Group + "]组 " + student.Name + "（" + student.Id + "） " + student.IP);
                    string routingKey = string.Format("student.{0}", student.IP);
                    Send(routingKey, msg);

                }
                catch (Exception ex)
                {
                    //////Console.WriteLine(ex.ToString());
                    //////Common.Trace("mqSend Error:" + ex.Message);
                    Console.WriteLine("MQCenter.cs Error 004 :" + ex.ToString());
                }

            });
        }

        public void SendToAll(MessageType type, object data)
        {
            Task.Run(() =>
            {
                try
                {
                    string s = "";
                    if (type == MessageType.ShareUsbCamera)//1号位
                    {
                        s = "1号位摄像头";
                    }
                    else if (type == MessageType.ShareDemo)//2号位
                    {
                        s = "2号位摄像头";
                    }
                    else if (type == MessageType.ShareClassRoomBack)//3号位
                    {
                        s = "3号位摄像头";
                    }

                    var msg = new Message
                    {
                        classId = Common.CurrentClassV2.ClassId,
                        lessonId = Common.CurrentLesson.Id,
                        classRoomId = Common.CurrentClassRoomV2.RoomId,
                        sendUserId = Common.CurrentUser.Id,
                        userType = UserType.teacher,
                        data = data,
                        type = type
                    }.ToJson();

                    Console.WriteLine(string.Format("群发MQ消息[1]{0}:{1}", s, msg));
                    foreach (var item in Common.StudentList)
                    {
                        string routingKey = string.Format("student.{0}", item.IP);
                        Send(routingKey, msg);
                    }
                }
                catch (Exception ex)
                {
                    //////Console.WriteLine(ex.ToString());
                    //////log.Error(ex);
                    Console.WriteLine("MQCenter.cs Error 005 :" + ex.ToString());
                }
            });
        }

        /// <summary>
        /// 发送到除开id外的所有学生
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="data"></param>
        public void SendToAll(long id, MessageType type, object data)
        {
            Task.Run(() =>
            {
                try
                {
                    var msg = new Message { classId = Common.CurrentClassV2.ClassId, lessonId = Common.CurrentLesson.Id, classRoomId = Common.CurrentClassRoomV2.RoomId, sendUserId = Common.CurrentUser.Id, userType = UserType.teacher, data = data, type = type }.ToJson();
                    Console.WriteLine("群发MQ消息[2]:" + msg);
                    foreach (var item in Common.StudentList)
                    {
                        if (id != item.Id)
                        {
                            string routingKey = string.Format("student.{0}", item.IP);
                            Send(routingKey, msg);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //////Console.WriteLine(ex.ToString()); 
                    log.Error(ex); 
                    Console.WriteLine("MQCenter.cs Error 006 :" + ex.ToString());
                }

            });
        }

        /// <summary>
        /// 清除队列
        /// </summary>
        /// <param name="queueName"></param>
        public void QueuePurge(string queueName)
        {
            using (IConnection connection = _factory?.CreateConnection())
            {
                using (IModel channel = connection?.CreateModel())
                {
                    //清空学生队列 ly 2024-03-05
                    if (queueExists(channel, queueName.ToString()))
                    {
                        channel.QueuePurge(queueName.ToString());
                    }
                    else
                    {
                        try
                        {
                            //Send(queueName, "");
                            channel.QueueDeclare(queue: queueName.ToString(),
                            durable: true,
                            exclusive: false,
                            autoDelete: false,
                            arguments: null);
                            channel.QueueBind(queueName.ToString(), "terminal.direct", queueName.ToString(), null);
                            var props = channel.CreateBasicProperties();
                            props.Persistent = true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("MQCenter.cs Error 007 :" + ex.ToString());
                        }
                        finally
                        {

                        }
                    }
                }
            }
        }

        /// <summary>
        /// 验证队列是否存在
        /// </summary>
        /// <param name="model"></param>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public bool queueExists(IModel model, string queueName)
        {
            try
            {
                model.QueueDeclarePassive(queueName);
                return true;
            }
            catch (OperationInterruptedException ex)
            {
                Console.WriteLine("MQCenter.cs Error 008 :" + ex.ToString());
                return false;
            }
        }

        public void ClearQueue()
        {
            isQueueAlived = false;
            T_Receive?.Abort();
        }

        /// <summary>
        /// 接收队列
        /// </summary>
        /// <param name="QueueName"></param>
        /// <returns></returns>
        public bool QueueReceive(object QueueName)
        {
            try
            {
                using (var connection = _factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare("terminal.direct", "direct", true, false);
                        channel.QueueDeclare(queue: QueueName.ToString(),
                                   durable: true,
                                   exclusive: false,
                                   autoDelete: false,
                                   arguments: null);
                        channel.QueueBind(QueueName.ToString(), "terminal.direct", QueueName.ToString(), null);
                        Console.WriteLine("教师队列：" + QueueName.ToString());
                        EventingBasicConsumer QConsumer = new EventingBasicConsumer(channel);

                        QConsumer.Received += (model, ea) =>
                        {
                            try
                            {
                                var body = ea.Body;
                                var msgJson = Encoding.UTF8.GetString(body);
                                //Console.WriteLine("mq 接收到 QueueReceive：" + msgJson);
                                //System.Windows.Forms.MessageBox.Show("mq 接收到 QueueReceive：" + msgJson);
                                if (msgJson.ToObject<Message>() is Message fanout)
                                {
                                    //System.Windows.Forms.MessageBox.Show("转码成功：" + fanout.ToJson());
                                    var eventType = eventAggregator.GetEvent<MQMessageEvent>();
                                    eventType.Publish(fanout);
                                    // log.Info("MQINFO:___"+msgJson);
                                }

                                channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                            }
                            catch (Exception ex)
                            {
                                //////Console.WriteLine(ex.ToString());
                                //////Common.Trace("QueueReceive Error:" + ex.Message);
                                Console.WriteLine("MQCenter.cs Error 009 :" + ex.ToString());
                            }
                        };
                        channel.BasicConsume(queue: QueueName.ToString(), false, QConsumer);
                        while (isQueueAlived)
                        {
                            if (!channel.IsOpen)
                            {
                                return false;
                            }
                            Thread.Sleep(1);
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                //////log.Error(ex);
                Console.WriteLine("MQCenter.cs Error 010 :" + ex.ToString());
            }
            return false;
        }
    }
}

using Calligraphyassistant.Message;
using CommonServiceLocator;
using FFmpeg.AutoGen;
using Google.Protobuf;
using Newtonsoft.Json;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    public class StudentSocketClient : SocketClient
    {
        private VideoDecoder videoDecoder = null;
        private Size lastImageSize = Size.Empty;
        private VideoDecoder previewVideoDecoder = null;
        private Size previewLastImageSize = Size.Empty;
        private IEventAggregator eventAggregator;

        public StudentInfo StudentInfo { get; set; }
        public StudentSocketClient(Socket socket) : base(socket)
        {
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
#if DEV
            StudentInfo = Common.StudentList.FirstOrDefault();
#endif
#if NODEV
            StudentInfo = Common.StudentList.FirstOrDefault(p => p.IP == ((IPEndPoint)socket.RemoteEndPoint).Address.ToString());
#endif
            if (StudentInfo != null)
            {
                StudentInfo.IsConnected = true;
                OnDisconnected += (x, y) => { StudentInfo.IsConnected = false; };
            }
        }

        public new void Dispose()
        {
            if (!IsDisposed)
            {
                ReleaseVideoDecoder();
                if (previewVideoDecoder != null)
                {
                    previewVideoDecoder.CloseAVCodec();
                    previewVideoDecoder = null;
                }
            }
            base.Dispose();
        }

        public new void Send(byte[] data)
        {
            if (StudentInfo == null || !StudentInfo.IsLogined)
            {
                return;
            }
            base.Send(data);
        }

        public void ReleaseVideoDecoder()
        {
            if (videoDecoder != null)
            {
                videoDecoder.CloseAVCodec();
                videoDecoder = null;
            }
        }

        public override void ProcessMessage(RemoteMessage remoteMessage)
        {
            if (remoteMessage != null)
            {
                switch ((ClientMessageType)remoteMessage.MessageId)
                {
                    case ClientMessageType.Login:
                        Login(remoteMessage);
                        break;
                    case ClientMessageType.Heartbeat:
                        break;
                    case ClientMessageType.SendPhoto:
                        ReceivePhoto(remoteMessage);
                        break;
                    case ClientMessageType.SendPreviewVideoFrame:
                    case ClientMessageType.SendVideoFrame:
                        ReceiveVideoFrame(remoteMessage);
                        break;
                    case ClientMessageType.GetLastImageList:
                        eventAggregator.GetEvent<GetLastImageListEvent>().Publish(new CommonModel()
                        {
                            Student = StudentInfo,
                            Sender = this
                        });
                        break;
                    default:
                        break;
                }
            }
            base.ProcessMessage(remoteMessage);
        }

        private void Login(RemoteMessage remoteMessage)
        {
            LoginMessage loginMessage = LoginMessage.Parser.ParseFrom(remoteMessage.Data);
            if (loginMessage != null)
            {
                IPEndPoint iPEndPoint = (IPEndPoint)socket.RemoteEndPoint;
                string remoteIP = iPEndPoint.Address.ToString();
#if DEV
                StudentInfo studentInfo = Common.StudentList.FirstOrDefault();
#endif
#if NODEV
                StudentInfo studentInfo = Common.StudentList.FirstOrDefault(p => p.IP == loginMessage.IP || p.IP == remoteIP);
#endif
                LoginFeedbackMessage loginFeedback = new LoginFeedbackMessage();
                if (studentInfo == null)
                {
                    loginFeedback.Result = 0;
                }
                else
                {
                    loginFeedback.Result = 1;
                    loginFeedback.Name = studentInfo.Name;
                    loginFeedback.Subject = Common.CurrentLesson.Name;
                    loginFeedback.Group = studentInfo.Group.ToString();
                    StudentInfo = studentInfo;
                }
                byte[] data = new byte[loginFeedback.CalculateSize()];
                CodedOutputStream stream = new CodedOutputStream(data);
                loginFeedback.WriteTo(stream);
                stream.Dispose();
                data = RemoteMessage.CreateRemoteMessage((int)ServerMessageType.LoginFeedback, RemoteClientType.Server, data);
                if (StudentInfo != null)
                {
                    StudentInfo.IsLogined = true;
                }
                base.Send(data);
                if (studentInfo != null)
                {
                    eventAggregator.GetEvent<StudentLoginEvent>().Publish(new CommonModel()
                    {
                        Student = StudentInfo,
                        Sender = this
                    });
                }
            }
        }

        private void ReceivePhoto(RemoteMessage remoteMessage)
        {
            ImageMessage message = ImageMessage.Parser.ParseFrom(remoteMessage.Data);
            if (message != null)
            {
                try
                {
                    string tempPath = Path.Combine(Common.SettingsPath, "Temp");
                    if (!Directory.Exists(tempPath))
                    {
                        Directory.CreateDirectory(tempPath);
                    }
                    tempPath = Path.Combine(tempPath, "Student_" + StudentInfo.IP + "_" + DateTime.Now.Ticks + ".png");
                    File.WriteAllBytes(tempPath, message.Data.ToByteArray());
                    eventAggregator.GetEvent<TakePhotosEvent>().Publish(new TakePhotosModel()
                    {
                        Student = StudentInfo,
                        ImagePath = tempPath,
                        Sender = this
                    });
                    Task.Run(() =>
                    {
                        StudentInfo studentInfo = StudentInfo;
                        if (!Common.SubmitStudentWork(tempPath, studentInfo))
                        {
                            Common.Debug("教师拍照 学生姓名：" + studentInfo.Name + "作品上传失败！");
                            Common.ShowTip("教师拍照：" + studentInfo.Name + " 作品上传失败！");
                        }
                        else
                        {
                            Common.ShowTip("教师拍照：" + studentInfo.Name + " 作品上传成功！");
                        }
                    });
                }
                catch (Exception ex)
                {
                    Common.Trace("StudentSocketClient ReceiveImage Error:" + ex.ToString());
                }
            }
        }

        [HandleProcessCorruptedStateExceptions]
        private void ReceiveVideoFrame(RemoteMessage remoteMessage)
        {
            VideoFrameMessage message = VideoFrameMessage.Parser.ParseFrom(remoteMessage.Data);
            if (message != null)
            {
                int width = message.Width;
                int height = message.Height;
                byte[] packet = message.Packet.ToByteArray();
                VideoDecoder decoder;
                dynamic eventType;
                if ((ClientMessageType)remoteMessage.MessageId == ClientMessageType.SendPreviewVideoFrame)
                {
                    if (previewLastImageSize.Width != width || previewLastImageSize.Height != height)
                    {
                        if (previewVideoDecoder != null)
                        {
                            previewVideoDecoder.CloseAVCodec();
                        }
                        previewVideoDecoder = new VideoDecoder();
                        previewVideoDecoder.InitAVCodec(width, height, 15);
                        previewLastImageSize.Width = width;
                        previewLastImageSize.Height = height;
                    }
                    decoder = previewVideoDecoder;
                    eventType = eventAggregator.GetEvent<PreviewVideoFrameEvent>();
                }
                else
                {
                    if (lastImageSize.Width != width || lastImageSize.Height != height || videoDecoder == null)
                    {
                        if (videoDecoder != null)
                        {
                            videoDecoder.CloseAVCodec();
                        }
                        videoDecoder = new VideoDecoder();
                        videoDecoder.InitAVCodec(width, height, 15);
                        lastImageSize.Width = width;
                        lastImageSize.Height = height;
                    }
                    decoder = videoDecoder;
                    eventType = eventAggregator.GetEvent<VideoFrameEvent>();
                }
                try
                {
                    if (decoder != null && decoder.DecodeVideoFrame(packet, packet.Length))
                    {
                        if (StudentInfo != null && StudentInfo.IsSelected)
                        {
                            eventType.Publish(new VideoFrameModel()
                            {
                                Sender = this,
                                Student = StudentInfo,
                                Buffer = decoder.RGB24Data,
                                Frame = message.Frame,
                                Width = width,
                                Height = height
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Common.Trace("StudentSocketClient ReceiveVideoFrame Error:" + ex.ToString());
                }
            }
        }
    }
}

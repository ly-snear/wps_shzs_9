using CalligraphyAssistantMain.Code;
using CommonServiceLocator;
using Newtonsoft.Json;
using Prism.Events;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalligraphyAssistantMain.Controls
{
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// StudentsAskQuestionControl.xaml 的交互逻辑
    /// </summary>
    public partial class StudentsAskQuestionControl : System.Windows.Controls.UserControl
    {
        public ObservableCollection<StudentPrompts> DataCollectionPaging { get; set; }=new ObservableCollection<StudentPrompts>();
        public List<StudentPrompts> Data{ get; set; } = new List<StudentPrompts>();
        public Pager<StudentPrompts> Pager { get; set; }
        private IEventAggregator eventAggregator;
        public StudentsAskQuestionControl()
        {
            InitializeComponent();
            this.DataContext = this;
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<MQMessageEvent>().Subscribe(Messaging);

            //Task.Run(() =>
            //{
            //    while (true)
            //    {
            //        if (Data.Count < 13)
            //        {
            //            this.Dispatcher.Invoke(() =>
            //            {
            //                Data.Add(new StudentPrompts() { id = 12, name = "张三1"+DateTime.Now.Second, type = MessageType.AskQuestion });
            //                InitData();
            //            });
            //        }
            //        Thread.Sleep(3000);
            //    }
            //});
        }
        private void InitData()
        {
            if (Data.Count > 0)
            {
                Pager = new Pager<StudentPrompts>(Data.Count > 5 ? 5 : Data.Count, Data);
                Pager.PagerUpdated += items =>
                {
                    DataCollectionPaging = new ObservableCollection<StudentPrompts>(items);
                };
                Pager.CurPageIndex = 1;
            }
            else
            {
                
            }

        }
        public void Clear()
        {
            DataCollectionPaging.Clear();
            Data.Clear();
        }
        private void btnClose_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(sender is Border border && border.Tag is StudentPrompts prompts)
            {
                DataCollectionPaging.Remove(prompts);
                Data.Remove(prompts);
                InitData();
            }

        }
        private void Messaging(Code.Message msg)
        {
            //System.Windows.Forms.MessageBox.Show("接收到消息：" + msg.classId);
            try
            {
                if (msg != null && msg.classId == Common.CurrentClassV2.ClassId && msg.lessonId == Common.CurrentLesson.Id && msg.userType == UserType.student)
                {
                    switch (msg.type)
                    {
                        case MessageType.HandUp:
                        case MessageType.AskQuestion:
                            {
                                var data = new { id = 0,name="" };
                                dynamic json = JsonConvert.DeserializeAnonymousType(msg.data.ToString(), data);
                                if (json != null)
                                {
                                    var info = Common.StudentList.FirstOrDefault(p => p.Id == json.id);
                                    this.Dispatcher.Invoke(() =>
                                    {
                                        Data.Add(new StudentPrompts()
                                        {
                                            id = json.id,
                                            name = json.name,
                                            type = msg.type
                                        });
                                        InitData();
                                    });

                                }

                            }
                            break;

                    }
                }
            }
            catch (Exception)
            {

            }

        }
    }
}

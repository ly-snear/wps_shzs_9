using CalligraphyAssistantMain.Code;
using CommonServiceLocator;
using Newtonsoft.Json;
using Prism.Events;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalligraphyAssistantMain.Controls
{
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// PaperControl.xaml 的交互逻辑
    /// </summary>
    public partial class PaperControl : UserControl
    {
        public ObservableCollection<PaperInfo> PaperCollectionPaging { get; set; }
        public Pager<PaperInfo> Pager { get; set; }
        private IEventAggregator eventAggregator;
        public PaperControl()
        {
            InitializeComponent();
            this.DataContext = this;
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<MQMessageEvent>().Subscribe(Messaging);
        }
        public void InitData()
        {
            if (Common.PaperList.Count > 0)
            {
                view.Visibility = Visibility.Visible;
                Pager = new Pager<PaperInfo>(Common.PaperList.Count > 5 ? 5 : Common.PaperList.Count, Common.PaperList);
                Pager.PagerUpdated += items =>
                {
                    PaperCollectionPaging = new ObservableCollection<PaperInfo>(items);
                };
                Pager.CurPageIndex = 1;
            }
            else
            {
                view.Visibility = Visibility.Hidden;
            }

        }

        private void statistics_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is PaperInfo info)
            {
                PaperStatisticControl paperStatisticControl = new PaperStatisticControl(info);
                paperStatisticControl.ShowDialog();
            }
        }

        private void start_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is PaperInfo info)
            {
                
                SelectStudentsControl selectStudentsControl = new SelectStudentsControl();
                selectStudentsControl.BindStudentList(Common.CameraList);
                if (selectStudentsControl.ShowDialog() == true)
                {
                    List<StudentInfo> students = new List<StudentInfo>();
                    List<ResourceDispensedUser> users = new List<ResourceDispensedUser>();
                    selectStudentsControl.CameraItemInfos.ForEach(p => p.StudentList.ForEach(s =>
                    {
                        if (s.IsSelected)
                        {
                            students.Add(s);
                            users.Add(new ResourceDispensedUser() { Id = s.Id, UserName = s.Name, ip = s.IP });
                        }
                    }));

                    if (students.Count > 0)
                    {
                       
                        info.DispensedUsers = users;
                        //通知选中的学生
                        if (students.Count > 0)
                        {
                            Task.Run(() =>
                            {
                                students.ForEach(s => MQCenter.Instance.Send(s, MessageType.PaperDistribute, new
                                {
                                    info.id,
                                    info.title,
                                }));
                            });
                        }
                        info.status = "1";
                    }
                }

            }
        }

        private void stop_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //通知所有学生该答题项关闭

            if (sender is Border border && border.Tag is PaperInfo info)
            {
                info.status = "2";
                MQCenter.Instance.SendToAll(MessageType.StopPaper, new { info.id });
            }
        }

        private void ReturnBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
        private void Messaging(Code.Message msg)
        {
            try
            {
                if (msg != null && msg.classId == Common.CurrentClassV2.ClassId && msg.lessonId == Common.CurrentLesson.Id && msg.userType == UserType.student)
                {
                    switch (msg.type)
                    {
                        case MessageType.SubmitPaper:
                            {
                                var data = new { id = 0 };
                                dynamic json = JsonConvert.DeserializeAnonymousType(msg.data.ToString(), data);
                                if (json != null)
                                {
                                    var item = Common.PaperList.FirstOrDefault(p => p.id == json.id);
                                    if (item != null)
                                    {
                                        item.DispensedUsers.ForEach(p =>
                                        {
                                            if (p.Id == msg.sendUserId)
                                            {
                                                p.IsComplete = true;
                                            }
                                        });
                                        item.CompleteCount = item.DispensedUsers.Count(p => p.IsComplete);
                                        item.paperResult = Common.GetPaperResultInfo(item.id);
                                    }

                                }

                            }
                            break;

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }
}

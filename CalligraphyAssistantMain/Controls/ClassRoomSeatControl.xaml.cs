using CalligraphyAssistantMain.Code;
using CommonServiceLocator;
using Newtonsoft.Json;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// <summary>
    /// ClassRoomSeatControl.xaml 的交互逻辑
    /// </summary>
    public partial class ClassRoomSeatControl : UserControl
    {
        public static bool initOK = false;
        public List<CameraItemInfo> studentGroups = null;
        public int minCol = 1;
        public int maxCol = 1;
        public int minRow = 1;
        public int maxRow = 1;
        private List<StudentControl> listSeat = new List<StudentControl>();
        private IEventAggregator eventAggregator;
        public ClassRoomSeatControl()
        {
            InitializeComponent();
            this.IsVisibleChanged += ClassRoomSeatControl_IsVisibleChanged;
            EventNotify.StudentCompleteMessage += EventNotify_StudentCompleteMessage;
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<MQMessageEvent>().Subscribe(Messaging);
        }

        private void EventNotify_StudentCompleteMessage(object sender, long e)
        {
            MessageBox.Show("学生ID-->" + e.ToString());
        }

        private void ClassRoomSeatControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue && !initOK)
            {
                initData();
                initUI();
                showSeat();
                initOK = true;
            }
        }

        public void initData()
        {
            studentGroups = Common.CameraList.Clone();
            string classCaption = Common.CurrentClass.Name;
            string roomCaption = Common.CurrentClassRoomV2.Name;
            string caption = $"{roomCaption}---{classCaption}";
            className.Text = caption;
        }

        public void initUI()
        {
            //MessageBox.Show(studentGroups.ToJson());
            Brush c1 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3C3C3C"));
            Brush c2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0000"));
            foreach (CameraItemInfo g in studentGroups)
            {
                foreach (StudentInfo s in g.StudentList)
                {
                    if (s.Col < minCol)
                    {
                        minCol = s.Col;
                    }
                    if (s.Col > maxCol)
                    {
                        maxCol = s.Col;
                    }
                    if (s.Row < minRow)
                    {
                        minRow = s.Row;
                    }
                    if (s.Row > maxRow)
                    {
                        maxRow = s.Row;
                    }
                    StudentControl seat = new StudentControl();
                    s.Foreground = c1;
                    seat.Id = s.Id;
                    seat.Name = s.Name;
                    seat.Cursor = Cursors.Hand;
                    //studentControl.MouseDoubleClick += StudentControl_MouseDoubleClick;
                    seat.IsComplete = false;
                    seat.DataContext = s;
                    seat.Tag = s;
                    listSeat.Add(seat);
                }
            }
            //MessageBox.Show($"minCol={minCol}-->maxCol={maxCol}-->minRow={minRow}-->maxRow={maxRow}");
            for (int i = minRow; i <= maxRow; i++)
            {
                RowDefinition rowDefinition = new RowDefinition();
                seats.RowDefinitions.Add(rowDefinition);
            }
            for (int i = minCol; i <= maxCol; i++)
            {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                seats.ColumnDefinitions.Add(columnDefinition);
            }

        }

        public void showSeat()
        {
            if (null == listSeat || 0 == listSeat.Count)
            {
                return;
            }
            seats.Children.Clear();
            foreach (StudentControl seat in listSeat)
            {
                seats.Children.Add(seat);
                Grid.SetRow(seat, (seat.DataContext as StudentInfo).Row - 1);
                Grid.SetColumn(seat, (seat.DataContext as StudentInfo).Col - 1);
            }
        }

        private void cancelGd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void Messaging(Message msg)
        {
            try
            {
                //MessageBox.Show("" + Common.CurrentLesson.Id + "-->" + Common.CurrentClassV2.ClassId);
                if (msg != null && msg.classId == Common.CurrentClassV2.ClassId && msg.lessonId == Common.CurrentLesson.Id && msg.userType == UserType.student)
                {
                    //MessageBox.Show("消息数据：" + msg.data.ToString());
                    switch (msg.type)
                    {
                        case MessageType.StudentComplete:
                            {
                                StudentSendMessage data = JsonConvert.DeserializeObject<StudentSendMessage>(msg.data.ToString());
                                if (null != data)
                                {
                                    //MessageBox.Show($"学生信息-->ID:{data.id}-->name:{data.name}--{this.Visibility}");
                                    if (this.Visibility != Visibility.Visible)
                                    {
                                        this.Dispatcher.Invoke(new Action(() =>
                                        {
                                            this.Visibility = Visibility.Visible;
                                        }));
                                    }
                                    if (data.id > 0)
                                    {
                                        StudentControl sc = listSeat.Find(s => s.Id == data.id);
                                        if (sc != null) {
                                            this.Dispatcher.Invoke(new Action(() => {
                                                Brush c2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00FF"));
                                                (listSeat.Find(s => s.Id == data.id).DataContext as StudentInfo).Foreground = c2;
                                            }));
                                        }
                                    }
                                }
                            }
                            break;

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("接收学生完成异常：" + ex.ToString());
            }

        }
    }
}

using CalligraphyAssistantMain.Code;
using CommonServiceLocator;
using Newtonsoft.Json;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalligraphyAssistantMain.Controls
{
    /// <summary>
    /// TaskCountControl.xaml 的交互逻辑
    /// </summary>
    public partial class TaskCountControl : UserControl
    {
        /// <summary>
        /// 拼图数据
        /// </summary>
        private MaterialCount mc { get; set; } = null;

        /// <summary>
        /// 统计数据
        /// </summary>
        private MaterialCountData mcd { get; set; } = new MaterialCountData();

        /// <summary>
        /// 学生列表数据
        /// </summary>
        private ObservableCollection<QuickAnswerStudent> quickAnswerStudents { get; set; } = new ObservableCollection<QuickAnswerStudent>();

        /// <summary>
        /// 学生坐位控件
        /// </summary>
        private ObservableCollection<QuickAnswerStudentControl> quickAnswerStudentControls { get; set; } = new ObservableCollection<QuickAnswerStudentControl>();

        /// <summary>
        /// 等待完成
        /// </summary>
        private string waitAnswer = "?";

        /// <summary>
        /// 教室学生分组
        /// </summary>
        private List<CameraItemInfo> studentGroups = new List<CameraItemInfo>();

        #region 学生坐位
        /// <summary>
        /// 开始列
        /// </summary>
        private int minCol = 1;

        /// <summary>
        /// 结束列
        /// </summary>
        private int maxCol = 1;

        /// <summary>
        /// 开始行
        /// </summary>
        private int minRow = 1;

        /// <summary>
        /// 结束行
        /// </summary>
        private int maxRow = 1;

        /// <summary>
        /// 控件宽度
        /// </summary>
        private int deskWidth = 200;

        /// <summary>
        /// 控件边距
        /// </summary>
        private int deskHMargin = 50;

        /// <summary>
        /// 控件高度
        /// </summary>
        private int deskHeight = 90;

        /// <summary>
        /// 控件边距
        /// </summary>
        private int deskVMargin = 25;
        #endregion

        #region 消息事件
        private IEventAggregator eventAggregator = null;
        #endregion

        /// <summary>
        /// 默认构造
        /// </summary>
        public TaskCountControl()
        {
            InitializeComponent();
            this.IsVisibleChanged += TaskCountControl_IsVisibleChanged;
        }

        /// <summary>
        /// 显示界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void TaskCountControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (null == mc || null == mc.students)
            {
                closeMaterialCount();
                return;
            }
            if ((bool)e.NewValue)
            {
                bindData();
                initStudent();
                initSeat();
                initCountdown();
                initMessage();
            }
        }

        /// <summary>
        /// 初始化数据.显示前
        /// </summary>
        /// <param name="_mc"></param>
        public void initData(MaterialCount _mc)
        {
            this.mc = _mc;
            Console.WriteLine(this.mc.ToJson());
            var data = new { };
            MQCenter.Instance.SendToAll(MessageType.CommitTaskStart, data);
        }


        /// <summary>
        /// 绑定数据
        /// </summary>
        private void bindData()
        {
            mcd.Total = mc.students.Count;
            mcount.DataContext = mcd;
        }

        /// <summary>
        /// 初始化学生
        /// </summary>
        private void initStudent()
        {
            quickAnswerStudents = new ObservableCollection<QuickAnswerStudent>();
            for (int i = 0; i < mc.students.Count; i++)
            {
                QuickAnswerStudent quickAnswerStudent = new QuickAnswerStudent();
                quickAnswerStudent.Id = mc.students[i].Id;
                quickAnswerStudent.Row = mc.students[i].Row;
                quickAnswerStudent.Col = mc.students[i].Col;
                quickAnswerStudent.Caption = waitAnswer;
                quickAnswerStudent.Name = mc.students[i].Name + quickAnswerStudent.Id;
                quickAnswerStudent.CaptionColor = new SolidColorBrush(Color.FromArgb(255, 34, 139, 34));
                quickAnswerStudent.NameColor = quickAnswerStudent.CaptionColor;
                quickAnswerStudent.BackColor = new SolidColorBrush(Color.FromArgb(255, 205, 133, 63));
                quickAnswerStudent.BorderColor = new SolidColorBrush(Color.FromArgb(255, 255, 20, 147));
                quickAnswerStudent.StartTime = 0;
                quickAnswerStudent.EndTime = 0;
                quickAnswerStudent.IsChecked = false;
                quickAnswerStudent.SubjectiveQuestionAnswer = waitAnswer;
                quickAnswerStudent.SubjectiveAudioUrl = string.Empty;
                quickAnswerStudent.Visibled = Visibility.Visible;
                quickAnswerStudents.Add(quickAnswerStudent);
            }
        }

        /// <summary>
        /// 初始化坐位
        /// </summary>
        private void initSeat()
        {
            quickAnswerStudentControls = new ObservableCollection<QuickAnswerStudentControl>();
            for (int i = 0; i < quickAnswerStudents.Count; i++)
            {
                QuickAnswerStudentControl quickAnswerStudentControl = new QuickAnswerStudentControl();
                quickAnswerStudentControl.DataContext = quickAnswerStudents[i];
                quickAnswerStudentControls.Add(quickAnswerStudentControl);
            }
            Brush c1 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3C3C3C"));
            Brush c2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF0000"));
            studentGroups = Common.CameraList.Clone();
            #region 学生坐位表行列
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
                }
            }
            #endregion
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
            seats.Children.Clear();
            foreach (QuickAnswerStudentControl seat in quickAnswerStudentControls)
            {
                seat.Cursor = Cursors.Hand;
                seats.Children.Add(seat);
                Grid.SetRow(seat, (seat.DataContext as QuickAnswerStudent).Row - 1);
                Grid.SetColumn(seat, (seat.DataContext as QuickAnswerStudent).Col - 1);
                //seat.MouseLeftButtonDown += Seat_MouseLeftButtonDown;
            }
        }

        /// <summary>
        /// 初始化倒计时
        /// </summary>
        private void initCountdown()
        {
            EventNotify.CountdownTrigger += EventNotify_CountdownTrigger;
            EventNotify.CountdownStopTrigger += EventNotify_CountdownStopTrigger;
            EventNotify.OnCountdownTrigger(2, () =>
            {
                countdownComplete(null);
            });
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        public void initMessage()
        {
            if (null == eventAggregator)
            {
                eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
                eventAggregator.GetEvent<MQMessageEvent>().Subscribe(Messaging);
            }
        }

        /// <summary>
        /// 关闭学习任务统计
        /// </summary>
        private void closeMaterialCount()
        {
            this.Visibility = Visibility.Collapsed;
            cdc.Stop();
            EventNotify.OnTaskCountClose(null, null);
        }

        /// <summary>
        /// 启动倒计时
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void EventNotify_CountdownTrigger(int id, Action act)
        {
            int tm = mc.countdown;
            if (tm <= 0)
            {
                tm = 1;
            }
            tm = tm * 60;
            cdc.Id = id;
            cdc.Start(tm, act);
        }

        /// <summary>
        /// 倒计时结束
        /// </summary>
        /// <param name="obj"></param>
        private void EventNotify_CountdownStopTrigger(int id)
        {
            if (cdc.Id == id)
            {
                cdc.Stop();
            }
        }

        /// <summary>
        /// 倒计时完成
        /// </summary>
        /// <param name="e"></param>
        private void countdownComplete(object e)
        {
            var data = new { };
            foreach (var s in this.mc.students)
            {
                MQCenter.Instance.Send(s, MessageType.CommitTaskStop, data);
            }
            Console.WriteLine("学习任务计时结束");
        }

        /// <summary>
        /// 播放文本
        /// </summary>
        /// <param name="text"></param>
        private void playText(string text)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
            synth.SetOutputToDefaultAudioDevice();
            synth.Volume = 100;
            synth.Speak(text);
            synth.Dispose();
            synth = null;
        }

        /// <summary>
        /// 闪烁
        /// </summary>
        /// <param name="id"></param>
        private void studentFlicker(long id)
        {
            if (null == quickAnswerStudentControls || 0 == quickAnswerStudentControls.Count)
            {
                MessageBox.Show($"没有初始化学生坐位", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int index = quickAnswerStudentControls.ToList().FindIndex(item => id == (item.DataContext as QuickAnswerStudent).Id);
            if (-1 == index)
            {
                MessageBox.Show($"没有找到学生：{id} 的坐位", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Border cn = quickAnswerStudentControls[index].FindName("_Container") as Border;
            if (null == cn)
            {
                MessageBox.Show($"没有找到学生坐位", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = 1.0;
            doubleAnimation.To = 0;
            doubleAnimation.Duration = TimeSpan.FromMilliseconds(50);
            doubleAnimation.AutoReverse = true;
            doubleAnimation.RepeatBehavior = new RepeatBehavior(10);
            Storyboard storyboard = new Storyboard();
            Storyboard.SetTarget(doubleAnimation, cn);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(System.Windows.Controls.Button.OpacityProperty));
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();
        }

        /// <summary>
        /// 处理学习任务完成逻辑
        /// </summary>
        /// <param name="cma"></param>
        private void handleStudentComment(CommitMaterialAnswer cma)
        {
            Console.WriteLine(cma.ToJson());
            string _message = string.Empty;
            if (null == cma)
            {
                _message = $"学生提交学习任务无效";
                Console.WriteLine(_message);
                return;
            }
            if (cma.student <= 0)
            {
                _message = $"学生提交学习任务完成消息数据错误-->需要提交学生ID";
                Console.WriteLine(_message);
                return;
            }
            int _student = quickAnswerStudents.ToList().FindIndex(item => item.Id == cma.student);
            if (_student < 0)
            {
                _message = $"学生提交学习任务完成消息数据错误-->无效学生ID：{cma.student}";
                Console.WriteLine(_message);
                return;
            }

            //处理统计

            this.mcd.Complete++;
            quickAnswerStudents[_student].StartTime = cma.start;
            quickAnswerStudents[_student].EndTime = cma.elapsed;
            quickAnswerStudents[_student].Caption = "完成";

            #region 播放语音
            this.Dispatcher.Invoke(new Action(() =>
            {
                string text = $",{quickAnswerStudents[_student].Name},完成";
                playText(text);
            }));
            #endregion

            #region 播放动画
            this.Dispatcher.Invoke(new Action(() =>
            {
                studentFlicker(cma.student);
            }));
            #endregion
        }

        /// <summary>
        /// 接收学生提交的消息
        /// </summary>
        /// <param name="msg"></param>
        private void Messaging(Message msg)
        {
            string _message = string.Empty;
            try
            {
                if (null == msg)
                {
                    _message = "消息无效";
                    Console.WriteLine(_message);
                    return;
                }
                if (msg.classId != Common.CurrentClassV2.ClassId)
                {
                    _message = $"提交教室：{msg.classId}-->上课教室：{Common.CurrentClassV2.ClassId}，不匹配";
                    Console.WriteLine(_message);
                    return;
                }
                if (msg.lessonId != Common.CurrentLesson.Id)
                {
                    _message = $"提交课堂：{msg.lessonId}-->上课课堂：{Common.CurrentLesson.Id}，不匹配";
                    Console.WriteLine(_message);
                    return;
                }
                if (msg.userType != UserType.student)
                {
                    _message = $"只能由学生提交";
                    Console.Write(_message);
                    return;
                }
                if (null == msg.data)
                {
                    _message = $"消息数据为空";
                    Console.Write(_message);
                    return;
                }
                switch (msg.type)
                {
                    case MessageType.CommitTaskComplete:
                        CommitMaterialAnswer cma = JsonConvert.DeserializeObject<CommitMaterialAnswer>(msg.data.ToString());
                        if (null == cma)
                        {
                            _message = $"消息数据无效：{msg.data}";
                            Console.Write(_message);
                            return;
                        }
                        this.Dispatcher.Invoke(() =>
                        {
                            handleStudentComment(cma);
                        });
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                _message = ex.ToJson();
                Console.WriteLine(_message);
            }
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void returnBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            closeMaterialCount();
        }
    }
}

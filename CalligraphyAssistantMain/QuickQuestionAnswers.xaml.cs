﻿using CalligraphyAssistantMain.Code;
using CalligraphyAssistantMain.Controls;
using CommonServiceLocator;
using Newtonsoft.Json;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Speech.Synthesis;
using Prism.Common;

namespace CalligraphyAssistantMain
{
    /// <summary>
    /// QuickQuestionAnswers.xaml 的交互逻辑
    /// </summary>
    public partial class QuickQuestionAnswers : Window
    {
        #region 绑定数据
        public QuickQuestionAnswersData Data { get; set; } = new QuickQuestionAnswersData();
        #endregion

        #region 问题数据

        /// <summary>
        /// 问题信息
        /// </summary>
        public QuickAnswerInfo QuickAnswer { get; set; } = new QuickAnswerInfo();

        #endregion

        #region 选择题选项
        /// <summary>
        /// 选择题选项
        /// </summary>
        public ObservableCollection<OptionItem> optionItems = new ObservableCollection<OptionItem>();


        /// <summary>
        /// 等待提交的答案
        /// </summary>
        private string waitAnswer = "?";

        /// <summary>
        /// 倍数
        /// </summary>
        private int multiple = 10;

        /// <summary>
        /// 抢答结果
        /// </summary>
        private CommitQuickAnswer firstAnswer = null;
        #endregion

        #region 学生列表
        public ObservableCollection<QuickAnswerStudent> quickAnswerStudents = new ObservableCollection<QuickAnswerStudent>();
        #endregion

        #region 学生坐位
        public ObservableCollection<QuickAnswerStudentControl> quickAnswerStudentControls = new ObservableCollection<QuickAnswerStudentControl>();
        #endregion

        #region 坐位行列
        /// <summary>
        /// 教室学生
        /// </summary>
        private List<CameraItemInfo> studentGroups = Common.CameraList.Clone();

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
        private IEventAggregator eventAggregator;
        #endregion

        #region 倒计时

        #endregion

        public QuickQuestionAnswers()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 问题构造
        /// </summary>
        /// <param name="info"></param>
        public QuickQuestionAnswers(QuickAnswerInfo info)
        {
            InitializeComponent();
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<MQMessageEvent>().Subscribe(Messaging);
            QuickAnswer = info;
            initData();
            initOptions();
            initSeat();
            EventNotify.CountdownTrigger += EventNotify_CountdownTrigger;
            EventNotify.CountdownStopTrigger += EventNotify_CountdownStopTrigger;
            EventNotify.OnCountdownTrigger(info.id, () =>
            {
                countdownComplete(info);
            });
            //MessageBox.Show("接收：" + QuickAnswer.ToJson());           
        }

        /// <summary>
        /// 倒计时完成
        /// </summary>
        /// <param name="e"></param>
        private void countdownComplete(object e)
        {
            //MessageBox.Show("结束：" + e.ToJson());
        }

        /// <summary>
        /// 倒计时结束
        /// </summary>
        /// <param name="obj"></param>
        private void EventNotify_CountdownStopTrigger(int id)
        {
            //MessageBox.Show("完成");
            if (cdc.Id == id)
            {
                cdc.Stop();
                //MessageBox.Show("执行");
            }
        }

        /// <summary>
        /// 启动倒计时
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void EventNotify_CountdownTrigger(int id, Action act)
        {
            //MessageBox.Show("启动");
            cdc.Id = id;
            cdc.Start(120, act);
        }

        /// <summary>
        /// 初始化问题
        /// 初始化学生坐位
        /// </summary>
        private void initOptions()
        {
            #region 初始化选择题统计图
            if (0 == QuickAnswer.answer_type && QuickAnswer.question > 0)
            {
                optionItems = new ObservableCollection<OptionItem>();
                for (int i = 0; i < QuickAnswer.question; i++)
                {
                    Random random = new Random();
                    OptionItem optionItem = new OptionItem();
                    optionItem.Caption = integerToLetter(i);
                    optionItem.Students = new List<QuickAnswerStudent>();
                    optionItem.Qty = 0;
                    optionItem.Width = optionItem.Qty * 10;
                    optionItem.Height = 20;
                    Thread.Sleep(10);
                    optionItem.Color = getRandBrush();
                    Thread.Sleep(10);
                    optionItem.BackColor = getRandBrush();
                    optionItems.Add(optionItem);
                }
            }
            qis.ItemsSource = optionItems;
            ois.ItemsSource = optionItems;
            #endregion

            #region 初始化学生及其坐位
            if (null != QuickAnswer.answer_students && QuickAnswer.answer_students.Count > 0)
            {
                quickAnswerStudents = new ObservableCollection<QuickAnswerStudent>();
                quickAnswerStudentControls = new ObservableCollection<QuickAnswerStudentControl>();
                for (int i = 0; i < QuickAnswer.answer_students.Count; i++)
                {
                    QuickAnswerStudent quickAnswerStudent = new QuickAnswerStudent();
                    quickAnswerStudent.Id = QuickAnswer.answer_students[i].Id;
                    quickAnswerStudent.Row = QuickAnswer.answer_students[i].Row;
                    quickAnswerStudent.Col = QuickAnswer.answer_students[i].Col;
                    quickAnswerStudent.Caption = waitAnswer;
                    quickAnswerStudent.Name = QuickAnswer.answer_students[i].Name + quickAnswerStudent.Id;
                    quickAnswerStudent.CaptionColor = new SolidColorBrush(Color.FromArgb(255, 34, 139, 34)); //getOptionItemBrush(quickAnswerStudent.Caption);
                    quickAnswerStudent.NameColor = quickAnswerStudent.CaptionColor;
                    quickAnswerStudent.BackColor = new SolidColorBrush(Color.FromArgb(255, 205, 133, 63));
                    quickAnswerStudent.BorderColor = new SolidColorBrush(Color.FromArgb(255, 255, 20, 147));
                    quickAnswerStudent.StartTime = 0;// GetTimeStamp(DateTime.Now, true);
                    quickAnswerStudent.EndTime = 0; //getUseTime();
                    quickAnswerStudent.IsChecked = false;
                    quickAnswerStudent.SubjectiveQuestionAnswer = string.Empty;
                    quickAnswerStudent.Visibled = Visibility.Visible;
                    quickAnswerStudents.Add(quickAnswerStudent);
                    QuickAnswerStudentControl quickAnswerStudentControl = new QuickAnswerStudentControl();
                    quickAnswerStudentControl.DataContext = quickAnswerStudent;
                    quickAnswerStudentControls.Add(quickAnswerStudentControl);
                }
            }
            #endregion
        }

        /// <summary>
        /// 初始化坐位
        /// </summary>
        public void initSeat()
        {
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
                }
            }
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
            seats.Width = (maxCol - minCol + 1) * (deskWidth + deskHMargin) + deskHMargin;
            seats.Height = (maxRow - minRow + 1) * (deskHeight + deskVMargin) + deskVMargin;
            //seats.Background = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0)); ;
            //MessageBox.Show($"Col[{minCol}--{maxCol}]-->Row[{minRow}--{maxRow}]");

            if (null == quickAnswerStudentControls || 0 == quickAnswerStudentControls.Count)
            {
                return;
            }
            seats.Children.Clear();
            foreach (QuickAnswerStudentControl seat in quickAnswerStudentControls)
            {
                seats.Children.Add(seat);
                Grid.SetRow(seat, (seat.DataContext as QuickAnswerStudent).Row - 1);
                Grid.SetColumn(seat, (seat.DataContext as QuickAnswerStudent).Col - 1);
            }
        }

        /// <summary>
        /// 序号转字母
        /// 从0开始
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string integerToLetter(int value)
        {
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            int asciiCode = 65 + value;
            byte[] byteArray = new byte[] { (byte)asciiCode };
            string strCharacter = asciiEncoding.GetString(byteArray);
            return strCharacter;
        }

        /// <summary>
        /// 随机颜色
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private Color getRandColor(int start = 0, int end = 256)
        {
            // 创建一个Random对象
            Random random = new Random();

            // 生成一个0到255之间的随机整数作为红色分量
            byte red = Convert.ToByte(random.Next(start, end));
            //int red = random.Next(start, end);

            // 生成一个0到255之间的随机整数作为绿色分量
            byte green = Convert.ToByte(random.Next(start, end));
            //int green = random.Next(start, end);

            // 生成一个0到255之间的随机整数作为蓝色分量
            byte blue = Convert.ToByte(random.Next(start, end));
            //int blue = random.Next(start, end);

            // 将RGB值转换为ARGB格式的颜色值
            Color color = Color.FromArgb(255, red, green, blue);

            return color;
        }

        /// <summary>
        /// 随机画刷
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        private Brush getRandBrush(int start = 0, int end = 256)
        {
            return new SolidColorBrush(getRandColor(start, end));
        }

        private string getOptionItem(int index)
        {
            return optionItems[index % optionItems.Count].Caption;
            /*
            int start = 0;
            int end = optionItems.Count - 1;
            Random random = new Random();
            int index = random.Next(start, end);
            OptionItem optionItem = optionItems[index];
            return optionItem.Caption;
            */
        }

        /// <summary>
        /// 获取选项画刷
        /// </summary>
        /// <param name="caption"></param>
        /// <returns></returns>
        private Brush getOptionItemBrush(string caption)
        {
            SolidColorBrush result = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            foreach (var item in optionItems)
            {
                if (item.Caption.Trim().ToLower().Equals(caption.Trim().ToLower()))
                {
                    return item.BackColor;
                }
            }
            return result;
        }

        public void initData()
        {
            this.DataContext = Data;
            if (0 == QuickAnswer.answer_type)
            {
                Data.SelectSubject = Visibility.Visible;
                Data.SubjectiveSubject = Visibility.Collapsed;
            }
            if (1 == QuickAnswer.answer_type)
            {
                Data.SelectSubject = Visibility.Collapsed;
                Data.SubjectiveSubject = Visibility.Visible;
            }
            Data.Caption = "正在答题";
            Data.Title = QuickAnswer.title;
            Data.SubjectiveQuestionAnswer = "等待学生回答主观题";
            this.firstAnswer = new CommitQuickAnswer();
        }

        /// <summary>
        /// 取指定时间的时间戳
        /// </summary>
        /// <param name="accurateToMilliseconds">是否精确到毫秒</param>
        /// <returns>返回long类型时间戳</returns>
        public long GetTimeStamp(DateTime dateTime, bool accurateToMilliseconds = false)
        {
            if (accurateToMilliseconds)
            {
                return new DateTimeOffset(dateTime).ToUnixTimeMilliseconds();
            }
            else
            {
                return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
            }
        }

        /// <summary>
        /// 指定时间戳转为时间。
        /// </summary>
        /// <param name="timeStamp">需要被反转的时间戳</param>
        /// <param name="accurateToMilliseconds">是否精确到毫秒</param>
        /// <returns>返回时间戳对应的DateTime</returns>
        public DateTime GetTime(long timeStamp, bool accurateToMilliseconds = false)
        {
            if (accurateToMilliseconds)
            {
                return DateTimeOffset.FromUnixTimeMilliseconds(timeStamp).LocalDateTime;
            }
            else
            {
                return DateTimeOffset.FromUnixTimeSeconds(timeStamp).LocalDateTime;
            }
        }

        private long getUseTime()
        {
            Random random = new Random();
            int result = random.Next(1, 10);
            return result * 10000;
        }

        /// <summary>
        /// 闪烁
        /// </summary>
        /// <param name="id"></param>
        private void studentFlicker(long id)
        {
            #region 数据修改
            /*
            if (null == quickAnswerStudents || 0 == quickAnswerStudents.Count)
            {
                MessageBox.Show($"没有初始化学生坐位", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int index = quickAnswerStudents.ToList().FindIndex(item => id == item.Id);
            if (-1 == index)
            {
                MessageBox.Show($"没有找到学生：{id}", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //quickAnswerStudents[index].BackColor = new SolidColorBrush(Color.FromArgb(255, 155, 0, 0));
            quickAnswerStudents[index].Name = "修改";
            quickAnswerStudents[index].StartTime = GetTimeStamp(DateTime.Now);
            */
            #endregion
            if (null == quickAnswerStudentControls || 0 == quickAnswerStudentControls.Count)
            {
                MessageBox.Show($"没有初始化学生坐位", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int index = quickAnswerStudentControls.ToList().FindIndex(item => id == (item.DataContext as QuickAnswerStudent).Id);
            if (-1 == index)
            {
                MessageBox.Show($"没有找到学生：{id}", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Border cn = quickAnswerStudentControls[index].FindName("_Container") as Border;
            if (null == cn)
            {
                MessageBox.Show($"没有找到学生坐位", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //cn.Background = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = 1.0;
            doubleAnimation.To = 0;
            doubleAnimation.Duration = TimeSpan.FromMilliseconds(50);
            doubleAnimation.AutoReverse = true;
            doubleAnimation.RepeatBehavior = new RepeatBehavior(10);//RepeatBehavior.Forever;
            Storyboard storyboard = new Storyboard();
            Storyboard.SetTarget(doubleAnimation, cn);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(Button.OpacityProperty));
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();
        }

        /// <summary>
        /// 处理消息
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
                    _message = $"只能有学生提交";
                    Console.Write(_message);
                    return;
                }
                switch (msg.type)
                {
                    case MessageType.CommitQuickQuestionAnswer:
                        CommitQuickAnswer cqa = JsonConvert.DeserializeObject<CommitQuickAnswer>(msg.data.ToString());
                        handleCommitResult(cqa);
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
        /// 处理提交结果
        /// </summary>
        /// <param name="cqa"></param>
        private void handleCommitResult(CommitQuickAnswer cqa)
        {
            string _message = string.Empty;
            if (null == cqa)
            {
                _message = $"学生提交快速问题答案数据无效";
                Console.WriteLine(_message);
                return;
            }
            if (cqa.id <= 0)
            {
                _message = $"学生提交快速问题答案数据错误-->需要提交题目ID";
                Console.WriteLine(_message);
                return;
            }
            if (cqa.student <= 0)
            {
                _message = $"学生提交快速问题答案数据错误-->需要提交学生ID";
                Console.WriteLine(_message);
                return;
            }
            if (string.IsNullOrEmpty(cqa.answer))
            {
                _message = $"学生提交快速问题答案数据错误-->需要提交题目答案";
                Console.WriteLine(_message);
                return;
            }
            int _student = quickAnswerStudents.ToList().FindIndex(item => item.Id == cqa.student);
            if (_student < 0)
            {
                _message = $"学生提交快速问题答案数据错误-->无效学生ID：{cqa.student}";
                Console.WriteLine(_message);
                return;
            }

            //选择题
            if (cqa.type == 0)
            {
                handleSelectSubject(cqa, _student);
            }

            //主观题
            if (cqa.type == 1)
            {

            }
        }

        /// <summary>
        /// 处理选择题
        /// </summary>
        /// <param name="cqa">学生端提交答案数据</param>
        /// <param name="_student">提交学生在学生列表中的索引</param>
        private void handleSelectSubject(CommitQuickAnswer cqa, int _student)
        {
            string _message = string.Empty;
            int _answer = optionItems.ToList().FindIndex(item => item.Caption.Trim().ToLower().Equals(cqa.answer.Trim().ToLower()));
            if (_answer < 0)
            {
                _message = $"没有找到问题答案：{cqa.answer.ToUpper()}";
                Console.WriteLine(_message);
                return;
            }

            #region 选择部分或者全部学生
            //MessageBox.Show("" + )
            if (2 == cqa.style)
            {
                //判断是否已经提交
                if (alreadyCommitQuickSelectSubject(cqa.student))
                {
                    _message = $"学生提交快速问题答案数据错误-->学生ID：{cqa.student}，已经提交";
                    Console.WriteLine(_message);
                    return;
                }
                //标注提交学生
                optionItems[_answer].Students.Add(quickAnswerStudents[_student]);
            }
            #endregion

            #region 随机抽人
            if (1 == cqa.style)
            {
                //次版本没有需要处理的逻辑
            }
            #endregion

            #region  处理抢答
            if (0 == cqa.style)
            {
                //仅发送给抢答的学生
                StudentInfo si = getSendStudentInfo(cqa.student);
                if (null == si)
                {
                    _message = $"没有找到抢答学生-->学生ID：{cqa.student}";
                    Console.WriteLine(_message);
                    return;
                }
                //判断是否已经完成抢答
                if (null != this.firstAnswer && this.firstAnswer.id > 0 && this.firstAnswer.student > 0)
                {
                    MQCenter.Instance.Send(si, MessageType.FirstAnswerAlreadyComplete, cqa);
                    return;
                }
                this.firstAnswer = cqa;
                //转发已经有同学抢答成功消息
                MQCenter.Instance.SendToAll(cqa.student, MessageType.FirstAnswerComplete, cqa);
                //发送抢答成功消息
                //MQCenter.Instance.Send(si, MessageType.FirstAnswerSuccess, cqa);
            }
            #endregion

            #region 处理统计
            optionItems[_answer].Qty++;
            optionItems[_answer].Width = optionItems[_answer].Qty * multiple;
            quickAnswerStudents[_student].Caption = cqa.answer.Trim().ToUpper();
            quickAnswerStudents[_student].StartTime = cqa.start;
            quickAnswerStudents[_student].EndTime = cqa.elapsed;
            quickAnswerStudents[_student].BackColor = optionItems[_answer].BackColor;
            #endregion

            #region 播放语音
            this.Dispatcher.Invoke(new Action(() =>
            {
                int _index = quickAnswerStudents.ToList().FindIndex(item => item.Id == cqa.student);
                if (-1 == _index)
                {
                    return;
                }
                if (string.IsNullOrEmpty(quickAnswerStudents[_index].Name))
                {
                    return;
                }
                string text = $"  {quickAnswerStudents[_index].Name}，已经提交答案";
                playText(text);
            }));
            #endregion

            #region 播放动画
            this.Dispatcher.Invoke(new Action(() =>
            {
                studentFlicker(cqa.student);
            }));
            #endregion
        }

        /// <summary>
        /// 获取学生信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private StudentInfo getSendStudentInfo(long id)
        {
            List<CameraItemInfo> CameraItemInfos = Common.CameraList.Clone();
            foreach (var g in CameraItemInfos)
            {
                foreach (var s in g.StudentList)
                {
                    if (s.Id == id)
                    {
                        return s;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 判断学生是否已经提交选择题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool alreadyCommitQuickSelectSubject(long id)
        {
            foreach (OptionItem oi in optionItems)
            {
                if (null != oi.Students && oi.Students.Count > 0)
                {
                    foreach (QuickAnswerStudent qa in oi.Students)
                    {
                        if (qa.Id == id)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

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
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.QuickAnswer.status = "1";
            this.DialogResult = true;
        }

        /// <summary>
        /// 进入公布答案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.QuickAnswer.status = "1";
            //this.DialogResult = true;
            Data.Caption = "公布答案";
            // closeBtn.Visibility
            //studentFlicker(2854);
            playText("  张三已经提交答案");
        }
    }
}

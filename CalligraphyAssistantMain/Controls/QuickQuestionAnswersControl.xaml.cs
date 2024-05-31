using CalligraphyAssistantMain.Code;
using CommonServiceLocator;
using Newtonsoft.Json;
using Prism.Events;
using Qiniu.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Media;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// QuickQuestionAnswersControl.xaml 的交互逻辑
    /// </summary>
    public partial class QuickQuestionAnswersControl : UserControl
    {
        /// <summary>
        /// 控件绑定数据
        /// </summary>
        public QuickQuestionAnswersData Data { get; set; } = new QuickQuestionAnswersData();

        /// <summary>
        /// 快速问题数据
        /// </summary>
        public QuickAnswerInfo QuickAnswer { get; set; } = new QuickAnswerInfo();

        /// <summary>
        /// 学生列表
        /// </summary>
        public ObservableCollection<QuickAnswerStudent> quickAnswerStudents { get; set; } = new ObservableCollection<QuickAnswerStudent>();

        #region 选择题
        /// <summary>
        /// 选择题选项
        /// </summary>
        public ObservableCollection<OptionItem> optionItems { get; set; } = new ObservableCollection<OptionItem>();

        /// <summary>
        /// 学生坐位控件列表
        /// </summary>
        public ObservableCollection<QuickAnswerStudentControl> quickAnswerStudentControls { get; set; } = new ObservableCollection<QuickAnswerStudentControl>();

        /// <summary>
        /// 答案高度
        /// </summary>
        public int optionItemHeight = 20;

        /// <summary>
        /// 答案条目边距
        /// 顶边距:5
        /// 底边距:5
        /// </summary>
        public int optionItemMargin = 10;

        /// <summary>
        /// 柱状图宽度
        /// 数量倍数
        /// </summary>
        private int multiple = 20;

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
        #endregion

        #region 主观题
        /// <summary>
        /// 学生控件
        /// </summary>
        public ObservableCollection<QuickAnswerSubjectiveControl> quickAnswerSubjectiveControls { get; set; } = new ObservableCollection<QuickAnswerSubjectiveControl>();

        /// <summary>
        /// 语音播放器
        /// </summary>
        private SoundPlayer soundPlayer = null;
        #endregion

        #region 显示时初始化数据
        /// <summary>
        /// 是否可以公布答案
        /// </summary>
        private bool submitAnswer = false;

        /// <summary>
        /// 是否可以点评答案
        /// </summary>
        private bool commentAnswer = false;

        /// <summary>
        /// 抢答结果
        /// </summary>
        private CommitQuickAnswer firstAnswer = null;

        /// <summary>
        /// 等待提交的答案
        /// </summary>
        private string waitAnswer = "?";

        /// <summary>
        /// 教时答案
        /// </summary>
        private string teacherAnswer = "";

        /// <summary>
        /// 教室学生分组
        /// </summary>
        private List<CameraItemInfo> studentGroups = new List<CameraItemInfo>();
        #endregion

        #region 消息事件
        private IEventAggregator eventAggregator;
        #endregion

        public QuickQuestionAnswersControl()
        {
            InitializeComponent();
            EventNotify.QuickQuestionAnswersOpenClick += EventNotify_QuickQuestionAnswersOpenClick;
            this.IsVisibleChanged += QuickQuestionAnswersControl_IsVisibleChanged;
        }

        /// <summary>
        /// 显示答题界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void EventNotify_QuickQuestionAnswersOpenClick(object sender, QuickAnswerInfo e)
        {
            QuickAnswer = e;
            this.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 答题界面显示时初始化数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuickQuestionAnswersControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                initVisibility();
                initData();
                initStudent();
                initSubject();
                initCountdown();
                initEvaluate();
                initMessage();
            }
        }

        /// <summary>
        /// 初始化显示控制属性
        /// </summary>
        private void initVisibility()
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
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void initData()
        {
            submitAnswer = false;
            commentAnswer = false;
            soundPlayer = null;
            teacherAnswer = "";
            Data.Caption = "正在答题";
            Data.Title = QuickAnswer.title;
            Data.SubjectiveQuestionAnswer = "等待学生回答主观题";
            Data.AudioUrl = "https://video.nnyun.net/wav/OSR_cn_000_0075_8k.wav";
            firstAnswer = new CommitQuickAnswer();
        }

        /// <summary>
        /// 初始化学生
        /// </summary>
        private void initStudent()
        {
            quickAnswerStudents = new ObservableCollection<QuickAnswerStudent>();
            for (int i = 0; i < QuickAnswer.answer_students.Count; i++)
            {
                QuickAnswerStudent quickAnswerStudent = new QuickAnswerStudent();
                quickAnswerStudent.Id = QuickAnswer.answer_students[i].Id;
                quickAnswerStudent.Row = QuickAnswer.answer_students[i].Row;
                quickAnswerStudent.Col = QuickAnswer.answer_students[i].Col;
                quickAnswerStudent.Caption = waitAnswer;
                quickAnswerStudent.Name = QuickAnswer.answer_students[i].Name + quickAnswerStudent.Id;
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
        /// 初始化题目
        /// </summary>
        private void initSubject()
        {
            //初始化选择题
            if (0 == QuickAnswer.answer_type)
            {
                initSelectSubjectOptions();
                initSelectSubjectSeat();
            }
            if (1 == QuickAnswer.answer_type)
            {
                initSubjectiveSubjectSeat();
            }
        }

        /// <summary>
        /// 初始化倒计时
        /// </summary>
        public void initCountdown()
        {
            EventNotify.CountdownTrigger += EventNotify_CountdownTrigger;
            EventNotify.CountdownStopTrigger += EventNotify_CountdownStopTrigger;
            EventNotify.OnCountdownTrigger(QuickAnswer.id, () =>
            {
                countdownComplete(QuickAnswer);
            });
        }

        /// <summary>
        /// 初始化评议控件
        /// </summary>
        public void initEvaluate()
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                myevaluate.Visibility = Visibility.Collapsed;
            }));
        }

        /// <summary>
        /// 初始化消息
        /// </summary>
        public void initMessage()
        {
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<MQMessageEvent>().Subscribe(Messaging);
        }

        /// <summary>
        /// 初始化选择题答案列表
        /// </summary>
        private void initSelectSubjectOptions()
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
            Data.SelectAnswerBarHeight = QuickAnswer.question * (optionItemHeight + optionItemMargin) + optionItemMargin;
            qis.ItemsSource = optionItems;
            ois.ItemsSource = optionItems;
        }

        /// <summary>
        /// 初始化选择题坐位
        /// </summary>
        private void initSelectSubjectSeat()
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
                seat.MouseLeftButtonDown += Seat_MouseLeftButtonDown;
            }
        }

        /// <summary>
        /// 初始化主观题学生坐位
        /// </summary>
        private void initSubjectiveSubjectSeat()
        {
            quickAnswerSubjectiveControls = new ObservableCollection<QuickAnswerSubjectiveControl>();
            for (int i = 0; i < quickAnswerStudents.Count; i++)
            {
                QuickAnswerSubjectiveControl quickAnswerSubjectiveControl = new QuickAnswerSubjectiveControl();
                quickAnswerSubjectiveControl.DataContext = quickAnswerStudents[i];
                quickAnswerSubjectiveControls.Add(quickAnswerSubjectiveControl);
            }
            #region 学生坐位表行列
            studentGroups = Common.CameraList.Clone();
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
            seatus.Rows = maxRow;
            seatus.Columns = maxCol;
            seatus.Children.Clear();
            foreach (QuickAnswerSubjectiveControl seatu in quickAnswerSubjectiveControls)
            {
                seatu.Cursor = Cursors.Hand;
                seatus.Children.Add(seatu);
                seatu.MouseLeftButtonDown += Seatu_MouseLeftButtonDown;
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

        /// <summary>
        /// 启动倒计时
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void EventNotify_CountdownTrigger(int id, Action act)
        {
            int tm = Common.qTime;
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
            QuickAnswerInfo info = e as QuickAnswerInfo;
            Console.WriteLine("结束：" + info.ToJson());
            info.status = "2";
            var data = new
            {
                info.id,
                answer_type = info.answer_type,
                type = info.category,
                info.title,
                info.question,
                info.content
            };
            Console.WriteLine("消息：" + data.ToJson());
            MQCenter.Instance.SendToAll(MessageType.StopQuickAnswer, data);
            submitAnswer = true;
            Console.WriteLine("答题时间到-->submitAnswer=" + submitAnswer);
        }

        /// <summary>
        /// 点评选择题
        /// </summary>
        /// <param name="quickAnswerStudentControl"></param>
        private void commentSelectSubject(QuickAnswerStudentControl quickAnswerStudentControl = null)
        {
            //多选
            if (2 == QuickAnswer.category)
            {
                commentMultipleSelect(quickAnswerStudentControl);
            }

            //随机抽人
            if (1 == QuickAnswer.category)
            {
                commentRandomSelect(quickAnswerStudentControl);
            }

            //抢答
            if (0 == QuickAnswer.category)
            {
                commentVieSelect(quickAnswerStudentControl);
            }
        }

        /// <summary>
        /// 点评主观题
        /// </summary>
        /// <param name="quickAnswerStudentControl"></param>
        private void commentSubjectiveSubject(QuickAnswerSubjectiveControl quickAnswerSubjectiveControl = null)
        {
            //多选
            if (2 == QuickAnswer.category)
            {
                commentMultipleSubjective(quickAnswerSubjectiveControl);
            }

            //随机抽人
            if (1 == QuickAnswer.category)
            {
                commentRandomSubjective(quickAnswerSubjectiveControl);
            }

            //抢答
            if (0 == QuickAnswer.category)
            {
                commentVieSubjective(quickAnswerSubjectiveControl);
            }
        }

        /// <summary>
        /// 多选.选择题
        /// </summary>
        private void commentMultipleSelect(QuickAnswerStudentControl quickAnswerStudentControl = null)
        {
            quickAnswerStudentControl = (null == quickAnswerStudentControl) ? getAnswerStudentControl() : quickAnswerStudentControl;
            executionCommentSelect(quickAnswerStudentControl);
        }

        /// <summary>
        /// 多选.主观题
        /// </summary>
        /// <param name="quickAnswerStudentControl"></param>
        private void commentMultipleSubjective(QuickAnswerSubjectiveControl quickAnswerSubjectiveControl = null)
        {
            quickAnswerSubjectiveControl = (null == quickAnswerSubjectiveControl) ? getAnswerSubjectiveControl() : quickAnswerSubjectiveControl;
            executionCommentSubjective(quickAnswerSubjectiveControl);
        }

        /// <summary>
        /// 随机抽人.选择题
        /// </summary>
        private void commentRandomSelect(QuickAnswerStudentControl quickAnswerStudentControl = null)
        {
            quickAnswerStudentControl = (null == quickAnswerStudentControl) ? getAnswerStudentControl() : quickAnswerStudentControl;
            executionCommentSelect(quickAnswerStudentControl);
        }

        /// <summary>
        /// 随机抽人.主观题
        /// </summary>
        /// <param name="quickAnswerStudentControl"></param>
        private void commentRandomSubjective(QuickAnswerSubjectiveControl quickAnswerSubjectiveControl = null)
        {
            quickAnswerSubjectiveControl = (null == quickAnswerSubjectiveControl) ? getAnswerSubjectiveControl() : quickAnswerSubjectiveControl;
            executionCommentSubjective(quickAnswerSubjectiveControl);
        }

        /// <summary>
        /// 抢答.选择题
        /// </summary>
        private void commentVieSelect(QuickAnswerStudentControl quickAnswerStudentControl = null)
        {
            quickAnswerStudentControl = (null == quickAnswerStudentControl) ? getAnswerStudentControl() : quickAnswerStudentControl;
            executionCommentSelect(quickAnswerStudentControl);
        }

        /// <summary>
        /// 抢答.主观题
        /// </summary>
        /// <param name="quickAnswerStudentControl"></param>
        private void commentVieSubjective(QuickAnswerSubjectiveControl quickAnswerSubjectiveControl = null)
        {
            quickAnswerSubjectiveControl = (null == quickAnswerSubjectiveControl) ? getAnswerSubjectiveControl() : quickAnswerSubjectiveControl;
            executionCommentSubjective(quickAnswerSubjectiveControl);
        }

        /// <summary>
        /// 执行选择题评议
        /// </summary>
        private void executionCommentSelect(QuickAnswerStudentControl quickAnswerStudentControl)
        {
            myevaluate.quickAnswerSubjectiveControl = null;
            myevaluate.quickAnswerStudentControl = quickAnswerStudentControl;
            this.Dispatcher.Invoke(new Action(() =>
            {
                myevaluate.Visibility = Visibility.Visible;
            }));
        }

        /// <summary>
        /// 执行主观题评议
        /// </summary>
        /// <param name="quickAnswerStudentControl"></param>
        private void executionCommentSubjective(QuickAnswerSubjectiveControl quickAnswerSubjectiveControl)
        {
            myevaluate.quickAnswerStudentControl = null;
            myevaluate.quickAnswerSubjectiveControl = quickAnswerSubjectiveControl;
            this.Dispatcher.Invoke(new Action(() =>
            {
                myevaluate.Visibility = Visibility.Visible;
            }));
        }

        /// <summary>
        /// 点评结果.选择题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myevaluate_CompleteCommentClick(object sender, QuickAnswerStudentControl e)
        {
            QuickAnswerStudent quickAnswerStudent = e.DataContext as QuickAnswerStudent;
            //批量点评
            if (0 == myevaluate.StyleId)
            {
                BatchComment(quickAnswerStudent);

            }

            //点评学生
            if (1 == myevaluate.StyleId)
            {
                StudentComment(quickAnswerStudent);
                MessageBox.Show($"完成对学生：${quickAnswerStudent.Name}的评价", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// 点评结果.主观题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myevaluate_CompleteSubjectiveClick(object sender, QuickAnswerSubjectiveControl e)
        {
            QuickAnswerStudent quickAnswerStudent = e.DataContext as QuickAnswerStudent;
            //批量点评
            if (0 == myevaluate.StyleId)
            {
                BatchSubjective(quickAnswerStudent);

            }

            //点评学生
            if (1 == myevaluate.StyleId)
            {
                SubjectiveComment(quickAnswerStudent);
                MessageBox.Show($"完成对学生：{quickAnswerStudent.Name}的评价", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// 批量点评.选择题
        /// </summary>
        private void BatchComment(QuickAnswerStudent quickAnswerStudent)
        {
            string names = "";
            foreach (var qsc in quickAnswerStudentControls)
            {
                QuickAnswerStudent qs = qsc.DataContext as QuickAnswerStudent;
                if (!string.IsNullOrEmpty(teacherAnswer) && qs.Caption.Trim().ToLower().Equals(teacherAnswer.Trim().ToLower()))
                {
                    names += qs.Name + "，";
                    QuickAnswerStudent ss = new QuickAnswerStudent();
                    ss.Id = qs.Id;
                    ss.Score = quickAnswerStudent.Score;
                    ss.Comment = quickAnswerStudent.Comment;
                    StudentComment(ss);
                }
            }
            if (!string.IsNullOrEmpty(names))
            {
                names = names.TrimEnd('，');
                MessageBox.Show($"完成对学生：${names}的评价", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// 批量点评.主观题
        /// </summary>
        private void BatchSubjective(QuickAnswerStudent quickAnswerStudent)
        {
            string names = "";
            foreach (var qsc in quickAnswerSubjectiveControls)
            {
                QuickAnswerStudent qs = qsc.DataContext as QuickAnswerStudent;

                if (!qs.SubjectiveQuestionAnswer.Equals(waitAnswer))
                {
                    names += qs.Name + "，";
                    QuickAnswerStudent ss = new QuickAnswerStudent();
                    ss.Id = qs.Id;
                    ss.Score = quickAnswerStudent.Score;
                    ss.Comment = quickAnswerStudent.Comment;
                    SubjectiveComment(ss);
                }
            }
            if (!string.IsNullOrEmpty(names))
            {
                names = names.TrimEnd('，');
                MessageBox.Show($"完成对学生：{names}的评价", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// 学生点评.选择题
        /// </summary>
        /// <param name="quickAnswerStudent"></param>
        private void StudentComment(QuickAnswerStudent quickAnswerStudent)
        {
            StudentCommentRemark(quickAnswerStudent);
            StudentCommentScore(quickAnswerStudent);
            StudentCommentStar(quickAnswerStudent);
        }

        /// <summary>
        /// 学生点评.主观题
        /// </summary>
        /// <param name="quickAnswerStudent"></param>
        private void SubjectiveComment(QuickAnswerStudent quickAnswerStudent)
        {
            SubjectiveCommentRemark(quickAnswerStudent);
            SubjectiveCommentScore(quickAnswerStudent);
            SubjectiveCommentStar(quickAnswerStudent);
        }

        /// <summary>
        /// 点评评语.选择题
        /// </summary>
        /// <param name="quickAnswerStudent"></param>
        private void StudentCommentRemark(QuickAnswerStudent quickAnswerStudent)
        {
            Task.Run(() =>
            {
                NameValueCollection dict = new NameValueCollection();
                dict.Add("id", QuickAnswer.id.ToString());
                dict.Add("id_student", quickAnswerStudent.Id.ToString());
                dict.Add("comment", quickAnswerStudent.Comment);
                Console.WriteLine($"评语：{quickAnswerStudent.Comment}");
                NameValueCollection heads = new NameValueCollection();
                heads.Add("token", Common.CurrentUser.Token);
                string url = $"{Common.WebAPI}/lesson/active/quick/student/comment";
                string jsonResult = HttpUtility.UploadValues(url, dict, Encoding.UTF8, Encoding.UTF8, heads);
                Console.WriteLine($"{Common.WebAPI}-->api:{url}--返回值：{jsonResult}");
                if (string.IsNullOrEmpty(jsonResult))
                {
                    Console.WriteLine($"快速问答，返回数据无效，调用参数：{dict.ToJson()}");
                    return;
                }
                ResultInfo<string> result = JsonConvert.DeserializeObject<ResultInfo<string>>(jsonResult);
                if (null == result)
                {
                    Console.WriteLine($"快速问答，返回数据无效，调用参数：{dict.ToJson()}，返回值：{jsonResult}");
                    return;
                }
                if (!result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"快速问答，返回数据错误：{result.Msg}");
                    return;
                }
                Console.WriteLine($"快速问答，评议成功，调用参数：{dict.ToJson()}");
            });
            /*
            var param = new
            {
                id = QuickAnswer.id,
                id_student = quickAnswerStudent.Id,
                comment = quickAnswerStudent.Comment
            };
            //MessageBox.Show(param.ToJson());
            string jsonResult = HttpUtility.UploadValues(Common.UserLogin, dict, Encoding.UTF8, Encoding.UTF8);
            */
        }

        /// <summary>
        /// 点评评语.主观题
        /// </summary>
        /// <param name="quickAnswerStudent"></param>
        private void SubjectiveCommentRemark(QuickAnswerStudent quickAnswerStudent)
        {
            Task.Run(() =>
            {
                NameValueCollection dict = new NameValueCollection();
                dict.Add("id", QuickAnswer.id.ToString());
                dict.Add("id_student", quickAnswerStudent.Id.ToString());
                dict.Add("comment", quickAnswerStudent.Comment);
                Console.WriteLine($"评语：{quickAnswerStudent.Comment}");
                NameValueCollection heads = new NameValueCollection();
                heads.Add("token", Common.CurrentUser.Token);
                string url = $"{Common.WebAPI}/lesson/active/subjective/student/comment";
                string jsonResult = HttpUtility.UploadValues(url, dict, Encoding.UTF8, Encoding.UTF8, heads);
                Console.WriteLine($"{Common.WebAPI}-->api:{url}--返回值：{jsonResult}");
                if (string.IsNullOrEmpty(jsonResult))
                {
                    Console.WriteLine($"主观题，返回数据无效，调用参数：{dict.ToJson()}");
                    return;
                }
                ResultInfo<string> result = JsonConvert.DeserializeObject<ResultInfo<string>>(jsonResult);
                if (null == result)
                {
                    Console.WriteLine($"主观题，返回数据无效，调用参数：{dict.ToJson()}，返回值：{jsonResult}");
                    return;
                }
                if (!result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"主观题，返回数据错误：{result.Msg}");
                    return;
                }
                Console.WriteLine($"主观题，评议成功，调用参数：{dict.ToJson()}");
            });
        }

        /// <summary>
        /// 点评得分.选择题
        /// </summary>
        /// <param name="quickAnswerStudent"></param>
        private void StudentCommentScore(QuickAnswerStudent quickAnswerStudent)
        {
            Task.Run(() =>
            {
                NameValueCollection dict = new NameValueCollection();
                dict.Add("id", QuickAnswer.id.ToString());
                dict.Add("id_student", quickAnswerStudent.Id.ToString());
                dict.Add("score", (quickAnswerStudent.Score * 20).ToString());
                Console.WriteLine($"得分：{quickAnswerStudent.Score * 20}");
                NameValueCollection heads = new NameValueCollection();
                heads.Add("token", Common.CurrentUser.Token);
                string url = $"{Common.WebAPI}/lesson/active/quick/student/score";
                string jsonResult = HttpUtility.UploadValues(url, dict, Encoding.UTF8, Encoding.UTF8, heads);
                if (string.IsNullOrEmpty(jsonResult))
                {
                    Console.WriteLine($"快速问答，返回数据无效，调用参数：{dict.ToJson()}");
                    return;
                }
                ResultInfo<string> result = JsonConvert.DeserializeObject<ResultInfo<string>>(jsonResult);
                if (null == result)
                {
                    Console.WriteLine($"快速问答，返回数据无效，调用参数：{dict.ToJson()}，返回值：{jsonResult}");
                    return;
                }
                if (!result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"快速问答，返回数据错误：{result.Msg}");
                    return;
                }
                Console.WriteLine($"快速问答，打分成功，调用参数：{dict.ToJson()}");
            });
        }

        /// <summary>
        /// 点评得分.主观题
        /// </summary>
        /// <param name="quickAnswerStudent"></param>
        private void SubjectiveCommentScore(QuickAnswerStudent quickAnswerStudent)
        {
            Task.Run(() =>
            {
                NameValueCollection dict = new NameValueCollection();
                dict.Add("id", QuickAnswer.id.ToString());
                dict.Add("id_student", quickAnswerStudent.Id.ToString());
                dict.Add("score", (quickAnswerStudent.Score * 20).ToString());
                Console.WriteLine($"得分：{quickAnswerStudent.Score * 20}");
                NameValueCollection heads = new NameValueCollection();
                heads.Add("token", Common.CurrentUser.Token);
                string url = $"{Common.WebAPI}/lesson/active/subjective/student/score";
                string jsonResult = HttpUtility.UploadValues(url, dict, Encoding.UTF8, Encoding.UTF8, heads);
                if (string.IsNullOrEmpty(jsonResult))
                {
                    Console.WriteLine($"主观题，返回数据无效，调用参数：{dict.ToJson()}");
                    return;
                }
                ResultInfo<string> result = JsonConvert.DeserializeObject<ResultInfo<string>>(jsonResult);
                if (null == result)
                {
                    Console.WriteLine($"主观题，返回数据无效，调用参数：{dict.ToJson()}，返回值：{jsonResult}");
                    return;
                }
                if (!result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"主观题，返回数据错误：{result.Msg}");
                    return;
                }
                Console.WriteLine($"主观题，打分成功，调用参数：{dict.ToJson()}");
            });
        }

        /// <summary>
        /// 点评得分.选择题
        /// </summary>
        /// <param name="quickAnswerStudent"></param>
        private void StudentCommentStar(QuickAnswerStudent quickAnswerStudent)
        {
            Task.Run(() =>
            {
                NameValueCollection dict = new NameValueCollection();
                dict.Add("id", QuickAnswer.id.ToString());
                dict.Add("id_student", quickAnswerStudent.Id.ToString());
                dict.Add("star", quickAnswerStudent.Score.ToString());
                Console.WriteLine($"得星：{quickAnswerStudent.Score}");
                NameValueCollection heads = new NameValueCollection();
                heads.Add("token", Common.CurrentUser.Token);
                string url = $"{Common.WebAPI}/lesson/active/quick/student/star";
                string jsonResult = HttpUtility.UploadValues(url, dict, Encoding.UTF8, Encoding.UTF8, heads);
                if (string.IsNullOrEmpty(jsonResult))
                {
                    Console.WriteLine($"快速问答，返回数据无效，调用参数：{dict.ToJson()}");
                    return;
                }
                ResultInfo<string> result = JsonConvert.DeserializeObject<ResultInfo<string>>(jsonResult);
                if (null == result)
                {
                    Console.WriteLine($"快速问答，返回数据无效，调用参数：{dict.ToJson()}，返回值：{jsonResult}");
                    return;
                }
                if (!result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"快速问答，返回数据错误：{result.Msg}");
                    return;
                }
                Console.WriteLine($"快速问答，得星成功，调用参数：{dict.ToJson()}");
            });
        }

        /// <summary>
        /// 点评得分.主观题
        /// </summary>
        /// <param name="quickAnswerStudent"></param>
        private void SubjectiveCommentStar(QuickAnswerStudent quickAnswerStudent)
        {
            Task.Run(() =>
            {
                NameValueCollection dict = new NameValueCollection();
                dict.Add("id", QuickAnswer.id.ToString());
                dict.Add("id_student", quickAnswerStudent.Id.ToString());
                dict.Add("star", quickAnswerStudent.Score.ToString());
                Console.WriteLine($"得星：{quickAnswerStudent.Score}");
                NameValueCollection heads = new NameValueCollection();
                heads.Add("token", Common.CurrentUser.Token);
                string url = $"{Common.WebAPI}/lesson/active/subjective/student/star";
                string jsonResult = HttpUtility.UploadValues(url, dict, Encoding.UTF8, Encoding.UTF8, heads);
                if (string.IsNullOrEmpty(jsonResult))
                {
                    Console.WriteLine($"主观题，返回数据无效，调用参数：{dict.ToJson()}");
                    return;
                }
                ResultInfo<string> result = JsonConvert.DeserializeObject<ResultInfo<string>>(jsonResult);
                if (null == result)
                {
                    Console.WriteLine($"主观题，返回数据无效，调用参数：{dict.ToJson()}，返回值：{jsonResult}");
                    return;
                }
                if (!result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"主观题，返回数据错误：{result.Msg}");
                    return;
                }
                Console.WriteLine($"主观题，得星成功，调用参数：{dict.ToJson()}");
            });
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
                handleSubjectiveSubject(cqa, _student);
            }
        }

        /// <summary>
        /// 处理选择题
        /// </summary>
        /// <param name="cqa">学生端提交答案数据</param>
        /// <param name="_student">提交学生在学生列表中的索引</param>
        private void handleSelectSubject(CommitQuickAnswer cqa, int _student)
        {
            if (cqa.answer == "1")
            {
                cqa.answer = "A";
            }
            else if (cqa.answer == "2")
            {
                cqa.answer = "B";
            }
            else if (cqa.answer == "3")
            {
                cqa.answer = "C";
            }
            else if (cqa.answer == "4")
            {
                cqa.answer = "D";
            }
            else if (cqa.answer == "5")
            {
                cqa.answer = "E";
            }

            string _message = string.Empty;
            int _answer = optionItems.ToList().FindIndex(item => item.Caption.Trim().ToLower().Equals(cqa.answer.Trim().ToLower()));
            if (_answer < 0)
            {
                _message = $"没有找到问题答案：{cqa.answer.ToUpper()}";
                Console.WriteLine(_message);
                return;
            }

            #region 选择部分或者全部学生
            if (2 == cqa.style)
            {
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
                optionItems[_answer].Students.Add(quickAnswerStudents[_student]);
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
                string text = $",{quickAnswerStudents[_index].Name},已答";
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
        /// 处理主观题
        /// </summary>
        /// <param name="cqa"></param>
        /// <param name="_student"></param>
        private void handleSubjectiveSubject(CommitQuickAnswer cqa, int _student)
        {
            string _message = string.Empty;

            #region 选择部分或者全部学生
            if (2 == cqa.style)
            {
                if (!quickAnswerStudents[_student].SubjectiveQuestionAnswer.Equals(waitAnswer))
                {
                    _message = $"学生提交快速问题答案数据错误-->学生ID：{cqa.student}，已经提交-->{quickAnswerStudents[_student].SubjectiveQuestionAnswer}";
                    Console.WriteLine(_message);
                    return;
                }
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
            quickAnswerStudents[_student].SubjectiveQuestionAnswer = cqa.answer;
            quickAnswerStudents[_student].StartTime = cqa.start;
            quickAnswerStudents[_student].EndTime = cqa.elapsed;
            quickAnswerStudents[_student].SubjectiveAudioUrl = cqa.audio;
            Data.SubjectiveQuestionAnswer = cqa.answer;
            Data.AudioUrl = cqa.audio;
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
                string text = $",{quickAnswerStudents[_index].Name},已答";
                playText(text);
            }));
            #endregion

            #region 播放动画
            this.Dispatcher.Invoke(new Action(() =>
            {
                studentFlicker2(cqa.student);
            }));

            #endregion
        }

        /// <summary>
        /// 选择题.判断学生是否提交答案
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
        /// 获取答题学生控件.选择题
        /// </summary>
        /// <returns></returns>
        private QuickAnswerStudentControl getAnswerStudentControl()
        {
            for (int i = 0; i < quickAnswerStudentControls.Count; i++)
            {
                QuickAnswerStudent quickAnswerStudent = quickAnswerStudentControls[i].DataContext as QuickAnswerStudent;
                Console.WriteLine(quickAnswerStudent.Caption.Trim().ToLower() + "::" + teacherAnswer.Trim().ToLower());
                if (quickAnswerStudent.Caption.Trim().ToLower().Equals(teacherAnswer.Trim().ToLower()))
                {
                    return quickAnswerStudentControls[i];
                }
            }
            return quickAnswerStudentControls[0];
        }

        /// <summary>
        /// 获取问题学生控件.主观题
        /// </summary>
        /// <returns></returns>
        private QuickAnswerSubjectiveControl getAnswerSubjectiveControl()
        {
            for (int i = 0; i < quickAnswerSubjectiveControls.Count; i++)
            {
                QuickAnswerStudent quickAnswerStudent = quickAnswerSubjectiveControls[i].DataContext as QuickAnswerStudent;
                Console.WriteLine(quickAnswerStudent.Caption.Trim().ToLower() + "::" + teacherAnswer.Trim().ToLower());
                if (quickAnswerStudent.Caption.Trim().ToLower().Equals(teacherAnswer.Trim().ToLower()))
                {
                    return quickAnswerSubjectiveControls[i];
                }
            }
            return quickAnswerSubjectiveControls[0];
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
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(Button.OpacityProperty));
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();
        }

        /// <summary>
        /// 主观题闪烁
        /// </summary>
        /// <param name="id"></param>
        private void studentFlicker2(long id)
        {
            if (null == quickAnswerSubjectiveControls || 0 == quickAnswerSubjectiveControls.Count)
            {
                MessageBox.Show($"没有初始化学生坐位", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int index = quickAnswerSubjectiveControls.ToList().FindIndex(item => id == (item.DataContext as QuickAnswerStudent).Id);
            if (-1 == index)
            {
                MessageBox.Show($"没有找到学生：{id} 的坐位", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Border cn = quickAnswerSubjectiveControls[index].FindName("_Container") as Border;
            if (null == cn)
            {
                MessageBox.Show($"没有找到学生坐位", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            cn.Background = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = 1.0;
            doubleAnimation.To = 0;
            doubleAnimation.Duration = TimeSpan.FromMilliseconds(50);
            doubleAnimation.AutoReverse = true;
            doubleAnimation.RepeatBehavior = new RepeatBehavior(10);
            Storyboard storyboard = new Storyboard();
            Storyboard.SetTarget(doubleAnimation, cn);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(Button.OpacityProperty));
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();
        }

        /// <summary>
        /// 终止答题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnComplete_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cdc.Stop();
            submitAnswer = true;
            QuickAnswer.status = "2";
            var data = new
            {
                id = QuickAnswer.id,
                answer_type = QuickAnswer.answer_type,
                type = QuickAnswer.category,
                title = QuickAnswer.title,
                question = QuickAnswer.question,
                content = QuickAnswer.content
            };
            Console.WriteLine("终止答题消息：" + data.ToJson());
            MQCenter.Instance.SendToAll(MessageType.StopQuickAnswer, data);
        }

        /// <summary>
        /// 播放学生提交的语音答案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (string.IsNullOrEmpty(Data.AudioUrl))
            {
                Console.WriteLine("没有找到声音文件");
                return;
            }
            if (null != soundPlayer)
            {
                soundPlayer.Stop();
                soundPlayer = null;
            }
            soundPlayer = new SoundPlayer(Data.AudioUrl);
            soundPlayer.Play();
        }

        /// <summary>
        /// 选择后评价.选择题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void Seat_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!commentAnswer)
            {
                Console.WriteLine("教师公布答案后进行点评");
                MessageBox.Show("教师公布答案后进行点评", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            QuickAnswerStudentControl quickAnswerStudentControl = sender as QuickAnswerStudentControl;
            QuickAnswerStudent quickAnswerStudent = quickAnswerStudentControl.DataContext as QuickAnswerStudent;
            if (quickAnswerStudent.Caption.Trim().ToLower().Equals(waitAnswer.Trim().ToLower()))
            {
                Console.WriteLine($"学生：{quickAnswerStudent.Name}，没有答题，不能点评");
                MessageBox.Show($"学生：{quickAnswerStudent.Name}，没有答题，不能点评", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            myevaluate.StyleId = 1;
            myevaluate.Title = (quickAnswerStudentControl.DataContext as QuickAnswerStudent).Name;
            commentSelectSubject(sender as QuickAnswerStudentControl);
        }

        ///
        /// <summary>
        /// 选择后评价.主观题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void Seatu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            QuickAnswerSubjectiveControl quickAnswerSubjectiveControl = sender as QuickAnswerSubjectiveControl;
            myevaluate.StyleId = 1;
            myevaluate.Title = (quickAnswerSubjectiveControl.DataContext as QuickAnswerStudent).Name;
            commentSubjectiveSubject(quickAnswerSubjectiveControl);
        }

        /// <summary>
        /// 关闭答题界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (null != soundPlayer)
            {
                soundPlayer.Stop();
                soundPlayer = null;
            }
            this.Visibility = Visibility.Collapsed;
            EventNotify.OnCountdownStopTrigger(QuickAnswer.id);
            EventNotify.OnQuickQuestionAnswersClose(sender, e);
        }

        /// <summary>
        /// 单击选择题答案
        /// 显示选择该答案的学生坐位（其它学生坐位隐藏）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectSubject_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!submitAnswer)
            {
                Console.WriteLine("正在答题，完成后公布答案");
                MessageBox.Show("正在答题，完成后公布答案", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            OptionItem selected = (sender as Border).Tag as OptionItem;
            teacherAnswer = selected.Caption;
            Console.WriteLine($"正确答案：{teacherAnswer}");
            var data = new
            {
                id = QuickAnswer.id,
                answer_type = QuickAnswer.answer_type,
                type = QuickAnswer.category,
                title = QuickAnswer.title,
                answer = (int)teacherAnswer.ToUpper().Trim().ToCharArray()[0] - 64
            };
            Console.WriteLine($"下发选择题答案消息：{data.ToJson()}");
            MQCenter.Instance.SendToAll(MessageType.PublishQuickAnswer, data);
            if (null == selected.Students || 0 == selected.Students.Count)
            {
                MessageBox.Show($"没有学生答对此题：{QuickAnswer.title}", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                commentAnswer = true;
                return;
            }
            for (int i = 0; i < quickAnswerStudentControls.Count; i++)
            {
                int _index = selected.Students.FindIndex(item => item.Id == (quickAnswerStudentControls[i].DataContext as QuickAnswerStudent).Id);
                if (-1 != _index)
                {
                    Console.WriteLine("答对学生：" + (quickAnswerStudentControls[i].DataContext as QuickAnswerStudent).Name);
                    quickAnswerStudentControls[i].Visibility = Visibility.Visible;
                }
                else
                {
                    quickAnswerStudentControls[i].Visibility = Visibility.Hidden;
                }
            }
            commentAnswer = true;
        }

        /// <summary>
        /// 批量点评
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnComment_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.QuickAnswer.status = "1";
            Data.Caption = "批量点评";

            //选择题
            if (0 == QuickAnswer.answer_type)
            {
                if (!commentAnswer)
                {
                    Console.WriteLine("教师公布答案后进行点评");
                    MessageBox.Show("教师公布答案后进行点评", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                myevaluate.StyleId = 0;
                commentSelectSubject();
            }

            //主观题
            if (1 == QuickAnswer.answer_type)
            {
                myevaluate.StyleId = 0;
                commentSubjectiveSubject();
            }
        }
    }
}

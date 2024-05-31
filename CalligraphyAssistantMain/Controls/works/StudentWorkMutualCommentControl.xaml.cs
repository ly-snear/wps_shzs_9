using CalligraphyAssistantMain.Code;
using CalligraphyAssistantMain.Controls.fivePoint;
using CommonServiceLocator;
using Newtonsoft.Json;
using Prism.Events;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Security.Cryptography;
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

namespace CalligraphyAssistantMain.Controls.works
{
    /// <summary>
    /// StudentWorkMutualCommentControl.xaml 的交互逻辑
    /// </summary>
    public partial class StudentWorkMutualCommentControl : UserControl
    {
        /// <summary>
        /// 参与互评的学生列表
        /// </summary>
        private List<StudentInfo> studentInfos = null;

        /// <summary>
        /// 学生作品
        /// </summary>
        private StudentWorkDetailsInfo studentWorkDetailsInfo = null;

        /// <summary>
        /// 评论列表
        /// </summary>
        private ObservableCollection<StudentWorkCommentItem> studentWorkCommentItems { get; set; } = new ObservableCollection<StudentWorkCommentItem>();

        /// <summary>
        /// 星星参数
        /// </summary>
        public ObservableCollection<FivePointStarModel> fps { get; set; } = new ObservableCollection<FivePointStarModel>();

        #region 星星参数

        /// <summary>
        /// 半径
        /// </summary>
        private readonly double radius = 20;

        /// <summary>
        /// 星星总计数量
        /// </summary>
        private double itemsCount = 5;

        /// <summary>
        /// 选择数量
        /// </summary>
        private double selectCount = 5;

        /// <summary>
        /// 得分 星星数量*20
        /// </summary>
        private decimal score = 1;

        /// <summary>
        /// 评语
        /// </summary>
        private string comment = "";

        /// <summary>
        /// 选择颜色
        /// </summary>
        private Brush selectBackground = new SolidColorBrush(Colors.YellowGreen);

        /// <summary>
        /// 未选中颜色
        /// </summary>
        private Brush unselectBackgroud = new SolidColorBrush(Colors.DarkGray);

        public static DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(double), typeof(StudentWorkMutualCommentControl), new UIPropertyMetadata());
        /// <summary>
        /// 五角星半径
        /// </summary>
        public double Radius
        {
            get
            {
                object result = GetValue(RadiusProperty);
                if (result == null)
                {
                    return radius;
                }
                return (double)result;
            }
            set
            {
                SetValue(RadiusProperty, value);
            }
        }

        public static DependencyProperty ItemsCountProperty = DependencyProperty.Register("ItemsCount", typeof(double), typeof(StudentWorkMutualCommentControl), new UIPropertyMetadata());
        /// <summary>
        /// 五角星个数
        /// </summary>
        public double ItemsCount
        {
            get
            {
                object result = GetValue(ItemsCountProperty);
                if (result == null)
                {
                    return itemsCount;
                }
                return (double)result;
            }
            set
            {
                SetValue(ItemsCountProperty, value);
                InitialData();
                this.InvalidateVisual();
            }
        }

        public static DependencyProperty SelectCountProperty = DependencyProperty.Register("SelectCount", typeof(double), typeof(StudentWorkMutualCommentControl), new UIPropertyMetadata());
        /// <summary>
        /// 选中的五角星个数
        /// </summary>
        public double SelectCount
        {
            get
            {
                object result = GetValue(SelectCountProperty);
                if (result == null)
                {
                    return selectCount;
                }
                return (double)result;
            }
            set
            {
                SetValue(SelectCountProperty, value);
                InitialData();
                this.InvalidateVisual();
            }
        }

        public static DependencyProperty SelectBackgroundProperty = DependencyProperty.Register("SelectBackground", typeof(Brush), typeof(StudentWorkMutualCommentControl), new UIPropertyMetadata());
        /// <summary>
        /// 选中颜色
        /// </summary>
        public Brush SelectBackground
        {
            get
            {
                object result = GetValue(SelectBackgroundProperty);
                if (result == null)
                {
                    return selectBackground;
                }
                return (Brush)result;
            }
            set
            {
                SetValue(SelectBackgroundProperty, value);
            }
        }

        public static DependencyProperty UnSelectBackgroundProperty = DependencyProperty.Register("UnSelectBackground", typeof(Brush), typeof(StudentWorkMutualCommentControl), new UIPropertyMetadata());
        /// <summary>
        /// 未选中颜色
        /// </summary>
        public Brush UnSelectBackground
        {
            get
            {
                object result = GetValue(UnSelectBackgroundProperty);
                if (result == null)
                {
                    return unselectBackgroud;
                }
                return (Brush)result;
            }
            set
            {
                SetValue(UnSelectBackgroundProperty, value);
            }
        }

        public static DependencyProperty ScoreProperty = DependencyProperty.Register("Score", typeof(decimal), typeof(StudentWorkMutualCommentControl), new UIPropertyMetadata());
        /// <summary>
        /// 得分
        /// </summary>
        public decimal Score
        {
            get
            {
                object result = GetValue(ScoreProperty);

                if (result == null)
                {
                    return score;
                }
                return (decimal)result;
            }
            set
            {
                SetValue(ScoreProperty, value);
            }
        }

        public static DependencyProperty CommentProperty = DependencyProperty.Register("Comment", typeof(string), typeof(StudentWorkMutualCommentControl), new UIPropertyMetadata());
        /// <summary>
        /// 评语
        /// </summary>
        public string Comment
        {
            get
            {
                object result = GetValue(CommentProperty);
                if (result == null)
                {
                    return comment;
                }
                return (string)result;
            }
            set
            {
                SetValue(CommentProperty, value);
            }
        }

        #endregion

        #region 消息事件
        private IEventAggregator eventAggregator = null;
        #endregion

        public StudentWorkMutualCommentControl()
        {
            InitializeComponent();
            this.IsVisibleChanged += StudentWorkMutualCommentControl_IsVisibleChanged;
        }

        /// <summary>
        /// 控件可视化变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void StudentWorkMutualCommentControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                if (null == studentWorkDetailsInfo || string.IsNullOrEmpty(studentWorkDetailsInfo.Work))
                {
                    MessageBox.Show("没有找到互评作品", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                    EventNotify.OnStudentWorkMutualCommentClose(sender, null);
                    return;
                }
                if (null == studentInfos || 0 == studentInfos.Count)
                {
                    MessageBox.Show("没有找到参与互评的学生", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                    EventNotify.OnStudentWorkMutualCommentClose(sender, null);
                    return;
                }
                //Console.WriteLine("学生:" + this.studentInfos.ToJson());
                //Console.WriteLine("作品:" + this.studentWorkDetailsInfo.ToJson());
                InitBind();
                SendMutualCommentMessage();
                LoadComment();
                //刘洋 2024-03-18
                //////var studentWorkCommentItems_sort = studentWorkCommentItems_o.OrderByDescending(item => item.id);
                //////var studentWorkCommentItems_top = studentWorkCommentItems_sort.Take(11);
                //////var studentWorkCommentItems_sort2 = studentWorkCommentItems_top.OrderBy(item => item.id);
                //////studentWorkCommentItems = new ObservableCollection<StudentWorkCommentItem>(studentWorkCommentItems_sort2);
                commentList.ItemsSource = studentWorkCommentItems;
                InitStar();
                InitMessage();
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void InitData(List<StudentInfo> _studentInfos, StudentWorkDetailsInfo _studentWorkDetailsInfo)
        {
            studentWorkCommentItems = new ObservableCollection<StudentWorkCommentItem>();
            this.studentInfos = _studentInfos;
            this.studentWorkDetailsInfo = _studentWorkDetailsInfo;
        }

        /// <summary>
        /// 绑定数据
        /// </summary>
        private void InitBind()
        {
            workInfo.DataContext = this.studentWorkDetailsInfo;
            mycomment.DataContext = this;
            this.Comment = "";
        }

        /// <summary>
        /// 下发开始互评开始消息
        /// </summary>
        private void SendMutualCommentMessage()
        {
            //2024-03-03 ly
            //////var data = new
            //////{
            //////    id = studentWorkDetailsInfo.Id,
            //////    url = studentWorkDetailsInfo.Work,
            //////    type = studentWorkDetailsInfo.type,
            //////    student = studentWorkDetailsInfo.id_student
            //////};
            //////foreach (var s in studentInfos)
            //////{
            //////    MQCenter.Instance.Send(s, MessageType.StudentWorkMutualCommentStart, data);
            //////}
            //string images = ".jpg.png.gif.jpeg.bmp";
            string videos = ".mp4.mpg.avi.wmv.mpeg.mov";
            int category = 1;
            string url = studentWorkDetailsInfo.Work.Trim();
            string ext = "." + url.Substring(url.LastIndexOf(@".") + 1);
            if (videos.IndexOf(ext) != -1)
            {
                category = 2;
            }
            string caption = studentWorkDetailsInfo.ClassName + "--" + studentWorkDetailsInfo.name_student;
            var data = new
            {
                id = studentWorkDetailsInfo.Id,
                url,
                studentWorkDetailsInfo.type,
                category,
                caption,
                student = studentWorkDetailsInfo.id_student
            };
            Console.WriteLine("下发开始作品评价消息:" + data.ToJson());
            //return;
            foreach (var s in studentInfos)
            {
                MQCenter.Instance.Send(s, MessageType.StudentWorkMutualCommentStart, data);
            }
        }

        /// <summary>
        /// 加载作品评论
        /// </summary>
        private void LoadComment()
        {
            long id_work = studentWorkDetailsInfo.Id;
            string url = Common.WebAPI + "/lesson/work/comment/list?work=" + id_work;
            Console.WriteLine("url:" + url);
            NameValueCollection header = new NameValueCollection
            {
                { "token", Common.CurrentUser.Token }
            };
            string jsonResult = HttpUtility.DownloadString(url, Encoding.UTF8, header);
            if (string.IsNullOrEmpty(jsonResult))
            {
                MessageBox.Show("读取作品评论错误，服务器返回无效数据", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                EventNotify.OnStudentWorkMutualCommentClose(null, null);
                return;
            }
            ResultInfo<List<StudentWorkCommentItem>> result = JsonConvert.DeserializeObject<ResultInfo<List<StudentWorkCommentItem>>>(jsonResult);
            if (result == null || !result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("读取作品评论错误，" + result.Msg, Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                EventNotify.OnStudentWorkMutualCommentClose(null, null);
                return;
            }
            //Console.WriteLine("原始数据：" + result.Body.ToJson());
            if (null != result.Body && result.Body.Count > 0)
            {
                studentWorkCommentItems = new ObservableCollection<StudentWorkCommentItem>();
                for (int i = 0; i < result.Body.Count; i++)
                {
                    result.Body[i].self = 0;
                    if (2 == result.Body[i].type && Common.CurrentUser.Id == result.Body[i].id_comment)
                    {
                        result.Body[i].self = 1;
                    }
                    studentWorkCommentItems.Add(result.Body[i]);
                }
                //studentWorkCommentItems = new ObservableCollection<StudentWorkCommentItem>(studentWorkCommentItems.OrderBy(item => item.id));
            }
            Console.WriteLine("评论数据：" + studentWorkCommentItems.ToJson());
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitStar()
        {
            this.SelectCount = 2.5;
            InitialData();
            //myfive.ItemsSource = this.fps;
        }

        /// <summary>
        /// 绘制星星
        /// </summary>
        private void InitialData()
        {
            int count = Convert.ToInt32(this.ItemsCount);
            if (count <= 0)
            {
                count = Convert.ToInt32(this.itemsCount);
            }
            fps.Clear();
            for (int i = 0; i < count; i++)
            {
                FivePointStarModel item = new FivePointStarModel
                {
                    ID = i + 1,
                    Radius = radius,
                    SelectBackground = SelectBackground,
                    UnselectBackgroud = UnSelectBackground,
                    Margins = new Thickness(radius, 0, radius, 0)
                };
                //在此设置星形显示的颜色
                if ((i + 1) > SelectCount && ((i + 1 - SelectCount) > 0) && (i + 1 - SelectCount) < 1)
                {
                    item.CurrentValue = 0.5;
                }
                else if ((i + 1) > SelectCount)
                {
                    item.CurrentValue = 0;
                }
                else
                {
                    item.CurrentValue = 1;
                }
                fps.Add(item);
            }
            //MessageBox.Show(fps.ToJson());
            //Console.WriteLine(fps.ToJson());
        }

        /// <summary>
        /// 初始化消息
        /// </summary>
        public void InitMessage()
        {
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<MQMessageEvent>().Subscribe(Messaging);
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
                    case MessageType.StudentWorkMutualCommentRX:
                    case MessageType.StudentWorkMutualCommentRXImage:
                    case MessageType.StudentWorkMutualCommentRXVoice:
                    case MessageType.StudentWorkMutualCommentRXScore:
                        StudentWorkCommentItem swci = JsonConvert.DeserializeObject<StudentWorkCommentItem>(msg.data.ToString());
                        if (null == swci)
                        {
                            _message = $"消息数据无效：{msg.data}";
                            Console.Write(_message);
                            return;
                        }
                        this.Dispatcher.Invoke(() =>
                        {
                            HandleStudentComment(swci);
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
        /// 处理学生提交评价
        /// </summary>
        /// <param name="swci"></param>
        private void HandleStudentComment(StudentWorkCommentItem swci)
        {
            Console.WriteLine("收到：" + swci.ToJson());
            if (swci.id_work != studentWorkDetailsInfo.Id)
            {
                Console.WriteLine($"提交作品：{swci.id_work}--作品：{studentWorkDetailsInfo.Id}");
                return;
            }
            swci.self = 0;
            studentWorkCommentItems.Insert(0, swci);
            //下发消息
            if (null != studentInfos && studentInfos.Count > 0)
            {
                foreach (var studentInfo in studentInfos)
                {
                    if (studentInfo.Id != swci.id_comment)
                    {
                        //2024-03-01 ly 注释
                        //MQCenter.Instance.Send(studentInfo, MessageType.StudentWorkMutualCommentNew, swci);
                        MQCenter.Instance.Send(studentInfo, MessageType.StudentWorkMutualCommentRX, swci);
                    }
                }
            }
        }

        /// <summary>
        /// 关闭互评页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReturnBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (null != studentInfos && studentInfos.Count > 0)
            {
                var data = new { };
                foreach (var studentInfo in studentInfos)
                {
                    MQCenter.Instance.Send(studentInfo, MessageType.StudentWorkMutualCommentEnd, data);
                }
            }
            EventNotify.OnStudentWorkMutualCommentClose(sender, e);
        }

        /// <summary>
        /// 评价打星
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FivePointStar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FivePointStar m = sender as FivePointStar;
            if (null == m || null == m.Tag)
            {
                return;
            }
            string sindex = m.Tag.ToString();
            int index = -1;
            if (!int.TryParse(sindex, out index))
            {
                return;
            }
            this.SelectCount = index;
        }

        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string url = (sender as Label).Tag as string;
                SoundPlayer soundPlayer = new SoundPlayer(url);
                soundPlayer.Play();
                //player.Open(new Uri(url, UriKind.Absolute));
                //player.Play();
                //MediaElement mediaElement = new MediaElement();
                //mediaElement.LoadedBehavior = MediaState.Manual;
                //mediaElement.UnloadedBehavior = MediaState.Stop;
                //Uri uri = new Uri("https://video.nnyun.net/mp3/2022_09_24_20_35_13_7489586_88dff767_4871_44eb_b896_c00616267ced.mp3");
                //mediaElement.Source = uri;
                //mediaElement.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToJson());
            }
        }

        /// <summary>
        /// 发布评论
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (null == this.mycomment.Text || 0 == this.mycomment.Text.Trim().Length)
            {
                MessageBox.Show("输入评语", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string _comment = this.mycomment.Text.Trim();
            var data = new
            {
                id = 0,
                pid = 0,
                title = $"{Common.CurrentUser.Name},老师点评",
                content = _comment,
                star = this.SelectCount,
                score = this.SelectCount * 20,
                grade = (int)this.SelectCount,
                audio = "",
                id_work = studentWorkDetailsInfo.Id
            };
            Console.WriteLine(data.ToJson());
            string url = Common.WebAPI + "/lesson/work/comment/save";
            Console.WriteLine("api:" + url);
            NameValueCollection dict = new NameValueCollection();
            dict.Add("token", Common.CurrentUser.Token);
            string jsonResult = HttpUtility.UploadValuesJson(url, data, Encoding.UTF8, Encoding.UTF8, dict);
            if (null == jsonResult || 0 == jsonResult.Trim().Length)
            {
                MessageBox.Show("调用评价接口失败", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ResultInfo<StudentWorkCommentItem> result = JsonConvert.DeserializeObject<ResultInfo<StudentWorkCommentItem>>(jsonResult);
            if (null == result)
            {
                MessageBox.Show($"评论失败，返回：{jsonResult}", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show($"评论失败，{result.Msg}", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (null == result.Body)
            {
                MessageBox.Show($"评论成功，没有返回数据", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            result.Body.self = 1;
            studentWorkCommentItems.Insert(0, result.Body);
            //下发消息
            if (null != studentInfos && studentInfos.Count > 0)
            {
                foreach (var studentInfo in studentInfos)
                {   //2024-03-01 ly 注释
                    //MQCenter.Instance.Send(studentInfo, MessageType.StudentWorkMutualCommentNew, result.Body);
                    MQCenter.Instance.Send(studentInfo, MessageType.StudentWorkMutualCommentRX, result.Body);
                }

                this.Dispatcher.Invoke(
                    new Action(() => mycomment.Text = ""),
                    System.Windows.Threading.DispatcherPriority.Render);

            }
        }
    }
}

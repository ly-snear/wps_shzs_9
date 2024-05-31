using CalligraphyAssistantMain.Code;
using CalligraphyAssistantMain.Controls.fivePoint;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
    /// CommentAnswerControl.xaml 的交互逻辑
    /// </summary>
    public partial class CommentAnswerControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 评价完成事件.选择题
        /// </summary>
        public event EventHandler<QuickAnswerStudentControl> CompleteCommentClick = null;

        /// <summary>
        /// 评价完成事件.主观题
        /// </summary>
        public event EventHandler<QuickAnswerSubjectiveControl> CompleteSubjectiveClick = null;

        public ObservableCollection<FivePointStarModel> fps = new ObservableCollection<FivePointStarModel>();

        /// <summary>
        /// 学生控件.选择题
        /// </summary>
        public QuickAnswerStudentControl quickAnswerStudentControl { get; set; } = null;

        /// <summary>
        /// 学生控件.主观题
        /// </summary>
        public QuickAnswerSubjectiveControl quickAnswerSubjectiveControl { get; set; } = null;

        /// <summary>
        /// 点评方式
        /// 0:批量点评 1:个人点评
        /// </summary>
        private int _styleId = 0;

        /// <summary>
        /// 点评方式
        /// 0:批量点评 1:个人点评
        /// </summary>
        public int StyleId
        {
            get { return _styleId; }
            set
            {
                _styleId = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("StyleId"));
                }
            }
        }

        /// <summary>
        /// 学生姓名
        /// </summary>
        private string _title = string.Empty;

        /// <summary>
        /// 学生姓名
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Title"));
                }
            }
        }

        public CommentAnswerControl()
        {
            InitializeComponent();
            this.Loaded += CommentAnswerControl_Loaded;
            this.myfive.ItemsSource = fps;
            this.myscore.DataContext = this;
            this.myremark.DataContext = this;
            this.snt.DataContext = this;
            this.snb.DataContext = this;
        }

        private void CommentAnswerControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitialData();
        }

        private double radius = 20;

        private double itemsCount = 5;

        private double selectCount = 5;

        private decimal score = 1;

        private string comment = "";

        private Brush selectBackground = new SolidColorBrush(Colors.YellowGreen);

        private Brush unselectBackgroud = new SolidColorBrush(Colors.DarkGray);

        public static DependencyProperty RadiusProperty = DependencyProperty.Register("Radius", typeof(double), typeof(CommentAnswerControl), new UIPropertyMetadata());
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

        public static DependencyProperty ItemsCountProperty = DependencyProperty.Register("ItemsCount", typeof(double), typeof(CommentAnswerControl), new UIPropertyMetadata());
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

        public static DependencyProperty SelectCountProperty = DependencyProperty.Register("SelectCount", typeof(double), typeof(CommentAnswerControl), new UIPropertyMetadata());
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

        public static DependencyProperty SelectBackgroundProperty = DependencyProperty.Register("SelectBackground", typeof(Brush), typeof(CommentAnswerControl), new UIPropertyMetadata());
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

        public static DependencyProperty UnSelectBackgroundProperty = DependencyProperty.Register("UnSelectBackground", typeof(Brush), typeof(CommentAnswerControl), new UIPropertyMetadata());
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

        public static DependencyProperty ScoreProperty = DependencyProperty.Register("Score", typeof(decimal), typeof(CommentAnswerControl), new UIPropertyMetadata());
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

        public static DependencyProperty CommentProperty = DependencyProperty.Register("Comment", typeof(string), typeof(CommentAnswerControl), new UIPropertyMetadata());
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

        public static RoutedEvent SelectCountChangePropertyEvent = EventManager.RegisterRoutedEvent("SelectCountChangeEvent", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(CommentAnswerControl));

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
                FivePointStarModel item = new FivePointStarModel();
                item.ID = i + 1;
                item.Radius = Radius;
                item.SelectBackground = SelectBackground;
                item.UnselectBackgroud = UnSelectBackground;
                item.Margins = new Thickness(Radius, 0, Radius, 0);
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
            Console.WriteLine(fps.ToJson());
            //MessageBox.Show(fps.ToJson());
        }

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
            this.Score = index;
            RaiseEvent(new RoutedEventArgs(SelectCountChangePropertyEvent, sender));
        }

        #region 只能输入数字
        private void myscore_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // 如果只允许整数，则可以使用 "^[0-9]*$"
            Regex regex = new Regex("[^0-9.]");
            int t = 0;
            if (!int.TryParse(e.Text, out t) && e.Text != ".")
            {
                e.Handled = true;
            }
        }

        private void myscore_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = (sender as TextBox).Text.Trim();
            double _score = 0;
            if (string.IsNullOrEmpty(text))
            {
                _score = 1;
            }
            if (!double.TryParse(text, out _score))
            {
                _score = 1;
            }
            if (_score < 1)
            {
                _score = 1;
            }
            if (_score > 5)
            {
                _score = 5;
            }
            /*
            (sender as TextBox).Text = text;
            */
            (sender as TextBox).Select((sender as TextBox).Text.Length, 0);
            (sender as TextBox).ScrollToEnd();
            this.SelectCount = _score;
        }

        private void myscore_LostFocus(object sender, RoutedEventArgs e)
        {
            string text = (sender as TextBox).Text.Trim();
            double _score = 0;
            if (string.IsNullOrEmpty(text))
            {
                _score = 1;
            }
            if (!double.TryParse(text, out _score))
            {
                _score = 1;
            }
            if (_score < 1)
            {
                _score = 1;
            }
            if (_score > 5)
            {
                _score = 5;
            }
            (sender as TextBox).Text = _score.ToString();
            (sender as TextBox).Select((sender as TextBox).Text.Length, 0);
            (sender as TextBox).ScrollToEnd();
        }
        #endregion

        /// <summary>
        /// 评语完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //选择题
            if (null != quickAnswerStudentControl)
            {
                (quickAnswerStudentControl.DataContext as QuickAnswerStudent).Score = Score;
                (quickAnswerStudentControl.DataContext as QuickAnswerStudent).Comment = myremark.Text;
                CompleteCommentClick?.Invoke(sender, quickAnswerStudentControl);
                return;
            }

            //主观题
            if (null != quickAnswerSubjectiveControl)
            {
                (quickAnswerSubjectiveControl.DataContext as QuickAnswerStudent).Score = Score;
                (quickAnswerSubjectiveControl.DataContext as QuickAnswerStudent).Comment = myremark.Text;
                CompleteSubjectiveClick?.Invoke(sender, quickAnswerSubjectiveControl);
                return;
            }

            Console.WriteLine("没有找到学生坐位控件");
        }
    }
}

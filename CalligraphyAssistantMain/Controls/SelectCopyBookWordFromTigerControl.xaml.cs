using CalligraphyAssistantMain.Code;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
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
    /// SelectCopyBookWordFromTigerControl.xaml 的交互逻辑
    /// </summary>
    public partial class SelectCopyBookWordFromTigerControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ShareCopyBook> shareCopyBooks { get; set; } = new ObservableCollection<ShareCopyBook>();

        #region 分页数据
        private int page;
        private int size;
        private int total;
        /// <summary>
        /// 当前页
        /// </summary>
        public int Page
        {
            get { return this.page; }
            set
            {
                this.page = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Page"));
                }
            }
        }
        /// <summary>
        /// 页码
        /// </summary>
        public int Size
        {
            get { return this.size; }
            set
            {
                this.size = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Size"));
                }
            }
        }
        /// <summary>
        /// 总页码
        /// </summary>
        public int Total
        {
            get { return this.total; }
            set
            {
                this.total = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Total"));
                }
            }
        }
        #endregion

        #region 搜索

        #region 碑帖名称 单字 书家
        private string copybook;
        private string word;
        private string author;

        /// <summary>
        /// 朝代
        /// </summary>
        public ObservableCollection<DynastyInfo> dynasty { get; set; } = new ObservableCollection<DynastyInfo>();

        /// <summary>
        /// 字体
        /// </summary>
        public ObservableCollection<FontInfo> fontInfos { get; set; } = new ObservableCollection<FontInfo>();

        /// <summary>
        /// 书家
        /// </summary>
        public ObservableCollection<AuthorInfoInfo> authorInfos { get; set; } = new ObservableCollection<AuthorInfoInfo>();

        /// <summary>
        /// 碑帖名称
        /// </summary>
        public string Copybook
        {
            get { return copybook; }
            set
            {
                this.copybook = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Copybook"));
                }
            }
        }

        /// <summary>
        /// 单字
        /// </summary>
        public string Word
        {
            get { return word; }
            set
            {
                this.word = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Word"));
                }
            }
        }

        /// <summary>
        /// 书家姓名
        /// </summary>
        public string Author
        {
            get { return author; }
            set
            {
                this.author = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Author"));
                }
            }
        }

        #endregion

        #endregion

        public SelectCopyBookWordFromTigerControl()
        {
            InitializeComponent();
            this.DataContext = this;
            this.listDynasty.DataContext = dynasty;
            this.listFont.DataContext = fontInfos;
            this.listAuthor.DataContext = authorInfos;
            this.IsVisibleChanged += SelectCopyBookWordFromTigerControl_IsVisibleChanged;
            this.listCopybook.DataContext = this;
        }

        /// <summary>
        /// 显示打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectCopyBookWordFromTigerControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                initData();
            }
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public void initData()
        {
            #region 朝代数据
            DynastyInfo di = new DynastyInfo();
            di.Id = 0;
            di.Title = "全部";
            di.IsSelect = true;
            this.dynasty.Clear();
            this.dynasty.Add(di);
            if (null != Common.DynastyList && Common.DynastyList.Length > 0)
            {
                for (int i = 0; i < Common.DynastyList.Length; i++)
                {
                    this.dynasty.Add(Common.DynastyList[i]);
                }
            }
            //MessageBox.Show(this.dynasty.ToJson());
            #endregion

            #region 字体数据
            FontInfo fi = new FontInfo();
            fi.Id = 0;
            fi.Title = "全部";
            fi.IsSelect = true;
            this.fontInfos.Clear();
            this.fontInfos.Add(fi);
            if (null != Common.FontList && Common.FontList.Length > 0)
            {
                for (int i = 0; i < Common.FontList.Length; i++)
                {
                    this.fontInfos.Add(Common.FontList[i]);
                }
            }
            #endregion

            #region 书家数据
            AuthorInfoInfo ai = new AuthorInfoInfo();
            ai.Id = 0;
            ai.Title = "全部";
            ai.IsSelect = true;
            this.authorInfos.Clear();
            this.authorInfos.Add(ai);
            if (null != Common.AuthorList && Common.AuthorList.Length > 0)
            {
                for (int i = 0; i < Common.AuthorList.Length; i++)
                {
                    this.authorInfos.Add(Common.AuthorList[i]);
                }
            }
            #endregion

            this.Page = 1;
            this.Size = 10;
        }

        /// <summary>
        /// 获取单字
        /// </summary>
        private void getCopybook()
        {
            #region 处理朝代
            string _dynasty = string.Empty;
            DynastyInfo _di = (dynasty.ToList()).Find(d => d.IsSelect);
            if (_di != null && !string.IsNullOrEmpty(_di.Title))
            {
                _dynasty = _di.Title;
                if (0 == _di.Id)
                {
                    _dynasty = string.Empty;
                }
            }
            #endregion

            #region 处理字体
            string _font = string.Empty;
            FontInfo _fi = (fontInfos.ToList()).Find(d => d.IsSelect);
            if (_fi != null && !string.IsNullOrEmpty(_fi.Title))
            {
                _font = _fi.Title;
                if (0 == _fi.Id)
                {
                    _font = string.Empty;
                }
            }
            #endregion

            if (string.IsNullOrEmpty(this.Word))
            {
                MessageBox.Show("输入单字", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var param = new
            {
                copybook = Copybook,
                word = Word,
                dynasty = _dynasty,
                font = _font,
                author = Author,
                size = Size,
                page = Page,
            };
            NameValueCollection headerDict = new NameValueCollection();
            headerDict.Add("token", Common.CurrentUser.Token);
            string jsonResult = HttpUtility.UploadValuesJson(Common.CalligraphyWordList, param, Encoding.UTF8, Encoding.UTF8, headerDict);
            //Console.WriteLine(jsonResult);
            //MessageBox.Show(jsonResult);
            //MessageBox.Show(param.ToJson());
            ResultInfo<ResultCalligraphyListInfo<CalligraphyWordDetailsInfo[]>> result = JsonConvert.DeserializeObject<ResultInfo<ResultCalligraphyListInfo<CalligraphyWordDetailsInfo[]>>>(jsonResult);
            if (result == null)
            {
                MessageBox.Show($"返回数据：{jsonResult}，无效", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!result.Ok.ToLower().Trim().Equals("true"))
            {
                MessageBox.Show(result.Msg, Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (result.Body == null)
            {
                MessageBox.Show($"返回数据无效", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (result.Body.Page == null)
            {
                MessageBox.Show($"返回分页数据无效", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.Total = result.Body.Page.Pages;
            if (result.Body.Data == null)
            {
                MessageBox.Show($"返回碑帖数据无效", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.shareCopyBooks.Clear();
            for (int i = 0; i < result.Body.Data.Length; i++)
            {
                ShareCopyBook shareCopyBook = new ShareCopyBook();
                shareCopyBook.Id = Guid.NewGuid().ToString();
                shareCopyBook.Title = $"{result.Body.Data[i].Copybook}({result.Body.Data[i].Author})({result.Body.Data[i].Font})";
                shareCopyBook.Url = result.Body.Data[i].Url;
                shareCopyBook.IsSelect = false;
                this.shareCopyBooks.Add(shareCopyBook);
            }
        }

        /// <summary>
        /// 关闭虎妞单字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void returnBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EventNotify.OnCopyBookTigerRordClose(this, e);
        }

        /// <summary>
        /// 选择虎妞单字
        /// 关闭页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            List<ShareCopyBook> selected = shareCopyBooks.ToList().Where(s => s.IsSelect).ToList();
            if (null == selected || 0 == selected.Count)
            {
                MessageBox.Show("选择虎妞单字字帖", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.Visibility = Visibility.Collapsed;
            for (int i = 0; i < selected.Count; i++)
            {
                selected[i].Type = 2;
                selected[i].IsSelect = false;
            }
            SendCopyBookControl.OnCopyBookWordConfirm(sender, selected);
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Page = 1;
            getCopybook();
        }

        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nextPage_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (0 >= this.Total)
            {
                getCopybook();
                return;
            }
            this.Page++;
            if (this.Page > this.Total)
            {
                this.Page = 1;
            }
            getCopybook();
        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviousPage_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (0 >= this.Total)
            {
                getCopybook();
                return;
            }
            this.Page--;
            if (this.Page <= 0)
            {
                this.Page = this.Total;
            }
            getCopybook();
        }
    }
}

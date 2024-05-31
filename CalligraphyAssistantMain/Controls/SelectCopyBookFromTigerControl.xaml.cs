using CalligraphyAssistantMain.Code;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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
    /// SelectCopyBookFromTigerControl.xaml 的交互逻辑
    /// </summary>
    public partial class SelectCopyBookFromTigerControl : UserControl, INotifyPropertyChanged
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

        #region 搜索条件

        #region 碑帖名称 书家
        private string copybook;
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


        public SelectCopyBookFromTigerControl()
        {
            InitializeComponent();
            this.DataContext = this;
            this.listDynasty.DataContext = dynasty;
            this.listFont.DataContext = fontInfos;
            this.listCopybook.DataContext = this;
            this.IsVisibleChanged += SelectCopyBookFromTigerControl_IsVisibleChanged;
        }

        /// <summary>
        /// 页面显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectCopyBookFromTigerControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
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

            this.Page = 1;
            this.Size = 10;
        }

        /// <summary>
        /// 获取碑帖
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

            var param = new
            {
                title = Copybook,
                dynasty = _dynasty,
                font = _font,
                author = Author,
                size = Size,
                page = Page,
            };
            NameValueCollection headerDict = new NameValueCollection();
            headerDict.Add("token", Common.CurrentUser.Token);
            string jsonResult = HttpUtility.UploadValuesJson(Common.CalligraphyImageList, param, Encoding.UTF8, Encoding.UTF8, headerDict);

            //MessageBox.Show(jsonResult);
            //Console.WriteLine(jsonResult);

            ResultInfo<ResultCalligraphyListInfo<CalligraphyImageDetailsInfo[]>> result = JsonConvert.DeserializeObject<ResultInfo<ResultCalligraphyListInfo<CalligraphyImageDetailsInfo[]>>>(jsonResult);
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
                //没有碑帖地址.忽略
                if (result.Body.Data[i].CoverArr == null || result.Body.Data[i].CoverArr.Length == 0 || result.Body.Data[i].CoverArr[0] == null || string.IsNullOrEmpty(result.Body.Data[i].CoverArr[0].Url))
                {
                    continue;
                }
                ShareCopyBook shareCopyBook = new ShareCopyBook();
                shareCopyBook.Id = Guid.NewGuid().ToString();
                shareCopyBook.Title = $"{result.Body.Data[i].Title}({result.Body.Data[i].Author})({result.Body.Data[i].Font})";
                shareCopyBook.Url = result.Body.Data[i].CoverArr[0].Url;
                shareCopyBook.IsSelect = false;
                this.shareCopyBooks.Add(shareCopyBook);
            }
        }

        /// <summary>
        /// 搜索碑帖
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void searchBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Page = 1;
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
        /// 关闭虎妞碑帖
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void returnBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EventNotify.OnCopyBookTigerClose(this, e);
        }

        /// <summary>
        /// 选择虎妞碑帖
        /// 关闭页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            List<ShareCopyBook> selected = shareCopyBooks.ToList().Where(s => s.IsSelect).ToList();
            if (null == selected || 0 == selected.Count)
            {
                MessageBox.Show("选择虎妞碑帖", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.Visibility = Visibility.Collapsed;
            for (int i = 0; i < selected.Count; i++)
            {
                selected[i].Type = 1;
                selected[i].IsSelect = false;
            }
            //MessageBox.Show(selected.ToJson());
            SendCopyBookControl.OnCopyBookConfirm(sender, selected);
        }
    }
}

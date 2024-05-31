using CalligraphyAssistantMain.Code;
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
    /// <summary>
    /// SelectCopyBookFromPrepareLessonControl.xaml 的交互逻辑
    /// </summary>
    public partial class SelectCopyBookFromPrepareLessonControl : UserControl
    {
        /// <summary>
        /// 数据模型
        /// </summary>
        public ObservableCollection<ShareCopyBook> shareCopyBooks { get; set; } = new ObservableCollection<ShareCopyBook>();
        
        /// <summary>
        /// 图形文件
        /// </summary>
        public string[] shh = new string[] { "png", "jpg", "jpeg", "gif", "bmp", "tiff", "svg", "webp" };

        public SelectCopyBookFromPrepareLessonControl()
        {
            InitializeComponent();
            copybook.DataContext = this;
            this.IsVisibleChanged += SelectCopyBookFromPrepareLessonControl_IsVisibleChanged;
        }

        private void SelectCopyBookFromPrepareLessonControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                shareCopyBooks.Clear();
                BindSandDrawList();
            }
        }

        public void BindSandDrawList()
        {
            if (null == Common.resourceItemInfos || 0 == Common.resourceItemInfos.Count)
            {
                MessageBox.Show("没有找到备课资源", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //MessageBox.Show(Common.resourceItemInfos.ToJson());
            Common.resourceItemInfos.ForEach(r =>
            {
                string[] exts = r.ServerUrl.Split(new char[] { '.' });
                string ext = exts[exts.Length - 1].ToLower();
                if (shh.Contains(ext))
                {
                    ShareCopyBook shareCopyBook = new ShareCopyBook();
                    shareCopyBook.Id = Guid.NewGuid().ToString();
                    shareCopyBook.Title = r.FileName;
                    shareCopyBook.Url = r.ServerUrl;
                    shareCopyBook.IsSelect = false;
                    shareCopyBooks.Add(shareCopyBook);
                }
            });
            //Console.WriteLine(shareCopyBooks.ToJson());
            //MessageBox.Show(shareCopyBooks.ToJson());
        }

        private void returnBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void selectBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            List<ShareCopyBook> selected = shareCopyBooks.ToList().Where(s => s.IsSelect).ToList();
            if(null==selected || 0 == selected.Count)
            {
                MessageBox.Show("选择备课碑帖", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            this.Visibility = Visibility.Collapsed;
            for(int i=0;i< selected.Count;i++)
            {
                selected[i].Type = 1;
                selected[i].IsSelect = false;
            }
            SendCopyBookControl.OnCopyBookPrepareConfirm(sender, selected);
            //SendCopyBookControl
        }
    }
}

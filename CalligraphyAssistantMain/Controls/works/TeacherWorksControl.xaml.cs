using CalligraphyAssistantMain.Code;
using Microsoft.Win32;
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

namespace CalligraphyAssistantMain.Controls.works
{
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// TeacherWorksControl.xaml 的交互逻辑
    /// </summary>
    public partial class TeacherWorksControl : UserControl
    {
        public event Action<StudentWorkDetailsInfo[], int> ImageClick = null;

        public ObservableCollection<StudentWorkDetailsInfo> TeacherWorkCollectionPaging { get; set; }
        public Pager<StudentWorkDetailsInfo> Pager { get; set; }
        public TeacherWorksControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public void InitData()
        {
            Common.GetTeacherWorkInfoList();
            if (Common.TeacherWorkList.Count > 0)
            {
                view.Visibility = Visibility.Visible;
                Pager = new Pager<StudentWorkDetailsInfo>(Common.TeacherWorkList.Count > 8 ? 8 : Common.TeacherWorkList.Count, Common.TeacherWorkList);
                Pager.PagerUpdated += items =>
                {
                    TeacherWorkCollectionPaging = new ObservableCollection<StudentWorkDetailsInfo>(items);
                };
                Pager.CurPageIndex = 1;
            }
            else
            {
                view.Visibility = Visibility.Hidden;
            }

        }

        private void UploadBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\desktop";
            openFileDialog.Filter = "Image1|*.bmp;*.jepg;*.png;*.mp4;*.avi";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == true)
            {
                string path = openFileDialog.FileName;
                if (Common.TeacherSubmitWork(path))
                {
                    InitData();
                    Common.ShowTip("作品已上传!");
                }
                else
                {
                    Common.ShowTip("作品上传失败！");
                }
            }
        }

        private void TeacherWorkItemControl_ImageClick(object sender, EventArgs e)
        {
            if (sender is StudentWorkDetailsInfo info)
            {
                ImageClick?.Invoke(TeacherWorkCollectionPaging.ToArray(), TeacherWorkCollectionPaging.IndexOf(info));
            }
        }

    }
}

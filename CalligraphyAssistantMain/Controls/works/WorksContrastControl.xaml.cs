using CalligraphyAssistantMain.Code;
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
    /// WorksContrastControl.xaml 的交互逻辑
    /// </summary>
    public partial class WorksContrastControl : UserControl
    {
        private List<StudentWorkDetailsInfo>  WorkDetailsInfos {  get; set; }
        public ObservableCollection<StudentWorkDetailsInfo> StudentWorkCollectionPaging { get; set; }
        public Pager<StudentWorkDetailsInfo> Pager { get; set; }
        public int ViewCount {  get; set; }
        public WorksContrastControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public void InitData(List<StudentWorkDetailsInfo> studentWorks)
        {
            WorkDetailsInfos=studentWorks;
            Init(4);
        }
        public void Init(int count=4)
        {
            StudentWorkCollectionPaging = null;
            ViewCount =count;
            if (WorkDetailsInfos.Count > 0)
            {
                view.Visibility = Visibility.Visible;
                Pager = new Pager<StudentWorkDetailsInfo>(WorkDetailsInfos.Count > count ? count : WorkDetailsInfos.Count, WorkDetailsInfos);
                Pager.PagerUpdated += items =>
                {
                    StudentWorkCollectionPaging = new ObservableCollection<StudentWorkDetailsInfo>(items);
                };
                Pager.CurPageIndex = 1;
            }
            else
            {
                view.Visibility = Visibility.Hidden;
            }

        }

        private void nineBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ViewCount!=9)
            {
                Init(9);
            }
        }

        private void fourBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ViewCount != 4)
            {
                Init(4);
            }
        }

        private void ReturnBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility= Visibility.Collapsed;
        }
    }
}

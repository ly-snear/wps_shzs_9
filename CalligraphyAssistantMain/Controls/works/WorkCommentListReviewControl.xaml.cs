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
    /// WorkCommentListReviewControl.xaml 的交互逻辑
    /// </summary>
    public partial class WorkCommentListReviewControl : UserControl
    {
        public WorkCommentListReviewControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public  List<StudentWorkDetailsInfo> WorkDetailsInfos { get; set; }
        private void ReturnBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WorkDetailsInfos.ForEach(p => p.IsSelected = false);
            this.Visibility = Visibility.Collapsed;
        }
        public void InitData(List<StudentWorkDetailsInfo> studentWorks)
        {
            WorkDetailsInfos =  studentWorks;
            WorkDetailsInfos.ForEach(p=>p.IsSelected = true);

        }

        private void btnOK_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            List<StudentWorkDetailsInfo> studentWorks = Common.StudentWorkList.Where(x => x.IsSelected).ToList();
            if (studentWorks.Count > 0)
            {
                EventNotify.OnCheckWorksContrastClick(studentWorks);
            }
        }
    }
}

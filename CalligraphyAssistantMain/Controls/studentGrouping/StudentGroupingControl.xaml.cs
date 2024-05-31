using CalligraphyAssistantMain.Code;
using PropertyChanged;
using System;
using System.Collections.Generic;
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

namespace CalligraphyAssistantMain.Controls.studentGrouping
{
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// StudentGroupingControl.xaml 的交互逻辑
    /// </summary>
    public partial class StudentGroupingControl : UserControl
    {
       public  List<CameraItemInfo> CameraItemInfos { get; set; }
        public StudentGroupingControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public void InitData()
        {
            CameraItemInfos = Common.CameraList;
        }

        private void ReturnBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void groupBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(sender is Border border &&border.Tag is CameraItemInfo cameraItem)
            {
                EventNotify.OnCheckCameraGroup(cameraItem);
            }
        }

        private void studentBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is StudentInfo studentInfo)
            {
                foreach(var cameraItemInfo in CameraItemInfos)
                {
                    if(cameraItemInfo.StudentList.FirstOrDefault(p => p.Id == studentInfo.Id) != null)
                    {
                        EventNotify.OnCheckCamera(cameraItemInfo,studentInfo);
                    }
                  
                }
              
            }
        }
    }

}

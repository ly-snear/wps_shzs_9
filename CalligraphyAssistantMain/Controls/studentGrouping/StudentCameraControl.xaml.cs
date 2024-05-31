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
    /// StudentCameraControl.xaml 的交互逻辑
    /// </summary>
    public partial class StudentCameraControl : UserControl
    {
        public StudentInfo Info { get; set; }
        public CameraItemInfo CameraInfo {  get; set; }
        public StudentCameraControl()
        {
            InitializeComponent();
            this.IsVisibleChanged += StudentCameraControl_IsVisibleChanged;
        }

        private void StudentCameraControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Info != null)
            {
                cameraItemControl.StopRecord();
                cameraItemControl.StopPreview();
                Info = null;
            }
        }

        public void InitData(CameraItemInfo cameraItem ,StudentInfo student)
        {
            Info = student;
            CameraInfo = cameraItem;
            cameraItemControl.InitData(Info, CameraInfo.Name);
            cameraItemControl.FullScreenCallback += FullScreenCallback;
            cameraItemControl.StartPreview();
        }

        private void ReturnBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;

        }

        private void FullScreenCallback(StudentInfo student)
        {
            fullScreenControl.Show(student, CameraInfo.Name);
            fullScreenControl.Visibility = Visibility.Visible;
        }


    }
}

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
    /// CameraContrastControl.xaml 的交互逻辑
    /// </summary>
    public partial class CameraContrastControl : UserControl
    {

        public List<StudentInfo> Students { get; set; }
        private List<RtmpHeper> RtmpControls { get; set; }=new List<RtmpHeper>();
        public CameraContrastControl()
        {
            InitializeComponent();
            this.DataContext = this;
            this.IsVisibleChanged += CameraContrastControl_IsVisibleChanged;
        }

        private void CameraContrastControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Students != null && Students.Count > 0)
            {
                RtmpControls.ForEach(p =>  p.Stop() ) ;
                Students.ForEach (p => p.Image=null ) ;
                RtmpControls.Clear();
                Students.Clear();
            }
        }

        public void InitData(List<StudentInfo> data)
        {
            Students = data;
            Students.ForEach(student =>
            {
                RtmpHeper rtmp = new RtmpHeper(student.Id);
                string previewUrl= string.Format("rtmp://{0}:{1}/live/{2}", Common.PushServer.host, Common.PushServer.port, student.Id);
                rtmp.OnRender += Rtmp_OnRender;
                Task.Run(() => rtmp.Start(previewUrl));
                RtmpControls.Add(rtmp);
            });
        }

        private void Rtmp_OnRender(int width, int height, IntPtr ptr, int count, int id)
        {
           var student= Students.FirstOrDefault(p => p.Id == id);
            if (student!=null)
            {
                this.Dispatcher.Invoke(() =>
                {
                    if (student.Image == null || width != student.Image.Width || height != student.Image.Height)
                    {
                        student.Image = new WriteableBitmap(width, height, 96.0, 96.0, PixelFormats.Bgr24, null);
                        student.PreviewImageSource = new ImageBrush(student.Image);
                    }
                    student.Image.Lock();
                    WindowsNativeMethods.Instance.CopyMemory(student.Image.BackBuffer, ptr, Convert.ToUInt32(count));
                    Int32Rect dirtyRect = new Int32Rect(0, 0, width, height);
                    student.Image.AddDirtyRect(dirtyRect);
                    student.Image.Unlock();
                });
            }
        }

        private void ReturnBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

    }

}

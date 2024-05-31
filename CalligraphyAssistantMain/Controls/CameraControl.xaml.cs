using CalligraphyAssistantMain.Code;
using CommonServiceLocator;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
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
    /// CameraControl.xaml 的交互逻辑
    /// </summary>
    public partial class CameraControl : UserControl
    {
        public event EventHandler TitleChanged;
        public Action<CameraItemInfo> FullScreenCallback;  
        private IEventAggregator eventAggregator;
        private bool RenderImage = true;
        public CameraItemInfo PreviewItem { get; set; }
        public CameraControl()
        {
            InitializeComponent();
            presetWp.Visibility = groupGd.Visibility = controlBarGd.Visibility = Visibility.Collapsed;
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<FullScreenChangeStudentEvent>().Subscribe(FullScreenChangeStudent);
            this.DataContext = null;
        }
        public void Play()
        {
            this.Dispatcher.Invoke(() =>
            {
                playBackGd.Visibility = presetWp.Visibility = groupGd.Visibility = controlBarGd.Visibility = Visibility.Visible;
            }); 
        }

        public void Stop()
        { 
            playBackGd.Visibility = presetWp.Visibility = groupGd.Visibility = controlBarGd.Visibility = Visibility.Collapsed;
        }

        public void Dispose()
        { 

        } 
		
		public void StopRender() {
            this.RenderImage = false;
        }

        public void StartRender()
        {
            this.RenderImage = true;
        }

        public void UpdateImage(int width, int height, byte[] data)
        {
            if (this.RenderImage)
            {
                this.Dispatcher.Invoke(() =>
                {
                    loadingGd.Visibility = Visibility.Collapsed;
                    playBackGd.Visibility = presetWp.Visibility = groupGd.Visibility = controlBarGd.Visibility = Visibility.Visible;
                });
                //imageRenderGrid.UpdateImage(width, height, data);
            }
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PreviewItem = this.DataContext as CameraItemInfo;
            //if (PreviewItem?.StudentList.Count > 0)
            //{
            //    player.InitData(PreviewItem.StudentList[0]);
            //}
        }
         
        private void moreBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            morePop.IsOpen = true;
        }

        private void NumberControl_ItemClick(object sender, EventArgs e)
        {
            StudentInfo studentInfo = (sender as CameraPresetControl).DataContext as StudentInfo;
            CameraItemInfo info = this.DataContext as CameraItemInfo;
            if (studentInfo != null && info != null)
            {
                if (studentInfo.IsSelected)
                {
                    return;
                }
                info.StudentList.ForEach(p => p.IsSelected = false);
                loadingGd.Visibility = Visibility.Visible;
                studentInfo.IsSelected = true;
                info.SelectedStudent = studentInfo.Name;
                info.NotifyPropertyChanged("Current");
                if (TitleChanged != null)
                {
                    TitleChanged(this, null);
                }
                //player.InitData(studentInfo);
            }
        }

        public bool ContainsStudent(string ip)
        {
            return PreviewItem != null && PreviewItem.StudentList.Any(p => p.IP.Equals(ip));
        } 

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
        }

        private void FullScreenChangeStudent(CommonModel model)
        {
            if (ContainsStudent(model.Student.IP))
            {
                loadingGd.Visibility = Visibility.Visible;
            }
        } 
        /// <summary>
        /// 拍照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void takePhotoBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            StudentInfo student = PreviewItem.StudentList.FirstOrDefault(p => p.IsSelected);
            if (student != null)
            {
                Common.SocketServer.TakePhoto(student);
            }
            morePop.IsOpen = false;
        }

        private void fullScreenBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (FullScreenCallback != null)
            {
                FullScreenCallback(this.DataContext as CameraItemInfo);
            }
            morePop.IsOpen = false;
        }

        private void rtmpPlayerControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (PreviewItem.Mode == 1 && e.ClickCount == 2 && FullScreenCallback != null)
            {
                FullScreenCallback(this.DataContext as CameraItemInfo);
            }
        }
        /// <summary>
        /// 分享
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void shareBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectStudentsControl selectStudentsControl = new SelectStudentsControl();
            selectStudentsControl.BindStudentList(Common.CameraList);
            if (selectStudentsControl.ShowDialog() == true)
            {
                List<StudentInfo> students = new List<StudentInfo>();
                selectStudentsControl.CameraItemInfos.ForEach(p => p.StudentList.ForEach(s =>
                {
                    if (s.IsSelected)
                    {
                        students.Add(s);
                    }
                }));
            }
        }
    }
}

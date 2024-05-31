using CalligraphyAssistantMain.Code;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Threading;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Rectangle = System.Drawing.Rectangle;
using UserControl = System.Windows.Controls.UserControl;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using Prism.Events;
using Path = System.IO.Path;
using Newtonsoft.Json;

namespace CalligraphyAssistantMain.Controls.studentGrouping
{
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// CameraItemControl.xaml 的交互逻辑
    /// </summary>
    public partial class CameraItemControl : UserControl
    {
        public Action<StudentInfo> FullScreenCallback;
        private string VideoRecordPath { get; set; }
        private RtmpHeper rtmp;
        Thread thPlayer;
        private System.Diagnostics.Process process = null;
        private string previewUrl;
        private bool IsPhotos { get; set; }
        public StudentInfo StudentCamera
        {
            get; set;
        }
        public string GroupName
        {
            get; set;
        }
        public CameraItemControl()
        {
            InitializeComponent();
            this.DataContext = this;

        }
        public void InitData(StudentInfo info, string gruopName)
        {
            rtmp = new RtmpHeper(info.Id);
            StudentCamera = info;
            StudentCamera.PreviewImageSource = new ImageBrush();
            GroupName = gruopName;
            previewUrl = string.Format("rtmp://{0}:{1}/live/{2}", Common.PushServer.host, Common.PushServer.port, StudentCamera.Id);
         //   previewUrl = "rtmp://liteavapp.qcloud.com/live/liteavdemoplayerstreamid";


        }

        public void StartPreview()
        {
            if (thPlayer == null)
            {
                thPlayer = new Thread(DeCoding)
                {
                    IsBackground = true
                };
                thPlayer.Start();
            }

        }
        public void StopPreview()
        {
            rtmp.Stop();
            StudentCamera.Image = null;
            thPlayer = null;
        }
        public void RecordFile(string fileName)
        {
            Task.Run(() =>
            {
                VideoRecordPath = fileName;
                var current = Environment.CurrentDirectory;
                var probe = @"Plugins\FFmpeg\ffmpeg.exe";
                string ffmpegPath = Path.Combine(current, probe);
                string cmd = string.Format(@"-y -i {0} -vcodec copy -an  -f mp4 {1}", previewUrl, fileName);
                process = new System.Diagnostics.Process();
                process.StartInfo.FileName = ffmpegPath;
                process.StartInfo.Arguments = cmd;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardInput = true;
               
                if (process.Start())
                {
                    //倒计时
                    this.Dispatcher.Invoke(() =>
                    {
                        void action()
                        {
                            StopRecord();
                        }
                        countdownView.Start(second: 3 * 60, action: action);
                        StudentCamera.IsTranscribe = true;
                    });
                }
                else
                {
                    Common.ShowTip("录像失败！");
                    process=null;
                }
            });
           
        }
        public void StopRecord()
        {
            try
            {

                if (StudentCamera.IsTranscribe)
                {
                    Task.Run(() =>
                    {
                        process?.StandardInput.WriteLine("q");
                        process?.WaitForExit();
                        //process?.Kill();
                        process = null;
                        StudentInfo studentInfo = StudentCamera;
                        if (!Common.SubmitVideo(VideoRecordPath, studentInfo))
                        {
                            Common.Debug("教师录像 学生姓名：" + studentInfo.Name + "作品上传失败！");
                            Common.ShowTip("教师录像：" + studentInfo.Name + " 作品上传失败！");
                        }
                        else
                        {
                            Common.ShowTip("教师录像：" + studentInfo.Name + " 作品上传成功！");
                        }
                    });
                    StudentCamera.IsTranscribe = false;
                }


                countdownView.Stop();

            }
            catch
            {

            }
           

        }
        private void saveVideoBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!StudentCamera.IsTranscribe)
            {
                string tempPath = Path.Combine(Common.SettingsPath, "Temp");
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                tempPath = Path.Combine(tempPath, "Student_" + StudentCamera.IP + "_" + DateTime.Now.Ticks + ".mp4");

                RecordFile(tempPath);
            }
            else
            {
                StopRecord();
       
            }

        }
        /// <summary>
        /// 拍照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void takePhotoBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsPhotos)
                photosType.Visibility = photosType.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
            //Common.SocketServer.TakePhoto(StudentCamera);

            morePop.IsOpen = false;
        }

        private void fullScreenBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (FullScreenCallback != null)
            {
                FullScreenCallback(StudentCamera);
            }
            morePop.IsOpen = false;
        }

        /// <summary>
        /// 分享
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void shareBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectStudentsControl selectStudentsControl = new SelectStudentsControl();
            selectStudentsControl.BindStudentList(Common.CameraList.Clone());
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

                var data = new
                {
                    studentid = StudentCamera.Id,
                    url= string.Format("rtmp://{0}:{1}/live/{2}", Common.PushServer.host, Common.PushServer.port, StudentCamera.Id)
            };
                if (students.Count > 0)
                {
                    students.ForEach(s => MQCenter.Instance.Send(s, MessageType.ShareScreen, data));
                }
            }
            morePop.IsOpen = false;
        }

        /// <summary>
        /// 播放线程执行方法
        /// </summary>
        private void DeCoding()
        {
            try
            {
                Console.WriteLine("DeCoding run...");
                rtmp.OnRender += Rtmp_OnRender;
                rtmp.Start(previewUrl);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("DeCoding exit");
                rtmp.Stop();
                StudentCamera.Image = null;
                thPlayer = null;

            }
        }

        private void Rtmp_OnRender(int width, int height, IntPtr ptr, int length,int id)
        {
            if (StudentCamera.Id == id)
            {
                this.Dispatcher.Invoke(new MethodInvoker(() =>
                {
                    if (StudentCamera.Image == null || width != StudentCamera.Image.Width || height != StudentCamera.Image.Height)
                    {
                        StudentCamera.Image = new WriteableBitmap(width, height, 96.0, 96.0, PixelFormats.Bgr24, null);
                        StudentCamera.PreviewImageSource = new ImageBrush(StudentCamera.Image);
                    }
                    StudentCamera.Image.Lock();
                    WindowsNativeMethods.Instance.CopyMemory(StudentCamera.Image.BackBuffer, ptr, Convert.ToUInt32(length));
                    Int32Rect dirtyRect = new Int32Rect(0, 0, width, height);
                    StudentCamera.Image.AddDirtyRect(dirtyRect);
                    StudentCamera.Image.Unlock();
                }),DispatcherPriority.Send);
            }
          
        }

        private void photoBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsPhotos = true;
            photosType.Visibility = Visibility.Collapsed;
            if (StudentCamera.PreviewImageSource.ImageSource == null)
            {
                Common.ShowTip("拍照失败！");
            }

            string tempPath = Path.Combine(Common.SettingsPath, "Temp");
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            tempPath = Path.Combine(tempPath, "Student_" + StudentCamera.IP + "_" + DateTime.Now.Ticks + ".png");
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create((BitmapSource)StudentCamera.PreviewImageSource.ImageSource));
            FileStream file = new FileStream(tempPath, FileMode.Create);
            encoder.Save(file);
            file.Close();
            IsPhotos = false;
            Task.Run(() =>
            {
                StudentInfo studentInfo = StudentCamera;
                if (!Common.SubmitStudentWork(tempPath, studentInfo))
                {
                    Common.Debug("教师拍照 学生姓名：" + studentInfo.Name + "作品上传失败！");
                    Common.ShowTip("教师拍照：" + studentInfo.Name + " 作品上传失败！");
                }
                else
                {
                    Common.ShowTip("教师拍照：" + studentInfo.Name + " 作品上传成功！");
                }
            });


        }

        private void delayedBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            photosType.Visibility = Visibility.Collapsed;
            counterTxt.Visibility = Visibility.Visible;
            Task.Run(() =>
            {
                int count = 5;
                while (count >= 0)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        counterTxt.Text = count.ToString();
                    });
                    count--;
                    Thread.Sleep(1000);
                }
                string tempPath = Path.Combine(Common.SettingsPath, "Temp");
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
       
                tempPath = Path.Combine(tempPath, "Student_" + StudentCamera.IP + "_" + DateTime.Now.Ticks + ".png");
                this.Dispatcher.Invoke(() =>
                {
                    if (StudentCamera.PreviewImageSource.ImageSource == null)
                    {
                        Common.ShowTip("拍照失败！");
                        return;
                    }
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create((BitmapSource)StudentCamera.PreviewImageSource.ImageSource));
                    FileStream file = new FileStream(tempPath, FileMode.Create);
                    encoder.Save(file);
                    file.Close();
                    counterTxt.Visibility = Visibility.Collapsed;
                });

                StudentInfo studentInfo = StudentCamera;
                if (!Common.SubmitStudentWork(tempPath, studentInfo))
                {
                    Common.Debug("教师拍照 学生姓名：" + studentInfo.Name + "作品上传失败！");
                    Common.ShowTip("教师拍照：" + studentInfo.Name + " 作品上传失败！");
                }
                else
                {
                    Common.ShowTip("教师拍照：" + studentInfo.Name + " 作品上传成功！");
                }
                IsPhotos = false;
            });

        }

        private void continuousBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            photosType.Visibility = Visibility.Collapsed;
            counterTxt.Visibility = Visibility.Visible;
            Task.Run(() =>
            {
                int count = 1;
                while (count <= 3)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        string tempPath = Path.Combine(Common.SettingsPath, "Temp");
                        if (!Directory.Exists(tempPath))
                        {
                            Directory.CreateDirectory(tempPath);
                        }
                        if (StudentCamera.PreviewImageSource.ImageSource == null)
                        {
                            Common.ShowTip("拍照失败！");
                            return;
                        }
                        tempPath = Path.Combine(tempPath, "Student_" + StudentCamera.IP + "_" + DateTime.Now.Ticks + ".png");
                        var encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create((BitmapSource)StudentCamera.PreviewImageSource.ImageSource));
                        FileStream file = new FileStream(tempPath, FileMode.Create);
                        encoder.Save(file);
                        file.Close();

                        StudentInfo studentInfo = StudentCamera;
                        if (!Common.SubmitStudentWork(tempPath, studentInfo))
                        {
                            Common.Debug("教师拍照 学生姓名：" + studentInfo.Name + "作品上传失败！");
                            Common.ShowTip("教师拍照：" + studentInfo.Name + " 作品上传失败！");
                        }
                        else
                        {
                            Common.ShowTip("教师拍照：" + studentInfo.Name + " 作品上传成功！");
                        }
                        counterTxt.Text = count.ToString();
                    });
                    count++;
                    Thread.Sleep(2000);
                }
                this.Dispatcher.Invoke(() =>
                {
                    counterTxt.Visibility = Visibility.Collapsed;
                });
                IsPhotos = false;
            });


        }

        private void contrastBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!StudentCamera.IsContrast)
            {
                int count = 0;
                foreach (var item in Common.CameraList)
                {
                    count += item.StudentList.Count(p => p.IsContrast);
                }
                if (count >= 4)
                {
                    Common.ShowTip("已填加4个摄像头对比！");
                }
                else
                {
                    StudentCamera.IsContrast = true;
                    EventNotify.OnCameraContrastEvent();
                }
            }
            else
            {
                StudentCamera.IsContrast = false;
            }
            morePop.IsOpen = false;
        }


        private void moreBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            photosType.Visibility=Visibility.Collapsed;
            morePop.IsOpen = true;
        }
    }
}

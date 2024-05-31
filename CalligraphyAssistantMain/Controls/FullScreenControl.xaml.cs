using CalligraphyAssistantMain.Code;
using CommonServiceLocator;
using Newtonsoft.Json;
using Prism.Events;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
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
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// FullScreenControl.xaml 的交互逻辑
    /// </summary>
    public partial class FullScreenControl : UserControl
    {
        //private CameraItemInfo cameraItem;
        private RtmpHeper rtmpHeper { get; set; }
        public  StudentInfo studentInfo {  get; set; }
        private IEventAggregator eventAggregator;
        private List<CameraItemInfo> cameraList;
        private CameraItemInfo cameraItem;
        public System.Windows.Media.Imaging.WriteableBitmap Image { get; set; }
        public FullScreenControl()
        {
            InitializeComponent();
            this.DataContext = this;
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<VideoFrameEvent>().Subscribe(RenderVideo);
        }

        public void Show(List<CameraItemInfo> cameraList, StudentInfo studentInfo)
        {
            this.Visibility = Visibility.Visible;
            //loadingGd.Visibility = Visibility.Visible;
            this.cameraList = cameraList;
            this.studentInfo = studentInfo;
           // Common.SocketServer.StartCameraLive(studentInfo);
            //rtmpPlayer.RenderImage = true;
            //rtmpPlayer.Play(studentInfo.Owner.GetFeatureUrl());
            _tipLb2.Text = studentInfo.Owner.Name + " - " + studentInfo.Name;
          
        }

        public void Show(StudentInfo studentInfo,string cameraName) {
            this.Visibility = Visibility.Visible;
            this.studentInfo = studentInfo;
            previousBtn.Visibility = Visibility.Visible;
            nextBtn.Visibility = Visibility.Visible;
            _tipLb2.Text = cameraName + " - " + studentInfo.Name;
            studentInfo.FullPreviewImageSource= studentInfo.PreviewImageSource.Clone();
            if (StopRtmp())
            {
                if (!StartRtmp())
                {
                    Common.ShowTip("学生摄像头无法查看！");
                }
            }
        }
        public void Show(CameraItemInfo item , StudentInfo studentInfo)
        {
            cameraItem = item;
            this.Visibility = Visibility.Visible;
            this.studentInfo = studentInfo;
            previousBtn.Visibility = Visibility.Visible;
            nextBtn.Visibility = Visibility.Visible;
            _tipLb2.Text = cameraItem.Name + " - " + studentInfo.Name;
        }
        private void RenderVideo(VideoFrameModel model)
        {
            if (model.Student == studentInfo && writeScreenControl.Visibility != Visibility.Visible)
            {
                if (loadingGd.Visibility == Visibility.Visible)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        loadingGd.Visibility = Visibility.Collapsed;
                    });
                }
                imageRenderGrid.UpdateImage(model.Width, model.Height, model.Buffer);
            }
        }

        private bool UploadImage(int selectedStudent)
        {
            ImageSource imageSource = writeScreenControl.GetImage();
            string tempPath = System.IO.Path.Combine(Common.SettingsPath, "Temp");
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            tempPath = System.IO.Path.Combine(tempPath, "Student_" + studentInfo.IP + "_" + DateTime.Now.Ticks + ".jpg");
            if (!Common.SaveImage(imageSource, tempPath))
            {
                MessageBoxEx.ShowError("点评保存失败！", Window.GetWindow(this));
                return false;
            }
            string imageUrl = Common.UploadImageV2(tempPath, selectedStudent);
            if (string.IsNullOrEmpty(imageUrl))
            {
                MessageBoxEx.ShowError("点评上传失败！", Window.GetWindow(this));
                return false;
            }
            NameValueCollection dict = new NameValueCollection();
            NameValueCollection data = new NameValueCollection();
            dict.Add("token", Common.CurrentUser.Token);
            data.Add("semester", Common.SemesterInfo.Semester.ToString());
            data.Add("product", imageUrl);
            data.Add("selectedStudent", selectedStudent.ToString());
            data.Add("idStudent", studentInfo.Id.ToString());
            data.Add("exStudent", studentInfo.Name);
            data.Add("idClass", Common.CurrentClass.Id.ToString());
            data.Add("idWork", "0");
            data.Add("exWork", "书画教室");
            data.Add("group", studentInfo.Group.ToString());
            data.Add("idLesson", Common.CurrentLesson.Id.ToString());
            string jsonResult = HttpUtility.UploadValues(Common.SubmitWork, data, Encoding.UTF8, Encoding.UTF8, dict);
            ResultInfo<int> result = JsonConvert.DeserializeObject<ResultInfo<int>>(jsonResult);
            if (result != null && result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                dict = new NameValueCollection();
                dict.Add("token", Common.CurrentUser.Token);
                string url = string.Format("{0}?page={1}&size=4&semester={2}&idYear={3}&idClass={4}&group={5}",
                    Common.GetWorkList, 0, Common.SemesterInfo.Semester,
                    Common.SemesterInfo.Id, Common.CurrentClass.Id, studentInfo.Owner.Index);
                jsonResult = HttpUtility.DownloadString(url, Encoding.UTF8, dict, "form-data");
                ResultInfo<StudentWorkInfo> result2 = JsonConvert.DeserializeObject<ResultInfo<StudentWorkInfo>>(jsonResult);
                if (result2 != null && result2.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    StudentWorkDetailsInfo workInfo = result2.Body.Content.FirstOrDefault(p => p.StudentId == studentInfo.Id);
                    if (workInfo != null)
                    {

                        dict = new NameValueCollection();
                        data = new NameValueCollection();
                        dict.Add("token", Common.CurrentUser.Token);
                        data.Add("id", workInfo.Id.ToString());
                        data.Add("score", "0");
                        data.Add("correct", imageUrl);
                        data.Add("comment", string.Empty);
                        jsonResult = HttpUtility.UploadValues(Common.CorrectWork, data, Encoding.UTF8, Encoding.UTF8, dict);
                        ResultInfo<StudentWorkDetailsInfo> result3 = JsonConvert.DeserializeObject<ResultInfo<StudentWorkDetailsInfo>>(jsonResult);
                        if (result3 != null && result3.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void ChangeStudent(CameraItemInfo cameraItem, StudentInfo studentInfo)
        {
            cameraItem.StudentList.ForEach(p => p.IsSelected = false);
            studentInfo.IsSelected = true;
            cameraItem.SelectedStudent = studentInfo.Name;
            cameraItem.NotifyPropertyChanged("Current");
            //Onvif.SetPreset(cameraItem, studentInfo.Preset);
            Show(studentInfo, cameraItem.Name);
            //eventAggregator.GetEvent<FullScreenChangeStudentEvent>().Publish(new CommonModel()
            //{
            //    Sender = this,
            //    Student = studentInfo
            //});
        }

        private void fullScreenGd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                exitFullScreenBtn_MouseLeftButtonDown(this, null);
                return;
            }
            if (controlBar.Visibility == Visibility.Visible)
            {
                controlBar.Visibility = Visibility.Collapsed;
            }
            else
            {
                controlBar.Visibility = Visibility.Visible;
            }
        }

        private void writeScreenBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if (loadingGd.Visibility == Visibility.Visible)
            //{
            //    return;
            //}
            if (studentInfo.FullPreviewImageSource != null)
            {
                ImageBrush imageBrush = studentInfo.FullPreviewImageSource.Clone();
                writeScreenControl.ClearDrawBoard();
                writeScreenControl.SetMode(false);
                writeScreenControl.SetBackground(imageBrush);
                writeScreenControl.Visibility = Visibility.Visible;
            }
            else
            {
                Common.ShowTip("无法点评！");
            }

        }

        private void exitFullScreenBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            StopRtmp();
            //rtmpPlayer.Stop();
            writeScreenControl.Visibility = Visibility.Collapsed;
            this.Visibility = Visibility.Collapsed;
            Common.SocketServer.StopAllCameraLive();
        }

        private void writeScreenControl_Back(object sender, EventArgs e)
        {
            if (writeScreenControl.IsChanged)
            {
                if (MessageBoxEx.ShowQuestion(Window.GetWindow(this), "是否上传点评到学生作品？", "提示") == MessageBoxResult.Yes)
                {
                    try
                    {
                        if (UploadImage(writeScreenControl.SelectedStudent))
                        {
                            MessageBoxEx.ShowInfo("点评已上传！", Window.GetWindow(this));
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.Trace("FullScreenControl writeScreenControl_Back Error:" + ex.ToString());
                    }
                }
            }
            //rtmpPlayer.RenderImage = true;
            writeScreenControl.Visibility = Visibility.Collapsed;
        }

        private void nextBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CameraItemInfo cameraItem = Common.CameraList.FirstOrDefault(p => p.StudentList.Contains(studentInfo));
            if (cameraItem != null)
            {
                int index = cameraItem.StudentList.IndexOf(studentInfo);
                if (index + 1 < cameraItem.StudentList.Count)
                {
                    Common.SocketServer.StopCameraLive(studentInfo);
                    studentInfo = cameraItem.StudentList[index + 1];
                    ChangeStudent(cameraItem, studentInfo);
                }
                else
                {
                    int index2 = Common.CameraList.IndexOf(cameraItem);
                    if (index2 + 1 < Common.CameraList.Count)
                    {
                        cameraItem = Common.CameraList[index2 + 1];
                        if (cameraItem.StudentList.Count > 0)
                        {
                            Common.SocketServer.StopCameraLive(studentInfo);
                            studentInfo = cameraItem.StudentList[0];
                            ChangeStudent(cameraItem, studentInfo);
                        }
                    }
                }
            }
            //int index = cameraItem.StudentList.IndexOf(studentInfo);
            //if (index + 1 < cameraItem.StudentList.Count)
            //{
            //    Common.SocketServer.StopCameraLive(studentInfo);
            //    studentInfo = cameraItem.StudentList[index + 1];
            //    ChangeStudent(cameraItem, studentInfo);
            //}

        }

        private void previousBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CameraItemInfo cameraItem = Common.CameraList.FirstOrDefault(p => p.StudentList.Contains(studentInfo));
            if (cameraItem != null)
            {
                int index = cameraItem.StudentList.IndexOf(studentInfo);
                if (index - 1 > -1)
                {
                    Common.SocketServer.StopCameraLive(studentInfo);
                    studentInfo = cameraItem.StudentList[index - 1];
                    ChangeStudent(cameraItem, studentInfo);
                }
                else
                {
                    int index2 = Common.CameraList.IndexOf(cameraItem);
                    if (index2 - 1 > -1)
                    {
                        cameraItem = Common.CameraList[index2 - 1];
                        if (cameraItem.StudentList.Count > 0)
                        {
                            Common.SocketServer.StopCameraLive(studentInfo);
                            studentInfo = cameraItem.StudentList[cameraItem.StudentList.Count - 1];
                            ChangeStudent(cameraItem, studentInfo);
                        }
                    }
                }
            }

            //int index = cameraItem.StudentList.IndexOf(studentInfo);
            //if (index - 1 > -1)
            //{
            //    Common.SocketServer.StopCameraLive(studentInfo);
            //    studentInfo = cameraItem.StudentList[index - 1];
            //    ChangeStudent(cameraItem, studentInfo);
            //}

        }

        public bool StartRtmp()
        {
            try
            {
                rtmpHeper = new RtmpHeper(studentInfo.Id);
                string previewUrl = string.Format("rtmp://{0}:{1}/live/{2}", Common.PushServer.host, Common.PushServer.port, studentInfo.Id);
                //previewUrl = "rtmp://liteavapp.qcloud.com/live/liteavdemoplayerstreamid";
                rtmpHeper.OnRender += Rtmp_OnRender;
                Task.Run(() => rtmpHeper.Start(previewUrl));
                return true;
            }catch (Exception)
            {
                return false;
            }
        }
        public bool StopRtmp()
        {
            try
            {
                if (rtmpHeper != null)
                {
                    rtmpHeper?.Stop();
                
                }
                this.Dispatcher.Invoke(() =>
                {
                    Image = null;
                });
                return true;
            }
            catch
            {
                return false;
            }

        }
        private void Rtmp_OnRender(int width, int height, IntPtr ptr, int count, int id)
        {
            if (studentInfo != null&& studentInfo.Id==id)
            {
                this.Dispatcher.Invoke(() =>
                {
                    if (Image == null || width !=Image.Width || height != Image.Height)
                    {
                        Image = new WriteableBitmap(width, height, 96.0, 96.0, PixelFormats.Bgr24, null);
                        studentInfo.FullPreviewImageSource = new ImageBrush(Image);
                    }
                    Image.Lock();
                    WindowsNativeMethods.Instance.CopyMemory(Image.BackBuffer, ptr, Convert.ToUInt32(count));
                    Int32Rect dirtyRect = new Int32Rect(0, 0, width, height);
                    Image.AddDirtyRect(dirtyRect);
                    Image.Unlock();
                },System.Windows.Threading.DispatcherPriority.Send);
            }
        }
    }
}

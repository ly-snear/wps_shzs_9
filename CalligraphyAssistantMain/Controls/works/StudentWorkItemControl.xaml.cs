using CalligraphyAssistantMain.Code;
using Microsoft.Win32;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace CalligraphyAssistantMain.Controls.works
{
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// StudentWorkItemControl.xaml 的交互逻辑
    /// </summary>
    public partial class StudentWorkItemControl : UserControl
    {
        private bool isBusy = false;
        public StudentWorkItemControl()
        {
            InitializeComponent();
        }
        public StudentWorkDetailsInfo StudentWorkInfo
        {
            get { return (StudentWorkDetailsInfo)GetValue(SaveMarkActionProperty); }
            set { SetValue(SaveMarkActionProperty, value); }
        }
        public event EventHandler EditImageClick = null;
        public event EventHandler ImageClick = null;
        public static readonly DependencyProperty SaveMarkActionProperty = DependencyProperty.Register("StudentWorkInfo", typeof(StudentWorkDetailsInfo),
                                                        typeof(StudentWorkItemControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnPropertyChanged)));

        private void StudentWorkInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LocalPath")
            {
                this.Dispatcher.InvokeAsync(() =>
                {
                    try
                    {
                        if (StudentWorkInfo.suffix.Contains(".mp4") || StudentWorkInfo.suffix.Contains(".avi"))
                        {
                            playMedia.Visibility = Visibility.Visible;
                            playMedia.UrlPath = StudentWorkInfo.LocalPath;
                            playMedia.StopMedia();
                            checkBox.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            image.Source = new BitmapImage(new Uri(StudentWorkInfo.LocalPath));
                            image.Visibility = Visibility.Visible;
                        }
                        loadingGd.Visibility = Visibility.Collapsed;
                    }
                    catch (Exception ex1)
                    {
                        Common.Trace("ImageItemControl3 SetImage Error1:" + ex1.Message);
                    }
                });
            }
        }


        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is StudentWorkItemControl element)
            {
                if (e.NewValue is StudentWorkDetailsInfo info)
                {
                    element.SetImage(info);
                }

            }
        }
        public void SetImage(StudentWorkDetailsInfo info)
        {
            StudentWorkInfo = info;
            if (StudentWorkInfo.review == 1)
            {
                reviewTxt.Text = "已点评";
                reviewTxt.Width = 80;
            }
            StudentWorkInfo.PropertyChanged += StudentWorkInfo_PropertyChanged;
            if (isBusy)
                return;
            isBusy = true;
            WebClient webClient = new WebClient();
            try
            {
                if (string.IsNullOrEmpty(StudentWorkInfo.LocalPath))
                {
                    return;
                }

                string downloadUrl;
                if (!string.IsNullOrEmpty(StudentWorkInfo.Correct))
                {
                    if (StudentWorkInfo.Correct.StartsWith("//"))
                    {
                        StudentWorkInfo.Correct = "http://" + StudentWorkInfo.Correct.TrimStart(new char[] { '/' });
                    }
                    downloadUrl = StudentWorkInfo.Correct;
                }
                else
                {
                    if (StudentWorkInfo.Work.StartsWith("//"))
                    {
                        StudentWorkInfo.Work = "http://" + StudentWorkInfo.Work.TrimStart(new char[] { '/' });
                    }
                    downloadUrl = StudentWorkInfo.Work;
                }
                if (File.Exists(StudentWorkInfo.LocalPath))
                {
                    if (StudentWorkInfo.suffix.Contains(".mp4") || StudentWorkInfo.suffix.Contains(".avi"))
                    {
                        playMedia.Visibility = Visibility.Visible;
                        playMedia.UrlPath = StudentWorkInfo.LocalPath;
                        playMedia.StopMedia();
                        checkBox.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        image.Source = new BitmapImage(new Uri(StudentWorkInfo.LocalPath));
                        image.Visibility = Visibility.Visible;
                    }
                    loadingGd.Visibility = Visibility.Collapsed;
                }
                else
                {
                    string tempPath = StudentWorkInfo.LocalPath + ".temp";
                    if (File.Exists(tempPath))
                    {
                        File.Delete(tempPath);
                    }
                    webClient.DownloadFileAsync(new Uri(downloadUrl), tempPath);
                    webClient.DownloadFileCompleted += (x, y) =>
                    {
                        webClient.Dispose();
                        File.Move(tempPath, StudentWorkInfo.LocalPath);
                        this.Dispatcher.InvokeAsync(() =>
                        {
                            try
                            {
                                if (StudentWorkInfo.suffix.Contains(".mp4") || StudentWorkInfo.suffix.Contains(".avi"))
                                {
                                    playMedia.Visibility = Visibility.Visible;
                                    playMedia.UrlPath = StudentWorkInfo.LocalPath;
                                    playMedia.StopMedia();
                                    checkBox.Visibility = Visibility.Collapsed;
                                }
                                else
                                {
                                    image.Source = new BitmapImage(new Uri(StudentWorkInfo.LocalPath));
                                    image.Visibility = Visibility.Visible;
                                }
                                loadingGd.Visibility = Visibility.Collapsed;
                            }
                            catch (Exception ex1)
                            {
                                Common.Trace("ImageItemControl3 SetImage Error1:" + ex1.Message);
                            }
                        });
                    };
                }
            }
            catch (Exception ex2)
            {
                webClient.Dispose();
                Common.Trace("ImageItemControl3 SetImage Error2:" + ex2.Message);
            }
            isBusy = false;
        }

        private void Button1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!File.Exists(StudentWorkInfo.LocalPath))
            {
                e.Handled = true;
                return;
            }
            if (EditImageClick != null)
            {
                EditImageClick(StudentWorkInfo, null);
            }
            e.Handled = true;
        }

        private void Button2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!File.Exists(StudentWorkInfo.LocalPath))
            {
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string correctStr = !string.IsNullOrEmpty(StudentWorkInfo.Correct) ? "[已批改]" : string.Empty;
            saveFileDialog.Filter = "*" + System.IO.Path.GetExtension(StudentWorkInfo.LocalPath) + "|*" + System.IO.Path.GetExtension(StudentWorkInfo.LocalPath);
            saveFileDialog.FileName = DateTime.Today.ToString("[yyyyMMdd]") + correctStr + StudentWorkInfo.ClassName + " - " + StudentWorkInfo.GroupName + " - " + StudentWorkInfo.StudentName;
            if ((bool)saveFileDialog.ShowDialog())
            {
                string filePath = saveFileDialog.FileName;
                try
                {
                    File.Copy(StudentWorkInfo.LocalPath, filePath);
                }
                catch (Exception ex)
                {
                    Common.Trace("ImageItemControl3 Button2_MouseLeftButtonUp Error:" + ex.Message);
                }
            }
        }

        private void Button3_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!File.Exists(StudentWorkInfo.LocalPath))
            {
                e.Handled = true;
                return;
            }
            Common.SocketServer.SendImage(StudentWorkInfo.LocalPath, 0, 0, true);
            MessageBoxEx.ShowInfo("作品已分享到学生书法屏！");
        }

        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ImageClick != null)
            {
                ImageClick(StudentWorkInfo, null);
            }
        }
        /// <summary>
        /// 分享
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnShare_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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
                if (students.Count > 0)
                {
                    Common.SaveWorkShare(students, StudentWorkInfo.Id, StudentWorkInfo.Work);
                    //通知选中的学生
                    var data = new
                    {
                        id_class = Common.CurrentClassV2.ClassId,
                        id_lesson = Common.CurrentLesson.Id,
                        id = StudentWorkInfo.Id,
                    };
                    string jsonStr = JsonConvert.SerializeObject(data);
                    if (students.Count > 0)
                    {
                        int cnt = 0;
                        if (StudentWorkInfo.suffix == ".mp4" || StudentWorkInfo.suffix == ".avi")
                        {
                            cnt = 1;
                        }
                        students.ForEach(s => MQCenter.Instance.Send(s, MessageType.WorkShare, new
                        {
                            id = StudentWorkInfo.Id,
                            type = cnt,
                            url = string.IsNullOrEmpty(StudentWorkInfo.Correct) ? StudentWorkInfo.Work : StudentWorkInfo.Correct
                        }));
                    }
                }

            }
        }
        /// <summary>
        /// 推荐
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSell_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            NameValueCollection dict = new NameValueCollection();
            dict.Add("token", Common.CurrentUser.Token);
            var data = new
            {
                id_class = Common.CurrentClassV2.ClassId,
                id_lesson = Common.CurrentLesson.Id,
                type = 1,
                sid = StudentWorkInfo.Id
            };

            string jsonResult = HttpUtility.UploadValuesJson(Common.SaveRecommend, data, Encoding.UTF8, Encoding.UTF8, dict);
            ResultInfo<int> result = JsonConvert.DeserializeObject<ResultInfo<int>>(jsonResult);
            if (result != null)
            {
                if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    Common.ShowTip("学生作品推荐成功");

                }
            }
        }
        /// <summary>
        /// 评价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnAssess_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show(StudentWorkInfo.ToJson());
            //Console.WriteLine(StudentWorkInfo.ToJson());
            #region v1
            //EventNotify.OnCheckShareList(StudentWorkInfo, "Student");
            #endregion
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
                if (students.Count <= 0)
                {
                    MessageBox.Show("选择参与互评的学生", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                Border send = sender as Border;
                send.Tag = students;
                StudentWorkInfo.type = 1;
                Console.WriteLine("学生作品：" + StudentWorkInfo.ToJson());
                EventNotify.OnStudentWorkMutualCommenOpen(send, StudentWorkInfo);
            }
        }
        /// <summary>
        /// 点评
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnReview_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (StudentWorkInfo.review == 1)
            {
                List<StudentWorkDetailsInfo> studentWorks = Common.StudentWorkList.Where(x => x.review == 1).ToList();
                if (studentWorks.Count > 0)
                {
                    EventNotify.OnCheckCommentListClick(studentWorks);
                }
            }
            else
            {
                NameValueCollection dict = new NameValueCollection();
                dict.Add("token", Common.CurrentUser.Token);
                List<object> works = new List<object>();
                works.Add(new { id_work = StudentWorkInfo.Id });
                var data = new
                {
                    id_class = Common.CurrentClassV2.ClassId,
                    id_lesson = Common.CurrentLesson.Id,
                    works
                };

                string jsonResult = HttpUtility.UploadValuesJson(Common.SaveComment, data, Encoding.UTF8, Encoding.UTF8, dict);
                ResultInfo<int> result = JsonConvert.DeserializeObject<ResultInfo<int>>(jsonResult);
                if (result != null)
                {
                    if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {

                        StudentWorkInfo.review = 1;
                        reviewTxt.Text = "已点评";
                        reviewTxt.Width = 80;
                        Common.ShowTip("学生作品点评成功");
                    }
                }
            }

        }
    }
}

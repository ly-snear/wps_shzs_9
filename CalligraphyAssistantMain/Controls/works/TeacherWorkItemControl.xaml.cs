using CalligraphyAssistantMain.Code;
using Microsoft.Win32;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Common = CalligraphyAssistantMain.Code.Common;

namespace CalligraphyAssistantMain.Controls.works
{
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// TeacherWorkItemControl.xaml 的交互逻辑
    /// </summary>
    public partial class TeacherWorkItemControl : UserControl
    {
        private bool isBusy = false;
        public TeacherWorkItemControl()
        {
            InitializeComponent();
        }
        public StudentWorkDetailsInfo TeacherWorkInfo
        {
            get { return (StudentWorkDetailsInfo)GetValue(TeacherActionProperty); }
            set { SetValue(TeacherActionProperty, value); }
        }
        public event EventHandler EditImageClick = null;
        public event EventHandler ImageClick = null;
        public event EventHandler CommentClick = null;
        public static readonly DependencyProperty TeacherActionProperty = DependencyProperty.Register("TeacherWorkInfo", typeof(StudentWorkDetailsInfo),
                                                        typeof(TeacherWorkItemControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnPropertyChanged)));
        private void TeacherWorkInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "LocalPath")
            {
                this.Dispatcher.InvokeAsync(() =>
                {
                    try
                    {
                        //image.Source = new BitmapImage(new Uri(TeacherWorkInfo.LocalPath));
                        if (TeacherWorkInfo.suffix.Contains(".mp4") || TeacherWorkInfo.suffix.Contains(".avi"))
                        {
                            playMedia.Visibility = Visibility.Visible;
                            playMedia.UrlPath = TeacherWorkInfo.LocalPath;
                            playMedia.StopMedia();
                        }
                        else
                        {
                            image.Source = new BitmapImage(new Uri(TeacherWorkInfo.LocalPath));
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
            if (d is TeacherWorkItemControl element)
            {
                if (e.NewValue is StudentWorkDetailsInfo info)
                {
                    element.SetImage(info);
                }
            }
        }
        public void SetImage(StudentWorkDetailsInfo info)
        {
            TeacherWorkInfo = info;
            TeacherWorkInfo.PropertyChanged += TeacherWorkInfo_PropertyChanged;
            if (isBusy)
                return;
            isBusy = true;
            WebClient webClient = new WebClient();
            try
            {
                if (string.IsNullOrEmpty(TeacherWorkInfo.LocalPath))
                {
                    return;
                }

                string downloadUrl;
                if (!string.IsNullOrEmpty(TeacherWorkInfo.Correct))
                {
                    if (TeacherWorkInfo.Correct.StartsWith("//"))
                    {
                        TeacherWorkInfo.Correct = "http://" + TeacherWorkInfo.Correct.TrimStart(new char[] { '/' });
                    }
                    downloadUrl = TeacherWorkInfo.Correct;
                }
                else
                {
                    if (TeacherWorkInfo.Work.StartsWith("//"))
                    {
                        TeacherWorkInfo.Work = "http://" + TeacherWorkInfo.Work.TrimStart(new char[] { '/' });
                    }
                    downloadUrl = TeacherWorkInfo.Work;
                }
                if (File.Exists(TeacherWorkInfo.LocalPath))
                {
                    //image.Source = new BitmapImage(new Uri(TeacherWorkInfo.LocalPath));
                    if (TeacherWorkInfo.suffix.Contains(".mp4") || TeacherWorkInfo.suffix.Contains(".avi"))
                    {
                        playMedia.Visibility = Visibility.Visible;
                        playMedia.UrlPath = TeacherWorkInfo.LocalPath;
                        playMedia.StopMedia();
                    }
                    else
                    {
                        image.Source = new BitmapImage(new Uri(TeacherWorkInfo.LocalPath));
                        image.Visibility = Visibility.Visible;
                    }
                    loadingGd.Visibility = Visibility.Collapsed;
                }
                else
                {
                    string tempPath = TeacherWorkInfo.LocalPath + ".temp";
                    if (File.Exists(tempPath))
                    {
                        File.Delete(tempPath);
                    }
                    webClient.DownloadFileAsync(new Uri(downloadUrl), tempPath);
                    webClient.DownloadFileCompleted += (x, y) =>
                    {
                        webClient.Dispose();
                        File.Move(tempPath, TeacherWorkInfo.LocalPath);
                        this.Dispatcher.InvokeAsync(() =>
                        {
                            try
                            {
                                image.Source = new BitmapImage(new Uri(TeacherWorkInfo.LocalPath));
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
            if (!File.Exists(TeacherWorkInfo.LocalPath))
            {
                e.Handled = true;
                return;
            }
            EditImageClick?.Invoke(TeacherWorkInfo, null);
            e.Handled = true;
        }

        private void Button2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!File.Exists(TeacherWorkInfo.LocalPath))
            {
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string correctStr = !string.IsNullOrEmpty(TeacherWorkInfo.Correct) ? "[已批改]" : string.Empty;
            saveFileDialog.Filter = "*" + System.IO.Path.GetExtension(TeacherWorkInfo.LocalPath) + "|*" + System.IO.Path.GetExtension(TeacherWorkInfo.LocalPath);
            saveFileDialog.FileName = DateTime.Today.ToString("[yyyyMMdd]") + correctStr + TeacherWorkInfo.ClassName + " - " + TeacherWorkInfo.GroupName + " - " + TeacherWorkInfo.StudentName;
            if ((bool)saveFileDialog.ShowDialog())
            {
                string filePath = saveFileDialog.FileName;
                try
                {
                    File.Copy(TeacherWorkInfo.LocalPath, filePath);
                }
                catch (Exception ex)
                {
                    Common.Trace("ImageItemControl3 Button2_MouseLeftButtonUp Error:" + ex.Message);
                }
            }
        }

        private void Button3_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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
                    if (Common.SaveWorkShare(students, TeacherWorkInfo.Id, TeacherWorkInfo.Work))
                    {
                        Common.ShowTip("教师作品分享成功");
                        //通知选中的学生
                        var data = new
                        {
                            id_class = Common.CurrentClassV2.ClassId,
                            id_lesson = Common.CurrentLesson.Id,
                            id = TeacherWorkInfo.Id,
                        };
                        string jsonStr = JsonConvert.SerializeObject(data);
                        if (students.Count > 0)
                        {
                            int cnt = 0;
                            if (TeacherWorkInfo.suffix == ".mp4" || TeacherWorkInfo.suffix == ".avi")
                            {
                                cnt = 1;
                            }
                            students.ForEach(s => MQCenter.Instance.Send(s, MessageType.WorkShare, new
                            {
                                id = TeacherWorkInfo.Id,
                                type = cnt,
                                url = string.IsNullOrEmpty(TeacherWorkInfo.Correct) ? TeacherWorkInfo.Work : TeacherWorkInfo.Correct
                            }));
                        }
                    }
                    else
                    {
                        Common.ShowTip("教师作品分享失败！");
                    }
                }
            }
        }

        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ImageClick?.Invoke(TeacherWorkInfo, null);
        }
        /// <summary>
        /// 学生点评
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button4_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            #region v1
            //EventNotify.OnCheckShareList(TeacherWorkInfo, "Teacher");
            #endregion
            TeacherWorkInfo.type = 2;
            TeacherWorkInfo.id_student = TeacherWorkInfo.id_teacher;
            TeacherWorkInfo.name_student = TeacherWorkInfo.name_teacher;
            Console.WriteLine("教师作品" + TeacherWorkInfo.ToJson());
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
                EventNotify.OnStudentWorkMutualCommenOpen(send, TeacherWorkInfo);
            }
        }
    }
}

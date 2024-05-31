using CalligraphyAssistantMain.Code;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
    /// <summary>
    /// MainControl.xaml 的交互逻辑
    /// </summary>
    public partial class MainControl : UserControl
    {
        private NavigationInfo navigationInfo = null;
        public event EventHandler CameraListClick = null;
        public event EventHandler SoftwareClick = null;
        public event EventHandler PaperClick = null;
        public event EventHandler StudentInteractClick = null;
        public event EventHandler ShowResourceClick = null;
        private bool shareDesk = false;
        private bool shareDemo = false;
        private bool shareUsbCamera = false;
        
        private bool shareClassRoomBack = false;
        public int optionTime { get; set; } = 1;

        public MainControl()
        {
            InitializeComponent();
            rightToolBar.Visibility = Visibility.Collapsed;
            camera1Text.Text = Common.Camera1Text;
            camera2Text.Text = Common.Camera2Text;
            camera3Text.Text = Common.Camera3Text;
            shareImageText.Text = Common.ShareImageText;
        }


        public void ToggleRightToolbar()
        {
            rightToolBar.Visibility = rightToolBar.Visibility == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed;
        }

        public void StartSynchronize(string resourceFolder)
        {
            resourceDirectoryControl.StartSynchronize(resourceFolder);
            resourceDirectoryControl.Visibility = Visibility.Visible;
        }

        public void ShowTeachVideo(int index)
        {
            MessageBox.Show("显示视频：" + index);
            navigationInfo.SelectedShareMenu = index;
            switch (index)
            {
                case 1:
                    Common.SocketServer.ShowVideoMode(Common.Camera1Url);
                    break;
                case 2:
                    Common.SocketServer.ShowVideoMode(Common.Camera2Url);
                    break;
                case 3:
                    Common.SocketServer.ShowVideoMode(Common.Camera3Url);
                    break;
                default:
                    break;
            }
        }
        public void ShareScreen()
        {
            // MessageBox.Show("分享屏幕");
            navigationInfo.SelectedShareMenu = 0;
            if (!string.IsNullOrEmpty(Common.ScreenShareUrl))
            {
                Common.SocketServer.ShowVideoMode(Common.ScreenShareUrl);
                Common.SocketServer.StartShareScreen();
            }
            else
            {
                if (!string.IsNullOrEmpty(Common.RtmpServerUrl))
                {
                    Common.SocketServer.StopLiveTransfer();
                    Common.SocketServer.StartShareScreen();
                }
                else
                {
                    Common.SocketServer.StopLiveTransfer();
                    Common.SocketServer.StartShareScreen();
                }
            }
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //MessageBox.Show("数据变化");
            navigationInfo = this.DataContext as NavigationInfo;
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int tag = Convert.ToInt32((sender as Grid).Tag);
            //MessageBox.Show("MouseLeftButtonUp-->" + tag);
            if (tag != -2 && !navigationInfo.IsClassBegin)
            {
                if (CameraListClick != null)
                {
                    CameraListClick(this, null);
                }
                return;
            }
            if (tag == -1)
            {
                if (CameraListClick != null)
                {
                    CameraListClick(this, null);
                }
                return;
            }
            if (tag == -2)
            {
                if (SoftwareClick != null)
                {
                    SoftwareClick(this, null);
                }
                return;
            }
            if (tag == 0)
            {
                //MessageBox.Show(Common.ScreenShareUrl + "-->" + Common.RtmpServerUrl);
                //Console.WriteLine(Common.ScreenShareUrl);
                if (!shareDesk)
                {
                    //开始分享
                    titleShareDesk1.Text = "停止";
                    titleShareDesk2.Text = "分享";
                    shareDesk = true;
                }
                else
                {
                    //停止分享
                    titleShareDesk1.Text = "分享";
                    titleShareDesk2.Text = "桌面";
                    shareDesk = false;
                }
                /*
                Common.SocketServer.ShowVideoMode(Common.ScreenShareUrl);
                if (!string.IsNullOrEmpty(Common.RtmpServerUrl))
                {
                    //Common.SocketServer.StopLiveTransfer();
                    Common.SocketServer.StartShareScreen();
                }
                else
                {
                    Common.SocketServer.StartShareScreen();
                }
                */
                //MessageBox.Show(Common.ScreenShareUrl);
                MQCenter.Instance.SendToAll(MessageType.ShareDesk, new
                {
                    state = shareDesk,
                    url = Common.ScreenShareUrl
                });
            }
            else if (tag == 4)
            {
                Common.SocketServer.ShowImageMode();
                Common.SocketServer.StopShareScreen();
                //Common.SocketServer.StopLiveTransfer();
            }
            else if (tag == 1)
            {
                shareUsbCamera = !shareUsbCamera;
                if (Common.CameraGroup1 != null)
                {
                    cameraGroupControl.SetNavigationInfo(navigationInfo);
                    cameraGroupControl.Visibility = Visibility.Visible;
                    cameraGroupControl.Play();
                    navigationInfo.SelectedShareMenu = tag;
                    MQCenter.Instance.SendToAll(MessageType.ShareUsbCamera, new
                    {
                        state = true,
                        url = Common.CameraGroup1.CameraGroupItemList[0].Url
                    });
                    return;
                }
                else
                {
                    Common.SocketServer.ShowVideoMode(Common.Camera1Url);
                }
            }
            else
            {
                //Common.SocketServer.ShowVideoMode();
                Common.SocketServer.StopShareScreen();
                switch (tag)
                {
                    //////case 1:
                    //////    shareUsbCamera = !shareUsbCamera;
                    //////    if (Common.CameraGroup1 != null)
                    //////    {
                    //////        cameraGroupControl.SetNavigationInfo(navigationInfo);
                    //////        cameraGroupControl.Visibility = Visibility.Visible;
                    //////        cameraGroupControl.Play();
                    //////        navigationInfo.SelectedShareMenu = tag;
                    //////        MQCenter.Instance.SendToAll(MessageType.ShareUsbCamera, new
                    //////        {
                    //////            state = true,
                    //////            url = Common.CameraGroup1.CameraGroupItemList[0].Url
                    //////        });
                    //////        return;
                    //////    }
                    //////    else
                    //////    {
                    //////        Common.SocketServer.ShowVideoMode(Common.Camera1Url);
                    //////    }
                    //////    break;
                    case 2:
                        //Common.SocketServer.ShowVideoMode(Common.Camera2Url);
                        shareDemo = !shareDemo;
                        MQCenter.Instance.SendToAll(MessageType.ShareDemo, new
                        {
                            state = shareDemo,
                            url = Common.Camera2Url
                        });
                        break;
                    case 3:
                        //Common.SocketServer.ShowVideoMode(Common.Camera3Url);
                        shareClassRoomBack = !shareClassRoomBack;
                        MQCenter.Instance.SendToAll(MessageType.ShareClassRoomBack, new
                        {
                            state = shareClassRoomBack,
                            url = Common.Camera3Url
                        });
                        break;
                    default:
                        break;
                }

            }
            if (navigationInfo.SelectedShareMenu == 0)
            {
                navigationInfo.SelectedShareMenu = tag;
            }
            else
            {
                if (navigationInfo.SelectedShareMenu == tag)
                {
                    return;
                }
                else
                {
                    navigationInfo.SelectedShareMenu = tag;
                }
            }
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("MouseEnter");
            navigationInfo.ShareCameraMenuHovered = true;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("MouseLeave");
            navigationInfo.ShareCameraMenuHovered = false;
        }

        private void closeBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MessageBoxEx.ShowQuestion("是否退出艺学宝？", "提示") == MessageBoxResult.Yes)
            {
                Common.EndClassOnClosing();
                Process.GetCurrentProcess().Kill();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Common.CurrentUser != null)
            {
                tipLb.Text = Common.CurrentUser.Name + " - " + Common.CurrentUser.Course;
            }
        }

        private void settingBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            settingControl.Visibility = Visibility.Visible;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            settingControl.Width = this.Width;
            settingControl.Height = this.Height;
        }

        private void resourceDirectoryBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!navigationInfo.IsClassBegin)
            {
                if (CameraListClick != null)
                {
                    CameraListClick(this, null);
                }
                return;
            }
            //resourceDirectoryControl.Visibility = Visibility.Visible;
            ShowResourceClick?.Invoke(this, e);
        }

        private void recordBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //IsRecoding = !IsRecoding;
            //if (IsRecoding)
            //{
            //    startRecordImage.Visibility = Visibility.Collapsed;
            //    stopRecordImage.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    startRecordImage.Visibility = Visibility.Visible;
            //    stopRecordImage.Visibility = Visibility.Collapsed;
            //}
            //if (RecordChanged != null)
            //{
            //    RecordChanged(this, new RecordChangedEventArgs()
            //    {
            //        Type = Convert.ToInt32(navigationInfo.SelectedShareMenu),
            //        IsRecord = IsRecoding
            //    });
            //    if (!IsRecoding)
            //    {
            //        string savePath = Common.CurrentRecordPath;
            //        if (File.Exists(savePath))
            //        {
            //            try
            //            {
            //                string folder = Common.AppPath + @"plugins\FFmpeg\";
            //                string appPath = folder + "ffmpeg.exe";
            //                string filePath = savePath.Substring(0, savePath.Length - 4);
            //                Process process = new Process();
            //                process.StartInfo.UseShellExecute = false;
            //                process.StartInfo.CreateNoWindow = true;
            //                process.StartInfo.WorkingDirectory = folder;
            //                process.StartInfo.FileName = appPath;
            //                process.StartInfo.Arguments = $"-i \"{filePath}\".flv -c copy \"{filePath}\".mp4";
            //                process.Start();
            //                process.WaitForExit(1000 * 30);
            //                string mp4Path = filePath + ".mp4";
            //                FileInfo fileInfo = new FileInfo(mp4Path);
            //                if (fileInfo.Exists && fileInfo.Length > 0)
            //                {
            //                    File.Delete(savePath);
            //                    savePath = mp4Path;
            //                }
            //            }
            //            catch (Exception ex)
            //            {
            //                Common.Trace("MainControl recordBtn_MouseLeftButtonDown Error:" + ex.Message);
            //            }
            //        }
            //        if (MessageBoxEx.ShowQuestion("录像已保存是否上传到艺淀云？", "提示") == MessageBoxResult.Yes)
            //        {
            //            //Common.SubmitVideo("d:\\测试.jpg.mp4");
            //            Common.SubmitVideo(savePath);
            //        }
            //    }
            //}
        }

        private void exercisesBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!navigationInfo.IsClassBegin)
            {
                if (CameraListClick != null)
                {
                    CameraListClick(this, null);
                }
                return;
            }
            PaperClick?.Invoke(this, e);
        }

        private void interactBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!navigationInfo.IsClassBegin)
            {
                if (CameraListClick != null)
                {
                    CameraListClick(this, null);
                }
                return;
            }
            StudentInteractClick?.Invoke(this, e);
        }

        /// <summary>
        /// 分享字帖
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void shareCopyBook_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show("分享字帖");
            EventNotify.OnSendCopyBook(sender, e);
        }
    }
}

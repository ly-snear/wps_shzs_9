using CalligraphyAssistantMain.Code;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
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
    /// ImageItemControl3.xaml 的交互逻辑
    /// </summary>
    public partial class ImageItemControl3 : UserControl
    {
        public StudentWorkDetailsInfo WorkInfo { get; private set; }
        public event EventHandler EditImageClick = null;
        public event EventHandler ImageClick = null;
        public ImageItemControl3()
        {
            InitializeComponent();
            this.DataContext = null;
        }

        public void SetImage(StudentWorkDetailsInfo workInfo, string savePath)
        {
            WebClient webClient = new WebClient();
            try
            {
                if (string.IsNullOrEmpty(savePath))
                {
                    return;
                }
                this.WorkInfo = workInfo;
                string downloadUrl;
                if (!string.IsNullOrEmpty(workInfo.Correct))
                {
                    if (workInfo.Correct.StartsWith("//"))
                    {
                        workInfo.Correct = "http://" + workInfo.Correct.TrimStart(new char[] { '/' });
                    }
                    downloadUrl = workInfo.Correct;
                }
                else
                {
                    if (workInfo.Work.StartsWith("//"))
                    {
                        workInfo.Work = "http://" + workInfo.Work.TrimStart(new char[] { '/' });
                    }
                    downloadUrl = workInfo.Work;
                }
                if (File.Exists(savePath))
                {
                    image.Source = new BitmapImage(new Uri(savePath));
                    workInfo.LocalPath = savePath;
                }
                else
                {
                    string tempPath = savePath + ".temp";
                    if (File.Exists(tempPath))
                    {
                        File.Delete(tempPath);
                    }
                    webClient.DownloadFileAsync(new Uri(downloadUrl), tempPath);
                    webClient.DownloadFileCompleted += (x, y) =>
                    {
                        webClient.Dispose();
                        File.Move(tempPath, savePath);
                        this.Dispatcher.InvokeAsync(() =>
                        {
                            try
                            {
                                image.Source = new BitmapImage(new Uri(savePath));
                                workInfo.LocalPath = savePath;
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
        }

        private void Button1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!File.Exists(WorkInfo.LocalPath))
            {
                e.Handled = true;
                return;
            }
            if (EditImageClick != null)
            {
                EditImageClick(this, null);
            }
            e.Handled = true;
        }

        private void Button2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!File.Exists(WorkInfo.LocalPath))
            {
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string correctStr = !string.IsNullOrEmpty(WorkInfo.Correct) ? "[已批改]" : string.Empty;
            saveFileDialog.Filter = "*" + System.IO.Path.GetExtension(WorkInfo.LocalPath) + "|*" + System.IO.Path.GetExtension(WorkInfo.LocalPath);
            saveFileDialog.FileName = DateTime.Today.ToString("[yyyyMMdd]") + correctStr + WorkInfo.ClassName + " - " + WorkInfo.GroupName + " - " + WorkInfo.StudentName;
            if ((bool)saveFileDialog.ShowDialog())
            {
                string filePath = saveFileDialog.FileName;
                try
                {
                    File.Copy(WorkInfo.LocalPath, filePath);
                }
                catch (Exception ex)
                {
                    Common.Trace("ImageItemControl3 Button2_MouseLeftButtonUp Error:" + ex.Message);
                }
            }
        }

        private void Button3_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!File.Exists(WorkInfo.LocalPath))
            {
                e.Handled = true;
                return;
            }
            Common.SocketServer.SendImage(WorkInfo.LocalPath, 0, 0, true);
            MessageBoxEx.ShowInfo("作品已分享到学生书法屏！");
        }

        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ImageClick != null)
            {
                ImageClick(this, null);
            }
        }
    }
}

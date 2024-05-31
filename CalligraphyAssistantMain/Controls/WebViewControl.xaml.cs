using CalligraphyAssistantMain.Code;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// <summary>
    /// WebViewControl.xaml 的交互逻辑
    /// </summary>
    public partial class WebViewControl : UserControl
    {
        private List<DownloadInfo> downloadList = new List<DownloadInfo>();
        private object lockObj = new object();
        public WebViewControl()
        {
            InitializeComponent();
        }

        public void Navigate(string url)
        {
            chrome.Address = url;
        }

        private void cancelBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void cancelBtn2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            downloadGd.Visibility = Visibility.Collapsed;
        }

        private void imageBtn1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (chrome.CanGoBack)
            {
                chrome.BackCommand.Execute(null);
            }
        }

        private void imageBtn2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (chrome.CanGoForward)
            {
                chrome.ForwardCommand.Execute(null);
            }
        }

        private void imageBtn3_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string downloadFolder = Common.SettingsPath + "Download\\";
            if (!Directory.Exists(downloadFolder))
            {
                Directory.CreateDirectory(downloadFolder);
            }
            Process.Start(downloadFolder);
        }

        private void imageBtn4_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            downloadGd.Visibility = Visibility.Visible;
        }

        private void chrome_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (e.DownloadItem.IsCancelled)
            {
                return;
            }
            DownloadInfo downloadInfo = downloadList.FirstOrDefault(p => p.Id == e.DownloadItem.Id);
            if (downloadInfo != null)
            {
                downloadInfo.IsComplete = e.DownloadItem.IsComplete;
                downloadInfo.CurrentProgress = e.DownloadItem.PercentComplete;
            }
            else
            {
                downloadInfo = new DownloadInfo()
                {
                    CurrentProgress = e.DownloadItem.PercentComplete,
                    Id = e.DownloadItem.Id,
                    FileName = System.IO.Path.GetFileName(e.DownloadItem.FullPath),
                    SavePath = e.DownloadItem.FullPath,
                    IsComplete = e.DownloadItem.IsComplete,
                    TotalProgress = 100,
                    Callback = e.Callback
                };
                lock (lockObj)
                {
                    downloadList.Add(downloadInfo);
                    this.Dispatcher.Invoke(() =>
                    {
                        DownloadItemControl downloadItemControl = new DownloadItemControl();
                        downloadItemControl.DataContext = downloadInfo;
                        downloadItemControl.CancelDownload += (x, y) =>
                        {
                            DownloadItemControl tempControl = x as DownloadItemControl;
                            DownloadInfo tempInfo = tempControl.DataContext as DownloadInfo;
                            lock (lockObj)
                            {
                                chrome.Cancel(tempInfo.Id);
                                downloadList.Remove(tempInfo);
                                downloadListSp.Children.Remove(tempControl);
                            }
                        };
                        downloadListSp.Children.Add(downloadItemControl);
                    });
                }
            }
        }
    }
}

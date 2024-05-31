using CalligraphyAssistantMain.Code;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// DownloadItemControl.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadItemControl : UserControl
    {
        public event EventHandler CancelDownload = null;
        public DownloadItemControl()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            DownloadInfo downloadInfo = this.DataContext as DownloadInfo;
            if (textBlock != null && downloadInfo != null)
            {
                try
                {
                    switch (textBlock.Text)
                    {
                        case "打开":
                            if (downloadInfo.IsComplete)
                            {
                                if (!System.IO.File.Exists(downloadInfo.SavePath))
                                {
                                    MessageBoxEx.ShowError("文件不存在！", Window.GetWindow(this));
                                    return;
                                }
                                Process.Start(downloadInfo.SavePath);
                            }
                            break;
                        case "打开文件夹":
                            string folder = System.IO.Path.GetDirectoryName(downloadInfo.SavePath);
                            Process.Start(folder);
                            //Process.Start("Explorer.exe", "/select,\"" + downloadInfo.SavePath + "\"");
                            break;
                        case "取消":
                            //downloadInfo.Callback.Cancel();
                            if (!downloadInfo.IsComplete && CancelDownload != null)
                            {
                                CancelDownload(this, null);
                            }
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Common.Trace("DownloadItemControl TextBlock_MouseLeftButtonDown Error:" + ex.Message);
                }
            }
        }
    }
}

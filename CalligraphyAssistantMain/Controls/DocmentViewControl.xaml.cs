using CalligraphyAssistantMain.Code;
using CefSharp;
using CefSharp.Wpf;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserControl = System.Windows.Controls.UserControl;

namespace CalligraphyAssistantMain.Controls
{
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// DocmentViewControl.xaml 的交互逻辑
    /// </summary>
    public partial class DocmentViewControl : UserControl
    {
        public string Url { get; set; }
        public DocmentViewControl()
        {
            InitializeComponent();
            chrome.LoadError += Chrome_LoadError;
            chrome.FrameLoadEnd += Chrome_FrameLoadEnd;
            this.DataContext = this;
         
        }

        private void Chrome_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (e.HttpStatusCode == 200)
                {
                    loading.Visibility = Visibility.Collapsed;
                }
                else
                {
                    loading.Visibility = Visibility.Collapsed;
                    loadError.Visibility = Visibility.Visible;
                }
            });
        }

        public void InitData(string fileName)
        {
            try
            {
                string url = string.Format("http://{0}:{1}/onlinePreview?url={2}", Common.DocmentServer.host, Common.DocmentServer.port, Encode(fileName));
                url = fileName;
                Console.WriteLine("源文件地址：" + fileName);
                Console.WriteLine("文件预览地址：" + url);
                loadError.Visibility = Visibility.Collapsed;
                loading.Visibility = Visibility.Visible;
                chrome.Address= url;
            }
            catch (Exception ex)
            {
                Console.WriteLine("文件预览加载失败!"+ ex.Message);
                this.Dispatcher.Invoke(() =>
                {
                    loading.Visibility = Visibility.Collapsed;
                    loadError.Visibility = Visibility.Visible;
                });

            }
        }

        private void Chrome_LoadError(object sender, LoadErrorEventArgs e)
        { 
            this.Dispatcher.Invoke(() =>
            {
                loadError.Visibility = Visibility.Visible;
            });
            Console.WriteLine("文件预览加载失败");
        }


        public string Encode(string input)
        {
            byte[] encbuff = Encoding.UTF8.GetBytes(input ?? "");
            return Convert.ToBase64String(encbuff).Replace("=", ",").Replace("+", "-").Replace("_", "/");
        }

        private void ReturnBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
            this.Visibility = Visibility.Collapsed;

        }

        private void chrome_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {

        }
    }
}

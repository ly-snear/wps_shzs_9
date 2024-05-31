using CalligraphyAssistantMain.Code;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
    /// AppControl.xaml 的交互逻辑
    /// </summary>
    public partial class AppControl : UserControl
    {
        public event EventHandler AppOpened = null;
        private string path = string.Empty;
        private string arguments = string.Empty;
        private bool isIntegrated = false;
        private bool admin = false;
        private WebViewControl webViewControl = null;
        private int auth = 0;
        public AppControl()
        {
            InitializeComponent();
        }

        public void BindApp(string title, string path, string arguments, bool admin = false, bool isIntegrated = false, WebViewControl webViewControl = null, int auth = 0)
        {
            if (File.Exists(path))
            {
                if (string.IsNullOrEmpty(title))
                {
                    titleLb.Text = System.IO.Path.GetFileNameWithoutExtension(path);
                }
                else
                {
                    titleLb.Text = title;
                }
                this.path = path;
                this.arguments = arguments;
                this.isIntegrated = isIntegrated;
                this.webViewControl = webViewControl;
                this.auth = auth;
                this.admin = admin;
                image.Source = GetIcon(path);
            }
        }

        private ImageSource GetIcon(string path)
        {
            BitmapSource bitmapSource = null;
            try
            {
                string ext = System.IO.Path.GetExtension(path).ToLower();
                switch (ext)
                {
                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                    case ".gif":
                    case ".bmp":
                    case ".ico":
                        bitmapSource = new BitmapImage(new Uri(path));
                        break;
                    case ".exe":
                        int count = WinAPI.PrivateExtractIcons(path, 0, 0, 0, null, null, 0, 0);
                        IntPtr[] intPtrs = new IntPtr[count];
                        int[] intArr = new int[count];
                        WinAPI.PrivateExtractIcons(path, 0, 48, 48, intPtrs, intArr, count, 0);
                        Icon icon = Icon.FromHandle(intPtrs[0]);
                        bitmapSource = Common.BitmapToBitmapSource(icon.ToBitmap(), PixelFormats.Pbgra32);
                        icon.Dispose();
                        for (int i = 0; i < count; i++)
                        {
                            WinAPI.DestroyIcon(intPtrs[i]);
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Common.Trace("AppControl GetIcon Error:" + ex.Message);
            }
            return bitmapSource;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //if (e.ClickCount == 2)
            //{
            try
            {
                string tempArgs = arguments;
                if (auth == 1)
                {
                    tempArgs += ("?token=" + Common.CurrentUser.Token + "&go=Home");
                }
                if (isIntegrated && webViewControl != null)
                {
                    webViewControl.Navigate(tempArgs);
                    webViewControl.Visibility = Visibility.Visible;
                    return;
                }
                Process process = new Process();
                process.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(path);
                process.StartInfo.FileName = path;
                process.StartInfo.Arguments = tempArgs;
                if (admin)
                {
                    if (Environment.OSVersion.Version.Major >= 6)
                    {
                        process.StartInfo.Verb = "runas";
                    }
                }
                process.Start();
                Window.GetWindow(this).WindowState = WindowState.Minimized;
                if (AppOpened != null)
                {
                    AppOpened(this, null);
                }
            }
            catch
            {
                MessageBox.Show("软件打开失败！");
            }
            //}
        }
    }
}

using CalligraphyAssistantMain.Code;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace CalligraphyAssistantMain.Controls
{
    /// <summary>
    /// WelcomeControl.xaml 的交互逻辑
    /// </summary>
    public partial class WelcomeControl : UserControl
    {
        private DateTime lastEnterTime = DateTime.MinValue;
        public event EventHandler ClassBegined = null;
        public WelcomeControl()
        {
            InitializeComponent();
            LoadImages();
            CheckControlBarVisibility();
        }

        private void CheckControlBarVisibility()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (controlSp.IsVisible && (DateTime.Now - lastEnterTime).TotalSeconds > 5)
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            if (IsMouseHover(goBackGd) || controlSp.Children.Cast<FrameworkElement>().Any(p => IsMouseHover(p)))
                            {
                                lastEnterTime = DateTime.Now;
                                return;
                            }
                            controlSp.Visibility = Visibility.Collapsed;
                        });
                    }
                    Thread.Sleep(1000);
                }
            });
        }

        private bool IsMouseHover(FrameworkElement element)
        {
            if (Mouse.DirectlyOver == element)
            {
                return true;
            }

            if (Mouse.DirectlyOver != null && Mouse.DirectlyOver is FrameworkElement child)
            {
                FrameworkElement parent = child.Parent as FrameworkElement;
                while (parent != null)
                {
                    if (parent == element)
                    {
                        return true;
                    }
                    else
                    {
                        parent = parent.Parent as FrameworkElement;
                    }
                }
            }
            return false;
        }

        private void LoadImages()
        {
            List<Image> imageList = Common.GetChildObjects<Image>(backGd, typeof(Image));
            for (int i = 0; i < 9; i++)
            {
                Image image = imageList.FirstOrDefault(p => p.Tag.ToString() == i.ToString());
                string imagePath = Common.AppPath + $"Images\\{i}.png";
                if (File.Exists(imagePath))
                {
                    image.Source = new BitmapImage(new Uri(imagePath));
                }
            }
        }

        private void Navigate(string url)
        {
            chromeGd.Visibility = Visibility.Visible;
            chrome.Address = url;
        }

        private void OpenProcess(string path, string appName)
        {
            if (!File.Exists(path))
            {
                MessageBox.Show(Window.GetWindow(this), $"没有找到“{appName}”！", "提示", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                Process process = new Process();
                process.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(path);
                process.StartInfo.FileName = path;
                process.Start();
                Window.GetWindow(this).WindowState = WindowState.Minimized;
            }
            catch
            {
            }
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = sender as Image;
            string tag = image.Tag.ToString();
            switch (tag)
            {
                case "0":
                    Navigate("http://shuhua.nnyun.net/private?token=" + Common.CurrentUser.Token);
                    break;
                case "1":
                    if (Common.CurrentClass != null)
                    {
                        this.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        beginClassControl.Visibility = Visibility.Visible;
                    }
                    break;
                case "2":
                    Navigate("http://yidian.nnyun.net/classwork/set?token=" + Common.CurrentUser.Token);
                    break;
                case "3":
                    OpenProcess(Common.AppPath + "CoolPaint\\CoolPaint.exe", "画板工具");
                    break;
                case "4":
                    Navigate("http://shuhua.nnyun.net/work/index?token=" + Common.CurrentUser.Token);
                    break;
                case "5":
                    OpenProcess(Common.AppPath + "ScreenRecorder\\ScreenRecorder.exe", "微课工具");
                    break;
                case "6":
                    Navigate("http://shuhua.nnyun.net/lesson/student?token=" + Common.CurrentUser.Token);
                    break;
                case "7":
                    Navigate("http://www.nnyun.net/homepage/?token=" + Common.CurrentUser.Token);
                    break;
                case "8":
                    Navigate("http://yidian.nnyun.net/home?token=" + Common.CurrentUser.Token);
                    break;
                default:
                    break;
            }
        }

        private void goBackGd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            controlSp.Visibility = Visibility.Collapsed;
            chromeGd.Visibility = Visibility.Collapsed;
        }

        private void minBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Window.GetWindow(this).WindowState = WindowState.Minimized;
        }

        private void settingBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            settingControl.Visibility = Visibility.Visible;
        }

        private void closeBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MessageBoxEx.ShowQuestion("是否退出艺学宝？", "提示") == MessageBoxResult.Yes)
            {
                Common.EndClassOnClosing();
                Process.GetCurrentProcess().Kill();
            }
        }

        private void beginClassControl_ClassBegined(object sender, EventArgs e)
        {
            if (ClassBegined != null)
            {
                ClassBegined(this, null);
                this.Visibility = Visibility.Collapsed;
            }
        }

        private void downloadFolderGd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string downloadFolder = Common.SettingsPath + "Download\\";
            if (!Directory.Exists(downloadFolder))
            {
                Directory.CreateDirectory(downloadFolder);
            }
            Process.Start(downloadFolder);
        }

        private void forwardGd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (chrome.CanGoForward)
            {
                chrome.ForwardCommand.Execute(null);
            }
        }

        private void backwardGd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (chrome.CanGoBack)
            {
                chrome.BackCommand.Execute(null);
            }
        }

        private void controlGd_MouseEnter(object sender, MouseEventArgs e)
        {
            lastEnterTime = DateTime.Now;
            controlSp.Visibility = Visibility.Visible;
        }
    }
}

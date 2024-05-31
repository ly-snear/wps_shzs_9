using CalligraphyAssistantMain.Code;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace CalligraphyAssistantMain
{
    /// <summary>
    /// FloatWindow.xaml 的交互逻辑
    /// </summary>
    public partial class FloatWindow : Window
    {
        public event EventHandler EndClass = null;
        public event EventHandler Back = null;
        public event EventHandler TakePhotos = null;
        public event EventHandler SendImage = null;
        public event EventHandler ShowTeachVideo = null;
        public int? ProcessID;
        public string ExternalApp;
        private DispatcherTimer timer;
        public FloatWindow()
        {
            InitializeComponent();
            if (SystemParameters.PrimaryScreenWidth != 1920 && SystemParameters.PrimaryScreenHeight != 1080)
            {
                this.Width *= SystemParameters.PrimaryScreenWidth / 1920;
                this.Height *= SystemParameters.PrimaryScreenHeight / 1080;
                backGd.Children.Remove(mainGd);
                viewBox.Child = mainGd;
                viewBox.Visibility = Visibility.Visible;
            }
            classSp.Visibility = Visibility.Collapsed;
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
           
        }

        public void Show(bool isClassBegin)
        {
            classSp.Visibility = isClassBegin ? Visibility.Visible : Visibility.Collapsed;
            base.Show();
            timer.Start();
        }
        public void TimerStop()
        {
            timer?.Stop();
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            if (grid != null && grid.Tag != null)
            {
                string title = grid.Tag.ToString();
                switch (title)
                {
                    case "屏写":
                        this.Visibility = Visibility.Collapsed;
                        System.Windows.Forms.Application.DoEvents();
                        Task.Run(() =>
                        {
                            Thread.Sleep(200);
                            this.Dispatcher.InvokeAsync(() =>
                            {
                                WriteScreenWindow writeScreenWindow = new WriteScreenWindow();
                                writeScreenWindow.Owner = this;
                                writeScreenWindow.Closing += (x, y) => { this.Visibility = Visibility.Visible; };
                                writeScreenWindow.SetBackground(new ImageBrush(Common.Screenshot()));
                                writeScreenWindow.Show();
                            });
                        });
                        break;
                    case "临摹":
                        if (SendImage != null)
                        {
                            SendImage(this, null);
                        }
                        break;
                    case "示范":
                        if (ShowTeachVideo != null)
                        {
                            ShowTeachVideo(this, null);
                        }
                        break;
                    case "拍照":
                        if (TakePhotos != null)
                        {
                            TakePhotos(this, null);
                        }
                        break;
                    case "返回":
                        Back?.Invoke(this, null);
                        break;
                    case "下课":
                        if (EndClass != null)
                        {
                            EndClass(this, null);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowInteropHelper windowInterop = new WindowInteropHelper(this);
            uint exStyle = WinAPI.GetWindowLong(windowInterop.Handle, WinAPI.GWL_EXSTYLE);
            exStyle &= (~WinAPI.WS_EX_APPWINDOW);    // 不显示在TaskBar
            exStyle |= WinAPI.WS_EX_TOOLWINDOW;      // 不显示在Alt-Tab
            WinAPI.SetWindowLong(windowInterop.Handle, WinAPI.GWL_EXSTYLE, exStyle);
          

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void minBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        { 
            leftToolBar.Visibility = Visibility.Collapsed;
            leftMiniToolBar.Visibility = Visibility.Visible;
        }

        private void leftMiniToolBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        { 
            leftToolBar.Visibility = Visibility.Visible;
            leftMiniToolBar.Visibility = Visibility.Collapsed;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                var wpsProcess = Process.GetProcesses().FirstOrDefault(p => p.Id == ProcessID || p.ProcessName == ExternalApp);
                if (wpsProcess == null)
                {
                    Back?.Invoke(this, null);
                }
            }
            catch (Exception)
            {
               
            }
        }
    }
}

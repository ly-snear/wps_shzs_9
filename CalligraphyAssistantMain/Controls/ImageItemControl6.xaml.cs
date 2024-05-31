using CalligraphyAssistantMain.Code;
using System;
using System.Collections.Generic;
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
    /// ImageItemControl6.xaml 的交互逻辑
    /// </summary>
    public partial class ImageItemControl6 : UserControl
    {
        public event EventHandler CheckChanged = null;
        public string ImagePath { get; private set; }
        public bool IsChecked { get { return (bool)checkBox.IsChecked; } }
        public ImageItemControl6()
        {
            InitializeComponent();
        }

        public void SetImage(string imageUrl, string savePath)
        {
            WebClient webClient = new WebClient();
            try
            {
                if (File.Exists(savePath))
                {
                    image.Source = new BitmapImage(new Uri(savePath));
                    ImagePath = savePath;
                }
                else
                {
                    string tempPath = savePath + ".temp";
                    if (File.Exists(tempPath))
                    {
                        File.Delete(tempPath);
                    }
                    if (imageUrl.StartsWith("//"))
                    {
                        imageUrl = "http://" + imageUrl.TrimStart(new char[] { '/' });
                    }
                    else
                    {
                        imageUrl = imageUrl.Replace("\\", string.Empty);
                    }
                    webClient.DownloadFileAsync(new Uri(imageUrl), tempPath);
                    webClient.DownloadFileCompleted += (x, y) =>
                    {
                        webClient.Dispose();
                        File.Move(tempPath, savePath);
                        this.Dispatcher.InvokeAsync(() =>
                        {
                            try
                            {
                                image.Source = new BitmapImage(new Uri(savePath));
                                ImagePath = savePath;
                            }
                            catch (Exception ex1)
                            {
                                Common.Trace("ImageItemControl5 SetImage Error1:" + ex1.Message);
                            }
                        });
                    };
                }
            }
            catch (Exception ex2)
            {
                webClient.Dispose();
                Common.Trace("ImageItemControl5 SetImage Error2:" + ex2.Message);
            }
        }

        public void SetChecked(bool isChecked)
        {
            checkBox.IsChecked = isChecked;
        }

        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            checkBox.IsChecked = !checkBox.IsChecked; 
            e.Handled = true;
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            if (CheckChanged != null)
            {
                CheckChanged(this, null);
            }
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (CheckChanged != null)
            {
                CheckChanged(this, null);
            }
        }
    }
}

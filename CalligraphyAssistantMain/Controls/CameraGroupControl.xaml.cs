using CalligraphyAssistantMain.Code;
using System;
using System.Collections.Generic;
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
using WPFMediaKit.DirectShow.Controls;

namespace CalligraphyAssistantMain.Controls
{
    /// <summary>
    /// CameraGroupControl.xaml 的交互逻辑
    /// </summary>
    public partial class CameraGroupControl : UserControl
    {
        private NavigationInfo navigationInfo = null;
        private int index = 0;
        private bool isOpened = false;
        private bool[] rtmpPlayerArr = new bool[3];
        private VisualBrush visualBrush = new VisualBrush();
        public CameraGroupControl()
        {
            InitializeComponent();
            playerBackGd.Background = visualBrush;
            if (Common.CameraGroup1 != null)
            {
                if (string.IsNullOrEmpty(Common.CameraGroup1.CameraGroupItemList[0].PreviewUrl))
                {
                    previewBd1.Visibility = Visibility.Collapsed;
                }
                if (string.IsNullOrEmpty(Common.CameraGroup1.CameraGroupItemList[1].PreviewUrl))
                {
                    previewBd2.Visibility = Visibility.Collapsed;
                }
                if (string.IsNullOrEmpty(Common.CameraGroup1.CameraGroupItemList[2].PreviewUrl))
                {
                    previewBd3.Visibility = Visibility.Collapsed;
                }
                if (string.IsNullOrEmpty(Common.CameraGroup1.CameraGroupItemList[0].Title))
                {
                    titleGd1.Visibility = Visibility.Collapsed;
                }
                else
                {
                    titleTxt1.Text = Common.CameraGroup1.CameraGroupItemList[0].Title;
                }
                if (string.IsNullOrEmpty(Common.CameraGroup1.CameraGroupItemList[1].Title))
                {
                    titleGd2.Visibility = Visibility.Collapsed;
                }
                else
                {
                    titleTxt2.Text = Common.CameraGroup1.CameraGroupItemList[1].Title;
                }
                if (string.IsNullOrEmpty(Common.CameraGroup1.CameraGroupItemList[2].Title))
                {
                    titleGd3.Visibility = Visibility.Collapsed;
                }
                else
                {
                    titleTxt3.Text = Common.CameraGroup1.CameraGroupItemList[2].Title;
                }
            }
        }

        public void SetNavigationInfo(NavigationInfo navigationInfo)
        {
            this.navigationInfo = navigationInfo;
        }

        public void Play()
        {
            if (Common.CameraGroup1 != null)
            {
                if (string.IsNullOrEmpty(Common.CameraGroup1.CameraGroupItemList[0].PreviewUrl))
                {
                    previewBd1.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (Common.CameraGroup1.CameraGroupItemList[0].PreviewUrl.StartsWith("usb://", StringComparison.OrdinalIgnoreCase))
                    {
                        player1.Visibility = Visibility.Collapsed;
                        usbPlayer1.Visibility = Visibility.Visible;
                        usbPlayer1.Play(Common.CameraGroup1.CameraGroupItemList[0].PreviewUrl);
                    }
                    else
                    {
                        player1.Visibility = Visibility.Visible;
                        usbPlayer1.Visibility = Visibility.Collapsed;
                        rtmpPlayerArr[0] = true;
                        player1.Play(Common.CameraGroup1.CameraGroupItemList[0].PreviewUrl);
                    }
                }
                if (string.IsNullOrEmpty(Common.CameraGroup1.CameraGroupItemList[1].PreviewUrl))
                {
                    previewBd2.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (Common.CameraGroup1.CameraGroupItemList[1].PreviewUrl.StartsWith("usb://", StringComparison.OrdinalIgnoreCase))
                    {
                        player2.Visibility = Visibility.Collapsed;
                        usbPlayer2.Visibility = Visibility.Visible;
                        usbPlayer2.Play(Common.CameraGroup1.CameraGroupItemList[1].PreviewUrl);
                    }
                    else
                    {
                        player2.Visibility = Visibility.Visible;
                        usbPlayer2.Visibility = Visibility.Collapsed;
                        rtmpPlayerArr[1] = true;
                        player2.Play(Common.CameraGroup1.CameraGroupItemList[1].PreviewUrl);
                    }
                }
                if (string.IsNullOrEmpty(Common.CameraGroup1.CameraGroupItemList[2].PreviewUrl))
                {
                    previewBd3.Visibility = Visibility.Collapsed;
                }
                else
                {
                    if (Common.CameraGroup1.CameraGroupItemList[2].PreviewUrl.StartsWith("usb://", StringComparison.OrdinalIgnoreCase))
                    {
                        player3.Visibility = Visibility.Collapsed;
                        usbPlayer3.Visibility = Visibility.Visible;
                        usbPlayer3.Play(Common.CameraGroup1.CameraGroupItemList[2].PreviewUrl);
                    }
                    else
                    {
                        player3.Visibility = Visibility.Visible;
                        usbPlayer3.Visibility = Visibility.Collapsed;
                        rtmpPlayerArr[2] = true;
                        player3.Play(Common.CameraGroup1.CameraGroupItemList[2].PreviewUrl);
                    }
                }
                if (isOpened)
                {
                    Common.SocketServer.ShowVideoMode(Common.CameraGroup1.CameraGroupItemList[index].Url);
                }
                isOpened = true;
            }
        }

        private void Stop()
        {
            this.Visibility = Visibility.Collapsed;
            player1.Stop();
            player2.Stop();
            player3.Stop();
            usbPlayer1.Stop();
            usbPlayer2.Stop();
            usbPlayer3.Stop();
            visualBrush.Visual = null;

            MQCenter.Instance.SendToAll(MessageType.ShareUsbCamera, new
            {
                state = false,
                url = "Turn off video streaming !"
            });
        }

        private void cancelBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Stop();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border border = sender as Border;
            if (border != null && border.Tag != null)
            {
                string tag = border.Tag.ToString();
                switch (tag)
                {
                    case "1":
                        previewBd1.BorderBrush = Consts.StudentControlColor2;
                        previewBd2.BorderBrush = previewBd3.BorderBrush = Brushes.Black;
                        index = 0;
                        if (player1.IsPlaying || usbPlayer1.IsPlaying)
                        {
                            if (rtmpPlayerArr[0])
                            {
                                visualBrush.Visual = player1;
                            }
                            else
                            {
                                visualBrush.Visual = usbPlayer1;
                            }
                            MQCenter.Instance.SendToAll(MessageType.ShareUsbCamera, new
                            {
                                state = true,
                                url = Common.CameraGroup1.CameraGroupItemList[0].Url
                            });
                        }
                        else
                        {
                            visualBrush.Visual = null;
                        }
                        break;
                    case "2":
                        previewBd2.BorderBrush = Consts.StudentControlColor2;
                        previewBd1.BorderBrush = previewBd3.BorderBrush = Brushes.Black;
                        index = 1;
                        if (player2.IsPlaying || usbPlayer2.IsPlaying)
                        {
                            if (rtmpPlayerArr[1])
                            {
                                visualBrush.Visual = player2;
                            }
                            else
                            {
                                visualBrush.Visual = usbPlayer2;
                            }
                            MQCenter.Instance.SendToAll(MessageType.ShareUsbCamera, new
                            {
                                state = true,
                                url = Common.CameraGroup1.CameraGroupItemList[1].Url
                            });
                        }
                        else
                        {
                            visualBrush.Visual = null;
                        }
                        break;
                    case "3":
                        previewBd3.BorderBrush = Consts.StudentControlColor2;
                        previewBd1.BorderBrush = previewBd2.BorderBrush = Brushes.Black;
                        index = 2;
                        if (player3.IsPlaying || usbPlayer3.IsPlaying)
                        {
                            if (rtmpPlayerArr[2])
                            {
                                visualBrush.Visual = player3;
                            }
                            else
                            {
                                visualBrush.Visual = usbPlayer3;
                            }
                            MQCenter.Instance.SendToAll(MessageType.ShareUsbCamera, new
                            {
                                state = true,
                                url = Common.CameraGroup1.CameraGroupItemList[2].Url
                            });
                        }
                        else
                        {
                            visualBrush.Visual = null;
                        }
                        break;
                    default:
                        return;
                }
                Common.SocketServer.ShowVideoMode(Common.CameraGroup1.CameraGroupItemList[index].Url);
            }
        }

        private void Player_Playing(object sender, EventArgs e)
        {
            if (visualBrush.Visual == null)
            {
                //this.Dispatcher.Invoke(()=> {
                switch (index)
                {
                    case 0:
                        visualBrush.Visual = (sender == player1 || sender == usbPlayer1) ? sender as FrameworkElement : null;
                        break;
                    case 1:
                        visualBrush.Visual = (sender == player2 || sender == usbPlayer2) ? sender as FrameworkElement : null;
                        break;
                    case 2:
                        visualBrush.Visual = (sender == player3 || sender == usbPlayer3) ? sender as FrameworkElement : null;
                        break;
                    default:
                        break;
                }
                //}); 
            }
        }

        private void shareBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Common.SocketServer.ShowVideoMode(Common.CameraGroup1.CameraGroupItemList[index].Url);
            Stop();
            navigationInfo.SelectedShareMenu = 1;
        }
    }
}

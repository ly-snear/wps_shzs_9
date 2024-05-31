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

namespace CalligraphyAssistantMain.Controls
{
    /// <summary>
    /// CameraGroupControlEx.xaml 的交互逻辑
    /// </summary>
    public partial class CameraGroupControlEx : UserControl
    {
        private NavigationInfo navigationInfo = null;
        public CameraGroupControlEx()
        {
            InitializeComponent();
        }

        public void SetNavigationInfo(NavigationInfo navigationInfo)
        {
            this.navigationInfo = navigationInfo;
        }

        public void Play()
        {
            if (Common.CameraGroup1 != null)
            {
                player1.Play(Common.CameraGroup1.CameraGroupItemList[0].PreviewUrl, 1);
                player2.Play(Common.CameraGroup1.CameraGroupItemList[1].PreviewUrl, 2);
                player3.Play(Common.CameraGroup1.CameraGroupItemList[2].PreviewUrl, 3);
                backPlayer.Play(Common.CameraGroup1.CameraGroupItemList[0].PreviewUrl, 4);
                Common.SocketServer.ShowVideoMode(Common.CameraGroup1.CameraGroupItemList[0].Url);
                navigationInfo.SelectedShareMenu = 1;
            }
        }

        private void Stop()
        {
            this.Visibility = Visibility.Collapsed;
            player1.Stop();
            player2.Stop();
            player3.Stop();
            backPlayer.Stop();
        }

        private void cancelBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Stop();
        }

        private void Player_Click(object sender, EventArgs e)
        {
            if (sender == player1)
            {
                previewBd1.BorderBrush = Consts.StudentControlColor2;
                previewBd2.BorderBrush = previewBd3.BorderBrush = Brushes.Black;
                Common.SocketServer.ShowVideoMode(Common.CameraGroup1.CameraGroupItemList[0].Url);
                backPlayer.Play(Common.CameraGroup1.CameraGroupItemList[0].PreviewUrl, 4);
                navigationInfo.SelectedShareMenu = 1;
            }
            else if (sender == player2)
            {
                previewBd2.BorderBrush = Consts.StudentControlColor2;
                previewBd1.BorderBrush = previewBd3.BorderBrush = Brushes.Black;
                Common.SocketServer.ShowVideoMode(Common.CameraGroup1.CameraGroupItemList[1].Url);
                backPlayer.Play(Common.CameraGroup1.CameraGroupItemList[1].PreviewUrl, 4);
                navigationInfo.SelectedShareMenu = 1;
            }
            else
            {
                previewBd3.BorderBrush = Consts.StudentControlColor2;
                previewBd1.BorderBrush = previewBd2.BorderBrush = Brushes.Black;
                Common.SocketServer.ShowVideoMode(Common.CameraGroup1.CameraGroupItemList[2].Url);
                backPlayer.Play(Common.CameraGroup1.CameraGroupItemList[2].PreviewUrl, 4);
                navigationInfo.SelectedShareMenu = 1;
            }
        }

        private void shareBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Stop();
            navigationInfo.SelectedShareMenu = 1;
        }
    }
}

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
using System.Windows.Shapes;

namespace CalligraphyAssistantMain
{
    /// <summary>
    /// TestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();
            FFmpeg.AutoGen.ffmpeg.RootPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Plugins\FFmpeg");
        }
        int index = 1;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Task.Run(() =>
            //{
            //    udpPlayer.Play(url);
            //});
            //Task.Run(() =>
            //{
                udpPlayer1.Play("http://192.168.110.251/8.utp", 1);
            //});
            //Task.Run(() =>
            //{
                udpPlayer2.Play("http://192.168.110.251/4.utp", 2);
            //});
            //Task.Run(() =>
            //{
                udpPlayer3.Play("http://192.168.110.251/8.utp", 3);
            //});
        }

        private void udpPlayer1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void udpPlayer1_Click(object sender, EventArgs e)
        {
            udpPlayer.Stop();
            udpPlayer.Play("http://192.168.110.251/8.utp", 4);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            udpPlayer1.Stop();
        }

        private void udpPlayer2_Click(object sender, EventArgs e)
        {
            udpPlayer.Stop();
            udpPlayer.Play("http://192.168.110.251/4.utp", 4);
        }

        private void udpPlayer3_Click(object sender, EventArgs e)
        {
            udpPlayer.Stop();
            udpPlayer.Play("http://192.168.110.251/8.utp", 4);
        }
    }
}

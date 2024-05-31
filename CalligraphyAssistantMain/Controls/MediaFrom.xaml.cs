using DirectShowLib;
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
using System.Windows.Shapes;



namespace CalligraphyAssistantMain.Controls
{
    /// <summary>
    /// MediaFrom.xaml 的交互逻辑
    /// </summary>
    public partial class MediaFrom : Window
    {
        public string FilePath { get; set; }
        public MediaFrom(string filePath)
        {
            InitializeComponent();

            // 绑定视频文件
            mediaElement.Source = new Uri(filePath);
            // 交互式控制
            mediaElement.LoadedBehavior = MediaState.Manual;
            // 添加元素加载完成事件 -- 自动开始播放
            mediaElement.Loaded += new RoutedEventHandler(media_Loaded);
            // 添加媒体播放结束事件 -- 重新播放
            mediaElement.MediaEnded += new RoutedEventHandler(media_MediaEnded);
            // 添加元素卸载完成事件 -- 停止播放
            mediaElement.Unloaded += new RoutedEventHandler(media_Unloaded);
        }
        /*
              元素事件 
            */
        private void media_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as MediaElement).Play();
        }
        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            // MediaElement需要先停止播放才能再开始播放，
            // 否则会停在最后一帧不动
            (sender as MediaElement).Stop();
            (sender as MediaElement).Play();
        }
        private void media_Unloaded(object sender, RoutedEventArgs e)
        {
            (sender as MediaElement).Stop();
        }
        /*
          播放控制按钮的点击事件 
        */
        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Play();
        }
        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Pause();
        }
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Stop();
        }
    }
}

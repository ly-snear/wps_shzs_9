using CalligraphyAssistantMain.Code;
using DirectShowLib;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalligraphyAssistantMain.Controls.works
{
    public enum windowType
    {
        min,
        max
    }
    /// <summary>
    /// PlayMediaControl.xaml 的交互逻辑
    /// </summary>
    public partial class PlayMediaControl : UserControl
    {

        public PlayMediaControl()
        {
            InitializeComponent();
            this.DataContext = this;
            action = new Action(() =>
            {

                mediaSlider.Value = mediaElement.Position.TotalSeconds;
                newTime.Text = mediaElement.Position.Minutes.ToString("00") + ":" + mediaElement.Position.Seconds.ToString("00");

            });
        }
        Timer timer;//时间迭代器
        Action action;

        #region 属性
        public bool isplaying
        {
            get;
            set;
        }

        /// <summary>
        /// 视频默认开始位置
        /// </summary>
        private TimeSpan position;
        public TimeSpan Position
        {
            get { return position; }
            set
            {
                position = value;
                try
                {
                    mediaElement.Position = position;
                }
                catch (Exception)
                {
                }

            }
        }

        /// <summary>
        /// 大小模式
        /// </summary>
        private windowType windowType = windowType.min;
        public windowType WindowType
        {
            get { return windowType; }
            set
            {
                windowType = value;
                if (value == windowType.max)
                {
                    windowMaxMin.Source = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/icon_media_tomin.png"));                                                                                                                                                                           
                    windowMaxMin.Tag = "max";
                }
            }
        }


        /// <summary>
        /// 视频地址
        /// </summary>
        public string UrlPath
        {
            get { return (string)GetValue(UrlPathProperty); }
            set {
                SetValue(UrlPathProperty, value);
            }
        }
        public static readonly DependencyProperty UrlPathProperty = DependencyProperty.Register("UrlPath", typeof(string),
                                                      typeof(PlayMediaControl), new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnPropertyChanged)));
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PlayMediaControl element && element!=null)
            {
                {
                    if (e.NewValue is string info)
                    {

                        try
                        {
                        element.mediaElement.Source = new Uri(info);
                        element.StopMedia();
                        }
                        catch { }
                    }

                }
            }
        }

        /// <summary>
        /// 视频音量
        /// </summary>
        public double MediaVoice
        {
            get { return mediaElement.Volume; }
            set { mediaElement.Volume = value; }
        }
        #endregion


        #region 事件

        /// <summary>
        /// 关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mediaSlider.Value = mediaElement.Position.TotalSeconds;
            newTime.Text = mediaElement.Position.Minutes.ToString("00") + ":" + mediaElement.Position.Seconds.ToString("00");
        }
        /// <summary>
        /// 点击播放暂停
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Play_pause_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image image = (Image)sender;
            if (image.Tag.Equals("pause"))//--》播放
            {
                mediaElement.Play();
                showPlayView();
            }
            else//-->暂停
            {
                mediaElement.Pause();
                showPauseView();
            }
        }
        /// <summary>
        /// 视频进度条发送变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaElement.Position = TimeSpan.FromSeconds(mediaSlider.Value);

        }


        /// <summary>
        /// 当声音进度条发现改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Voice_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            mediaElement.Volume = voice_Slider.Value;
        }

        /// <summary>
        /// 视频加载完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                Console.WriteLine("视频加载完成");
                mediaSlider.Maximum = mediaElement.NaturalDuration.TimeSpan.TotalSeconds;
                allTime.Text = mediaElement.NaturalDuration.TimeSpan.Minutes.ToString("00") + ":" + mediaElement.NaturalDuration.TimeSpan.Seconds.ToString("00");
                if (Position != null)
                {
                    try
                    {
                        mediaElement.Position = position;
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 是否是静音
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Voice_but_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (mediaElement.IsMuted)//静音模式--》有声模式
            {
                voice_but.Source = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/icon_media_voice1.png"));
                mediaElement.IsMuted = false;
                voice_Slider.IsEnabled = true;
            }
            else//有声模式--》静音模式
            {
                voice_but.Source = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/icon_media_voice0.png"));
                mediaElement.IsMuted = true;
                voice_Slider.IsEnabled = false;
            }
        }

        /// <summary>
        /// 点击缩小放大时的代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowMaxMin_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (windowType == windowType.min)//小模式--》大模式
            {
                PlayVideo playVideo = new PlayVideo();//大窗口
                                                      //显示大窗口之前的准备工作

                playVideo.media.UrlPath = UrlPath;//传递路径
                playVideo.media.Position = mediaElement.Position;//同步进度
                playVideo.media.mediaElement.IsMuted = mediaElement.IsMuted;//是否是静音
                playVideo.media.mediaElement.Volume = mediaElement.Volume;//音量
                playVideo.media.mediaSlider.Maximum = mediaSlider.Maximum;//视频进度条
                playVideo.media.mediaSlider.Value = mediaSlider.Value;//视频进度条

                playVideo.media.voice_Slider.Maximum = voice_Slider.Maximum;//声音进度条
                playVideo.media.voice_Slider.Value = voice_Slider.Value;//声音进度条
                playVideo.media.voice_Slider.IsEnabled = voice_Slider.IsEnabled;//声音进度条

                playVideo.media.voice_but.Tag = voice_but.Tag;//静音Tag
                playVideo.media.voice_but.Source = voice_but.Source;//静音图片

                //同步播放状态
                if (isplaying)
                {
                    playVideo.media.mediaElement.Play();
                    playVideo.media.showPlayView();
                }
                else
                {
                    playVideo.media.showPauseView();
                }
                mediaElement.Pause();//暂停本地
                showPauseView();//调整为暂停样式
                allView.Visibility = Visibility.Hidden;//隐藏
                //显示大窗口
                playVideo.ShowDialog();
                //关闭大窗口之后的工作

                mediaElement.Position = playVideo.media.mediaElement.Position;//进度
                mediaElement.IsMuted = playVideo.media.mediaElement.IsMuted;//是否是静音
                mediaElement.Volume = playVideo.media.mediaElement.Volume;//音量
                mediaSlider.Maximum = playVideo.media.mediaSlider.Maximum;//视频进度条
                mediaSlider.Value = playVideo.media.mediaSlider.Value;//视频进度条

                voice_Slider.Maximum = playVideo.media.voice_Slider.Maximum;//声音进度条
                voice_Slider.Value = playVideo.media.voice_Slider.Value;//声音进度条
                voice_Slider.IsEnabled = playVideo.media.voice_Slider.IsEnabled;//声音进度条

                voice_but.Tag = playVideo.media.voice_but.Tag;//静音Tag
                voice_but.Source = playVideo.media.voice_but.Source;//静音图片
                if (playVideo.media.isplaying)//同步播放状态
                {
                    mediaElement.Play();
                    showPlayView();
                }
                else
                {
                    showPauseView();
                }
                allView.Visibility = Visibility.Visible;//显示
            }
            else//大模式--》小模式
            {
                //通知PlayVideo关闭
                if (closeWindow != null)
                    closeWindow?.Invoke();
            }
        }

        //播放结束
        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            //设置视频路径
            //Console.WriteLine("设置视频路径" + urlPath);
            //mediaElement.Source = new Uri(urlPath);
            //mediaElement.Position = new TimeSpan();
            mediaSlider.Value = 0;
            newTime.Text = "00:00";
            mediaElement.Stop();
            showPauseView();
        }
        /// <summary>
        /// 播放错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaElement_ScriptCommand(object sender, MediaScriptCommandRoutedEventArgs e)
        {
            showPauseView();
            MessageBox.Show("播放错误");
        }
        /// <summary>
        /// 屏幕播放按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BigPlay_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mediaElement.Play();
            showPlayView();
        }
        /// <summary>
        /// 暂停时的布局显示
        /// </summary>
        private void showPauseView()
        {
            bigPlay.Visibility = Visibility.Visible;
            play_pause.Source = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/icon_media_play1.png"));
            play_pause.Tag = "pause";
            isplaying = false;
            if (timer != null)
            {
                timer.Enabled = false;
                timer.Stop();
                timer.Close();
                timer = null;
            }
        }

        /// <summary>
        /// 播放时的布局显示
        /// </summary>
        private void showPlayView()
        {
            bigPlay.Visibility = Visibility.Collapsed;
            play_pause.Source = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/icon_media_pause1.png"));
            play_pause.Tag = "play";
            isplaying = true;
            if (timer != null)
            {
                timer.Enabled = false;
                timer.Stop();
                timer.Close();
                timer = null;
            }


            timer = new Timer(100);
            timer.Enabled = true;//是否执行
            timer.Elapsed += Timer_Elapsed;//执行的事件
            timer.Start();


        }
        /// <summary>
        /// 延时器执行的代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {

            mediaSlider.Dispatcher.Invoke(action);

            //VideoPressage = 0;

        }

        #endregion


        #region 方法
        /// <summary>
        /// 停止并重置媒体使其从头播放。
        /// </summary>
        public void StopMedia()
        {
            try
            {
                newTime.Text = "00:00";
                mediaSlider.Value = 0;
                mediaElement.Stop();
                showPauseView();
            }catch (Exception) { }
        }
        /// <summary>
        /// 从当前位置播放媒体。
        /// </summary>
        public void playMedia()
        {
            mediaElement.Play();
            showPlayView();
        }
        /// <summary>
        /// 在当前位置暂停媒体。
        /// </summary>
        public void pauseMedia()
        {
            mediaElement.Pause();
            showPauseView();
        }
        /// <summary>
        /// 关闭媒体。
        /// </summary>
        public void closeMedia()
        {
            mediaElement.Close();
            showPauseView();
        }
        #endregion

        public delegate void CloseWindow();
        private CloseWindow closeWindow;
        public void closeWin(CloseWindow close)
        {
            this.closeWindow = close;
        }

    }

    
}

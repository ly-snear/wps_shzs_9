using FFmpeg.AutoGen;
using CalligraphyAssistantMain.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Controls
{
    public class RtmpPlayerControl_WPF : Grid
    {
        public event EventHandler Reconnect = null;
        public event EventHandler Playing = null;
        public event EventHandler BeginPlay = null;
        public event EventHandler Click = null;
        private List<byte[]> audioDataList = new List<byte[]>();
        private object lockObj = new object();
        //public bool IsPlaying { get { return isPlaying; } }
        private bool isStarted = false;
        private bool isCreatedAudioPlayer = false;
        private bool isClearingAudioResourceForChangeUrl = false;
        private bool isShowWaitImage = false;
        private bool isInLoop = false;
        private byte[] emptyBuffer = new byte[4096];
        private byte[] imageData = null;
        //private System.Drawing.Bitmap waitImage = null;
        private SDLVideoInfo videoInfo = null;
        private SDLAudioInfo audioInfo = null;
        private VideoStreamDecoderEx vsd = null;
        private VideoFrameConverterEx vfc = null;
        private WriteableBitmap renderBitmap = null;
        private WriteableBitmap waitImageRenderBitmap = null;
        private string nextPlayUrl = string.Empty;
        private string currentUrl = string.Empty;
        private DateTime lastStopTime = DateTime.MinValue;
        private float secondDataSize = 0;
        private int decodeMode = -1;//-1 自动 0 软件解码 1 硬件解码
        private DateTime nextStartTime = DateTime.MinValue;//下次开始直播时间，期间用于释放上次调用Stop的资源
        private DateTime lastThreadAlivedTime = DateTime.MinValue;
        private PlayState currentPlayState = PlayState.Stoped;
        private string currentThreadTag;
        private Thread thread = null;
        private int videoWidth = 0;
        private int videoHeight = 0;
        private ManualResetEventSlim eventSlim = null;
        public bool EnableAudio { get; set; }
        public bool IsUniformScale { get; set; }
        public bool IsPlaying { get { return currentPlayState == PlayState.Playing; } }
        public int ReconnectTimes { get; private set; }
        public int StartProbeSize { get; set; }
        public bool RenderImage { get; set; } = true;

        public RtmpPlayerControl_WPF()
        {
            ReconnectTimes = 0;
            StartProbeSize = 4096 * 10;
            IsUniformScale = true;
            IntPtr inptr = Properties.Resources.Loading.GetHbitmap();
            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
            inptr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            WinAPI.DeleteObject(inptr);
            waitImageRenderBitmap = new WriteableBitmap(bitmapSource);
            eventSlim = new ManualResetEventSlim(false);
        }

        public void SetDecodeMode(int decodeMode)
        {
            this.decodeMode = decodeMode;
        }

        public void Play(string url, bool enableAudio = false)
        {
            if (DateTime.Now < nextStartTime)
            {
                return;
            }
            //if (url.Equals(currentUrl))
            //{ 
            //        return; 
            //}
            if (isStarted)
            {
                this.EnableAudio = enableAudio;
                if (!url.Equals(currentUrl))
                {
                    nextPlayUrl = url;
                }
                //线程10秒没有反应认为线程已经异常退出
                if ((DateTime.Now - lastThreadAlivedTime).TotalSeconds > 10)
                {
                    Common.Debug("RtmpPlayerControl_WPF Play 线程长时间未响应：" + (DateTime.Now - lastThreadAlivedTime).TotalSeconds);
                    goto Init;
                }
                return;
            }
            else
            {
                if (isInLoop)
                {
                    //超过10秒上一次直播还没有停止（出现在直播过程推流中断） 强制结束线程 可能存在问题
                    if ((DateTime.Now - lastStopTime).TotalSeconds > 10)
                    {
                        Common.Debug("RtmpPlayerControl_WPF Play 线程退出等待超时：" + (DateTime.Now - lastStopTime).TotalSeconds);
                        //if (thread != null)
                        //{
                        //    thread.Abort();
                        //    thread = null;
                        //}
                        goto Init;
                    }
                    if (currentPlayState != PlayState.Stoped)
                    {
                        //已经初始化过并停止过 上次停止未完全结束转为清理资源后继续播放
                        if (currentPlayState == PlayState.Connecting ||
                          currentPlayState == PlayState.Buffering ||
                          currentPlayState == PlayState.Disposing)
                        {
                            nextPlayUrl = url;
                            isStarted = true;
                            DrawVideo();
                        }
                        return;
                    }
                    return;
                }
            }
        Init:
            currentUrl = url;
            isStarted = true;
            currentPlayState = PlayState.Prepareing;
            this.EnableAudio = enableAudio;
            SetWaitImageVisible(true);
            DrawVideo();
            currentThreadTag = Guid.NewGuid().ToString();
            Common.Debug("RtmpPlayerControl_WPF Play 初始化2：" + currentThreadTag);
            DecodeAllFramesToImages(url, currentThreadTag);
        }

        public void Stop()
        {
            nextPlayUrl = string.Empty;
            currentUrl = string.Empty;
            isStarted = false;
            lastStopTime = DateTime.Now;
            eventSlim.Set();
            //SetWaitImageVisible(false);
        }

        public void Stop(int waitTime)
        {
            nextStartTime = DateTime.Now.AddSeconds(waitTime);
            Stop();
        }

        public BitmapSource TakePhotos()
        {
            if (renderBitmap != null)
            {
                lock (lockObj)
                {
                    try
                    {
                        RenderTargetBitmap rtbitmap = new RenderTargetBitmap(renderBitmap.PixelWidth, renderBitmap.PixelHeight, renderBitmap.DpiX, renderBitmap.DpiY, PixelFormats.Default);
                        DrawingVisual drawingVisual = new DrawingVisual();
                        using (var dc = drawingVisual.RenderOpen())
                        {
                            dc.DrawImage(renderBitmap, new Rect(0, 0, renderBitmap.Width, renderBitmap.Height));
                        }
                        rtbitmap.Render(drawingVisual);
                        return rtbitmap;
                    }
                    catch (Exception ex)
                    {
                        Common.Trace("RtmpPlayerControl_WPF TakePhotos Error:" + ex.Message);
                        return null;
                    }
                }
            }
            return null;
        }

        public void Dispose()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Common.Trace("RtmpPlayerControl_WPF Dispose Error:" + ex.Message);
            }
        }

        private void SetWaitImageVisible(bool visible)
        {
            try
            {
                isShowWaitImage = visible;
                this.Dispatcher.Invoke(new Action(() =>
                {
                    try
                    {
                        this.InvalidateVisual();
                    }
                    catch (Exception ex)
                    {
                        Common.Trace("RtmpPlayerControl_WPF SetWaitImageVisible Error1:" + ex.Message);
                    }
                }));
            }
            catch (Exception ex)
            {
                Common.Trace("RtmpPlayerControl_WPF SetWaitImageVisible Error2:" + ex.Message);
            }
        }

        private unsafe void DrawVideo()
        {
            Task.Factory.StartNew(() =>
            {
                while (isStarted)
                {
                    try
                    {
                        if (imageData != null)
                        {
                            this.Dispatcher.Invoke(new Action(() =>
                            {
                            InitRenderBitmap:
                                if (renderBitmap == null)
                                {
                                    renderBitmap = new WriteableBitmap(videoWidth, videoHeight, 96, 96, PixelFormats.Bgr24, null);
                                }
                                else
                                {
                                    if (videoWidth != renderBitmap.Width || videoHeight != renderBitmap.Height)
                                    {
                                        renderBitmap = null;
                                        goto InitRenderBitmap;
                                    }
                                }
                                if (currentPlayState != PlayState.Playing)
                                {
                                    ReconnectTimes = 0;
                                    SetWaitImageVisible(false);
                                }
                                currentPlayState = PlayState.Playing;
                                if (isStarted)
                                {
                                    if (Playing != null)
                                    {
                                        Playing(this, null);
                                    }
                                }
                                if (renderBitmap != null)
                                {
                                    lock (lockObj)
                                    {
                                        renderBitmap.Lock();
                                        fixed (byte* pointer = imageData)
                                        {
                                            renderBitmap.WritePixels(new Int32Rect(0, 0, videoWidth, videoHeight), (IntPtr)pointer, videoWidth * videoHeight * 3, videoWidth * 3);
                                        }
                                        renderBitmap.Unlock();
                                    }
                                }
                                this.InvalidateVisual();
                            }));
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.Trace("RtmpPlayerControl_WPF DecodeAllFramesToImages Error3:" + ex.Message);
                    }
                    eventSlim.Wait(1000);
                    eventSlim.Reset();
                }
            }, TaskCreationOptions.LongRunning);
        }

        private unsafe void DecodeAllFramesToImages(string url, string threadTag)
        {
            //ThreadPool.QueueUserWorkItem(p =>
            thread = new Thread(p =>
            {
                isInLoop = true;
                lastThreadAlivedTime = DateTime.Now;
                while (isStarted)
                {
                    try
                    {
                        if (threadTag != currentThreadTag)
                        {
                            goto End;
                        }
                        int probeSize = StartProbeSize * (ReconnectTimes + 1);
                        if (probeSize > StartProbeSize * 10)
                        {
                            probeSize = StartProbeSize * 10;
                        }
                        lastThreadAlivedTime = DateTime.Now;
                        currentPlayState = PlayState.Connecting;
                        vsd = new VideoStreamDecoderEx(url, probeSize, true);
                        currentPlayState = PlayState.Buffering;
                        lastThreadAlivedTime = DateTime.Now;
                        if (IsStopOrChangeUrl())
                        {
                            goto End;
                        }
                    }
                    catch (Exception ex)
                    {
                        ReconnectTimes++;
                        if (isStarted)
                        {
                            if (Reconnect != null)
                            {
                                Reconnect(this, null);
                            }
                        }
                        Common.Trace("RtmpPlayerControl_WPF DecodeAllFramesToImages Error1:" + ex.Message + "\r\n" + url);
                        goto End;
                    }
                    var info = vsd.GetContextInfo();
                    var sourceSize = vsd.FrameSize;
                    var sourcePixelFormat = vsd.PixelFormat;
                    var destinationSize = sourceSize;
                    var destinationPixelFormat = AVPixelFormat.AV_PIX_FMT_BGR24;
                    try
                    {
                        if (vsd.HasAudio)
                        {
                            vfc = new VideoFrameConverterEx(sourceSize, sourcePixelFormat, destinationSize, destinationPixelFormat, (long)vsd.AudioChannelLayout, vsd.AudioSampleRate);
                        }
                        else
                        {
                            vfc = new VideoFrameConverterEx(sourceSize, sourcePixelFormat, destinationSize, destinationPixelFormat);
                        }
                        if (IsStopOrChangeUrl())
                        {
                            goto End;
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.Trace("RtmpPlayerControl_WPF DecodeAllFramesToImages Error2:" + ex.Message);
                        Thread.Sleep(1000);
                        goto End;
                    }
                    SetWaitImageVisible(false);
                    if (BeginPlay != null)
                    {
                        BeginPlay(this, null);
                    }
                    AVFrame frame;
                    AVMediaType type;
                    try
                    {
                        while (vsd.TryDecodeNextFrame(out frame, out type))
                        {
                            lastThreadAlivedTime = DateTime.Now;
                            if (IsStopOrChangeUrl())
                            {
                                goto End;
                            }
                            //var convertedFrame = vfc.Convert(vsd, frame, type);
                            var convertedFrame = vfc.ConvertEx(vsd, frame, type);
                            if (type == AVMediaType.AVMEDIA_TYPE_VIDEO)
                            {
                                videoWidth = convertedFrame.width;
                                videoHeight = convertedFrame.height;
                                int imageSize = videoWidth * videoHeight * 3;
                                if (imageData == null || imageData.Length != imageSize)
                                {
                                    lock (lockObj)
                                    {
                                        imageData = new byte[imageSize];
                                    }
                                }
                                lock (lockObj)
                                {
                                    Buffer.BlockCopy(convertedFrame.data, 0, imageData, 0, convertedFrame.data.Length);
                                }
                                eventSlim.Set();
                            }
                            if (type == AVMediaType.AVMEDIA_TYPE_AUDIO)
                            {
                                //IntPtr intPtr = new IntPtr(convertedFrame.data[0]);
                                //byte[] byteArr = new byte[convertedFrame.linesize[0]];
                                //try
                                //{
                                //    if (intPtr == IntPtr.Zero)
                                //    {
                                //        goto End;
                                //    }
                                //    Marshal.Copy(intPtr, byteArr, 0, byteArr.Length);
                                //}
                                //catch (Exception ex)
                                //{
                                //    Common.Trace("RtmpPlayerControl_WPF DecodeAllFramesToImages Error4:" + ex.Message);
                                //    goto End;
                                //}
                                //lock (lockObj)
                                //{
                                //    audioDataList.Add(byteArr);
                                //}
                                try
                                {
                                    lock (lockObj)
                                    {
                                        audioDataList.Add(convertedFrame.data);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Common.Trace("RtmpPlayerControl_WPF DecodeAllFramesToImages Error4:" + ex.Message);
                                    goto End;
                                }

                                if (vsd.HasAudio && !isCreatedAudioPlayer)
                                {
                                    isCreatedAudioPlayer = true;
                                    int nb_samples = frame.nb_samples;
                                    ThreadPool.QueueUserWorkItem(x =>
                                    {
                                        #region SDL2
                                        try
                                        {
                                            secondDataSize = vsd.AudioSampleRate * vsd.AudioBitsPerSample / 8 * vsd.AudioChannels;
                                            audioInfo = SDLHelperEx.CreateAudioPlayer(vsd.AudioSampleRate, vsd.AudioChannels, nb_samples, PCMPlayExCallback);
                                            if (audioInfo != null)
                                            {
                                                SDLHelperEx.PlayAudio(audioInfo);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Common.Trace("RtmpPlayerControl_WPF DecodeAllFramesToImages Error7:" + ex.Message);
                                        }
                                        #endregion
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.Trace("RtmpPlayerControl_WPF DecodeAllFramesToImages Error5:" + ex.Message);
                    }
                End:
                    currentPlayState = PlayState.Disposing;
                    lastThreadAlivedTime = DateTime.Now;
                    if (vfc != null)
                    {
                        vfc.Dispose();
                        vfc = null;
                    }
                    if (vsd != null)
                    {
                        vsd.Dispose();
                        vsd = null;
                    }
                    isClearingAudioResourceForChangeUrl = true;
                    renderBitmap = null;
                    int waitedTime = 0;
                    while (isCreatedAudioPlayer && isClearingAudioResourceForChangeUrl)
                    {
                        Thread.Sleep(10);
                        waitedTime += 10;
                        //音频数据5秒未清理完成 可能没有触发音频没有PCMPlayExCallback 没有外接播放设备时可能出现
                        if (waitedTime > 1000 * 5)
                        {
                            Common.Debug("RtmpPlayerControl_WPF DecodeAllFramesToImages 等待清理音频数据超时:" + waitedTime);
                            try
                            {
                                lock (lockObj)
                                {
                                    audioDataList.Clear();
                                    if (audioInfo != null)
                                    {
                                        SDLHelperEx.StopAudio(audioInfo);
                                        audioInfo = null;
                                    }
                                    isCreatedAudioPlayer = false;
                                }
                            }
                            catch
                            {
                            }
                            break;
                        }
                    }
                    try
                    {
                        //退出时不销毁 下一次创建时销毁上一次的
                        //SDLHelperEx.CloseVideoPlayer(videoInfo);
                    }
                    catch (Exception ex)
                    {
                        Common.Trace("RtmpPlayerControl_WPF DecodeAllFramesToImages Error6:" + ex.Message);
                    }
                    if (isStarted)
                    {
                        if (threadTag == currentThreadTag)
                        {
                            if (waitedTime < 1000)
                            {
                                Thread.Sleep(1000 - waitedTime);
                            }
                            if (!string.IsNullOrEmpty(nextPlayUrl))
                            {
                                url = currentUrl = nextPlayUrl;
                                nextPlayUrl = string.Empty;
                            }
                            SetWaitImageVisible(true);
                        }
                        else
                        {
                            goto Exit;
                        }
                    }
                    else
                    {
                        SetWaitImageVisible(false);
                        currentPlayState = PlayState.Stoped;
                    }
                }
            Exit:
                Common.Debug("退出循环:" + url + " - " + threadTag);
                isInLoop = false;
                thread = null;
                ////退出循环默认标记为停止播放
                //Stop();
                //currentPlayState = PlayState.Stoped; 
            });
            thread.IsBackground = true;
            thread.Start();
        }
        //SDL2
        private unsafe void PCMPlayExCallback(IntPtr userdata, IntPtr stream, int len)
        {
            if (!EnableAudio)
            {
                try
                {
                    for (int i = 0; i < len; i++)
                    {
                        ((byte*)stream)[i] = 0;
                    }
                }
                catch (Exception ex)
                {
                    Common.Trace("RtmpPlayerControl_WPF PCMPlayExCallback Error4:" + ex.Message);
                }
                goto End;
            }
            if (isStarted)
            {
                if (audioDataList.Count > 0)
                {
                    byte[] byteArr = null;
                    lock (lockObj)
                    {
                        try
                        {
                            byteArr = audioDataList[0];
                            //Common.Trace("len:" + len + " - byteArr:" + byteArr.Length);
                            Marshal.Copy(byteArr, 0, stream, (len < byteArr.Length ? len : byteArr.Length));
                            //数据超过1秒 
                            if (audioDataList.Count * byteArr.Length > secondDataSize)
                            {
                                while (audioDataList.Count > 20)
                                {
                                    audioDataList.RemoveAt(0);
                                }
                            }
                            else
                            {
                                audioDataList.RemoveAt(0);
                            }
                        }
                        catch (Exception ex1)
                        {
                            Common.Trace("RtmpPlayerControl_WPF PCMPlayExCallback Error1:" + ex1.Message);
                            try
                            {
                                for (int i = 0; i < len; i++)
                                {
                                    ((byte*)stream)[i] = 0;
                                }
                            }
                            catch (Exception ex2)
                            {
                                Common.Trace("RtmpPlayerControl_WPF PCMPlayExCallback Error2:" + ex2.Message);
                            }
                        }
                    }
                }
                else
                {
                    try
                    {
                        for (int i = 0; i < len; i++)
                        {
                            ((byte*)stream)[i] = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Common.Trace("RtmpPlayerControl_WPF PCMPlayExCallback Error3:" + ex.Message);
                    }
                }
            }
            else
            {
                audioDataList.Clear();
                if (audioInfo != null)
                {
                    SDLHelperEx.StopAudio(audioInfo);
                    audioInfo = null;
                }
                //sdl.SDL_StopAudio();
                isCreatedAudioPlayer = false;
            }
        End:
            if (isClearingAudioResourceForChangeUrl)
            {
                lock (lockObj)
                {
                    audioDataList.Clear();
                }
                isClearingAudioResourceForChangeUrl = false;
            }
        }

        private bool IsStopOrChangeUrl()
        {
            return !isStarted || !string.IsNullOrEmpty(nextPlayUrl);
        }

        public void Dispose(bool disposing)
        {
            if (videoInfo != null)
            {
                try
                {
                    SDLHelperEx.CloseVideoPlayer(videoInfo);
                    videoInfo = null;
                }
                catch (Exception ex)
                {
                    Common.Trace("RtmpPlayerControl_WPF Dispose Error:" + ex.Message);
                }
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            if (!RenderImage)
            {
                return;
            }
            //if (Common.CurrentLiveSettings.DecodeHardwareAccelerate)
            //{
            //    return;
            //}
            if (renderBitmap != null && currentPlayState == PlayState.Playing)
            {
                if (IsUniformScale)
                {
                    double x = 0;
                    double y = 0;
                    double width = 0;
                    double height = 0;
                    if (renderBitmap.Width / renderBitmap.Height >= this.ActualWidth / this.ActualHeight)
                    {
                        width = this.ActualWidth;
                        height = renderBitmap.Height * (this.ActualWidth / renderBitmap.Width);
                    }
                    else
                    {
                        height = this.ActualHeight;
                        width = renderBitmap.Width * (this.ActualHeight / renderBitmap.Height);
                    }
                    x = Math.Abs(this.ActualWidth - width) / 2;
                    y = Math.Abs(this.ActualHeight - height) / 2;
                    dc.DrawImage(renderBitmap, new Rect(x, y, width, height));

                }
                else
                {
                    dc.DrawImage(renderBitmap, new Rect(0, 0, this.ActualWidth, this.ActualHeight));
                }
            }
            else
            {
                dc.DrawRectangle(Brushes.Black, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));
            }
            if (isStarted && isShowWaitImage && waitImageRenderBitmap != null)
            {
                double x = (this.ActualWidth - waitImageRenderBitmap.Width) / 2;
                double y = (this.ActualHeight - waitImageRenderBitmap.Height) / 2;
                dc.DrawImage(waitImageRenderBitmap, new Rect(x, y, waitImageRenderBitmap.Width, waitImageRenderBitmap.Height));
            }
        }
    }
}

using CalligraphyAssistantMain.Code;
using DirectShowLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFMediaKit.DirectShow.Controls;

namespace CalligraphyAssistantMain.Controls
{
    public unsafe class USBPlayerControl : Grid
    {
        public event EventHandler Playing = null;
        public bool IsUniformScale { get; set; }
        public bool IsPlaying { get { return currentPlayState == PlayState.Playing; } }
        public bool RenderImage { get; set; } = true;
        private int cameraIndex = -1;
        private Size imageSize = Size.Empty;
        private bool isStarted = false;
        private bool isShowWaitImage = false;
        private bool isRotate = false;
        private bool isMirror = false;
        private PlayState currentPlayState = PlayState.Stoped;
        private VideoCaptureEx videoCapture = null;
        private WriteableBitmap renderBitmap = null;
        private WriteableBitmap waitImageRenderBitmap = null;
        private ManualResetEventSlim eventSlim = null;
        private object lockObj = new object();
        public USBPlayerControl()
        {
            IsUniformScale = true;
            IntPtr inptr = Properties.Resources.Loading.GetHbitmap();
            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
            inptr, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            WinAPI.DeleteObject(inptr);
            waitImageRenderBitmap = new WriteableBitmap(bitmapSource);
            eventSlim = new ManualResetEventSlim(false);
        }

        public void Play(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                string paramStr = url.Substring("usb://".Length);
                string[] paramArr = paramStr.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                if (paramArr.Length == 5)
                {
                    string[] sizeArr = paramArr[1].Split(new string[] { "x" }, StringSplitOptions.RemoveEmptyEntries);
                    isRotate = bool.Parse(paramArr[2]);
                    isMirror = bool.Parse(paramArr[3]);
                    if (sizeArr.Length == 2)
                    {
                        if (!int.TryParse(paramArr[0], out cameraIndex))
                        {
                            cameraIndex = Common.GetDeviceIndex(paramArr[0], false);
                        }
                        imageSize = new Size(int.Parse(sizeArr[0]), int.Parse(sizeArr[1]));
                        if (!isStarted)
                        {
                            //Common.Trace("USBPlayer OpenCamera : " + cameraIndex);
                            OpenCamera(cameraIndex, imageSize, isRotate, isMirror, paramArr[4]);
                        }
                    }
                }
            }
        }

        public void Stop()
        {
            if (!isStarted)
            {
                return;
            }
            //Common.Trace("USBPlayer CloseCameraA : " + cameraIndex);
            //videoCapture.Dispose();
            //Common.Trace("USBPlayer CloseCameraB : " + cameraIndex);
            videoCapture = null;
            //isStarted = false;
            //eventSlim.Set();
            //Common.Trace("USBPlayer CloseCameraC : " + cameraIndex);
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            if (!RenderImage)
            {
                return;
            }
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

        private void OpenCamera(int index, Size imageSize, bool isRotate, bool isMirror, string rtmpUrl)
        {
            videoCapture = new VideoCaptureEx(index, (int)imageSize.Width, (int)imageSize.Height, 24, isRotate, isMirror, null, rtmpUrl);
            videoCapture.BufferCB += VideoCapture_BufferCB;
            isStarted = true;
            DrawVideo();
        }

        private void DrawVideo()
        {
            try { 
            Task.Factory.StartNew(() =>
            {
                while (isStarted)
                {
                    if (videoCapture != null && videoCapture.ImageData != null && this.IsVisible)
                    {
                        this.Dispatcher.Invoke(new Action(() =>
                        {
                        InitRenderBitmap:
                            if (renderBitmap == null)
                            {
                                renderBitmap = new WriteableBitmap((int)imageSize.Width, (int)imageSize.Height, 96, 96, PixelFormats.Bgr24, null);
                            }
                            else
                            {
                                if (imageSize.Width != renderBitmap.Width || imageSize.Height != renderBitmap.Height)
                                {
                                    renderBitmap = null;
                                    goto InitRenderBitmap;
                                }
                            }
                            if (currentPlayState != PlayState.Playing)
                            {
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
                            if (videoCapture != null)
                            {
                                lock (lockObj)
                                {
                                    renderBitmap.Lock();
                                    fixed (byte* pointer = videoCapture.ImageData)
                                    {
                                        renderBitmap.WritePixels(new Int32Rect(0, 0, (int)imageSize.Width, (int)imageSize.Height), (IntPtr)pointer, (int)(imageSize.Width * imageSize.Height * 3), (int)(imageSize.Width * 3));
                                    }
                                    renderBitmap.Unlock();
                                }
                                this.InvalidateVisual();
                            }
                        }));
                    }
                    eventSlim.Wait(1000);
                    eventSlim.Reset();
                }
            }, TaskCreationOptions.LongRunning);
            }
            catch (Exception e)
            {
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【USBPlayerControl.cs Error 001】" + e.ToString());
                Console.WriteLine("【------------------------】");
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
                        //Common.Trace("RtmpPlayerControl_WPF SetWaitImageVisible Error1:" + ex.Message);
                        Console.WriteLine("【------------------------】");
                        Console.WriteLine("【USBPlayerControl.cs Error 002】" + ex.ToString());
                        Console.WriteLine("【------------------------】");
                    }
                }));
            }
            catch (Exception ex)
            {
                //Common.Trace("RtmpPlayerControl_WPF SetWaitImageVisible Error2:" + ex.Message);
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【USBPlayerControl.cs Error 003】" + ex.ToString());
                Console.WriteLine("【------------------------】");
            }
        }

        private void VideoCapture_BufferCB(object sender, BufferCBEventArgs e)
        {
            if (this.IsVisible)
            {
                eventSlim.Set();
            }
            this.Dispatcher.Invoke(() =>
            {
                if (Playing != null)
                {
                    Playing(this, null);
                }
            });
        }
    }
}

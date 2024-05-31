using FFmpeg.AutoGen;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.ExceptionServices;
using System.Windows;

namespace CalligraphyAssistantMain.Code
{
    public class ScreenCapture : IAVPacketProvider
    {
        public event EventHandler<NewAVPacketEventArgs> NewAVPacket;
        public Rectangle ScreenRect { get; private set; }
        public bool IsCursorHighlight { get; set; }
        public int FPS { get; private set; }
        public bool PauseCapture { get; set; }
        public bool IsStoped { get { return isStoped; } }
        private Thread thread = null;
        private Bitmap bitmap = null;
        private Bitmap scaleImage = null;
        private ImageConverterInfo imageConverterInfo = null;
        private Graphics g = null;
        private Graphics scaleG = null;
        private ImageConverterInfo rgb2yuvConverterInfo = null;
        private VideoEncoder videoEncoder = null;
        private AVPacketSender packetSender = null;
        private bool isStoped = false;
        private byte[] buffer = null;
        private bool startCapture = false;
        private bool isResize = false;
        private object lockObj = new object();
        private int outputWidth;
        private int outputHeight;
        private float screenScaleX = 1.0f;
        private float screenScaleY = 1.0f;
        private bool useDXGI = false;
        private bool useGDIPlusScale = false;
        private bool isDisposed = false;
        private string rtmpUrl = string.Empty;

        #region DXGI
        private Texture2D screenTexture;
        private Device device;
        private Output1 output1;
        private OutputDuplication duplicatedOutput;
        private OutputDuplicateFrameInformation duplicateFrameInformation;
        private SharpDX.DXGI.Resource screenResource;
        private int dxgiWidth = 0;
        private int dxgiHeight = 0;
        #endregion

        public ScreenCapture(Rectangle screenRect, int outputWidth, int outputHeight, int fps, string url)
        {
            this.rtmpUrl = url;
            Init(screenRect, outputWidth, outputHeight, fps);
        }

        public ScreenCapture(int x, int y, int width, int height, int outputWidth, int outputHeight, int fps, string url)
        {
            this.rtmpUrl = url;
            Init(new Rectangle(x, y, width, height), outputWidth, outputHeight, fps);
        }

        public void Stop()
        {
            startCapture = false;
        }

        [HandleProcessCorruptedStateExceptions]
        public void Dispose()
        {
            //if (!startCapture)
            //{
            //    return;
            //}
            if (isDisposed)
            {
                return;
            }
            isDisposed = true;
            Stop();
            while (!isStoped)
            {
                Thread.Sleep(1);
            }
            if (g != null)
            {
                g.Dispose();
                g = null;
            }
            if (bitmap != null)
            {
                bitmap.Dispose();
                bitmap = null;
            }
            if (imageConverterInfo != null)
            {
                ImageConverter.CloseConverter(imageConverterInfo);
                imageConverterInfo = null;
            }
            if (scaleG != null)
            {
                scaleG.Dispose();
                scaleG = null;
            }
            if (scaleImage != null)
            {
                scaleImage.Dispose();
                scaleImage = null;
            }
            if (imageConverterInfo != null)
            {
                ImageConverter.CloseConverter(imageConverterInfo);
                imageConverterInfo = null;
            }
            if (rgb2yuvConverterInfo != null)
            {
                ImageConverter.CloseConverter(rgb2yuvConverterInfo);
                rgb2yuvConverterInfo = null;
            }
            if (videoEncoder != null)
            {
                videoEncoder.CloseAVCodec();
            }
            if (packetSender != null)
            {
                packetSender.Stop();
            }
            DisposeDXGI();
        }

        private void Init(Rectangle screenRect, int outputWidth, int outputHeight, int fps)
        {
            ScreenRect = screenRect;
            screenScaleX = Common.ScaleX;
            screenScaleY = Common.ScaleY;
            FPS = fps;
            this.outputWidth = outputWidth;
            this.outputHeight = outputHeight;
        }

        private void CheckDXGI()
        {
            //useDXGI = false;
            //return;
            if (outputWidth < outputHeight)
            {
                return;
            }
            Version compareToVersion = new Version("6.2");//Win8及以上
            useDXGI = Environment.OSVersion.Version >= compareToVersion;
            if (useDXGI)
            {
                try
                {
                    InitDXGI();
                }
                catch (Exception ex)
                {
                    useDXGI = false;
                    Common.Trace("ScreenCapture: CheckDXGI Error:" + ex.ToString());
                    Console.WriteLine("【------------------------】");
                    Console.WriteLine("【ScreenCapture Error 001】" + ex.ToString());
                    Console.WriteLine("【------------------------】");
                }
            }
        }

        [HandleProcessCorruptedStateExceptions]
        private unsafe void Capture()
        {
            int per = (int)(1000f / FPS);
            Stopwatch stopwatch = new Stopwatch();
            int frame = 1;
            while (startCapture)
            {
                if (PauseCapture)
                {
                    Thread.Sleep(per);
                    continue;
                }
                stopwatch.Restart();
                try
                {
                    if (useDXGI)
                    {
                        CopyScreenFromDXGI(bitmap);
                    }
                    else
                    {
                        g.CopyFromScreen(ScreenRect.X, ScreenRect.Y, 0, 0, ScreenRect.Size);
                    }
                }
                catch (System.ComponentModel.Win32Exception ex)
                {
                }
                catch (Exception ex)
                {
                    Common.Trace("ScreenCapture: Capture Error2:" + ex.ToString());
                    Console.WriteLine("【------------------------】");
                    Console.WriteLine("【ScreenCapture Error 002】" + ex.ToString());
                    Console.WriteLine("【------------------------】");
                    startCapture = false;
                }
                if (!startCapture)
                {
                    goto End;
                }
                //if (NewAVPacket != null)
                {
                    BitmapData data = null;
                    if (isResize)//缩放
                    {
                        try
                        {
                            if (useGDIPlusScale)
                            {
                                if (scaleImage == null)
                                {
                                    scaleImage = new Bitmap(outputWidth, outputHeight, PixelFormat.Format24bppRgb);
                                    scaleG = Graphics.FromImage(scaleImage);
                                    rgb2yuvConverterInfo = ImageConverter.CreateConverter(AVPixelFormat.AV_PIX_FMT_BGR24, AVPixelFormat.AV_PIX_FMT_YUV420P, outputWidth, outputHeight);
                                }
                                scaleG.DrawImage(bitmap, new Rectangle(0, 0, outputWidth, outputHeight), new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
                                BitmapData data2 = scaleImage.LockBits(new Rectangle(System.Drawing.Point.Empty, scaleImage.Size), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                                ImageConverter.Convert(rgb2yuvConverterInfo, (byte*)data2.Scan0, ref buffer);
                                scaleImage.UnlockBits(data2);
                            }
                            else
                            {
                                data = bitmap.LockBits(new Rectangle(System.Drawing.Point.Empty, bitmap.Size), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                                ImageConverter.Convert(imageConverterInfo, (byte*)data.Scan0, ref buffer);
                            }
                        }
                        catch (Exception ex)
                        {
                            useGDIPlusScale = true;
                            if (data != null)
                            {
                                bitmap.UnlockBits(data);
                                data = null;
                            }
                            Common.Trace("ScreenCapture Capture Error3:" + ex.Message);
                            Console.WriteLine("【------------------------】");
                            Console.WriteLine("【ScreenCapture Error 003】" + ex.ToString());
                            Console.WriteLine("【------------------------】");
                            goto Next;
                        }
                    }
                    else
                    {
                        data = bitmap.LockBits(new Rectangle(System.Drawing.Point.Empty, bitmap.Size), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                        Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);
                    }
                    if (data != null)
                    {
                        bitmap.UnlockBits(data);
                        data = null;
                    }
                    AVPacket packet;
                    if (videoEncoder.EncodeVideoFrame(buffer, frame, out packet))
                    {
                        try
                        {
                            //if (packetSender != null)
                            //{
                            //    NewAVPacket(this, new NewAVPacketEventArgs()
                            //    {
                            //        Frame = frame,
                            //        Packet = packet,
                            //        Size = new System.Windows.Size(outputWidth, outputHeight)
                            //    });
                            //}
                            if (packetSender != null)
                            {
                                packet.pts = packet.dts = packetSender.GetTimestamp();
                                packetSender.AddPacket(packet);
                            }
                            else
                            {
                                ffmpeg.av_packet_unref(&packet);
                            }
                            frame++;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("【------------------------】");
                            Console.WriteLine("【ScreenCapture Error 004】" + ex.ToString());
                            Console.WriteLine("【------------------------】");
                        }
                    }
                }
            Next:
                long useTime = stopwatch.ElapsedMilliseconds;
                if (useTime < per)
                {
                    Thread.Sleep((int)(per - useTime));
                }
            }
        End:
            stopwatch.Stop();
            isStoped = true;
        }

        private void InitDXGI()
        {
            // # of graphics card adapter
            int numAdapter = 0;
            // # of output device (i.e. monitor)
            int numOutput = 0;
            // Create DXGI Factory1
            Factory1 factory = new Factory1();
            Adapter1 adapter = factory.GetAdapter1(numAdapter);
            // Create device from Adapter
            device = new Device(adapter);
            // Get DXGI.Output
            Output output = adapter.GetOutput(numOutput);
            output1 = output.QueryInterface<Output1>();
            // Width/Height of desktop to capture
            dxgiWidth = output.Description.DesktopBounds.Right - output.Description.DesktopBounds.Left;
            dxgiHeight = output.Description.DesktopBounds.Bottom - output.Description.DesktopBounds.Top;
            // Create Staging texture CPU-accessible
            Texture2DDescription textureDesc = new Texture2DDescription
            {
                CpuAccessFlags = CpuAccessFlags.Read,
                BindFlags = BindFlags.None,
                Format = SharpDX.DXGI.Format.B8G8R8A8_UNorm,
                Width = dxgiWidth,
                Height = dxgiHeight,
                OptionFlags = ResourceOptionFlags.None,
                MipLevels = 1,
                ArraySize = 1,
                SampleDescription = { Count = 1, Quality = 0 },
                Usage = ResourceUsage.Staging
            };
            screenTexture = new Texture2D(device, textureDesc);
            // Duplicate the output
            duplicatedOutput = output1.DuplicateOutput(device);
            //第一次获取可能是黑屏 前几次可能会绿屏
            for (int i = 0; i < 5; i++)
            {
                CopyScreenFromDXGI(null);
            }
        }

        private void DisposeDXGI()
        {
            if (useDXGI)
            {
                try
                {
                    if (screenTexture != null)
                    {
                        screenTexture.Dispose();
                        screenTexture = null;
                    }
                    if (device != null)
                    {
                        device.Dispose();
                        device = null;
                    }
                    if (output1 != null)
                    {
                        output1.Dispose();
                        output1 = null;
                    }
                    if (duplicatedOutput != null)
                    {
                        duplicatedOutput.Dispose();
                        duplicatedOutput = null;
                    }
                    if (screenResource != null)
                    {
                        screenResource.Dispose();
                        screenResource = null;
                    }
                }
                catch (Exception ex)
                {
                    Common.Trace("ScreenCapture: DisposeDXGI error:" + ex.ToString());
                    Console.WriteLine("【------------------------】");
                    Console.WriteLine("【ScreenCapture Error 005】" + ex.ToString());
                    Console.WriteLine("【------------------------】");
                }
            }
        }

        private unsafe void CopyScreenFromDXGI(Bitmap bitmap, int timeout = 0)
        {
            try
            {
                // Try to get duplicated frame within given time
                duplicatedOutput.AcquireNextFrame(timeout, out duplicateFrameInformation, out screenResource);
                // copy resource into memory that can be accessed by the CPU
                using (Texture2D screenTexture2D = screenResource.QueryInterface<Texture2D>())
                    device.ImmediateContext.CopyResource(screenTexture2D, screenTexture);
                // Get the desktop capture texture
                DataBox mapSource = device.ImmediateContext.MapSubresource(screenTexture, 0, MapMode.Read, MapFlags.None);
                if (bitmap != null)
                {
                    BitmapData mapTarget = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, bitmap.PixelFormat);
                    int targetWidth = ScreenRect.Width;
                    int targetHeight = ScreenRect.Height;
                    int startX = ScreenRect.Left;
                    int startY = ScreenRect.Top;
                    int offsetX = startX * 4;
                    byte* sourceP = (byte*)mapSource.DataPointer;
                    byte* targetP = (byte*)mapTarget.Scan0;
                    if (startY > 0)
                    {
                        sourceP += mapSource.RowPitch * startY;
                    }
                    for (int y = 0; y < targetHeight; y++)
                    {
                        sourceP += offsetX;
                        for (int x = 0; x < targetWidth; x++)
                        {
                            targetP[0] = sourceP[0];
                            targetP[1] = sourceP[1];
                            targetP[2] = sourceP[2];
                            targetP += 3;
                            sourceP += 4;
                        }
                        targetP += (mapTarget.Stride - (targetWidth * 3));
                        sourceP += (mapSource.RowPitch - (targetWidth * 4 + offsetX));
                    }
                    bitmap.UnlockBits(mapTarget);
                }
                device.ImmediateContext.UnmapSubresource(screenTexture, 0);
                screenResource.Dispose();
                duplicatedOutput.ReleaseFrame();
            }
            catch (SharpDXException ex)
            {
                if (ex.ResultCode.Code != SharpDX.DXGI.ResultCode.WaitTimeout.Result.Code)
                {
                    //startCapture = false;
                    useDXGI = false;
                    //Common.Trace("ScreenCapture: CopyScreenFromDXGI error:" + ex.ToString());
                    Console.WriteLine("【------------------------】");
                    Console.WriteLine("【ScreenCapture Error 006】" + ex.ToString());
                    Console.WriteLine("【------------------------】");
                }
            }
        }

        public void Start()
        {
            if (startCapture)
            {
                return;
            }
            IsCursorHighlight = true;
            startCapture = true;
            bitmap = new Bitmap(ScreenRect.Width, ScreenRect.Height, PixelFormat.Format24bppRgb);
            if (ScreenRect.Width != outputWidth || ScreenRect.Height != outputHeight)//缩放
            {
                isResize = true;
                buffer = new byte[outputWidth * outputHeight * 3];
                imageConverterInfo = ImageConverter.CreateConverter(AVPixelFormat.AV_PIX_FMT_BGR24, AVPixelFormat.AV_PIX_FMT_YUV420P, ScreenRect.Width, ScreenRect.Height, outputWidth, outputHeight);
            }
            else
            {
                BitmapData data = bitmap.LockBits(new Rectangle(0, 0, ScreenRect.Width, ScreenRect.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                buffer = new byte[data.Stride * ScreenRect.Height];
                bitmap.UnlockBits(data);
            }
            CheckDXGI();
            videoEncoder = new VideoEncoder();

            bool cnt = videoEncoder.InitAVCodec(outputWidth, outputHeight, FPS, rtmpUrl);//开始推流
            if (cnt)
            {
                if (!string.IsNullOrEmpty(rtmpUrl))
                {
                    packetSender = new AVPacketSender(videoEncoder);
                    packetSender.Start();
                }
                g = Graphics.FromImage(bitmap);
                thread = new Thread(Capture)
                {
                    IsBackground = true
                };
                thread.Start();
            }
            else
            {
                if (MessageBox.Show("桌面推流地址重复，是否退出系统？", "退出系统？", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    Application.Current.Shutdown();
                }
            }
            //////videoEncoder.InitAVCodec(outputWidth, outputHeight, FPS, rtmpUrl);
            //////if (!string.IsNullOrEmpty(rtmpUrl))
            //////{
            //////    packetSender = new AVPacketSender(videoEncoder);
            //////    packetSender.Start();
            //////}
            //////g = Graphics.FromImage(bitmap);
            //////thread = new Thread(Capture);
            //////thread.IsBackground = true;
            //////thread.Start();
        }
    }
}

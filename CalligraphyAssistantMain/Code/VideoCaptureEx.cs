/****************************************************************************
While the underlying libraries are covered by LGPL, this sample is released 
as public domain.  It is distributed in the hope that it will be useful, but 
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE.  
*****************************************************************************/

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using DirectShowLib;
using FFmpeg.AutoGen;

namespace CalligraphyAssistantMain.Code
{
    /// <summary> Summary description for MainForm. </summary>
    public unsafe class VideoCaptureEx : ISampleGrabberCB, IDisposable
    {

        #region Member variables

        /// <summary> graph builder interface. </summary>
        private IFilterGraph2 m_FilterGraph = null;

        // Used to snap picture on Still pin
        private IAMVideoControl m_VidControl = null;
        private IPin m_pinStill = null;
        private IBaseFilter capFilter = null;
        /// <summary> so we can wait for the async job to finish </summary>
        private ManualResetEvent m_PictureReady = null;

        private bool m_WantOne = false;

        /// <summary> Dimensions of the image, calculated once in constructor for perf. </summary>
        private int m_videoWidth;
        private int m_videoHeight;
        private int m_stride;
        private string m_rtmpUrl;
        private VideoEncoder videoEncoder = null;
        private AVPacketSender packetSender = null;
        private ImageConverterInfo imageConverterInfo = null;
        private byte[] imageData = null;
        private byte[] yuvData = null;
        private int frame = 1;
        private bool isRotate = false;
        private bool isMirror = false;

        /// <summary> buffer for bitmap data.  Always release by caller</summary>
        private IntPtr m_ipBuffer = IntPtr.Zero;
        public bool PauseCapture { get; set; }
        public IAMVideoProcAmp AMVideoProcAmp { get { return capFilter as IAMVideoProcAmp; } }
        public IAMVideoControl VideoControl { get { return m_VidControl as IAMVideoControl; } }
        public IBaseFilter BaseFilter { get { return capFilter; } }
        public byte[] ImageData { get { return imageData; } }
#if DEBUG
        // Allow you to "Connect to remote graph" from GraphEdit
        DsROTEntry m_rot = null;
#endif
        #endregion

        #region APIs
        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
        private static extern void CopyMemory(IntPtr Destination, IntPtr Source, [MarshalAs(UnmanagedType.U4)] int Length);
        #endregion
        public event BufferCBEventHandler BufferCB = null;
        // Zero based device index and device params and output window
        public VideoCaptureEx(int iDeviceNum, int iWidth, int iHeight, short iBPP, bool isRotate, bool isMirror, Control hControl, string rtmpUrl)
        {
            DsDevice[] capDevices;
            this.isRotate = isRotate;
            this.isMirror = isMirror;
            // Get the collection of video devices
            capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            m_rtmpUrl = rtmpUrl;
            m_videoWidth = iWidth;
            m_videoHeight = iHeight;
            videoEncoder = new VideoEncoder();
            videoEncoder.InitAVCodec(iWidth, iHeight, 15, rtmpUrl);
            if (!string.IsNullOrEmpty(rtmpUrl))
            {
                packetSender = new AVPacketSender(videoEncoder);
                packetSender.Start();
            }
            if (iDeviceNum + 1 > capDevices.Length)
            {
                throw new Exception("No video capture devices found at that index!");
            }

            try
            {
                // Set up the capture graph
                SetupGraph(capDevices[iDeviceNum], iWidth, iHeight, iBPP, hControl);

                // tell the callback to ignore new images
                m_PictureReady = new ManualResetEvent(false);
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }
        }

        /// <summary> release everything. </summary>
        public void Dispose()
        {
#if DEBUG
            if (m_rot != null)
            {
                m_rot.Dispose();
            }
#endif
            CloseInterfaces();
            if (m_PictureReady != null)
            {
                m_PictureReady.Close();
            }
            if (videoEncoder != null)
            {
                videoEncoder.CloseAVCodec();
            }
            if (packetSender != null)
            {
                packetSender.Stop();
            }
            if (imageConverterInfo != null)
            {
                ImageConverter.CloseConverter(imageConverterInfo);
                imageConverterInfo = null;
            }
        }
        // Destructor
        ~VideoCaptureEx()
        {
            Dispose();
        }

        /// <summary>
        /// Get the image from the Still pin.  The returned image can turned into a bitmap with
        /// Bitmap b = new Bitmap(cam.Width, cam.Height, cam.Stride, PixelFormat.Format24bppRgb, m_ip);
        /// If the image is upside down, you can fix it with
        /// b.RotateFlip(RotateFlipType.RotateNoneFlipY);
        /// </summary>
        /// <returns>Returned pointer to be freed by caller with Marshal.FreeCoTaskMem</returns>
        public IntPtr Click()
        {
            int hr;

            // get ready to wait for new image
            m_PictureReady.Reset();
            m_ipBuffer = Marshal.AllocCoTaskMem(Math.Abs(m_stride) * m_videoHeight);

            try
            {
                m_WantOne = true;

                // If we are using a still pin, ask for a picture
                if (m_VidControl != null)
                {
                    // Tell the camera to send an image
                    hr = m_VidControl.SetMode(m_pinStill, VideoControlFlags.Trigger);
                    DsError.ThrowExceptionForHR(hr);
                }

                // Start waiting
                if (!m_PictureReady.WaitOne(9000, false))
                {
                    throw new Exception("Timeout waiting to get picture");
                }
            }
            catch
            {
                Marshal.FreeCoTaskMem(m_ipBuffer);
                m_ipBuffer = IntPtr.Zero;
                throw;
            }

            // Got one
            return m_ipBuffer;
        }

        public int Width
        {
            get
            {
                return m_videoWidth;
            }
        }
        public int Height
        {
            get
            {
                return m_videoHeight;
            }
        }
        public int Stride
        {
            get
            {
                return m_stride;
            }
        }


        /// <summary> build the capture graph for grabber. </summary>
        private void SetupGraph(DsDevice dev, int iWidth, int iHeight, short iBPP, Control hControl)
        {
            int hr;

            ISampleGrabber sampGrabber = null;
            IPin pCaptureOut = null;
            IPin pSampleIn = null;
            IPin pRenderIn = null;

            // Get the graphbuilder object
            m_FilterGraph = new FilterGraph() as IFilterGraph2;

            try
            {
#if DEBUG
                m_rot = new DsROTEntry(m_FilterGraph);
#endif
                // add the video input device
                hr = m_FilterGraph.AddSourceFilterForMoniker(dev.Mon, null, dev.Name, out capFilter);
                DsError.ThrowExceptionForHR(hr);

                //// Find the still pin
                //m_pinStill = DsFindPin.ByCategory(capFilter, PinCategory.Still, 0);

                //// Didn't find one.  Is there a preview pin?
                //if (m_pinStill == null)
                //{
                //    m_pinStill = DsFindPin.ByCategory(capFilter, PinCategory.Preview, 0);
                //}

                // Still haven't found one.  Need to put a splitter in so we have
                // one stream to capture the bitmap from, and one to display.  Ok, we
                // don't *have* to do it that way, but we are going to anyway.
                if (m_pinStill == null)
                {
                    IPin pRaw = null;
                    IPin pSmart = null;

                    // There is no still pin
                    m_VidControl = null;

                    // Add a splitter
                    IBaseFilter iSmartTee = (IBaseFilter)new SmartTee();

                    //IAMCameraControl接口可以获取摄像头信息
                    //IAMCameraControl cc = capFilter as IAMCameraControl;
                    //int min;
                    //int max;
                    //int step;
                    //int def;
                    //int current;
                    //CameraControlFlags fl;
                    //cc.GetRange(CameraControlProperty.Roll, out min, out max, out step, out def, out fl);
                    //cc.Get(CameraControlProperty.Roll, out current, out fl);
                    try
                    {
                        hr = m_FilterGraph.AddFilter(iSmartTee, "SmartTee");
                        DsError.ThrowExceptionForHR(hr);

                        // Find the find the capture pin from the video device and the
                        // input pin for the splitter, and connnect them
                        pRaw = DsFindPin.ByCategory(capFilter, PinCategory.Capture, 0);
                        pSmart = DsFindPin.ByDirection(iSmartTee, PinDirection.Input, 0);

                        hr = m_FilterGraph.Connect(pRaw, pSmart);
                        DsError.ThrowExceptionForHR(hr);

                        // Now set the capture and still pins (from the splitter)
                        m_pinStill = DsFindPin.ByName(iSmartTee, "Preview");
                        //pCaptureOut = DsFindPin.ByName(iSmartTee, "Capture");

                        // If any of the default config items are set, perform the config
                        // on the actual video device (rather than the splitter)
                        if (iHeight + iWidth + iBPP > 0)
                        {
                            SetConfigParms(pRaw, iWidth, iHeight, iBPP);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("【------------------------】");
                        Console.WriteLine("【VideoCaptureEx Error 001】" + ex.ToString());
                        Console.WriteLine("【------------------------】");
                    }
                    finally
                    {
                        if (pRaw != null)
                        {
                            Marshal.ReleaseComObject(pRaw);
                        }
                        if (pRaw != pSmart)
                        {
                            Marshal.ReleaseComObject(pSmart);
                        }
                        if (pRaw != iSmartTee)
                        {
                            Marshal.ReleaseComObject(iSmartTee);
                        }
                    }
                }
                else
                {
                    // Get a control pointer (used in Click())
                    m_VidControl = capFilter as IAMVideoControl;

                    pCaptureOut = DsFindPin.ByCategory(capFilter, PinCategory.Capture, 0);

                    // If any of the default config items are set
                    if (iHeight + iWidth + iBPP > 0)
                    {
                        SetConfigParms(m_pinStill, iWidth, iHeight, iBPP);
                    }
                }

                // Get the SampleGrabber interface
                sampGrabber = new SampleGrabber() as ISampleGrabber;

                // Configure the sample grabber
                IBaseFilter baseGrabFlt = sampGrabber as IBaseFilter;
                ConfigureSampleGrabber(sampGrabber);
                pSampleIn = DsFindPin.ByDirection(baseGrabFlt, PinDirection.Input, 0);

                // Get the default video renderer
                //IBaseFilter pRenderer = new VideoRendererDefault() as IBaseFilter;
                //hr = m_FilterGraph.AddFilter(pRenderer, "Renderer");
                //DsError.ThrowExceptionForHR(hr);

                //pRenderIn = DsFindPin.ByDirection(pRenderer, PinDirection.Input, 0);

                // Add the sample grabber to the graph
                hr = m_FilterGraph.AddFilter(baseGrabFlt, "Ds.NET Grabber");
                DsError.ThrowExceptionForHR(hr);

                if (m_VidControl == null)
                {
                    // Connect the Still pin to the sample grabber
                    hr = m_FilterGraph.Connect(m_pinStill, pSampleIn);
                    DsError.ThrowExceptionForHR(hr);

                    //// Connect the capture pin to the renderer
                    //hr = m_FilterGraph.Connect(pCaptureOut, pRenderIn);
                    //DsError.ThrowExceptionForHR(hr);
                }
                else
                {
                    // Connect the capture pin to the renderer
                    hr = m_FilterGraph.Connect(pCaptureOut, pRenderIn);
                    DsError.ThrowExceptionForHR(hr);

                    // Connect the Still pin to the sample grabber
                    hr = m_FilterGraph.Connect(m_pinStill, pSampleIn);
                    DsError.ThrowExceptionForHR(hr);
                }

                //// Learn the video properties
                //SaveSizeInfo(sampGrabber);
                //ConfigVideoWindow(hControl);

                // Start the graph
                IMediaControl mediaCtrl = m_FilterGraph as IMediaControl;
                hr = mediaCtrl.Run();
                DsError.ThrowExceptionForHR(hr);
            }
            catch (Exception ex)
            {
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【VideoCaptureEx Error 002】" + ex.ToString());
                Console.WriteLine("【------------------------】");
            }
            finally
            {
                if (sampGrabber != null)
                {
                    Marshal.ReleaseComObject(sampGrabber);
                    sampGrabber = null;
                }
                if (pCaptureOut != null)
                {
                    Marshal.ReleaseComObject(pCaptureOut);
                    pCaptureOut = null;
                }
                if (pRenderIn != null)
                {
                    Marshal.ReleaseComObject(pRenderIn);
                    pRenderIn = null;
                }
                if (pSampleIn != null)
                {
                    Marshal.ReleaseComObject(pSampleIn);
                    pSampleIn = null;
                }
            }
        }

        private void SaveSizeInfo(ISampleGrabber sampGrabber)
        {
            int hr;

            // Get the media type from the SampleGrabber
            AMMediaType media = new AMMediaType();

            hr = sampGrabber.GetConnectedMediaType(media);
            DsError.ThrowExceptionForHR(hr);

            if ((media.formatType != FormatType.VideoInfo) || (media.formatPtr == IntPtr.Zero))
            {
                throw new NotSupportedException("Unknown Grabber Media Format");
            }

            // Grab the size info
            VideoInfoHeader videoInfoHeader = (VideoInfoHeader)Marshal.PtrToStructure(media.formatPtr, typeof(VideoInfoHeader));
            m_videoWidth = videoInfoHeader.BmiHeader.Width;
            m_videoHeight = videoInfoHeader.BmiHeader.Height;
            m_stride = m_videoWidth * (videoInfoHeader.BmiHeader.BitCount / 8);

            DsUtils.FreeAMMediaType(media);
            media = null;
        }

        // Set the video window within the control specified by hControl
        private void ConfigVideoWindow(Control hControl)
        {
            int hr;

            IVideoWindow ivw = m_FilterGraph as IVideoWindow;

            // Set the parent
            hr = ivw.put_Owner(hControl.Handle);
            DsError.ThrowExceptionForHR(hr);

            // Turn off captions, etc
            hr = ivw.put_WindowStyle(WindowStyle.Child | WindowStyle.ClipChildren | WindowStyle.ClipSiblings);
            DsError.ThrowExceptionForHR(hr);

            // Yes, make it visible
            hr = ivw.put_Visible(OABool.True);
            DsError.ThrowExceptionForHR(hr);

            // Move to upper left corner
            Rectangle rc = hControl.ClientRectangle;
            hr = ivw.SetWindowPosition(0, 0, rc.Right, rc.Bottom);
            DsError.ThrowExceptionForHR(hr);
        }

        private void ConfigureSampleGrabber(ISampleGrabber sampGrabber)
        {
            int hr;
            AMMediaType media = new AMMediaType();

            // Set the media type to Video/RBG24
            media.majorType = MediaType.Video;
            media.subType = MediaSubType.RGB24;
            media.formatType = FormatType.VideoInfo;
            hr = sampGrabber.SetMediaType(media);
            DsError.ThrowExceptionForHR(hr);

            DsUtils.FreeAMMediaType(media);
            media = null;

            // Configure the samplegrabber
            hr = sampGrabber.SetCallback(this, 1);
            DsError.ThrowExceptionForHR(hr);
        }

        // Set the Framerate, and video size
        private void SetConfigParms(IPin pStill, int iWidth, int iHeight, short iBPP)
        {
            int hr;
            AMMediaType media;
            VideoInfoHeader v;

            IAMStreamConfig videoStreamConfig = pStill as IAMStreamConfig;

            // Get the existing format block
            hr = videoStreamConfig.GetFormat(out media);
            DsError.ThrowExceptionForHR(hr);

            try
            {
                // copy out the videoinfoheader
                v = new VideoInfoHeader();
                Marshal.PtrToStructure(media.formatPtr, v);

                // if overriding the width, set the width
                if (iWidth > 0)
                {
                    v.BmiHeader.Width = iWidth;
                }

                // if overriding the Height, set the Height
                if (iHeight > 0)
                {
                    v.BmiHeader.Height = iHeight;
                }

                // if overriding the bits per pixel
                if (iBPP > 0)
                {
                    v.BmiHeader.BitCount = iBPP;
                }

                // Copy the media structure back
                Marshal.StructureToPtr(v, media.formatPtr, false);

                // Set the new format
                hr = videoStreamConfig.SetFormat(media);
                DsError.ThrowExceptionForHR(hr);
            }
            finally
            {
                DsUtils.FreeAMMediaType(media);
                media = null;
            }
        }

        /// <summary> Shut down capture </summary>
        private void CloseInterfaces()
        {
            int hr;

            try
            {
                if (m_FilterGraph != null)
                {
                    IMediaControl mediaCtrl = m_FilterGraph as IMediaControl;

                    // Stop the graph
                    hr = mediaCtrl.Stop();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            if (m_FilterGraph != null)
            {
                Marshal.ReleaseComObject(m_FilterGraph);
                m_FilterGraph = null;
            }

            if (m_VidControl != null)
            {
                Marshal.ReleaseComObject(m_VidControl);
                m_VidControl = null;
            }

            if (m_pinStill != null)
            {
                Marshal.ReleaseComObject(m_pinStill);
                m_pinStill = null;
            }
        }



        /// <summary> sample callback, NOT USED. </summary>
        int ISampleGrabberCB.SampleCB(double SampleTime, IMediaSample pSample)
        {
            Marshal.ReleaseComObject(pSample);
            return 0;
        }

        /// <summary> buffer callback, COULD BE FROM FOREIGN THREAD. </summary>
        int ISampleGrabberCB.BufferCB(double SampleTime, IntPtr pBuffer, int BufferLen)
        {
            if (BufferCB != null && !PauseCapture)
            {
                if (imageData == null || imageData.Length != BufferLen)
                {
                    imageData = new byte[BufferLen];
                    yuvData = new byte[Width * Height + Width * Height / 2];
                    imageConverterInfo = ImageConverter.CreateConverter(AVPixelFormat.AV_PIX_FMT_BGR24, AVPixelFormat.AV_PIX_FMT_YUV420P, Width, Height, Width, Height);
                }
                Marshal.Copy(pBuffer, imageData, 0, BufferLen);
                if (isRotate)
                {
                    Common.FlipY(ref imageData, Width, Height);
                }
                if (isMirror)
                {
                    Common.FlipX(ref imageData, Width, Height);
                }
                if (!string.IsNullOrEmpty(m_rtmpUrl))
                {
                    AVPacket packet;
                    fixed (byte* pionter = imageData)
                    {
                        ImageConverter.Convert(imageConverterInfo, pionter, ref yuvData);
                    }
                    if (videoEncoder.EncodeVideoFrame(yuvData, frame, out packet))
                    {
                        try
                        {
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
                        catch
                        {
                        }
                    }
                }
                BufferCB(this, new BufferCBEventArgs() { PBuffer = pBuffer, BufferSize = BufferLen });
            }
            return 0;

            // Note that we depend on only being called once per call to Click.  Otherwise
            // a second call can overwrite the previous image.
            //////Debug.Assert(BufferLen == Math.Abs(m_stride) * m_videoHeight, "Incorrect buffer length");

            //////if (m_WantOne)
            //////{
            //////    m_WantOne = false;
            //////    Debug.Assert(m_ipBuffer != IntPtr.Zero, "Unitialized buffer");

            //////    // Save the buffer
            //////    CopyMemory(m_ipBuffer, pBuffer, BufferLen);

            //////    // Picture is ready.
            //////    m_PictureReady.Set();
            //////}

            //////return 0;
        }
    }
}

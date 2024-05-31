using FFmpeg.AutoGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CalligraphyAssistantMain.Code
{
    public unsafe class VideoDecoder
    {
        private unsafe AVFrame* yuv420Frame = null;
        private unsafe AVCodecContext* videoCodecContext = null;
        private unsafe AVCodec* videoCodec = null;
        //private AVBitStreamFilterContext* _h264bsfc;
        private int videoFrameIndex = 0;
        private byte[] rgb24Data = null;
        private object lockObj = new object();
        public bool IsInited { get; private set; }
        public byte[] RGB24Data { get { return rgb24Data; } }
        private ImageConverterInfo converterInfo;

        public unsafe bool InitAVCodec(int width, int height, int fps)
        {
            //编码器上下文
            videoCodecContext = InitVideoCodecContext(width, height, fps, string.Empty);
            converterInfo = ImageConverter.CreateConverter(AVPixelFormat.AV_PIX_FMT_YUV420P, AVPixelFormat.AV_PIX_FMT_BGR24, width, height);
            rgb24Data = new byte[converterInfo.LineSize[0] * height];
            //_h264bsfc = ffmpeg.av_bitstream_filter_init("h264_mp4toannexb");
            //av_log_set_callback_callback logCallback = (p0, level, format, vl) =>
            //          {
            //              //if (level > ffmpeg.av_log_get_level()) return;

            //              var lineSize = 1024;
            //              var lineBuffer = stackalloc byte[lineSize];
            //              var printPrefix = 1;
            //              ffmpeg.av_log_format_line(p0, level, format, vl, lineBuffer, lineSize, &printPrefix);
            //              var line = Marshal.PtrToStringAnsi((IntPtr)lineBuffer);
            //              Console.Write(line);
            //          };
            //ffmpeg.av_log_set_callback(logCallback); 
            //fs = new FileStream("d:\\" + DateTime.Now.Ticks + ".h264", FileMode.Create);
            IsInited = true;
            return true;
        }

        [HandleProcessCorruptedStateExceptions]
        public unsafe bool DecodeVideoFrame(byte[] buffer, int count)
        {
            try
            {
                if (!IsInited)
                {
                    return false;
                }
                AVPacket packet;
                ffmpeg.av_new_packet(&packet, count);
                packet.size = count;
                Marshal.Copy(buffer, 0, (IntPtr)packet.data, count);
                int ret = ffmpeg.avcodec_send_packet(videoCodecContext, &packet);
                if (ret != 0)
                {
                    Console.WriteLine(FFmpegHelper.av_strerror(ret));
                }
                ret = ffmpeg.avcodec_receive_frame(videoCodecContext, yuv420Frame);
                if (ret != 0)
                {
                    Console.WriteLine(FFmpegHelper.av_strerror(ret));
                }
                ffmpeg.av_packet_unref(&packet);
                //if (count == 32)
                //{
                //    videoCodecContext->extradata = (byte*)ffmpeg.av_mallocz((ulong)count);
                //    videoCodecContext->extradata_size = count;
                //    Marshal.Copy(buffer, 0, (IntPtr)videoCodecContext->extradata, count);
                //    return false;
                //}

                //AVPacket* packet = ffmpeg.av_packet_alloc();
                ////ffmpeg.av_init_packet(packet);
                //packet->size = count;
                //packet->data = (byte*)Marshal.AllocHGlobal(count);
                //Marshal.Copy(buffer, 0, (IntPtr)packet->data, count);
                ////int error = ffmpeg.av_bitstream_filter_filter(_h264bsfc, videoCodecContext, null, &packet->data, &packet->size, packet->data, packet->size, 0);

                //int ret = ffmpeg.avcodec_send_packet(videoCodecContext, packet);
                //if (ret != 0)
                //{
                //    Console.WriteLine(FFmpegHelper.av_strerror(ret));
                //    Marshal.FreeHGlobal((IntPtr)packet->data);
                //    ffmpeg.av_packet_unref(packet);
                //    return false;
                //}

                //ret = ffmpeg.avcodec_receive_frame(videoCodecContext, yuv420Frame);
                //if (ret != 0)
                //{
                //    Console.WriteLine(FFmpegHelper.av_strerror(ret));
                //}
                //else
                //{
                //    int a = 1;
                //    int b = a;
                //}
                //Marshal.FreeHGlobal((IntPtr)packet->data);
                //ffmpeg.av_packet_unref(packet);



                if (ret >= 0)
                {
                    AVFrame frame = *yuv420Frame;
                    ImageConverter.Convert(converterInfo, frame, ref rgb24Data);
                    //ffmpeg.sws_scale(converterInfo.Context, frame.data, frame.linesize, 0, frame.height, converterInfo.Data, converterInfo.LineSize);
                    //byte[] byteArr = new byte[converterInfo.LineSize[0] * frame.height];
                    //Marshal.Copy((IntPtr)converterInfo.Data[0], byteArr, 0, byteArr.Length);
                    //int width = frame.width;
                    //int height = frame.height;
                    //Common.window.Dispatcher.Invoke(() =>
                    //{
                    //    BitmapSource bitmap = BitmapSource.Create(width, height, 96, 96, PixelFormats.Bgr24, null, byteArr, converterInfo.LineSize[0]);
                    //    Common.window.Background = new ImageBrush(bitmap);
                    //});
                    //ImageConverter.Convert(converterInfo, frame.data,ref rgbArr);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Common.Trace("VideoDecoder DecodeVideoFrame Error:" + ex.ToString());
            }
            return false;
        }

        [HandleProcessCorruptedStateExceptions]
        public unsafe void CloseAVCodec()
        {
            lock (lockObj)
            {
                IsInited = false;  
                ReleaseAVFrame(yuv420Frame);
                ReleaseAVCodecContext(videoCodecContext);
                ReleaseImageConverterInfo(converterInfo);
                yuv420Frame = null;
                videoCodecContext = null;
                converterInfo = null;
                rgb24Data = null;
            }
        }

        private unsafe void ReleaseAVFrame(AVFrame* frame)
        {
            try
            {
                if (frame != null)
                {
                    ffmpeg.av_frame_unref(frame);
                }
            }
            catch
            {
            }
        }

        private unsafe void ReleaseAVCodecContext(AVCodecContext* avCodecCtx)
        {
            try
            {
                if (avCodecCtx != null)
                {
                    ffmpeg.avcodec_close(avCodecCtx);
                }
            }
            catch
            {
            }
        }

        private unsafe void ReleaseImageConverterInfo(ImageConverterInfo converterInfo)
        {
            try
            {
                if (converterInfo != null)
                {
                    ImageConverter.CloseConverter(converterInfo);
                }
            }
            catch
            {
            }
        }

        [HandleProcessCorruptedStateExceptions]
        private unsafe AVCodecContext* InitVideoCodecContext(int width, int height, int fps, string hwEncoder)
        {
            if (!string.IsNullOrEmpty(hwEncoder))
            {
                videoCodec = ffmpeg.avcodec_find_decoder_by_name(hwEncoder);
            }
            else
            {
                videoCodec = ffmpeg.avcodec_find_decoder(AVCodecID.AV_CODEC_ID_H264);
            }
            if (videoCodec == null)
            {
                return null;
            }
            yuv420Frame = ffmpeg.av_frame_alloc();
            yuv420Frame->format = (int)AVPixelFormat.AV_PIX_FMT_YUV420P;
            yuv420Frame->width = width;
            yuv420Frame->height = height;
            yuv420Frame->pts = 0;
            int ret = ffmpeg.av_frame_get_buffer(yuv420Frame, 1);
            if (ret != 0)
            {
                return null;
            }
            AVCodecContext* videoCtx = ffmpeg.avcodec_alloc_context3(videoCodec);
            //videoCtx->flags |= ffmpeg.AV_CODEC_FLAG_GLOBAL_HEADER;
            //videoCtx->flags |= ffmpeg.AV_CODEC_FLAG_TRUNCATED;
            //videoCtx->codec_id = videoCodec->id;

            //videoCtx->width = width;
            //videoCtx->height = height;
            //videoCtx->time_base.num = 1;
            //videoCtx->time_base.den = fps;
            //videoCtx->framerate.num = fps;
            //videoCtx->framerate.den = 1;
            //videoCtx->pix_fmt = AVPixelFormat.AV_PIX_FMT_YUV420P;
            ret = ffmpeg.avcodec_open2(videoCtx, videoCodec, null).ThrowExceptionIfError();
            if (ret != 0)
            {
                return null;
            }
            return videoCtx;
        }
    }
}

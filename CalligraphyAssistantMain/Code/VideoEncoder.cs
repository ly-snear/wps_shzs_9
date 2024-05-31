using FFmpeg.AutoGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    public class VideoEncoder
    {
        private unsafe AVFrame* yuv420Frame = null;
        private unsafe AVCodecContext* videoCodecContext = null;
        private unsafe AVFormatContext* rtmpAVFormatContext = null;
        private unsafe AVStream* rtmpVideoStream;
        private int videoFrameIndex = 0;
        private int width;
        private int height;
        private int fps;
        private string url;
        private object lockObj = new object();
        public bool IsInited { get; private set; }
        public string Url { get { return url; } }

        public unsafe int VideoStreamIndex
        {
            get
            {
                if (rtmpVideoStream != null)
                {
                    return rtmpVideoStream->index;
                }
                return -1;
            }
        }

        public unsafe bool InitAVCodec(int width, int height, int fps, string url)
        {
            this.width = width;
            this.height = height;
            this.fps = fps;
            this.url = url;
            //编码器上下文
            videoCodecContext = InitVideoCodecContext(width, height, fps, string.Empty);
            if (!string.IsNullOrEmpty(url))
            {
                bool result = InitAVFormatContext(url, "flv", fps, ref rtmpAVFormatContext, ref rtmpVideoStream);
                if (!result)
                {
                    return false;
                }
            }
            IsInited = true;
            return true;
        }

        public unsafe bool EncodeVideoFrame(byte[] buffer, long pts, out AVPacket packet)
        {
            packet = new AVPacket();
            try
            {

                if (!IsInited)
                {
                    return false;
                }
                Marshal.Copy(buffer, 0, (IntPtr)yuv420Frame->data[0], yuv420Frame->width * yuv420Frame->height);
                Marshal.Copy(buffer, yuv420Frame->width * yuv420Frame->height, (IntPtr)yuv420Frame->data[1], yuv420Frame->width * yuv420Frame->height / 4);
                Marshal.Copy(buffer, yuv420Frame->width * yuv420Frame->height + yuv420Frame->width * yuv420Frame->height / 4, (IntPtr)yuv420Frame->data[2], yuv420Frame->width * yuv420Frame->height / 4);
                int ret = 0;
                yuv420Frame->pts = pts;
                AVPacket _pkt;
                ffmpeg.av_init_packet(&_pkt);
                if (videoCodecContext->codec_type == AVMediaType.AVMEDIA_TYPE_VIDEO)
                {
                    if (videoFrameIndex == 0)
                    {
                        yuv420Frame->pict_type = AVPictureType.AV_PICTURE_TYPE_I;
                    }
                    ret = ffmpeg.avcodec_send_frame(videoCodecContext, yuv420Frame);
                    if (videoFrameIndex == 0)
                    {
                        yuv420Frame->pict_type = AVPictureType.AV_PICTURE_TYPE_NONE;
                    }
                }
                if (ret != 0)
                {
                    ffmpeg.av_packet_unref(&_pkt);
                    return false;
                }
                ret = ffmpeg.avcodec_receive_packet(videoCodecContext, &_pkt);
                if (ret != 0)
                {
                    ffmpeg.av_packet_unref(&_pkt);
                    if (ret == ffmpeg.AVERROR(ffmpeg.EAGAIN))//返回-11 不处理
                    {
                        return true;
                    }
                    return false;
                }
                _pkt.pts = pts;
                _pkt.dts = pts;
                _pkt.duration = 0;
                _pkt.pos = -1;
                //_pkt.stream_index = rtmpVideoStream->index;
                packet = _pkt;
                videoFrameIndex++;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【VideoEncoder Error 003】" + ex.ToString());
                Console.WriteLine("【------------------------】");
                return false;
            }
        }

        [HandleProcessCorruptedStateExceptions]
        public unsafe bool EncodeVideoFrame(AVFrame* frame, long pts, out AVPacket packet)
        {
            packet = new AVPacket();
            if (!IsInited)
            {
                return false;
            }
            int ret = 0;
            frame->pts = pts;
            AVPacket _pkt;
            ffmpeg.av_init_packet(&_pkt);
            if (videoCodecContext->codec_type == AVMediaType.AVMEDIA_TYPE_VIDEO)
            {
                if (videoFrameIndex == 0)
                {
                    frame->pict_type = AVPictureType.AV_PICTURE_TYPE_I;
                }
                ret = ffmpeg.avcodec_send_frame(videoCodecContext, frame);
                if (videoFrameIndex == 0)
                {
                    frame->pict_type = AVPictureType.AV_PICTURE_TYPE_NONE;
                }
            }
            if (ret != 0)
            {
                ffmpeg.av_packet_unref(&_pkt);
                return false;
            }
            ret = ffmpeg.avcodec_receive_packet(videoCodecContext, &_pkt);
            if (ret != 0)
            {
                ffmpeg.av_packet_unref(&_pkt);
                if (ret == ffmpeg.AVERROR(ffmpeg.EAGAIN))//返回-11 不处理
                {
                    return true;
                }
                return false;
            }
            _pkt.pts = pts;
            _pkt.dts = pts;
            _pkt.duration = 0;
            _pkt.pos = -1;
            //_pkt.stream_index = rtmpVideoStream->index;
            packet = _pkt;
            videoFrameIndex++;
            return true;
        }

        public unsafe bool WriteFrame(AVPacket packet)
        {
            if (packet.stream_index == this.VideoStreamIndex)
            {
                yuv420Frame->pts = packet.pts;
            }
            int ret = ffmpeg.av_write_frame(rtmpAVFormatContext, &packet);   //++Huey  
            ffmpeg.av_packet_unref(&packet);
            if (ret == 0)
            {
                return true;
            }
            return false;
        }

        public unsafe void CloseAVCodec()
        {
            lock (lockObj)
            {
                IsInited = false;
            }
            try
            {
                if (rtmpAVFormatContext != null)
                {
                    ffmpeg.av_write_trailer(rtmpAVFormatContext);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【VideoEncoder Error 001】" + ex.ToString());
                Console.WriteLine("【------------------------】");
            }
            ReleaseAVFrame(yuv420Frame);
            ReleaseAVCodecContext(videoCodecContext);
            yuv420Frame = null;
            videoCodecContext = null;
            if (rtmpAVFormatContext != null)
            {
                try
                {
                    AVFormatContext* _avFormatCtx = rtmpAVFormatContext;
                    ffmpeg.avformat_close_input(&_avFormatCtx);
                    rtmpAVFormatContext = null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("【------------------------】");
                    Console.WriteLine("【VideoEncoder Error 002】" + ex.ToString());
                    Console.WriteLine("【------------------------】");
                }
            }
        }

        public unsafe bool ReinitAVCodec()
        {
            if (IsInited)
            {
                CloseAVCodec();
            }
            return InitAVCodec(width, height, fps, url);
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

        private unsafe bool InitAVFormatContext(string url, string format, int fps, ref AVFormatContext* avFormatContext, ref AVStream* videoStream)
        {
            try
            {
                AVFormatContext* _ofmt_ctx2 = avFormatContext;
            int ret = ffmpeg.avformat_alloc_output_context2(&_ofmt_ctx2, null, format, null);  //++Huey
            avFormatContext = _ofmt_ctx2;
            //添加视频流
            videoStream = ffmpeg.avformat_new_stream(avFormatContext, null);
            if (videoStream == null)
            {
                return false;
            }
            videoStream->codec->codec_tag = 0;   //++Huey 
            ffmpeg.avcodec_parameters_from_context(videoStream->codecpar, videoCodecContext);
            videoStream->time_base.num = 1;
            videoStream->time_base.den = fps;
            ffmpeg.av_dump_format(avFormatContext, 0, url, 1);
            ret = ffmpeg.avio_open(&avFormatContext->pb, url, ffmpeg.AVIO_FLAG_WRITE);
            if (ret != 0)
            {
                return false;
            }
            ret = ffmpeg.avformat_write_header(avFormatContext, null);
            if (ret != 0)
            {
                return false;
            }
            return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("【------------------------】");
                Console.WriteLine("【VideoEncoder Error 004】" + ex.ToString());
                Console.WriteLine("【------------------------】");
                return false;
            }
        }

        private unsafe AVCodecContext* InitVideoCodecContext(int width, int height, int fps, string hwEncoder)
        {
            AVCodecContext* videoCtx = null;
            AVCodec* videoCodec = null;
            if (!string.IsNullOrEmpty(hwEncoder))
            {
                videoCodec = ffmpeg.avcodec_find_encoder_by_name(hwEncoder);
            }
            else
            {
                videoCodec = ffmpeg.avcodec_find_encoder(AVCodecID.AV_CODEC_ID_H264);
            }
            //AVCodec* videoCodec = ffmpeg.avcodec_find_encoder_by_name("h264_nvenc");
            //AVCodec* videoCodec = ffmpeg.avcodec_find_encoder_by_name("libx264");
            //AVCodec* videoCodec = ffmpeg.avcodec_find_encoder_by_name("h264_qsv");
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
            videoCtx = ffmpeg.avcodec_alloc_context3(videoCodec);
            videoCtx->codec_id = videoCodec->id;
            if (!string.IsNullOrEmpty(url))
            {
                videoCtx->flags |= ffmpeg.AV_CODEC_FLAG_GLOBAL_HEADER;
            }
            AVDictionary* param = null;
            switch (hwEncoder)
            {
                case ""://libx264
                    ffmpeg.av_dict_set(&param, "preset", "superfast", 0);  //编码形式修改
                    ffmpeg.av_dict_set(&param, "tune", "zerolatency", 0);  //实时编码     
                    ffmpeg.av_dict_set(&param, "profile", "baseline", 0);
                    break;
                case "h264_qsv"://h264_qsv
                    //ffmpeg.av_dict_set(&param, "preset", "veryfast", 0);  //编码形式修改
                    ffmpeg.av_dict_set(&param, "preset", "faster", 0);  //编码形式修改
                    ffmpeg.av_dict_set(&param, "tune", "zerolatency", 0);  //实时编码     
                    ffmpeg.av_dict_set(&param, "profile", "baseline", 0);
                    break;
                case "h264_nvenc"://h264_nvenc
                    ffmpeg.av_dict_set(&param, "preset", "medium", 0);  //编码形式修改
                    ffmpeg.av_dict_set(&param, "tune", "zerolatency", 0);  //实时编码  （未验证是否可用）    
                    ffmpeg.av_dict_set(&param, "profile", "baseline", 0);
                    break;
                case "h264_amf"://h264_amf
                    ffmpeg.av_dict_set(&param, "usage", "ultralowlatency", 0);  //实时编码（未验证是否可用）
                    ffmpeg.av_dict_set(&param, "tune", "zerolatency", 0);  //实时编码 （未验证是否可用）
                    ffmpeg.av_dict_set(&param, "quality", "speed", 0);  //实时编码（未验证是否可用）   
                    break;
                default:
                    break;
            }
            videoCtx->width = width;
            videoCtx->height = height;
            videoCtx->time_base.num = 1;
            videoCtx->time_base.den = fps;
            videoCtx->framerate.num = fps;
            videoCtx->framerate.den = 1;
            videoCtx->keyint_min = 20;//最小关键帧间隔
            //videoCtx->qmin = 30;   //调节清晰度和编码速度 //这个值调节编码后输出数据量越大输出数据量越小，越大编码速度越快，清晰度越差
            //videoCtx->qmax = 35;

            videoCtx->qmin = 25;   //调节清晰度和编码速度 //这个值调节编码后输出数据量越大输出数据量越小，越大编码速度越快，清晰度越差
            videoCtx->qmax = 30;

            //if (hwEncoder == "h264_qsv")
            //{
            //    videoCtx->qmin = 10;
            //    videoCtx->qmax = 15;
            //}
            videoCtx->gop_size = 25;   //编码一旦有gopsize很大的时候或者用了opencodec，有些播放器会等待I帧，无形中增加延迟。
            videoCtx->max_b_frames = 0;    //编码时如果有B帧会再解码时缓存很多帧数据才能解B帧，因此只留下I帧和P帧。
            //videoCtx->pix_fmt = AVPixelFormat.AV_PIX_FMT_YUV420P;
            videoCtx->pix_fmt = *videoCodec->pix_fmts;//qsv为nv12格式
            ret = ffmpeg.avcodec_open2(videoCtx, videoCodec, &param);
            if (ret != 0)
            {
                return null;
            }
            return videoCtx;
        }
    }
}

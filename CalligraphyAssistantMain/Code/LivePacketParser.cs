using FFmpeg.AutoGen;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    public unsafe class LivePacketParser : IAVPacketProvider
    {
        private AVFormatContext* _pFormatContext;
        private AVCodecContext* _pVideoCodecContext;
        private AVBitStreamFilterContext* _h264bsfc;
        private string url;
        private int probeSize;
        private bool isStarted = false;
        private bool isStoped = true;
        private VideoEncoder videoEncoder = null;
        public string Url { get { return url; } }
        public event EventHandler<NewAVPacketEventArgs> NewAVPacket;
        public LivePacketParser()
        {

        }

        public void Init(string url, int probeSize)
        {
            this.url = url;
            this.probeSize = probeSize;
        }

        public void Dispose()
        {
            if (isStoped)
            {
                try
                {
                    if (_pVideoCodecContext != null)
                    {
                        ffmpeg.avcodec_close(_pVideoCodecContext);
                        _pVideoCodecContext = null;
                    }
                }
                catch
                {
                }
                try
                {
                    var pFormatContext = _pFormatContext;
                    ffmpeg.avformat_close_input(&pFormatContext);
                    pFormatContext = null;
                    _pFormatContext = null;
                }
                catch
                {
                }
                try
                {
                    if (videoEncoder != null)
                    {
                        videoEncoder.CloseAVCodec();
                        videoEncoder = null;
                    }
                }
                catch
                {
                }
                //try
                //{
                //    if (_h264bsfc != null)
                //    {
                //        ffmpeg.av_bitstream_filter_close(_h264bsfc);
                //        _h264bsfc = null;
                //    }
                //}
                //catch
                //{
                //}
            }
        }

        public void Start()
        {
            if (isStarted)
            {
                return;
            }
            isStarted = true;
            Thread thread = new Thread(ParseLive);
            thread.IsBackground = true;
            thread.Start();
        }

        public void Stop()
        {
            isStarted = false;
        }

        private void ParseLive()
        {
            while (isStarted)
            {
                isStoped = false;
                AVFormatContext* _pFormatContext = ffmpeg.avformat_alloc_context();
                var pFormatContext = _pFormatContext;
                try
                {
                    AVDictionary* options = null;
                    //自编译ffmpeg增加的功能 读写等待超时
                    ffmpeg.av_dict_set(&options, "rtmp_timeout", "5", 0);
                    ffmpeg.avformat_open_input(&pFormatContext, url, null, &options).ThrowExceptionIfError();
                    _pFormatContext->probesize = probeSize;
                    _pFormatContext->flags |= ffmpeg.AVFMT_FLAG_NOBUFFER;
                    ffmpeg.avformat_find_stream_info(_pFormatContext, null).ThrowExceptionIfError();

                }
                catch
                {
                    goto Next;
                }
                // find the first video stream
                AVStream* pVideoStream = null;
                for (var i = 0; i < _pFormatContext->nb_streams; i++)
                {
                    if (_pFormatContext->streams[i]->codec->codec_type == AVMediaType.AVMEDIA_TYPE_VIDEO)
                    {
                        pVideoStream = _pFormatContext->streams[i];
                        break;
                    }
                }
                if (pVideoStream == null)
                {
                    if (pFormatContext != null)
                    {
                        try
                        {
                            ffmpeg.avformat_close_input(&pFormatContext);
                            pFormatContext = null;
                            _pFormatContext = null;
                        }
                        catch
                        {
                        }
                    }
                    goto Next;
                }
                int _videoStreamIndex = pVideoStream->index;
                _pVideoCodecContext = pVideoStream->codec;
                var videoCodecId = _pVideoCodecContext->codec_id;
                var pVideoCodec = ffmpeg.avcodec_find_decoder(videoCodecId);
                if (pVideoCodec == null)
                {
                    Common.Trace("LivePacketParser ParseLive Error:Unsupported codec.");
                    goto End;
                }
                ffmpeg.avcodec_open2(_pVideoCodecContext, pVideoCodec, null).ThrowExceptionIfError();
                Size frameSize = new Size(_pVideoCodecContext->width, _pVideoCodecContext->height);

                AVPacket* _pPacket = ffmpeg.av_packet_alloc();
                AVFrame* _pFrame = ffmpeg.av_frame_alloc();
                //_h264bsfc = ffmpeg.av_bitstream_filter_init("h264_mp4toannexb");
                int error;
                videoEncoder = new VideoEncoder();
                videoEncoder.InitAVCodec(frameSize.Width, frameSize.Height, 15, url);
                int frame = 1;
                while (isStarted)
                {
                    do
                    {
                        try
                        {
                            if (!isStarted)
                            {
                                goto Next;
                            }
                            do
                            {
                                error = ffmpeg.av_read_frame(_pFormatContext, _pPacket);
                                if (error == ffmpeg.AVERROR_EOF)
                                {
                                    if (!isStarted)
                                    {
                                        goto Next;
                                    }
                                    continue;
                                }
                                error.ThrowExceptionIfError();
                            } while (!(_pPacket->stream_index == _videoStreamIndex));
                            if (_pPacket->stream_index == _videoStreamIndex)
                            {
                                //error = ffmpeg.av_bitstream_filter_filter(_h264bsfc, _pVideoCodecContext, null, &_pPacket->data, &_pPacket->size, _pPacket->data, _pPacket->size, 0);
                                error = ffmpeg.avcodec_send_packet(_pVideoCodecContext, _pPacket);
                                error = ffmpeg.avcodec_receive_frame(_pVideoCodecContext, _pFrame);
                                AVPacket packet;
                                videoEncoder.EncodeVideoFrame(_pFrame, frame, out packet);
                                if (NewAVPacket != null)
                                {
                                    NewAVPacket(this, new NewAVPacketEventArgs()
                                    {
                                        Frame = frame,
                                        Packet = packet,
                                        Size = new System.Windows.Size(frameSize.Width, frameSize.Height)
                                    });
                                }
                                ffmpeg.av_packet_unref(&packet);
                            }
                        }
                        finally
                        {
                            ffmpeg.av_packet_unref(_pPacket);
                        }

                    } while (error == ffmpeg.AVERROR(ffmpeg.EAGAIN));
                }

            Next:
                isStoped = true;
                Dispose();
                for (int i = 0; i < 10; i++)
                {
                    if (!isStarted)
                    {
                        goto End;
                    }
                    Thread.Sleep(100);
                };
            }
        End:
            isStoped = true;
            Dispose();
        }
    }
}

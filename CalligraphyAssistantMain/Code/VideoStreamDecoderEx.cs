using FFmpeg.AutoGen;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace CalligraphyAssistantMain.Code
{
    public sealed unsafe class VideoStreamDecoderEx : IDisposable
    {
        private AVCodecContext* _pVideoCodecContext;
        private AVCodecContext* _pAudioCodecContext;
        private AVFormatContext* _pFormatContext;
        private int _videoStreamIndex = -1;
        private int _audioStreamIndex = -1;
        private AVFrame* _pFrame;
        private AVPacket* _pPacket;
        private bool enableAudio = true;
        public bool HasAudio { get; set; }
        public bool EnableAudio { get { return enableAudio; } }

        public VideoStreamDecoderEx(string url, int probeSize, bool enableAudio = true)
        {
            this.enableAudio = enableAudio;
            _pFormatContext = ffmpeg.avformat_alloc_context();
            var pFormatContext = _pFormatContext; 
            AVDictionary* options = null; 
            //自编译ffmpeg增加的功能 读写等待超时
            ffmpeg.av_dict_set(&options, "rtmp_timeout", "5", 0);
            ffmpeg.avformat_open_input(&pFormatContext, url, null, &options).ThrowExceptionIfError();
            _pFormatContext->probesize = probeSize;
            _pFormatContext->flags |= ffmpeg.AVFMT_FLAG_NOBUFFER;
            ffmpeg.avformat_find_stream_info(_pFormatContext, null).ThrowExceptionIfError();

            // find the first video stream
            AVStream* pVideoStream = null;
            AVStream* pAudioStream = null;
            for (var i = 0; i < _pFormatContext->nb_streams; i++)
                if (_pFormatContext->streams[i]->codec->codec_type == AVMediaType.AVMEDIA_TYPE_VIDEO)
                {
                    pVideoStream = _pFormatContext->streams[i];
                    break;
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
                throw new InvalidOperationException("Could not found video stream.");
            }

            _videoStreamIndex = pVideoStream->index;
            _pVideoCodecContext = pVideoStream->codec;

            var videoCodecId = _pVideoCodecContext->codec_id;
            var pVideoCodec = ffmpeg.avcodec_find_decoder(videoCodecId);
            if (pVideoCodec == null) throw new InvalidOperationException("Unsupported codec.");
            ffmpeg.avcodec_open2(_pVideoCodecContext, pVideoCodec, null).ThrowExceptionIfError();

            VideoCodecName = ffmpeg.avcodec_get_name(videoCodecId);
            FrameSize = new Size(_pVideoCodecContext->width, _pVideoCodecContext->height);
            PixelFormat = _pVideoCodecContext->pix_fmt;

            //if (enableAudio)
            //{
            for (var i = 0; i < _pFormatContext->nb_streams; i++)
            {
                if (_pFormatContext->streams[i]->codec->codec_type == AVMediaType.AVMEDIA_TYPE_AUDIO)
                {
                    pAudioStream = _pFormatContext->streams[i];
                    break;
                }
            }
            if (pAudioStream != null)
            {
                HasAudio = true;
                _audioStreamIndex = pAudioStream->index;
                _pAudioCodecContext = pAudioStream->codec;
                var audioCodecId = _pAudioCodecContext->codec_id;
                var pAudioCodec = ffmpeg.avcodec_find_decoder(audioCodecId);
                //AudioPlaneSize = ffmpeg.av_get_bytes_per_sample(_pAudioCodecContext->sample_fmt);
                //AudioChannels = _pAudioCodecContext->channels;
                //AudioSampleRate = _pAudioCodecContext->sample_rate;
                //AudioChannelLayout = _pAudioCodecContext->channel_layout;

                //这里从_pAudioCodecContext获取的参数不准确 暂时设置为固定 FLTP、立体声、48000 
                AudioPlaneSize = ffmpeg.av_get_bytes_per_sample(AVSampleFormat.AV_SAMPLE_FMT_FLTP);
                AudioChannels = 2;
                AudioSampleRate = 48000;
                AudioChannelLayout = ffmpeg.AV_CH_LAYOUT_STEREO; 

                AudioBitsPerSample = 16;
                ffmpeg.avcodec_open2(_pAudioCodecContext, pAudioCodec, null).ThrowExceptionIfError();
            }
            //}
            _pPacket = ffmpeg.av_packet_alloc();
            _pFrame = ffmpeg.av_frame_alloc();
        }

        public string VideoCodecName { get; private set; }
        public Size FrameSize { get; private set; }
        public AVPixelFormat PixelFormat { get; private set; }

        public int AudioPlaneSize { get; set; }

        public int AudioChannels { get; set; }

        public ulong AudioChannelLayout { get; set; }

        public int AudioSampleRate { get; set; }

        public int AudioBitsPerSample { get; set; }

        public void Dispose()
        {
            try
            {
                if (_pFrame != null)
                {
                    ffmpeg.av_frame_unref(_pFrame);
                }
            }
            catch
            {
            }
            try
            {
                if (_pFrame != null)
                {
                    ffmpeg.av_free(_pFrame);
                    _pFrame = null;
                }
            }
            catch
            {
            }
            try
            {
                if (_pPacket != null)
                {
                    ffmpeg.av_packet_unref(_pPacket);
                }
            }
            catch
            {
            }
            try
            {
                if (_pPacket != null)
                {
                    ffmpeg.av_free(_pPacket);
                    _pPacket = null;
                }
            }
            catch
            {
            }
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
                if (_pAudioCodecContext != null)
                {
                    ffmpeg.avcodec_close(_pAudioCodecContext);
                    _pAudioCodecContext = null;
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

        }

        public bool TryDecodeNextFrame(out AVFrame frame, out AVMediaType type)
        {
            type = AVMediaType.AVMEDIA_TYPE_UNKNOWN;
            ffmpeg.av_frame_unref(_pFrame);
            int error;
            int currentIndex = -1;
            do
            {
                try
                {
                    do
                    {
                        error = ffmpeg.av_read_frame(_pFormatContext, _pPacket);
                        if (error == ffmpeg.AVERROR_EOF)
                        {
                            frame = *_pFrame;
                            return false;
                        }

                        error.ThrowExceptionIfError();
                    } while (!(_pPacket->stream_index == _videoStreamIndex || _pPacket->stream_index == _audioStreamIndex));
                    if (_pPacket->stream_index == _videoStreamIndex)
                    {
                        ffmpeg.avcodec_send_packet(_pVideoCodecContext, _pPacket).ThrowExceptionIfError();
                    }
                    //if (enableAudio)
                    //{
                    if (_pPacket->stream_index == _audioStreamIndex)
                    {
                        ffmpeg.avcodec_send_packet(_pAudioCodecContext, _pPacket).ThrowExceptionIfError();
                    }
                    //}
                    currentIndex = _pPacket->stream_index;
                }
                finally
                {
                    ffmpeg.av_packet_unref(_pPacket);
                }
                if (currentIndex == _videoStreamIndex)
                {
                    error = ffmpeg.avcodec_receive_frame(_pVideoCodecContext, _pFrame);
                    type = AVMediaType.AVMEDIA_TYPE_VIDEO;
                }
                //if (enableAudio)
                //{
                if (currentIndex == _audioStreamIndex)
                {
                    error = ffmpeg.avcodec_receive_frame(_pAudioCodecContext, _pFrame);
                    type = AVMediaType.AVMEDIA_TYPE_AUDIO;
                }
                //}
            } while (error == ffmpeg.AVERROR(ffmpeg.EAGAIN));

            error.ThrowExceptionIfError();
            //Console.WriteLine(_pVideoCodecContext->frame_number);
            frame = *_pFrame;
            if (type == AVMediaType.AVMEDIA_TYPE_VIDEO)
            {
                frame.display_picture_number = _pVideoCodecContext->frame_number;
            }
            if (!enableAudio && type == AVMediaType.AVMEDIA_TYPE_AUDIO)
            {
                type = AVMediaType.AVMEDIA_TYPE_UNKNOWN;
            }
            return true;
        }

        public Dictionary<string, string> GetContextInfo()
        {
            AVDictionaryEntry* tag = null;
            var result = new Dictionary<string, string>();
            while ((tag = ffmpeg.av_dict_get(_pFormatContext->metadata, "", tag, ffmpeg.AV_DICT_IGNORE_SUFFIX)) != null)
            {
                var key = Marshal.PtrToStringAnsi((IntPtr)tag->key);
                var value = Marshal.PtrToStringAnsi((IntPtr)tag->value);
                result.Add(key, value);
            }

            return result;
        }
    }
}
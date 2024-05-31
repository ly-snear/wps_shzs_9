using FFmpeg.AutoGen;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.ExceptionServices;

namespace CalligraphyAssistantMain.Code
{
    public sealed unsafe class VideoFrameConverterEx : IDisposable
    {
        private IntPtr _convertedFrameBufferPtr;
        private Size _destinationSize;
        private byte_ptrArray4 _dstData;
        private int_array4 _dstLinesize;
        private SwsContext* _pConvertContext;
        private SwrContext* swr;
        private int audioErrorTimes = 0;

        public VideoFrameConverterEx(Size sourceSize, AVPixelFormat sourcePixelFormat,
            Size destinationSize, AVPixelFormat destinationPixelFormat)
        {
            _destinationSize = destinationSize;
            audioErrorTimes = 0;
            _pConvertContext = ffmpeg.sws_getContext(sourceSize.Width, sourceSize.Height, sourcePixelFormat,
            destinationSize.Width,
            destinationSize.Height, destinationPixelFormat,
            ffmpeg.SWS_FAST_BILINEAR, null, null, null);
            if (_pConvertContext == null) throw new ApplicationException("Could not initialize the conversion context.");

            var convertedFrameBufferSize = ffmpeg.av_image_get_buffer_size(destinationPixelFormat, destinationSize.Width, destinationSize.Height, 1);
            _convertedFrameBufferPtr = Marshal.AllocHGlobal(convertedFrameBufferSize);
            _dstData = new byte_ptrArray4();
            _dstLinesize = new int_array4();

            ffmpeg.av_image_fill_arrays(ref _dstData, ref _dstLinesize, (byte*)_convertedFrameBufferPtr, destinationPixelFormat, destinationSize.Width, destinationSize.Height, 1);
        }

        public VideoFrameConverterEx(Size sourceSize, AVPixelFormat sourcePixelFormat,
            Size destinationSize, AVPixelFormat destinationPixelFormat, long channelLayout, int sampleRate)
        {
            _destinationSize = destinationSize;
            audioErrorTimes = 0;
            _pConvertContext = ffmpeg.sws_getContext(sourceSize.Width, sourceSize.Height, sourcePixelFormat,
            destinationSize.Width,
            destinationSize.Height, destinationPixelFormat,
            ffmpeg.SWS_FAST_BILINEAR, null, null, null);
            if (_pConvertContext == null) throw new ApplicationException("Could not initialize the conversion context.");

            var convertedFrameBufferSize = ffmpeg.av_image_get_buffer_size(destinationPixelFormat, destinationSize.Width, destinationSize.Height, 1);
            _convertedFrameBufferPtr = Marshal.AllocHGlobal(convertedFrameBufferSize);
            _dstData = new byte_ptrArray4();
            _dstLinesize = new int_array4();

            ffmpeg.av_image_fill_arrays(ref _dstData, ref _dstLinesize, (byte*)_convertedFrameBufferPtr, destinationPixelFormat, destinationSize.Width, destinationSize.Height, 1);

            swr = ffmpeg.swr_alloc();
            ffmpeg.av_opt_set_int(swr, "in_channel_layout", channelLayout, 0);
            ffmpeg.av_opt_set_int(swr, "out_channel_layout", channelLayout, 0);
            ffmpeg.av_opt_set_int(swr, "in_sample_rate", sampleRate, 0);
            ffmpeg.av_opt_set_int(swr, "out_sample_rate", sampleRate, 0);
            ffmpeg.av_opt_set_sample_fmt(swr, "in_sample_fmt", AVSampleFormat.AV_SAMPLE_FMT_FLTP, 0);
            ffmpeg.av_opt_set_sample_fmt(swr, "out_sample_fmt", AVSampleFormat.AV_SAMPLE_FMT_S16, 0);
            ffmpeg.swr_init(swr);

            // In your decoder loop, after decoding an audio frame:
            //AVFrame* audioFrame = ...;
            //int16_t* outputBuffer = ...;
            //swr_convert(&outputBuffer, audioFrame->nb_samples, audioFrame->extended_data, audioFrame->nb_samples);

        }

        public void Dispose()
        {
            try
            {
                if (_convertedFrameBufferPtr != null &&
                    _convertedFrameBufferPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(_convertedFrameBufferPtr);
                    _convertedFrameBufferPtr = IntPtr.Zero;
                }
            }
            catch
            {
            }
            try
            {
                if (_pConvertContext != null)
                {
                    ffmpeg.sws_freeContext(_pConvertContext);
                    _pConvertContext = null;
                }
            }
            catch
            {
            }
        }

        public AVFrame Convert(VideoStreamDecoderEx vsd, AVFrame sourceFrame, AVMediaType type)
        {
            if (type == AVMediaType.AVMEDIA_TYPE_VIDEO)
            {
                ffmpeg.sws_scale(_pConvertContext, sourceFrame.data, sourceFrame.linesize, 0, sourceFrame.height, _dstData, _dstLinesize);

                var data = new byte_ptrArray8();
                data.UpdateFrom(_dstData);
                var linesize = new int_array8();
                linesize.UpdateFrom(_dstLinesize);

                return new AVFrame
                {
                    data = data,
                    linesize = linesize,
                    width = _destinationSize.Width,
                    height = _destinationSize.Height
                };
            }
            if (type == AVMediaType.AVMEDIA_TYPE_AUDIO)
            {
                byte* tempP;
                int out_samples = (int)ffmpeg.av_rescale_rnd(ffmpeg.swr_get_delay(swr, vsd.AudioSampleRate) +
                    sourceFrame.nb_samples, sourceFrame.sample_rate, vsd.AudioSampleRate, AVRounding.AV_ROUND_UP);
                ffmpeg.av_samples_alloc(&tempP, null, sourceFrame.channels, out_samples, AVSampleFormat.AV_SAMPLE_FMT_S16, 0);
                int current_out_samples = ffmpeg.swr_get_out_samples(swr, out_samples);
                byte[] byteArr = null;
                IntPtr intPtr = new IntPtr(tempP);
                int output;
                if (current_out_samples >= out_samples)
                {
                    output = ffmpeg.swr_convert(swr, &tempP, out_samples, sourceFrame.extended_data, sourceFrame.nb_samples);
                    byteArr = new byte[output * vsd.AudioPlaneSize];
                }
                else
                {
                    output = ffmpeg.swr_convert(swr, &tempP, current_out_samples, sourceFrame.extended_data, current_out_samples);
                    byteArr = new byte[output * vsd.AudioPlaneSize];
                }
                try
                {
                    Marshal.Copy(intPtr, byteArr, 0, byteArr.Length);
                }
                catch
                {
                }
                ffmpeg.av_free(tempP);
                int size = byteArr.Length;
                var data = new byte_ptrArray8();
                var linesize = new int_array8();
                //fixed (byte* pointer = byteArr)
                //{
                //    data[0] = pointer;
                //}
                fixed (byte* pointer = byteArr)
                {
                    //data[0] = pointer;
                    data.UpdateFrom(new byte*[] { pointer });
                }

                linesize[0] = size;
                return new AVFrame
                {
                    data = data,
                    linesize = linesize
                };
            }
            return new AVFrame();
        }

        [HandleProcessCorruptedStateExceptions]
        public AVFrameEx ConvertEx(VideoStreamDecoderEx vsd, AVFrame sourceFrame, AVMediaType type)
        {
            if (type == AVMediaType.AVMEDIA_TYPE_VIDEO)
            {
                ffmpeg.sws_scale(_pConvertContext, sourceFrame.data, sourceFrame.linesize, 0, sourceFrame.height, _dstData, _dstLinesize);
                byte[] byteArr = new byte[_dstLinesize[0] * sourceFrame.height];
                Marshal.Copy((IntPtr)_dstData[0], byteArr, 0, byteArr.Length);
                return new AVFrameEx
                {
                    data = byteArr,
                    linesize = byteArr.Length,
                    width = _destinationSize.Width,
                    height = _destinationSize.Height
                };
            }
            if (type == AVMediaType.AVMEDIA_TYPE_AUDIO)
            {
                if (audioErrorTimes>10)
                {
                    type = AVMediaType.AVMEDIA_TYPE_UNKNOWN;
                    return new AVFrameEx();
                }
                byte* tempP;
                int out_samples = (int)ffmpeg.av_rescale_rnd(ffmpeg.swr_get_delay(swr, vsd.AudioSampleRate) +
                    sourceFrame.nb_samples, sourceFrame.sample_rate, vsd.AudioSampleRate, AVRounding.AV_ROUND_UP);
                ffmpeg.av_samples_alloc(&tempP, null, sourceFrame.channels, out_samples, AVSampleFormat.AV_SAMPLE_FMT_S16, 0);
                int current_out_samples = ffmpeg.swr_get_out_samples(swr, out_samples);
                byte[] byteArr = null;
                IntPtr intPtr = new IntPtr(tempP);
                int output;
                try
                {
                    if (current_out_samples >= out_samples)
                    {
                        output = ffmpeg.swr_convert(swr, &tempP, out_samples, sourceFrame.extended_data, sourceFrame.nb_samples);
                        byteArr = new byte[output * vsd.AudioPlaneSize];
                    }
                    else
                    {
                        output = ffmpeg.swr_convert(swr, &tempP, current_out_samples, sourceFrame.extended_data, current_out_samples);
                        byteArr = new byte[output * vsd.AudioPlaneSize];
                    }
                    try
                    {
                        Marshal.Copy(intPtr, byteArr, 0, byteArr.Length);
                    }
                    catch
                    {
                        type = AVMediaType.AVMEDIA_TYPE_UNKNOWN;
                    }
                    finally
                    {
                        ffmpeg.av_free(tempP);
                    }
                    return new AVFrameEx
                    {
                        data = byteArr,
                        linesize = byteArr.Length
                    };
                }
                catch (Exception ex)
                {
                    ffmpeg.av_free(tempP);
                    type = AVMediaType.AVMEDIA_TYPE_UNKNOWN;
                    audioErrorTimes++;
                    Common.Trace("VideoFrameConverterEx ConvertEx Error:" + ex.Message + "\r\nAudio Error Times:+" + audioErrorTimes);
                }
            }
            return new AVFrameEx();
        }

    }
}
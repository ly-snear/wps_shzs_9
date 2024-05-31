using FFmpeg.AutoGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    public static class ImageConverter
    {
        public unsafe static ImageConverterInfo CreateConverter(AVPixelFormat sourceFormat, AVPixelFormat targetFormat, int width, int height)
        {
            byte_ptrArray4 _dstData = new byte_ptrArray4();
            int_array4 _dstLinesize = new int_array4();
            ffmpeg.av_image_alloc(ref _dstData, ref _dstLinesize, width, height, targetFormat, 1);
            SwsContext* _pConvertContext = ffmpeg.sws_getContext(
                width, height, sourceFormat,
                width, height, targetFormat,
                ffmpeg.SWS_BICUBIC, null, null, null);
            return new ImageConverterInfo()
            {
                Data = _dstData,
                LineSize = _dstLinesize,
                Context = _pConvertContext,
                TargetWidth = width,
                TargetHeight = height,
                SourceWidth = width,
                SourceHeight = height,
                SourceFormat = sourceFormat,
                TargetFormat = targetFormat
            };
        }

        public unsafe static ImageConverterInfo CreateConverter(AVPixelFormat sourceFormat, AVPixelFormat targetFormat, int sourceWidth, int sourceHeight, int targetWidth, int targetHeight)
        {
            byte_ptrArray4 _dstData = new byte_ptrArray4();
            int_array4 _dstLinesize = new int_array4();
            ffmpeg.av_image_alloc(ref _dstData, ref _dstLinesize, targetWidth, targetHeight, targetFormat, 1);
            SwsContext* _pConvertContext = ffmpeg.sws_getContext(
                sourceWidth, sourceHeight, sourceFormat,
                targetWidth, targetHeight, targetFormat,
                ffmpeg.SWS_BICUBIC, null, null, null);
            return new ImageConverterInfo()
            {
                Data = _dstData,
                LineSize = _dstLinesize,
                Context = _pConvertContext,
                TargetWidth = targetWidth,
                TargetHeight = targetHeight,
                SourceWidth = sourceWidth,
                SourceHeight = sourceHeight,
                SourceFormat = sourceFormat,
                TargetFormat = targetFormat
            };
        }

        public unsafe static void Convert(ImageConverterInfo info, AVFrame frame, ref byte[] ouputBuffer)
        {
            if (info.SourceFormat == AVPixelFormat.AV_PIX_FMT_RGB24 || info.SourceFormat == AVPixelFormat.AV_PIX_FMT_BGR24)
            {
                if (info.TargetFormat == AVPixelFormat.AV_PIX_FMT_RGB24 || info.TargetFormat == AVPixelFormat.AV_PIX_FMT_BGR24)
                {
                    ffmpeg.sws_scale(info.Context, frame.data, frame.linesize, 0, info.SourceHeight, info.Data, info.LineSize.ToArray());
                    Marshal.Copy(new IntPtr(info.Data[0]), ouputBuffer, 0, info.TargetWidth * info.TargetHeight * 3);
                }
                else
                {
                    ffmpeg.sws_scale(info.Context, frame.data, frame.linesize, 0, info.SourceHeight, info.Data, info.LineSize.ToArray());
                    Marshal.Copy(new IntPtr(info.Data[0]), ouputBuffer, 0, info.TargetWidth * info.TargetHeight);
                    Marshal.Copy(new IntPtr(info.Data[1]), ouputBuffer, info.TargetWidth * info.TargetHeight, info.TargetWidth * info.TargetHeight / 4);
                    Marshal.Copy(new IntPtr(info.Data[2]), ouputBuffer, info.TargetWidth * info.TargetHeight + info.TargetWidth * info.TargetHeight / 4, info.TargetWidth * info.TargetHeight / 4);
                }
            }
            else if (info.SourceFormat == AVPixelFormat.AV_PIX_FMT_YUV420P)
            {
                ffmpeg.sws_scale(info.Context, frame.data, frame.linesize, 0, info.SourceHeight, info.Data, info.LineSize.ToArray());
                Marshal.Copy(new IntPtr(info.Data[0]), ouputBuffer, 0, info.TargetWidth * info.TargetHeight * 3);
            }
        }

        public unsafe static void Convert(ImageConverterInfo info, byte* inputBuffer, ref byte[] ouputBuffer)
        {
            if (info.SourceFormat == AVPixelFormat.AV_PIX_FMT_RGB24 || info.SourceFormat == AVPixelFormat.AV_PIX_FMT_BGR24)
            {
                if (info.TargetFormat == AVPixelFormat.AV_PIX_FMT_RGB24 || info.TargetFormat == AVPixelFormat.AV_PIX_FMT_BGR24)
                {
                    ffmpeg.sws_scale(info.Context, new byte*[] { inputBuffer }, new int[] { info.SourceWidth * 3 }, 0, info.SourceHeight, info.Data, info.LineSize.ToArray());
                    Marshal.Copy(new IntPtr(info.Data[0]), ouputBuffer, 0, info.TargetWidth * info.TargetHeight * 3);
                }
                else
                {
                    ffmpeg.sws_scale(info.Context, new byte*[] { inputBuffer }, new int[] { info.SourceWidth * 3 }, 0, info.SourceHeight, info.Data, info.LineSize.ToArray());
                    Marshal.Copy(new IntPtr(info.Data[0]), ouputBuffer, 0, info.TargetWidth * info.TargetHeight);
                    Marshal.Copy(new IntPtr(info.Data[1]), ouputBuffer, info.TargetWidth * info.TargetHeight, info.TargetWidth * info.TargetHeight / 4);
                    Marshal.Copy(new IntPtr(info.Data[2]), ouputBuffer, info.TargetWidth * info.TargetHeight + info.TargetWidth * info.TargetHeight / 4, info.TargetWidth * info.TargetHeight / 4);
                }
            }
            else if (info.SourceFormat == AVPixelFormat.AV_PIX_FMT_YUV420P)
            {
                Marshal.Copy(new IntPtr(info.Data[0]), ouputBuffer, 0, info.TargetWidth * info.TargetHeight * 3);
            }
        }

        public unsafe static void Convert(ImageConverterInfo info, byte[] inputBuffer, ref byte[] ouputBuffer)
        {
            fixed (byte* potiner = inputBuffer)
            {
                Convert(info, potiner, ref ouputBuffer);
            }
        }

        public unsafe static void CloseConverter(ImageConverterInfo info)
        {
            if (info != null)
            {
                try
                {
                    ffmpeg.sws_freeContext(info.Context);
                }
                catch
                {
                }
                try
                {
                    ffmpeg.av_free(info.Data[0]);
                }
                catch
                {
                }
            }
        }
    }
}

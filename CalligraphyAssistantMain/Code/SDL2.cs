using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    public class SDL2
    { 
        private const string nativeLibName = "SDL2";

        public const uint SDL_INIT_AUDIO = 16;
        public const uint SDL_INIT_EVENTS = 16384;
        public const uint SDL_INIT_EVERYTHING = 62001;
        public const uint SDL_INIT_GAMECONTROLLER = 8192;
        public const uint SDL_INIT_HAPTIC = 4096;
        public const uint SDL_INIT_JOYSTICK = 512;
        public const uint SDL_INIT_NOPARACHUTE = 1048576;
        public const uint SDL_INIT_SENSOR = 32768;
        public const uint SDL_INIT_TIMER = 1;
        public const uint SDL_INIT_VIDEO = 32;

        public const ushort AUDIO_U8 = 0x0008;
        public const ushort AUDIO_S8 = 0x8008;
        public const ushort AUDIO_U16LSB = 0x0010;
        public const ushort AUDIO_S16LSB = 0x8010;
        public const ushort AUDIO_U16MSB = 0x1010;
        public const ushort AUDIO_S16MSB = 0x9010;
        public const ushort AUDIO_U16 = AUDIO_U16LSB;
        public const ushort AUDIO_S16 = AUDIO_S16LSB;
        public const ushort AUDIO_S32LSB = 0x8020;
        public const ushort AUDIO_S32MSB = 0x9020;
        public const ushort AUDIO_S32 = AUDIO_S32LSB;
        public const ushort AUDIO_F32LSB = 0x8120;
        public const ushort AUDIO_F32MSB = 0x9120;
        public const ushort AUDIO_F32 = AUDIO_F32LSB;

        public static readonly ushort AUDIO_U16SYS =
            BitConverter.IsLittleEndian ? AUDIO_U16LSB : AUDIO_U16MSB;
        public static readonly ushort AUDIO_S16SYS =
            BitConverter.IsLittleEndian ? AUDIO_S16LSB : AUDIO_S16MSB;
        public static readonly ushort AUDIO_S32SYS =
            BitConverter.IsLittleEndian ? AUDIO_S32LSB : AUDIO_S32MSB;
        public static readonly ushort AUDIO_F32SYS =
            BitConverter.IsLittleEndian ? AUDIO_F32LSB : AUDIO_F32MSB;

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void SDL_AudioCallback(
            IntPtr userdata,
            IntPtr stream,
            int len
        );

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_AudioSpec
        {
            public int freq;
            public ushort format; // SDL_AudioFormat
            public byte channels;
            public byte silence;
            public ushort samples;
            public uint size;
            public SDL_AudioCallback callback;
            public IntPtr userdata; // void*
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SDL_Rect
        {
            public int x;
            public int y;
            public int w;
            public int h;
        }

        [Flags]
        public enum SDL_RendererFlags : uint
        {
            SDL_RENDERER_SOFTWARE = 0x00000001,
            SDL_RENDERER_ACCELERATED = 0x00000002,
            SDL_RENDERER_PRESENTVSYNC = 0x00000004,
            SDL_RENDERER_TARGETTEXTURE = 0x00000008
        }

        public enum SDL_PIXELTYPE_ENUM
        {
            SDL_PIXELTYPE_UNKNOWN,
            SDL_PIXELTYPE_INDEX1,
            SDL_PIXELTYPE_INDEX4,
            SDL_PIXELTYPE_INDEX8,
            SDL_PIXELTYPE_PACKED8,
            SDL_PIXELTYPE_PACKED16,
            SDL_PIXELTYPE_PACKED32,
            SDL_PIXELTYPE_ARRAYU8,
            SDL_PIXELTYPE_ARRAYU16,
            SDL_PIXELTYPE_ARRAYU32,
            SDL_PIXELTYPE_ARRAYF16,
            SDL_PIXELTYPE_ARRAYF32
        }

        public enum SDL_PIXELORDER_ENUM
        {
            /* BITMAPORDER */
            SDL_BITMAPORDER_NONE,
            SDL_BITMAPORDER_4321,
            SDL_BITMAPORDER_1234,
            /* PACKEDORDER */
            SDL_PACKEDORDER_NONE = 0,
            SDL_PACKEDORDER_XRGB,
            SDL_PACKEDORDER_RGBX,
            SDL_PACKEDORDER_ARGB,
            SDL_PACKEDORDER_RGBA,
            SDL_PACKEDORDER_XBGR,
            SDL_PACKEDORDER_BGRX,
            SDL_PACKEDORDER_ABGR,
            SDL_PACKEDORDER_BGRA,
            /* ARRAYORDER */
            SDL_ARRAYORDER_NONE = 0,
            SDL_ARRAYORDER_RGB,
            SDL_ARRAYORDER_RGBA,
            SDL_ARRAYORDER_ARGB,
            SDL_ARRAYORDER_BGR,
            SDL_ARRAYORDER_BGRA,
            SDL_ARRAYORDER_ABGR
        }

        public enum SDL_PACKEDLAYOUT_ENUM
        {
            SDL_PACKEDLAYOUT_NONE,
            SDL_PACKEDLAYOUT_332,
            SDL_PACKEDLAYOUT_4444,
            SDL_PACKEDLAYOUT_1555,
            SDL_PACKEDLAYOUT_5551,
            SDL_PACKEDLAYOUT_565,
            SDL_PACKEDLAYOUT_8888,
            SDL_PACKEDLAYOUT_2101010,
            SDL_PACKEDLAYOUT_1010102
        }

        public enum SDL_TextureAccess
        {
            SDL_TEXTUREACCESS_STATIC,
            SDL_TEXTUREACCESS_STREAMING,
            SDL_TEXTUREACCESS_TARGET
        }

        public enum SDL_AudioStatus
        {
            SDL_AUDIO_STOPPED,
            SDL_AUDIO_PLAYING,
            SDL_AUDIO_PAUSED
        }

        public static readonly uint SDL_PIXELFORMAT_UNKNOWN = 0;
        public static readonly uint SDL_PIXELFORMAT_INDEX1LSB =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_INDEX1,
                SDL_PIXELORDER_ENUM.SDL_BITMAPORDER_4321,
                0,
                1, 0
            );
        public static readonly uint SDL_PIXELFORMAT_INDEX1MSB =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_INDEX1,
                SDL_PIXELORDER_ENUM.SDL_BITMAPORDER_1234,
                0,
                1, 0
            );
        public static readonly uint SDL_PIXELFORMAT_INDEX4LSB =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_INDEX4,
                SDL_PIXELORDER_ENUM.SDL_BITMAPORDER_4321,
                0,
                4, 0
            );
        public static readonly uint SDL_PIXELFORMAT_INDEX4MSB =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_INDEX4,
                SDL_PIXELORDER_ENUM.SDL_BITMAPORDER_1234,
                0,
                4, 0
            );
        public static readonly uint SDL_PIXELFORMAT_INDEX8 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_INDEX8,
                0,
                0,
                8, 1
            );
        public static readonly uint SDL_PIXELFORMAT_RGB332 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_PACKED8,
                SDL_PIXELORDER_ENUM.SDL_PACKEDORDER_XRGB,
                SDL_PACKEDLAYOUT_ENUM.SDL_PACKEDLAYOUT_332,
                8, 1
            );
        public static readonly uint SDL_PIXELFORMAT_RGB444 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_PACKED16,
                SDL_PIXELORDER_ENUM.SDL_PACKEDORDER_XRGB,
                SDL_PACKEDLAYOUT_ENUM.SDL_PACKEDLAYOUT_4444,
                12, 2
            );
        public static readonly uint SDL_PIXELFORMAT_RGB555 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_PACKED16,
                SDL_PIXELORDER_ENUM.SDL_PACKEDORDER_XRGB,
                SDL_PACKEDLAYOUT_ENUM.SDL_PACKEDLAYOUT_1555,
                15, 2
            );
        public static readonly uint SDL_PIXELFORMAT_BGR555 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_INDEX1,
                SDL_PIXELORDER_ENUM.SDL_BITMAPORDER_4321,
                SDL_PACKEDLAYOUT_ENUM.SDL_PACKEDLAYOUT_1555,
                15, 2
            );
        public static readonly uint SDL_PIXELFORMAT_ARGB4444 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_PACKED16,
                SDL_PIXELORDER_ENUM.SDL_PACKEDORDER_ARGB,
                SDL_PACKEDLAYOUT_ENUM.SDL_PACKEDLAYOUT_4444,
                16, 2
            );
        public static readonly uint SDL_PIXELFORMAT_RGBA4444 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_PACKED16,
                SDL_PIXELORDER_ENUM.SDL_PACKEDORDER_RGBA,
                SDL_PACKEDLAYOUT_ENUM.SDL_PACKEDLAYOUT_4444,
                16, 2
            );
        public static readonly uint SDL_PIXELFORMAT_ABGR4444 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_PACKED16,
                SDL_PIXELORDER_ENUM.SDL_PACKEDORDER_ABGR,
                SDL_PACKEDLAYOUT_ENUM.SDL_PACKEDLAYOUT_4444,
                16, 2
            );
        public static readonly uint SDL_PIXELFORMAT_BGRA4444 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_PACKED16,
                SDL_PIXELORDER_ENUM.SDL_PACKEDORDER_BGRA,
                SDL_PACKEDLAYOUT_ENUM.SDL_PACKEDLAYOUT_4444,
                16, 2
            );
        public static readonly uint SDL_PIXELFORMAT_ARGB1555 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_PACKED16,
                SDL_PIXELORDER_ENUM.SDL_PACKEDORDER_ARGB,
                SDL_PACKEDLAYOUT_ENUM.SDL_PACKEDLAYOUT_1555,
                16, 2
            );
        public static readonly uint SDL_PIXELFORMAT_RGBA5551 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_PACKED16,
                SDL_PIXELORDER_ENUM.SDL_PACKEDORDER_RGBA,
                SDL_PACKEDLAYOUT_ENUM.SDL_PACKEDLAYOUT_5551,
                16, 2
            );
        public static readonly uint SDL_PIXELFORMAT_ABGR1555 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_PACKED16,
                SDL_PIXELORDER_ENUM.SDL_PACKEDORDER_ABGR,
                SDL_PACKEDLAYOUT_ENUM.SDL_PACKEDLAYOUT_1555,
                16, 2
            );
        public static readonly uint SDL_PIXELFORMAT_BGRA5551 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_PACKED16,
                SDL_PIXELORDER_ENUM.SDL_PACKEDORDER_BGRA,
                SDL_PACKEDLAYOUT_ENUM.SDL_PACKEDLAYOUT_5551,
                16, 2
            );
        public static readonly uint SDL_PIXELFORMAT_RGB565 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_PACKED16,
                SDL_PIXELORDER_ENUM.SDL_PACKEDORDER_XRGB,
                SDL_PACKEDLAYOUT_ENUM.SDL_PACKEDLAYOUT_565,
                16, 2
            );
        public static readonly uint SDL_PIXELFORMAT_BGR565 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_PACKED16,
                SDL_PIXELORDER_ENUM.SDL_PACKEDORDER_XBGR,
                SDL_PACKEDLAYOUT_ENUM.SDL_PACKEDLAYOUT_565,
                16, 2
            );
        public static readonly uint SDL_PIXELFORMAT_RGB24 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_ARRAYU8,
                SDL_PIXELORDER_ENUM.SDL_ARRAYORDER_RGB,
                0,
                24, 3
            );
        public static readonly uint SDL_PIXELFORMAT_BGR24 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_ARRAYU8,
                SDL_PIXELORDER_ENUM.SDL_ARRAYORDER_BGR,
                0,
                24, 3
            );
        public static readonly uint SDL_PIXELFORMAT_RGB888 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_PACKED32,
                SDL_PIXELORDER_ENUM.SDL_PACKEDORDER_XRGB,
                SDL_PACKEDLAYOUT_ENUM.SDL_PACKEDLAYOUT_8888,
                24, 4
            );
        public static readonly uint SDL_PIXELFORMAT_RGBX8888 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_PACKED32,
                SDL_PIXELORDER_ENUM.SDL_PACKEDORDER_RGBX,
                SDL_PACKEDLAYOUT_ENUM.SDL_PACKEDLAYOUT_8888,
                24, 4
            );
        public static readonly uint SDL_PIXELFORMAT_BGR888 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_PACKED32,
                SDL_PIXELORDER_ENUM.SDL_PACKEDORDER_XBGR,
                SDL_PACKEDLAYOUT_ENUM.SDL_PACKEDLAYOUT_8888,
                24, 4
            );
        public static readonly uint SDL_PIXELFORMAT_BGRX8888 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_PACKED32,
                SDL_PIXELORDER_ENUM.SDL_PACKEDORDER_BGRX,
                SDL_PACKEDLAYOUT_ENUM.SDL_PACKEDLAYOUT_8888,
                24, 4
            );
        public static readonly uint SDL_PIXELFORMAT_ARGB8888 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_PACKED32,
                SDL_PIXELORDER_ENUM.SDL_PACKEDORDER_ARGB,
                SDL_PACKEDLAYOUT_ENUM.SDL_PACKEDLAYOUT_8888,
                32, 4
            );
        public static readonly uint SDL_PIXELFORMAT_RGBA8888 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_PACKED32,
                SDL_PIXELORDER_ENUM.SDL_PACKEDORDER_RGBA,
                SDL_PACKEDLAYOUT_ENUM.SDL_PACKEDLAYOUT_8888,
                32, 4
            );
        public static readonly uint SDL_PIXELFORMAT_ABGR8888 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_PACKED32,
                SDL_PIXELORDER_ENUM.SDL_PACKEDORDER_ABGR,
                SDL_PACKEDLAYOUT_ENUM.SDL_PACKEDLAYOUT_8888,
                32, 4
            );
        public static readonly uint SDL_PIXELFORMAT_BGRA8888 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_PACKED32,
                SDL_PIXELORDER_ENUM.SDL_PACKEDORDER_BGRA,
                SDL_PACKEDLAYOUT_ENUM.SDL_PACKEDLAYOUT_8888,
                32, 4
            );
        public static readonly uint SDL_PIXELFORMAT_ARGB2101010 =
            SDL_DEFINE_PIXELFORMAT(
                SDL_PIXELTYPE_ENUM.SDL_PIXELTYPE_PACKED32,
                SDL_PIXELORDER_ENUM.SDL_PACKEDORDER_ARGB,
                SDL_PACKEDLAYOUT_ENUM.SDL_PACKEDLAYOUT_2101010,
                32, 4
            );
        public static readonly uint SDL_PIXELFORMAT_YV12 =
            SDL_DEFINE_PIXELFOURCC(
                (byte)'Y', (byte)'V', (byte)'1', (byte)'2'
            );
        public static readonly uint SDL_PIXELFORMAT_IYUV =
            SDL_DEFINE_PIXELFOURCC(
                (byte)'I', (byte)'Y', (byte)'U', (byte)'V'
            );
        public static readonly uint SDL_PIXELFORMAT_YUY2 =
            SDL_DEFINE_PIXELFOURCC(
                (byte)'Y', (byte)'U', (byte)'Y', (byte)'2'
            );
        public static readonly uint SDL_PIXELFORMAT_UYVY =
            SDL_DEFINE_PIXELFOURCC(
                (byte)'U', (byte)'Y', (byte)'V', (byte)'Y'
            );
        public static readonly uint SDL_PIXELFORMAT_YVYU =
            SDL_DEFINE_PIXELFOURCC(
                (byte)'Y', (byte)'V', (byte)'Y', (byte)'U'
            );

        public static uint SDL_DEFINE_PIXELFORMAT(
            SDL_PIXELTYPE_ENUM type,
            SDL_PIXELORDER_ENUM order,
            SDL_PACKEDLAYOUT_ENUM layout,
            byte bits,
            byte bytes
        )
        {
            return (uint)(
                (1 << 28) |
                (((byte)type) << 24) |
                (((byte)order) << 20) |
                (((byte)layout) << 16) |
                (bits << 8) |
                (bytes)
            );
        }

      
        public static uint SDL_DEFINE_PIXELFOURCC(byte A, byte B, byte C, byte D)
        {
            return SDL_FOURCC(A, B, C, D);
        } 

        public static uint SDL_FOURCC(byte A, byte B, byte C, byte D)
        {
            return (uint)(A | (B << 8) | (C << 16) | (D << 24));
        }

        public static unsafe string UTF8_ToManaged(IntPtr s, bool freePtr = false)
        {
            if (s == IntPtr.Zero)
            {
                return null;
            }

            /* We get to do strlen ourselves! */
            byte* ptr = (byte*)s;
            while (*ptr != 0)
            {
                ptr++;
            }

            /* TODO: This #ifdef is only here because the equivalent
             * .NET 2.0 constructor appears to be less efficient?
             * Here's the pretty version, maybe steal this instead:
             *
            string result = new string(
                (sbyte*) s, // Also, why sbyte???
                0,
                (int) (ptr - (byte*) s),
                System.Text.Encoding.UTF8
            );
             * See the CoreCLR source for more info.
             * -flibit
             */
#if NETSTANDARD2_0
			/* Modern C# lets you just send the byte*, nice! */
			string result = System.Text.Encoding.UTF8.GetString(
				(byte*) s,
				(int) (ptr - (byte*) s)
			);
#else
            /* Old C# requires an extra memcpy, bleh! */
            int len = (int)(ptr - (byte*)s);
            if (len == 0)
            {
                return string.Empty;
            }
            char* chars = stackalloc char[len];
            int strLen = System.Text.Encoding.UTF8.GetChars((byte*)s, len, chars, len);
            string result = new string(chars, 0, strLen);
#endif

            /* Some SDL functions will malloc, we have to free! */
            if (freePtr)
            {
                SDL_free(s);
            }
            return result;
        }

        public static string SDL_GetError()
        {
            return UTF8_ToManaged(INTERNAL_SDL_GetError());
        }

        internal static byte[] UTF8_ToNative(string s)
        {
            if (s == null)
            {
                return null;
            } 
            // Add a null terminator. That's kind of it... :/
            return System.Text.Encoding.UTF8.GetBytes(s + '\0');
        }

        public static uint SDL_OpenAudioDevice(
            string device,
            int iscapture,
            ref SDL_AudioSpec desired,
            out SDL_AudioSpec obtained,
            int allowed_changes
        )
        {
            return INTERNAL_SDL_OpenAudioDevice(
                UTF8_ToNative(device),
                iscapture,
                ref desired,
                out obtained,
                allowed_changes
            );
        }

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_OpenAudio(
            ref SDL_AudioSpec desired,
            out SDL_AudioSpec obtained
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_OpenAudio(
            ref SDL_AudioSpec desired,
            IntPtr obtained
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_Init(uint flags);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_PauseAudio(int pause_on);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_Delay(UInt32 ms); 

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_CloseAudio();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_Quit();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyTexture(IntPtr texture);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyRenderer(IntPtr renderer);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_DestroyWindow(IntPtr window);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RaiseWindow(IntPtr window);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RestoreWindow(IntPtr window);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateWindowFrom(IntPtr data);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_ShowWindow(IntPtr window);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_SetWindowSize(
            IntPtr window,
            int w,
            int h
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateRenderer(
            IntPtr window,
            int index,
            SDL_RendererFlags flags
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr SDL_CreateTexture(
            IntPtr renderer,
            uint format,
            int access,
            int w,
            int h
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_UpdateTexture(
            IntPtr texture,
            ref SDL_Rect rect,
            IntPtr pixels,
            int pitch
        ); 


        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderClear(IntPtr renderer);


        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopy(
            IntPtr renderer,
            IntPtr texture,
            IntPtr srcrect,
            ref SDL_Rect dstrect
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopy(
            IntPtr renderer,
            IntPtr texture,
            ref SDL_Rect srcrect,
            IntPtr dstrect
        );


        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopy(
            IntPtr renderer,
            IntPtr texture,
            ref SDL_Rect srcrect,
            ref SDL_Rect dstrect
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int SDL_RenderCopy(
            IntPtr renderer,
            IntPtr texture,
            IntPtr srcrect,
            IntPtr dstrect
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_RenderPresent(IntPtr renderer);

        [DllImport(nativeLibName, EntryPoint = "SDL_GetError", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr INTERNAL_SDL_GetError();

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void SDL_free(IntPtr memblock);

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern SDL_AudioStatus SDL_GetAudioDeviceStatus(
            uint dev
        );

        [DllImport(nativeLibName, EntryPoint = "SDL_OpenAudioDevice", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint INTERNAL_SDL_OpenAudioDevice(
            byte[] device,
            int iscapture,
            ref SDL_AudioSpec desired,
            out SDL_AudioSpec obtained,
            int allowed_changes
        );
         
        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_PauseAudioDevice(
            uint dev,
            int pause_on
        );

        [DllImport(nativeLibName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void SDL_CloseAudioDevice(uint dev);

    }
}

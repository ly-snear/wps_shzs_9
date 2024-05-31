using CalligraphyAssistantMain.Code;
using FFmpeg.AutoGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CalligraphyAssistantMain.Controls
{
    public unsafe class UDPPlayerControl : Grid
    {
        [DllImport("libptsd.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int create_pts_context(int id, ref IntPtr context, string url, IntPtr hwnd, PTS_CONTEXT_CALLBACK handle, uint play_audio, uint ipbind, uint drawer, uint decoder);

        [DllImport("libptsd.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int delete_pts_context(ref IntPtr context);

        [DllImport("libptsd.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int pts_context_record(ref IntPtr context, string record_fname, bool bRecord);

        [DllImport("libptsd.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int pts_context_audio(ref IntPtr context, int play_audio);

        [DllImport("libptsd.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int pts_context_rotate(ref IntPtr context, int rotate);

        public static class PTS_CONTEXT_TYPE
        {
            public const int h264 = 100;
            public const int h265 = 101;
            public const int mjpeg = 102;
            public const int yuv420 = 109;
            public const int g711 = 110;
            public const int aac = 111;
        }

        public static class PTS_ROTATE_TYPE
        {
            public const int rotate_none = 0;
            public const int rotate_90 = 1;
            public const int rotate_180 = 2;
            public const int rotate_270 = 3;
        }

        public string Url { get; private set; } = string.Empty;

        private bool isStarted = false;
        private IntPtr context;
        private WindowsFormsHost windowsFormsHost = null;
        private PictureBox pictureBox = null;
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int PTS_CONTEXT_CALLBACK(int id, int type, IntPtr data, IntPtr size, int width, int height, int video_keyframe, int audio_samplerate, int audio_chn, UInt64 ts);
        public event EventHandler Click = null;

        public UDPPlayerControl()
        {
            windowsFormsHost = new WindowsFormsHost();
            this.Children.Add(windowsFormsHost);
        }

        public void Play(string url, int id)
        {
            if (!string.IsNullOrEmpty(url) && this.Url != url)
            {
                if (pictureBox != null)
                {
                    pictureBox.Dispose();
                }
                pictureBox = new PictureBox();
                pictureBox.Click += PictureBox_Click;
                windowsFormsHost.Child = pictureBox;
                this.Url = url;
                context = IntPtr.Zero;
                IntPtr hwnd = IntPtr.Zero;
                this.Dispatcher.Invoke(() =>
                {
                    hwnd = pictureBox.Handle;
                });
                create_pts_context(id, ref context, url, hwnd, null, 1, 0, 0, 0);
                pts_context_rotate(ref context, PTS_ROTATE_TYPE.rotate_none);
            }
        }

        public void Stop()
        {
            Url = string.Empty;
            delete_pts_context(ref context);
            if (pictureBox != null)
            {
                pictureBox.Dispose();
            }
            pictureBox = null;
        }

        private void PictureBox_Click(object sender, EventArgs e)
        {
            if (Click != null)
            {
                Click(this, null);
            }
        }
    }
}

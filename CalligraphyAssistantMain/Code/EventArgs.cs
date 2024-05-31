using CefSharp;
using FFmpeg.AutoGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CalligraphyAssistantMain.Code
{
    public class NewAVPacketEventArgs : EventArgs
    {
        public AVPacket Packet { get; set; }
        public int Frame { get; set; }
        public Size Size { get; set; }
    }

    public class RecordChangedEventArgs : EventArgs
    {
        public bool IsRecord { get; set; }
        public int Type { get; set; }
    }

    public class DownloadProgressChangedEventArgs : EventArgs
    {
        public DownloadItem DownloadItem { get; set; }
        public IDownloadItemCallback Callback { get; set; }
    }

    public class DeleteResourceItemEventArgs : EventArgs
    { 
        public ResourceItemInfo ResourceItemInfo { get; set; }
    } 
}

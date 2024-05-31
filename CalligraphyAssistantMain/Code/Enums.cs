using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    public enum DrawMode
    {
        Pen,
        Line,
        Rectangle,
        Ellipse,
        Eraser,
        Drag,
        None,
        ClosedPolyLine,
        NonClosedPolyLine,
        SignRectangle,
        DragSignRectangle,
        ScaleSignRectangle,
        Text
    }

    public enum PicShowMod
    {
        stretch = 0,//拉伸
        center = 1,//居中
        adaptive = 2,//自适应
    }

    public enum RemoteClientType
    {
        Unknown = -1,
        Server = 0,
        Client = 1,
    }

    public enum ClientMessageType
    {
        Login = 0x01,
        Heartbeat = 0x02,
        SendPhoto = 0x03,
        SendPreviewVideoFrame = 0x04,
        SendVideoFrame = 0x05,
        GetLastImageList = 0x06,

        CompleteVote=0x07,
        CompleteQuickAnswer =0x08,
        PaperAccept=0X09,
        FileAccept = 0X09

    }

    public enum ServerMessageType
    {
        LoginFeedback = 0x01,
        Heartbeat = 0x02,
        SendImage = 0x03,
        SendVideoFrame = 0x04,
        ImageMode = 0x05,
        VideoMode = 0x06,
        StartCameraLive = 0x07,
        StopCameraLive = 0x08,
        LockScreen = 0x09,
        TakePhoto = 0x10,
        Logout = 0x11,

        ShareScreen = 0x12,
        StartVote=0X13,
        StopVote=0x14,
        StartQuickAnswer=0x15,
        StopQuickAnswer=0x16,
        PublishQuickAnswer=0x17,
        FileDistribute =0x18,
        PaperDistribute=0X19,
        ControlOperation=0X20,
        WorkShare=0x21
    }

    public enum DownloadState
    {
        Downloading = 1,
        Downloaded = 2,
        Canceled = 3
    }

    public enum CalligraphyMode
    {
        Image = 0,
        Word = 1
    }

    public enum PlayState
    {
        Stoped,
        Disposing,
        Prepareing,
        Playing,
        Connecting,
        Buffering
    }
}

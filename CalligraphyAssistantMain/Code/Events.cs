using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    public class PreviewVideoFrameEvent : PubSubEvent<VideoFrameModel>
    {
    }

    public class VideoFrameEvent : PubSubEvent<VideoFrameModel>
    {
    }

    public class TakePhotosEvent : PubSubEvent<TakePhotosModel>
    {
    }

    public class StudentLoginEvent : PubSubEvent<CommonModel>
    {
    }

    public class GetLastImageListEvent : PubSubEvent<CommonModel>
    {
    }

    public class FullScreenChangeStudentEvent : PubSubEvent<CommonModel>
    {

    }

    public class VideoFrameModel
    {
        public object Sender { get; set; }
        public StudentInfo Student { get; set; }
        public int Frame { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] Buffer { get; set; }
    }

    public class TakePhotosModel
    {
        public object Sender { get; set; }
        public StudentInfo Student { get; set; }
        public string ImagePath { get; set; }
    }

    public class CommonModel
    {
        public object Sender { get; set; }
        public StudentInfo Student { get; set; }
    }
}

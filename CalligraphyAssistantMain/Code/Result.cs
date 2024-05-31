using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    public class Result
    {
        public int code { get; set; }
        public string msg { get; set; }
    }

    public class Upload
    {
        public string title { get; set; }
        public string url { get; set; }
    }

    public class ResultUpload : Result
    {
        public Upload data { get; set; }
    }
}

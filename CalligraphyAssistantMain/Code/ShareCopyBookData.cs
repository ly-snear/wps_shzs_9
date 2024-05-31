using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    /// <summary>
    /// 分享字帖返回数据
    /// </summary>
    public class ShareCopyBookData
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public int Type { get; set; }
        public ShareCopyBookData() { }
        public ShareCopyBookData(string title, string url, int type)
        {
            Title = title;
            Url = url;
            Type = type;
        }
    }
}

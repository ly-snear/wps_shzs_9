using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    public class CommitMaterialAnswer
    {
        /// <summary>
        /// 拼图后保存到云端的地址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public long start { get; set; }

        /// <summary>
        /// 耗时时长
        /// </summary>
        public long elapsed { get; set; }

        /// <summary>
        /// 学生ID
        /// </summary>
        public long student;

        /// <summary>
        /// 学生名称
        /// </summary>
        public string name { get; set; }
    }
}

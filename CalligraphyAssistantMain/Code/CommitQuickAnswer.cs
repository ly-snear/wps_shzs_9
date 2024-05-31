using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    /// <summary>
    /// 提交快问题
    /// </summary>
    public class CommitQuickAnswer
    {
        /// <summary>
        /// 答案
        /// </summary>
        public string answer { get; set; }

        /// <summary>
        /// 声音答案地址
        /// </summary>
        public string audio { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public long start { get; set; }

        /// <summary>
        /// 耗时时长
        /// </summary>
        public long elapsed { get; set; }

        /// <summary>
        /// 题目ID
        /// </summary>
        public long id;

        /// <summary>
        /// 学生ID
        /// </summary>
        public long student;

        /// <summary>
        /// 学生名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 题型 0:选择题 1:主观题
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 答题方式 0:抢答 1:随机抽人 2:选择答题学生
        /// </summary>
        public int style { get; set; }
    }
}

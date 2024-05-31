using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    /// <summary>
    /// 拼图统计
    /// </summary>
    public class MaterialCount
    {
        /// <summary>
        /// 倒计时
        /// </summary>
        public int countdown { get; set; }

        /// <summary>
        /// 学生信息
        /// </summary>
        public List<StudentInfo> students { get; set; }

        public MaterialCount() { }

        public MaterialCount(int countdown, List<StudentInfo> students)
        {
            this.countdown = countdown;
            this.students = students;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    public class MaterialCountData: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _total;
        private int _complete;

        /// <summary>
        /// 答题学生总数
        /// </summary>
        public int Total
        {
            get { return _total; }
            set { 
                _total = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Total"));
                }
            }
        }

        /// <summary>
        /// 完成数量
        /// </summary>
        public int Complete
        {
            get { return _complete; }
            set { 
                _complete = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Complete"));
                }
            }
        }
    }
}

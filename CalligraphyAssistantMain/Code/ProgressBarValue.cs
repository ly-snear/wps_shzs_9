using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    public class ProgressBarValue: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private long _min;
        private long _value;
        private long _total;
        public long Value
        {
            get { return _value; }
            set { 
                _value = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Value"));
                }
            }
        }
        public long Total
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
        public long Min
        {
            get { return _min; }
            set
            {
                _min = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Min"));
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    public class ShareCopyBook: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _id;
        private bool _isSelect;
        private string _title;
        private string _url;
        private int _type;

        public string Id { 
            get { return _id; }
            set {  
                _id = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Id"));
                }
            } 
        }
        public string Title { 
            get { return _title; } 
            set {
                _title = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Title"));
                }
            }
        }
        public string Url { 
            get { return _url; } 
            set {
                _url = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Url"));
                }
            } 
        }
        public bool IsSelect { 
            get { return _isSelect; } 
            set { 
                _isSelect = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("IsSelect"));
                }
            } 
        }

        public int Type
        {
            get { return _type; }
            set { 
                _type = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Type"));
                }
            }
        }
    }
}

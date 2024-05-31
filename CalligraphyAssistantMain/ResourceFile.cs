using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain
{
    public class ResourceFile : INotifyPropertyChanged
    {
        private long _id;
        private string _name;
        private string _url;
        private bool _isSelect;
        public event PropertyChangedEventHandler PropertyChanged;

        public ResourceFile(long _id, string _name, string _url, bool _isSelect)
        {
            id = _id;
            name = _name;
            url = _url;
            isSelect = _isSelect;
        }

        public ResourceFile()
        {

        }

        public long id
        {
            get { return _id; }
            set
            {
                _id = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("id"));
                }
            }
        }

        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("name"));
                }
            }
        }

        public string url
        {
            get { return _url; }
            set
            {
                _url = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("url"));
                }
            }
        }

        public bool isSelect
        {
            get { return _isSelect; }
            set
            {
                _isSelect = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("isSelect"));
                }
            }
        }
    }
}

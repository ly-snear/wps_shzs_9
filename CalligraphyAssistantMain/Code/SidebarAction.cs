using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Code
{
    public class SidebarAction : INotifyPropertyChanged
    {
        /// <summary>
        /// 动作标题
        /// </summary>
        private string title;

        /// <summary>
        /// 动作状态
        /// </summary>
        private int state;

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Title"));
                }
            }
        }

        public int State
        {
            set { state = value; }
            get { return state; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}

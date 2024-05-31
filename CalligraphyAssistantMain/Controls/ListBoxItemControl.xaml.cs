using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalligraphyAssistantMain.Controls
{
    /// <summary>
    /// ListBoxItemControl.xaml 的交互逻辑
    /// </summary>
    public partial class ListBoxItemControl : UserControl, INotifyPropertyChanged
    {
        private string text = string.Empty;
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                NotifyPropertyChanged("Text");
            }
        }

        public ListBoxItemControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}

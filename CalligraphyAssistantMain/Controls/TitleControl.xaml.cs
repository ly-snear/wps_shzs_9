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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalligraphyAssistantMain.Controls
{
    /// <summary>
    /// TitleControl.xaml 的交互逻辑
    /// </summary>
    public partial class TitleControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler CloseClick = null;
        public bool CanMove { get; set; } = true;
        private string title = string.Empty;
        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }
        public TitleControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public void HideMinimized()
        { 
            minBtn.Visibility = Visibility.Collapsed;
        } 

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void backGd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource != null &&
              e.OriginalSource is Image &&
             ((e.OriginalSource as Image).Parent == closeBtn))
            {
                return;
            }
            if (CanMove)
            {
                Window owner = Window.GetWindow(this);
                if (owner != null)
                {
                    owner.DragMove();
                }
            }
        }

        private void minBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Window.GetWindow(this).WindowState = WindowState.Minimized;
        }

        private void closeBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CloseClick != null)
            {
                CloseClick(this, null);
            }
        }
    }
}

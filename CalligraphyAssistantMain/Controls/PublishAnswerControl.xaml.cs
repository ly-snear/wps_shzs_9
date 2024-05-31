using PropertyChanged;
using System;
using System.Collections.Generic;
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
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// PublishAnswerControl.xaml 的交互逻辑
    /// </summary>
    public partial class PublishAnswerControl : Window
    {
        public int OptionCount { get; set; } = 2;
        public int Answer {  get; set; }
        public PublishAnswerControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void btnOk_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = true;
        }

        private void closeBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

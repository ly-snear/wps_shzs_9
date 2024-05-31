using CalligraphyAssistantMain.Code;
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
    /// <summary>
    /// ImageTagControl.xaml 的交互逻辑
    /// </summary>
    public partial class ImageTagControl : UserControl
    {
        public event EventHandler CloseClick = null;
        public ImageTagInfo TagInfo { get; set; }
        public ImageTagControl()
        {
            InitializeComponent();
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (CloseClick != null)
            {
                CloseClick(this, null);
            }
        }
    }
}

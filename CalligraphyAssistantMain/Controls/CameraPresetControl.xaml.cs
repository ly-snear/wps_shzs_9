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
    /// CameraNumberControl.xaml 的交互逻辑
    /// </summary>
    public partial class CameraPresetControl : UserControl
    {
        public event EventHandler ItemClick = null;
        public CameraPresetControl()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ItemClick != null)
            {
                ItemClick(this, null);
            }
            e.Handled = true;
        }
    }
}

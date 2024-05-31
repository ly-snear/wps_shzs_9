using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CalligraphyAssistantMain
{
    /// <summary>
    /// WriteScreenWindow.xaml 的交互逻辑
    /// </summary>
    public partial class WriteScreenWindow : Window
    {
        public WriteScreenWindow()
        {
            InitializeComponent();
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
            if (SystemParameters.PrimaryScreenWidth != 1920 && SystemParameters.PrimaryScreenHeight != 1080)
            { 
                backGd.Children.Remove(writeScreenControl);
                viewBox.Child = writeScreenControl;
                viewBox.Visibility = Visibility.Visible;
            }
        }

        public void SetBackground(ImageBrush brush)
        {
            writeScreenControl.SetBackground(brush);
            writeScreenControl.SetMode(true);
        }

        private void writeScreenControl_Back(object sender, EventArgs e)
        { 
            this.Close();
        }
    }
}

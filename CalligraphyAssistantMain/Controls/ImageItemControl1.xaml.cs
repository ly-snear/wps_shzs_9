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
    /// ImageItemControl1.xaml 的交互逻辑
    /// </summary>
    public partial class ImageItemControl1 : UserControl
    {
        public string ImagePath { get; private set; }
        public int Type { get; private set; }
        public event EventHandler DeleteClick = null;
        public ImageItemControl1()
        {
            InitializeComponent();
        }

        public void SetImage(string imagePath, int type = 0)
        {
            try
            {
                image.Source = new BitmapImage(new Uri(imagePath));
                ImagePath = imagePath;
                Type = type;
            }
            catch
            {
            }
        }

        private void deleteBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DeleteClick != null)
            {
                DeleteClick(this, null);
            }
            e.Handled = true;
        }
    }
}

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
using System.Windows.Shapes;

namespace CalligraphyAssistantMain.Controls
{
    /// <summary>
    /// DocFrom.xaml 的交互逻辑
    /// </summary>
    public partial class DocFrom : Window
    {
        public string FilePath { get; set; }
        public DocFrom(string parameter)
        {
            InitializeComponent();
            FilePath = parameter;
            Window_Loaded(FilePath);
        }

        private void Window_Loaded(string url)
        {
            Uri fileUri = new Uri(url);
            webBrowser.Navigate(fileUri);
        }


    }
}

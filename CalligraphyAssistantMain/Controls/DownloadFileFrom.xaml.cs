using CalligraphyAssistantMain.Code;
using Microsoft.Graph;
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
    /// DownloadFileFrom.xaml 的交互逻辑
    /// </summary>
    public partial class DownloadFileFrom : Window
    {
        public string FilePath;
        public string FileName;
        public string SavePath;
        public DownloadFileFrom(string filePath, string fileName, string savePath)
        {
            FilePath = filePath;
            FileName = fileName;
            SavePath = savePath;
            InitializeComponent();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            bool b = HttpClass.DownloadFile(FilePath, SavePath + "\\" + FileName, progressBar1, label1);
            if (b)
                MessageBox.Show("下载成功");
            else
                MessageBox.Show("下载失败");
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

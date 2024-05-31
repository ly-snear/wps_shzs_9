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
    /// ImageViewControl.xaml 的交互逻辑
    /// </summary>
    public partial class ImageViewControl : UserControl
    {
        private int selectedImageIndex = 0;
        private StudentWorkDetailsInfo[] workArr = null;
        public ImageViewControl()
        {
            InitializeComponent();
        }

        public void ShowImage(StudentWorkDetailsInfo[] workArr, int index)
        {
            this.workArr = workArr;
            this.selectedImageIndex = index;
            ShowImage(index);
            this.Visibility = Visibility.Visible;
        }

        private void ShowImage(int index)
        {
            try
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(workArr[index].LocalPath, UriKind.Absolute));
                image.Source = bitmapImage;
                this.selectedImageIndex = index;
                studentInfoLb.Text = workArr[index].ClassName + " - " + workArr[index].StudentName;
                pageLb.Text = (index + 1) + "/" + workArr.Length;
            }
            catch (Exception ex)
            {
                Common.Trace("ImageViewControl ShowImage Error:" + ex.Message);
            }
        }

        private void cancelBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }


        private void previousBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        { 
            if (selectedImageIndex > 0)
            {
                ShowImage(selectedImageIndex - 1);
            }
        }

        private void nextBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        { 
            if (selectedImageIndex + 1 < workArr.Length)
            {
                ShowImage(selectedImageIndex + 1);
            }
        }
    }
}

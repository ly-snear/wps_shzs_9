using CalligraphyAssistantMain.Code;
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
    /// ImageItemControl1.xaml 的交互逻辑
    /// </summary>
    public partial class ImageItemControl2 : UserControl, INotifyPropertyChanged
    {
        private string title;
        private int count;
        private bool isSelected;
        private SolidColorBrush defaultBorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9E9E9E"));
        private SolidColorBrush selectedBorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EE5C3B"));
        public event PropertyChangedEventHandler PropertyChanged;
        public string ImagePath { get; private set; }
        public string Title { get => title; set { title = value; NotifyPropertyChanged("Title"); } }
        public int Count { get => count; set { count = value; NotifyPropertyChanged("Count"); } }
        public bool IsSelected { get => isSelected; set { isSelected = value; NotifyPropertyChanged("IsSelected"); NotifyPropertyChanged("CurrentBorderBrush"); } }
        public SolidColorBrush CurrentBorderBrush { get => isSelected ? selectedBorderBrush : defaultBorderBrush; }

        public event EventHandler ImageClick = null;
        public ImageItemControl2()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public virtual void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void SetImage(string imagePath)
        {
            if (imagePath.StartsWith("//"))
            {
                imagePath = "http://" + imagePath.TrimStart(new char[] { '/' });
            } 
            try
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.UriSource = new Uri(imagePath);
                bitmapImage.EndInit();
                image.Source = bitmapImage;
            }
            catch (Exception ex)
            {
                Common.Trace("ImageItemControl2 SetImage Error:" + ex.Message);
            }
            ImagePath = imagePath;
        }

        private void image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (ImageClick != null)
            {
                ImageClick(this, null);
            }
            e.Handled = true;
        }
    }
}

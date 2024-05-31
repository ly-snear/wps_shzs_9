using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CalligraphyAssistantMain.Code
{
    public static class Consts
    {
        public const string Version = "V1.0.1";
        public const string Key = "NeNaYuSH";
        public const double KB = 1024;
        public const double MB = KB * KB;
        public const double GB = MB * KB;
        public static ImageSource CameraPresetImage1 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/Item.png", UriKind.Absolute));
        public static ImageSource CameraPresetImage2 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/Item2.png", UriKind.Absolute));
        public static ImageSource SelectButton1 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/SelectButton.png", UriKind.Absolute));
        public static ImageSource SelectButton2 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/SelectButton2.png", UriKind.Absolute));
        public static SolidColorBrush SelectButtonColor1 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3A3A3A"));
        //public static SolidColorBrush SelectButtonColor2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#86725E"));
        public static SolidColorBrush MainButtonColor1 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BD9B4E"));
        public static SolidColorBrush MainButtonColor2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EE5C3B"));
        public static SolidColorBrush ImageEditButtonColor1 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#BF8C45"));
        public static SolidColorBrush ImageEditButtonColor2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AA2F18"));
        public static SolidColorBrush ImageEditButton2Color1 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3A3A3A"));
        public static SolidColorBrush ImageEditButton2Color2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#AA2F18"));
        public static SolidColorBrush StudentControlColor1 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F1DA9E"));
        public static SolidColorBrush StudentControlColor2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEA356"));

        public static BitmapImage MainMenuButton1 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/Button1.png", UriKind.Absolute));
        public static BitmapImage MainMenuButton1_2 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/Button1_2.png", UriKind.Absolute));
        public static BitmapImage MainMenuButton2 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/Button2.png", UriKind.Absolute));
        public static BitmapImage MainMenuButton2_2 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/Button2_2.png", UriKind.Absolute));
        public static BitmapImage DefaultImageButton1 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/DefaultButton.png", UriKind.Absolute));
        public static BitmapImage DefaultImageButton2 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/DefaultButton2.png", UriKind.Absolute));
        public static BitmapImage ImageEditButton2 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/ImageEditIcon2.png", UriKind.Absolute));
        public static BitmapImage ImageEditButton2_2 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/ImageEditIcon2_2.png", UriKind.Absolute));
        public static BitmapImage ImageEditButton3 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/ImageEditIcon4.png", UriKind.Absolute));
        public static BitmapImage ImageEditButton3_2 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/ImageEditIcon4_2.png", UriKind.Absolute));
        public static BitmapImage ImageEditButton4 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/ImageEditIcon3.png", UriKind.Absolute));
        public static BitmapImage ImageEditButton4_2 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/ImageEditIcon3_2.png", UriKind.Absolute));
        public static BitmapImage ImageEditButton5 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/ImageEditIcon6.png", UriKind.Absolute));
        public static BitmapImage ImageEditButton5_2 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/ImageEditIcon6_2.png", UriKind.Absolute));
        public static BitmapImage ImageEditButton9 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/ImageEditIcon9.png", UriKind.Absolute));
        public static BitmapImage ImageEditButton9_2 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/ImageEditIcon9_2.png", UriKind.Absolute));
        public static BitmapImage ImageEditButton10 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/ImageEditIcon10.png", UriKind.Absolute));
        public static BitmapImage ImageEditButton10_2 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/ImageEditIcon10_2.png", UriKind.Absolute));
        public static BitmapImage ImageEditButton11 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/ImageEditIcon11.png", UriKind.Absolute));
        public static BitmapImage ImageEditButton11_2 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/ImageEditIcon11_2.png", UriKind.Absolute));
        public static BitmapImage ImageEditButton12 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/ImageEditIcon12.png", UriKind.Absolute));
        public static BitmapImage ImageEditButton12_2 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/ImageEditIcon12_2.png", UriKind.Absolute));
        public static BitmapImage ImageEditButton13 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/ImageEditIcon13.png", UriKind.Absolute));
        public static BitmapImage ImageEditButton13_2 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/ImageEditIcon13_2.png", UriKind.Absolute));
        public static BitmapImage ImageEditButton14 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/ImageEditIcon14.png", UriKind.Absolute));
        public static BitmapImage ImageEditButton14_2 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/ImageEditIcon14_2.png", UriKind.Absolute));
        public static BitmapImage DownloadingIcon = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/DownloadingIcon.png", UriKind.Absolute));
        public static BitmapImage DownloadedIcon = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/DownloadedIcon.png", UriKind.Absolute));
        public static SolidColorBrush BorderColor1 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#9E9E9E"));
        public static SolidColorBrush BorderColor2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00AF7A"));
        public static SolidColorBrush BorderBackColor1 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F6F7FA"));


    }
}

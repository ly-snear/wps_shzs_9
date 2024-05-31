using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace CalligraphyAssistantMain.Code
{
    public class NumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return 0;
            return System.Convert.ToInt32(value) + 1;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StudentControlConverter : IValueConverter
    {
        //源属性传给目标属性时，调用此方法ConvertBack
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string parm = parameter.ToString();
            switch (parm)
            {
                case "Color":
                    bool online = (bool)value;
                    return online ? Consts.MainButtonColor2 : Consts.SelectButtonColor1;
                case "Enabled":
                    online = (bool)value;
                    return online;
                default:
                    break;
            }
            return null;
        }

        //目标属性传给源属性时，调用此方法ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class CameraControlConverter : IValueConverter
    {
        //源属性传给目标属性时，调用此方法ConvertBack
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string parm = parameter.ToString();
            switch (parm)
            {
                case "Title":
                    CameraItemInfo itemInfo = value as CameraItemInfo;
                    if (itemInfo.Mode == 1)
                    {
                        StudentInfo student = itemInfo.StudentList.FirstOrDefault(p => p.IsSelected);
                        if (student == null)
                        {
                            return itemInfo.Name;
                        }
                        return itemInfo.Name + " - " + student.Name;
                    }
                    return itemInfo.Name + " - " + "全景";
                case "Mode":
                    return (int)value == 1 ? "全景" : "特写";
                case "Enabled":
                    return (int)value == 1;
                case "Visibility":
                    return (int)value == 1 ? Visibility.Visible : Visibility.Collapsed;
                default:
                    break;
            }
            return null;
        }

        //目标属性传给源属性时，调用此方法ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class MainControlConverter : IValueConverter
    {
        //源属性传给目标属性时，调用此方法ConvertBack
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string parm = parameter.ToString();
            if (parm.StartsWith("Menu1_Back1"))
            {
                string[] strArr = parm.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                if (strArr.Length == 3)
                {
                    return value.ToString().Equals(strArr[2]) ? Consts.SelectButton2 : Consts.SelectButton1;
                }
                return null;
            }
            if (parm.StartsWith("Menu1_Color1"))
            {
                string[] strArr = parm.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                if (strArr.Length == 3)
                {
                    return value.ToString().Equals(strArr[2]) ? Brushes.White : Consts.SelectButtonColor1;
                }
                return null;
            }
            switch (parm)
            {
                case "Hover":
                    return (bool)value ? Visibility.Visible : Visibility.Collapsed;
                case "Menu1_Color_1":
                    return (int)value > -1 && (int)value < 5 ? Consts.MainButtonColor2 : Consts.MainButtonColor1;
                //case "Menu1_Color_2":
                //    return (int)value == 0 ? Consts.MainButtonColor2 : Consts.MainButtonColor1;
                default:
                    break;
            }
            return null;
        }

        //目标属性传给源属性时，调用此方法ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class MainWindowConverter : IValueConverter
    {
        //源属性传给目标属性时，调用此方法ConvertBack
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string parm = parameter.ToString();
            if (parm.StartsWith("MenuButton_"))
            {
                string[] strArr = parm.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                if (strArr.Length == 2)
                {
                    if (strArr[1].Equals("1"))
                    {
                        return value.ToString().Equals("1") ? Consts.MainMenuButton1_2 : Consts.MainMenuButton1;
                    }
                    else
                    {
                        return value.ToString().Equals("2") ? Consts.MainMenuButton2_2 : Consts.MainMenuButton2;
                    }
                }
            }
            else if (parm.StartsWith("Visibility_"))
            {
                string[] strArr = parm.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                if (strArr.Length == 2)
                {
                    if (strArr[1].Equals("1"))
                    {
                        return value.ToString().Equals("1") ? Visibility.Visible : Visibility.Collapsed;
                    }
                    else
                    {
                        return value.ToString().Equals("2") ? Visibility.Visible : Visibility.Collapsed;
                    }
                }
            }
            switch (parm)
            {
                case "ShowMoreMenu":
                    return (int)value == 2 ? Visibility.Visible : Visibility.Collapsed;
                case "IsClassBegin_1":
                    return !(bool)value ? Visibility.Visible : Visibility.Collapsed;
                case "IsClassBegin_2":
                    return (bool)value ? Visibility.Visible : Visibility.Collapsed;
                default:
                    break;
            }
            return null;
        }

        //目标属性传给源属性时，调用此方法ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ImageEditControlConverter : IValueConverter
    {
        //源属性传给目标属性时，调用此方法ConvertBack
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string parm = parameter.ToString();
            if (parm.StartsWith("TextColor1_"))
            {
                string[] strArr = parm.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                if (strArr.Length == 2)
                {
                    return value.ToString().Equals(strArr[1]) ? Consts.ImageEditButtonColor2 : Consts.ImageEditButtonColor1;
                }
                return null;
            }
            else if (parm.StartsWith("Visibility1_"))
            {
                string[] strArr = parm.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                if (strArr.Length == 2)
                {
                    return value.ToString().Equals(strArr[1]) ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            else if (parm.Equals("Visibility2"))
            {
                return (int)value > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            else if (parm.StartsWith("Image_"))
            {
                string[] strArr = parm.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                if (strArr.Length == 2)
                {
                    string temp = value.ToString();
                    switch (strArr[1])
                    {
                        case "2":
                            return temp.Equals("2") ? Consts.ImageEditButton2_2 : Consts.ImageEditButton2;
                        //case "3":
                        //    return temp.Equals("3") ? Consts.ImageEditButton3_2 : Consts.ImageEditButton3;
                        case "4":
                            return temp.Equals("4") ? Consts.ImageEditButton4_2 : Consts.ImageEditButton4;
                        case "5":
                            return temp.Equals("5") ? Consts.ImageEditButton5_2 : Consts.ImageEditButton5;
                        case "9":
                            return temp.Equals("9") ? Consts.ImageEditButton9_2 : Consts.ImageEditButton9;
                        default:
                            break;
                    }
                }
            }
            else if (parm.StartsWith("Image2_"))
            {
                string[] strArr = parm.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                if (strArr.Length == 2)
                {
                    string temp = value.ToString();
                    switch (strArr[1])
                    {
                        case "10":
                            return temp.Equals("10") ? Consts.ImageEditButton10_2 : Consts.ImageEditButton10;
                        case "11":
                            return temp.Equals("11") ? Consts.ImageEditButton11_2 : Consts.ImageEditButton11;
                        case "12":
                            return temp.Equals("12") ? Consts.ImageEditButton12_2 : Consts.ImageEditButton12;
                        case "13":
                            return temp.Equals("13") ? Consts.ImageEditButton13_2 : Consts.ImageEditButton13;
                        case "14":
                            return temp.Equals("14") ? Consts.ImageEditButton14_2 : Consts.ImageEditButton14;
                        default:
                            break;
                    }
                }
            }
            else if (parm.StartsWith("TextColor2_"))
            {
                string[] strArr = parm.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                if (strArr.Length == 2)
                {
                    string temp = value.ToString();
                    switch (strArr[1])
                    {
                        case "2":
                            return temp.Equals("2") ? Consts.ImageEditButton2Color2 : Consts.ImageEditButton2Color1;
                        case "4":
                            return temp.Equals("4") ? Consts.ImageEditButton2Color2 : Consts.ImageEditButton2Color1;
                        case "5":
                            return temp.Equals("5") ? Consts.ImageEditButton2Color2 : Consts.ImageEditButton2Color1;
                        case "9":
                            return temp.Equals("9") ? Consts.ImageEditButton2Color2 : Consts.ImageEditButton2Color1;
                        default:
                            break;
                    }
                }
            }
            return null;
        }

        //目标属性传给源属性时，调用此方法ConvertBack
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class IsNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string str)
            {
                return string.IsNullOrEmpty(str);
            }
            return (value == null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("IsNullConverter can only be used OneWay.");
        }
    }

    public class CutoffConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int count && parameter is int par)
            {
                return count > par;
            }

            double.TryParse(value.ToString(), out var cutoff);
            double.TryParse(parameter.ToString(), out var cutoffp);

            return cutoff > cutoffp;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class OptionsToIsCheckedCvt : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() == parameter?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (bool.TryParse(value?.ToString(), out var isChecked) &&
                int.TryParse(parameter?.ToString(), out var param))
            {
                if (isChecked)
                {
                    return param;
                }
            }
            return value;
        }
    }

    /// <summary>
    /// 字符串转图片
    /// </summary>
    public class StringToImageSourceConverter : IValueConverter
    {
        #region Converter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = (string)value;
            if (!string.IsNullOrEmpty(path))
            {
                //MessageBox.Show(path);
                return new BitmapImage(new Uri(path, UriKind.Absolute));
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        #endregion
    }

    /// <summary>
    /// 整数返回显示方式
    /// 0:可见Visible
    /// 1:隐藏Hidden
    /// 2:折叠Collapsed
    /// </summary>
    public class IntegerToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int v = -1;
            if (null == value || !int.TryParse(value.ToString(), out v))
            {
                return Visibility.Collapsed;
            }
            if (v < 0 || v > 2)
            {
                return Visibility.Collapsed;
            }
            return (Visibility)Enum.ToObject(typeof(Visibility), v);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 数量转化为答案列表
    /// </summary>
    public class QtyToOptionList : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int v = -1;
            if (null == value || !int.TryParse(value.ToString(), out v))
            {
                return null;
            }
            if (v <= 0)
            {
                return null;
            }
            ASCIIEncoding asciiEncoding = new ASCIIEncoding();
            List<string> list = new List<string>();
            for (int i = 0; i < v; i++)
            {
                int asciiCode = 65 + i;
                byte[] byteArray = new byte[] { (byte)asciiCode };
                string strCharacter = asciiEncoding.GetString(byteArray);
                list.Add(strCharacter);
            }
            return list.ToArray();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 时间戳转字符串
    /// </summary>
    public class LongToHHMMSS : IValueConverter
    {
        public DateTime GetTime(long timeStamp, bool accurateToMilliseconds = false)
        {
            if (accurateToMilliseconds)
            {
                return DateTimeOffset.FromUnixTimeMilliseconds(timeStamp).LocalDateTime;
            }
            else
            {
                return DateTimeOffset.FromUnixTimeSeconds(timeStamp).LocalDateTime;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long v = -1;
            if (null == value || !long.TryParse(value.ToString(), out v))
            {
                return null;
            }
            if (v <= 0)
            {
                return null;
            }
            DateTime dt = GetTime(v, true);
            if (null == dt)
            {
                return null;
            }
            return dt.ToString("HH:mm:ss.fff");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 时间转字符串
    /// </summary>
    public class LongToTime : IValueConverter
    {
        //将秒数转化为时分秒
        private string sec_to_hms(long duration)
        {
            TimeSpan ts = new TimeSpan(0, 0, System.Convert.ToInt32(duration));
            string str = "";
            if (ts.Hours > 0)
            {
                str = string.Format("{0:00}", ts.Hours) + ":" + string.Format("{0:00}", ts.Minutes) + ":" + string.Format("{0:00}", ts.Seconds);
            }
            if (ts.Hours == 0 && ts.Minutes > 0)
            {
                str = "00:" + string.Format("{0:00}", ts.Minutes) + ":" + string.Format("{0:00}", ts.Seconds);
            }
            if (ts.Hours == 0 && ts.Minutes == 0)
            {
                str = "00:00:" + string.Format("{0:00}", ts.Seconds);
            }
            return str;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long v = -1;
            if (null == value || !long.TryParse(value.ToString(), out v))
            {
                return null;
            }
            if (v <= 0)
            {
                return null;
            }
            return sec_to_hms(v);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 声音地址转显示类型
    /// </summary>
    public class AudioUrlToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null == value)
            {
                return Visibility.Collapsed;
            }
            string url = value as string;
            if (string.IsNullOrEmpty(url))
            {
                return Visibility.Collapsed;
            }
            return Visibility.Visible; ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 批量点评
    /// </summary>
    public class CommentBatchToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null != value)
            {
                int _style = -1;
                if (int.TryParse(value.ToString(), out _style))
                {
                    if (0 == _style)
                    {
                        return Visibility.Visible;
                    }
                }
            }
            return Visibility.Collapsed; ;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 学生点评
    /// </summary>
    public class CommentStudentToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null != value)
            {
                int _style = -1;
                if (int.TryParse(value.ToString(), out _style))
                {
                    if (1 == _style)
                    {
                        return Visibility.Visible;
                    }
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 字符串--是否显示
    /// </summary>
    public class StringToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null != value)
            {
                if (!string.IsNullOrEmpty(value.ToString()))
                {
                    return Visibility.Visible;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 字符串转显示控制
    /// </summary>
    public class ScoreToVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int v = -1;
            if (null == value || !int.TryParse(value.ToString(), out v))
            {
                return Visibility.Collapsed;
            }
            if (v > 0)
            {
                return Visibility.Collapsed;
            }
            return (Visibility)Enum.ToObject(typeof(Visibility), v);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 评价类型转文字颜色
    /// 2:#006400  0,100,0
    /// 其它：#008000 0,128,0
    /// </summary>
    public class StudentWorkCommentTypeToBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color = Color.FromArgb(255, 0, 128, 0);
            if (null != value)
            {
                int s = -1;
                if (int.TryParse(value.ToString(), out s) && 2 == s)
                {
                    color = Color.FromArgb(255, 178, 34, 34);
                    return new SolidColorBrush(color);
                }
            }
            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// 评价角色转对齐方式
    /// 0：其它 左对齐
    /// 1：自己 右对齐
    /// </summary>
    public class StudentWorkCommentRoleToHorizontalAlignment : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (null != value)
            {
                int _style = -1;
                if (int.TryParse(value.ToString(), out _style))
                {
                    if (1 == _style)
                    {
                        return System.Windows.HorizontalAlignment.Right;
                    }
                }
            }
            return System.Windows.HorizontalAlignment.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

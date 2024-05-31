using CalligraphyAssistantMain.Code;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
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
    /// SendImagesControl.xaml 的交互逻辑
    /// </summary>
    public partial class SendImagesControl : UserControl
    {
        private bool isBusy = false;
        public SendImagesControl()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            //for (int i = 0; i < 4; i++)
            //{
            //    ImageItemControl2 image = new ImageItemControl2();
            //    image.Title = "主题" + (i + 1);
            //    image.Count = (i + 2);
            //    image.ImageClick += Image_ImageClick; ;
            //    imageWp2.Children.Add(image);
            //}
        }

        private void AddPreview(string[] imagesArr, int[] typeArr = null)
        {
            int index = 0;
            ImageItemControl1[] controls = imageWp1.Children.Cast<FrameworkElement>().Where(p => p is ImageItemControl1).Cast<ImageItemControl1>().ToArray();
            foreach (string item in imagesArr)
            {
                if (controls.Any(p => p.ImagePath.Equals(item, StringComparison.OrdinalIgnoreCase)))
                {
                    index++;
                    continue;
                }
                if (File.Exists(item))
                {
                    ImageItemControl1 image = new ImageItemControl1();
                    image.DeleteClick += image_DeleteClick;
                    image.SetImage(item, typeArr == null ? 0 : typeArr[index]);
                    imageWp1.Children.Insert(imageWp1.Children.Count - 1, image);
                }
                index++;
            }
        }

        private void GetThemes()
        {
            if (isBusy)
            {
                return;
            }
            isBusy = true;
            imageWp2.Children.Clear();
            NameValueCollection dict = new NameValueCollection();
            dict.Add("token", Common.CurrentUser.Token);
            Task.Run(() =>
            {
                string jsonResult = HttpUtility.DownloadString(Common.GetImageThemes, Encoding.UTF8, dict);
                ResultInfo<List<ImageThemeInfo>> result = JsonConvert.DeserializeObject<ResultInfo<List<ImageThemeInfo>>>(jsonResult);
                if (result != null)
                {
                    if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        string themePath = Common.SettingsPath + "Theme\\";
                        if (!Directory.Exists(themePath))
                        {
                            Directory.CreateDirectory(themePath);
                        }
                        this.Dispatcher.Invoke(() =>
                        {
                            foreach (ImageThemeInfo item in result.Body)
                            {
                                ImageItemControl2 image = new ImageItemControl2();
                                image.Title = item.Name;
                                image.Count = item.Count;
                                image.Tag = item;
                                image.SetImage(item.Url);
                                image.ImageClick += Image_ImageClick;
                                image.MouseDoubleClick += Image_MouseDoubleClick;
                                imageWp2.Children.Add(image);
                            }
                        });
                    }
                }
                isBusy = false;
            });
        }

        private void GetThemeDetails(int id)
        {
            if (isBusy)
            {
                return;
            }
            isBusy = true;
            imageWp3.Children.Clear();
            NameValueCollection dict = new NameValueCollection();
            dict.Add("token", Common.CurrentUser.Token);
            Task.Run(() =>
            {
                string jsonResult = HttpUtility.DownloadString(Common.GetImageThemeDetails + id, Encoding.UTF8, dict);
                ResultInfo<List<ImageThemeItemInfo>> result = JsonConvert.DeserializeObject<ResultInfo<List<ImageThemeItemInfo>>>(jsonResult);
                if (result != null)
                {
                    if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        string themePath = Common.SettingsPath + "Theme\\" + id + "\\";
                        if (!Directory.Exists(themePath))
                        {
                            Directory.CreateDirectory(themePath);
                        }
                        this.Dispatcher.Invoke(() =>
                        {
                            foreach (ImageThemeItemInfo item in result.Body)
                            {
                                //string savePath = themePath + item.Id + System.IO.Path.GetExtension(item.Url);
                                string savePath = themePath + item.Id + System.IO.Path.GetFileName(item.Url);
                                ImageItemControl4 image = new ImageItemControl4();
                                image.Tag = item;
                                image.SetImage(item.Url, savePath);
                                imageWp3.Children.Add(image);
                            }
                        });
                    }
                }
                isBusy = false;
            });
        }

        private void SetImageMode()
        {
            Common.SocketServer.ShowImageMode();
            Common.SocketServer.StopShareScreen();
            Common.SocketServer.StopLiveTransfer();
            Common.NavigationInfo.SelectedShareMenu = 4;
        }

        private void Image_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            imageWp3.Visibility = Visibility.Visible;
            imageWp2.Visibility = Visibility.Collapsed;
            ImageThemeInfo info = (sender as ImageItemControl2).Tag as ImageThemeInfo;
            GetThemeDetails(info.Id);
        }

        private void image_DeleteClick(object sender, EventArgs e)
        {
            try
            {
                imageWp1.Children.Remove(sender as ImageItemControl1);
            }
            catch
            {
            }
        }

        private void Image_ImageClick(object sender, EventArgs e)
        {
            ImageItemControl2 image = sender as ImageItemControl2;
            if (!image.IsSelected)
            {
                imageWp2.Children.Cast<ImageItemControl2>().ToList().ForEach(p => p.IsSelected = false);
                image.IsSelected = true;
            }
        }

        private void sendBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            string[] imageArr = null;
            int[] typeArr = null;
            if (imageWp1.Visibility == Visibility.Visible)
            {
                if (imageWp1.Children.Count == 1)
                {
                    MessageBoxEx.ShowInfo("请至少选择一张图片！");
                    return;
                }
                ImageItemControl1[] controls = imageWp1.Children.Cast<FrameworkElement>().Where(p => p is ImageItemControl1).Cast<ImageItemControl1>().ToArray();
                imageArr = controls.Select(p => p.ImagePath).ToArray();
                typeArr = controls.Select(p => p.Type).ToArray();
            }
            else
            {
                ImageItemControl4[] controls = imageWp3.Children.Cast<FrameworkElement>().Where(p => p is ImageItemControl4).Cast<ImageItemControl4>().Where(p => p.IsChecked).ToArray();
                if (controls.Length == 0)
                {
                    MessageBoxEx.ShowInfo("请至少选择一张图片！");
                    return;
                }
                imageArr = controls.Select(p => p.ImagePath).ToArray();
                typeArr = controls.Select(p => 0).ToArray();
            }
            //if (Common.AxCCOCX != null)
            //{
            //    for (int i = 0; i < imageArr.Length; i++)
            //    {
            //        Common.AxCCOCX.SendPicture(i, imageArr[i], (int)PicShowMod.adaptive);
            //    }
            Common.SocketServer.ClearImageList();
            for (int i = 0; i < imageArr.Length; i++)
            {
                Common.SocketServer.SendImage(imageArr[i], i, typeArr[i], i == 0);
            }
            //}
            SetImageMode();
            this.Visibility = Visibility.Collapsed;
        }

        private void clearBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            while (imageWp1.Children.Count > 1)
            {
                imageWp1.Children.RemoveAt(0);
            }
        }

        private void cancelBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void imageBtn1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            imageBtn1.Source = Consts.DefaultImageButton2;
            imageBtn2.Source = Consts.DefaultImageButton1;
            imageWp1.Visibility = Visibility.Visible;
            imageWp2.Visibility = Visibility.Collapsed;
            imageWp3.Visibility = Visibility.Collapsed;
            imageWp3.Children.Clear();
            clearGd.Visibility = Visibility.Visible;
        }

        private void imageBtn2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //imageBtn1.Source = Consts.DefaultImageButton1;
            //imageBtn2.Source = Consts.DefaultImageButton2;
            //imageWp1.Visibility = Visibility.Collapsed;
            //imageWp2.Visibility = Visibility.Visible;
            //imageWp3.Visibility = Visibility.Collapsed;
            //clearGd.Visibility = Visibility.Collapsed;
            calligraphyListControl.Show(9 - imageWp1.Children.Count);
            //GetThemes();
        }

        private void addImageBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择分享的字帖";
            openFileDialog.Filter = "支持的类型(*.png;*.jpg;*.gif;*.bmp)|*.png;*.jpg;*.gif;*.bmp";
            openFileDialog.Multiselect = true;
            if ((bool)openFileDialog.ShowDialog())
            {
                if (openFileDialog.FileNames.Length + imageWp1.Children.Count > 9)
                {
                    MessageBoxEx.ShowInfo("最多支持同时发送8张字帖！");
                    return;
                }
                AddPreview(openFileDialog.FileNames, null);
            }
        }

        private void calligraphyListControl_CalligraphyListSelected(object sender, EventArgs e)
        {
            List<CalligraphyPickImageInfo> list = calligraphyListControl.GetSelectedList();
            List<string> imageList = new List<string>();
            List<int> typeList = new List<int>();
            foreach (var item in list)
            {
                if (item.Mode == CalligraphyMode.Word)
                {
                    imageList.Add(item.LocalPath);
                    typeList.Add(1);
                    //string savePath = item.LocalPath + $"x{item.ClipCount}.jpg"; 
                    //string backImagePath = null;
                    //if (item.ClipCount != 1)
                    //{
                    //    backImagePath = Common.AppPath + (item.ClipCount == 4 ? "x4.jpg" : "x8.jpg");
                    //}
                    //bool result = Common.MergeImage(item.LocalPath, savePath, 1920, 1080, item.ClipCount, backImagePath);
                    //if (result)
                    //{
                    //    imageList.Add(savePath);
                    //}
                }
                else
                {
                    imageList.Add(item.LocalPath);
                    typeList.Add(0);
                }
            }
            AddPreview(imageList.ToArray(), typeList.ToArray());
        }
    }
}

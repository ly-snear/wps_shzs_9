using CalligraphyAssistantMain.Code;
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
    /// CalligraphyListControl.xaml 的交互逻辑
    /// </summary>
    public partial class CalligraphyListControl : UserControl
    {
        public event EventHandler CalligraphyListSelected = null;
        public CalligraphyMode CalligraphyMode { get; private set; } = CalligraphyMode.Image;
        private int pageIndex1 = 0;
        private int pageIndex2 = 0;
        private int pageSize = 8;
        private bool isInited = false;
        private int maxImage = 0;
        private List<CalligraphyPickImageInfo> selectedImageList = new List<CalligraphyPickImageInfo>();
        public CalligraphyListControl()
        {
            InitializeComponent();
            wordCountWp.Visibility = Visibility.Collapsed;
        }

        public void Show(int maxImage)
        {
            if (!isInited)
            {
                isInited = true;
                Init();
                Search(1);
            }
            this.maxImage = maxImage;
            selectedLb.Text = $"已选择(0/{maxImage})";
            selectedImageList = new List<CalligraphyPickImageInfo>();
            CheckImageControlSelected();
            CheckWordControlSelected();
            this.Visibility = Visibility.Visible;
        }

        public List<CalligraphyPickImageInfo> GetSelectedList()
        {
            return selectedImageList;
        }

        private void Init()
        {
            dynastyWp.Children.Clear();
            authorWp.Children.Clear();
            fontWp.Children.Clear();
            if (Common.DynastyList != null)
            {
                RadioButton radioButton = new RadioButton()
                {
                    Content = "无",
                    Style = FindResource("TextRadioButtonStyle") as Style,
                    IsChecked = true
                };
                radioButton.Checked += (x, y) => { BindAuthorList(((RadioButton)x).Content.ToString()); };
                dynastyWp.Children.Add(radioButton);
                foreach (var item in Common.DynastyList)
                {
                    radioButton = new RadioButton()
                    {
                        Content = item.Title,
                        Style = FindResource("TextRadioButtonStyle") as Style,
                    };
                    radioButton.Checked += (x, y) => { BindAuthorList(((RadioButton)x).Content.ToString()); };
                    dynastyWp.Children.Add(radioButton);
                }
            }
            if (Common.FontList != null)
            {
                fontWp.Children.Add(new RadioButton()
                {
                    Content = "无",
                    Style = FindResource("TextRadioButtonStyle") as Style,
                    IsChecked = true
                });
                foreach (var item in Common.FontList)
                {
                    fontWp.Children.Add(new RadioButton()
                    {
                        Content = item.Title,
                        Style = FindResource("TextRadioButtonStyle") as Style,
                    });
                }
            }
            authorWp.Children.Add(new RadioButton()
            {
                Content = "无",
                Style = FindResource("TextRadioButtonStyle") as Style,
                IsChecked = true
            });
        }

        private void BindAuthorList(string dynasty)
        {
            authorWp.Children.Clear();
            authorWp.Children.Add(new RadioButton()
            {
                Content = "无",
                Style = FindResource("TextRadioButtonStyle") as Style,
                IsChecked = true
            });
            var authorList = Common.AuthorList.Where(p => p.Dynasty == dynasty);
            foreach (var item in authorList)
            {
                authorWp.Children.Add(new RadioButton()
                {
                    Content = item.Title,
                    Style = FindResource("TextRadioButtonStyle") as Style,
                });
            }
        }

        private void Search(int pageIndex)
        {
            string dynasty = dynastyWp.Children.Cast<RadioButton>().FirstOrDefault(p => (bool)p.IsChecked).Content.ToString();
            string author = authorWp.Children.Cast<RadioButton>().FirstOrDefault(p => (bool)p.IsChecked).Content.ToString();
            string font = fontWp.Children.Cast<RadioButton>().FirstOrDefault(p => (bool)p.IsChecked).Content.ToString();
            NameValueCollection headerDict = new NameValueCollection();
            headerDict.Add("token", Common.CurrentUser.Token);
            string jsonResult = null;
            if (CalligraphyMode == CalligraphyMode.Image)
            {
                CalligraphyImageSearchInfo calligraphyImageSearchInfo = new CalligraphyImageSearchInfo()
                {
                    Title = searchTxt1.Text.Trim(),
                    Author = (author == "无" ? string.Empty : author),
                    Font = (font == "无" ? string.Empty : font),
                    Dynasty = (dynasty == "无" ? string.Empty : dynasty),
                    Size = pageSize,
                    Page = pageIndex
                };
                jsonResult = HttpUtility.UploadValuesJson(Common.CalligraphyImageList, calligraphyImageSearchInfo, Encoding.UTF8, Encoding.UTF8, headerDict);
                ResultInfo<ResultCalligraphyListInfo<CalligraphyImageDetailsInfo[]>> result = JsonConvert.DeserializeObject<ResultInfo<ResultCalligraphyListInfo<CalligraphyImageDetailsInfo[]>>>(jsonResult);
                BindCalligraphyImageList(result.Body);
                CheckImageControlSelected();
            }
            else
            {
                if (string.IsNullOrEmpty(searchTxt2.Text))
                {
                    MessageBoxEx.ShowInfo("请输入要搜索的文字！");
                    return;
                }
                CalligraphyWordSearchInfo calligraphyWordSearchInfo = new CalligraphyWordSearchInfo()
                {
                    Word = searchTxt2.Text.Trim(),
                    Author = (author == "无" ? string.Empty : author),
                    Font = (font == "无" ? string.Empty : font),
                    Dynasty = (dynasty == "无" ? string.Empty : dynasty),
                    Size = pageSize,
                    Page = pageIndex
                };
                jsonResult = HttpUtility.UploadValuesJson(Common.CalligraphyWordList, calligraphyWordSearchInfo, Encoding.UTF8, Encoding.UTF8, headerDict);
                ResultInfo<ResultCalligraphyListInfo<CalligraphyWordDetailsInfo[]>> result = JsonConvert.DeserializeObject<ResultInfo<ResultCalligraphyListInfo<CalligraphyWordDetailsInfo[]>>>(jsonResult);
                BindCalligraphyWordList(result.Body);
            }
        }

        private void BindCalligraphyImageList(ResultCalligraphyListInfo<CalligraphyImageDetailsInfo[]> result)
        {
            string themePath = Common.SettingsPath + "虎妞书画\\Image\\";
            if (!Directory.Exists(themePath))
            {
                Directory.CreateDirectory(themePath);
            }
            imageWp.Children.Clear();
            foreach (var item in result.Data)
            {
                ImageItemControl5 imageItemControl = new ImageItemControl5();
                imageItemControl.DataContext = item;
                imageItemControl.ImageClick += ImageItemControl_ImageClick;
                int index = 0;
                foreach (var cover in item.CoverArr)
                {
                    string savePath = $"{themePath}{item.DynastyId}\\{item.AuthorId}\\";
                    if (!Directory.Exists(savePath))
                    {
                        Directory.CreateDirectory(savePath);
                    }
                    string url = cover.Url.Replace("\\", string.Empty);
                    savePath = $"{savePath}{item.Id}-{cover.Order}{System.IO.Path.GetFileName(url)}";
                    if (index == 0)
                    {
                        imageItemControl.SetImage(url, savePath);
                    }
                    index++;
                }
                imageWp.Children.Add(imageItemControl);
            }
            pageLb1.Text = result.Page.Page + "/" + result.Page.Pages;
            pageIndex1 = result.Page.Page;
        }

        private void CheckImageControlSelected()
        {
            foreach (ImageItemControl5 item in imageWp.Children)
            {
                CalligraphyImageDetailsInfo wordDetailsInfo = item.DataContext as CalligraphyImageDetailsInfo;
                item.SetHighlight(selectedImageList.Any(p => wordDetailsInfo.CoverArr != null && wordDetailsInfo.CoverArr.Any(q => q.Url == p.Url)));
            }
        }

        private void CheckWordControlSelected()
        {
            foreach (ImageItemControl6 item in wordWp.Children)
            {
                CalligraphyWordDetailsInfo wordDetailsInfo = item.DataContext as CalligraphyWordDetailsInfo;
                item.SetChecked(selectedImageList.Any(p => p.Url == wordDetailsInfo.Url));
            }
        }

        private void BindCalligraphyWordList(ResultCalligraphyListInfo<CalligraphyWordDetailsInfo[]> result)
        {
            string themePath = Common.SettingsPath + "虎妞书画\\Word\\";
            if (!Directory.Exists(themePath))
            {
                Directory.CreateDirectory(themePath);
            }
            wordWp.Children.Clear();
            foreach (var item in result.Data)
            {
                ImageItemControl6 imageItemControl = new ImageItemControl6();
                imageItemControl.DataContext = item;
                imageItemControl.SetChecked(selectedImageList.Any(p => p.Url == item.Url));
                imageItemControl.CheckChanged += ImageItemControl_CheckChanged; ;
                string savePath = $"{themePath}{item.DynastyId}\\{item.AuthorId}\\";
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                string url = item.Url.Replace("\\", string.Empty);
                savePath = $"{savePath}{item.Id}-{System.IO.Path.GetFileName(url)}";
                item.FullTitle = $"{item.Word} {item.Author} {item.Dynasty} {item.Font}";
                item.LocalPath = savePath;
                imageItemControl.SetImage(url, savePath);
                wordWp.Children.Add(imageItemControl);
            }
            pageLb2.Text = result.Page.Page + "/" + result.Page.Pages;
            pageIndex2 = result.Page.Page;
        }

        private void ImageItemControl_CheckChanged(object sender, EventArgs e)
        {
            if (CalligraphyMode == CalligraphyMode.Word)
            {
                ImageItemControl6 imageItemControl = sender as ImageItemControl6;
                if (imageItemControl != null && imageItemControl.DataContext is CalligraphyWordDetailsInfo detailsInfo)
                {
                    if (imageItemControl.IsChecked)
                    {
                        if (selectedImageList.Count >= maxImage)
                        {
                            imageItemControl.SetChecked(false);
                            return;
                        }
                        if (!selectedImageList.Any(p => p.Url == detailsInfo.Url))
                        {
                            selectedImageList.Add(new CalligraphyPickImageInfo() { LocalPath = detailsInfo.LocalPath, Url = detailsInfo.Url, Mode = CalligraphyMode.Word, Title = detailsInfo.FullTitle });
                        }
                    }
                    else
                    {
                        selectedImageList.RemoveAll(p => p.Url == detailsInfo.Url);
                    }
                    selectedLb.Text = $"已选择({selectedImageList.Count}/{maxImage})";
                }
            }
        }

        private void ImageItemControl_ImageClick(object sender, EventArgs e)
        {
            if (CalligraphyMode == CalligraphyMode.Image)
            {
                ImageItemControl5 imageItemControl = sender as ImageItemControl5;
                if (imageItemControl != null && imageItemControl.DataContext is CalligraphyImageDetailsInfo detailsInfo)
                {
                    calligraphyListDetailsControl.ShowImage(detailsInfo, selectedImageList, 0);
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void okBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectedImageList.ForEach(p => p.ClipCount = (bool)x1Rb.IsChecked ? 1 : ((bool)x4Rb.IsChecked ? 4 : 8));
            if (CalligraphyListSelected != null)
            {
                CalligraphyListSelected(this, null);
            }
            this.Visibility = Visibility.Collapsed;
        }

        private void cancelBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void searchBtn1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Search(1);
        }

        private void imageBtn1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            imageBtn1.Source = Consts.DefaultImageButton2;
            imageBtn2.Source = Consts.DefaultImageButton1;
            imageGd.Visibility = searchTxt1.Visibility = pageLb1.Visibility = Visibility.Visible;
            //wordGd.Visibility = searchTxt2.Visibility = pageLb2.Visibility = wordCountWp.Visibility = Visibility.Collapsed;
            wordGd.Visibility = searchTxt2.Visibility = pageLb2.Visibility = Visibility.Collapsed;
            CalligraphyMode = CalligraphyMode.Image;
            searchLb.Text = "碑帖名称：";
        }

        private void imageBtn2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            imageBtn1.Source = Consts.DefaultImageButton1;
            imageBtn2.Source = Consts.DefaultImageButton2;
            imageGd.Visibility = searchTxt1.Visibility = pageLb1.Visibility = Visibility.Collapsed;
            //wordGd.Visibility = searchTxt2.Visibility = pageLb2.Visibility = wordCountWp.Visibility = Visibility.Visible;
            wordGd.Visibility = searchTxt2.Visibility = pageLb2.Visibility = Visibility.Visible;
            CalligraphyMode = CalligraphyMode.Word;
            searchLb.Text = "单字名称：";
        }

        private void previousBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CalligraphyMode == CalligraphyMode.Image)
            {
                Search(--pageIndex1);
            }
            else
            {
                Search(--pageIndex2);
            }
        }

        private void nextBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (CalligraphyMode == CalligraphyMode.Image)
            {
                Search(++pageIndex1);
            }
            else
            {
                Search(++pageIndex2);
            }
        }

        private void selectedBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selectedImageList.Count > 0)
            {
                calligraphyListDetailsControl.PickImage(selectedImageList, 0);
            }
        }

        private void calligraphyListDetailsControl_DetilsListChanged(object sender, EventArgs e)
        {
            List<CalligraphyPickImageInfo> tempList = calligraphyListDetailsControl.GetSelectedPickList();
            if (calligraphyListDetailsControl.IsPickMode)
            {
                selectedImageList.Clear();
                selectedImageList.AddRange(tempList);
            }
            else
            {
                CalligraphyImageDetailsInfo detailsInfo = calligraphyListDetailsControl.LastImageDetails;
                foreach (CalligraphyPickImageInfo item in tempList)
                {
                    if (!selectedImageList.Any(p => p.Url == item.Url))
                    {
                        selectedImageList.Add(item);
                    }
                }
                foreach (CalligraphyImageDetailsCoverInfo item in detailsInfo.CoverArr)
                {
                    if (!tempList.Any(p => p.Url == item.Url))
                    {
                        selectedImageList.RemoveAll(p => p.Url == item.Url);
                    }
                }

            }
            CheckImageControlSelected();
            CheckWordControlSelected();
            //selectedImageList.AddRange(tempList.Where(p => !selectedImageList.Any(q => q.Url == p.Url)));
            if (selectedImageList.Count > maxImage)
            {
                selectedImageList = selectedImageList.Take(maxImage).ToList();
            }
            selectedLb.Text = $"已选择({selectedImageList.Count}/{maxImage})";
        }
    }
}

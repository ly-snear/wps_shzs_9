using CalligraphyAssistantMain.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
    /// CalligraphyListDetailsControl.xaml 的交互逻辑
    /// </summary>
    public partial class CalligraphyListDetailsControl : UserControl
    {
        public event EventHandler DetilsListChanged = null;
        public bool IsPickMode { get; private set; } = false;
        public CalligraphyImageDetailsInfo LastImageDetails { get; private set; } = null;
        private int selectedImageIndex = 0;
        private List<CalligraphyImageDetailsCoverInfo> selectedCoverImageList = null;
        private List<CalligraphyPickImageInfo> selectedPickImageList = null;
        private List<CalligraphyPickImageInfo> sourcePickImageList = null;
        private bool isDownloading = false;
        public CalligraphyListDetailsControl()
        {
            InitializeComponent();
            this.DataContext = null;
        }

        public void ShowImage(CalligraphyImageDetailsInfo detailsInfo, List<CalligraphyPickImageInfo> selectedImageList, int index)
        {
            this.LastImageDetails = detailsInfo;
            this.selectedImageIndex = index;
            IsPickMode = false;
            selectedCoverImageList = detailsInfo.CoverArr.Where(p => selectedImageList.Any(q => q.Url == p.Url)).ToList();
            titleLb.Text = $"{detailsInfo.Title} {detailsInfo.Author} {detailsInfo.Dynasty} {detailsInfo.Font}";
            ShowImage(index);
            this.Visibility = Visibility.Visible;
        }

        public void PickImage(List<CalligraphyPickImageInfo> selectedImageList, int index)
        {
            sourcePickImageList = selectedImageList.ToList();
            selectedPickImageList = selectedImageList.ToList();
            this.selectedImageIndex = index;
            IsPickMode = true;
            PickImage(index);
            this.Visibility = Visibility.Visible;
        }

        public List<CalligraphyPickImageInfo> GetSelectedPickList()
        {
            if (IsPickMode)
            {
                return selectedPickImageList;
            }
            return selectedCoverImageList.Select(p => new CalligraphyPickImageInfo() { Title = p.FullTitle, LocalPath = p.LocalPath, Url = p.Url, Mode = CalligraphyMode.Image }).ToList();
        }

        private void ShowImage(int index)
        {
            isDownloading = true;
            string themePath = Common.SettingsPath + "虎妞书画\\Image\\";
            if (!Directory.Exists(themePath))
            {
                Directory.CreateDirectory(themePath);
            }
            string savePath = $"{themePath}{LastImageDetails.DynastyId}\\{LastImageDetails.AuthorId}\\";
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }
            CalligraphyImageDetailsCoverInfo cover = LastImageDetails.CoverArr[index];
            cover.FullTitle = titleLb.Text + " - " + (index + 1);
            string url = cover.Url.Replace("\\", string.Empty);
            savePath = $"{savePath}{LastImageDetails.Id}-{cover.Order}{System.IO.Path.GetFileName(url)}";
            WebClient webClient = new WebClient();
            try
            {
                if (File.Exists(savePath))
                {
                    cover.LocalPath = savePath;
                    image.Source = new BitmapImage(new Uri(savePath));
                    image.Tag = cover;
                    this.selectedImageIndex = index;
                    pageLb.Text = (index + 1) + "/" + LastImageDetails.ImageCount;
                    checkBd.Visibility = selectedCoverImageList.Any(p => p == cover) ? Visibility.Visible : Visibility.Collapsed;
                    isDownloading = false;
                }
                else
                {
                    string tempPath = savePath + ".temp";
                    if (File.Exists(tempPath))
                    {
                        File.Delete(tempPath);
                    }
                    webClient.DownloadFileAsync(new Uri(url), tempPath);
                    webClient.DownloadFileCompleted += (x, y) =>
                    {
                        webClient.Dispose();
                        File.Move(tempPath, savePath);
                        this.Dispatcher.InvokeAsync(() =>
                        {
                            try
                            {
                                cover.LocalPath = savePath;
                                image.Source = new BitmapImage(new Uri(savePath));
                                this.selectedImageIndex = index;
                                pageLb.Text = (index + 1) + "/" + LastImageDetails.ImageCount;
                                checkBd.Visibility = selectedCoverImageList.Any(p => p == cover) ? Visibility.Visible : Visibility.Collapsed;
                            }
                            catch (Exception ex1)
                            {
                                Common.Trace("CalligraphyListDetailsControl SetImage Error1:" + ex1.Message);
                            }
                            isDownloading = false;
                        });
                    };
                }
            }
            catch (Exception ex)
            {
                webClient.Dispose();
                isDownloading = false;
                Common.Trace("CalligraphyListDetailsControl SetImage Error2:" + ex.Message);
            }
        }

        private void PickImage(int index)
        {
            CalligraphyPickImageInfo info = sourcePickImageList[index];
            titleLb.Text = info.Title;
            try
            {
                if (File.Exists(info.LocalPath))
                {
                    image.Source = new BitmapImage(new Uri(info.LocalPath));
                    image.Tag = info;
                    this.selectedImageIndex = index;
                    pageLb.Text = (index + 1) + "/" + sourcePickImageList.Count;
                    checkBd.Visibility = sourcePickImageList.Any(p => p == info) ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                Common.Trace("CalligraphyListDetailsControl PickImage Error:" + ex.Message);
            }
        }

        private void cancelBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            if (selectedCoverImageList != null)
            {
                selectedCoverImageList.Clear();
            }
        }


        private void previousBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsPickMode)
            {
                if (selectedImageIndex > 0)
                {
                    PickImage(selectedImageIndex - 1);
                }
            }
            else
            {
                if (!isDownloading && selectedImageIndex > 0)
                {
                    ShowImage(selectedImageIndex - 1);
                }
            }
        }

        private void nextBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsPickMode)
            {
                if (selectedImageIndex + 1 < selectedPickImageList.Count)
                {
                    PickImage(selectedImageIndex + 1);
                }
            }
            else
            {
                if (!isDownloading && selectedImageIndex + 1 < LastImageDetails.ImageCount)
                {
                    ShowImage(selectedImageIndex + 1);
                }
            }
        }

        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsPickMode)
            {
                if (checkBd.Visibility == Visibility.Collapsed)
                {
                    checkBd.Visibility = Visibility.Visible;
                    selectedPickImageList.Add(image.Tag as CalligraphyPickImageInfo);
                }
                else
                {
                    checkBd.Visibility = Visibility.Collapsed;
                    selectedPickImageList.Remove(image.Tag as CalligraphyPickImageInfo);
                }
            }
            else
            {
                if (checkBd.Visibility == Visibility.Collapsed)
                {
                    checkBd.Visibility = Visibility.Visible;
                    selectedCoverImageList.Add(image.Tag as CalligraphyImageDetailsCoverInfo);
                }
                else
                {
                    checkBd.Visibility = Visibility.Collapsed;
                    selectedCoverImageList.Remove(image.Tag as CalligraphyImageDetailsCoverInfo);
                }
            }
        }

        private void selectBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DetilsListChanged != null)
            {
                DetilsListChanged(this, null);
            }
            this.Visibility = Visibility.Collapsed;
        }
    }
}
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
    /// ImageEditControl.xaml 的交互逻辑
    /// </summary>
    public partial class ImageEditControl : UserControl
    {
        private ImageEditInfo imageEditInfo = new ImageEditInfo();
        private int selectedImageIndex = 0;
        private StudentWorkDetailsInfo[] workArr = null;
        private bool isBusy = false;
        private bool hasChanged = false;
        public event EventHandler Back = null;
        public ImageEditControl()
        {
            InitializeComponent();
            this.DataContext = imageEditInfo;
            drawBoardCanvas.DrawMode = DrawMode.Pen;
        }

        public void EditImages(StudentWorkDetailsInfo[] workArr, int index)
        {
            this.workArr = workArr;
            this.selectedImageIndex = index;
            ShowImage(index);
            this.Visibility = Visibility.Visible;
        }

        private void ShowImage(int index)
        {
            if (hasChanged || drawBoardCanvas.CanSave())
            {
                if (MessageBoxEx.ShowQuestion(Window.GetWindow(this), "是否保存本次批改？", "提示") == MessageBoxResult.Yes)
                {
                    drawBoardCanvas.StopEdit();
                    ImageSource image = drawBoardCanvas.GetImage(false);
                    CorrectWork(image);
                }
            }
            ClearDrawBoard();
            hasChanged = false;
            BitmapImage bitmapImage = new BitmapImage(new Uri(workArr[index].LocalPath, UriKind.Absolute));
            //if (!string.IsNullOrEmpty(workArr[index].Correct))
            //{
            //    bitmapImage = new BitmapImage(new Uri(workArr[index].Correct, UriKind.Absolute));
            //}
            //else
            //{
            //    bitmapImage = new BitmapImage(new Uri(workArr[index].LocalPath, UriKind.Absolute));
            //}
            ImageBrush imageBrush = new ImageBrush(bitmapImage) { Stretch = Stretch.Uniform };
            drawBoardCanvas.Background = imageBrush;
            this.selectedImageIndex = index;
            studentInfoLb.Text = workArr[index].ClassName + " - " + workArr[index].StudentName;
            tagWp.Children.Clear();
            if (!string.IsNullOrEmpty(workArr[index].Comment))
            {
                string[] strArr = workArr[index].Comment.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in strArr)
                {
                    ImageTagInfo info = new ImageTagInfo() { Title = item };
                    ImageTagControl control = new ImageTagControl() { TagInfo = info, DataContext = info };
                    control.CloseClick += Control_CloseClick;
                    tagWp.Children.Add(control);
                }
            }
            pageLb.Text = (index + 1) + "/" + workArr.Length;
        }

        private void CorrectWork(ImageSource image)
        {
            if (isBusy)
            {
                return;
            }
            isBusy = true;
            string tempPath = Common.SettingsPath + "Temp\\";
            string savePath = tempPath + Guid.NewGuid() + ".png";
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }
            if (!Common.SaveImage(image, savePath))
            {
                isBusy = false;
                MessageBoxEx.ShowError("批改保存失败！", Window.GetWindow(this));
                return;
            }
            string imageUrl = Common.UploadImageV2(savePath, workArr[selectedImageIndex].Id);
            if (string.IsNullOrEmpty(imageUrl))
            {
                isBusy = false;
                MessageBoxEx.ShowError("批改上传失败！", Window.GetWindow(this));
                return;
            }

            NameValueCollection dict = new NameValueCollection();
            NameValueCollection data = new NameValueCollection();
            dict.Add("token", Common.CurrentUser.Token);
            data.Add("id", workArr[selectedImageIndex].Id.ToString());
            data.Add("score", "0");
            data.Add("correct", imageUrl);
            data.Add("comment", string.Join(",", tagWp.Children.Cast<ImageTagControl>().Select(p => p.TagInfo.Title).ToArray()));
            string jsonResult = HttpUtility.UploadValues(Common.CorrectWork, data, Encoding.UTF8, Encoding.UTF8, dict);
            ResultInfo<StudentWorkDetailsInfo> result = JsonConvert.DeserializeObject<ResultInfo<StudentWorkDetailsInfo>>(jsonResult);

            if (result != null)
            {
                if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    workArr[selectedImageIndex].Correct = result.Body.Correct;
                    workArr[selectedImageIndex].Comment = result.Body.Comment;
                    string workPath = Common.SettingsPath + "Work\\";
                    string newPath = workPath + System.IO.Path.GetFileName(imageUrl);
                    File.Copy(savePath, newPath);
                    workArr[selectedImageIndex].LocalPath = newPath;
                    if (this.Visibility == Visibility.Visible)
                    {
                        MessageBoxEx.ShowInfo("批改已上传！", Window.GetWindow(this));
                    }
                }
                else
                {
                    if (this.Visibility == Visibility.Visible)
                    {
                        MessageBoxEx.ShowError(result.Msg, Window.GetWindow(this));
                    }
                }
            }
            else
            {
                if (this.Visibility == Visibility.Visible)
                {
                    MessageBoxEx.ShowError("批改上传失败！", Window.GetWindow(this));
                }
            }
            hasChanged = false;
            isBusy = false;
        }

        private void ResetColorRectWidth(Rectangle without)
        {
            foreach (var item in colorWp.Children)
            {
                if (item is Rectangle && item != without)
                {
                    (item as Rectangle).Height = 30;
                }
            }
        }

        private void ClearDrawBoard()
        {
            drawBoardCanvas.StopEdit();
            drawBoardCanvas.ClearDrawBoard();
        }

        private void Rotation()
        {
            try
            {
                if (drawBoardCanvas.Background != null)
                {
                    ClearDrawBoard();
                    ImageBrush imageBrush = drawBoardCanvas.Background as ImageBrush;
                    BitmapImage image = imageBrush.ImageSource as BitmapImage;
                    BitmapImage newImage = new BitmapImage();
                    newImage.BeginInit();
                    if (image.Rotation == System.Windows.Media.Imaging.Rotation.Rotate0)
                    {
                        newImage.Rotation = System.Windows.Media.Imaging.Rotation.Rotate90;
                    }
                    else if (image.Rotation == System.Windows.Media.Imaging.Rotation.Rotate90)
                    {
                        newImage.Rotation = System.Windows.Media.Imaging.Rotation.Rotate180;
                    }
                    else if (image.Rotation == System.Windows.Media.Imaging.Rotation.Rotate180)
                    {
                        newImage.Rotation = System.Windows.Media.Imaging.Rotation.Rotate270;
                    }
                    else
                    {
                        newImage.Rotation = System.Windows.Media.Imaging.Rotation.Rotate0;
                    }
                    newImage.UriSource = image.UriSource;
                    newImage.EndInit();
                    imageBrush.ImageSource = newImage;
                }
                hasChanged = true;
            }
            catch
            {
            }
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int tag = Convert.ToInt32((sender as Grid).Tag);
            if (imageEditInfo.SelectedMenu == 0)
            {
                imageEditInfo.SelectedMenu = tag;
            }
            else
            {
                if (imageEditInfo.SelectedMenu == tag)
                {
                    imageEditInfo.SelectedMenu = 0;
                }
                else
                {
                    imageEditInfo.SelectedMenu = tag;
                }
            }
        }

        private void GraphicMenu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isBusy)
            {
                MessageBoxEx.ShowInfo("正在上传作业批改，请稍候！");
                return;
            }
            int tag = Convert.ToInt32((sender as Grid).Tag);
            switch (tag)
            {
                case 1://旋转
                    Rotation();
                    break;
                case 2://铅笔
                    drawBoardCanvas.DrawMode = DrawMode.Pen;
                    imageEditInfo.SelectedGraphicMenu = 0;
                    imageEditInfo.SelectedImageEditMenu = tag;
                    break;
                case 3://颜色 
                    colorPop.IsOpen = true;
                    break;
                case 4://文本
                    drawBoardCanvas.DrawMode = DrawMode.Text;
                    imageEditInfo.SelectedGraphicMenu = 0;
                    imageEditInfo.SelectedImageEditMenu = tag;
                    break;
                case 5://橡皮
                    drawBoardCanvas.DrawMode = DrawMode.Eraser;
                    imageEditInfo.SelectedGraphicMenu = 0;
                    imageEditInfo.SelectedImageEditMenu = tag;
                    break;
                case 6://清除
                    drawBoardCanvas.ClearDrawBoard();
                    break;
                case 7://保存
                    drawBoardCanvas.StopEdit();
                    ImageSource image = drawBoardCanvas.GetImage(false);
                    CorrectWork(image);
                    break;
                case 8://返回
                    if (hasChanged || drawBoardCanvas.CanSave())
                    {
                        if (MessageBoxEx.ShowQuestion(Window.GetWindow(this), "是否保存本次批改？", "提示") == MessageBoxResult.Yes)
                        {
                            drawBoardCanvas.StopEdit();
                            image = drawBoardCanvas.GetImage(false);
                            CorrectWork(image);
                        }
                    }
                    ClearDrawBoard();
                    tagWp.Children.Clear();
                    hasChanged = false;
                    if (Back != null)
                    {
                        Back(this, null);
                    }
                    this.Visibility = Visibility.Collapsed;
                    break;
                case 9://图形
                    graphicPop.IsOpen = true;
                    break;
                default:
                    break;
            }
        }

        private void ColorWp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            colorPop.IsOpen = false;
            drawBoardCanvas.SetPenColor((sender as Rectangle).Fill);
            (sender as Rectangle).Height = 40;
            ResetColorRectWidth(sender as Rectangle);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox.SelectedIndex == -1)
            {
                return;
            }
            //int tag = Convert.ToInt32(listBox.Tag);
            string title = (listBox.SelectedItem as TextBlock).Text;
            listBox.SelectedIndex = -1;
            imageEditInfo.SelectedMenu = 1;
            //ImageTagControl oldControl = tagWp.Children.Cast<ImageTagControl>().FirstOrDefault(p => p.TagInfo.Tag == tag);
            //if (oldControl != null)
            //{
            //    tagWp.Children.Remove(oldControl);
            //}
            //ImageTagInfo info = new ImageTagInfo() { Tag = tag, Title = title };
            if (listBox == gradeLb)
            {
                for (int i = 0; i < tagWp.Children.Count; i++)
                {
                    ImageTagControl tagControl = tagWp.Children[i] as ImageTagControl;
                    for (int j = 0; j < gradeLb.Items.Count; j++)
                    {
                        TextBlock text = gradeLb.Items[j] as TextBlock;
                        if (tagControl.TagInfo.Title.Equals(text.Text))
                        {
                            tagControl.TagInfo.Title = title;
                            hasChanged = true;
                            return;
                        }
                    }
                }
            }
            if (tagWp.Children.Count >= 5)
            {
                MessageBoxEx.ShowInfo("最多选择5个评语！", Window.GetWindow(this));
                return;
            }

            ImageTagControl oldControl = tagWp.Children.Cast<ImageTagControl>().FirstOrDefault(p => p.TagInfo.Title.Equals(title));
            if (oldControl != null)
            {
                return;
            }
            ImageTagInfo info = new ImageTagInfo() { Title = title };
            ImageTagControl control = new ImageTagControl() { TagInfo = info, DataContext = info };
            control.CloseClick += Control_CloseClick;
            tagWp.Children.Add(control);
            hasChanged = true;
        }

        private void Control_CloseClick(object sender, EventArgs e)
        {
            tagWp.Children.Remove(sender as Control);
            hasChanged = true;
        }

        private void GraphicWp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int tag = Convert.ToInt32((sender as Grid).Tag);
            imageEditInfo.SelectedImageEditMenu = 9;
            imageEditInfo.SelectedGraphicMenu = tag;
            switch (tag)
            {
                case 10:
                    drawBoardCanvas.DrawMode = DrawMode.Rectangle;
                    break;
                case 11:
                    drawBoardCanvas.DrawMode = DrawMode.Ellipse;
                    break;
                case 12:
                    drawBoardCanvas.DrawMode = DrawMode.Line;
                    break;
                case 13:
                    drawBoardCanvas.DrawMode = DrawMode.NonClosedPolyLine;
                    break;
                case 14:
                    drawBoardCanvas.DrawMode = DrawMode.ClosedPolyLine;
                    break;
                default:
                    break;
            }
            graphicPop.IsOpen = false;
        }

        private void previousBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isBusy)
            {
                MessageBoxEx.ShowInfo("正在上传作业批改，请稍候！");
                return;
            }
            if (selectedImageIndex > 0)
            {
                ShowImage(selectedImageIndex - 1);
            }
        }

        private void nextBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isBusy)
            {
                MessageBoxEx.ShowInfo("正在上传作业批改，请稍候！");
                return;
            }
            if (selectedImageIndex + 1 < workArr.Length)
            {
                ShowImage(selectedImageIndex + 1);
            }
        }
    }
}

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
    /// ImageListControl.xaml 的交互逻辑
    /// </summary>
    public partial class ImageListControl : UserControl
    {
        private int currentPageIndex = 0;
        private int pageCount = 0;
        private int selectedGroup = -1;
        private bool isBusy = false;
        public ImageListControl()
        {
            InitializeComponent();
            groupListBox.Items.Clear();
        }

        public void Init()
        {
            BindGroup();
        }

        public void ShowWorkList()
        {
            GetWorkList(currentPageIndex, selectedGroup);
            this.Visibility = Visibility.Visible;
        }

        private void BindGroup()
        {
            //List<IGrouping<int, StudentInfo>> groupList = Common.StudentList.GroupBy(p => p.Group).OrderBy(p => p.Key).ToList();
            groupListBox.Items.Clear();
            foreach (var item in Common.CurrentClassRoomV2.GroupList)
            {
                ListBoxItemControl itemControl = new ListBoxItemControl()
                {
                    Width = 140,
                    Text = item.GroupName,
                    Tag = item.GroupId
                };
                groupListBox.Items.Add(itemControl);
            }
            groupListBox.Items.Add(new ListBoxItemControl()
            {
                Width = 140,
                Text = "所有分组",
                Tag = -1
            });
            groupListBox.SelectionChanged -= ListBox_SelectionChanged;
            groupListBox.SelectedIndex = groupListBox.Items.Count - 1;
            groupListBox.SelectionChanged += ListBox_SelectionChanged;
        }

        private void GetWorkList(int pageIndex, int group)
        {
            if (isBusy)
            {
                return;
            }
            isBusy = true;
            imageWp.Children.Clear();
            this.currentPageIndex = pageIndex;
            this.selectedGroup = group;
            NameValueCollection dict = new NameValueCollection();
            dict.Add("token", Common.CurrentUser.Token);
            Task.Run(() =>
            {
                string url = string.Format("{0}?page={1}&size=8&semester={2}&idYear={3}&idClass={4}",
                    Common.GetWorkList, pageIndex, Common.SemesterInfo.Semester,
                    Common.SemesterInfo.Id, Common.CurrentClass.Id, group);
                if (group > 0)
                {
                    url += "&group=" + group;
                }
                string worksPath = Common.SettingsPath + "Work\\";
                if (!Directory.Exists(worksPath))
                {
                    Directory.CreateDirectory(worksPath);
                }
                string jsonResult = HttpUtility.DownloadString(url, Encoding.UTF8, dict, "form-data");
                ResultInfo<StudentWorkInfo> result = JsonConvert.DeserializeObject<ResultInfo<StudentWorkInfo>>(jsonResult);
                if (result != null)
                {
                    if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        pageCount = result.Body.PageCount;
                        this.Dispatcher.Invoke(() =>
                        {
                            foreach (StudentWorkDetailsInfo item in result.Body.Content)
                            {
                                item.ClassName = Common.CurrentClass.Name;
                                GroupV2Info groupInfo = Common.CurrentClassRoomV2.GroupList.FirstOrDefault(p => p.GroupId.ToString() == item.Group);
                                if (groupInfo != null)
                                {
                                    item.GroupName = groupInfo.GroupName;
                                } 
                                StudentInfo studentInfo = Common.StudentList.FirstOrDefault(p => p.Id == item.StudentId);
                                if (studentInfo != null)
                                {
                                    item.Number = studentInfo.Number;
                                }
                                string savePath;
                                if (!string.IsNullOrEmpty(item.Correct))
                                {
                                    savePath = worksPath + System.IO.Path.GetFileName(item.Correct);
                                }
                                else
                                {
                                    savePath = worksPath + System.IO.Path.GetFileName(item.Work);
                                }
                                ImageItemControl3 imageItemControl3 = new ImageItemControl3();
                                imageItemControl3.SetImage(item, savePath);
                                imageItemControl3.EditImageClick += ImageItemControl3_EditImageClick;
                                imageItemControl3.ImageClick += ImageItemControl3_ImageClick;
                                imageItemControl3.DataContext = item;
                                imageWp.Children.Add(imageItemControl3);
                            }
                            pageLb.Text = string.Format("{0}/{1}", currentPageIndex + 1, pageCount);
                        });
                    }
                }
                isBusy = false;
            });
        }

        private void groupBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            groupPop.IsOpen = true;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            groupPop.IsOpen = false;
            if ((sender as ListBox).SelectedItem != null)
            {
                ListBoxItemControl control = (sender as ListBox).SelectedItem as ListBoxItemControl;
                groupLb.Text = control.Text;
                GetWorkList(0, Convert.ToInt32(control.Tag));
            }
        }

        private void cancelBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void ImageItemControl3_EditImageClick(object sender, EventArgs e)
        {
            int index = imageWp.Children.IndexOf(sender as FrameworkElement);
            StudentWorkDetailsInfo[] strArr = imageWp.Children.Cast<ImageItemControl3>().Select(p => p.WorkInfo).ToArray();
            imageEditControl.EditImages(strArr, index);
        }

        private void ImageItemControl3_ImageClick(object sender, EventArgs e)
        {
            int index = imageWp.Children.IndexOf(sender as FrameworkElement);
            StudentWorkDetailsInfo[] strArr = imageWp.Children.Cast<ImageItemControl3>().Select(p => p.WorkInfo).ToArray();
            ImageViewControl.ShowImage(strArr, index);
        }

        private void previousBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (currentPageIndex > 0)
            {
                GetWorkList(currentPageIndex - 1, selectedGroup);
            }
        }

        private void nextBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (currentPageIndex + 1 < pageCount)
            {
                GetWorkList(currentPageIndex + 1, selectedGroup);
            }
        }

        private void imageEditControl_Back(object sender, EventArgs e)
        {
            ImageItemControl3[] itemArr = imageWp.Children.Cast<ImageItemControl3>().ToArray();
            Array.ForEach(itemArr, p => p.SetImage(p.WorkInfo, p.WorkInfo.LocalPath));
        }
    }
}

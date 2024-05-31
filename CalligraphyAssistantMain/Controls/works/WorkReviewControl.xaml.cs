using CalligraphyAssistantMain.Code;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Permissions;
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

namespace CalligraphyAssistantMain.Controls.works
{
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// WorkReviewControl.xaml 的交互逻辑
    /// </summary>
    public partial class WorkReviewControl : UserControl
    {
        public ShareInfo CurrentShare { get; set; } = new ShareInfo();
        public List<DiscussionInfo> CurrentShareDiscussionlist { get; set; } = new List<DiscussionInfo>();
        public StudentWorkDetailsInfo WorkInfo { get; set; }
        public FrameworkElement ViewControl {  get; set; }
        public WorkReviewControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public void InitData(StudentWorkDetailsInfo workInfo, ShareInfo info,string type)
        {
            if (type== "Student")
            {
                gridView.Visibility= Visibility.Visible;
            }
            else
            {
                gridView.Visibility = Visibility.Collapsed;
            }
            if (workInfo.suffix == ".mp4")
            {
               var control = new PlayMediaControl() { UrlPath = workInfo.LocalPath };
                control.StopMedia();
                ViewControl = control;
            }
            else
            {
                ViewControl = new Image() { Source = new BitmapImage(new Uri(workInfo.LocalPath, UriKind.Absolute)) };
            }
            content.Text=string.Empty;
            WorkInfo = workInfo;
            CurrentShare = info;
            fivePointStarGroup1.SelectCount = 5;
            userCount.Text = string.Format("分享学生（{0}人）:", info.members.Count);
            GetDiscussionList();
        }
        private void GetDiscussionList()
        {
            Task.Run(() =>
            {
                NameValueCollection dict = new NameValueCollection();
                dict.Add("token", Common.CurrentUser.Token);
                var data = new
                {
                    id_class = Common.CurrentClassV2.ClassId,
                    id_lesson = Common.CurrentLesson.Id,
                    id_share = CurrentShare.id,
                    type = 1,
                };
                string jsonResult = HttpUtility.UploadValuesJson(Common.GetDiscussionList, data, Encoding.UTF8, Encoding.UTF8, dict);
                ResultInfo<List<DiscussionInfo>> result = JsonConvert.DeserializeObject<ResultInfo<List<DiscussionInfo>>>(jsonResult);
                if (result != null && result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        CurrentShareDiscussionlist = result.Body;
                        CurrentShareDiscussionlist.ForEach(p =>
                        {
                            p.IsSender = p.id_member == Common.CurrentUser.Id;
                        });
                    });
                }
            });
        }
        private void closeBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;

        }

        private void sendBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!string.IsNullOrEmpty(content.Text))
            {
                NameValueCollection dict = new NameValueCollection();
                dict.Add("token", Common.CurrentUser.Token);
                var data = new
                {
                    id_class = Common.CurrentClassV2.ClassId,
                    id_lesson = Common.CurrentLesson.Id,
                    id_share = CurrentShare.id,
                    content = content.Text,
                    type = 1,
                };
                Task.Run(() =>
                {
                   
                    string jsonResult = HttpUtility.UploadValuesJson(Common.SaveDiscussion, data, Encoding.UTF8, Encoding.UTF8, dict);
                    ResultInfo<object> result = JsonConvert.DeserializeObject<ResultInfo<object>>(jsonResult);
                    if (result != null && result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            content.Text = string.Empty;
                        });
                        GetDiscussionList();
                    }
                });
            }
        }

        private void scoringBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NameValueCollection dict = new NameValueCollection();
            dict.Add("token", Common.CurrentUser.Token);
            var data = new
            {
                id_class = Common.CurrentClassV2.ClassId,
                id_lesson = Common.CurrentLesson.Id,
                id_share = CurrentShare.id,
                star = fivePointStarGroup1.SelectCount,
                type = 1,
            };
            Task.Run(() =>
            {
               
                string jsonResult = HttpUtility.UploadValuesJson(Common.SaveDiscussion, data, Encoding.UTF8, Encoding.UTF8, dict);
                ResultInfo<object> result = JsonConvert.DeserializeObject<ResultInfo<object>>(jsonResult);
                if (result != null && result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        content.Text = string.Empty;
                    });
                    GetDiscussionList();
                }
            });
        }
    }
}

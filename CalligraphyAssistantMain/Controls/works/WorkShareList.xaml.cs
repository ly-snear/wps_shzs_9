using CalligraphyAssistantMain.Code;
using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace CalligraphyAssistantMain.Controls.works
{
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// WorkShareList.xaml 的交互逻辑
    /// </summary>
    public partial class WorkShareList : UserControl
    {
        public List<ShareInfo> ShareListList { get; set; } = new List<ShareInfo>();
        public ObservableCollection<ShareInfo> ShareListCollectionPaging { get; set; }
        public Pager<ShareInfo> Pager { get; set; }
        public StudentWorkDetailsInfo WorkInfo {  get; set; }
        public ShareInfo CurrentShare {  get; set; }=new ShareInfo();
        public List<DiscussionInfo> CurrentShareDiscussionlist { get; set; } =new List<DiscussionInfo>();
        private string type { get; set; }
        public WorkShareList()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public void InitData()
        {
            ShareListCollectionPaging = null;
            if (ShareListList.Count > 0)
            {
                view.Visibility = Visibility.Visible;
                Pager = new Pager<ShareInfo>(ShareListList.Count > 10 ? 10 : ShareListList.Count, ShareListList);
                Pager.PagerUpdated += items =>
                {
                    ShareListCollectionPaging = new ObservableCollection<ShareInfo>(items);
                };
                Pager.CurPageIndex = 1;
            }
            else
            {
                view.Visibility = Visibility.Hidden;
            }
        }
        public void GetShareList(StudentWorkDetailsInfo work,string type)
        {
            this.type = type;
            WorkInfo =work;
            NameValueCollection dict = new NameValueCollection();
            dict.Add("token", Common.CurrentUser.Token);
            var data = new
            {
                id_class = Common.CurrentClassV2.ClassId,
                id_lesson = Common.CurrentLesson.Id,
                sid= work.Id,
                type = 1,
            };
            string jsonResult = HttpUtility.UploadValuesJson(Common.GetshareList, data, Encoding.UTF8, Encoding.UTF8, dict);
            ResultInfo<List<ShareInfo>> result = JsonConvert.DeserializeObject<ResultInfo<List<ShareInfo>>>(jsonResult);
            if (result != null && result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                ShareListList = result.Body;
                InitData();
            }
        }

        private void ReturnBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility= Visibility.Collapsed;
        }

        private void CheckBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(sender is Border border&&border.Tag is ShareInfo info)
            {
                EventNotify.OnCheckShareItem(WorkInfo,info, type);
            }
        }

       
    }
}

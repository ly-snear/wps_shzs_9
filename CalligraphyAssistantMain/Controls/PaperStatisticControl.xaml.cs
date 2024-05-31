using CalligraphyAssistantMain.Code;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace CalligraphyAssistantMain.Controls
{
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// PaperStatisticControl.xaml 的交互逻辑
    /// </summary>
    public partial class PaperStatisticControl : Window
    {
        public ObservableCollection<PaperTopicInfo> PaperTopicCollectionPaging { get; set; }
        public Pager<PaperTopicInfo> Pager { get; set; }
        public PaperInfo PaperData { get; set; }
        public PaperStatisticControl(PaperInfo paperInfo)
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
            Common.ShowTipCallback += (x) => { tipControl.ShowTip(x); };
            this.DataContext = this;

            PaperData = paperInfo;
            if (PaperData.topics.Count > 0)
            {
                view.Visibility = Visibility.Visible;
                Pager = new Pager<PaperTopicInfo>(PaperData.topics.Count > 3 ? 3: PaperData.topics.Count, PaperData.topics);
                Pager.PagerUpdated += items =>
                {
                    PaperTopicCollectionPaging = new ObservableCollection<PaperTopicInfo>(items);
                };
                Pager.CurPageIndex = 1;
            }
            else
            {
                view.Visibility = Visibility.Hidden;
            }
            EventNotify.CheckWorksClick += EventNotify_CheckWorksClick;
        }

        private void EventNotify_CheckWorksClick(object sender, EventArgs e)
        {
            this.DialogResult = false;
            EventNotify.CheckWorksClick -= EventNotify_CheckWorksClick;
        }

        /// <summary>
        /// 查看统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void statistics_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(sender is Border border&& border.Tag is PaperTopicInfo paperTopicInfo)
            {
                if (PaperData.paperResult != null)
                {
                    var topicResult = PaperData.paperResult.topics.FirstOrDefault(p => p.id == paperTopicInfo.id);
                    if (topicResult != null&& topicResult.students!=null)
                    {
                        StatisticControl statisticControl = new StatisticControl(topicResult);
                        if (statisticControl.ShowDialog() == true)
                        { 
                        }
                     }
                    else
                    {
                        Common.ShowTip("该题没有学生提交答案");
                    }
                }
                else
                {
                    Common.ShowTip("没有学生提交答案");
                }
            }

        }

        private void ReturnBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult=false;
        }
    }
}

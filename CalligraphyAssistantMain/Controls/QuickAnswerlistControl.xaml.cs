using CalligraphyAssistantMain.Code;
using CommonServiceLocator;
using Newtonsoft.Json;
using Prism.Events;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceModel.Security;
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
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// QuickAnswerlistControl.xaml 的交互逻辑
    /// </summary>
    public partial class QuickAnswerlistControl : UserControl
    {
        public event EventHandler AddQuickAnswerClick = null;
        public event EventHandler StatisticsClick = null;
        public event EventHandler BeganWritingClick = null; 
        public ObservableCollection<QuickAnswerInfo> QuickAnswerCollectionPaging {  get; set; }
        public Pager<QuickAnswerInfo> Pager { get; set; }
        private IEventAggregator eventAggregator;
        public QuickAnswerlistControl()
        {
            InitializeComponent();
            this.DataContext = this;
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<MQMessageEvent>().Subscribe(Messaging);
        }
        public void InitData()
        {
            if (Common.QuickAnswerList.Count>0)
            {
                view.Visibility = Visibility.Visible;
                Pager = new Pager<QuickAnswerInfo>(Common.QuickAnswerList.Count > 6? 6 : Common.QuickAnswerList.Count, Common.QuickAnswerList);
                Pager.PagerUpdated += items =>
                {
                    QuickAnswerCollectionPaging = new ObservableCollection<QuickAnswerInfo>(items);
                };
                Pager.CurPageIndex = 1;
            }
            else
            {
                view.Visibility = Visibility.Hidden;
            }
         
        }

        private void addBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            AddQuickAnswerClick?.Invoke(this, e);
        }

        private void closeBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
        private void statistics_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is QuickAnswerInfo info)
            {
                StatisticsClick.Invoke(info, e);
            }
        }

        private void start_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(sender is Border border&&border.Tag is QuickAnswerInfo info)
            {
                BeganWritingClick.Invoke(info, e);
            }
        }

        private void stop_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {            
            //通知所有学生该答题项关闭
            if (sender is Border border && border.Tag is QuickAnswerInfo info)
            {
                info.status = "2";
                MQCenter.Instance.SendToAll(MessageType.StopQuickAnswer, new
                {
                    info.id,
                    answer_type = info.type == "quick" ? 0 : 1,
                    info.title,
                    info.question,
                    info.content
                });
                EventNotify.OnCountdownStopTrigger(info.id);
            }
        }
        private void Messaging(Code.Message msg)
        {
            try
            {
                if (msg != null && msg.classId == Common.CurrentClassV2.ClassId && msg.lessonId == Common.CurrentLesson.Id && msg.userType == UserType.student)
                {
                    switch (msg.type)
                    {
                        case MessageType.CompleteQuickAnswer:
                            {
                                return;
                                var data = new { id = 0 };
                                dynamic json = JsonConvert.DeserializeAnonymousType(msg.data.ToString(), data);
                                if (json != null)
                                {
                                    var info = Common.QuickAnswerList.FirstOrDefault(p => p.id == json.id);
                                    if (info != null)
                                    {
                                        if(info.status == "1")
                                        {

                                            NameValueCollection dict = new NameValueCollection();
                                            dict.Add("token", Common.CurrentUser.Token);
                                            if (info.type == "quick")
                                            {
                                                string jsonResult = HttpUtility.UploadValuesJson(Common.GetQuickResults, new{ info.id}, Encoding.UTF8, Encoding.UTF8, dict);
                                                ResultInfo<ResultCalligraphyListInfo<List<Answer>>> result = JsonConvert.DeserializeObject<ResultInfo<ResultCalligraphyListInfo<List<Answer>>>>(jsonResult);
                                                if (result != null && result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    ResultCalligraphyListInfo<List<Answer>> resultCalligraphyListInfo = result.Body;
                                                
                                                    info.students = resultCalligraphyListInfo.Data;

                                                }

                                            }
                                        }
                                    }

                                }

                            }
                            break;

                    }
                }
            }
            catch (Exception)
            {

            }

        }
    }
}

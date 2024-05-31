using CalligraphyAssistantMain.Code;
using CommonServiceLocator;
using Newtonsoft.Json;
using Prism.Events;
using PropertyChanged;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
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
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// VoteListControl.xaml 的交互逻辑
    /// </summary>
    public partial class VoteListControl : UserControl
    {
        public ObservableCollection<VoteInfo> VoteCollectionPaging { get; set; }
        public Pager<VoteInfo> Pager { get; set; }
        private IEventAggregator eventAggregator;
        public VoteListControl()
        {
            InitializeComponent();
            this.DataContext = this;
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<MQMessageEvent>().Subscribe(Messaging);
        }

        public void InitData()
        {
            if (Common.VoteList.Count > 0)
            {
                view.Visibility = Visibility.Visible;
                Pager = new Pager<VoteInfo>(Common.VoteList.Count > 6 ? 6 : Common.VoteList.Count, Common.VoteList);
                Pager.PagerUpdated += items =>
                {
                    VoteCollectionPaging = new ObservableCollection<VoteInfo>(items);
                };
                Pager.CurPageIndex = 1;
            }
            else
            {
                view.Visibility = Visibility.Hidden;
            }
        }



        /// <summary>
        /// 新建投票
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            AddVoteControl addVote = new AddVoteControl();
            if (addVote.ShowDialog() == true)
            {
                //显示投票统计
                var info = Common.VoteList.FirstOrDefault(p => p.id == addVote.VoteId);
                if (info != null)
                {
                    info.status = "1";
                    MQCenter.Instance.SendToAll(MessageType.StartVote, new { info.id, info.title, info.content });

                    //倒计时结束
                    EventNotify.OnCountdownTrigger(info.id, () =>
                    {
                        info.status = "2";
                        MQCenter.Instance.SendToAll(MessageType.StopVote, new { info.id, info.title, info.content });
                    });
                    showStatistics(info);
                }

            }
            this.Visibility = Visibility.Collapsed;
        }

        private void CloseBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 投票统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Statistics_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is VoteInfo info)
            {
                showStatistics(info);
            }
        }

        /// <summary>
        /// 投票统计窗体
        /// </summary>
        /// <param name="info"></param>
        private void showStatistics(VoteInfo info)
        {
            NameValueCollection dict = new NameValueCollection();
            dict.Add("token", Common.CurrentUser.Token);

            string jsonResult = HttpUtility.DownloadString(Common.GetVoteResults + $"?id_vote={info.id}", Encoding.UTF8, dict);
            ResultInfo<List<VoteResult>> result = JsonConvert.DeserializeObject<ResultInfo<List<VoteResult>>>(jsonResult);
            if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
            {
                if (result.Body.Count > 0)
                {
                    info.voteResult = result.Body[0];
                    StatisticControl statisticControl = new StatisticControl(info);
                    if (statisticControl.ShowDialog() == true)
                    {
                        if (info.status == "1")
                        {
                            PublishAnswerControl publishAnswerControl = new PublishAnswerControl();
                            publishAnswerControl.OptionCount = info.content;
                            if (publishAnswerControl.ShowDialog() == true)
                            {
                                //公布答案
                                MQCenter.Instance.SendToAll(MessageType.PublishVote, new { info.id, info.title, answer = publishAnswerControl.Answer });
                            }
                        }
                    }
                }

            }
        }

        private void Start_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is VoteInfo info)
            {
                info.status = "1";
                MQCenter.Instance.SendToAll(MessageType.StartVote, new { info.id, info.title, info.content });
                //倒计时结束
                EventNotify.OnCountdownTrigger(info.id, () =>
                {
                    info.status = "2";
                    MQCenter.Instance.SendToAll(MessageType.StopVote, new { info.id, info.title, info.content });
                });
            }
        }

        private void Stop_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (sender is Border border && border.Tag is VoteInfo info)
            {
                info.status = "2";
                MQCenter.Instance.SendToAll(MessageType.StopVote, new { info.id, info.title, info.content });
                EventNotify.OnCountdownStopTrigger(info.id);
            }
        }

        /// <summary>
        /// 接收学生端消息
        /// </summary>
        /// <param name="msg"></param>
        private void Messaging(Code.Message msg)
        {
            try
            {
                if (msg != null && msg.classId == Common.CurrentClassV2.ClassId && msg.lessonId == Common.CurrentLesson.Id && msg.userType == UserType.student)
                {
                    switch (msg.type)
                    {
                        case MessageType.CompleteVote:
                            {
                                var data = new { id = 0 };
                                dynamic json = JsonConvert.DeserializeAnonymousType(msg.data.ToString(), data);
                                Console.WriteLine("接收学生端消息：" + msg.data.ToString());
                                if (json != null)
                                {
                                    var info = Common.VoteList.FirstOrDefault(p => p.id == json.id);
                                    if (info != null)
                                    {
                                        if (info.status == "1")
                                        {
                                            Task.Run(() =>
                                            {
                                                NameValueCollection dict = new NameValueCollection();
                                                dict.Add("token", Common.CurrentUser.Token);
                                                string jsonResult = HttpUtility.DownloadString(Common.GetVoteResults + $"?id_vote={info.id}", Encoding.UTF8, dict);
                                                ResultInfo<List<VoteResult>> result = JsonConvert.DeserializeObject<ResultInfo<List<VoteResult>>>(jsonResult);
                                                if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                                                {
                                                    if (result.Body.Count > 0)
                                                    {
                                                        info.voteResult = result.Body[0];
                                                    }
                                                }
                                            });
                                        }
                                    }
                                }
                            }
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("接收学生端消息：data null");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("接收学生端消息异常：" + e.ToString());
            }

        }
    }
}

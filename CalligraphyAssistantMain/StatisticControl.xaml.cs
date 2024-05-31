using CalligraphyAssistantMain.Code;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
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
using PropertyChanged;
using System.Runtime.Remoting.Contexts;
using Prism.Events;
using CommonServiceLocator;
using Newtonsoft.Json;
using System.Collections.Specialized;

namespace CalligraphyAssistantMain
{
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// StatisticControl.xaml 的交互逻辑
    /// </summary>
    public partial class StatisticControl : Window
    {
        private string[] Answers = new string[] { "无效", "A", "B", "C", "D", "E" };
        private readonly IEventAggregator eventAggregator;
        private StatisticControl()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Owner = Application.Current.MainWindow;
            //eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            //eventAggregator.GetEvent<MQMessageEvent>().Subscribe(Messaging);

        }
        public StatisticControl(QuickAnswerInfo info) : this()
        {
            title.Text = "答题统计";
            startBtn.Visibility = Visibility.Visible;
            content.Text = string.Format("{0}、{1}", info.id, info.title);
            QuickAnswerInfo = info;

            ListData = info.students.Select(p => p.name_student).ToList();
            txt.Text = string.Format("选择A的名单");
            info.PropertyChanged += Info_PropertyChanged;
            UpQuickInfo();
        }
        public StatisticControl(VoteInfo info) : this()
        {
            VoteData = info;
            title.Text = "投票统计";
            startBtn.Visibility = Visibility.Visible;
            content.Text = string.Format("{0}、{1}", info.id, info.title);

            ListData = info.voteResult.student.Where(p => p.result == 1).Select(p => p.name).ToList();
            txt.Text = string.Format("选择A的名单");
            info.PropertyChanged += Info_PropertyChanged;
            UpVoteInfo();
        }
        public StatisticControl(TopicsItem info) : this()
        {
            topicsItem = info;
            title.Text = "答题统计";
            startBtn.Visibility = Visibility.Collapsed;
            int correctCount = 0;
            int errorCount = 0;
            //单选题
            if (info.type == 1)
            {
                foreach (var item in info.answers)
                {
                    if (item.answer == "1")
                    {
                        info.students.ForEach(p =>
                        {
                            correctCount += p.answer.Count(x => x.value == item.order.ToString());
                        });
                    }
                    if (item.answer == "0")
                    {
                        info.students.ForEach(p =>
                        {
                            errorCount += p.answer.Count(x => x.value == item.order.ToString());
                        });
                    }
                }
                content.Text = string.Format("答题正确-{0}  错误-{1}", correctCount, errorCount);
                ListData = info.students.Where(p => p.answer[0].value == "1").Select(p => p.name).ToList();
                txt.Text = string.Format("选择A的名单");

                Labels = Answers.Take(info.answers.Count + 1).ToList();
                ChartValues<double> achievement = new ChartValues<double>();

                for (int i = 0; i <= info.answers.Count; i++)
                {
                    achievement.Add(info.students.Count(p => p.answer[0].value == i.ToString()));
                }
                var column = new ColumnSeries();
                column.DataLabels = false;
                column.LabelsPosition = BarLabelPosition.Perpendicular;
                column.Values = achievement;
                column.Title = "答题";
                Achievement.Clear();
                Achievement.Add(column);
                cartesian.DataClick += TopicCartesian_DataClick;

                if (Labels.Count > 0 && info.students.Count > 0)
                {
                    cartesian.Visibility = Visibility.Visible;
                }
            }
            //判断题
            if (info.type == 3)
            {
                foreach (var item in info.answers)
                {
                    if (item.answer == "1")
                    {
                        info.students.ForEach(p =>
                        {
                            correctCount += p.answer.Count(x => x.value == item.order.ToString());
                        });
                    }
                    if (item.answer == "0")
                    {
                        info.students.ForEach(p =>
                        {
                            errorCount += p.answer.Count(x => x.value == item.order.ToString());
                        });
                    }
                }
                content.Text = string.Format("答题正确-{0}  错误-{1}", correctCount, errorCount);
                ListData = info.students.Where(p => p.answer[0].value == "1").Select(p => p.name).ToList();
                txt.Text = string.Format("选择正确的名单");

                Labels = Answers.Take(info.answers.Count + 1).ToList();
                ChartValues<double> achievement = new ChartValues<double>();

                for (int i = 0; i <= info.answers.Count; i++)
                {
                    achievement.Add(info.students.Count(p => p.answer[0].value == i.ToString()));
                }
                var column = new ColumnSeries();
                column.DataLabels = false;
                column.LabelsPosition = BarLabelPosition.Perpendicular;
                column.Values = achievement;
                column.Title = "答题";
                Achievement.Clear();
                Achievement.Add(column);
                cartesian.DataClick += (s, chartPoint) =>
                {
                    var selectedLabel = chartPoint.X;
                    txt.Text = string.Format("{0}名单", selectedLabel == 0 ? "正确" : "错误");

                    ListData = topicsItem.students.Where(p => p.answer[0].value == selectedLabel.ToString()).Select(p => p.name).ToList();
                };

                if (Labels.Count > 0 && info.students.Count > 0)
                {
                    cartesian.Visibility = Visibility.Visible;
                }
            }



        }

        private void TopicCartesian_DataClick(object sender, ChartPoint chartPoint)
        {
            var selectedLabel = chartPoint.X;
            string type = Answers[(int)selectedLabel];
            if (type != "无效")
            {
                txt.Text = string.Format("选择{0}的名单", type);
            }
            else
            {
                txt.Text = string.Format("{0}名单", type);
            }

            ListData = topicsItem.students.Where(p => p.answer[0].value == selectedLabel.ToString()).Select(p => p.name).ToList();
        }

        private void Info_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "voteResult")
            {
                this.Dispatcher.Invoke(() =>
                {
                    UpVoteInfo();
                });
            }
            if (e.PropertyName == "students")
            {
                this.Dispatcher.Invoke(() =>
                {
                    UpQuickInfo();
                });
            }
        }
        private void UpQuickInfo()
        {
            Labels = Answers.Take(QuickAnswerInfo.question + 1).ToList();
            ChartValues<double> achievement = new ChartValues<double>();

            for (int i = 0; i <= QuickAnswerInfo.question; i++)
            {
                achievement.Add(QuickAnswerInfo.students.Count(p => p.value == i));
            }
            var column = new ColumnSeries();
            column.DataLabels = false;
            column.LabelsPosition = BarLabelPosition.Perpendicular;
            column.Values = achievement;
            column.Title = "投票";
            Achievement.Clear();
            Achievement.Add(column);
            cartesian.DataClick += Cartesian_DataClick;

            if (Labels.Count > 0 && QuickAnswerInfo.students.Count > 0)
            {
                cartesian.Visibility = Visibility.Visible;
            }
        }

        private void Cartesian_DataClick(object sender, ChartPoint chartPoint)
        {
            var selectedLabel = chartPoint.X;
            string type = Answers[(int)selectedLabel];
            if (type != "无效")
            {
                txt.Text = string.Format("选择{0}的名单", type);
            }
            else
            {
                txt.Text = string.Format("{0}名单", type);
            }
            ListData = QuickAnswerInfo.students.Where(p => p.value == selectedLabel).Select(p => p.name_student).ToList();
        }

        private void UpVoteInfo()
        {
            Labels = Answers.Take(VoteData.content + 1).ToList();
            ChartValues<double> achievement = new ChartValues<double>();

            for (int i = 0; i <= VoteData.content; i++)
            {
                achievement.Add(VoteData.voteResult.student.Count(p => p.result == i));
            }
            var column = new ColumnSeries();
            column.DataLabels = false;
            column.LabelsPosition = BarLabelPosition.Perpendicular;
            column.Values = achievement;
            column.Title = "投票";
            Achievement.Clear();
            Achievement.Add(column);
            cartesian.DataClick += VoteCartesian_DataClick;

            if (Labels.Count > 0)
            {
                cartesian.Visibility = Visibility.Visible;
            }
        }

        private void VoteCartesian_DataClick(object sender, ChartPoint chartPoint)
        {
            var selectedLabel = chartPoint.X;
            string type = Answers[(int)selectedLabel];
            if (type != "无效")
            {
                txt.Text = string.Format("选择{0}的名单", type);
            }
            else
            {
                txt.Text = string.Format("{0}名单", type);
            }
            ListData = VoteData.voteResult.student.Where(p => p.result == selectedLabel).Select(p => p.name).ToList();
        }

        private void AnswerCartesian_DataClick(object sender, ChartPoint chartPoint)
        {
            //var selectedLabel = chartPoint.X;
            //string type = Answers[(int)selectedLabel];
            //if (type != "无效")
            //{
            //    txt.Text = string.Format("选择{0}的名单", type);
            //}
            //else
            //{
            //    txt.Text = string.Format("{0}名单", type);
            //}
            //ListData = voteResult.student.Where(p => p.result == selectedLabel).Select(p => p.name).ToList();
        }

        #region 属性
        private TopicsItem topicsItem { get; set; }
        private VoteInfo VoteData { get; set; }
        private QuickAnswerInfo QuickAnswerInfo { get; set; }
        public List<string> ListData { get; set; }
        /// <summary>
        /// 成绩柱状图
        /// </summary>
        public SeriesCollection Achievement
        {
            get; set;
        } = new SeriesCollection();

        public List<string> Labels
        {
            get; set;
        } = new List<string>();
        #endregion

        private void closeBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = false;
        }

        private void startBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = true;
        }



    }
}

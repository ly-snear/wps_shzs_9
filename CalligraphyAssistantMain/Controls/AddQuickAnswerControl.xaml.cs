using CalligraphyAssistantMain.Code;
using Newtonsoft.Json;
using PropertyChanged;
using Qiniu.Storage;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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

namespace CalligraphyAssistantMain.Controls
{
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// AddQuickAnswerControl.xaml 的交互逻辑
    /// </summary>
    public partial class AddQuickAnswerControl : UserControl
    {
        public event EventHandler RandomlySelectClick = null;
        public event EventHandler SelectUserClick = null;
        public event EventHandler QuickResponseClick = null;
        public bool IsChoice { get; set; } = true;
        public string Theme { get; set; }
        public int optionCount { get; set; } = 2;
        /// <summary>
        /// 倒计时时长
        /// </summary>
        public int optionTime { get; set; } = 1;
        public QuickAnswerInfo QuickAnswerInfo { get; set; }
        public AddQuickAnswerControl()
        {
            InitializeComponent();
            this.DataContext = this;
            IsVisibleChanged += AddQuickAnswerControl_IsVisibleChanged;
            //禁用“抢答”按钮
            this.quickResponseBtn.IsEnabled = false;
            this.quickResponseBtn.Background = new SolidColorBrush(Colors.Gray);
        }

        private void AddQuickAnswerControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            optionCount = 2;
            Theme = string.Empty;
            view.IsEnabled = true;
            QuickAnswerInfo = null;
            IsChoice = true;
        }
        public void InitData(QuickAnswerInfo info)
        {
            QuickAnswerInfo = info;
            if (QuickAnswerInfo.type == "subjective")
            {
                Theme = QuickAnswerInfo.title;
                IsChoice = false;
            }
            else
            {
                IsChoice = true;
                optionCount = QuickAnswerInfo.question;
            }

            view.IsEnabled = false;
        }

        private void closeBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void saveBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border)
            {
                string questionId = string.Empty;
                if (QuickAnswerInfo == null)
                {
                    NameValueCollection dict = new NameValueCollection
                {
                    { "token", Common.CurrentUser.Token }
                };

                    if (!IsChoice)
                    {
                        if (string.IsNullOrEmpty(Theme))
                        {
                            MessageBoxEx.ShowError("主观题内容添加失败！", Application.Current.MainWindow);
                            return;
                        }
                        var data = new
                        {
                            id = 0,
                            id_class = Common.CurrentClassV2.ClassId,
                            id_lesson = Common.CurrentLesson.Id,
                            title = Theme,

                        };
                        string jsonResult = HttpUtility.UploadValuesJson(Common.SavetSubjective, data, Encoding.UTF8, Encoding.UTF8, dict);
                        if (!string.IsNullOrEmpty(jsonResult))
                        {
                            ResultInfo<string> resultInfo = JsonConvert.DeserializeObject<ResultInfo<string>>(jsonResult);
                            if (resultInfo != null && resultInfo.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                            {
                                Common.GetQuickAnswer();
                                questionId = resultInfo.Body;
                            }

                        }
                    }
                    else
                    {
                        var data = new
                        {
                            id = 0,
                            id_class = Common.CurrentClassV2.ClassId,
                            id_lesson = Common.CurrentLesson.Id,
                            question = optionCount,
                        };

                        string jsonResult = HttpUtility.UploadValuesJson(Common.SavetQuickAnswer, data, Encoding.UTF8, Encoding.UTF8, dict);
                        if (!string.IsNullOrEmpty(jsonResult))
                        {
                            ResultInfo<string> resultInfo = JsonConvert.DeserializeObject<ResultInfo<string>>(jsonResult);
                            if (resultInfo != null && resultInfo.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                            {
                                Common.GetQuickAnswer();
                                questionId = resultInfo.Body;
                            }

                        }

                    }
                }
                else
                {
                    questionId = QuickAnswerInfo.id.ToString();
                }

                if (!string.IsNullOrEmpty(questionId))
                {
                    int tm = optionTime;
                    if (!string.IsNullOrEmpty(tname.Text))
                    {
                        int _t = -1;
                        if (int.TryParse(tname.Text, out _t))
                        {
                            if (_t > 0 && _t < 45)
                            {
                                tm = _t;
                            }
                        }
                    }  
                    Common.qTime = tm;
                    switch (border.Name)
                    {
                        case "candidateBtn":
                            SelectUserClick?.Invoke(questionId, e);
                            break;
                        case "quickResponseBtn":
                            QuickResponseClick?.Invoke(questionId, e);
                            break;
                        case "randomlyBtn":
                            RandomlySelectClick?.Invoke(questionId, e);
                            break;
                    }
                    this.Visibility = Visibility.Collapsed;
                }

            }

        }

        private void saveBtn_PreviewMouseLeftButtonDown20240205(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border)
            {
                string questionId = string.Empty;
                if (QuickAnswerInfo == null)
                {
                    NameValueCollection dict = new NameValueCollection
                {
                    { "token", Common.CurrentUser.Token }
                };

                    if (!IsChoice)
                    {
                        if (string.IsNullOrEmpty(Theme))
                        {
                            MessageBoxEx.ShowError("主观题内容添加失败！", Application.Current.MainWindow);
                            return;
                        }
                        var data = new
                        {
                            id = 0,
                            id_class = Common.CurrentClassV2.ClassId,
                            id_lesson = Common.CurrentLesson.Id,
                            title = Theme,

                        };
                        string jsonResult = HttpUtility.UploadValuesJson(Common.SavetSubjective, data, Encoding.UTF8, Encoding.UTF8, dict);
                        if (!string.IsNullOrEmpty(jsonResult))
                        {
                            ResultInfo<string> resultInfo = JsonConvert.DeserializeObject<ResultInfo<string>>(jsonResult);
                            if (resultInfo != null && resultInfo.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                            {
                                Common.GetQuickAnswer();
                                questionId = resultInfo.Body;
                            }

                        }
                    }
                    else
                    {
                        var data = new
                        {
                            id = 0,
                            id_class = Common.CurrentClassV2.ClassId,
                            id_lesson = Common.CurrentLesson.Id,
                            question = optionCount,
                        };

                        string jsonResult = HttpUtility.UploadValuesJson(Common.SavetQuickAnswer, data, Encoding.UTF8, Encoding.UTF8, dict);
                        if (!string.IsNullOrEmpty(jsonResult))
                        {
                            ResultInfo<string> resultInfo = JsonConvert.DeserializeObject<ResultInfo<string>>(jsonResult);
                            if (resultInfo != null && resultInfo.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                            {
                                Common.GetQuickAnswer();
                                questionId = resultInfo.Body;
                            }

                        }

                    }
                }
                else
                {
                    questionId = QuickAnswerInfo.id.ToString();
                }

                if (!string.IsNullOrEmpty(questionId))
                {
                    switch (border.Name)
                    {
                        case "candidateBtn":
                            SelectUserClick?.Invoke(questionId, e);
                            break;
                        case "quickResponseBtn":
                            QuickResponseClick?.Invoke(questionId, e);
                            break;
                        case "randomlyBtn":
                            RandomlySelectClick?.Invoke(questionId, e);
                            break;
                    }
                    this.Visibility = Visibility.Collapsed;
                }

            }

        }
    }
}

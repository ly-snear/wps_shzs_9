using CalligraphyAssistantMain.Code;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    /// <summary>
    /// AddVoteControl.xaml 的交互逻辑
    /// </summary>
    public partial class AddVoteControl : Window
    {
        public int VoteId { get; set; }
        private int Count = 2;
        public AddVoteControl()
        {
            InitializeComponent();
            this.DataContext = this;
            this.Owner = Application.Current.MainWindow;
        }

        private void btn_ok_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NameValueCollection dict = new NameValueCollection
                {
                    { "token", Common.CurrentUser.Token }
                };

            NameValueCollection data = new NameValueCollection();
            //data.Add("id_class", Common.CurrentClassV2.ClassId.ToString());
            data.Add("id_lesson", Common.CurrentLesson.Id.ToString());
            data.Add("count", Count.ToString());
            string jsonResult = HttpUtility.UploadValues(Common.SaveVote, data, Encoding.UTF8, Encoding.UTF8, dict);
            if (!string.IsNullOrEmpty(jsonResult))
            {
                ResultInfo<int> resultInfo = JsonConvert.DeserializeObject<ResultInfo<int>>(jsonResult);
                if (resultInfo != null && resultInfo.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    Common.GetVoteInfoList();
                    VoteId = resultInfo.Body;
                }

            }
            this.DialogResult = true;
        }

        private void closeBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = false;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                switch (radioButton.Content)
                {
                    case "5个":
                        Count = 5;
                        break;
                    case "2个":
                        Count = 2;
                        break;
                    case "3个":
                        Count = 3;
                        break;
                    case "4个":
                        Count = 4;
                        break;
                }
            }

        }
    }
}

using CalligraphyAssistantMain.Code;
using PropertyChanged;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalligraphyAssistantMain.Controls
{
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// FileDistributionListControl.xaml 的交互逻辑
    /// </summary>
    public partial class FileDistributionListControl : UserControl
    {
        public ResourceItemInfo Data { get; set; }
        public FileDistributionListControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public void InitData(ResourceItemInfo info)
        {
            Data=info;
        }

        private void ReturnBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// 重新分发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resendBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
             var users= Data.DispensedUsers.Where(p=>!p.IsComplete).ToList();
            users.ForEach(p =>
            {
                var user = Common.StudentList.FirstOrDefault(s=>s.Id == p.Id);
                if (user != null)
                {
                    MQCenter.Instance.Send(user, MessageType.FileDistribute, new
                    {
                        id = Data.ServerId,
                        fileName=Data.FileName,
                        url=Data.ServerUrl,
                        fileSize=Data.FileSize,
                    });
                }
              
            });
        
        }
    }
}

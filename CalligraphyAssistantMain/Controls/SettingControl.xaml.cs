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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalligraphyAssistantMain.Controls
{
    /// <summary>
    /// BeginClassControl.xaml 的交互逻辑
    /// </summary>
    public partial class SettingControl : UserControl
    { 
        public SettingControl()
        {
            InitializeComponent();
        }

      
        private void okBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Common.LoginInfo.AutoLogin = (bool)autoLoginCb.IsChecked;
            Common.LoginInfo.RememberPassword = (bool)rememberPasswordCb.IsChecked;
            Common.SaveLoginSettings(Common.LoginInfo);
            this.Visibility = Visibility.Collapsed;
        }

        private void cancelBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            autoLoginCb.IsChecked = Common.LoginInfo.AutoLogin;
            rememberPasswordCb.IsChecked = Common.LoginInfo.RememberPassword;
        }
    }
}

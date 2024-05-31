using CalligraphyAssistantMain.Code;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
    /// SoftwareControl.xaml 的交互逻辑
    /// </summary>
    public partial class SoftwareControl : UserControl
    {
        public SoftwareControl()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            string settingPath = Common.AppPath + "App.xml";
            if (File.Exists(settingPath))
            {
                try
                {
                    List<AppInfo> list = Common.XmlDeserializeFromFile<List<AppInfo>>(settingPath);
                    foreach (AppInfo item in list)
                    {
                        AppControl appControl = new AppControl();
                        appControl.BindApp(item.Title, item.Path, item.Arguments, item.Admin, item.Integrated, webViewControl, item.Auth);
                        appControl.AppOpened += (x, y) => { this.Visibility = Visibility.Collapsed; };
                        switch (item.Type)
                        {
                            case 1:
                                appWp1.Children.Add(appControl);
                                break;
                            case 2:
                                appWp2.Children.Add(appControl);
                                break;
                            case 3:
                                appWp3.Children.Add(appControl);
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch(Exception ex)
                {
                }
            }
        }

        private void cancelBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}

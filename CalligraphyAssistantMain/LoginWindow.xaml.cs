using CalligraphyAssistantMain.Code;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CalligraphyAssistantMain
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public bool CanMove { get { return titleControl.CanMove; } set { titleControl.CanMove = value; } }
        private bool isBusy = false;
        private int tabIndex = 0;
        private ThicknessAnimation thicknessAnimation = new ThicknessAnimation(new Thickness(0, 60, 0, 0), new Thickness(-340, 60, 0, 0), TimeSpan.FromSeconds(0.25));
        private SolidColorBrush tabBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00A569"));
        private SolidColorBrush buttonBrush1 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00AF7A"));
        private SolidColorBrush buttonBrush2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C6C6C9"));
        public LoginWindow()
        {
            InitializeComponent();
            if (SystemParameters.PrimaryScreenWidth != 1920 && SystemParameters.PrimaryScreenHeight != 1080)
            {
                this.Width *= SystemParameters.PrimaryScreenWidth / 1920;
                this.Height *= SystemParameters.PrimaryScreenHeight / 1080;
                backGd.Children.Remove(mainBd);
                viewBox.Child = mainBd;
                viewBox.Visibility = Visibility.Visible;
            }
            //Common.MergeImage(@"D:\Users\Xp\Documents\CalligraphyAssistant\虎妞书画\Word\156\371\1105-2022_09_19_12_16_49_4225372_77da0020_0c28_4448_9635_102c45aef974.jpg", "d:\\x1.jpg", 1920, 1080, 1);
            //Common.MergeImage(@"D:\Users\Xp\Documents\CalligraphyAssistant\虎妞书画\Word\156\371\1105-2022_09_19_12_16_49_4225372_77da0020_0c28_4448_9635_102c45aef974.jpg", "d:\\x4.jpg", 1920, 1080, 4);
            //Common.MergeImage(@"D:\Users\Xp\Documents\CalligraphyAssistant\虎妞书画\Word\156\371\1105-2022_09_19_12_16_49_4225372_77da0020_0c28_4448_9635_102c45aef974.jpg", "d:\\x8.jpg", 1920, 1080, 8);
        }

        public void HideMinimized()
        {
            titleControl.HideMinimized();
        }

        private void LoginV2()
        {
            if (isBusy)
            {
                return;
            }
            string loginName = loginNameTxt.Text.Trim();
            string password = passwordTxt.Password;
            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(password))
            {
                MessageBoxEx.ShowInfo("用户名和密码不能为空！", this);
                return;
            }
            isBusy = true;
            loginBtn.IsEnabled = false;
            loginBtn.Background = buttonBrush2;
            Task.Run(() =>
            {
                NameValueCollection dict = new NameValueCollection();
                dict.Add("username", loginName);
                dict.Add("password", password);
                string jsonResult = HttpUtility.UploadValues(Common.UserLogin, dict, Encoding.UTF8, Encoding.UTF8);
                ResultInfo<TeacherInfo> result = JsonConvert.DeserializeObject<ResultInfo<TeacherInfo>>(jsonResult);
                if (result != null)
                {
                    if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        dict.Clear();
                        dict.Add("token", result.Body.Token);
                        InitCalligraphyList(dict);
                        jsonResult = HttpUtility.DownloadString(Common.GetClassList, Encoding.UTF8, dict);
                        ResultInfo<ClassInfo[]> result2 = JsonConvert.DeserializeObject<ResultInfo<ClassInfo[]>>(jsonResult);
                        if (result2 != null)
                        {
                            if (result2.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                            {
                                Common.ClassList = result2.Body;
                                //获取班级
                                jsonResult = HttpUtility.UploadValues(Common.GetCourseList, new NameValueCollection(), Encoding.UTF8, Encoding.UTF8, dict);
                                ResultInfo<string[]> result3 = JsonConvert.DeserializeObject<ResultInfo<string[]>>(jsonResult);
                                Common.CourseList = result3.Body;
                                //获取教室
                                string jsonResultV2 = HttpUtility.DownloadString(Common.GetClassRoomListV2 + result.Body.IdSchool, Encoding.UTF8, dict);
                                ResultInfo<ClassRoomV2Info[]> resultV2 = JsonConvert.DeserializeObject<ResultInfo<ClassRoomV2Info[]>>(jsonResultV2);
                                if (resultV2 != null)
                                {
                                    if (resultV2.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Common.ClassRoomV2List = resultV2.Body;
                                        this.Dispatcher.Invoke(() =>
                                        {
                                            Common.CurrentUser = result.Body;
                                            Common.LoginInfo.Account = loginName;
                                            Common.LoginInfo.Password = (bool)rememberPasswordCb.IsChecked ? Common.Encrypt(Common.GetSystemInstallDate() + "s^h&z*s" + password, Consts.Key) : string.Empty;
                                            Common.LoginInfo.RememberPassword = (bool)rememberPasswordCb.IsChecked;
                                            Common.LoginInfo.AutoLogin = (bool)autoLoginCb.IsChecked;
                                            Common.SaveLoginSettings(Common.LoginInfo);
                                            if (!string.IsNullOrEmpty(Common.RtmpServerUrl))
                                            {
                                                Common.ScreenShareUrl = Common.RtmpServerUrl + "live/teacher_screen_" + Common.CurrentUser.Id;
                                            }
                                            this.DialogResult = true;
                                            this.Close();
                                        });
                                    }
                                    else
                                    {
                                        this.Dispatcher.Invoke(() =>
                                        {
                                            MessageBoxEx.ShowError("登录失败,教室信息获取失败！", this);
                                        });
                                    }
                                }
                                else
                                {
                                    this.Dispatcher.Invoke(() =>
                                    {
                                        MessageBoxEx.ShowInfo(resultV2.Msg, this);
                                    });
                                }
                                //ResultInfo<ClassRoomResultInfo> result4 = JsonConvert.DeserializeObject<ResultInfo<ClassRoomResultInfo>>(jsonResult);
                                //if (result4 != null)
                                //{
                                //    if (result4.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                                //    {
                                //        Common.ClassRoomList = result4.Body.ClassRoomList;

                                //        this.Dispatcher.Invoke(() =>
                                //        {
                                //            Common.CurrentUser = result.Body;
                                //            Common.LoginInfo.Account = loginName;
                                //            Common.LoginInfo.Password = (bool)rememberPasswordCb.IsChecked ? Common.Encrypt(Common.GetSystemInstallDate() + "s^h&z*s" + password, Consts.Key) : string.Empty;
                                //            Common.LoginInfo.RememberPassword = (bool)rememberPasswordCb.IsChecked;
                                //            Common.LoginInfo.AutoLogin = (bool)autoLoginCb.IsChecked;
                                //            Common.SaveLoginSettings(Common.LoginInfo);
                                //            this.DialogResult = true;
                                //            this.Close();
                                //        });
                                //    }
                                //    else
                                //    {
                                //        this.Dispatcher.Invoke(() =>
                                //        {
                                //            MessageBoxEx.ShowError("登录失败,教室信息获取失败！", this);
                                //        });
                                //    }
                                //}
                                //else
                                //{
                                //    this.Dispatcher.Invoke(() =>
                                //    {
                                //        MessageBoxEx.ShowInfo(result4.Msg, this);
                                //    });
                                //}
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    MessageBoxEx.ShowInfo(result2.Msg, this);
                                });
                            }
                        }
                        else
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                MessageBoxEx.ShowError("登录失败,教学信息获取失败！", this);
                            });
                        }
                    }
                    else
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            MessageBoxEx.ShowInfo(result.Msg, this);
                        });
                    }
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        MessageBoxEx.ShowError("登录失败,系统错误！", this);
                    });
                }
                isBusy = false;
                this.Dispatcher.Invoke(() =>
                {
                    loginBtn.IsEnabled = true;
                    loginBtn.Background = buttonBrush1;
                });
            });
        }

        private void InitCalligraphyList(NameValueCollection tokenDict)
        {
            Task.Run(() =>
            {
                string jsonResult = HttpUtility.DownloadString(Common.GetDynastyList, Encoding.UTF8, tokenDict);
                ResultInfo<DynastyInfo[]> result1 = JsonConvert.DeserializeObject<ResultInfo<DynastyInfo[]>>(jsonResult);
                if (result1.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    Common.DynastyList = result1.Body;
                }
                jsonResult = HttpUtility.DownloadString(Common.GetFontList, Encoding.UTF8, tokenDict);
                ResultInfo<FontInfo[]> result2 = JsonConvert.DeserializeObject<ResultInfo<FontInfo[]>>(jsonResult);
                if (result2.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    Common.FontList = result2.Body;
                }
                jsonResult = HttpUtility.DownloadString(Common.GetAuthorList, Encoding.UTF8, tokenDict);
                ResultInfo<AuthorInfoInfo[]> result3 = JsonConvert.DeserializeObject<ResultInfo<AuthorInfoInfo[]>>(jsonResult);
                if (result3.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    Common.AuthorList = result3.Body;
                }
            });
        }

        private void Login()
        {
            LoginV2();
            return;
            if (isBusy)
            {
                return;
            }
            string loginName = loginNameTxt.Text.Trim();
            string password = passwordTxt.Password;
            if (string.IsNullOrEmpty(loginName) || string.IsNullOrEmpty(password))
            {
                MessageBoxEx.ShowInfo("用户名和密码不能为空！", this);
                return;
            }
            isBusy = true;
            loginBtn.IsEnabled = false;
            loginBtn.Background = buttonBrush2;
            Task.Run(() =>
            {
                NameValueCollection dict = new NameValueCollection();
                dict.Add("username", loginName);
                dict.Add("password", password);
                string jsonResult = HttpUtility.UploadValues(Common.UserLogin, dict, Encoding.UTF8, Encoding.UTF8);
                ResultInfo<TeacherInfo> result = JsonConvert.DeserializeObject<ResultInfo<TeacherInfo>>(jsonResult);
                if (result != null)
                {
                    if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        dict.Clear();
                        dict.Add("token", result.Body.Token);
                        jsonResult = HttpUtility.DownloadString(Common.GetClassList, Encoding.UTF8, dict);
                        ResultInfo<ClassInfo[]> result2 = JsonConvert.DeserializeObject<ResultInfo<ClassInfo[]>>(jsonResult);
                        if (result2 != null)
                        {
                            if (result2.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                            {
                                Common.ClassList = result2.Body;
                                jsonResult = HttpUtility.DownloadString(Common.GetClassRoomList + result.Body.IdSchool, Encoding.UTF8, dict);
                                ResultInfo<ClassRoomResultInfo> result3 = JsonConvert.DeserializeObject<ResultInfo<ClassRoomResultInfo>>(jsonResult);
                                if (result3 != null)
                                {
                                    if (result3.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                                    {
                                        Common.ClassRoomList = result3.Body.ClassRoomList;
                                        this.Dispatcher.Invoke(() =>
                                        {
                                            Common.CurrentUser = result.Body;
                                            Common.LoginInfo.Account = loginName;
                                            Common.LoginInfo.Password = (bool)rememberPasswordCb.IsChecked ? Common.Encrypt(Common.GetSystemInstallDate() + "s^h&z*s" + password, Consts.Key) : string.Empty;
                                            Common.LoginInfo.RememberPassword = (bool)rememberPasswordCb.IsChecked;
                                            Common.LoginInfo.AutoLogin = (bool)autoLoginCb.IsChecked;
                                            Common.SaveLoginSettings(Common.LoginInfo);
                                            this.DialogResult = true;
                                            this.Close();
                                        });
                                    }
                                    else
                                    {
                                        this.Dispatcher.Invoke(() =>
                                        {
                                            MessageBoxEx.ShowError("登录失败,教室信息获取失败！", this);
                                        });
                                    }
                                }
                                else
                                {
                                    this.Dispatcher.Invoke(() =>
                                    {
                                        MessageBoxEx.ShowInfo(result3.Msg, this);
                                    });
                                }
                            }
                            else
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    MessageBoxEx.ShowInfo(result2.Msg, this);
                                });
                            }
                        }
                        else
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                MessageBoxEx.ShowError("登录失败,教学信息获取失败！", this);
                            });
                        }
                    }
                    else
                    {
                        this.Dispatcher.Invoke(() =>
                        {
                            MessageBoxEx.ShowInfo(result.Msg, this);
                        });
                    }
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        MessageBoxEx.ShowError("登录失败,系统错误！", this);
                    });
                }
                isBusy = false;
                this.Dispatcher.Invoke(() =>
                {
                    loginBtn.IsEnabled = true;
                    loginBtn.Background = buttonBrush1;
                });
            });
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            titleControl.Title = this.Title = "艺学宝登录" + Consts.Version;
            //////屏蔽，刘洋，2024-03-13
            serverTxt.Text = Common.LoginInfo.Server;
            portTxt.Text = Common.LoginInfo.Port;
            //////serverTxt.Text = Common.LoginIP.ToString();
            //////portTxt.Text = Common.LoginPort.ToString();
            loginNameTxt.Text = Common.LoginInfo.Account;
            autoLoginCb.IsChecked = Common.LoginInfo.AutoLogin;
            rememberPasswordCb.IsChecked = Common.LoginInfo.RememberPassword;
            if (Common.LoginInfo.RememberPassword && !string.IsNullOrEmpty(Common.LoginInfo.Password))
            {
                string password = Common.Decrypt(Common.LoginInfo.Password, Consts.Key);
                string[] strArr = password.Split(new string[] { "s^h&z*s" }, StringSplitOptions.RemoveEmptyEntries);
                if (strArr.Length == 2)
                {
                    string installDate = Common.GetSystemInstallDate();
                    if (installDate.Equals(strArr[0]))
                    {
                        passwordTxt.Password = strArr[1];
                    }
                }
            }
            if (Common.LoginInfo.AutoLogin)
            {
                Login();
            }
        }

        private void TitleControl_CloseClick(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void loginTab_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (tabIndex != 0)
            {
                tabIndex = 0;
                thicknessAnimation.From = new Thickness(-324, 40, 0, 0);
                thicknessAnimation.To = new Thickness(0, 40, 0, 0);
                loginWp.BeginAnimation(FrameworkElement.MarginProperty, thicknessAnimation);
                loginTab.BorderBrush = tabBrush;
                settingTab.BorderBrush = Brushes.Transparent;
                loginLb.FontWeight = FontWeights.Bold;
                settingLb.FontWeight = FontWeights.Normal;
            }
        }

        private void settingTab_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (tabIndex != 1)
            {
                tabIndex = 1;
                thicknessAnimation.To = new Thickness(-324, 40, 0, 0);
                thicknessAnimation.From = new Thickness(0, 40, 0, 0);
                loginWp.BeginAnimation(FrameworkElement.MarginProperty, thicknessAnimation);
                loginTab.BorderBrush = Brushes.Transparent;
                settingTab.BorderBrush = tabBrush;
                loginLb.FontWeight = FontWeights.Normal;
                settingLb.FontWeight = FontWeights.Bold;
            }
        }

        private void loginBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Login();
        }

        /// <summary>
        /// 修改保存配置文件内容，刘洋 2024-03-13
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string server = serverTxt.Text.Trim();
            string port = portTxt.Text.Trim();
            if (string.IsNullOrEmpty(server) || string.IsNullOrEmpty(port))
            {
                MessageBoxEx.ShowInfo("服务器地址和端口不能为空！", this);
                return;
            }
            //////Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //////cfa.AppSettings.Settings["LoginIP"].Value = server;
            //////cfa.AppSettings.Settings["LoginPort"].Value = port;
            //////cfa.Save();
            //////ConfigurationManager.RefreshSection("appSettings");
            Common.LoginInfo.Server = server;
            Common.LoginInfo.Port = port;
            Common.SaveLoginSettings(Common.LoginInfo);
            MessageBoxEx.ShowInfo("配置已保存！");
            loginTab_MouseLeftButtonDown(this, null);
        }
    }
}

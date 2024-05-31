using CalligraphyAssistantMain.Code;
using Newtonsoft.Json;
using RabbitMQ.Client.Impl;
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
    public partial class BeginClassControl : UserControl
    {
        public event EventHandler ClassBegined = null;
        private bool isBusy = false;
        private bool isLoaded = false;
        public BeginClassControl()
        {
            InitializeComponent();
        }

        private LessonInfo CreateLesson(int classId, NameValueCollection header)
        {
            LessonInfo[] lessonInfos = GetLessonList(classId, header);
            if (lessonInfos != null && lessonInfos.Length > 0)
            {
                DateTime date;
                if (DateTime.TryParse(lessonInfos[lessonInfos.Length - 1].Created, out date))
                {
                    if (date.AddHours(1) > DateTime.Now)
                    {
                        return lessonInfos[lessonInfos.Length - 1];
                    }
                }
            }
            NameValueCollection data = new NameValueCollection();
            data.Add("idClass", classId.ToString());
            data.Add("name", Common.CurrentUser.Course);
            string jsonResult = HttpUtility.UploadValues(Common.CreateLessonList, data, Encoding.UTF8, Encoding.UTF8, header);
            ResultInfo<LessonInfo> result = JsonConvert.DeserializeObject<ResultInfo<LessonInfo>>(jsonResult);
            if (result != null)
            {
                if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    return result.Body;
                }
                else
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        MessageBoxEx.ShowError(result.Msg, Window.GetWindow(this));
                    });
                }

            }
            else
            {
                this.Dispatcher.Invoke(() =>
                {
                    MessageBoxEx.ShowError("课程信息获取失败！", Window.GetWindow(this));
                });
            }
            return null;
        }

        private LessonInfo[] GetLessonList(int classId, NameValueCollection header)
        {
            string jsonResult = HttpUtility.DownloadString(Common.GetLessonList + classId, Encoding.UTF8, header);
            ResultInfo<LessonInfo[]> result = JsonConvert.DeserializeObject<ResultInfo<LessonInfo[]>>(jsonResult);
            if (result != null)
            {
                if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    return result.Body;
                }
            }
            return null;
        }

        private void classBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            classPop.IsOpen = true;
        }

        private void classRoomBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            classRoomPop.IsOpen = true;
        }

        private void courseBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            coursePop.IsOpen = true;
        }

        private void okBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (classRoomListBox.SelectedIndex == -1)
            {
                MessageBoxEx.ShowInfo("请选择上课的教室！", Window.GetWindow(this));
                return;
            }
            if (classListBox.SelectedIndex == -1)
            {
                MessageBoxEx.ShowInfo("请选择上课的班级！", Window.GetWindow(this));
                return;
            }
            if (courseListBox.SelectedIndex == -1)
            {
                MessageBoxEx.ShowInfo("请选择上课的课程！", Window.GetWindow(this));
                return;
            }
            if (isBusy)
            {
                return;
            }

            isBusy = true;
            //获取流服务器信息
            Common.GetPushServerInfo();
            //获取文件服务器
            Common.GetDocmentServerInfo();
            //获取mq服务信息
            Common.GetMQServerInfo();
            //oss server
            Common.GetOssServerInfo();
            ClassInfo classInfo = (classListBox.SelectedItem as ListBoxItemControl).Tag as ClassInfo;
            ClassRoomV2Info classRoomInfo = (classRoomListBox.SelectedItem as ListBoxItemControl).Tag as ClassRoomV2Info;            
            string course = (courseListBox.SelectedItem as ListBoxItemControl).Tag as string;
            NameValueCollection dict = new NameValueCollection();
            dict.Add("token", Common.CurrentUser.Token);
            Task.Run(() =>
            {
                //获取教室、摄像头、分组和学生信息
                string jsonResult = HttpUtility.DownloadString(Common.GetSeatListV2 + $"?idRoom={classRoomInfo.Id}&idClass={classInfo.Id}", Encoding.UTF8, dict);
                ResultClassRoomV2Info resultClassRoomV2Info = Common.GetClassRoomV2Info(jsonResult);
                ResultInfo<ResultClassRoomV2Info> result = JsonConvert.DeserializeObject<ResultInfo<ResultClassRoomV2Info>>(jsonResult);
                if (result != null)
                {
                    if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        jsonResult = HttpUtility.DownloadString(Common.GetSemester, Encoding.UTF8, dict);
                        ResultInfo<SemesterInfo[]> result2 = JsonConvert.DeserializeObject<ResultInfo<SemesterInfo[]>>(jsonResult);
                        if (result2 != null)
                        {
                            if (result2.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                            {
                                Common.SemesterInfo = result2.Body[0];
                            }
                        }
                        Common.CurrentCourse = course;
                        Common.CurrentClass = classInfo;
                        //Common.CurrentClassV2 = result.Body.ClassData;
                        //Common.CurrentClassRoomV2 = result.Body.ClassRoomData;
                        Common.CurrentClassV2 = resultClassRoomV2Info.ClassData;
                        Common.CurrentClassRoomV2 = resultClassRoomV2Info.ClassRoomData;
                        Common.CurrentLesson = CreateLesson(classInfo.Id, dict);
                        if (Common.CurrentLesson != null)
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                if (ClassBegined != null)
                                {
                                    ClassBegined(this, null);
                                }
                                this.Visibility = Visibility.Collapsed;
                            });
                            //获取快速答题列表  根据课程ID 班级ID 
                            Common.GetQuickAnswer();
                            //获取投票列表
                            Common.GetVoteInfoList();
                            //获取试题
                            Common.GetPaperInfoList();
                            //获取课堂交互
                            Common.GetActiveInfoList();
                            //获取作品
                            Common.GetStudentWorkInfoList();
                            Common.GetTeacherWorkInfoList();
                            SeatV2Info[] _seatV2Info = resultClassRoomV2Info.ClassRoomData.SeatList;
                            if (_seatV2Info.Length > 0)
                            {
                                foreach (SeatV2Info s in _seatV2Info)
                                {
                                    MQCenter.Instance.QueuePurge("student." + s.IP.ToString());
                                }
                            }
                        }
                        else
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                MessageBoxEx.ShowError("教室信息获取失败！", Window.GetWindow(this));
                            });
                        }

                       

                    }
                }
                isBusy = false;
            });
        }

        private void cancelBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == classListBox)
            {
                classPop.IsOpen = false;
                classLb.Text = ((sender as ListBox).SelectedItem as ListBoxItemControl).Text;
            }
            else if (sender == classRoomListBox)
            {
                classRoomPop.IsOpen = false;
                classRoomLb.Text = ((sender as ListBox).SelectedItem as ListBoxItemControl).Text;
            }
            else if (sender == courseListBox)
            {
                coursePop.IsOpen = false;
                courseLb.Text = ((sender as ListBox).SelectedItem as ListBoxItemControl).Text;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (isLoaded)
            {
                return;
            }
            isLoaded = true;
            if (Common.ClassList != null)
            {
                foreach (ClassInfo item in Common.ClassList)
                {
                    ListBoxItemControl itemControl = new ListBoxItemControl()
                    {
                        Width = 240,
                        Text = item.Name,
                        Tag = item
                    };
                    classListBox.Items.Add(itemControl);
                }
            }
            if (Common.ClassRoomV2List != null)
            {
                foreach (ClassRoomV2Info item in Common.ClassRoomV2List)
                {
                    ListBoxItemControl itemControl = new ListBoxItemControl()
                    {
                        Width = 240,
                        Text = item.Name,
                        Tag = item
                    };
                    classRoomListBox.Items.Add(itemControl);
                }
            }
            if (Common.CourseList != null)
            {
                foreach (string item in Common.CourseList)
                {
                    ListBoxItemControl itemControl = new ListBoxItemControl()
                    {
                        Width = 240,
                        Text = item,
                        Tag = item
                    };
                    courseListBox.Items.Add(itemControl);
                }
            }
        }
    }
}

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
    /// RightToolBarControl.xaml 的交互逻辑
    /// </summary>
    public partial class RightToolBarControl : UserControl
    {
        public RightToolBarControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 坐姿提醒 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void remindBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectStudentsControl selectStudentsControl = new SelectStudentsControl();
            selectStudentsControl.BindStudentList(Common.CameraList);
            if (selectStudentsControl.ShowDialog() == true)
            {
                List<StudentInfo> students = new List<StudentInfo>();
                selectStudentsControl.CameraItemInfos.ForEach(p => p.StudentList.ForEach(s =>
                {
                    if (s.IsSelected)
                    {
                        students.Add(s);
                    }

                }));
                if (students.Count > 0)
                {
                    NameValueCollection dict = new NameValueCollection();
                    dict.Add("token", Common.CurrentUser.Token);
                    NameValueCollection data = new NameValueCollection
                {
                    { "id", "0" },
                    { "id_class", Common.CurrentClassV2.ClassId.ToString() },
                    { "id_lesson", Common.CurrentLesson.Id.ToString() },
                    { "member_type", "2" },
                    { "id_member", Common.CurrentUser.Id.ToString() },
                    { "type", "12" },
                    { "describe", "课堂坐姿提醒" },
                    { "content", string.Join(",", students.Select(p => p.Name).ToArray()) }
                };
                    string jsonResult = HttpUtility.UploadValues(Common.SaveTeacherActive, data, Encoding.UTF8, Encoding.UTF8, dict);
                    ResultInfo<string> result = JsonConvert.DeserializeObject<ResultInfo<string>>(jsonResult);
                    if (result != null)
                    {
                        if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            students.ForEach(s =>
                            {
                                MQCenter.Instance.Send(s, MessageType.ReminderPosture, null);
                            });
                            Common.GetActiveInfoList();
                            Common.ShowTip("课堂坐姿提醒成功");
                        }
                    }

                }

            }


        }
        /// <summary>
        /// 随机抽人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pickBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RandomlyDrawPeopleControl drawPeopleControl = new RandomlyDrawPeopleControl("1");
            if (drawPeopleControl.ShowDialog() == true)
            {
                //通知选中的学生
                var selectUser = drawPeopleControl.SelectUser;
                NameValueCollection dict = new NameValueCollection();
                dict.Add("token", Common.CurrentUser.Token);
                NameValueCollection data = new NameValueCollection();
                data.Add("id", "0");
                data.Add("id_class", Common.CurrentClassV2.ClassId.ToString());
                data.Add("id_lesson", Common.CurrentLesson.Id.ToString());
                data.Add("member_type", "1");
                data.Add("id_member", selectUser.Id.ToString());
                data.Add("type", "4");
                data.Add("describe", "课堂随机抽人");
                data.Add("content", "随机抽人");
                string jsonResult = HttpUtility.UploadValues(Common.SaveTeacherActive, data, Encoding.UTF8, Encoding.UTF8, dict);
                ResultInfo<string> result = JsonConvert.DeserializeObject<ResultInfo<string>>(jsonResult);
                if (result != null)
                {
                    if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        MQCenter.Instance.Send(selectUser, MessageType.RandomlySelected, null);
                        Common.GetActiveInfoList();
                        Common.ShowTip("课堂随机抽人成功");
                    }
                }
            }
        }
        /// <summary>
        /// 课堂表扬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void praiseBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectStudentsControl selectStudentsControl = new SelectStudentsControl();
            selectStudentsControl.BindStudentList(Common.CameraList);
            if (selectStudentsControl.ShowDialog() == true)
            {
                List<StudentInfo> students = new List<StudentInfo>();
                selectStudentsControl.CameraItemInfos.ForEach(p => p.StudentList.ForEach(s =>
                {
                    if (s.IsSelected)
                    {
                        students.Add(s);
                    }

                }));
                if (students.Count > 0)
                {
                    NameValueCollection dict = new NameValueCollection();
                    dict.Add("token", Common.CurrentUser.Token);
                    NameValueCollection data = new NameValueCollection();
                    data.Add("id", "0");
                    data.Add("id_class", Common.CurrentClassV2.ClassId.ToString());
                    data.Add("id_lesson", Common.CurrentLesson.Id.ToString());
                    data.Add("member_type", "1");
                    data.Add("id_member", Common.CurrentUser.Id.ToString());
                    data.Add("type", "14");
                    data.Add("describe", "课堂表扬");
                    data.Add("content", string.Join(",", students.Select(p => p.Name).ToArray()));
                    string jsonResult = HttpUtility.UploadValues(Common.SaveTeacherActive, data, Encoding.UTF8, Encoding.UTF8, dict);
                    ResultInfo<string> result = JsonConvert.DeserializeObject<ResultInfo<string>>(jsonResult);
                    if (result != null)
                    {
                        if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            students.ForEach(s =>
                            {
                                MQCenter.Instance.Send(s, MessageType.Praise, null);
                            });
                            Common.GetActiveInfoList();
                            Common.ShowTip("课堂表扬成功");
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 课堂点名
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void callBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NameValueCollection dict = new NameValueCollection();
            dict.Add("token", Common.CurrentUser.Token);
            NameValueCollection data = new NameValueCollection();
            data.Add("id", "0");
            data.Add("id_class", Common.CurrentClassV2.ClassId.ToString());
            data.Add("id_lesson", Common.CurrentLesson.Id.ToString());
            data.Add("member_type", "2");
            data.Add("id_member", Common.CurrentUser.Id.ToString());
            data.Add("type", "13");
            data.Add("describe", "课堂点名");
            data.Add("content", "点名");
            string jsonResult = HttpUtility.UploadValues(Common.SaveTeacherActive, data, Encoding.UTF8, Encoding.UTF8, dict);
            ResultInfo<string> result = JsonConvert.DeserializeObject<ResultInfo<string>>(jsonResult);
            if (result != null)
            {
                if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    MQCenter.Instance.SendToAll(MessageType.RollCall, null);
                    Common.GetActiveInfoList();
                    Common.ShowTip("课堂点名开始");
                }
            }
        }
        /// <summary>
        /// 查看作品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worksBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EventNotify.OnCheckWorks(sender, e);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                MQCenter.Instance.SendToAll(MessageType.ControlOperation, new
                {
                    type = checkBox.IsChecked == true ? 1 : 0,
                });
                if (checkBox.IsChecked == true)
                {
                    checkBox.Background = Brushes.Orange;
                }
                else
                {
                    checkBox.Background = Brushes.Bisque;
                }
            }
        }

        /// <summary>
        /// 下发资料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void materialBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EventNotify.OnMaterialSend(sender, e);
        }
    }
}

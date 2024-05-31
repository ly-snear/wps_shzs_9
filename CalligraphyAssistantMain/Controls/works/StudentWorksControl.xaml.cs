using CalligraphyAssistantMain.Code;
using Microsoft.Win32;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CalligraphyAssistantMain.Controls.works
{
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// StudentWorksControl.xaml 的交互逻辑
    /// </summary>
    public partial class StudentWorksControl : UserControl
    {
        public event Action<StudentWorkDetailsInfo[], int> EditImageClick = null;
        public event Action<StudentWorkDetailsInfo[], int> ImageClick = null;
        public ObservableCollection<StudentWorkDetailsInfo> StudentWorkCollectionPaging { get; set; }
        public Pager<StudentWorkDetailsInfo> Pager { get; set; }
        public StudentWorksControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public void InitData()
        {
            // 获取当前课程学生作品
            Common.GetStudentWorkInfoList();

            if (Common.StudentWorkList.Count > 0)
            {
                view.Visibility = Visibility.Visible;
                Pager = new Pager<StudentWorkDetailsInfo>(Common.StudentWorkList.Count > 8 ? 8 : Common.StudentWorkList.Count, Common.StudentWorkList);
                Pager.PagerUpdated += items =>
                {
                    StudentWorkCollectionPaging = new ObservableCollection<StudentWorkDetailsInfo>(items);
                };
                Pager.CurPageIndex = 1;
            }
            else
            {
                view.Visibility = Visibility.Hidden;
            }
        }

        private void StudentWorkItemControl_EditImageClick(object sender, EventArgs e)
        {
            if (sender is StudentWorkDetailsInfo info)
            {
                EditImageClick?.Invoke(StudentWorkCollectionPaging.ToArray(), StudentWorkCollectionPaging.IndexOf(info));
            }
        }

        private void StudentWorkItemControl_ImageClick(object sender, EventArgs e)
        {
            if (sender is StudentWorkDetailsInfo info)
            {
                ImageClick?.Invoke(StudentWorkCollectionPaging.ToArray(), StudentWorkCollectionPaging.IndexOf(info));
            }
        }

        private void contrastBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            List<StudentWorkDetailsInfo> studentWorks = Common.StudentWorkList.Where(x => x.IsSelected).ToList();
            if (studentWorks.Count > 0)
            {
                EventNotify.OnCheckWorksContrastClick(studentWorks);
            }
        }

        private void UploadBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.InitialDirectory = "c:\\desktop";
                    openFileDialog.Filter = "Image1|*.bmp;*.jepg;*.png;*.mp4;*.avi";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;
                    if (openFileDialog.ShowDialog() == true)
                    {
                        string path = openFileDialog.FileName;
                        Task.Factory.StartNew(() =>
                        {
                            foreach (var studentInfo in students)
                            {
                                string token = string.Empty;
                                //NameValueCollection data = new NameValueCollection();

                                //data.Add("id", studentInfo.Id.ToString());
                                //data.Add("sn", studentInfo.SN);

                                //string jsonResult = HttpUtility.UploadValues(Common.WebAPI+ "/student/login/id/sn", data, Encoding.UTF8, Encoding.UTF8);

                                //ResultInfo<TeacherInfo> result = JsonConvert.DeserializeObject<ResultInfo<TeacherInfo>>(jsonResult);
                                //if (result != null && result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                                //{
                                //    token = result.Body.Token;
                                //}
                                if (Common.SubmitStudentWork(path, studentInfo, token))
                                {
                                    Common.ShowTip("学生作品：" + studentInfo.Name + " 作品上传成功！");
                                }
                                else
                                {
                                    Common.ShowTip("学生作品：" + studentInfo.Name + " 作品上传失败！");
                                }
                            }
                            this.Dispatcher.Invoke(() =>
                            {
                                InitData();
                            });
                        });
                    }
                }

            }


        }
    }
}

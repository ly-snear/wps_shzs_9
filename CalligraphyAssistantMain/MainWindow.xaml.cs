using CalligraphyAssistantMain.Code;
using CalligraphyAssistantMain.Controls;
using CalligraphyAssistantMain.Controls.studentGrouping;
using CalligraphyAssistantMain.Controls.works;
using CommonServiceLocator;
using Google.Protobuf;
using log4net;
using Newtonsoft.Json;
using Prism.Events;
using Prism.Regions;
using Qiniu.Storage;
using SharpDX.DXGI;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.AxHost;
using Application = System.Windows.Application;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace CalligraphyAssistantMain
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Process> ProcessIdList { get; set; } = new List<Process>();
        private NavigationInfo navigationInfo = new NavigationInfo();
        private FloatWindow floatWindow = new FloatWindow();
        private List<StudentInfo> takePhotosList = new List<StudentInfo>();
        private bool isTakePhotosThreadStarted = false;
        private object lockObj = new object();
        private bool isShowMagnifier = false;
        private IEventAggregator eventAggregator;
        private static ILog log = LogManager.GetLogger("Debug");
        private SidebarAction dataBlack;
        private SidebarAction dataWhite;

        public MainWindow()
        {
            InitializeComponent();
            initSwitch();
            //this.Width = viewBox.Width = sendImagesControl.Width = imageListControl.Width = softwareControl.Width = writeScreenControl.Width = cameraListControl.Width = mainControl.Width = beginClassControl.Width =
            //    SystemParameters.PrimaryScreenWidth;
            //this.Height = viewBox.Height = sendImagesControl.Height = imageListControl.Height = softwareControl.Height = writeScreenControl.Height = cameraListControl.Height = mainControl.Height = beginClassControl.Height =
            //    SystemParameters.PrimaryScreenHeight;
            mainGd.Width = sendImagesControl.Width = imageListControl.Width = softwareControl.Width = writeScreenControl.Width = cameraListControl.Width = mainControl.Width = beginClassControl.Width = welcomeControl.Width =
                1920;
            mainGd.Height = sendImagesControl.Height = imageListControl.Height = softwareControl.Height = writeScreenControl.Height = cameraListControl.Height = mainControl.Height = beginClassControl.Height = welcomeControl.Height =
                1080;
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
            if (this.Width != 1920 && this.Height != 1080)
            {
                backGd.Children.Remove(mainGd);
                viewBox.Child = mainGd;
                viewBox.Visibility = Visibility.Visible;
            }
            this.DataContext = cameraListControl.DataContext = mainControl.DataContext = navigationInfo;
            floatWindow.Back += (x, y) =>
            {
                this.WindowState = WindowState.Normal;
                this.ShowMianWindowEvent();
            };
            floatWindow.EndClass += (x, y) => { this.WindowState = WindowState.Normal; EndClass(); };
            floatWindow.TakePhotos += (x, y) => { this.WindowState = WindowState.Normal; navigationInfo.SelectedMainMenu = 1; studentListControl.ShowTakePhotos(); };
            floatWindow.ShowTeachVideo += (x, y) => { this.WindowState = WindowState.Normal; mainControl.ShowTeachVideo(1); };
            floatWindow.SendImage += (x, y) => { this.WindowState = WindowState.Normal; navigationInfo.SelectedMainMenu = 1; sendImagesControl.Visibility = Visibility.Visible; };
            Common.ShowTipCallback += (x) => { tipControl.ShowTip(x); };
            Common.NavigationInfo = navigationInfo;
            EventNotify.CheckWorksClick += EventNotify_CheckWorksClick;
            EventNotify.MaterialSendClick += EventNotify_MaterialSendClick;
            EventNotify.MaterialCloseClick += EventNotify_MaterialCloseClick;
            EventNotify.MaterialCountCloseClick += EventNotify_MaterialCountCloseClick;
            EventNotify.TaskCountCloseClick += EventNotify_TaskCountCloseClick;
            EventNotify.SendCopyBookClick += EventNotify_SendCopyBookClick;
            EventNotify.SendCopyBookCloseClick += EventNotify_SendCopyBookCloseClick;
            EventNotify.CheckShareListClick += EventNotify_CheckShareListClick;
            EventNotify.CheckShareItemClick += EventNotify_CheckShareItemClick;
            EventNotify.CheckWorksContrastClick += EventNotify_CheckWorksContrastClick;
            EventNotify.CheckCameraGroup += EventNotify_CheckCameraGroup;
            EventNotify.CheckCamera += EventNotify_CheckCamera;
            EventNotify.ShowDocEvent += EventNotify_ShowDocEvent;
            EventNotify.CountdownTrigger += EventNotify_CountdownTrigger;
            EventNotify.CountdownStopTrigger += EventNotify_CountdownStopTrigger;
            EventNotify.CameraContrastEvent += EventNotify_CameraContrastEvent;
            EventNotify.CheckCommentListClick += EventNotify_CheckCommentListClick;
            EventNotify.CopyBookPrepareLessonOpenClick += EventNotify_CopyBookPrepareLessonOpenClick;
            EventNotify.CopyBookPrepareLessonCloseClick += EventNotify_CopyBookPrepareLessonCloseClick;
            EventNotify.CopyBookTigerRordOpenClick += EventNotify_CopyBookTigerRordOpenClick;
            EventNotify.CopyBookTigerRordCloseClick += EventNotify_CopyBookTigerRordCloseClick;
            EventNotify.CopyBookTigerOpenClick += EventNotify_CopyBookTigerOpenClick;
            EventNotify.CopyBookTigerCloseClick += EventNotify_CopyBookTigerCloseClick;
            EventNotify.QuickQuestionAnswersCloseClick += EventNotify_QuickQuestionAnswersCloseClick;
            EventNotify.StudentWorkMutualCommenOpenClick += EventNotify_StudentWorkMutualCommenOpenClick;
            EventNotify.StudentWorkMutualCommentCloseClick += EventNotify_StudentWorkMutualCommentCloseClick;
            //Common.CameraGroup1 = new CameraGroupInfo()
            //{
            //    Title = "Test",
            //    CameraGroupItemList = new List<CameraGroupItemInfo>() {
            //        new CameraGroupItemInfo(){Title="G1",PreviewUrl = "1",Url="2" },
            //        new CameraGroupItemInfo(){Title="G2",PreviewUrl = "1",Url="2" },
            //        new CameraGroupItemInfo(){Title="G3",PreviewUrl = "1",Url="2" },
            //    }
            //};
            //Common.XmlSerializeToFile(Common.CameraGroup1,"CameraGroup1.xml");
            //LoginWindow loginWindow = new LoginWindow();
            //if (!(bool)loginWindow.ShowDialog())
            //{
            //    Process.GetCurrentProcess().Kill();
            //}
            FFmpeg.AutoGen.ffmpeg.RootPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Plugins\FFmpeg");
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<MQMessageEvent>().Subscribe(Messaging);

        }

        /// <summary>
        /// 关闭学习任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventNotify_TaskCountCloseClick(object sender, EventArgs e)
        {
            goBackGd.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 关闭拼图统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void EventNotify_MaterialCountCloseClick(object sender, EventArgs e)
        {
            goBackGd.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 关闭学生作品互评
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventNotify_StudentWorkMutualCommentCloseClick(object sender, EventArgs e)
        {
            studentWorkMutualComment.Visibility = Visibility.Collapsed;
            goBackGd.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 打开学生作品互评
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventNotify_StudentWorkMutualCommenOpenClick(object sender, StudentWorkDetailsInfo e)
        {
            //System.Windows.MessageBox.Show("开始执行");
            goBackGd.Visibility = Visibility.Collapsed;
            studentWorkMutualComment.InitData((List<StudentInfo>)(sender as Border).Tag, e);
            studentWorkMutualComment.Visibility = Visibility.Visible;
        }

        private void EventNotify_QuickQuestionAnswersCloseClick(object sender, EventArgs e)
        {
            goBackGd.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 关闭虎妞碑帖
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void EventNotify_CopyBookTigerCloseClick(object sender, EventArgs e)
        {
            copyBookTigerControl.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 打开虎妞碑帖
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void EventNotify_CopyBookTigerOpenClick(object sender, EventArgs e)
        {
            copyBookTigerControl.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 关闭虎妞单字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventNotify_CopyBookTigerRordCloseClick(object sender, EventArgs e)
        {
            copyBookTigerWordControl.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 打开虎妞单字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventNotify_CopyBookTigerRordOpenClick(object sender, EventArgs e)
        {
            copyBookTigerWordControl.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 关闭备课文件选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventNotify_CopyBookPrepareLessonCloseClick(object sender, EventArgs e)
        {
            copyBookPrepareLessonControl.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 打开备课文件选择字帖
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void EventNotify_CopyBookPrepareLessonOpenClick(object sender, EventArgs e)
        {
            copyBookPrepareLessonControl.Visibility = Visibility.Visible;
        }

        private void EventNotify_SendCopyBookCloseClick(object sender, EventArgs e)
        {
            goBackGd.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 打开字帖分享
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventNotify_SendCopyBookClick(object sender, EventArgs e)
        {
            goBackGd.Visibility = Visibility.Collapsed;
            sendCopyBookControl.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 关闭发送素材
        /// 显示返回按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventNotify_MaterialCloseClick(object sender, EventArgs e)
        {
            //goBackGd.Visibility = Visibility.Visible;
            //显示统计控件
            materialCountControl.initData((sender as Border).Tag as MaterialCount);
            materialCountControl.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 发送素材窗口
        /// 隐藏返回按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventNotify_MaterialSendClick(object sender, EventArgs e)
        {
            goBackGd.Visibility = Visibility.Collapsed;
            materialSendControl.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 初始化开关
        /// </summary>
        private void initSwitch()
        {
            try
            {
                #region 绑定黑屏
                dataBlack = new SidebarAction();
                dataBlack.Title = "黑屏锁";
                dataBlack.State = 0;
                System.Windows.Data.Binding binding = new System.Windows.Data.Binding();
                binding.Source = dataBlack;
                binding.Path = new PropertyPath("Title");
                BindingOperations.SetBinding(this.switchBlack, TextBlock.TextProperty, binding);
                #endregion
                #region 绑定白屏
                dataWhite = new SidebarAction();
                dataWhite.Title = "白屏锁";
                dataWhite.State = 0;
                System.Windows.Data.Binding binding2 = new System.Windows.Data.Binding();
                binding2.Source = dataWhite;
                binding2.Path = new PropertyPath("Title");
                BindingOperations.SetBinding(this.switchWhite, TextBlock.TextProperty, binding2);
                #endregion
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void Messaging(Code.Message msg)
        {
            if (msg != null && msg.classId == Common.CurrentClassV2.ClassId && msg.lessonId == Common.CurrentLesson.Id && msg.userType == UserType.student)
            {
                switch (msg.type)
                {
                    case MessageType.CompleteVote:
                        {

                        }
                        break;
                    case MessageType.CompleteQuickAnswer:
                        {

                        }
                        break;
                    case MessageType.PaperAccept:
                        {

                        }
                        break;
                }
            }

        }

        private void EventNotify_CameraContrastEvent()
        {
            cameraContrastControl.Visibility = Visibility.Visible;
            studentCameraGroupControl.Visibility = Visibility.Collapsed;
            studentCameraControl.Visibility = Visibility.Collapsed;
            List<StudentInfo> students = new List<StudentInfo>();
            foreach (var item in Common.CameraList)
            {
                item.StudentList.ForEach(p =>
                {
                    if (p.IsContrast)
                    {
                        students.Add(p);
                    }
                });
            }
            cameraContrastControl.InitData(students);
        }

        private void EventNotify_CountdownTrigger(int id, Action obj)
        {
            return;
            countdownView.Id = id;
            countdownView.Start(action: obj);


        }

        private void EventNotify_CountdownStopTrigger(int id)
        {
            return;
            if (countdownView.Id == id)
            {
                countdownView.Stop();
            }
        }

        private void EventNotify_ShowDocEvent(string obj)
        {
            //goBackGd.Visibility = Visibility.Collapsed;
            //docmentViewControl.Visibility= Visibility.Visible;
            //docmentViewControl.InitData(obj);
            this.WindowState = WindowState.Minimized;
            string processName = "";
            int? processId = 0;
            try
            {
                var filePath = obj;
                ProcessStartInfo file_object = new ProcessStartInfo(filePath);

                Process process = Process.Start(file_object);

                if (process != null && !string.IsNullOrEmpty(process.ProcessName))
                {
                    processName = process.ProcessName;
                    processId = process.Id;
                    if (process?.Id != 0)
                    {
                        ProcessIdList.Add(process);
                        floatWindow.ProcessID = processId;
                        floatWindow.ExternalApp = processName;
                    }
                    Console.WriteLine(string.Format("外部进程id:{0}  名称：{1}", processId, processName));
                }
            }
            catch (Exception)
            {

            }


        }

        private void ShowMianWindowEvent()
        {
            try
            {
                foreach (var pid in ProcessIdList)
                {

                    if (pid.ProcessName == "wps")
                    {
                        Task.Run(() =>
                        {
                            WinAPI_V2.CloseWindow("wps");
                        });
                    }
                    else if (pid.ProcessName == "msedge")
                    {
                        Task.Run(() =>
                        {
                            WinAPI_V2.CloseWindow("msedge");
                        });
                    }
                    else
                    {
                        WinAPI_V2.CloseWindow(pid.ProcessName);
                    }
                }
                ProcessIdList.Clear();
                floatWindow.ProcessID = 0;
                floatWindow.ExternalApp = String.Empty;
            }
            catch (Exception)
            {

            }

        }

        /// <summary>
        /// 开始上课
        /// </summary>
        public void ClassBegined()
        {
            try
            {
                Common.CameraList = new List<CameraItemInfo>();
                for (int i = 0; i < Common.CurrentClassRoomV2.GroupList.Length; i++)
                {
                    GroupV2Info groupV2Info = Common.CurrentClassRoomV2.GroupList[i];
                    CameraItemInfo cameraItemInfo = new CameraItemInfo()
                    {
                        Index = groupV2Info.GroupId,
                        Name = groupV2Info.GroupName,
                    };
                    int number = 1;
                    cameraItemInfo.StudentList = Common.CurrentClassV2.StudentList.Where(p => p.GroupId == groupV2Info.GroupId).OrderBy(p => p.Row).Select(
                        q => new StudentInfo()
                        {
                            Name = q.StudentName,
                            Group = groupV2Info.GroupId,
                            Id = q.StudentId,
                            Number = number++,
                            IP = q.IP,
                            Col = q.Col,
                            Row = q.Row,
                            SN = q.SN,
                            Owner = cameraItemInfo,
                        }).ToList();
                    Common.CameraList.Add(cameraItemInfo);
                }

                Common.StudentList = Common.CameraList.SelectMany(p => p.StudentList).ToArray();
                Task.Run(() =>
                {
                    Console.WriteLine("学生数量：" + Common.StudentList.Count());

                    if (MQCenter.Instance.InitListen(Common.MqServer.host, Common.MqServer.port, Common.MqServer.account, Common.MqServer.password))
                    {
                        Common.ShowTip("消息服务器链接成功");
                        MQCenter.Instance.CreateQueue(string.Format("teacher.{0}.{1}.{2}", Common.CurrentClassV2.ClassId, Common.CurrentLesson.Id, Common.CurrentUser.Id));

                        Common.StudentList.ToList().ForEach(t =>
                        {
                            var data = new
                            {
                                student = t,
                                teacherId = Common.CurrentClassV2.TeacherId,
                                teacherToken = Common.CurrentUser.Token,
                                pushUrl = string.Format("rtmp://{0}:{1}/live/{2}", Common.PushServer.host, Common.PushServer.port, t.Id),
                                studentList = Common.StudentList.ToList(),
                                Lesson = Common.CurrentLesson.Clone(),
                            };
                            MQCenter.Instance.Send(t, MessageType.StartLesson, data);
                        });
                    }
                    else
                    {
                        Common.ShowTip("消息服务器链接失败");
                        Console.WriteLine("MQ消息服务器链接失败[" + Common.MqServer.host + ":" + Common.MqServer.port + " " + Common.MqServer.account + " " + Common.MqServer.password + "]");
                    }
                });

                navigationInfo.IsClassBegin = true;
                cameraListControl.BindList(Common.CameraList);
                Common.CameraList.ForEach(p => { if (p.StudentList.Count > 0) { p.StudentList[0].IsSelected = true; p.SelectedStudent = p.StudentList[0].Name; } });
                imageListControl.Init();
                studentListControl.BindStudentList(Common.CameraList);

                string resourceFolder = $"{Common.SettingsPath}Resource\\{Common.CurrentClassV2.ClassId}\\{Common.CurrentUser.Id}\\{Common.CurrentLesson.Name}\\";
                if (!Directory.Exists(resourceFolder))
                {
                    Directory.CreateDirectory(resourceFolder);
                }
                //mainControl.StartSynchronize(resourceFolder);
                resourceDirectoryControl.StartSynchronize(resourceFolder);
                resourceDirectoryControl.Visibility = Visibility.Visible;

                mainControl.ShareScreen();//屏幕分享
                Common.SocketServer.Start(Common.ServicePort);
            }
            catch (Exception ex)
            {
                Common.Trace("MainWindow beginClassControl_ClassBegined Error:" + ex.Message);
                MessageBoxEx.ShowError("摄像头信息获取失败！", this);
            }
        }

        /// <summary>
        /// 下课
        /// </summary>
        private void EndClass()
        {
            if (MessageBoxEx.ShowQuestion(this, "是否结束上课？", "提示") == MessageBoxResult.Yes)
            {
                cameraListControl.StopAll();
                navigationInfo.IsClassBegin = false;
                navigationInfo.SelectedMainMenu = 2;
                navigationInfo.SelectedShareMenu = -1;
                Common.SocketServer.ShowImageMode();
                Common.SocketServer.StopLiveTransfer();
                Common.SocketServer.StopShareScreen();
                //Common.SocketServer.LogoutAll();
                Common.StudentWorkList.Clear();
                Common.TeacherWorkList.Clear();
                Common.QuickAnswerList.Clear();
                Common.VoteList.Clear();
                Common.PaperList.Clear();
                Common.ActiveList.Clear();
                resourceDirectoryControl.StopSynchronize();
                countdownView.Stop();
                MQCenter.Instance.SendToAll(MessageType.StopLesson, null);
                Task.Run(() => { Thread.Sleep(2000); Common.SocketServer.Stop(); });
                studentsAskQuestionControl.Clear();
            }
        }

        private void CheckStreamTransfer()
        {
            string transferAppPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"StreamTransfer.exe");
            string transferConfigPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"StreamTransfer.xml");
            if (File.Exists(transferAppPath) && File.Exists(transferConfigPath))
            {
                Task.Run(() =>
                {
                    Process process = new Process();
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    process.StartInfo.FileName = transferAppPath;
                    process.StartInfo.Arguments = $"-pid {Process.GetCurrentProcess().Id} -config \"StreamTransfer.xml\"";
                    process.Start();
                });
            }
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int tag = Convert.ToInt32((sender as FrameworkElement).Tag);
            if (tag == 1 && !navigationInfo.IsClassBegin)
            {
                MessageBoxEx.ShowInfo("请选择上课班级！", this);
                beginClassControl.Visibility = Visibility.Visible;
                return;
            }
            if (tag == 3)
            {
                softwareControl.Visibility = Visibility.Visible;
                return;
            }
            navigationInfo.SelectedMainMenu = tag;
        }

        private void mainControl_CameraListClick(object sender, EventArgs e)
        {
            if (!navigationInfo.IsClassBegin)
            {
                MessageBoxEx.ShowInfo("请选择上课班级！", this);
                beginClassControl.Visibility = Visibility.Visible;
                return;
            }
            studentGroupingControl.Visibility = Visibility.Visible;
            studentGroupingControl.InitData();
            //navigationInfo.SelectedMainMenu = 1;
        }

        private void mainControl_SoftwareClick(object sender, EventArgs e)
        {
            softwareControl.Visibility = Visibility.Visible;
        }

        private void cameraListControl_SendImagesClick(object sender, EventArgs e)
        {
            sendImagesControl.Visibility = Visibility.Visible;
        }

        private void cameraListControl_StudentWorksClick(object sender, EventArgs e)
        {
            imageListControl.Visibility = Visibility.Visible;
            imageListControl.ShowWorkList();
        }

        private void cameraListControl_StudentListClick(object sender, EventArgs e)
        {
            studentListControl.Visibility = Visibility.Visible;
        }

        private void cameraListControl_FullScreenClick(object sender, CameraItemInfo e)
        {
            StudentInfo studentInfo = e.StudentList.FirstOrDefault(p => p.IsSelected);
            if (studentInfo != null)
            {
                fullScreenControl.Show(Common.CameraList, studentInfo);
            }
        }

        private void beginClassControl_ClassBegined(object sender, EventArgs e)
        {
            ClassBegined();
        }

        private void WriteScreen()
        {
            leftToolBar.Visibility = Visibility.Collapsed;
            System.Windows.Forms.Application.DoEvents();
            Task.Run(() =>
            {
                Thread.Sleep(200);
                this.Dispatcher.InvokeAsync(() =>
                {
                    writeScreenControl.ClearDrawBoard();
                    writeScreenControl.SetMode(true);
                    writeScreenControl.SetBackground(new ImageBrush(Common.Screenshot()));
                    writeScreenControl.Visibility = Visibility.Visible;
                });
            });
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;
            if (grid != null && grid.Tag != null)
            {
                string title = grid.Tag.ToString();
                switch (title)
                {
                    case "上课":
                        beginClassControl.Visibility = Visibility.Visible;
                        break;
                    case "下课":
                        EndClass();
                        break;
                    case "屏写":
                        WriteScreen();
                        break;
                    case "更多":
                        mainControl.ToggleRightToolbar();
                        break;
                    case "桌面":
                        this.WindowState = WindowState.Minimized;
                        break;
                    case "放大镜":
                        // EventNotify.OnStudentComplete(this, 1122);
                        isShowMagnifier = !isShowMagnifier;
                        if (!isShowMagnifier)
                            MagnifierCircle.Visibility = Visibility.Hidden;
                        break;
                    default:
                        break;
                }
            }
        }

        private void writeScreenControl_Back(object sender, EventArgs e)
        {
            writeScreenControl.Visibility = Visibility.Collapsed;
            leftToolBar.Visibility = Visibility.Visible;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                floatWindow.Left = 0;
                floatWindow.Top = (SystemParameters.PrimaryScreenHeight - 500) / 2;
                floatWindow.Show(navigationInfo.IsClassBegin);
            }
            else
            {
                floatWindow.Hide();
                floatWindow.TimerStop();
                this.Topmost = true;
            }
        }
        AnkHotKey ankHotKey;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //NameValueCollection dict = new NameValueCollection();
            //dict.Add("username", "15012340000");
            //dict.Add("password", "15012340000");
            //string jsonResult = HttpUtility.UploadValues(Consts.UserLogin, dict, Encoding.UTF8, Encoding.UTF8);
            //ResultInfo<TeacherInfo> result = JsonConvert.DeserializeObject<ResultInfo<TeacherInfo>>(jsonResult);
            //if (result != null && result.Ok.Equals("true"))
            //{
            //    Common.CurrentUser = result.Body;
            //    dict.Clear();
            //    dict.Add("token", result.Body.Token);
            //    jsonResult = HttpUtility.DownloadString(Consts.GetClassRoomList + result.Body.IdSchool, Encoding.UTF8, dict);
            //    jsonResult = HttpUtility.DownloadString(Consts.GetClassList, Encoding.UTF8, dict);
            //    jsonResult = HttpUtility.DownloadString(Consts.GetSeatList + 1, Encoding.UTF8, dict);
            //} 
            //InitCCOCX();
            try
            {
                ankHotKey = new AnkHotKey(this, AnkHotKey.KeyFlags.MOD_ALT | AnkHotKey.KeyFlags.MOD_CONTROL, Keys.Q);
                ankHotKey.OnHotKey += () =>
                {
                    if (this.WindowState != WindowState.Minimized && leftToolBar.Visibility != Visibility.Collapsed)
                    {
                        WriteScreen();
                    }
                };
            }
            catch (Exception)
            {
                System.Windows.Forms.MessageBox.Show("快捷键注册失败！");
            }
            mainGd.Visibility = Visibility.Collapsed;
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Owner = this;
            loginWindow.CanMove = false;
            loginWindow.ShowInTaskbar = false;
            loginWindow.HideMinimized();
            if (!(bool)loginWindow.ShowDialog())
            {
                Process.GetCurrentProcess().Kill();
            }
            welcomeControl.Visibility = Visibility.Visible;
            mainGd.Visibility = Visibility.Visible;
            CheckStreamTransfer();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void studentListControl_StudentClick(object sender, EventArgs e)
        {
            StudentInfo studentInfo = (sender as StudentControl).DataContext as StudentInfo;
            CameraItemInfo info = studentInfo.Owner;
            if (studentInfo != null && info != null)
            {
                if (!studentInfo.IsSelected)
                {
                    info.StudentList.ForEach(p => p.IsSelected = false);
                    studentInfo.IsSelected = true;
                    info.SelectedStudent = studentInfo.Name;
                    info.NotifyPropertyChanged("Current");
                }
            }
            fullScreenControl.Show(Common.CameraList, studentInfo);
        }
        private void studentListControl_ResetStudentList(object sender, EventArgs e)
        {
            cameraListControl.ResetStudentList();
        }
        private void sendImagesControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Common.LowConfigurationMode)
            {
                if (sendImagesControl.IsVisible)
                {
                    cameraListControl.StopRenderAll();
                }
                else
                {
                    cameraListControl.StartRenderAll();
                }
            }
        }

        private void studentListControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Common.LowConfigurationMode)
            {
                if (studentListControl.IsVisible)
                {
                    cameraListControl.StopRenderAll();
                }
                else
                {
                    cameraListControl.StartRenderAll();
                }
            }
        }

        private void imageListControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Common.LowConfigurationMode)
            {
                if (imageListControl.IsVisible)
                {
                    cameraListControl.StopRenderAll();
                }
                else
                {
                    cameraListControl.StartRenderAll();
                }
            }
        }

        private void goBackGd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            welcomeControl.Visibility = Visibility.Visible;
        }
        private void SetImageMode()
        {
            Common.SocketServer.ShowImageMode();
            Common.SocketServer.StopShareScreen();
            Common.SocketServer.StopLiveTransfer();
            Common.NavigationInfo.SelectedShareMenu = 4;
        }

        private void sendBlackBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!navigationInfo.IsClassBegin)
            {
                MessageBoxEx.ShowInfo("请选择上课班级！", this);
                beginClassControl.Visibility = Visibility.Visible;
                return;
            }
            SetImageMode();
            MessageType type = MessageType.LockScreen;
            if (0 == dataBlack.State)
            {
                dataBlack.State = 1;
                dataBlack.Title = "黑屏开";
                //type = MessageType.LockScreen;
            }
            else
            {
                dataBlack.State = 0;
                dataBlack.Title = "黑屏锁";
                //type = MessageType.UnLockScreen;
            }
            //Common.SocketServer.LockScreen(false);
            MQCenter.Instance.SendToAll(type, new
            {
                state = 1 == dataBlack.State
            });
        }

        private void sendWhiteBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!navigationInfo.IsClassBegin)
            {
                MessageBoxEx.ShowInfo("请选择上课班级！", this);
                beginClassControl.Visibility = Visibility.Visible;
                return;
            }
            SetImageMode();
            //Common.SocketServer.LockScreen(true);
            MessageType type = MessageType.WhiteScreen;
            if (0 == dataWhite.State)
            {
                dataWhite.State = 1;
                dataWhite.Title = "白屏开";
                //type = MessageType.LockScreen;
            }
            else
            {
                dataWhite.State = 0;
                dataWhite.Title = "白屏锁";
                //type = MessageType.UnLockScreen;
            }
            MQCenter.Instance.SendToAll(type, new
            {
                state = 1 == dataWhite.State
            });
        }
        double _factor = 0.3;

        private void mainGd_MouseMove(object sender, MouseEventArgs e)
        {
            if (isShowMagnifier)
            {
                var center = Mouse.GetPosition(mainGd);
                var length = MagnifierCircle.ActualWidth * _factor;
                var radius = length / 2;
                var viewboxRect = new Rect(center.X - radius, center.Y - radius, length, length);
                MagnifierBrush.Viewbox = viewboxRect;
                MagnifierCircle.SetValue(Canvas.LeftProperty, e.GetPosition(backGd).X - MagnifierCircle.ActualWidth / 2);
                MagnifierCircle.SetValue(Canvas.TopProperty, e.GetPosition(backGd).Y - MagnifierCircle.ActualHeight / 2);
            }
        }

        private void mainGd_MouseEnter(object sender, MouseEventArgs e)
        {
            if (isShowMagnifier)
                MagnifierCircle.Visibility = Visibility.Visible;
        }

        private void mainGd_MouseLeave(object sender, MouseEventArgs e)
        {
            MagnifierCircle.Visibility = Visibility.Hidden;
        }

        private void voteBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!navigationInfo.IsClassBegin)
            {
                MessageBoxEx.ShowInfo("请选择上课班级！", this);
                beginClassControl.Visibility = Visibility.Visible;
                return;
            }
            voteListControl.Visibility = Visibility.Visible;
            voteListControl.InitData();
        }

        private void answerBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!navigationInfo.IsClassBegin)
            {
                MessageBoxEx.ShowInfo("请选择上课班级！", this);
                beginClassControl.Visibility = Visibility.Visible;
                return;
            }
            quickAnswerlistControl.Visibility = Visibility.Visible;
        }

        private void quickAnswerlistControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (quickAnswerlistControl.Visibility == Visibility.Visible)
            {
                quickAnswerlistControl.InitData();
            }
            else
            {
            }
        }


        private void quickAnswerlistControl_AddQuickAnswerClick(object sender, EventArgs e)
        {
            addQuickAnswerControl.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// 快速出题-抢答
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addQuickAnswerControl_QuickResponseClick(object sender, EventArgs e)
        {

            if (sender is string id && !string.IsNullOrEmpty(id))
            {
                QuickAnswerInfo info = Common.QuickAnswerList.FirstOrDefault(p => p.id.ToString().Contains(id));
                info.answer_type = (info.type == "quick" ? 0 : 1);
                info.category = 0;
                if (info != null)
                {
                    //通知所有学生 开始抢答
                    info.answer_students = new List<StudentInfo>();
                    Common.CameraList.ForEach(p => p.StudentList.ForEach(s =>
                    {
                        info.answer_students.Add(s);
                        MQCenter.Instance.Send(s, MessageType.StartQuickAnswer, new
                        {
                            info.id,
                            answer_type = info.type == "quick" ? 0 : 1,
                            type = 0,
                            info.title,
                            info.question,
                            info.content
                        });
                    }
                    ));

                    //倒计时结束
                    /* 关闭倒计时
                    EventNotify.OnCountdownTrigger(info.id, () =>
                    {
                        info.status = "2";
                        MQCenter.Instance.SendToAll(MessageType.StopQuickAnswer, new
                        {
                            info.id,
                            answer_type = info.type == "quick" ? 0 : 1,
                            type = 0,
                            info.title,
                            info.question,
                            info.content
                        });
                    });
                    */
                    info.status = "1";
                    QuickAnswerlistControl_StatisticsClick(info, e);
                }
            }
        }

        /// <summary>
        /// 快速出题-随机抽选学生答题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addQuickAnswerControl_RandomlySelectClick(object sender, EventArgs e)
        {
            if (sender is string id && !string.IsNullOrEmpty(id))
            {
                QuickAnswerInfo info = Common.QuickAnswerList.FirstOrDefault(p => p.id.ToString().Contains(id));
                info.answer_type = (info.type == "quick" ? 0 : 1);
                info.category = 1;
                if (info != null)
                {
                    info.answer_students = new List<StudentInfo>();
                    RandomlyDrawPeopleControl drawPeopleControl = new RandomlyDrawPeopleControl();
                    if (drawPeopleControl.ShowDialog() == true)
                    {
                        //通知选中的学生
                        var selectUser = drawPeopleControl.SelectUser;
                        info.answer_students.Add(selectUser);
                        MQCenter.Instance.Send(selectUser, MessageType.StartQuickAnswer, new
                        {
                            info.id,
                            answer_type = info.type == "quick" ? 0 : 1,
                            type = 1,
                            info.title,
                            info.question,
                            info.content
                        });
                        //倒计时结束
                        /* 关闭倒计时
                        EventNotify.OnCountdownTrigger(info.id, () =>
                        {
                            info.status = "2";
                            MQCenter.Instance.SendToAll(MessageType.StopQuickAnswer, new
                            {
                                info.id,
                                answer_type = info.type == "quick" ? 0 : 1,
                                type = 1,
                                info.title,
                                info.question,
                                info.content
                            });
                        });
                        */
                        info.status = "1";
                        QuickAnswerlistControl_StatisticsClick(info, e);
                    }
                }
            }

        }
        /// <summary>
        /// 快速出题-选择学生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addQuickAnswerControl_SelectUserClick(object sender, EventArgs e)
        {
            if (sender is string id && !string.IsNullOrEmpty(id))
            {
                QuickAnswerInfo info = Common.QuickAnswerList.FirstOrDefault(p => p.id.ToString().Contains(id));
                info.answer_type = (info.type == "quick" ? 0 : 1);
                info.category = 2;
                if (info != null)
                {
                    info.answer_students = new List<StudentInfo>();
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
                                //记录选择的答题学生
                                info.answer_students.Add(s);
                            }
                        }));

                        if (students.Count > 0)
                        {
                            //通知选中的学生
                            students.ForEach(s => MQCenter.Instance.Send(s, MessageType.StartQuickAnswer, new
                            {
                                info.id,
                                answer_type = info.type == "quick" ? 0 : 1,
                                type = 2,
                                info.title,
                                info.question,
                                info.content
                            }));
                            //倒计时结束
                            /* 关闭倒计时
                            EventNotify.OnCountdownTrigger(info.id, () =>
                            {
                                //System.Windows.MessageBox.Show("下发到时消息");
                                info.status = "2";
                                MQCenter.Instance.SendToAll(MessageType.StopQuickAnswer, new
                                {
                                    info.id,
                                    answer_type = info.type == "quick" ? 0 : 1,
                                    type = 2,
                                    info.title,
                                    info.question,
                                    info.content
                                });
                            });
                            */
                            info.status = "1";
                            QuickAnswerlistControl_StatisticsClick(info, e);
                        }
                    }
                }


            }

        }

        private void exercisesBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            addQuickAnswerControl.Visibility = Visibility.Visible;
        }

        private void QuickAnswerlistControl_StatisticsClick(object sender, EventArgs e)
        {
            if (sender is QuickAnswerInfo info)
            {
                NameValueCollection dict = new NameValueCollection();
                dict.Add("token", Common.CurrentUser.Token);
                if (info.type == "quick")
                {
                    var data = new
                    {
                        id = info.id,
                    };
                    string jsonResult = HttpUtility.UploadValuesJson(Common.GetQuickResults, data, Encoding.UTF8, Encoding.UTF8, dict);
                    ResultInfo<ResultCalligraphyListInfo<List<Answer>>> result = JsonConvert.DeserializeObject<ResultInfo<ResultCalligraphyListInfo<List<Answer>>>>(jsonResult);
                    if (result != null && result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        ResultCalligraphyListInfo<List<Answer>> resultCalligraphyListInfo = result.Body;
                        info.students = resultCalligraphyListInfo.Data;
                        goBackGd.Visibility = Visibility.Collapsed;
                        EventNotify.OnQuickQuestionAnswersOpen(sender, info);
                        #region v2 暂停使用

                        /*
                        QuickQuestionAnswers quickQuestionAnswers = new QuickQuestionAnswers(info);                        
                        if (quickQuestionAnswers.ShowDialog() == true)
                        {
                            //System.Windows.MessageBox.Show("状态：" + info.status);
                        }
                        quickQuestionAnswers.Close();
                        //System.Windows.MessageBox.Show("准备发送：" + info.ToJson());
                        */
                        #endregion

                        #region v1 暂停使用
                        /*
                        StatisticControl quickAnswerStatisticsControl = new StatisticControl(info);
                        if (quickAnswerStatisticsControl.ShowDialog() == true)
                        {
                            if (info.status == "1")
                            {
                                PublishAnswerControl publishAnswerControl = new PublishAnswerControl();
                                publishAnswerControl.OptionCount = info.question;                                
                                if (publishAnswerControl.ShowDialog() == true)
                                {
                                    //公布答案
                                    MQCenter.Instance.SendToAll(MessageType.PublishQuickAnswer, new { info.id, info.title, answer_type = info.type == "quick" ? 0 : 1, answer = publishAnswerControl.Answer });
                                }
                            }
                        }
                        */
                        #endregion
                    }

                }
                if (info.type == "subjective")
                {
                    var data = new
                    {
                        id = info.id,
                    };
                    string jsonResult = HttpUtility.UploadValuesJson(Common.GetQuickResults, data, Encoding.UTF8, Encoding.UTF8, dict);
                    ResultInfo<ResultCalligraphyListInfo<List<Answer>>> result = JsonConvert.DeserializeObject<ResultInfo<ResultCalligraphyListInfo<List<Answer>>>>(jsonResult);
                    if (result != null && result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        ResultCalligraphyListInfo<List<Answer>> resultCalligraphyListInfo = result.Body;
                        info.students = resultCalligraphyListInfo.Data;
                        goBackGd.Visibility = Visibility.Collapsed;
                        EventNotify.OnQuickQuestionAnswersOpen(sender, info);
                    }
                }

            }

        }

        private void QuickAnswerlistControl_BeganWritingClick(object sender, EventArgs e)
        {
            if (sender is QuickAnswerInfo info)
            {
                addQuickAnswerControl.Visibility = Visibility.Visible;
                addQuickAnswerControl.InitData(info);
            }
        }

        private void MainControl_PaperClick(object sender, EventArgs e)
        {
            paperControl.Visibility = Visibility.Visible;
            paperControl.InitData();
        }

        private void MainControl_StudentInteractClick(object sender, EventArgs e)
        {
            classInteractionControl.Visibility = Visibility.Visible;
            classInteractionControl.InitData();
        }

        private void MainControl_ShowResourceClick(object sender, EventArgs e)
        {
            resourceDirectoryControl.Visibility = Visibility.Visible;
        }

        private void ResourceDirectoryControl_CheckDistributionClick(object sender, EventArgs e)
        {
            if (sender is ResourceItemInfo info)
            {
                fileDistributionListControl.Visibility = Visibility.Visible;
                fileDistributionListControl.InitData(info);
            }
        }
        private void EventNotify_CheckWorksClick(object sender, EventArgs e)
        {
            if (worksControl.Visibility != Visibility.Visible)
            {
                worksControl.Visibility = Visibility.Visible;
                worksControl.InitData();
            }

        }
        private void EventNotify_CheckShareListClick(StudentWorkDetailsInfo info, string type)
        {
            workShareList.Visibility = Visibility.Visible;
            workShareList.GetShareList(info, type);
        }
        private void EventNotify_CheckShareItemClick(StudentWorkDetailsInfo WorkInfo, ShareInfo arg1, string arg2)
        {
            workReviewControl.Visibility = Visibility.Visible;
            workReviewControl.InitData(WorkInfo, arg1, arg2);
        }
        private void EventNotify_CheckWorksContrastClick(List<StudentWorkDetailsInfo> obj)
        {
            worksContrastControl.Visibility = Visibility.Visible;
            worksContrastControl.InitData(obj);
        }
        private void EventNotify_CheckCommentListClick(List<StudentWorkDetailsInfo> obj)
        {
            WorkCommentList.Visibility = Visibility.Visible;
            WorkCommentList.InitData(obj);
            worksControl.Visibility = Visibility.Collapsed;
        }

        private void EventNotify_CheckCamera(CameraItemInfo camera, StudentInfo obj)
        {
            studentCameraControl.Visibility = Visibility.Visible;
            studentCameraControl.InitData(camera, obj);
        }

        private void EventNotify_CheckCameraGroup(CameraItemInfo obj)
        {
            studentCameraGroupControl.Visibility = Visibility.Visible;
            studentCameraGroupControl.InitData(obj);
        }

        private void DocmentViewControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            goBackGd.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 显示任务统计
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            List<StudentInfo> studentInfos = new List<StudentInfo>();
            foreach (CameraItemInfo g in Common.CameraList)
            {
                foreach (StudentInfo s in g.StudentList)
                {
                    studentInfos.Add(s);
                }
            }
            if (null == studentInfos || studentInfos.Count <= 0)
            {
                System.Windows.MessageBox.Show("没有找到班级学生", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int tm = 3;
            MaterialCount mc = new MaterialCount(tm, studentInfos);
            taskCountControl.initData(mc);
            goBackGd.Visibility = Visibility.Collapsed;
            taskCountControl.Visibility = Visibility.Visible;
        }
    }
}

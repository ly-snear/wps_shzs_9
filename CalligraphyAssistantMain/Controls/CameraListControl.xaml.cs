using CalligraphyAssistantMain.Code;
using CommonServiceLocator;
using Newtonsoft.Json;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    /// CameraListControl.xaml 的交互逻辑
    /// </summary>
    public partial class CameraListControl : UserControl
    {
        private CameraControl[] cell4Arr;
        private CameraControl[] cell9Arr;
        private bool isCell4Mode = false;
        private int pageCount = 0;
        private int pageIndex = 0;
        private BitmapImage cell4Image = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/4Cell.png", UriKind.Absolute));
        private BitmapImage cell4Image2 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/4Cell_2.png", UriKind.Absolute));
        private BitmapImage cell9Image = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/9Cell.png", UriKind.Absolute));
        private BitmapImage cell9Image2 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/9Cell_2.png", UriKind.Absolute));
        private BitmapImage noPhotosImage = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/NoPhotosImage.png", UriKind.Absolute));
        private BitmapImage noPhotosImage2 = new BitmapImage(new Uri("pack://application:,,,/CalligraphyAssistantMain;component/Images/NoPhotosImage_2.png", UriKind.Absolute));
        private List<CameraItemInfo> list;
        public event EventHandler SendImagesClick = null;
        public event EventHandler StudentWorksClick = null;
        public event EventHandler StudentListClick = null;
        public event EventHandler<CameraItemInfo> FullScreenClick = null;

        public CameraListControl()
        {
            InitializeComponent();
            cell4Arr = new CameraControl[] { cameraControl1, cameraControl2, cameraControl4, cameraControl5 };
            cell9Arr = new CameraControl[] {
                cameraControl1, cameraControl2,cameraControl3,
                cameraControl4, cameraControl5,cameraControl6,
                cameraControl7,cameraControl8,cameraControl9
            };
            isCell4Mode = true;
            row3.Height = column3.Width = new GridLength(0);
            IEventAggregator eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<PreviewVideoFrameEvent>().Subscribe(PreviewVideo);
        }

        public void StopAll()
        {
            CameraControl[] tempArr = isCell4Mode ? cell4Arr : cell9Arr;
            for (int i = 0; i < tempArr.Length; i++)
            {
                tempArr[i].Stop();
            }
        }

        public void StopRenderAll()
        {
            CameraControl[] tempArr = isCell4Mode ? cell4Arr : cell9Arr;
            for (int i = 0; i < tempArr.Length; i++)
            {
                tempArr[i].StopRender();
            }
        }

        public void StartRenderAll()
        {
            CameraControl[] tempArr = isCell4Mode ? cell4Arr : cell9Arr;
            for (int i = 0; i < tempArr.Length; i++)
            {
                tempArr[i].StartRender();
            }
        }
        public void BindList(List<CameraItemInfo> list)
        {
            this.list = list;
            ResetPageList();
            GotoPage(0);
            classLb.Text = Common.CurrentClass.Name + " " + Common.CurrentUser.Course;
        }

        private void PreviewVideo(VideoFrameModel model)
        {
            CameraControl[] tempArr = isCell4Mode ? cell4Arr : cell9Arr;
            foreach (CameraControl item in tempArr)
            {
                if (item.ContainsStudent(model.Student.IP))
                {
                    item.UpdateImage(model.Width, model.Height, model.Buffer);
                }
            }
        }

        private void ResetPageList()
        {
            int pageSize = isCell4Mode ? 4 : 9;
            pageIndex = 0;
            pageCount = list.Count / pageSize + (list.Count % pageSize == 0 ? 0 : 1);
        }

        public void ResetStudentList()
        {
            NameValueCollection dict = new NameValueCollection();
            dict.Add("token", Common.CurrentUser.Token);
            Task.Run(() =>
            {
                //获取教室、摄像头、分组和学生信息
                string jsonResult = HttpUtility.DownloadString(Common.GetSeatListV2 + $"?idRoom={Common.CurrentClassRoomV2.RoomId}&idClass={Common.CurrentClassV2.ClassId}", Encoding.UTF8, dict);
                ResultInfo<ResultClassRoomV2Info> result = JsonConvert.DeserializeObject<ResultInfo<ResultClassRoomV2Info>>(jsonResult);
                if (result != null)
                {
                    if (result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        Common.CurrentClassV2 = result.Body.ClassData;
                        List<CameraItemInfo> cameraList = new List<CameraItemInfo>();
                        for (int i = 0; i < Common.CurrentClassRoomV2.GroupList.Length; i++)
                        {
                            GroupV2Info groupV2Info = Common.CurrentClassRoomV2.GroupList[i];
                            CameraItemInfo cameraItemInfo = new CameraItemInfo()
                            {
                                Index = i + 1,
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
                                    Owner = cameraItemInfo,
                                }).ToList();
                            cameraList.Add(cameraItemInfo);
                        }
                        Common.StudentList = cameraList.SelectMany(p => p.StudentList).ToArray();
                        cameraList.ForEach(p => { if (p.StudentList.Count > 0) { p.StudentList[0].IsSelected = true; p.SelectedStudent = p.StudentList[0].Name; } });
                        this.list = cameraList;
                        this.Dispatcher.Invoke(() =>
                        {
                            GotoPage(0, true);
                        });
                    }
                }
            });
        }

        private void GotoPage(int index, bool refresh = false)
        {
            int pageSize = isCell4Mode ? 4 : 9;
            int start = index * pageSize;
            CameraControl[] tempArr = isCell4Mode ? cell4Arr : cell9Arr;
            for (int i = 0; i < pageSize; i++)
            {
                if (list.Count > start + i)
                {
                    if (refresh)
                    {
                        tempArr[i].DataContext = null;
                    }
                    tempArr[i].DataContext = list[start + i];
                    tempArr[i].Play();
                    tempArr[i].Visibility = Visibility.Visible;
                    tempArr[i].FullScreenCallback = CameraListControl_FullScreen;
                }
                else
                {
                    if (refresh)
                    {
                        tempArr[i].DataContext = null;
                    }
                    tempArr[i].DataContext = null;
                    tempArr[i].Stop();
                    tempArr[i].Visibility = Visibility.Collapsed;
                    tempArr[i].FullScreenCallback = null;
                }
            }
            pageIndex = index;
        }

        private void SetImageMode()
        {
            Common.SocketServer.ShowImageMode();
            Common.SocketServer.StopShareScreen();
            Common.SocketServer.StopLiveTransfer();
            Common.NavigationInfo.SelectedShareMenu = 4;
        }

        private void CameraListControl_FullScreen(CameraItemInfo cameraItemInfo)
        {
            if (FullScreenClick != null)
            {
                FullScreenClick(this, cameraItemInfo);
            }
        }

        private void cell4ModeBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!isCell4Mode)
            {
                row3.Height = column3.Width = new GridLength(0);
                Array.ForEach(cell9Arr, p => p.Stop());
                isCell4Mode = true;
                cell4ModeBtn.Source = cell4Image2;
                cell9ModeBtn.Source = cell9Image;
                ResetPageList();
                GotoPage(0);
            }
        }

        private void cell9ModeBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isCell4Mode)
            {
                row3.Height = column3.Width = new GridLength(1, GridUnitType.Star);
                Array.ForEach(cell4Arr, p => p.Stop());
                isCell4Mode = false;
                cell4ModeBtn.Source = cell4Image;
                cell9ModeBtn.Source = cell9Image2;
                ResetPageList();
                GotoPage(0);
            }
        }

        private void nextBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (pageIndex + 1 >= pageCount)
            {
                return;
            }
            GotoPage(++pageIndex);
        }

        private void previousBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (pageIndex - 1 < 0)
            {
                return;
            }
            GotoPage(--pageIndex);
        }

        private void sendImagesBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (SendImagesClick != null)
            {
                SendImagesClick(this, null);
            }
        }

        private void sendBlackBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetImageMode();
            Common.SocketServer.LockScreen(false);
        }

        private void sendWhiteBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetImageMode();
            Common.SocketServer.LockScreen(true);
        }

        private void studentWorksBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //if (StudentWorksClick != null)
            //{
            //    StudentWorksClick(this, null);
            //}
            EventNotify.OnCheckWorks(sender, e);
        }

        private void noPhotosBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Common.IsNoPhotos = !Common.IsNoPhotos;
            noPhotosBtn.Source = Common.IsNoPhotos ? noPhotosImage2 : noPhotosImage;
            noPhotosBtn.ToolTip = Common.IsNoPhotos ? "学生拍照已禁用" : "学生拍照已启用";
        }

        private void studetListBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (StudentListClick != null)
            {
                StudentListClick(this, null);
            }
        }
    }
}

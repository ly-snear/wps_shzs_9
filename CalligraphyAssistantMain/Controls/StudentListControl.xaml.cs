using CalligraphyAssistantMain.Code;
using CommonServiceLocator;
using Newtonsoft.Json;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
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
    /// StudentListControl.xaml 的交互逻辑
    /// </summary>
    public partial class StudentListControl : UserControl
    {
        public event EventHandler StudentClick = null;
        public event EventHandler ResetStudentList = null;
        private int totalPhotos;
        private bool isTakingPhotos = false;
        private bool isDragMode = false;
        private bool isDraging = false; 
        private object lockObj = new object();
        private List<CameraItemInfo> cameraList = null;
        private IEventAggregator eventAggregator;
        private List<StudentInfo> takePhotosList = new List<StudentInfo>();
        private List<StudentControl> studentControls = new List<StudentControl>();
        private List<BlankStudentControl> blankStudentControls = new List<BlankStudentControl>();
        private VisualBrush visualBrush = new VisualBrush() { Stretch = Stretch.None };
        private Point dragOffset = new Point();
        private Point dragPoint = new Point();
        private FrameworkElement fromControl;
        private FrameworkElement toControl;
        private bool hasChanged = false;
        public StudentListControl()
        {
            InitializeComponent();
        }

        public void BindStudentList(List<CameraItemInfo> cameraList)
        {
            backGd.BeginInit();
            ClassRoomV2Info classRoomInfo = Common.ClassRoomV2List.FirstOrDefault(p => p.Id == Common.CurrentClassRoomV2.RoomId);
            if (classRoomInfo != null)
            {
                borderBd.Width = borderGd.Width = classRoomInfo.Cols * 160;
            }
            backGd.Children.Clear();
            studentControls.Clear();
            blankStudentControls.Clear();
            this.cameraList = cameraList;
            WrapPanel wrapPanel = null;
            List<StudentInfo> list = new List<StudentInfo>();
            for (int i = 0; i < cameraList.Count; i++)
            {
                List<StudentInfo> studentList = cameraList[i].StudentList;
                list.AddRange(studentList);
            }
            for (int i = 1; i <= classRoomInfo.Rows; i++)
            {
                wrapPanel = new WrapPanel();
                wrapPanel.HorizontalAlignment = HorizontalAlignment.Center;
                wrapPanel.VerticalAlignment = VerticalAlignment.Bottom;
                wrapPanel.Margin = new Thickness(0, 0, 0, 90 * (i - 1));
                backGd.Children.Add(wrapPanel);
                for (int j = 1; j <= classRoomInfo.Cols; j++)
                {
                    ////行和列是反的
                    //StudentInfo studentInfo = list.FirstOrDefault(p => p.Col == i && p.Row == j); 
                    StudentInfo studentInfo = list.FirstOrDefault(p => p.Col == j && p.Row == i);
                    SeatV2Info seatInfo = Common.CurrentClassRoomV2.SeatList.FirstOrDefault(p => p.Col == i && p.Row == j);
                    if (studentInfo != null)
                    {
                        StudentControl studentControl = new StudentControl();
                        studentControl.Cursor = Cursors.Hand;
                        studentControl.MouseDoubleClick += StudentControl_MouseDoubleClick;
                        studentControl.DataContext = studentInfo;
                        studentControl.Tag = seatInfo;
                        wrapPanel.Children.Add(studentControl);
                        studentControls.Add(studentControl);
                    }
                    else
                    {
                        BlankStudentControl blankStudentControl = new BlankStudentControl()
                        {
                            Visibility = Visibility.Hidden,
                            Tag = seatInfo
                        };
                        if (seatInfo == null)
                        {
                            blankStudentControl.backRect.Fill = Consts.BorderBackColor1;
                        }
                        wrapPanel.Children.Add(blankStudentControl);
                        blankStudentControls.Add(blankStudentControl);
                    }
                }
            }
            backGd.EndInit();
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<TakePhotosEvent>().Subscribe(GetPhoto);
        }
		
		public void ShowTakePhotos()
        {
            this.Visibility = Visibility.Visible;
            imageBtn1_MouseLeftButtonUp(this, null);
        }

        private void GetPhoto(TakePhotosModel model)
        {
            if (isTakingPhotos)
            {
                if (takePhotosList.Contains(model.Student))
                {
                    takePhotosList.Remove(model.Student);
                    this.Dispatcher.InvokeAsync(() =>
                    {
                        progressLb.Text = string.Format("正在拍照，已完成（{0}/{1}）", totalPhotos - takePhotosList.Count, totalPhotos);
                        if (isTakingPhotos && takePhotosList.Count == 0)
                        {
                            MessageBoxEx.ShowInfo("拍照完成！", Window.GetWindow(this));
                            imageBtn2Gd.IsEnabled = true;
                            progressLb.Text = string.Empty;
                        }
                    });
                }
            }
        }

        private void CancelTakePhotos()
        {
            lock (lockObj)
            {
                isTakingPhotos = false;
                takePhotosList.Clear();
                progressLb.Text = string.Empty;
                imageBtn2Gd.IsEnabled = true;
                studentControls.ForEach(p => p.Deselect());
            }
        }

        private void UpdateSeatList()
        {
            this.IsEnabled = false;
            List<object> list = new List<object>();
            List<int> idList = new List<int>();
            foreach (var item in studentControls)
            {
                SeatV2Info seatInfo = (SeatV2Info)item.Tag;
                StudentInfo studentInfo = item.DataContext as StudentInfo;
                if (idList.Contains(studentInfo.Id))
                {
                    continue;
                }
                list.Add(new
                {
                    id_student = studentInfo.Id,
                    id_seat = seatInfo.SeatId
                });
                idList.Add(studentInfo.Id);
            }
            Task.Run(() =>
            {
                var data = new
                {
                    idClass = Common.CurrentClassV2.ClassId,
                    reset = 1,
                    seats = list
                };
                bool hasSaved = false;
                NameValueCollection headerDict = new NameValueCollection();
                headerDict.Add("token", Common.CurrentUser.Token);
                string jsonResult = HttpUtility.UploadValuesJson(Common.SaveSeatListV2, data, Encoding.UTF8, Encoding.UTF8, headerDict);
                if (!string.IsNullOrEmpty(jsonResult))
                {
                    ResultInfo<string> resultInfo = JsonConvert.DeserializeObject<ResultInfo<string>>(jsonResult);
                    if (resultInfo != null && resultInfo.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                    {
                        if (ResetStudentList != null)
                        {
                            ResetStudentList(this, null);
                        }
                        hasSaved = true;
                    }
                }
                this.Dispatcher.Invoke(() =>
                {
                    if (hasSaved)
                    {
                        MessageBoxEx.ShowInfo("座位已保存!");
                    }
                    else
                    {
                        MessageBoxEx.ShowError("座位保存失败!");
                    }
                    this.IsEnabled = true;
                });
            });
        }

        private FrameworkElement GetIntersectControl(Rectangle dragRect)
        {
            Point fromPoint = Mouse.GetPosition(dragRect);
            Rect fromRect = new Rect(fromPoint, new Size(dragRect.ActualWidth, dragRect.ActualHeight));
            Dictionary<FrameworkElement, Rect> controlDict = new Dictionary<FrameworkElement, Rect>();
            foreach (var item in studentControls)
            {
                Point toPoint = Mouse.GetPosition(item);
                Rect toRect = new Rect(toPoint, new Size(item.ActualWidth, item.ActualHeight));
                if (fromRect.IntersectsWith(toRect))
                {
                    controlDict.Add(item, toRect);
                }
            }
            foreach (var item in blankStudentControls)
            {
                Point toPoint = Mouse.GetPosition(item);
                Rect toRect = new Rect(toPoint, new Size(item.ActualWidth, item.ActualHeight));
                if (fromRect.IntersectsWith(toRect))
                {
                    controlDict.Add(item, toRect);
                }
            }
            FrameworkElement tempControl = null;
            foreach (var item in controlDict)
            {
                if (tempControl == null)
                {
                    tempControl = item.Key;
                }
                else
                {
                    Rect rect1 = Rect.Intersect(controlDict[tempControl], fromRect);
                    Rect rect2 = Rect.Intersect(item.Value, fromRect);
                    double size1 = rect1.Width * rect1.Height;
                    double size2 = rect2.Width * rect2.Height;
                    if (size2 > size1)
                    {
                        tempControl = item.Key;
                    }
                }
            }
            if (tempControl != null && tempControl.Tag == null)
            {
                return null;
            }
            return tempControl;
        }

        private void StudentControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (isDragMode)
            {
                return;
            }
            if (StudentClick != null && imageBtn1Gd.Visibility == Visibility.Visible)
            {
                StudentClick(sender, null);
            }
        }

        private void cancelBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (textLb2.Text == "取消")
            {
                textLb1.Text = "调整座位";
                textLb2.Text = "返回";
                studentControls.ForEach(p => { p.Cursor = Cursors.Hand; p.SetToolTip("双击学生全屏查看"); });
                blankStudentControls.ForEach(p => p.Visibility = Visibility.Hidden);
                leftGd.Visibility = Visibility.Visible;
                tipLb1.Visibility = Visibility.Collapsed;
                isDragMode = false;
                BindStudentList(cameraList);
            }
            else
            {
                this.Visibility = Visibility.Collapsed;
                CancelTakePhotos();
                if (imageBtn1Gd.Visibility != Visibility.Visible)
                {
                    imageBtn3_MouseLeftButtonUp(this, null);
                }
            }
        }

        private void imageBtn1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            imageBtn4Gd.Visibility = Visibility.Collapsed;
            imageBtn1Gd.Visibility = Visibility.Collapsed;
            imageBtn2Gd.Visibility = imageBtn3Gd.Visibility = Visibility.Visible;
            studentControls.ForEach(p => p.SetCheckMode(true));
        }

        private void imageBtn2_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            List<StudentInfo> list = studentControls.Where(p => p.IsChecked).Select(p => p.DataContext as StudentInfo).ToList();
            if (list.Count == 0)
            {
                MessageBoxEx.ShowInfo("请至少选择一个学生！", Window.GetWindow(this));
                return;
            }
            isTakingPhotos = true;
            this.takePhotosList = list;
            totalPhotos = takePhotosList.Count;
            imageBtn2Gd.IsEnabled = false;
            progressLb.Text = string.Format("正在拍照，已完成（{0}/{1}）", 0, totalPhotos);
            Array.ForEach(list.ToArray(), p => { Common.SocketServer.TakePhoto(p); });
        }

        private void imageBtn3_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            imageBtn4Gd.Visibility = Visibility.Visible;
            CancelTakePhotos();
            imageBtn1Gd.Visibility = Visibility.Visible;
            imageBtn2Gd.Visibility = imageBtn3Gd.Visibility = Visibility.Collapsed;
            studentControls.ForEach(p => p.SetCheckMode(false));
        }

        private void dragGd_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isDragMode)
            {
                if (e.Source is StudentControl)
                {
                    FrameworkElement element = e.Source as FrameworkElement;
                    visualBrush.Visual = element;
                    dragRect.Width = element.ActualWidth;
                    dragRect.Height = element.ActualHeight;
                    dragRect.Fill = visualBrush;
                    dragRect.Visibility = Visibility.Visible;
                    dragPoint = e.GetPosition(dragGd);
                    dragOffset = e.GetPosition(element);
                    dragRect.Margin = new Thickness(dragPoint.X - dragOffset.X, dragPoint.Y - dragOffset.Y, 0, 0);
                    fromControl = e.Source as StudentControl;
                    (fromControl as StudentControl).SetDragMode(true);
                    isDraging = true;
                }
            }
        }

        private void dragGd_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragMode && isDraging)
            {
                dragPoint = e.GetPosition(dragGd);
                dragRect.Margin = new Thickness(dragPoint.X - dragOffset.X, dragPoint.Y - dragOffset.Y, 0, 0);
                FrameworkElement control = GetIntersectControl(dragRect);
                studentControls.ForEach(p => p.SetFocus(p == control));
                blankStudentControls.ForEach(p => p.SetFocus(p == control));
            }
        }

        private void dragGd_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (fromControl != null)
            {
                if (fromControl is StudentControl)
                {
                    (fromControl as StudentControl).SetDragMode(false);
                }
                toControl = GetIntersectControl(dragRect);
                if (toControl != null)
                {
                    WrapPanel fromWarp = fromControl.Parent as WrapPanel;
                    WrapPanel toWarp = toControl.Parent as WrapPanel;
                    int fromIndex = fromWarp.Children.IndexOf(fromControl);
                    int toIndex = toWarp.Children.IndexOf(toControl);
                    if (fromWarp != toWarp)
                    {
                        fromWarp.Children.Remove(fromControl);
                        toWarp.Children.Remove(toControl);
                        fromWarp.Children.Insert(fromIndex, toControl);
                        toWarp.Children.Insert(toIndex, fromControl);
                    }
                    else
                    {
                        WrapPanel wrapPanel = fromWarp;
                        if (fromIndex > toIndex)
                        {
                            wrapPanel.Children.Remove(fromControl);
                            wrapPanel.Children.Insert(toIndex, fromControl);
                            wrapPanel.Children.Remove(toControl);
                            wrapPanel.Children.Insert(fromIndex, toControl);
                        }
                        else
                        {
                            wrapPanel.Children.Remove(toControl);
                            wrapPanel.Children.Insert(fromIndex, toControl);
                            wrapPanel.Children.Remove(fromControl);
                            wrapPanel.Children.Insert(toIndex, fromControl);
                        }
                    }
                    object temp = fromControl.Tag;
                    fromControl.Tag = toControl.Tag;
                    toControl.Tag = temp;
                }
                fromControl = null;
                toControl = null;
                hasChanged = true;
            }
            studentControls.ForEach(p => p.SetFocus(false));
            blankStudentControls.ForEach(p => p.SetFocus(false));
            dragRect.Visibility = Visibility.Collapsed;
            isDraging = false;
        }

        private void imageBtn4_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!isDragMode)
            {
                textLb1.Text = "保存座位";
                textLb2.Text = "取消";
                studentControls.ForEach(p => { p.Cursor = Cursors.SizeAll; p.SetToolTip("拖动调整学生座位"); });
                blankStudentControls.ForEach(p => p.Visibility = Visibility.Visible);
                leftGd.Visibility = Visibility.Collapsed;
                tipLb1.Visibility = Visibility.Visible;
                isDragMode = true;
                hasChanged = false;
            }
            else
            {
                textLb1.Text = "调整座位";
                textLb2.Text = "返回";
                studentControls.ForEach(p => { p.Cursor = Cursors.Hand; p.SetToolTip("双击学生全屏查看"); });
                blankStudentControls.ForEach(p => p.Visibility = Visibility.Hidden);
                leftGd.Visibility = Visibility.Visible;
                tipLb1.Visibility = Visibility.Collapsed;
                isDragMode = false;
                if (hasChanged)
                {
                    UpdateSeatList();
                }
            }
        }
    }
}

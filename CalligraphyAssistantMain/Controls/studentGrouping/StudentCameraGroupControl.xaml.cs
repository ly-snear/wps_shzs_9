using CalligraphyAssistantMain.Code;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace CalligraphyAssistantMain.Controls.studentGrouping
{
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// StudentCameraGroupControl.xaml 的交互逻辑
    /// </summary>
    public partial class StudentCameraGroupControl : System.Windows.Controls.UserControl
    {
        private List<CameraItemControl> cameraItemControls = new List<CameraItemControl>();
        public ObservableCollection<CameraItemControl> CameraViewCollectionPaging { get; set; }
        public Pager<CameraItemControl> Pager { get; set; }
        public int ViewCount { get; set; }
        public CameraItemInfo Info { get; set; }

        [Obsolete]
        public StudentCameraGroupControl()
        {
            InitializeComponent();
            this.DataContext= this;
            this.view.SizeChanged += View_SizeChanged;
            this.IsVisibleChanged += StudentCameraGroupControl_IsVisibleChanged; 
        }

        private void StudentCameraGroupControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            cameraItemControls.ForEach(p => p.StopPreview());
            cameraItemControls.Clear();
        }

        [Obsolete]
        private void View_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Init(4);
        }

        [Obsolete]
        public void InitData(CameraItemInfo cameraItem)
        {
            Info = cameraItem;
            cameraItemControls = new List<CameraItemControl>();
            foreach(var student in Info.StudentList)
            {
                var item = new CameraItemControl();
                item.InitData(student, cameraItem.Name);
                cameraItemControls.Add(item);
                item.FullScreenCallback += FullScreenCallback;
            }
            this.Init(4);
        }

        [Obsolete]
        private void FullScreenCallback(StudentInfo student)
        {
            fullScreenControl.Show(student, Info.Name);
            fullScreenControl.Visibility = Visibility.Visible;
        }
        private void ReturnBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        [Obsolete]
        public void Init(int count = 4)
        {
            ViewCount = count;
            if(cameraItemControls.Count > 0)
            {
                Pager = new Pager<CameraItemControl>(cameraItemControls.Count > count ? count : cameraItemControls.Count, cameraItemControls);
                Pager.PagerUpdated += items =>
                {
                    if(CameraViewCollectionPaging!=null)
                    {
                        foreach (var item in CameraViewCollectionPaging.ToList())
                        {
                          if (items.FirstOrDefault(p => p.StudentCamera.Id == item.StudentCamera.Id)==null)
                            {
                                item.StopPreview();
                                item.StopRecord();
                            }
                        }
                    }
                    CameraViewCollectionPaging = new ObservableCollection<CameraItemControl>(items);
                    foreach (var item in CameraViewCollectionPaging)
                    {
                        if (ViewCount == 4)
                        {
                            item.Height = view.ActualHeight/2;
                            item.Width = view.ActualWidth / 2;
                        }
                        if (ViewCount == 9)
                        {
                            item.Height = view.ActualHeight / 3;
                            item.Width = view.ActualWidth / 3;
                        }
                        item.StartPreview();
                    }
                };
                Pager.CurPageIndex = 1;
            }
        }

        [Obsolete]
        private void NineBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ViewCount != 9)
            {
                this.Init(9);
            }
        }

        [Obsolete]
        private void FourBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ViewCount != 4)
            {
                this.Init(4);
            }
        }

        /// <summary>
        /// 上一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviousBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(Pager.CurPageIndex<= Pager.PageCount && Pager.CurPageIndex >1 )
            Pager.CurPageIndex--;
        }

        /// <summary>
        /// 下一页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Pager.CurPageIndex < Pager.PageCount && Pager.CurPageIndex >= 1)
                Pager.CurPageIndex++;
        }
    }
}

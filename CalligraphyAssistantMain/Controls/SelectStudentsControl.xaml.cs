using CalligraphyAssistantMain.Code;
using PropertyChanged;
using System;
using System.Collections.Generic;
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
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// SelectStudentsControl.xaml 的交互逻辑
    /// </summary>
    public partial class SelectStudentsControl :Window
    {
        public List<CameraItemInfo> CameraItemInfos { get; set; } = new List<CameraItemInfo>();

        public SelectStudentsControl()
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
            this.DataContext = this;

        }

        public void BindStudentList(List<CameraItemInfo> data)
        {
            CameraItemInfos = data.Clone();
            CameraItemInfos.ForEach(item =>
            {
                item.IsSelected = true;
                item.StudentList.ForEach(s =>
                {
                    s.PropertyChanged += PropertyChanged;
                    s.IsSelected = item.IsSelected;
                }
                );
                item.PropertyChanged += PropertyChanged;
            });

        }

        private void PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
          if(e.PropertyName == "IsSelected"&& sender is CameraItemInfo info)
            {
                info.StudentList.ForEach(s => s.IsSelected = info.IsSelected);
            }
            if (e.PropertyName == "IsSelected" && sender is StudentInfo student)
            {
                int count=0;
                 CameraItemInfos.ForEach(item => item.StudentList.ForEach(s =>
                 {
                     if(s.IsSelected)
                         count++;
                 }));
                selectCount.Text = count.ToString();
            }
        }

        private void closeBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btn_cancel_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult =false;
        }

        private void btn_ok_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = true;
        }

        private void selectAllBtn_Checked(object sender, RoutedEventArgs e)
        {
            CameraItemInfos.ForEach(item =>
            {
                item.IsSelected = true;
            });
        }

        private void invertBtn_Checked(object sender, RoutedEventArgs e)
        {
            CameraItemInfos.ForEach(item =>
            {
                item.StudentList.ForEach(s => s.IsSelected = !s.IsSelected);
               
            });
        }

        private void uncheckBtn_Checked(object sender, RoutedEventArgs e)
        {
            CameraItemInfos.ForEach(item =>
            {
                item.IsSelected = false;
                item.StudentList.ForEach(s => s.IsSelected = false);
            });
        }
    }
}

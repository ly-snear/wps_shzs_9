using CalligraphyAssistantMain.Code;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalligraphyAssistantMain.Controls
{
    /// <summary>
    /// MaterialSendControl.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class MaterialSendControl : UserControl
    {
        /// <summary>
        /// 学生分组列表
        /// </summary>
        public List<CameraItemInfo> CameraItemInfos { get; set; } = new List<CameraItemInfo>();

        public ObservableCollection<ResourceFile> ResourceItemInfos { get; set; } = new ObservableCollection<ResourceFile>();

        public string[] shh = new string[] { "png", "jpg", "jpeg", "gif", "bmp", "tiff", "svg", "webp" };

        public int optionTime { get; set; } = 1;

        public MaterialSendControl()
        {
            InitializeComponent();
            this.IsVisibleChanged += MaterialSendControl_IsVisibleChanged;
        }

        private void MaterialSendControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                BindStudentList();
                BindResourceList();
                BindSandDrawList();
                t1.DataContext = this;
            }
        }

        public void BindStudentList()
        {
            if (null == Common.CameraList || 0 == Common.CameraList.Count)
            {
                MessageBox.Show("没有找到学生", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CameraItemInfos = Common.CameraList.Clone();
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
            //MessageBox.Show(CameraItemInfos.ToJson());
            this.DataContext = this;
            //Console.WriteLine(CameraItemInfos.ToJson());
        }

        public void BindResourceList()
        {
            if (null == Common.resourceItemInfos || 0 == Common.resourceItemInfos.Count)
            {
                MessageBox.Show("没有找到备课资源", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //ResourceItemInfos = new List<ResourceFile>();
            ResourceItemInfos.Clear();
            Common.resourceItemInfos.ForEach(r =>
            {
                ResourceItemInfos.Add(new ResourceFile(r.ServerId, r.FileName, r.ServerUrl, false));
            });
            //MessageBox.Show(ResourceItemInfos.ToJson());
        }

        public void BindSandDrawList()
        {
            if (null == Common.resourceItemInfos || 0 == Common.resourceItemInfos.Count)
            {
                MessageBox.Show("没有找到备课资源", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            ResourceItemInfos.Clear();
            Common.resourceItemInfos.ForEach(r =>
            {
                string[] exts = r.ServerUrl.Split(new char[] { '.' });
                string ext = exts[exts.Length - 1].ToLower();
                if (shh.Contains(ext))
                {
                    ResourceItemInfos.Add(new ResourceFile(r.ServerId, r.FileName, r.ServerUrl, false));
                }
            });
            //MessageBox.Show(ResourceItemInfos.ToJson());
        }

        private void PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected" && sender is CameraItemInfo info)
            {
                info.StudentList.ForEach(s => s.IsSelected = info.IsSelected);
            }
            if (e.PropertyName == "IsSelected" && sender is StudentInfo student)
            {
                int count = 0;
                CameraItemInfos.ForEach(item => item.StudentList.ForEach(s =>
                {
                    if (s.IsSelected)
                        count++;
                }));
                selectCount.Text = count.ToString();
            }
        }

        private void returnBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            EventNotify.OnMaterialClose(sender, e);
        }

        private void sendBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            #region 学生列表
            List<StudentInfo> students = new List<StudentInfo>();
            CameraItemInfos.ForEach(g =>
            {
                g.StudentList.ForEach(s =>
                {
                    if (s.IsSelected)
                    {
                        students.Add(s);
                    }
                });
            });
            if (null == students || 0 == students.Count)
            {
                MessageBox.Show("选择要发送的学生", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            List<ResourceFile> resourceFiles = new List<ResourceFile>();
            for (int i = 0; i < ResourceItemInfos.Count; i++)
            {
                if (ResourceItemInfos[i].isSelect)
                {
                    resourceFiles.Add(ResourceItemInfos[i]);
                }
            }
            if (null == resourceFiles || 0 == resourceFiles.Count)
            {
                MessageBox.Show("选择要发送的沙画拼图素材", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            #endregion

            #region 兼容备课
            /*
            MessageType type = MessageType.ShareMaterial;
            if (getShaHua())
            {
                type = MessageType.ShareSandDraw;
            }
            */
            #endregion

            #region 发送消息
            MessageType type = MessageType.ShareSandDraw;
            students.ForEach(s =>
            {
                MQCenter.Instance.Send(s, type, resourceFiles);
            });
            Common.ShowTip("沙画拼图素材推送完成");
            #endregion

            #region 获取拼图时间
            int time = optionTime;
            int _time = -1;
            if (int.TryParse(ctime.Text, out _time) && _time > 0)
            {
                time = _time;
            }
            //MessageBox.Show("耗时：" + time);
            #endregion

            MaterialCount mc = new MaterialCount(time, students);
            (sender as Border).Tag = mc;
            this.Visibility = Visibility.Collapsed;
            EventNotify.OnMaterialClose(sender, e);
        }

        /// <summary>
        /// 判断是否仅有沙画
        /// </summary>
        /// <returns></returns>
        private bool getShaHua()
        {
            bool? shh = onlyShaHua.IsChecked;
            if (shh.HasValue)
            {
                return shh.Value;
            }
            return false;
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectAllBtn_Checked(object sender, RoutedEventArgs e)
        {
            CameraItemInfos.ForEach(item =>
            {
                item.IsSelected = true;
            });
        }

        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void invertBtn_Checked(object sender, RoutedEventArgs e)
        {
            CameraItemInfos.ForEach(item =>
            {
                item.StudentList.ForEach(s => s.IsSelected = !s.IsSelected);

            });
        }

        /// <summary>
        /// 取消选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uncheckBtn_Checked(object sender, RoutedEventArgs e)
        {
            CameraItemInfos.ForEach(item =>
            {
                item.IsSelected = false;
                item.StudentList.ForEach(s => s.IsSelected = false);
            });
        }

        private void onlyShaHua_Checked(object sender, RoutedEventArgs e)
        {
            BindSandDrawList();
        }

        private void onlyShaHua_Unchecked(object sender, RoutedEventArgs e)
        {
            BindResourceList();
        }
    }
}

using CalligraphyAssistantMain.Code;
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
    /// <summary>
    /// StudentControl.xaml 的交互逻辑
    /// </summary>
    public partial class StudentControl : UserControl
    {
        private bool isCheckMode = false;
        private bool isDragMode = false;
        private bool isComplete = false;
        private long id;
        public bool IsChecked { get { return (bool)checkBox.IsChecked; } }
        /// <summary>
        /// 座位学生是否完成学习任务
        /// 有学生自行提交
        /// </summary>
        public bool IsComplete
        {
            get { return isComplete; }
            set
            {
                isComplete = value;
            }
        }

        /// <summary>
        /// 提交任务的学生ID
        /// </summary>
        public long Id
        {
            get { return id; }
            set
            {
                id = value;
            }
        }
        public StudentControl()
        {
            InitializeComponent();
            checkBox.Visibility = Visibility.Collapsed;
        }

        public void SetFocus(bool isFocus)
        {
            if (isFocus)
            {
                focusRect.Visibility = Visibility.Visible;
            }
            else
            {
                focusRect.Visibility = Visibility.Collapsed;
            }
        }

        public void SetCheckMode(bool isCheckMode)
        {
            this.isCheckMode = isCheckMode;
            checkBox.Visibility = isCheckMode ? Visibility.Visible : Visibility.Collapsed;
        }

        public void Deselect()
        {
            checkBox.IsChecked = false;
        }

        public void SetDragMode(bool isDragMode)
        {
            if (isDragMode)
            {
                rectBd1.Background = rectBd2.Background = Consts.StudentControlColor2;
            }
            else
            {
                rectBd1.Background = rectBd2.Background = Consts.StudentControlColor1;
            }
            this.isDragMode = isDragMode;
        }

        public void SetToolTip(string toolTip)
        {
            tipLb.ToolTip = toolTip;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (isCheckMode)
            {
                checkBox.IsChecked = !checkBox.IsChecked;
            }
        }
    }
}

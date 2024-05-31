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

namespace CalligraphyAssistantMain.Controls.works
{
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// WorksControl.xaml 的交互逻辑
    /// </summary>
    public partial class WorksControl : UserControl
    {
        public WorksControl()
        {
            InitializeComponent();
            studentWorksControl.EditImageClick += StudentWorksControl_EditImageClick;
            studentWorksControl.ImageClick += StudentWorksControl_ImageClick;
            teacherWorksControl.ImageClick += StudentWorksControl_ImageClick;
        }

        private void StudentWorksControl_ImageClick(Code.StudentWorkDetailsInfo[] arg1, int arg2)
        {
            imageViewControl.ShowImage(arg1, arg2);

        }

        private void StudentWorksControl_EditImageClick(Code.StudentWorkDetailsInfo[] arg1,int arg2)
        {
            imageEditControl.EditImages(arg1, arg2);
        }

        public void InitData()
        {
            studentWorksControl.InitData();
            teacherWorksControl.InitData();
        }
        private void ReturnBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void imageEditControl_Back(object sender, EventArgs e)
        {

        }
    }
}

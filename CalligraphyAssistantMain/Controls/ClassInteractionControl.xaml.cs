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
using CalligraphyAssistantMain.Code;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace CalligraphyAssistantMain.Controls
{
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// ClassInteractionControl.xaml 的交互逻辑
    /// </summary>
    public partial class ClassInteractionControl : UserControl
    {
        public ObservableCollection<ActiveInfo> InteractionCollectionPaging { get; set; }
        public Pager<ActiveInfo> Pager { get; set; }
        public ClassInteractionControl()
        {
            InitializeComponent();
            this.DataContext=this;
        }
        public void InitData()
        {
            Common.GetActiveInfoList();

            if (Common.ActiveList.Count > 0)
            {
                view.Visibility = Visibility.Visible;
                Pager = new Pager<ActiveInfo>(Common.ActiveList.Count > 8 ? 8 : Common.ActiveList.Count, Common.ActiveList);
                Pager.PagerUpdated += items =>
                {
                    InteractionCollectionPaging = new ObservableCollection<ActiveInfo>(items);
                };
                Pager.CurPageIndex = 1;
            }
            else
            {
                view.Visibility = Visibility.Hidden;
            }

        }

        private void statistics_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is ActiveInfo info)
            {
              
            }
        }

        private void ReturnBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility=Visibility.Collapsed;
        }


    }
}

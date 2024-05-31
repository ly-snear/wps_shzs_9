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
    /// ItemCopyBook.xaml 的交互逻辑
    /// </summary>
    public partial class ItemCopyBook : UserControl
    {
        public ItemCopyBook()
        {
            InitializeComponent();
        }

        private void deleteBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ShareCopyBook rf = (sender as Image).Tag as ShareCopyBook;
            if (rf == null || string.IsNullOrEmpty(rf.Id) || string.IsNullOrEmpty(rf.Url))
            {
                return;
            }
            //MessageBox.Show(rf.ToJson());
            SendCopyBookControl.OnDeleteCopyBook(sender, rf);
        }
    }
}

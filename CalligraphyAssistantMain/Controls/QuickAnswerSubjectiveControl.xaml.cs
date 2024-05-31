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
    /// QuickAnswerSubjectiveControl.xaml 的交互逻辑
    /// </summary>
    public partial class QuickAnswerSubjectiveControl : UserControl
    {
        /// <summary>
        /// 播放学生声音答案
        /// </summary>
        public event EventHandler<string> PlayAudioClick = null;

        public QuickAnswerSubjectiveControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void play_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string url = (sender as Label).Tag as string;
            Console.WriteLine(url);
            PlayAudioClick?.Invoke(sender, url);
        }
    }
}

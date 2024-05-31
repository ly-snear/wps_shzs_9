using CalligraphyAssistantMain.Code;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
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
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// RandomlyDrawPeopleControl.xaml 的交互逻辑
    /// </summary>
    public partial class RandomlyDrawPeopleControl : Window
    {
        private readonly System.Timers.Timer timer = new System.Timers.Timer(500);
        private readonly Random Ran = new Random();
        public StudentInfo SelectUser { get; set; }
        public RandomlyDrawPeopleControl(string type=null)
        {
            InitializeComponent();
            if(type == "1")
            {
                publishBtnTxt.Text = "确认";
            }
            this.Owner = Application.Current.MainWindow;
            timer.Elapsed += Timer_Elapsed;   
            timer.AutoReset = true;
            this.Loaded += RandomlyDrawPeopleControl_Loaded;
        }

        private void RandomlyDrawPeopleControl_Loaded(object sender, RoutedEventArgs e)
        {
            timer.Start();
            startBtn.IsEnabled = false;
            stopBtn.IsEnabled = true;
            publishBtn.IsEnabled = false;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (Common.StudentList.Count() > 0)
            {
                this.Dispatcher.Invoke(() =>
                {
                    SelectUser = Common.StudentList[Ran.Next(0, Common.StudentList.Count() - 1)];
                    this.userName.Text = SelectUser.Name;
                });
            }
        }
        private void closeBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = false;
            timer.Stop();
            startBtn.IsEnabled = true;
            stopBtn.IsEnabled = false;
            publishBtn.IsEnabled = false;

        }

        private void startBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            timer.Start();
            startBtn.IsEnabled = false;
            stopBtn.IsEnabled = true;
            publishBtn.IsEnabled = false;
  
        }

        private void stopBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            timer.Stop();
            startBtn.IsEnabled = true;
            stopBtn.IsEnabled = false;
            publishBtn.IsEnabled = true;
        
        }

        private void publishBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
           
            if (!string.IsNullOrEmpty(this.userName.Text))
            {
                this.DialogResult = true;
            }
        }
    }
}

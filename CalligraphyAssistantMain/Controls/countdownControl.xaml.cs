using CalligraphyAssistantMain.Code;
using CalligraphyAssistantMain.Controls;
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
using System.Windows.Threading;

namespace CalligraphyAssistantMain.Controls
{
    /// <summary>
    /// countdownControl.xaml 的交互逻辑
    /// </summary>
    public partial class countdownControl : UserControl
    {
        private DispatcherTimer timer;
        private ProcessCount processCount;
        private Action action=null;
        public bool IsStart { get; set; }
        public int Id {  get; set; }
        public countdownControl()
        {
            InitializeComponent();
        }

        public void Start(int second = 180, Action action=null)
        {
            if (this.action == null)
            {
                IsStart=true;
                this.action = action;
             
                timer = new DispatcherTimer();
                timer.Interval = new TimeSpan(10000000);
                timer.Tick += new EventHandler(timer_Tick);

                processCount = new ProcessCount(second);
                HourArea.Text = processCount.GetHour();
                MinuteArea.Text = processCount.GetMinute();
                SecondArea.Text = processCount.GetSecond();
                CountDown += new CountDownHandler(processCount.ProcessCountDown);
                timer.Start();
                this.Visibility = Visibility.Visible;
            }
           
        }
        public void Stop()
        {
            IsStart=false;
            this.Visibility = Visibility.Collapsed;
            timer?.Stop();
            action = null;
        }

        /// <summary>
        /// Timer触发的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (OnCountDown())
            {
                HourArea.Text = processCount.GetHour();
                MinuteArea.Text = processCount.GetMinute();
                SecondArea.Text = processCount.GetSecond();
            }
            else
            {
                IsStart = false;
                this.Visibility = Visibility.Collapsed;
                timer.Stop();
                action?.Invoke();
                action = null;
            }
        }

             /// <summary>
        /// 处理事件
        /// </summary>
        public event CountDownHandler CountDown;
        public bool OnCountDown()
        {
            if (CountDown != null)
               return CountDown();

            return false;
        }
    }

    /// <summary>
    /// 处理倒计时的委托
    /// </summary>
    /// <returns></returns>
    public delegate bool CountDownHandler();
    

    public class ProcessCount
    {
        private Int32 _TotalSecond;
        public Int32 TotalSecond
        {
            get { return _TotalSecond; }
            set { _TotalSecond = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProcessCount(Int32 totalSecond)
        {
            this._TotalSecond = totalSecond;
        }

        /// <summary>
        /// 减秒
        /// </summary>
        /// <returns></returns>
        public bool ProcessCountDown()
        {
            if (_TotalSecond == 0)
                return false;
            else
            {
                _TotalSecond--;
                return true;
            }
        }

        /// <summary>
        /// 获取小时显示值
        /// </summary>
        /// <returns></returns>
        public string GetHour()
        {
            return String.Format("{0:D2}", (_TotalSecond / 3600));
        }

        /// <summary>
        /// 获取分钟显示值
        /// </summary>
        /// <returns></returns>
        public string GetMinute()
        {
            return String.Format("{0:D2}", (_TotalSecond % 3600) / 60);
        }

        /// <summary>
        /// 获取秒显示值
        /// </summary>
        /// <returns></returns>
        public string GetSecond()
        {
            return String.Format("{0:D2}", _TotalSecond % 60);
        }

    }
}

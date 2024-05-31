using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalligraphyAssistantMain.Controls
{
    /// <summary>
    /// TipControl.xaml 的交互逻辑
    /// </summary>
    public partial class TipControl : UserControl
    {
        private string currentTip = string.Empty;
        private Timer timer = new Timer();
        private int times = 0;
        private bool isHide = true;
        public TipControl()
        {
            InitializeComponent();
            timer.Interval = 1000;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        public void ShowTip(string tip)
        {
            if (string.IsNullOrEmpty(tip))
            {
                currentTip = string.Empty;
                return;
            }
            //if (currentTip == tip)
            //{
            //    return;
            //}
            currentTip = tip;
            this.Dispatcher.Invoke(new Action(() =>
            {
                this.Visibility = System.Windows.Visibility.Visible;
                _tip.Text = tip;
                times = 0;
                DoubleAnimation doubleAnimation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(0.15)));
                this.BeginAnimation(FrameworkElement.OpacityProperty, doubleAnimation);
                isHide = false;
            }),System.Windows.Threading.DispatcherPriority.Send);
        }

        private void HideTip()
        { 
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                DoubleAnimation doubleAnimation = new DoubleAnimation(1, 0, new Duration(TimeSpan.FromSeconds(0.15)));
                doubleAnimation.Completed += (x, y) =>
                {
                    this.Visibility = System.Windows.Visibility.Collapsed;
                };
                this.BeginAnimation(FrameworkElement.OpacityProperty, doubleAnimation);
                isHide = true;
            }));
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            times++;
            if (!isHide && times >=3)
            {
                HideTip();
            }
        }
    }
}

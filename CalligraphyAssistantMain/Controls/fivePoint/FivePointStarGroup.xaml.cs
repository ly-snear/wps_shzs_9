﻿using System;
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

namespace CalligraphyAssistantMain.Controls.fivePoint
{
    /// <summary>
    /// FivePointStarGroup.xaml 的交互逻辑
    /// </summary>
    /// <summary>
    /// FivePointStarGroup.xaml 的交互逻辑
    /// </summary>
    public partial class FivePointStarGroup : UserControl
    {
        private double radius = 20;


        private double itemsCount = 5;


        private double selectCount = 5;


        private Brush selectBackground = new SolidColorBrush(Colors.YellowGreen);


        private Brush unselectBackgroud = new SolidColorBrush(Colors.DarkGray);


        /// <summary>
        /// 五角星半径
        /// </summary>
        public double Radius
        {
            get
            {
                object result = GetValue(RadiusProperty);


                if (result == null)
                {
                    return radius;
                }


                return (double)result;
            }


            set
            {
                SetValue(RadiusProperty, value);
            }
        }


        public static DependencyProperty RadiusProperty =
           DependencyProperty.Register("Radius", typeof(double),
           typeof(FivePointStarGroup), new UIPropertyMetadata());


        /// <summary>
        /// 五角星个数
        /// </summary>
        public double ItemsCount
        {
            get
            {
                object result = GetValue(ItemsCountProperty);


                if (result == null)
                {
                    return itemsCount;
                }


                return (double)result;
            }


            set
            {
                SetValue(ItemsCountProperty, value);


                InitialData();


                this.InvalidateVisual();
            }
        }


        public static DependencyProperty ItemsCountProperty =
           DependencyProperty.Register("ItemsCount", typeof(double),
           typeof(FivePointStar), new UIPropertyMetadata());


        /// <summary>
        /// 选中的五角星个数
        /// </summary>
        public double SelectCount
        {
            get
            {
                object result = GetValue(SelectCountProperty);


                if (result == null)
                {
                    return selectCount;
                }


                return (double)result;
            }


            set
            {
                SetValue(SelectCountProperty, value);


                InitialData();


                this.InvalidateVisual();
            }
        }


        public static DependencyProperty SelectCountProperty =
           DependencyProperty.Register("SelectCount", typeof(double),
           typeof(FivePointStar), new UIPropertyMetadata());


        public event RoutedEventHandler SelectCountChangeEvent
        {
            add { AddHandler(SelectCountChangePropertyEvent, value); }


            remove { RemoveHandler(SelectCountChangePropertyEvent, value); }
        }


        /// <summary>
        /// 选中颜色
        /// </summary>
        public Brush SelectBackground
        {
            get
            {
                object result = GetValue(SelectBackgroundProperty);


                if (result == null)
                {
                    return selectBackground;
                }


                return (Brush)result;
            }


            set
            {
                SetValue(SelectBackgroundProperty, value);
            }
        }


        public static DependencyProperty SelectBackgroundProperty =
           DependencyProperty.Register("SelectBackground", typeof(Brush),
           typeof(FivePointStarGroup), new UIPropertyMetadata());


        /// <summary>
        /// 未选中颜色
        /// </summary>
        public Brush UnSelectBackground
        {
            get
            {
                object result = GetValue(UnSelectBackgroundProperty);


                if (result == null)
                {
                    return unselectBackgroud;
                }


                return (Brush)result;
            }


            set
            {
                SetValue(UnSelectBackgroundProperty, value);
            }
        }


        public static DependencyProperty UnSelectBackgroundProperty =
           DependencyProperty.Register("UnSelectBackground", typeof(Brush),
           typeof(FivePointStarGroup), new UIPropertyMetadata());


        public static RoutedEvent SelectCountChangePropertyEvent =
             EventManager.RegisterRoutedEvent("SelectCountChangeEvent",
             RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Control));


        public FivePointStarGroup()
        {
            InitializeComponent();


            this.Loaded += new RoutedEventHandler(FivePointStarGroup_Loaded);
        }


        void FivePointStarGroup_Loaded(object sender, RoutedEventArgs e)
        {
            InitialData();
        }


        private void InitialData()
        {
            List<FivePointStarModel> list = new List<FivePointStarModel>();


            int count = Convert.ToInt32(this.ItemsCount);


            if (count <= 0)
            {
                count = Convert.ToInt32(this.itemsCount);
            }


            for (int i = 0; i < count; i++)
            {
                FivePointStarModel item = new FivePointStarModel();


                item.ID = i + 1;


                item.Radius = Radius;


                item.SelectBackground = SelectBackground;


                item.UnselectBackgroud = UnSelectBackground;


                item.Margins = new Thickness(Radius, 0, Radius, 0);


                //在此设置星形显示的颜色
                if ((i + 1) > SelectCount && ((i + 1 - SelectCount) > 0) &&
                    (i + 1 - SelectCount) < 1)
                {
                    item.CurrentValue = 0.5;
                }
                else if ((i + 1) > SelectCount)
                {
                    item.CurrentValue = 0;
                }
                else
                {
                    item.CurrentValue = 1;
                }


                list.Add(item);
            }


            this.lsbchildCategory.ItemsSource = list;
        }


        private void FivePointStar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            FivePointStar m = sender as FivePointStar;


            if (m == null)
            {
                return;
            }


            int index = Convert.ToInt32(m.Tag);


            this.SelectCount = index;


            RaiseEvent(new RoutedEventArgs(SelectCountChangePropertyEvent, sender));
        }
    }
}

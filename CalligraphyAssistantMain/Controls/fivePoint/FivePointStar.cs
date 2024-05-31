using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CalligraphyAssistantMain.Controls.fivePoint
{
    public class FivePointStar : UserControl
    {
        private double radius = 20;


        private double currentPart = 1;


        private Brush selectBackground = new SolidColorBrush(Colors.YellowGreen);


        private Brush unselectBackgroud = new SolidColorBrush(Colors.DarkGray);


        /// <summary>
        /// 半径
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


                this.InvalidateVisual();
            }
        }


        public static DependencyProperty RadiusProperty =
           DependencyProperty.Register("Radius", typeof(double), typeof(FivePointStar), new UIPropertyMetadata());


        /// <summary>
        /// 当前是否是一颗星
        /// </summary>
        public double CurrentPart
        {
            get
            {
                object result = GetValue(CurrentPartProperty);


                if (result == null)
                {
                    return currentPart;
                }
                return (double)result;
            }


            set
            {
                SetValue(CurrentPartProperty, value);


                this.InvalidateVisual();
            }
        }


        public static DependencyProperty CurrentPartProperty =
           DependencyProperty.Register("CurrentPart", typeof(double), typeof(FivePointStar), new UIPropertyMetadata());


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


                //this.InvalidateVisual();
            }
        }


        public static DependencyProperty SelectBackgroundProperty =
           DependencyProperty.Register("SelectBackground", typeof(Brush), typeof(FivePointStar), new UIPropertyMetadata());


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
           DependencyProperty.Register("UnSelectBackground", typeof(Brush), typeof(FivePointStar), new UIPropertyMetadata());




        public FivePointStar()
            : base()
        {
            this.Loaded += new RoutedEventHandler(FivePointStar_Loaded);
        }


        void FivePointStar_Loaded(object sender, RoutedEventArgs e)
        {
            //如果使用第一种画法就要开启此注释
            //this.MinHeight = Radius * 2;


            //this.MaxHeight = Radius * 2;


            //this.MinWidth = Radius * 2;


            //this.MaxWidth = Radius * 2;


            //this.Background = Brushes.Transparent;


            this.MinHeight = 0;


            this.MaxHeight = 0;


            this.MinWidth = 0;


            this.MaxWidth = 0;


            this.Background = Brushes.Transparent;
        }


        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            base.OnRender(dc);


            Point center = new Point();


            PointCollection Points = GetFivePoint2(center, Radius);


            Canvas ca = new Canvas();


            if (CurrentPart == 1)
            {
                Polygon plg = new Polygon();


                plg.Points = Points;


                plg.Stroke = Brushes.Transparent;


                plg.StrokeThickness = 2;


                plg.Fill = this.SelectBackground;


                plg.FillRule = FillRule.Nonzero;


                ca.Children.Add(plg);
            }
            else if (CurrentPart == 0)
            {
                Polygon plg = new Polygon();


                plg.Points = Points;


                plg.Stroke = Brushes.Transparent;


                plg.StrokeThickness = 2;


                plg.Fill = this.UnSelectBackground;


                plg.FillRule = FillRule.Nonzero;


                ca.Children.Add(plg);
            }
            else
            {
                //半边五角星的画法
                Polygon plg1 = new Polygon();


                Polygon plg2 = new Polygon();


                plg1.Points = Points;


                plg1.Stroke = Brushes.Transparent;


                plg1.StrokeThickness = 2;


                plg1.FillRule = FillRule.Nonzero;


                plg2.Points = Points;


                plg2.Stroke = Brushes.Transparent;


                plg2.StrokeThickness = 2;


                plg2.FillRule = FillRule.Nonzero;


                //左半边：3,4,5,6,7,8
                //右半边：1,2,3,8,9,10
                plg1.Points = new PointCollection()
                {
                    Points[2],
                    Points[3],
                    Points[4],
                    Points[5],
                    Points[6],
                    Points[7],
                };


                plg1.Fill = SelectBackground;


                plg2.Points = new PointCollection()
                {
                    Points[0],
                    Points[1],
                    Points[2],
                    Points[7],
                    Points[8],
                    Points[9],
                };


                plg2.Fill = UnSelectBackground;


                ca.Children.Add(plg1);


                ca.Children.Add(plg2);
            }


            ca.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;


            ca.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;


            this.Content = ca;


            //Brush b = new SolidColorBrush(Colors.Yellow);


            //Pen p = new Pen(b, 2);


            //var path = new Path();


            //var gc = new GeometryConverter();


            //path.Data = (Geometry)gc.ConvertFromString(string.Format("M {0} {1} {2} {3} {4} Z",
            //    Points[0], Points[1], Points[2], Points[3], Points[4]));


            //path.Fill = Brushes.Yellow;


            //dc.DrawGeometry(b, p, path.Data);
        }


        /// <summary>
        ///第一种画法 根据半径和圆心确定五个点
        /// </summary>
        /// <param name="center"></param>
        /// <returns></returns>
        private PointCollection GetFivePoint1(Point center, double r)
        {
            double h1 = r * Math.Sin(18 * Math.PI / 180);


            double h2 = r * Math.Cos(18 * Math.PI / 180);


            double h3 = r * Math.Sin(36 * Math.PI / 180);


            double h4 = r * Math.Cos(36 * Math.PI / 180);


            Point p1 = new Point(r, center.X);


            Point p2 = new Point(r - h2, r - h1);


            Point p3 = new Point(r - h3, r + h4);


            Point p4 = new Point(r + h3, p3.Y);


            Point p5 = new Point(r + h2, p2.Y);


            List<Point> values = new List<Point>() { p1, p3, p5, p2, p4 };


            PointCollection pcollect = new PointCollection(values);


            return pcollect;
        }


        /// <summary>
        ///第二种画法 根据半径和圆心确定十个点
        /// </summary>
        /// <param name="center"></param>
        /// <returns></returns>
        private PointCollection GetFivePoint2(Point center, double r)
        {
            int i;


            //两个圆的半径 和第一个点初始角度
            //r1 = r / 2.5, r2 = r值的互换确定是正五角星还是倒五角星
            double r1 = r / 2.5, r2 = r, g = 18;


            double pi = Math.PI;


            List<Point> values = new List<Point>(10);//十个点


            List<Point> values1 = new List<Point>(5);//(内)外接五个点


            List<Point> values2 = new List<Point>(5);//(外)内接五个点


            for (i = 0; i < 5; i++)
            {
                //计算10个点的坐标
                Point p1 = new Point(r1 * Math.Cos(g * pi / 180), r1 * Math.Sin(g * pi / 180));


                Point p2 = new Point(r2 * Math.Cos((g + 36) * pi / 180), r2 * Math.Sin((g + 36) * pi / 180));


                values1.Add(p1);


                values2.Add(p2);


                g += 72;
            }
            //左半边：3,4,5,6,7,8
            //右半边：1,2,3,8,9,10
            values.Add(values1[0]);//1
            values.Add(values2[0]);//2
            values.Add(values1[1]);//3
            values.Add(values2[1]);//4
            values.Add(values1[2]);//5
            values.Add(values2[2]);//6
            values.Add(values1[3]);//7
            values.Add(values2[3]);//8
            values.Add(values1[4]);//9
            values.Add(values2[4]);//10

            PointCollection pcollect = new PointCollection(values);


            return pcollect;
        }
    }
}

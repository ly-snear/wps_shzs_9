using CalligraphyAssistantMain.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CalligraphyAssistantMain.Controls
{
    public class DrawBoardCanvas : Canvas
    {
        public class GeometryObject
        {
            public Geometry Geometry { get; set; }
            public Pen Pen { get; set; }
            public string Text { get; set; }
        }
        private List<GeometryObject> drawPathList = new List<GeometryObject>();
        private DrawMode drawMode = DrawMode.None;
        private PathGeometry pathGeometry = null;
        private PathFigure pathFigure = null;
        private PathSegmentCollection segmentCollection = null;
        private SolidColorBrush penColor = Brushes.Red;
        private Pen pen = new Pen(Brushes.Red, 5);
        private DashStyle dashStyle = DashStyles.Solid;
        private double penWidth = 5;
        //private Pen backBorderPen = null;
        private readonly Pen eraserPen = new Pen(Brushes.Black, 10);
        private Point mouseDownPoint = new Point();
        private Point lastEraserLocation = new Point(-32000, -32000);
        private Point[] startPoints = null;
        private GeometryObject currentGeometry = null;
        private TextBox textBox = null;
        private Border textBoxBd = null;
        //private bool showBoderLine = true;
        private bool mouseDown = false;
        private bool isChanged = false;
        public DrawMode DrawMode
        {
            get { return drawMode; }
            set
            {
                if (drawMode != value &&
                    currentGeometry != null)
                {
                    currentGeometry = null;
                }
                if (textBoxBd != null)
                {
                    if (drawMode == DrawMode.Text &&
                       drawMode != value &&
                       textBoxBd.Visibility == System.Windows.Visibility.Visible)
                    {
                        SetTextGeometry();
                    }
                }
                drawMode = value;
            }
        }
        public bool IsChanged
        {
            get
            {
                if (drawPathList == null || drawPathList.Count == 0)
                {
                    return false;
                }
                return isChanged;
            }
            set
            {
                isChanged = value;
            }
        }
        public DrawBoardCanvas()
        {

        }

        public void SetPenColor(Brush brush)
        {
            this.penColor = brush as SolidColorBrush;
            pen = new Pen(brush, penWidth);
            pen.DashStyle = dashStyle;
            if (drawMode == DrawMode.Text && textBox != null)
            {
                textBox.Foreground = pen.Brush;
            }
            if (currentGeometry != null)
            {
                currentGeometry = null;
            }
        }

        public void SetDashStyle(DashStyle dashStyle)
        {
            this.dashStyle = dashStyle;
            pen = new Pen(penColor, penWidth);
            pen.DashStyle = dashStyle;
        }

        public void SetPenWidth(double width)
        {
            this.penWidth = width;
            pen = new Pen(penColor, penWidth);
            pen.DashStyle = dashStyle;
        }

        public void ClearDrawBoard()
        {
            currentGeometry = null;
            drawPathList.Clear();
            this.InvalidateVisual();
        }

        public bool CheckPathOverflow()
        {
            if (Background == null)
            {
                return false;
            }
            if (Background != null && Background is ImageBrush)
            {
                double width = (Background as ImageBrush).ImageSource.Width;
                double height = (Background as ImageBrush).ImageSource.Height;
                double right = 0;
                double bottom = 0;
                foreach (GeometryObject item in drawPathList)
                {
                    right = Math.Max(right, item.Geometry.Bounds.Right);
                    bottom = Math.Max(bottom, item.Geometry.Bounds.Bottom);
                }
                if (right > width || bottom > height)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public bool CanSave()
        {
            if (drawPathList == null ||
                drawPathList.Count == 0)
            {
                return false;
            }
            return true;
        }

        public ImageSource GetImage(bool justBackgoundArea = true)
        {
            //showBoderLine = false;
            this.InvalidateVisual();
            System.Windows.Forms.Application.DoEvents();
            RenderTargetBitmap bitmap = null;
            double right = 0;
            double bottom = 0;
            double left = int.MaxValue;
            double top = int.MaxValue;
            if (Background != null && Background is ImageBrush)
            {
                double width = (Background as ImageBrush).ImageSource.Width;
                double height = (Background as ImageBrush).ImageSource.Height;
                if (justBackgoundArea)
                {
                    bitmap = new RenderTargetBitmap((int)width, (int)height, 96d, 96d, PixelFormats.Pbgra32);
                    bitmap.Render(this);
                    //showBoderLine = true;
                    return bitmap;
                }
                left = 0;
                top = 0;
                right = width;
                bottom = height;
            }
            foreach (GeometryObject item in drawPathList)
            {
                left = Math.Min(left, item.Geometry.Bounds.Left);
                top = Math.Min(top, item.Geometry.Bounds.Top);
                right = Math.Max(right, item.Geometry.Bounds.Right);
                bottom = Math.Max(bottom, item.Geometry.Bounds.Bottom);
            }
            if (right + 10 < this.ActualWidth)
            {
                right += 10;
            }
            if (bottom + 10 < this.ActualHeight)
            {
                bottom += 10;
            }
            //bitmap = new RenderTargetBitmap((int)right, (int)bottom, 96d, 96d, PixelFormats.Pbgra32);
            bitmap = new RenderTargetBitmap((int)this.ActualWidth, (int)this.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
            bitmap.Render(this);
            if (left > 0 || top > 0)
            {
                if (left - 10 > 0)
                {
                    left -= 10;
                }
                if (top - 10 > 0)
                {
                    top -= 10;
                }
                Int32Rect cutRect = new Int32Rect((int)left, (int)top, (int)(right - left), (int)(bottom - top));
                int stride = bitmap.Format.BitsPerPixel * cutRect.Width / 8;
                byte[] data = new byte[cutRect.Height * stride];
                bitmap.CopyPixels(cutRect, data, stride, 0);
                BitmapSource cutImage = BitmapSource.Create(cutRect.Width, cutRect.Height, 0, 0, PixelFormats.Pbgra32, null, data, stride);
                //showBoderLine = true;
                return cutImage;
            }
            //showBoderLine = true;
            return bitmap;
        }

        public void ResetMouseDown()
        {
            mouseDown = false;
        }

        private void InitPathGeometry(MouseButtonEventArgs e)
        {
            pathGeometry = new PathGeometry();
            pathFigure = new PathFigure();
            segmentCollection = new PathSegmentCollection();
            pathGeometry.Figures.Add(pathFigure);
            pathFigure.Segments = segmentCollection;
            pathFigure.StartPoint = e.GetPosition(this);
            drawPathList.Add(new GeometryObject() { Geometry = pathGeometry, Pen = pen });
            IsChanged = true;
            this.InvalidateVisual();
        }

        private void InitLineGeometry(MouseButtonEventArgs e)
        {
            startPoints = new Point[] { e.GetPosition(this) };
            LineGeometry line = new LineGeometry();
            line.StartPoint = startPoints[0];
            currentGeometry = new GeometryObject() { Geometry = line, Pen = pen };
            drawPathList.Add(currentGeometry);
        }

        private void InitRectangleGeometry(MouseButtonEventArgs e)
        {
            startPoints = new Point[] { e.GetPosition(this) };
            RectangleGeometry rect = new RectangleGeometry();
            rect.Rect = new Rect(startPoints[0], new Point());
            currentGeometry = new GeometryObject() { Geometry = rect, Pen = pen };
            drawPathList.Add(currentGeometry);
        }

        private void InitTextGeometry(MouseButtonEventArgs e)
        {
            if (textBoxBd != null && textBoxBd.Visibility == System.Windows.Visibility.Visible)
            {
                SetTextGeometry();
                return;
            }
            if (textBoxBd == null)
            {
                textBoxBd = new Border();
                textBox = new TextBox
                {
                    Margin = new Thickness(0),
                    Padding = new Thickness(0),
                    Background = Brushes.Transparent,
                    BorderBrush = Brushes.Transparent,
                    FontSize = 20,
                    AcceptsReturn = true,
                    BorderThickness = new Thickness(0),
                    TextWrapping = TextWrapping.Wrap
                };
                textBox.TextChanged += TextBox_TextChanged;
                textBoxBd.Child = textBox;
                this.Children.Add(textBoxBd);
                textBoxBd.BorderThickness = new Thickness(1);
                textBoxBd.BorderBrush = Brushes.Black;
            }
            textBox.Text = string.Empty;
            textBox.Foreground = pen.Brush;
            textBox.Width = 100;
            textBox.Height = 56;
            Canvas.SetLeft(textBoxBd, e.GetPosition(this).X);
            Canvas.SetTop(textBoxBd, e.GetPosition(this).Y);
            textBoxBd.Visibility = System.Windows.Visibility.Visible;
            textBox.Focus();
            textBox.SelectAll();
        }

        private void SetTextGeometry()
        {
            textBoxBd.Visibility = System.Windows.Visibility.Collapsed;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                return;
            }
            RectangleGeometry rect = new RectangleGeometry
            {
                Rect = new Rect(Canvas.GetLeft(textBoxBd) + 3, Canvas.GetTop(textBoxBd) + 1, textBox.Width, textBox.Height)
            };
            currentGeometry = new GeometryObject() { Geometry = rect, Pen = pen, Text = textBox.Text };
            drawPathList.Add(currentGeometry);
            this.InvalidateVisual();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            FormattedText format = new FormattedText(textBox.Text, System.Globalization.CultureInfo.CurrentCulture, textBox.FlowDirection, new Typeface(textBox.FontFamily, textBox.FontStyle, textBox.FontWeight, textBox.FontStretch), textBox.FontSize, textBox.Foreground);
            format.MaxTextWidth = 500;
            format.MaxTextHeight = 400;
            textBox.Width = format.Width + 30;
            textBox.Height = format.Height + 30;
            if (textBox.Width >= 500)
            {
                textBox.Width = 500;
            }
            if (textBox.Height >= 400)
            {
                textBox.Height = 400;
            }
            if (textBox.Width <= 100)
            {
                textBox.Width = 100;
            }
            if (textBox.Height <= 56)
            {
                textBox.Height = 56;
            }
        }

        private void InitEllipseGeometry(MouseButtonEventArgs e)
        {
            startPoints = new Point[] { e.GetPosition(this) };
            EllipseGeometry ellipse = new EllipseGeometry();
            currentGeometry = new GeometryObject() { Geometry = ellipse, Pen = pen };
            drawPathList.Add(currentGeometry);
        }

        private void InitPolyLineGeometry(MouseButtonEventArgs e, bool isClosed)
        {
            pathGeometry = new PathGeometry();
            pathFigure = new PathFigure();
            segmentCollection = new PathSegmentCollection();
            segmentCollection.Add(new LineSegment() { Point = e.GetPosition(this), IsSmoothJoin = true });
            pathGeometry.Figures.Add(pathFigure);
            pathFigure.Segments = segmentCollection;
            pathFigure.StartPoint = e.GetPosition(this);
            pathFigure.IsClosed = isClosed;
            currentGeometry = new GeometryObject() { Geometry = pathGeometry, Pen = pen };
            drawPathList.Add(currentGeometry);
        }

        private void SetLineEndPoint(MouseEventArgs e)
        {
            if (mouseDown && currentGeometry != null && currentGeometry.Geometry is LineGeometry)
            {
                if (e.LeftButton != MouseButtonState.Pressed)
                {
                    currentGeometry = null;
                    mouseDown = false;
                    return;
                }
                Point point = e.GetPosition(this);
                (currentGeometry.Geometry as LineGeometry).EndPoint = point;
                this.InvalidateVisual();
            }
        }

        private void SetRectangleBounds(MouseEventArgs e)
        {
            if (mouseDown && currentGeometry != null && currentGeometry.Geometry is RectangleGeometry)
            {
                if (e.LeftButton != MouseButtonState.Pressed)
                {
                    currentGeometry = null;
                    mouseDown = false;
                    return;
                }
                Point point = e.GetPosition(this);
                (currentGeometry.Geometry as RectangleGeometry).Rect = new Rect(
                    Math.Min(startPoints[0].X, point.X),
                    Math.Min(startPoints[0].Y, point.Y),
                    Math.Abs(startPoints[0].X - point.X),
                    Math.Abs(startPoints[0].Y - point.Y));
                this.InvalidateVisual();
            }
        }

        private void SetEllipseBounds(MouseEventArgs e)
        {
            if (mouseDown && currentGeometry != null && currentGeometry.Geometry is EllipseGeometry)
            {
                if (e.LeftButton != MouseButtonState.Pressed)
                {
                    currentGeometry = null;
                    mouseDown = false;
                    return;
                }
                Point point = e.GetPosition(this);
                Rect rect = new Rect(
                    Math.Min(startPoints[0].X, point.X),
                    Math.Min(startPoints[0].Y, point.Y),
                    Math.Abs(startPoints[0].X - point.X),
                    Math.Abs(startPoints[0].Y - point.Y));
                (currentGeometry.Geometry as EllipseGeometry).Center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
                (currentGeometry.Geometry as EllipseGeometry).RadiusX = rect.Width / 2;
                (currentGeometry.Geometry as EllipseGeometry).RadiusY = rect.Height / 2;
                this.InvalidateVisual();
            }
        }

        private void SetPolyLineEndPoint(MouseEventArgs e)
        {
            if (mouseDown && currentGeometry != null && currentGeometry.Geometry is PathGeometry)
            {
                if (segmentCollection != null && segmentCollection.Count > 0)
                {
                    Point point = e.GetPosition(this);
                    (segmentCollection[segmentCollection.Count - 1] as LineSegment).Point = point;
                    this.InvalidateVisual();
                }
            }
        }

        private void AddPathPoint(MouseEventArgs e)
        {
            if (mouseDown)
            {
                if (e.LeftButton != MouseButtonState.Pressed)
                {
                    mouseDown = false;
                    return;
                }
                if (segmentCollection == null)
                {
                    return;
                }
                if (segmentCollection.Count > 2)
                {
                    Point newPoint = e.GetPosition(this);

                    if (newPoint.X == (segmentCollection[segmentCollection.Count - 1] as LineSegment).Point.X &&
                        newPoint.X == (segmentCollection[segmentCollection.Count - 2] as LineSegment).Point.X)
                    {
                        (segmentCollection[segmentCollection.Count - 1] as LineSegment).Point = newPoint;
                    }
                    else if (newPoint.Y == (segmentCollection[segmentCollection.Count - 1] as LineSegment).Point.Y &&
                         newPoint.Y == (segmentCollection[segmentCollection.Count - 2] as LineSegment).Point.Y)
                    {
                        (segmentCollection[segmentCollection.Count - 1] as LineSegment).Point = newPoint;
                    }
                    else
                    {
                        double angle1 = Common.GetAngle((segmentCollection[segmentCollection.Count - 2] as LineSegment).Point, new Point(0, 0), (segmentCollection[segmentCollection.Count - 1] as LineSegment).Point);
                        double angle2 = Common.GetAngle((segmentCollection[segmentCollection.Count - 2] as LineSegment).Point, new Point(0, 0), newPoint);
                        if (Math.Round(angle1, 1) == Math.Round(angle2, 1))
                        {
                            (segmentCollection[segmentCollection.Count - 1] as LineSegment).Point = newPoint;
                        }
                        else
                        {
                            segmentCollection.Add(new LineSegment() { Point = newPoint, IsSmoothJoin = true });
                        }
                    }
                }
                else
                {
                    segmentCollection.Add(new LineSegment() { Point = e.GetPosition(this), IsSmoothJoin = true });
                }
            }
        }

        private void RemovePath(MouseEventArgs e)
        {
            if (mouseDown)
            {
                Point[] points;
                Point current = e.GetPosition(this);
                if (lastEraserLocation.X != -32000 && lastEraserLocation != current)
                {
                    points = Common.GetPointsFromLine(lastEraserLocation, current);
                }
                else
                {
                    points = new Point[1] { current };
                }
                lastEraserLocation = current;
                if (e.LeftButton != MouseButtonState.Pressed)
                {
                    mouseDown = false;
                    return;
                }
                List<GeometryObject> tempList = new List<GeometryObject>();
                foreach (GeometryObject item in drawPathList)
                {
                    foreach (Point point in points)
                    {
                        if (item.Geometry.StrokeContains(eraserPen, point))
                        {
                            if (!tempList.Contains(item))
                            {
                                tempList.Add(item);
                                break;
                            }
                        }
                    }
                }
                if (tempList.Count > 0)
                {
                    foreach (GeometryObject item in tempList)
                    {
                        drawPathList.Remove(item);
                    }
                    this.InvalidateVisual();
                }
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            mouseDownPoint = e.GetPosition(this);
            lastEraserLocation = mouseDownPoint;
            if (e.ChangedButton == MouseButton.Left)
            {
                mouseDown = true;
                switch (drawMode)
                {
                    case DrawMode.Pen:
                        InitPathGeometry(e);
                        break;
                    case DrawMode.Text:
                        InitTextGeometry(e);
                        break;
                    case DrawMode.Line:
                        InitLineGeometry(e);
                        break;
                    case DrawMode.Rectangle:
                        InitRectangleGeometry(e);
                        break;
                    case DrawMode.Ellipse:
                        InitEllipseGeometry(e);
                        break;
                    case DrawMode.ClosedPolyLine:
                    case DrawMode.NonClosedPolyLine:
                        if (currentGeometry == null)
                        {
                            InitPolyLineGeometry(e, DrawMode == DrawMode.ClosedPolyLine);
                            this.InvalidateVisual();
                        }
                        else
                        {
                            AddPathPoint(e);
                        }
                        break;
                    default:
                        break;
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (mouseDown)
            {
                switch (drawMode)
                {
                    case DrawMode.Pen:
                        AddPathPoint(e);
                        break;
                    case DrawMode.Line:
                        SetLineEndPoint(e);
                        break;
                    case DrawMode.Rectangle:
                        SetRectangleBounds(e);
                        break;
                    case DrawMode.Ellipse:
                        SetEllipseBounds(e);
                        break;
                    case DrawMode.ClosedPolyLine:
                    case DrawMode.NonClosedPolyLine:
                        if (currentGeometry != null)
                        {
                            SetPolyLineEndPoint(e);
                        }
                        break;
                    case DrawMode.Text:
                        break;
                    case DrawMode.Eraser:
                        RemovePath(e);
                        break;
                    case DrawMode.None:
                        break;
                    default:
                        break;
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            if (DrawMode == DrawMode.Line || DrawMode == DrawMode.Rectangle || DrawMode == DrawMode.Ellipse)
            {
                if (startPoints != null &&
                    startPoints.Length > 0 &&
                    currentGeometry != null)
                {
                    Point point = e.GetPosition(this);
                    if (Math.Abs(point.X - startPoints[0].X) < 3 &&
                        Math.Abs(point.Y - startPoints[0].Y) < 3)
                    {
                        if (drawPathList.Contains(currentGeometry))
                        {
                            drawPathList.Remove(currentGeometry);
                            currentGeometry = null;
                            mouseDown = false;
                            base.OnMouseUp(e);
                            return;
                        }
                    }
                }
            }
            if (e.ChangedButton == MouseButton.Right)
            {
                if (DrawMode == DrawMode.ClosedPolyLine || DrawMode == DrawMode.NonClosedPolyLine)
                {
                    currentGeometry = null;
                    mouseDown = false;
                    base.OnMouseUp(e);
                    return;
                }
            }
            //currentGeometry = null;
            mouseDown = false;
            base.OnMouseUp(e);
        }

        protected override void OnRender(DrawingContext dc)
        {
            dc.DrawRectangle(Brushes.White, null, new Rect(0, 0, RenderSize.Width, RenderSize.Height));
            //if (showBoderLine && Background != null && Background is ImageBrush)
            //{
            //    double width = (Background as ImageBrush).ImageSource.Width + 1;
            //    double height = (Background as ImageBrush).ImageSource.Height + 1;
            //    if (backBorderPen == null)
            //    {

            //        backBorderPen = new Pen(Brushes.Red, 1);
            //        backBorderPen.DashStyle = DashStyles.Dash;
            //    }
            //    dc.DrawLine(backBorderPen, new Point(0, height), new Point(width, height));
            //    dc.DrawLine(backBorderPen, new Point(width, 0), new Point(width, height));
            //}
            base.OnRender(dc);
            foreach (GeometryObject item in drawPathList)
            {
                if (item.Geometry is PathGeometry)
                {
                    PathGeometry path = item.Geometry as PathGeometry;
                    if (path.Figures.Count == 1)
                    {
                        PathFigure figure = path.Figures[0];
                        if (figure.Segments.Count == 1 &&
                            figure.Segments[0] is LineSegment &&
                            (figure.Segments[0] as LineSegment).Point == mouseDownPoint)
                        {
                            dc.DrawEllipse(item.Pen.Brush, null, (figure.Segments[0] as LineSegment).Point, item.Pen.Thickness / 2, item.Pen.Thickness / 2);
                            continue;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(item.Text))
                {
                    RectangleGeometry rectangle = item.Geometry as RectangleGeometry;
                    FormattedText format = new FormattedText(item.Text, System.Globalization.CultureInfo.CurrentCulture, textBox.FlowDirection, new Typeface(textBox.FontFamily, textBox.FontStyle, textBox.FontWeight, textBox.FontStretch), textBox.FontSize, item.Pen.Brush)
                    {
                        MaxTextWidth = rectangle.Bounds.Width,
                        MaxTextHeight = rectangle.Bounds.Height
                    };
                    dc.DrawText(format, rectangle.Bounds.Location);
                }
                else
                {
                    dc.DrawGeometry(null, item.Pen, item.Geometry);
                }
            }
        }

        public void StopEdit()
        {
            if (drawMode == DrawMode.Text && textBoxBd != null &&
                textBoxBd.Visibility == System.Windows.Visibility.Visible)
            {
                SetTextGeometry();
            }
        }
    }
}

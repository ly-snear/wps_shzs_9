using CalligraphyAssistantMain.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// WriteScreenControl.xaml 的交互逻辑
    /// </summary>
    public partial class WriteScreenControl : UserControl
    {
        private ImageSource image = null;
        private bool isCrop = false;
        private bool isCropMove = false;
        private bool isDragHandle = false;
        private bool isCroped = false;
        private Point cropStartPoint;
        private Point cropMoveStartPoint;
        private Point handleStartPoint;
        private Rectangle dragHandleRect = null;
        public event EventHandler Back = null;
        public ImageSource Image { get { return image; } }
        public bool IsChanged { get { return isCroped || drawBoardCanvas.IsChanged; } }
        public int SelectedStudent { get; private set; }
        public bool IsWriteScreen { get; private set; }
        public WriteScreenControl()
        {
            InitializeComponent();
            DrawPen();
        }

        public void SetBackground(Brush brush)
        {
            isCroped = false;
            drawBoardCanvas.Background = brush;
        }

        public void SetMode(bool isWriteScreen)
        {
            SelectedStudent = 0;
            IsWriteScreen = isWriteScreen;
            if (isWriteScreen)
            {
                studentGd.Visibility = Visibility.Collapsed;
                cropBtn.Visibility = Visibility.Visible;
                topBarBd.Width = 467;
            }
            else
            {
                studentGd.Visibility = Visibility.Visible;
                cropBtn.Visibility = Visibility.Collapsed;
                topBarBd.Width = 500;
            }
        }

        public void ClearDrawBoard()
        {
            drawBoardCanvas.StopEdit();
            drawBoardCanvas.ClearDrawBoard();
        }

        public ImageSource GetImage()
        {
            return drawBoardCanvas.GetImage(false);
        }

        private void DrawPen()
        {
            drawBoardCanvas.DrawMode = DrawMode.Pen;
        }

        private void ResetColorRectWidth(Rectangle without)
        {
            foreach (var item in colorWp.Children)
            {
                if (item is Rectangle && item != without)
                {
                    (item as Rectangle).Width = 20;
                }
            }
        }

        private void CommonMouseUp()
        {
            if (isCrop)
            {
                isCrop = false;
                cropBar.Visibility = Visibility.Visible;
                cropRect.Background = Brushes.Transparent;
                cropRect.Cursor = Cursors.SizeAll;
                drawBoardCanvas.ResetMouseDown();
            }
            if (isCropMove)
            {
                isCropMove = false;
            }
            if (isDragHandle)
            {
                isDragHandle = false;
                cropRect.Background = Brushes.Transparent;
                cropRect.Cursor = Cursors.SizeAll;
            }
        }

        private void ResetCrop()
        {
            cropRect.Visibility = Visibility.Collapsed;
            cropRect.Width = 0;
            cropRect.Height = 0;
            drawBoardCanvas.Cursor = Cursors.Arrow;
            drawBoardCanvas.DrawMode = DrawMode.Pen;
            topBarBd.Visibility = Visibility.Visible;
        }

        private void CropImage(Int32Rect cutRect)
        {
            RenderTargetBitmap bitmap = new RenderTargetBitmap((int)this.ActualWidth, (int)this.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
            bitmap.Render(drawBoardCanvas);
            int stride = bitmap.Format.BitsPerPixel * cutRect.Width / 8;
            byte[] data = new byte[cutRect.Height * stride];
            bitmap.CopyPixels(cutRect, data, stride, 0);
            BitmapSource cutImage = BitmapSource.Create(cutRect.Width, cutRect.Height, 0, 0, PixelFormats.Pbgra32, null, data, stride);
            drawBoardCanvas.Background = new ImageBrush(cutImage) { Stretch = Stretch.Uniform };
        }

        private void ColorWp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            drawBoardCanvas.SetPenColor((sender as Rectangle).Fill);
            (sender as Rectangle).Width = 30;
            ResetColorRectWidth(sender as Rectangle);
        }

        private void lineStyleBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            lineStyleCb.IsDropDownOpen = true;
        }

        private void lineSizeBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            lineSizeCb.IsDropDownOpen = true;
        }

        private void lineSizeCb_SelectionChanged(object sender, RoutedEventArgs e)
        {
            double width = Convert.ToDouble((lineSizeCb.SelectedItem as TextBlock).Text);
            lineSizeBtn.Text = (lineSizeCb.SelectedItem as TextBlock).Text;
            drawBoardCanvas.SetPenWidth(width);
        }

        private void lineStyleCb_SelectionChanged(object sender, RoutedEventArgs e)
        {
            lineStyleBtn.Background = (lineStyleCb.SelectedItem as Control).Background;
            DashStyle dashStyle = DashStyles.Solid;
            switch ((lineStyleCb.SelectedItem as Control).ToolTip.ToString())
            {
                case "实线":
                    dashStyle = DashStyles.Solid;
                    break;
                case "虚线":
                    dashStyle = DashStyles.Dash;
                    break;
                case "虚线点":
                    dashStyle = DashStyles.DashDot;
                    break;
                case "虚线点点":
                    dashStyle = DashStyles.DashDotDot;
                    break;
                case "点":
                    dashStyle = DashStyles.Dot;
                    break;
                default:
                    return;
            }
            drawBoardCanvas.SetDashStyle(dashStyle);
        }

        private void clearBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            drawBoardCanvas.ClearDrawBoard();
        }

        private void closeBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.Back != null)
            {
                this.Back(this, null);
            }
        }

        private void cropBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cropRect.Visibility = Visibility.Visible;
            cropRect.Width = 0;
            cropRect.Height = 0;
            drawBoardCanvas.Cursor = Cursors.Cross;
            drawBoardCanvas.DrawMode = DrawMode.None;
            topBarBd.Visibility = Visibility.Collapsed;
            drawBoardCanvas.ReleaseMouseCapture();
        }

        private void drawBoardCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (cropRect.Visibility == Visibility.Visible)
            {
                isCrop = true;
                cropStartPoint = e.GetPosition(drawBoardCanvas);
                cropRect.Margin = new Thickness(cropStartPoint.X, cropStartPoint.Y, 0, 0);
                cropRect.Background = null;
                cropRect.Width = 0;
                cropRect.Height = 0;
                cropRect.Cursor = Cursors.Cross;
                cropBar.Visibility = Visibility.Collapsed;
            }
        }

        private void drawBoardCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isCrop)
            {
                Point endPoint = e.GetPosition(drawBoardCanvas);
                if (endPoint.X - cropStartPoint.X < 0 || endPoint.Y - cropStartPoint.Y < 0)
                {
                    return;
                }
                cropRect.Width = endPoint.X - cropStartPoint.X;
                cropRect.Height = endPoint.Y - cropStartPoint.Y;
            }
            if (isDragHandle)
            {
                Point endPoint = e.GetPosition(drawBoardCanvas);
                if (dragHandleRect == leftHandleRect)
                {
                    double x = endPoint.X - handleStartPoint.X;
                    if (cropRect.Margin.Left + x > 0 && cropRect.Width - x > 20)
                    {
                        cropRect.Margin = new Thickness(cropRect.Margin.Left + x, cropRect.Margin.Top, 0, 0);
                        cropRect.Width -= x;
                    }
                }
                else if (dragHandleRect == topHandleRect)
                {
                    double y = endPoint.Y - handleStartPoint.Y;
                    if (cropRect.Margin.Top + y > 0 && cropRect.Height - y > 20)
                    {
                        cropRect.Margin = new Thickness(cropRect.Margin.Left, cropRect.Margin.Top + y, 0, 0);
                        cropRect.Height -= y;
                    }
                }
                else if (dragHandleRect == rightHandleRect)
                {
                    double x = endPoint.X - handleStartPoint.X;
                    if (cropRect.Width + x > 20 && cropRect.Margin.Left + cropRect.Width + x < this.ActualWidth)
                    {
                        cropRect.Width += x;
                    }
                }
                else if (dragHandleRect == bottomHandleRect)
                {
                    double y = endPoint.Y - handleStartPoint.Y;
                    if (cropRect.Height + y > 20 && cropRect.Margin.Top + cropRect.Height + y < this.ActualHeight)
                    {
                        cropRect.Height += y;
                    }
                }
                handleStartPoint = endPoint;
            }
        }

        private void drawBoardCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CommonMouseUp();
        }

        private void drawBoardCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (cropRect.Visibility == Visibility.Visible)
            {
                ResetCrop();
                e.Handled = true;
            }
        }

        private void cropRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isCropMove = true;
            cropMoveStartPoint = e.GetPosition(drawBoardCanvas);
        }

        private void cropRect_MouseMove(object sender, MouseEventArgs e)
        {
            if (isCropMove)
            {
                Point endPoint = e.GetPosition(drawBoardCanvas);
                double x = endPoint.X - cropMoveStartPoint.X;
                double y = endPoint.Y - cropMoveStartPoint.Y;
                double newX = cropRect.Margin.Left + x;
                double newY = cropRect.Margin.Top + y;
                if (newX < 0)
                {
                    newX = 0;
                }
                if (newY < 0)
                {
                    newY = 0;
                }
                if (newX + cropRect.Width > this.ActualWidth)
                {
                    newX = this.ActualWidth - cropRect.Width;
                }
                if (newY + cropRect.Height > this.ActualHeight)
                {
                    newY = this.ActualHeight - cropRect.Height;
                }
                cropRect.Margin = new Thickness(newX, newY, 0, 0);
                cropMoveStartPoint = endPoint;
            }
        }

        private void cropRect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CommonMouseUp();
        }

        private void handleRect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            dragHandleRect = sender as Rectangle;
            handleStartPoint = e.GetPosition(drawBoardCanvas);
            cropRect.Cursor = dragHandleRect.Cursor;
            cropRect.Background = null;
            isCropMove = false;
            isDragHandle = true;
            e.Handled = true;
        }

        private void handleRect_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void handleRect_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CommonMouseUp();
        }

        private void okBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CropImage(new Int32Rect((int)cropRect.Margin.Left, (int)cropRect.Margin.Top, (int)cropRect.Width, (int)cropRect.Height));
            ResetCrop();
            isCroped = true;
            e.Handled = true;
        }

        private void cancelBtn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ResetCrop();
            e.Handled = true;
        }

        private void student1Btn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectedStudent = 1;
            CropImage(new Int32Rect((int)(Common.Student1Rect.Left * this.ActualWidth), (int)(Common.Student1Rect.Top * this.ActualHeight), (int)(Common.Student1Rect.Width * this.ActualWidth), (int)(Common.Student1Rect.Height * this.ActualHeight)));
            studentGd.Visibility = Visibility.Collapsed; 
            topBarBd.Width = 437;
            isCroped = true;
            drawBoardCanvas.ClearDrawBoard();
        }

        private void student2Btn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SelectedStudent = 2;
            CropImage(new Int32Rect((int)(Common.Student2Rect.Left * this.ActualWidth), (int)(Common.Student2Rect.Top * this.ActualHeight), (int)(Common.Student2Rect.Width * this.ActualWidth), (int)(Common.Student2Rect.Height * this.ActualHeight)));
            studentGd.Visibility = Visibility.Collapsed;
            topBarBd.Width = 437;
            isCroped = true;
            drawBoardCanvas.ClearDrawBoard();
        }
    }
}

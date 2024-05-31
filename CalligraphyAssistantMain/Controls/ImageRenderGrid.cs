using CalligraphyAssistantMain.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CalligraphyAssistantMain.Controls
{
    public class ImageRenderGrid : Grid
    {
        public bool IsUniformScale { get; set; } = true;
        private WriteableBitmap renderBitmap = null;
        private object lockObj = new object();
        public void UpdateImage(int width, int height, byte[] data)
        {
            this.Dispatcher.Invoke(() =>
            {
                if (renderBitmap == null)
                {
                    renderBitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr24, null);
                }
                renderBitmap.Lock();
                renderBitmap.WritePixels(new Int32Rect(0, 0, width, height), data, width * 3, 0);
                renderBitmap.Unlock();
                this.InvalidateVisual();
            });
        }

        public BitmapSource GetImage()
        {
            lock (lockObj)
            {
                try
                {
                    BitmapSource bitmap = BitmapSource.Create(renderBitmap.PixelWidth, renderBitmap.PixelHeight, 96, 96, PixelFormats.Bgr24, null, renderBitmap.BackBuffer, renderBitmap.BackBufferStride * renderBitmap.PixelHeight, renderBitmap.BackBufferStride);
                    return bitmap;
                }
                catch (Exception ex)
                { 
                    Common.Trace("ImageRenderGrid GetImage Error:" + ex.Message);
                } 
            }
            return null;
        }

        protected override void OnRender(DrawingContext dc)
        {
            if (renderBitmap != null)
            {
                if (IsUniformScale)
                {
                    double width;
                    double height;
                    if (renderBitmap.Width / renderBitmap.Height >= this.ActualWidth / this.ActualHeight)
                    {
                        width = this.ActualWidth;
                        height = renderBitmap.Height * (this.ActualWidth / renderBitmap.Width);
                    }
                    else
                    {
                        height = this.ActualHeight;
                        width = renderBitmap.Width * (this.ActualHeight / renderBitmap.Height);
                    }
                    double x = Math.Abs(this.ActualWidth - width) / 2;
                    double y = Math.Abs(this.ActualHeight - height) / 2;
                    dc.DrawImage(renderBitmap, new Rect(x, y, width, height));
                }
                else
                {
                    dc.DrawImage(renderBitmap, new Rect(0, 0, this.ActualWidth, this.ActualHeight));
                }
            }
            else
            {
                dc.DrawRectangle(Brushes.Black, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));
            }
            base.OnRender(dc);
        }
    }
}

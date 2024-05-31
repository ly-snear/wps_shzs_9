using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using PPt = Microsoft.Office.Interop.PowerPoint;

namespace CalligraphyAssistantMain.Controls
{
    public partial class PPTFrom : Window
    {
        // 定义PowerPoint应用程序对象
        PPt.Application pptApplication;
        // 定义演示文稿对象
        PPt.Presentation presentation;
        PPt.SlideShowWindows objSSWs;
        PPt.SlideShowTransition objSST;
        PPt.SlideShowSettings objSSS;
        PPt.SlideRange objSldRng;
        PPt.SlideShowView Slideshowview;
        // 定义幻灯片集合对象
        PPt.Slides slides;
        // 定义单个幻灯片对象
        PPt.Slide slide;
        // 幻灯片的数量
        int slidescount;
        // 幻灯片的索引
        int slideIndex;
        //获取当前幻灯片页码
        int IndexCount = 0;
        public string FilePath { get; set; }
        public PPTFrom(string parameter)
        {
            InitializeComponent();
            if (pptApplication != null)
            {
                return;
            }
            pptApplication = new PPt.Application();
            FilePath = parameter;
            //this.Loaded += new RoutedEventHandler(Window_Loaded);
            Presentation prs = this.OpenPpt(FilePath);
            this.PlayPpt(prs);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Presentation prs= this.OpenPpt(FilePath);
                this.PlayPpt(prs);
            }
            catch (Exception ex)
            {

            }
        }

        public PPt.Presentation OpenPpt(string url)
        {
            var objPresSet = pptApplication.Presentations;
            Presentation objPrs = objPresSet.Open(url, MsoTriState.msoTrue, MsoTriState.msoTrue, MsoTriState.msoFalse);
            return objPrs;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr childIntPtr, IntPtr parentIntPtr);

        public void PlayPpt(PPt.Presentation objPrs)
        {
            presentation = objPrs;
            //进入播放模式
            var objSlides = objPrs.Slides;
            var objSss = objPrs.SlideShowSettings;
            objSss.LoopUntilStopped = MsoTriState.msoCTrue;
            objSss.StartingSlide = 1;
            objSss.EndingSlide = objSlides.Count;
            objSss.ShowType = PPt.PpSlideShowType.ppShowTypeKiosk;
            var sw = objSss.Run();

            Slideshowview = objPrs.SlideShowWindow.View;
            var wn = (IntPtr)sw.HWND;

            //嵌入窗体
            var fromVisual = (HwndSource)PresentationSource.FromVisual(Panel);
            if (fromVisual == null)
            {
                return;
            }
            var parentHwnd = fromVisual.Handle;
            Thread.Sleep(200);
            SetParent(wn, parentHwnd);
            Thread.Sleep(200);
        }

        //////private void PptPlayWindow_OnLoaded(object sender, RoutedEventArgs e)
        //////{
        //////    Closed += OnClosed;
        //////}

        //////private void OnClosed(object sender, EventArgs eventArgs)
        //////{
        //////    try
        //////    {
        //////        ObjPrs.Close();
        //////        ObjApp.Quit();
        //////    }
        //////    catch
        //////    {

        //////    }
        //////}















    }
}

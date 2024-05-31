using CalligraphyAssistantMain.Code;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using PPt = Microsoft.Office.Interop.PowerPoint;
using OFFICECORE = Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Word;
using SharpDX;
using System.Windows.Xps.Packaging;
using Microsoft.Win32;



namespace CalligraphyAssistantMain.Controls
{
    /// <summary>
    /// ResourceItemControl.xaml 的交互逻辑
    /// </summary>
    public partial class ResourceItemControl : UserControl
    {
        public event EventHandler<DeleteResourceItemEventArgs> DeleteResourceItem = null;
        public event EventHandler DispenseClick = null;

        PPt.Application pptApplication;
        PPt.Presentation presentation;
        PPt.SlideShowWindows objSSWs;
        PPt.SlideShowTransition objSST;
        PPt.SlideShowSettings objSSS;
        PPt.SlideRange objSldRng;
        PPt.SlideShowView Slideshowview;
        PPt.Slides slides;
        PPt.Slide slide;
        int slidescount;
        int slideIndex;
        int IndexCount = 0;
        public ResourceItemControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openLb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Office.Interop.PowerPoint.Application pptApp = new Microsoft.Office.Interop.PowerPoint.Application();
            if (this.DataContext is ResourceItemInfo resourceItemInfo && File.Exists(resourceItemInfo.LocalPath))
            {
                string resourceName = resourceItemInfo.FileName;
                string extension = System.IO.Path.GetExtension(resourceName);
                if (extension.ToLower().Contains("pdf"))
                {
                    PDFFrom _PDFFrom = new PDFFrom(resourceItemInfo.LocalPath);
                    _PDFFrom.ShowDialog();
                }
                else if (extension.ToLower().Contains("ppt"))
                {
                    this.OpenPPT(resourceItemInfo.LocalPath);
                }
                else if (extension.ToLower().Contains("doc"))
                {
                    this.OpenDoc(resourceItemInfo.LocalPath);
                }
                else if (extension.ToLower().Contains("mp4") || extension.ToLower().Contains("avi"))
                {
                    MediaFrom _MediaFrom = new MediaFrom(resourceItemInfo.LocalPath);
                    _MediaFrom.Width = 800;
                    _MediaFrom.Height = 600;
                    _MediaFrom.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    _MediaFrom.WindowStyle = WindowStyle.SingleBorderWindow;
                    _MediaFrom.ShowDialog();
                }
                else
                {
                    EventNotify.OnShowDocEvent(resourceItemInfo.LocalPath);
                }
            }
        }

        private void DownloadFile(object sender, MouseButtonEventArgs e)
        {
            ResourceItemInfo resourceItemInfo = this.DataContext as ResourceItemInfo;
            //string load = AppDomain.CurrentDomain.BaseDirectory.Replace(@"\", "/").ToString();
            //string url = load + "Filedown/" + resourceItemInfo.LocalPath + ""; // 下载文件的网址
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();
            string Selected_Path = "";
            //选择下载保存位置
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Selected_Path = openFileDialog.SelectedPath;
                DownloadFileFrom df = new DownloadFileFrom(resourceItemInfo.ServerUrl, resourceItemInfo.FileName, Selected_Path);
                df.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                df.WindowStyle = WindowStyle.SingleBorderWindow;
                df.ShowDialog();
            }
        }

        /// <summary>
        /// 打开音频文件
        /// </summary>
        /// <param name="filePath"></param>
        public void ShowVideo(string filePath)
        {
            VideoFrom window = new VideoFrom();
            window.Width = 1024;
            window.Height = 768;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.WindowStyle = WindowStyle.SingleBorderWindow;

            double screenHeight = SystemParameters.FullPrimaryScreenHeight;
            double screenWidth = SystemParameters.FullPrimaryScreenWidth;
            window.Top = (screenHeight - window.Height) / 2;
            window.Left = (screenWidth - window.Width) / 2;

            MediaElement player = new MediaElement();
            //player.Margin = new Thickness(1, 1, 1, 1);
            player.Width = 800;
            player.Height = 600;

            player.Source = new Uri(filePath, UriKind.RelativeOrAbsolute);
            player.LoadedBehavior = MediaState.Manual;
            player.Stop();
            player.Play();

            window.Content = player;
            window.ShowDialog();
        }

        /// <summary>
        /// 打开PPT文件
        /// </summary>
        /// <param name="filePath"></param>
        public void OpenPPT(string filePath)
        {
            PPt.Application objApp = new PPt.Application();
            presentation = objApp.Presentations.Open(filePath, OFFICECORE.MsoTriState.msoFalse, OFFICECORE.MsoTriState.msoFalse, OFFICECORE.MsoTriState.msoFalse);
            slidescount = presentation.Slides.Count;
            objSldRng = presentation.Slides.Range(slidescount);
            objSST = objSldRng.SlideShowTransition;
            objSSS = presentation.SlideShowSettings;
            objSSS.Run();

            pptApplication = Marshal.GetActiveObject("PowerPoint.Application") as PPt.Application;
            Slideshowview = presentation.SlideShowWindow.View;
        }

        /// <summary>
        /// 打开doc文件 1 
        /// </summary>
        /// <param name="filePath"></param>
        public void OpenDoc(string filePath)
        {
            VideoFrom window = new VideoFrom();
            window.Width = 1024;
            window.Height = 768;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.WindowStyle = WindowStyle.SingleBorderWindow;

            DocumentViewer documentViewer = new DocumentViewer
            {
                Document = ConvertWordToXPS(filePath).GetFixedDocumentSequence()
            };
            documentViewer.FitToWidth();
            window.Content = documentViewer;
            window.ShowDialog();
        }

        /// <summary>
        /// 打开doc文件 2 
        /// </summary>
        /// <param name="filePath"></param>
        public void OpenDoc2(string filePath)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = filePath,
                    UseShellExecute = true
                };
                Process.Start(startInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error opening document: " + ex.Message);
            }
        }

        /// <summary>
        /// 打开doc文件 3 
        /// </summary>
        /// <param name="wordDocName"></param>
        /// <returns></returns>
        private XpsDocument ConvertWordToXPS(string wordDocName)
        {
            FileInfo fi = new FileInfo(wordDocName);
            XpsDocument result = null;
            string xpsDocName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.InternetCache), fi.Name);
            xpsDocName = xpsDocName.Replace(".docx", ".xps").Replace(".doc", ".xps");
            Microsoft.Office.Interop.Word.Application wordApplication = new Microsoft.Office.Interop.Word.Application();
            try
            {
                if (!File.Exists(xpsDocName))
                {
                    wordApplication.Documents.Add(wordDocName);
                    Document doc = wordApplication.ActiveDocument;
                    doc.ExportAsFixedFormat(xpsDocName, WdExportFormat.wdExportFormatXPS, false, WdExportOptimizeFor.wdExportOptimizeForPrint, WdExportRange.wdExportAllDocument, 0, 0, WdExportItem.wdExportDocumentContent, true, true, WdExportCreateBookmarks.wdExportCreateHeadingBookmarks, true, true, false, Type.Missing);
                    result = new XpsDocument(xpsDocName, System.IO.FileAccess.Read);
                }
                if (File.Exists(xpsDocName))
                {
                    result = new XpsDocument(xpsDocName, FileAccess.Read);
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                wordApplication.Quit(WdSaveOptions.wdDoNotSaveChanges);
            }
            wordApplication.Quit(WdSaveOptions.wdDoNotSaveChanges);
            return result;
        }

        private void deleteLb_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.DataContext is ResourceItemInfo resourceItemInfo)
            {
                if (System.Windows.MessageBox.Show("是否要删除该文件？", "提示", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question) == System.Windows.MessageBoxResult.Yes)
                {
                    DeleteResourceItem?.Invoke(this, new DeleteResourceItemEventArgs() { ResourceItemInfo = resourceItemInfo });
                }
            }
        }

        private void dispense_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DispenseClick?.Invoke(this.DataContext, e);
        }
    }
}



//嵌入窗体测试代码 刘洋 2024-03-12
/*
DocFrom _DocFrom = new DocFrom(resourceItemInfo.LocalPath);
_DocFrom.ShowDialog();

DocumentViewer documentViewer = new DocumentViewer();
var aaa = this.ConvertWordToXPS(resourceItemInfo.LocalPath);
documentViewer.Document = ConvertWordToXPS(aaa.Uri.LocalPath).GetFixedDocumentSequence();
documentViewer.FitToWidth();


//EventNotify.OnShowDocEvent(resourceItemInfo.LocalPath);
Microsoft.Office.Interop.Word.Application winword = new Microsoft.Office.Interop.Word.Application();
Microsoft.Office.Interop.Word.Document document = new Microsoft.Office.Interop.Word.Document();
try
{
    //Set status for word application is to be visible or not.
    //Create a missing variable for missing value
    object readOnly = false;
    object missing = System.Reflection.Missing.Value;
    object isVisible = true;
    document = winword.Documents.Open(resourceItemInfo.LocalPath, ReadOnly: false, Visible: true);
    document.Activate();
    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(winword);
    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(document);
}
catch (Exception ex)
{
    // WB.Close(false, Type.Missing, Type.Missing);
    throw;
}    */

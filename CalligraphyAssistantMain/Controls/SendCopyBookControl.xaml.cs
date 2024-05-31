using CalligraphyAssistantMain.Code;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
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
    /// <summary>
    /// SendCopyBookControl.xaml 的交互逻辑
    /// </summary>
    [AddINotifyPropertyChangedInterface]
    public partial class SendCopyBookControl : UserControl
    {
        /// <summary>
        /// 删除碑帖条目
        /// </summary>
        public static event EventHandler<ShareCopyBook> DeleteCopyBookClick = null;

        /// <summary>
        /// 备课文件选择字帖确认
        /// </summary>
        public static event EventHandler<List<ShareCopyBook>> CopyBookPrepareConfirmClick = null;

        /// <summary>
        /// 虎妞单字选择字帖确认
        /// </summary>
        public static event EventHandler<List<ShareCopyBook>> CopyBookWordConfirmClick = null;

        /// <summary>
        /// 虎妞碑帖选择字帖确认
        /// </summary>
        public static event EventHandler<List<ShareCopyBook>> CopyBookConfirmClick = null;

        public static void OnDeleteCopyBook(object sender, ShareCopyBook copybook)
        {
            DeleteCopyBookClick?.Invoke(sender, copybook);
        }

        /// <summary>
        /// 备课资源确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="copybooks"></param>
        public static void OnCopyBookPrepareConfirm(object sender, List<ShareCopyBook> copybooks)
        {
            CopyBookPrepareConfirmClick?.Invoke(sender, copybooks);
        }

        /// <summary>
        /// 虎妞单字确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="copybooks"></param>
        public static void OnCopyBookWordConfirm(object sender, List<ShareCopyBook> copybooks)
        {
            CopyBookWordConfirmClick?.Invoke(sender, copybooks);
        }

        /// <summary>
        /// 虎妞碑帖确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="copybooks"></param>
        public static void OnCopyBookConfirm(object sender, List<ShareCopyBook> copybooks)
        {
            CopyBookConfirmClick?.Invoke(sender, copybooks);
        }

        /// <summary>
        /// 将本地文件上传到指定的服务器
        /// 带有进度条
        /// </summary>
        /// <param name="url">接收文件路径</param>
        /// <param name="values">附加参数值</param>
        /// <param name="filePath">上传文件路径</param>
        /// <param name="progress">上传进度回调</param>
        /// <param name="exceptioned">上传异常回调</param>
        /// <param name="completed">上传成功回调</param>
        public void UploadFile(string url, string filePath, NameValueCollection values, Action<string, string, long, long, int> progress, Action<string, Exception> exceptioned, Action<string, string> completed)
        {
            var fileStream = File.OpenRead(filePath);
            var fileName = System.IO.Path.GetFileName(filePath);
            var ms = new MemoryStream();
            // Make a copy of the input stream in case sb uses disposable stream
            fileStream.CopyTo(ms);
            // Stream position needs to be set to zero - just to be sure.
            ms.Position = 0;

            try
            {
                const string contentType = "application/octet-stream";

                var request = WebRequest.Create(url);
                request.Method = "POST";
                request.Headers.Add("source", "pc");

                var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x", NumberFormatInfo.InvariantInfo);
                request.ContentType = "multipart/form-data; boundary=" + boundary;
                boundary = "--" + boundary;

                var dataStream = new MemoryStream();
                byte[] buffer;
                // Write the values
                foreach (string name in values.Keys)
                {
                    buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                    dataStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.ASCII.GetBytes(string.Format("Content-Disposition: form-data; name=\"{0}\"{1}{1}", name, Environment.NewLine));
                    dataStream.Write(buffer, 0, buffer.Length);
                    buffer = Encoding.UTF8.GetBytes(values[name] + Environment.NewLine);
                    dataStream.Write(buffer, 0, buffer.Length);
                }

                // Write the file
                buffer = Encoding.ASCII.GetBytes(boundary + Environment.NewLine);
                dataStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.UTF8.GetBytes($"Content-Disposition: form-data; name=\"file\"; filename=\"{fileName}\"{Environment.NewLine}");
                dataStream.Write(buffer, 0, buffer.Length);
                buffer = Encoding.ASCII.GetBytes(string.Format("Content-Type: {0}{1}{1}", contentType, Environment.NewLine));
                dataStream.Write(buffer, 0, buffer.Length);
                ms.CopyTo(dataStream);
                buffer = Encoding.ASCII.GetBytes(Environment.NewLine);
                dataStream.Write(buffer, 0, buffer.Length);

                buffer = Encoding.ASCII.GetBytes(boundary + "--");
                dataStream.Write(buffer, 0, buffer.Length);


                dataStream.Position = 0;
                // Important part: set content length to directly write to network socket
                request.ContentLength = dataStream.Length;
                var requestStream = request.GetRequestStream();

                // Write data in chunks and report progress
                var size = dataStream.Length;
                const int chunkSize = 64 * 1024;
                buffer = new byte[chunkSize];
                long bytesSent = 0;
                int readBytes;
                while ((readBytes = dataStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    requestStream.Write(buffer, 0, readBytes);
                    bytesSent += readBytes;

                    var status = "Uploading... " + bytesSent / 1024 + "KB of " + size / 1024 + "KB";
                    var percentage = Convert.ToInt32(100 * bytesSent / size);
                    progress(fileName, status, bytesSent, size, percentage);
                }

                // Get response from host
                using (var response = request.GetResponse())
                using (var responseStream = response.GetResponseStream())
                using (var stream = new MemoryStream())
                {
                    responseStream.CopyTo(stream);
                    var result = Encoding.UTF8.GetString(stream.ToArray());
                    completed(fileName, result);
                }
                ms.Close();
                ms.Dispose();
            }
            catch (Exception ex)
            {
                exceptioned(fileName, ex);
            }
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        /// <summary>
        /// 文件数量
        /// </summary>
        public ProgressBarValue uploadFiles { get; set; } = new ProgressBarValue();

        /// <summary>
        /// 文件进度
        /// </summary>
        public ProgressBarValue uploadFile { get; set; } = new ProgressBarValue();

        /// <summary>
        /// 分享字帖列表
        /// </summary>
        public ObservableCollection<ShareCopyBook> shareCopyBooks { get; set; } = new ObservableCollection<ShareCopyBook>();

        /// <summary>
        /// 上传索引
        /// </summary>
        public int uploadIndex = 0;

        /// <summary>
        /// 上传数量
        /// </summary>
        public int uploadFileCount = 0;

        /// <summary>
        /// 上传文件名称列表
        /// </summary>
        public string[] uploadFileNames = new string[] { };

        public List<CameraItemInfo> CameraItemInfos { get; set; } = new List<CameraItemInfo>();

        public SendCopyBookControl()
        {
            InitializeComponent();
            this.IsVisibleChanged += SendCopyBookControl_IsVisibleChanged;
            pbFiles.DataContext = uploadFiles;
            pbFile.DataContext = uploadFile;
            stuList.DataContext = this;
            copybook.DataContext = this;
            DeleteCopyBookClick += SendCopyBookControl_DeleteCopyBookClick;
            CopyBookPrepareConfirmClick += SendCopyBookControl_CopyBookPrepareConfirmClick;
            CopyBookWordConfirmClick += SendCopyBookControl_CopyBookWordConfirmClick;
            CopyBookConfirmClick += SendCopyBookControl_CopyBookConfirmClick;
        }

        /// <summary>
        /// 虎妞碑帖确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void SendCopyBookControl_CopyBookConfirmClick(object sender, List<ShareCopyBook> e)
        {
            if (null == e || 0 == e.Count) return;
            foreach (ShareCopyBook shareCopyBook in e)
            {
                shareCopyBooks.Add(shareCopyBook);
            }
        }

        /// <summary>
        /// 虎妞单字确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void SendCopyBookControl_CopyBookWordConfirmClick(object sender, List<ShareCopyBook> e)
        {
            if (null == e || 0 == e.Count) return;
            foreach (ShareCopyBook shareCopyBook in e)
            {
                shareCopyBooks.Add(shareCopyBook);
            }
        }

        /// <summary>
        /// 备课文件选择碑帖确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void SendCopyBookControl_CopyBookPrepareConfirmClick(object sender, List<ShareCopyBook> e)
        {
            if (null == e || 0 == e.Count) return;
            foreach (ShareCopyBook shareCopyBook in e)
            {
                shareCopyBooks.Add(shareCopyBook);
            }
        }

        /// <summary>
        /// 删除字帖
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendCopyBookControl_DeleteCopyBookClick(object sender, ShareCopyBook e)
        {
            if (null == e || string.IsNullOrEmpty(e.Id))
            {
                return;
            }
            int index = shareCopyBooks.ToList().IndexOf(e);
            if (index < 0)
            {
                return;
            }
            shareCopyBooks.RemoveAt(index);
        }

        private void SendCopyBookControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                BindStudentList();
            }
        }

        /// <summary>
        /// 关闭字帖发送框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
            EventNotify.OnSendCopyBookClose(sender, e);
        }

        /// <summary>
        /// 备课
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void prepareLesson_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EventNotify.OnCopyBookPrepareLessonOpen(sender, e);
        }

        /// <summary>
        /// 选择本地文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectFile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog dlg = new System.Windows.Forms.OpenFileDialog();
            dlg.Multiselect = true;
            dlg.Title = "选择字帖文件";
            dlg.Filter = "图片文件(*.jpg,*.gif,*.bmp,*.png,*.jpeg)|*.jpg;*.gif;*.bmp,*.png,*.jpeg|所有文件(*.*)|*.*";
            dlg.DefaultExt = ".jpg";
            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK || 0 == (uploadFileCount = dlg.FileNames.Length))
            {
                System.Windows.Forms.MessageBox.Show("选择的字帖文件无效", Common.AppCaption, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return;
            }
            //pb.Visibility = Visibility.Visible;
            uploadFiles.Min = 0;
            uploadFiles.Total = uploadFileCount;
            uploadFiles.Value = 0;
            uploadIndex = 0;
            uploadFileNames = dlg.FileNames;
            //uploadFileAct();
            NameValueCollection param = new NameValueCollection();
            param.Add("title", "");
            foreach (string file in dlg.FileNames)
            {
                uploadFile.Min = 0;
                uploadFile.Total = 100;
                UploadFile(Common.FileServerUrl, uploadFileNames[uploadIndex], param, uploadProgress, uploadExceptioned, uploadCompleted);
            }
        }

        /// <summary>
        /// 保留
        /// </summary>
        private void uploadFileAct()
        {
            System.Windows.Forms.MessageBox.Show($"{uploadIndex + 1}/{uploadFileNames.Length}");
            if (uploadIndex >= uploadFileNames.Length)
            {
                System.Windows.Forms.MessageBox.Show("上传完毕");
                return;
            }
            uploadFile.Min = 0;
            uploadFile.Total = 100;
            NameValueCollection param = new NameValueCollection();
            param.Add("title", "");
            UploadFile(Common.FileServerUrl, uploadFileNames[uploadIndex], param, uploadProgress, uploadExceptioned, uploadCompleted);
        }

        /// <summary>
        /// 上传进度回调
        /// </summary>
        private void uploadProgress(string file, string status, long value, long total, int percentage)
        {
            uploadFile.Value = percentage;
        }

        /// <summary>
        /// 上传文件异常
        /// </summary>
        /// <param name="file"></param>
        /// <param name="ex"></param>
        private void uploadExceptioned(string file, Exception ex)
        {
            Common.ShowTip($"上传文件：{file}-->异常：{JsonHelper.ToJson(ex)}");
            //System.Windows.Forms.MessageBox.Show($"上传文件：{file}-->异常：{JsonHelper.ToJson(ex)}");
            uploadFiles.Value++;
            uploadIndex++;
            if (uploadIndex >= uploadFileNames.Length)
            {
                uploadFinish();
            }
        }

        /// <summary>
        /// 上传完成一个文件
        /// </summary>
        /// <param name="file"></param>
        /// <param name="result"></param>
        private void uploadCompleted(string file, string result)
        {
            uploadFiles.Value++;
            uploadIndex++;
            //System.Windows.Forms.MessageBox.Show(result);
            ResultUpload resultUpload = result.ToObject<ResultUpload>();
            if (resultUpload != null && resultUpload.code == 0 && resultUpload.data != null && !string.IsNullOrEmpty(resultUpload.data.title) && !string.IsNullOrEmpty(resultUpload.data.url))
            {
                ShareCopyBook shareCopyBook = new ShareCopyBook();
                shareCopyBook.Id = Guid.NewGuid().ToString();
                shareCopyBook.Title = resultUpload.data.title;
                shareCopyBook.Url = resultUpload.data.url.Replace(":/", "://");
                shareCopyBook.IsSelect = false;
                shareCopyBook.Type = 1;
                //System.Windows.Forms.MessageBox.Show(shareCopyBook.ToJson());
                shareCopyBooks.Add(shareCopyBook);
            }
            else
            {
                Common.ShowTip($"上传文件：{file}，后服务器返回无效数据");
                //System.Windows.Forms.MessageBox.Show($"上传文件：{file}，后服务器返回无效数据");
            }
            if (uploadIndex >= uploadFileNames.Length)
            {
                uploadFinish();
            }
        }

        /// <summary>
        /// 上传文件结束
        /// </summary>
        private void uploadFinish()
        {
            //System.Windows.Forms.MessageBox.Show($"文件表：{shareCopyBooks.ToJson()}");
        }

        /// <summary>
        /// 绑定学生列表
        /// </summary>
        public void BindStudentList()
        {
            if (null == Common.CameraList || 0 == Common.CameraList.Count)
            {
                MessageBox.Show("没有找到学生", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CameraItemInfos = Common.CameraList.Clone();
            CameraItemInfos.ForEach(item =>
            {
                item.IsSelected = true;
                item.StudentList.ForEach(s =>
                {
                    s.PropertyChanged += PropertyChanged;
                    s.IsSelected = item.IsSelected;
                }
                );
                item.PropertyChanged += PropertyChanged;
            });
        }

        private void PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected" && sender is CameraItemInfo info)
            {
                info.StudentList.ForEach(s => s.IsSelected = info.IsSelected);
            }
            if (e.PropertyName == "IsSelected" && sender is StudentInfo student)
            {
                int count = 0;
                CameraItemInfos.ForEach(item => item.StudentList.ForEach(s =>
                {
                    if (s.IsSelected)
                        count++;
                }));
                selectCount.Text = count.ToString();
            }
        }

        /// <summary>
        /// 删除对个列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deleteCopybooks_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            List<ShareCopyBook> selected = shareCopyBooks.ToList().Where(sc => sc.IsSelect).ToList();
            if (null == selected || 0 == selected.Count)
            {
                MessageBox.Show("选择要删除的字帖", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            for (int i = shareCopyBooks.Count - 1; i >= 0; i--)
            {
                if (shareCopyBooks[i].IsSelect)
                {
                    shareCopyBooks.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// 发送到学生端
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendCopybooks_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            List<StudentInfo> students = new List<StudentInfo>();
            CameraItemInfos.ForEach(g =>
            {
                g.StudentList.ForEach(s =>
                {
                    if (s.IsSelected)
                    {
                        students.Add(s);
                    }
                });
            });
            if (null == students || 0 == students.Count)
            {
                MessageBox.Show("选择接收字帖的学生", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<ShareCopyBook> selected = shareCopyBooks.ToList().Where(s => s.IsSelect).ToList();
            if (null == selected || 0 == selected.Count)
            {
                MessageBox.Show("选择要分享的字帖", Common.AppCaption, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            List<ShareCopyBookData> data = new List<ShareCopyBookData>();
            foreach (ShareCopyBook sc in selected)
            {
                data.Add(new ShareCopyBookData(sc.Title, sc.Url, sc.Type));
            }
            //MessageBox.Show(data.ToJson());
            MessageType type = MessageType.ShareCopyBook;
            students.ForEach(s =>
            {
                MQCenter.Instance.Send(s, type, data);
            });
            Common.ShowTip("完成字帖分享");
            this.Visibility = Visibility.Collapsed;
            EventNotify.OnSendCopyBookClose(sender, e);
        }

        /// <summary>
        /// 虎妞单字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tigerWord_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EventNotify.OnCopyBookTigerRordOpen(sender, e);
        }

        /// <summary>
        /// 虎妞碑帖
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tiger_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            EventNotify.OnCopyBookTigerOpen(sender, e);
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectAllBtn_Checked(object sender, RoutedEventArgs e)
        {
            CameraItemInfos.ForEach(item =>
            {
                item.IsSelected = true;
            });
        }

        /// <summary>
        /// 反选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void invertBtn_Checked(object sender, RoutedEventArgs e)
        {
            CameraItemInfos.ForEach(item =>
            {
                item.StudentList.ForEach(s => s.IsSelected = !s.IsSelected);

            });
        }

        /// <summary>
        /// 取消选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uncheckBtn_Checked(object sender, RoutedEventArgs e)
        {
            CameraItemInfos.ForEach(item =>
            {
                item.IsSelected = false;
                item.StudentList.ForEach(s => s.IsSelected = false);
            });
        }
    }
}

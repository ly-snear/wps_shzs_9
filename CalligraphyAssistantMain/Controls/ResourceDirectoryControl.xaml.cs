using CalligraphyAssistantMain.Code;
using CommonServiceLocator;
using Newtonsoft.Json;
using Prism.Events;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CalligraphyAssistantMain.Controls
{
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// ResourceDirectoryControl.xaml 的交互逻辑
    /// </summary>
    public partial class ResourceDirectoryControl : UserControl
    {
        public event EventHandler CheckDistributionClick = null;
        private string localResourceDirectory = string.Empty;
        private bool startSynchronizeProcess = false;
        public List<ResourceItemInfo> ResourceList { get; set; } = new List<ResourceItemInfo>();
        public ObservableCollection<ResourceItemInfo> ResourceListCollectionPaging { get; set; }
        public Pager<ResourceItemInfo> Pager { get; set; }
        private IEventAggregator eventAggregator;

        public ResourceDirectoryControl()
        {
            InitializeComponent();
            this.DataContext = this;
            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<MQMessageEvent>().Subscribe(Messaging);
        }

        private void RemoveItem(ResourceItemInfo resourceItemInfo)
        {
            try
            {
                if (File.Exists(resourceItemInfo.LocalPath))
                {
                    File.Delete(resourceItemInfo.LocalPath);
                }
                if (resourceItemInfo.State == DownloadState.Downloading)
                {
                    resourceItemInfo.State = DownloadState.Canceled;
                    resourceItemInfo.CancelToken.Cancel();
                }
                ResourceList.Remove(resourceItemInfo);
            }
            catch (Exception ex)
            {
                Common.Trace(ex.ToString());
            }
        }
        public void Clear()
        {
            ResourceListCollectionPaging?.Clear();
            ResourceList?.Clear();
        }

        private void ResourceItemControl_DeleteResourceItem(object sender, DeleteResourceItemEventArgs e)
        {
            ResourceItemInfo resourceItemInfo = e.ResourceItemInfo;
            RemoveItem(resourceItemInfo);
        }

        public void StartSynchronize(string localResourceDirectory)
        {
            this.localResourceDirectory = localResourceDirectory;
            if (!Directory.Exists(localResourceDirectory))
            {
                Directory.CreateDirectory(localResourceDirectory);
            }
            StartSynchronizeProcess();
            InitData();

        }
        public void InitData()
        {
            Common.resourceItemInfos = ResourceList;
            if (ResourceList.Count > 0)
            {
                view.Visibility = Visibility.Visible;
                Pager = new Pager<ResourceItemInfo>(ResourceList.Count > 8 ? 8 : ResourceList.Count, ResourceList);
                Pager.PagerUpdated += items =>
                {
                    ResourceListCollectionPaging = new ObservableCollection<ResourceItemInfo>(items);
                };
                Pager.CurPageIndex = 1;
            }
            else
            {
                view.Visibility = Visibility.Hidden;
            }
        }

        public void StopSynchronize()
        {
            startSynchronizeProcess = false;
            ResourceListCollectionPaging?.Clear();
            ResourceList?.Clear();
        }

        private void StartSynchronizeProcess()
        {
            if (startSynchronizeProcess)
            {
                return;
            }

            startSynchronizeProcess = true;
            Task.Factory.StartNew(() =>
            {
                NameValueCollection headerDict = new NameValueCollection
                {
                    { "token", Common.CurrentUser.Token }
                };
                string jsonResult = HttpUtility.DownloadString(Common.GetResourceDirectory + $"?course={Common.CurrentLesson.Name}", Encoding.UTF8, headerDict);
                ResultInfo<List<ResultResourceFolderInfo>> result = JsonConvert.DeserializeObject<ResultInfo<List<ResultResourceFolderInfo>>>(jsonResult);
                if (result != null && result.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                {
                    //ResultResourceFolderInfo folderInfo = result.Body.FirstOrDefault(p => p.Text == Common.CurrentLesson.Name);
                    //if (folderInfo != null)
                    //{
                    //////while (startSynchronizeProcess)
                    //////{
                        try
                        {
                            //jsonResult = HttpUtility.DownloadString(Common.GetResourceSubDirectory + $"?catalog={folderInfo.Id}", Encoding.UTF8, headerDict);
                            jsonResult = HttpUtility.DownloadString(Common.GetResourceSubDirectoryEx + $"?course={Common.CurrentLesson.Name}", Encoding.UTF8, headerDict);
                            ResultInfo<List<ResultResourceItemInfo>> result2 = JsonConvert.DeserializeObject<ResultInfo<List<ResultResourceItemInfo>>>(jsonResult);
                            if (result2 != null && result2.Ok.Equals("true", StringComparison.OrdinalIgnoreCase))
                            {
                                List<ResultResourceItemInfo> tempList = result2.Body.ToList();
                                List<ResourceItemInfo> downloadList = ResourceList.ToList();
                                foreach (ResultResourceItemInfo item in tempList)
                                {
                                    if (item.Url.StartsWith("//"))
                                    {
                                        item.Url = "http:" + item.Url;
                                    }
                                    ResourceItemInfo resourceItemInfo = downloadList.FirstOrDefault(p => p.ServerUrl == item.Url);
                                    if (resourceItemInfo != null)
                                    {
                                        if (resourceItemInfo.ServerId != item.Id)
                                        {
                                            resourceItemInfo.FileSize = HttpUtility.GetServerFileSize(item.Url);
                                            resourceItemInfo.State = DownloadState.Downloading;
                                            if (File.Exists(resourceItemInfo.LocalPath))
                                            {
                                                File.Delete(resourceItemInfo.LocalPath);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        long fileSize;
                                        string localPath = localResourceDirectory + item.Name;
                                        if (File.Exists(localPath))
                                        {
                                            fileSize = new FileInfo(localPath).Length;
                                        }
                                        else
                                        {
                                            fileSize = HttpUtility.GetServerFileSize(item.Url);
                                        }
                                        this.Dispatcher.Invoke(() =>
                                        {
                                            ResourceItemInfo resourceItem = new ResourceItemInfo()
                                            {
                                                ServerId = item.Id,
                                                FileName = item.Name,
                                                ServerUrl = item.Url,
                                                State = File.Exists(localPath) ? DownloadState.Downloaded : DownloadState.Downloading,
                                                LocalPath = localPath,
                                                FileIcon = GetPathIco(item.Name),
                                                ModifyTime = item.Time,
                                                FileSize = fileSize,
                                                CancelToken = new CancellationTokenSource()
                                            };
                                            ResourceList.Add(resourceItem);
                                            //ResourceItemControl resourceItemControl = new ResourceItemControl();
                                            //resourceItemControl.DataContext = resourceItem;
                                            //resourceItemControl.DeleteResourceItem += ResourceItemControl_DeleteResourceItem;
                                            //resourceItem.Owner = resourceItemControl;
                                            InitData();
                                        });
                                    }
                                }

                                if (downloadList.Count > 0)
                                {
                                    foreach (var item in downloadList)
                                    {
                                        if (!tempList.Any(p => p.Id == item.ServerId))
                                        {
                                            this.Dispatcher.Invoke(() =>
                                            {
                                                RemoveItem(item);
                                            });
                                            continue;
                                        }
                                        if (File.Exists(item.LocalPath))
                                        {
                                            item.State = DownloadState.Downloaded;
                                            continue;
                                        }
                                        else
                                        {
                                            item.State = DownloadState.Downloading;
                                            if (item.CancelToken == null)
                                            {
                                                item.CancelToken = new CancellationTokenSource();
                                            }
                                        }
                                        string tempFile = item.LocalPath + ".temp";
                                        if (HttpUtility.DownloadFile(item.ServerUrl, tempFile, item.CancelToken))
                                        {
                                            File.Move(tempFile, item.LocalPath);
                                            item.State = DownloadState.Downloaded;
                                        }
                                        item.CancelToken.Dispose();
                                        item.CancelToken = null;
                                        if (item.State == DownloadState.Canceled)
                                        {
                                            try
                                            {
                                                if (File.Exists(tempFile))
                                                {
                                                    File.Delete(tempFile);
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Common.Trace(ex.ToString());
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Common.Trace(ex.ToString());
                        }
                    //////    Thread.Sleep(5000);
                    //////}
                    //}
                }
            }, TaskCreationOptions.LongRunning);
        }

        private BitmapSource GetPathIco(string path)
        {
            Icon icon = IconTools.GetFileIcon(path);
            BitmapSource bitmapSource = Common.BitmapToBitmapSource(icon.ToBitmap(), PixelFormats.Pbgra32);
            icon.Dispose();
            return bitmapSource;
        }

        private void cancelBtn_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void ReturnBtn_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }

        private void ResourceItemControl_DispenseClick(object sender, EventArgs e)
        {
            if (sender is ResourceItemInfo info)
            {
                if (info.DispensedUsers.Count > 0)
                {
                    CheckDistributionClick?.Invoke(info, e);
                }
                else
                {
                    SelectStudentsControl selectStudentsControl = new SelectStudentsControl();
                    selectStudentsControl.BindStudentList(Common.CameraList);
                    if (selectStudentsControl.ShowDialog() == true)
                    {
                        List<ResourceDispensedUser> users = new List<ResourceDispensedUser>();
                        selectStudentsControl.CameraItemInfos.ForEach(p => p.StudentList.ForEach(s =>
                        {
                            if (s.IsSelected)
                            {
                                users.Add(new ResourceDispensedUser() { Id = s.Id, UserName = s.Name, ip = s.IP });
                                //通知选中的学生
                                MQCenter.Instance.Send(s, MessageType.FileDistribute, new
                                {
                                    id = info.ServerId,
                                    fileName = info.FileName,
                                    url = info.ServerUrl,
                                    fileSize = info.FileSize,
                                });
                            }
                        }));

                        if (users.Count > 0)
                        {
                            info.DispensedUsers = users;
                        }
                    }
                }

            }
        }
        private void Messaging(Code.Message msg)
        {
            try
            {
                if (msg != null && msg.classId == Common.CurrentClassV2.ClassId && msg.lessonId == Common.CurrentLesson.Id && msg.userType == UserType.student)
                {
                    switch (msg.type)
                    {
                        case MessageType.FileAccept:
                            {
                                var data = new { id = 0 };
                                dynamic json = JsonConvert.DeserializeAnonymousType(msg.data.ToString(), data);
                                if (json != null)
                                {
                                    var item = ResourceList.FirstOrDefault(p => p.ServerId == json.id);
                                    if (item != null)
                                    {
                                        item.DispensedUsers.ForEach(p =>
                                        {
                                            if (p.Id == msg.sendUserId)
                                            {
                                                p.IsComplete = true;
                                            }
                                        });
                                        item.CompleteCount = item.DispensedUsers.Count(p => p.IsComplete);
                                    }

                                }

                            }
                            break;

                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
    }
}
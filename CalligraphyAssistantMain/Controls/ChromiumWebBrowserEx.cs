using CalligraphyAssistantMain.Code;
using CefSharp;
using CefSharp.Wpf;
using CefSharp.Wpf.Internals;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalligraphyAssistantMain.Controls
{
    public class ChromiumWebBrowserEx : ChromiumWebBrowser, ILifeSpanHandler, IContextMenuHandler, IKeyboardHandler, IDownloadHandler
    {
        private Dictionary<int, DateTime> cancelDict = new Dictionary<int, DateTime>();
        public event EventHandler<DownloadProgressChangedEventArgs> DownloadProgressChanged = null;
        public ChromiumWebBrowserEx()
        {
            this.LifeSpanHandler = this;
            this.MenuHandler = this;
            this.DownloadHandler = this;
#if DEBUG
            this.KeyboardHandler = this;
#endif
        }

        #region ILifeSpanHandler
        public bool DoClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {
            if (browser.IsDisposed || browser.IsPopup)
            {
                return false;
            }
            return true;
        }

        public void OnAfterCreated(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {

        }

        public void OnBeforeClose(IWebBrowser chromiumWebBrowser, IBrowser browser)
        {

        }

        public bool OnBeforePopup(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, string targetUrl, string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures, IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            this.Dispatcher.Invoke(() =>
            {
                this.Address = targetUrl;
            });
            newBrowser = null;
            return true;
        }

        #endregion

        #region IContextMenuHandler
        public void OnBeforeContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            model.Clear();
            return;
        }

        public bool OnContextMenuCommand(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {
            return false;
        }

        public void OnContextMenuDismissed(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame)
        {

        }

        public bool RunContextMenu(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            return false;
        }
        #endregion

        #region IKeyboardHandler
        public bool OnPreKeyEvent(IWebBrowser chromiumWebBrowser, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
        {
            return false;
        }

        public bool OnKeyEvent(IWebBrowser chromiumWebBrowser, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey)
        {
            if (type == KeyType.KeyUp && (VirtualKeys)windowsKeyCode == VirtualKeys.F12)
            {
                browser.ShowDevTools();
            }
            return false;
        }
        #endregion

        #region IDownloadHandler

        public void OnBeforeDownload(IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            string downloadFolder = Common.SettingsPath + "Download\\";
            if (!Directory.Exists(downloadFolder))
            {
                Directory.CreateDirectory(downloadFolder);
            }
            string savePath = Common.SettingsPath + "Download\\" + downloadItem.SuggestedFileName;
            if (File.Exists(savePath))
            {
                try
                {
                    int index = 1;
                    string tempPath = savePath;
                    while (true)
                    {
                        string filePath = tempPath = Common.SettingsPath + "Download\\" + Path.GetFileNameWithoutExtension(downloadItem.SuggestedFileName);
                        string ext = Path.GetExtension(downloadItem.SuggestedFileName);
                        string newSavePath = $"{filePath}({index}){ext}";
                        if (File.Exists(newSavePath))
                        {
                            tempPath = newSavePath;
                            index++;
                        }
                        else
                        {
                            savePath = newSavePath;
                            break;
                        }
                        if (index > 100)
                        {
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Common.Trace("ChromiumWebBrowserEx OnBeforeDownload Error:" + ex.Message);
                }
            }
            callback.Continue(savePath, true);
        }

        public void OnDownloadUpdated(IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {
            if (cancelDict.Count > 0 && cancelDict.ContainsKey(downloadItem.Id))
            {
                try
                {
                    cancelDict.Remove(downloadItem.Id);
                    callback.Cancel();
                    return;
                }
                catch (Exception ex)
                {
                    Common.Trace("ChromiumWebBrowserEx OnDownloadUpdated Error:" + ex.Message);
                }
            }
            if (DownloadProgressChanged != null && !string.IsNullOrEmpty(downloadItem.FullPath))
            {
                DownloadProgressChanged(this, new DownloadProgressChangedEventArgs() { DownloadItem = downloadItem, Callback = callback });
            }
        }
        #endregion

        public void Cancel(int id)
        {
            if (!cancelDict.ContainsKey(id))
            {
                cancelDict.Add(id, DateTime.Now);
            }
        }
    }
}

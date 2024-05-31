using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;

namespace CalligraphyAssistantMain.Code
{
    public class WinAPI
    {
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        public const int GWL_STYLE = (-16);
        public const int GWL_EXSTYLE = (-20);
        public const int WM_HOTKEY = 0x312;

        public const uint WS_OVERLAPPED = 0;
        public const uint WS_POPUP = 0x80000000;
        public const uint WS_CHILD = 0x40000000;
        public const uint WS_MINIMIZE = 0x20000000;
        public const uint WS_VISIBLE = 0x10000000;
        public const uint WS_DISABLED = 0x8000000;
        public const uint WS_CLIPSIBLINGS = 0x4000000;
        public const uint WS_CLIPCHILDREN = 0x2000000;
        public const uint WS_MAXIMIZE = 0x1000000;
        public const uint WS_CAPTION = 0xC00000;      // WS_BORDER or WS_DLGFRAME  
        public const uint WS_BORDER = 0x800000;
        public const uint WS_DLGFRAME = 0x400000;
        public const uint WS_VSCROLL = 0x200000;
        public const uint WS_HSCROLL = 0x100000;
        public const uint WS_SYSMENU = 0x80000;
        public const uint WS_THICKFRAME = 0x40000;
        public const uint WS_GROUP = 0x20000;
        public const uint WS_TABSTOP = 0x10000;
        public const uint WS_MINIMIZEBOX = 0x20000;
        public const uint WS_MAXIMIZEBOX = 0x10000;
        public const uint WS_TILED = WS_OVERLAPPED;
        public const uint WS_ICONIC = WS_MINIMIZE;
        public const uint WS_SIZEBOX = WS_THICKFRAME;

        public const uint WS_EX_APPWINDOW = 0x40000;
        public const uint WS_EX_TOOLWINDOW = 0x80;

        public const int WM_COPYDATA = 0x004A;

        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOMOVE = 0x0002;

        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_RBUTTONDOWN = 0x0204;

        private const int SW_NORMAL = 1;//正常
        private const int SW_SHOWMINIMIZED = 2;//最大化
        private const int SW_SHOWMAXIMIZED = 3;//最小化
        private const int SW_SHOWNOACTIVATE = 4;
        private const int SW_SHOW = 5;//显示
        private const int SW_RESTORE = 9;//保持原状

        private const uint GW_OWNER = 4;
        private const uint GW_HWNDNEXT = 2;

        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;

        public const uint SDC_APPLY = 0x00000080;
        public const uint SDC_TOPOLOGY_INTERNAL = 0x00000001;
        public const uint SDC_TOPOLOGY_CLONE = 0x00000002;
        /// <summary>
        /// 扩展模式
        /// </summary>
        public const uint SDC_TOPOLOGY_EXTEND = 0x00000004;

        #region DeviceCaps常量
        public const int HORZRES = 8;
        public const int VERTRES = 10;
        public const int LOGPIXELSX = 88;
        public const int LOGPIXELSY = 90;
        public const int DESKTOPVERTRES = 117;
        public const int DESKTOPHORZRES = 118;
        #endregion

        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        public static readonly IntPtr HWND_TOP = new IntPtr(0);
        public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        private delegate bool EnumWindowsProc(IntPtr hWnd, ref SearchData data);

        private delegate bool EnumDialogWindowsProc(IntPtr hWnd);

        public class SearchData
        {
            public string Title;
            public IntPtr hWnd;
        }

        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }

        public enum SetWindowState
        {
            Max,
            Min,
            Normal,
            Default,
            Top
        }

        [Flags]
        enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0,
            SMTO_BLOCK = 0x1,
            SMTO_ABORTIFHUNG = 0x2,
            SMTO_NOTIMEOUTIFNOTHUNG = 0x8,
            SMTO_ERRORONEXIT = 0x20
        }

        public enum StretchBltMode : int
        {
            STRETCH_ANDSCANS = 1,
            STRETCH_ORSCANS = 2,
            STRETCH_DELETESCANS = 3,
            STRETCH_HALFTONE = 4,
        }

        [DllImport("user32.dll")]//拖动无窗体的控件
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        // 关闭64位（文件系统）的操作转向
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);
        // 开启64位（文件系统）的操作转向
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

        [DllImport("user32", EntryPoint = "SetWindowLong")]
        public static extern int SetWindowLong(IntPtr hwnd, int nIndex, uint dwNewLong);

        [DllImport("user32", EntryPoint = "GetWindowLong")]
        public static extern uint GetWindowLong(IntPtr hwnd, int nIndex);

        /// <summary>
        /// 获取调用线程的活动窗口
        /// </summary>
        /// <returns></returns>
        [DllImport("user32", EntryPoint = "GetActiveWindow")]
        public static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, ref SearchData data);

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr intPtr);

        [DllImport("user32.dll")]
        private static extern bool EnumThreadWindows(uint dwThreadId, EnumDialogWindowsProc lpEnumFunc, IntPtr intPtr);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessageTimeout(
            IntPtr hWnd,
            uint Msg,
            int wParam,
            ref COPYDATASTRUCT lParam,
            SendMessageTimeoutFlags fuFlags,
            uint uTimeout,
            out UIntPtr lpdwResult);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindow(IntPtr hWnd, uint wCmd);

        [DllImport("user32.dll")]
        static extern bool EndDialog(IntPtr hDlg, out IntPtr nResult);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetWindowText(IntPtr hWnd, StringBuilder title, int maxBufSize);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool FlashWindow(IntPtr hWnd, bool bInvert);

        [DllImport("user32.dll")]
        public static extern bool IsZoomed(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsWindowEnabled(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32")]
        public static extern bool ChangeWindowMessageFilter(uint msg, int flags);

        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory")]
        public static extern void CopyMemory(byte[] Destination, IntPtr Source, [MarshalAs(UnmanagedType.U4)] int Length);

        [DllImport("kernel32.dll")]
        public static extern void CopyMemory(IntPtr destination, IntPtr soruce, int length);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);

        [DllImport("user32.dll", EntryPoint = "GetParent", SetLastError = true)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("winmm.dll")]
        public static extern long waveOutSetVolume(UInt32 deviceID, UInt32 Volume);

        [DllImport("gdi32.dll")]
        private static extern bool BitBlt(
            IntPtr hdcDest, //目标设备的句柄
            int nXDest,     // 目标对象的左上角的X坐标
            int nYDest,     // 目标对象的左上角的X坐标
            int nWidth,     // 目标对象的矩形的宽度
            int nHeight,    // 目标对象的矩形的长度
            IntPtr hdcSrc,  // 源设备的句柄
            int nXSrc,      // 源对象的左上角的X坐标
            int nYSrc,      // 源对象的左上角的X坐标
            int dwRop       // 光栅的操作值
            );

        [DllImport("gdi32", EntryPoint = "StretchBlt")]
        public static extern int StretchBlt(
             IntPtr hdc,
             int x,
             int y,
             int nWidth,
             int nHeight,
             IntPtr hSrcDC,
             int xSrc,
             int ySrc,
             int nSrcWidth,
             int nSrcHeight,
             int dwRop
        );
        [DllImport("gdi32.dll")]
        public static extern int SetStretchBltMode(IntPtr hdc, StretchBltMode iStretchMode);

        public const int SRCCOPY = 0xCC0020;
        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdcPtr);
        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hdcPtr, IntPtr hObject);
        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern bool DeleteDC(IntPtr hdcPtr);
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern long SetDisplayConfig(uint numPathArrayElements, IntPtr pathArray, uint numModeArrayElements,
                                                    IntPtr modeArray, uint flags);
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr ptr);

        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(
            IntPtr hdc, // handle to DC  
            int nIndex // index of capability  
        );

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);

        [DllImport("User32.dll")]
        public static extern int PrivateExtractIcons(
            string lpszFile, //文件名可以是exe,dll,ico,cur,ani,bmp
            int nIconIndex,  //从第几个图标开始获取
            int cxIcon,      //获取图标的尺寸x
            int cyIcon,      //获取图标的尺寸y
            IntPtr[] phicon, //获取到的图标指针数组
            int[] piconid,   //图标对应的资源编号
            int nIcons,      //指定获取的图标数量，仅当文件类型为.exe 和 .dll时候可用
            int flags        //标志，默认0就可以，具体可以看LoadImage函数
        );
        [DllImport("User32.dll")]
        public static extern bool DestroyIcon(
              IntPtr hIcon //A handle to the icon to be destroyed. The icon must not be in use.
          );
        /// 
        /// 注册热键
        /// 
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, ModifierKeys fsModifuers, int vk);

        /// 
        /// 注销热键
        /// 
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        public static IntPtr SearchForWindow(string title)
        {
            SearchData searchData = new SearchData { Title = title };
            EnumWindows(new EnumWindowsProc(EnumProc), ref searchData);
            return searchData.hWnd;
        }

        public static void SetTopMost(IntPtr hWnd, bool top)
        {
            SetWindowPos(hWnd, top ? HWND_TOPMOST : HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
        }

        public static void SetMute(IntPtr hWnd)
        {
            SendMessage(hWnd, WM_APPCOMMAND, (int)hWnd,
                APPCOMMAND_VOLUME_MUTE);
        }

        public static bool IsChildControl(IntPtr handle, IntPtr parentHandle)
        {
            while (handle != IntPtr.Zero)
            {
                if (handle != parentHandle)
                {
                    handle = WinAPI.GetParent(handle);
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 改变窗体的状态
        /// </summary>
        /// <param name="hWnd">窗体句柄</param>
        /// <param name="setWindowState">窗体状态</param>
        public static void SetWindowStyle(IntPtr hWnd, SetWindowState setWindowState)
        {
            switch (setWindowState)
            {
                case SetWindowState.Max:
                    ShowWindow(hWnd, SW_SHOWMAXIMIZED);
                    break;
                case SetWindowState.Min:
                    ShowWindow(hWnd, SW_SHOWMINIMIZED);
                    return;
                case SetWindowState.Normal:
                    ShowWindow(hWnd, SW_NORMAL);
                    break;
                case SetWindowState.Top:
                default:
                    ShowWindow(hWnd, IsZoomed(hWnd) ? SW_SHOWMAXIMIZED : SW_SHOWNOACTIVATE);
                    break;
            }
            SetForegroundWindow(hWnd);
        }

        public static void SendCustomMessage(IntPtr hWnd, string message)
        {
            byte[] sarr = Encoding.Default.GetBytes(message);
            int len = sarr.Length;
            COPYDATASTRUCT cds;
            cds.dwData = (IntPtr)100;
            cds.lpData = message;
            cds.cbData = len + 1;
            //SendMessage(hWnd, WM_COPYDATA, 0, ref cds); 
            UIntPtr result;
            SendMessageTimeout(hWnd, WM_COPYDATA, 0, ref cds, SendMessageTimeoutFlags.SMTO_NORMAL, 200, out result);
        }

        public static void CloseAllMessageBox()
        {
            Process process = Process.GetCurrentProcess();
            if (process.MainWindowHandle != IntPtr.Zero)
            {
                for (int i = 0; i < process.Threads.Count; i++)
                {
                    try
                    {
                        EnumThreadWindows((uint)process.Threads[i].Id, new EnumDialogWindowsProc(EnumProc), IntPtr.Zero);
                    }
                    catch
                    {
                    }
                }
            }
        }

        private static bool EnumProc(IntPtr hWnd)
        {
            if (hWnd != IntPtr.Zero)
            {
                IntPtr ownerHwnd = GetWindow(hWnd, GW_OWNER);
                if (ownerHwnd != IntPtr.Zero &&
                    !IsWindowEnabled(ownerHwnd))
                {
                    IntPtr result;
                    EndDialog(hWnd, out result);
                }
            }
            return true;
        }

        private static bool EnumProc(IntPtr hWnd, ref SearchData data)
        {
            StringBuilder stringBuilder = new StringBuilder(256);
            if (GetWindowText(hWnd, stringBuilder, 256))
            {
                string title = stringBuilder.ToString();
                if (title.StartsWith(data.Title))
                {
                    data.hWnd = hWnd;
                    return false;
                }
            }
            return true;
        }
    }
    public static class WinAPI_V2
    {

        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        public static extern int waveOutGetNumDevs();
        [DllImport("Winmm.dll", CharSet = CharSet.Auto)]
        public static extern int midiOutGetNumDevs();
        [DllImport("kernel32")] // 写入配置文件的接口
        public static extern bool WritePrivateProfileString(byte[] section, byte[] key, byte[] val, string filePath);
        [DllImport("kernel32")] // 读取配置文件的接口
        public static extern int GetPrivateProfileString(byte[] section, byte[] key, byte[] def, byte[] retVal, int size, string filePath);

        [DllImportAttribute("Kernel32.dll")]
        public static extern void SetLocalTime(SystemTime st);

        // 读取配置文件的值
        public static string ProfileReadValue(string section, string key, string fileName)
        {
            string encodingName = "utf-8";
            int size = 1024;
            byte[] buffer = new byte[size];
            int count = GetPrivateProfileString(getBytes(section, encodingName), getBytes(key, encodingName), getBytes("", encodingName), buffer, size, fileName);
            return Encoding.GetEncoding(encodingName).GetString(buffer, 0, count).Trim();

        }
        // 向配置文件写入值
        public static bool ProfileWriteValue(string section, string key, string value, string fileName)
        {
            string encodingName = "utf-8";
            return WritePrivateProfileString(getBytes(section, encodingName), getBytes(key, encodingName), getBytes(value, encodingName), fileName);
        }

        private static byte[] getBytes(string s, string encodingName)
        {
            return null == s ? null : Encoding.GetEncoding(encodingName).GetBytes(s);
        }


        [StructLayoutAttribute(LayoutKind.Sequential)]
        public class SystemTime
        {
            public ushort vYear;
            public ushort vMonth;
            public ushort vDayOfWeek;
            public ushort vDay;
            public ushort vHour;
            public ushort vMinute;
            public ushort vSecond;
        }
        internal const uint GW_OWNER = 4;
        public delegate bool WNDENUMPROC(IntPtr hwnd, uint lParam);
        internal delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern int GetWindowThreadProcessId(IntPtr hWnd, out IntPtr lpdwProcessId);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll", EntryPoint = "EnumWindows", SetLastError = true)]

        public static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, uint lParam);

        [DllImport("user32.dll", EntryPoint = "GetParent", SetLastError = true)]

        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "GetWindowThreadProcessId")]

        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref uint lpdwProcessId);

        [DllImport("user32.dll", EntryPoint = "IsWindow")]

        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]

        public static extern void SetLastError(uint dwErrCode);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]

        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        public static IntPtr GetMainWindowHandle(int processId)
        {
            IntPtr MainWindowHandle = IntPtr.Zero;

            EnumWindows(new EnumWindowsProc((hWnd, lParam) =>
            {
                IntPtr PID;
                GetWindowThreadProcessId(hWnd, out PID);

                if (PID == lParam &&
                   IsWindowVisible(hWnd) &&
                    GetWindow(hWnd, GW_OWNER) == IntPtr.Zero)
                {
                    MainWindowHandle = hWnd;
                    return false;
                }

                return true;

            }), new IntPtr(processId));

            return MainWindowHandle;
        }
        public static void CloseWindow(string name)
        {
            const int WM_CLOSE = 0x0010;
            IntPtr ptrWnd = IntPtr.Zero;
            var processes = Process.GetProcessesByName(name);
            foreach (Process proc in processes)
            {
                ptrWnd = GetMainWindowHandle(proc.Id);
                SendMessage(ptrWnd, WM_CLOSE, 0, 0);
            }

        }
        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);
        public static void ShowWindow(uint i)
        {
            IntPtr trayHwnd = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "SunAwtFrame", null);
            if (trayHwnd != IntPtr.Zero)
            {
                ShowWindow(trayHwnd, i);
            }
        }
        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        //定义钩子句柄
        public static int hHook = 0;
        //定义钩子类型
        public const int WH_MOUSE_LL = 14;
        //安装钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);
        //卸载钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);
        //调用下一个钩子
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);
        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }
        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public POINT pt;
            public int hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }
        public static void KillP(string pName)
        {
            try
            {
                Process cur = Process.GetCurrentProcess();

                Process[] processes = Process.GetProcesses();
                for (int number = 0; number < processes.Length; number++)
                {
                    if (processes[number].ProcessName.Contains(pName) && processes[number].Id != cur.Id)
                    {
                        processes[number].Kill();
                    }

                }
            }
            catch
            {

            }

        }
     
    }
}

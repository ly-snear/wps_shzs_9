using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using CalligraphyAssistantMain;
using CalligraphyAssistantMain.Code;
using CefSharp;
using Prism.Ioc;
using Prism.Unity;

namespace CalligraphyAssistant
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : PrismApplication
    {        
        private static Mutex mutex;
        private static bool createdNew = false;
        protected override Window CreateShell()
        {
            mutex = new Mutex(true, "艺学宝", out createdNew);
            if (!createdNew)
            {
                IntPtr hWnd = WinAPI.SearchForWindow("艺学宝主界面");
                if (hWnd != IntPtr.Zero)
                {
                    string message = "Show";
                    WinAPI.SendCustomMessage(hWnd, message);
                }
                else
                {
                    hWnd = WinAPI.SearchForWindow("艺学宝登录");
                    if (hWnd != IntPtr.Zero)
                    {
                        string message = "Show";
                        WinAPI.SendCustomMessage(hWnd, message);
                    }
                }
                Process.GetCurrentProcess().Kill(); 
            }
            Common.Init();
            InitializeCefSharp();
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            //return Container.Resolve<TestWindow>();
            return Container.Resolve<MainWindow>();
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void InitializeCefSharp()
        {
            var settings = new CefSettings();
            settings.Locale = "zh-CN";
            settings.AcceptLanguageList = "zh-CN";
            settings.LogSeverity = LogSeverity.Disable;
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            CefSharpSettings.WcfEnabled = true;
            //settings.CefCommandLineArgs["enable-system-flash"] = "1";
            //settings.CefCommandLineArgs.Add("ppapi-flash-version", "33.0.0.432");
            //settings.CefCommandLineArgs.Add("ppapi-flash-path", @"plugins\pepflashplayer32_33_0_0_432.dll");
            // Set BrowserSubProcessPath based on app bitness at runtime
            settings.BrowserSubprocessPath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                                                   "CefSharp.BrowserSubprocess.exe");

            // Make sure you set performDependencyCheck false
            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
             
        }
        //private void Application_Startup(object sender, StartupEventArgs e)
        //{
        //    mutex = new Mutex(true, "艺学宝", out createdNew);
        //    if (!createdNew)
        //    {
        //        IntPtr hWnd = WinAPI.SearchForWindow("艺学宝主界面");
        //        if (hWnd != IntPtr.Zero)
        //        {
        //            string message = "Show";
        //            WinAPI.SendCustomMessage(hWnd, message);
        //        }
        //        else
        //        {
        //            hWnd = WinAPI.SearchForWindow("艺学宝登录");
        //            if (hWnd != IntPtr.Zero)
        //            {
        //                string message = "Show";
        //                WinAPI.SendCustomMessage(hWnd, message);
        //            }
        //        }
        //        Process.GetCurrentProcess().Kill();
        //        return;
        //    }

        //    Common.Init();
        //    Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
        //    MainWindow mainWindow = new MainWindow();
        //    mainWindow.Show();
        //}

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Common.Trace("App UnhandledException Error：" + e.Exception.ToString());
            Console.WriteLine("【------------------------】");
            Console.WriteLine("【App.xaml.cs Error 001】" + e.ToString());
            Console.WriteLine("【------------------------】");
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            RegisterEvents();
            base.OnStartup(e);
        }
        private void RegisterEvents()
        {
            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;//Task异常 

            //UI线程未捕获异常处理事件（UI主线程）
            DispatcherUnhandledException += App_DispatcherUnhandledException;

            //非UI线程未捕获异常处理事件(例如自己创建的一个子线程)
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        //Task线程内未捕获异常处理事件
        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            try
            {
                var exception = e.Exception as Exception;
                if (exception != null)
                {
                    HandleException(exception);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                e.SetObserved();
            }
        }

        //非UI线程未捕获异常处理事件(例如自己创建的一个子线程)      
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var exception = e.ExceptionObject as Exception;
                if (exception != null)
                {
                    HandleException(exception);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                //ignore
            }
        }

        //UI线程未捕获异常处理事件（UI主线程）
        private static void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            
            try
            {
                HandleException(e.Exception);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                e.Handled = true;
            }
            
        }

        //日志记录
        private static void HandleException(Exception ex)
        {
            //MessageBox.Show("出错了，请与开发人员联系：" + ex.ToJson());
            //记录日志
            //CalligraphyAssistantMain.Code.Common.Trace("未捕获异常：" + ex.Message);
            Console.WriteLine("【------------------------】");
            Console.WriteLine("【App.xaml.cs Error 002】" + ex.ToString());
            Console.WriteLine("【------------------------】");
        }
    }
}

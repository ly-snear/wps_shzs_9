﻿#pragma checksum "..\..\..\Controls\BeginClassControl - 复制.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "1F4CFAA855CE49F813252B1E8D791E7FA41B7DEA6A28C49B9A49C8E5683B1EC3"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using CalligraphyAssistantMain.Controls;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace CalligraphyAssistantMain.Controls {
    
    
    /// <summary>
    /// BeginClassControl
    /// </summary>
    public partial class BeginClassControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\..\Controls\BeginClassControl - 复制.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border classBtn;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\Controls\BeginClassControl - 复制.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock classLb;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\..\Controls\BeginClassControl - 复制.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Primitives.Popup classPop;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\Controls\BeginClassControl - 复制.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox classListBox;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\Controls\BeginClassControl - 复制.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image okBtn;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\..\Controls\BeginClassControl - 复制.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image cancelBtn;
        
        #line default
        #line hidden
        
        
        #line 85 "..\..\..\Controls\BeginClassControl - 复制.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal CalligraphyAssistantMain.Controls.ImageEditControl imageEditControl;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/CalligraphyAssistantMain;component/controls/beginclasscontrol%20-%20%e5%a4%8d%e5" +
                    "%88%b6.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Controls\BeginClassControl - 复制.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 8 "..\..\..\Controls\BeginClassControl - 复制.xaml"
            ((CalligraphyAssistantMain.Controls.BeginClassControl)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.classBtn = ((System.Windows.Controls.Border)(target));
            
            #line 24 "..\..\..\Controls\BeginClassControl - 复制.xaml"
            this.classBtn.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.classBtn_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 3:
            this.classLb = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.classPop = ((System.Windows.Controls.Primitives.Popup)(target));
            return;
            case 5:
            this.classListBox = ((System.Windows.Controls.ListBox)(target));
            
            #line 34 "..\..\..\Controls\BeginClassControl - 复制.xaml"
            this.classListBox.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.ListBox_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            this.okBtn = ((System.Windows.Controls.Image)(target));
            
            #line 45 "..\..\..\Controls\BeginClassControl - 复制.xaml"
            this.okBtn.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.okBtn_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 46 "..\..\..\Controls\BeginClassControl - 复制.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.okBtn_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 8:
            this.cancelBtn = ((System.Windows.Controls.Image)(target));
            
            #line 49 "..\..\..\Controls\BeginClassControl - 复制.xaml"
            this.cancelBtn.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.cancelBtn_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 50 "..\..\..\Controls\BeginClassControl - 复制.xaml"
            ((System.Windows.Controls.TextBlock)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.cancelBtn_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 10:
            this.imageEditControl = ((CalligraphyAssistantMain.Controls.ImageEditControl)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}


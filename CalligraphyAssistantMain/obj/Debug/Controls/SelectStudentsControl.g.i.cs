﻿#pragma checksum "..\..\..\Controls\SelectStudentsControl.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "867BB29BD3881E1834F270A4111C9529D2827E0ED136851ADB654E244D976F4B"
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
    /// SelectStudentsControl
    /// </summary>
    public partial class SelectStudentsControl : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 75 "..\..\..\Controls\SelectStudentsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid closeBtn;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\..\Controls\SelectStudentsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton selectAllBtn;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\..\Controls\SelectStudentsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton invertBtn;
        
        #line default
        #line hidden
        
        
        #line 89 "..\..\..\Controls\SelectStudentsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton uncheckBtn;
        
        #line default
        #line hidden
        
        
        #line 122 "..\..\..\Controls\SelectStudentsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock selectCount;
        
        #line default
        #line hidden
        
        
        #line 125 "..\..\..\Controls\SelectStudentsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border btn_cancel;
        
        #line default
        #line hidden
        
        
        #line 128 "..\..\..\Controls\SelectStudentsControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border btn_ok;
        
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
            System.Uri resourceLocater = new System.Uri("/CalligraphyAssistantMain;component/controls/selectstudentscontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Controls\SelectStudentsControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
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
            this.closeBtn = ((System.Windows.Controls.Grid)(target));
            
            #line 75 "..\..\..\Controls\SelectStudentsControl.xaml"
            this.closeBtn.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.closeBtn_PreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.selectAllBtn = ((System.Windows.Controls.RadioButton)(target));
            
            #line 87 "..\..\..\Controls\SelectStudentsControl.xaml"
            this.selectAllBtn.Checked += new System.Windows.RoutedEventHandler(this.selectAllBtn_Checked);
            
            #line default
            #line hidden
            return;
            case 3:
            this.invertBtn = ((System.Windows.Controls.RadioButton)(target));
            
            #line 88 "..\..\..\Controls\SelectStudentsControl.xaml"
            this.invertBtn.Checked += new System.Windows.RoutedEventHandler(this.invertBtn_Checked);
            
            #line default
            #line hidden
            return;
            case 4:
            this.uncheckBtn = ((System.Windows.Controls.RadioButton)(target));
            
            #line 89 "..\..\..\Controls\SelectStudentsControl.xaml"
            this.uncheckBtn.Checked += new System.Windows.RoutedEventHandler(this.uncheckBtn_Checked);
            
            #line default
            #line hidden
            return;
            case 5:
            this.selectCount = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.btn_cancel = ((System.Windows.Controls.Border)(target));
            
            #line 125 "..\..\..\Controls\SelectStudentsControl.xaml"
            this.btn_cancel.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.btn_cancel_PreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btn_ok = ((System.Windows.Controls.Border)(target));
            
            #line 128 "..\..\..\Controls\SelectStudentsControl.xaml"
            this.btn_ok.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.btn_ok_PreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


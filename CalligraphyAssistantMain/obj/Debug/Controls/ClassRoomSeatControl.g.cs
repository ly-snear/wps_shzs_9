﻿#pragma checksum "..\..\..\Controls\ClassRoomSeatControl.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "5FF26E7C702EC32ED6CCFFD31C6AA15773BD9234D8091B0168B65122AC03D213"
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
    /// ClassRoomSeatControl
    /// </summary>
    public partial class ClassRoomSeatControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 10 "..\..\..\Controls\ClassRoomSeatControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border borderBd;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\Controls\ClassRoomSeatControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid borderGd;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\Controls\ClassRoomSeatControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock className;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\Controls\ClassRoomSeatControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid seats;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\Controls\ClassRoomSeatControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid cancelGd;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\Controls\ClassRoomSeatControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image cancelBtn;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\Controls\ClassRoomSeatControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock textLb2;
        
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
            System.Uri resourceLocater = new System.Uri("/CalligraphyAssistantMain;component/controls/classroomseatcontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Controls\ClassRoomSeatControl.xaml"
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
            this.borderBd = ((System.Windows.Controls.Border)(target));
            return;
            case 2:
            this.borderGd = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.className = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.seats = ((System.Windows.Controls.Grid)(target));
            return;
            case 5:
            this.cancelGd = ((System.Windows.Controls.Grid)(target));
            
            #line 26 "..\..\..\Controls\ClassRoomSeatControl.xaml"
            this.cancelGd.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.cancelGd_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.cancelBtn = ((System.Windows.Controls.Image)(target));
            return;
            case 7:
            this.textLb2 = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

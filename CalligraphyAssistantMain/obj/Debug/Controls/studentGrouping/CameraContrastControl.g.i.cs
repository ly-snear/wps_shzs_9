﻿#pragma checksum "..\..\..\..\Controls\studentGrouping\CameraContrastControl.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "CB1BE4377D30457008E04B7C8199FE1AB557F2075CB6EF2E9AEA0014D18679F6"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using CalligraphyAssistantMain.Code;
using CalligraphyAssistantMain.Controls;
using CalligraphyAssistantMain.Controls.studentGrouping;
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


namespace CalligraphyAssistantMain.Controls.studentGrouping {
    
    
    /// <summary>
    /// CameraContrastControl
    /// </summary>
    public partial class CameraContrastControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 34 "..\..\..\..\Controls\studentGrouping\CameraContrastControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ItemsControl view;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\..\Controls\studentGrouping\CameraContrastControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border returnBtn;
        
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
            System.Uri resourceLocater = new System.Uri("/CalligraphyAssistantMain;component/controls/studentgrouping/cameracontrastcontro" +
                    "l.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Controls\studentGrouping\CameraContrastControl.xaml"
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
            this.view = ((System.Windows.Controls.ItemsControl)(target));
            return;
            case 2:
            this.returnBtn = ((System.Windows.Controls.Border)(target));
            
            #line 59 "..\..\..\..\Controls\studentGrouping\CameraContrastControl.xaml"
            this.returnBtn.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.ReturnBtn_PreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


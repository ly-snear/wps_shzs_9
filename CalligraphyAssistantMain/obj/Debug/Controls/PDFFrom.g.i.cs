﻿#pragma checksum "..\..\..\Controls\PDFFrom.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "3E6E88D9CD161521C430976CCCCE17751C5E1E97C62EC7B196476376D16E9FA9"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using MoonPdfLib;
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
    /// PDFFrom
    /// </summary>
    public partial class PDFFrom : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 41 "..\..\..\Controls\PDFFrom.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MoonPdfLib.MoonPdfPanel moonPdfPanel;
        
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
            System.Uri resourceLocater = new System.Uri("/CalligraphyAssistantMain;component/controls/pdffrom.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Controls\PDFFrom.xaml"
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
            
            #line 8 "..\..\..\Controls\PDFFrom.xaml"
            ((CalligraphyAssistantMain.Controls.PDFFrom)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            
            #line 24 "..\..\..\Controls\PDFFrom.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ZoomInButton_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            
            #line 26 "..\..\..\Controls\PDFFrom.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ZoomOutButton_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            
            #line 28 "..\..\..\Controls\PDFFrom.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.NormalButton_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 30 "..\..\..\Controls\PDFFrom.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.TwoNormalButton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 32 "..\..\..\Controls\PDFFrom.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.FitToHeightButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 34 "..\..\..\Controls\PDFFrom.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SinglePageButton_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 36 "..\..\..\Controls\PDFFrom.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.FacingButton_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.moonPdfPanel = ((MoonPdfLib.MoonPdfPanel)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

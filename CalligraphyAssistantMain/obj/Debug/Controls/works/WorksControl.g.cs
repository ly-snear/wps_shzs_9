﻿#pragma checksum "..\..\..\..\Controls\works\WorksControl.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "8A49B2160860D89A0A00A56145263E4499008AFD9A9932B0FF1E3C4E24F86CC9"
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
using CalligraphyAssistantMain.Controls.works;
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


namespace CalligraphyAssistantMain.Controls.works {
    
    
    /// <summary>
    /// WorksControl
    /// </summary>
    public partial class WorksControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 31 "..\..\..\..\Controls\works\WorksControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock classLb;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\..\Controls\works\WorksControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal CalligraphyAssistantMain.Controls.works.StudentWorksControl studentWorksControl;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\..\..\Controls\works\WorksControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal CalligraphyAssistantMain.Controls.works.TeacherWorksControl teacherWorksControl;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\..\Controls\works\WorksControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border returnBtn;
        
        #line default
        #line hidden
        
        
        #line 58 "..\..\..\..\Controls\works\WorksControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal CalligraphyAssistantMain.Controls.ImageEditControl imageEditControl;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\..\Controls\works\WorksControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal CalligraphyAssistantMain.Controls.ImageViewControl imageViewControl;
        
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
            System.Uri resourceLocater = new System.Uri("/CalligraphyAssistantMain;component/controls/works/workscontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Controls\works\WorksControl.xaml"
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
            this.classLb = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 2:
            this.studentWorksControl = ((CalligraphyAssistantMain.Controls.works.StudentWorksControl)(target));
            return;
            case 3:
            this.teacherWorksControl = ((CalligraphyAssistantMain.Controls.works.TeacherWorksControl)(target));
            return;
            case 4:
            this.returnBtn = ((System.Windows.Controls.Border)(target));
            
            #line 51 "..\..\..\..\Controls\works\WorksControl.xaml"
            this.returnBtn.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.ReturnBtn_PreviewMouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.imageEditControl = ((CalligraphyAssistantMain.Controls.ImageEditControl)(target));
            return;
            case 6:
            this.imageViewControl = ((CalligraphyAssistantMain.Controls.ImageViewControl)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

﻿#pragma checksum "..\..\..\..\HFL\View\Account.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1CF3C239D0293FED38116B737946F905"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
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


namespace HandsFreeLeveler {
    
    
    /// <summary>
    /// Account
    /// </summary>
    public partial class Account : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 6 "..\..\..\..\HFL\View\Account.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label username;
        
        #line default
        #line hidden
        
        
        #line 7 "..\..\..\..\HFL\View\Account.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label password;
        
        #line default
        #line hidden
        
        
        #line 8 "..\..\..\..\HFL\View\Account.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label AccountType;
        
        #line default
        #line hidden
        
        
        #line 9 "..\..\..\..\HFL\View\Account.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button upButton;
        
        #line default
        #line hidden
        
        
        #line 10 "..\..\..\..\HFL\View\Account.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button singleButton;
        
        #line default
        #line hidden
        
        
        #line 11 "..\..\..\..\HFL\View\Account.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button multiButton;
        
        #line default
        #line hidden
        
        
        #line 12 "..\..\..\..\HFL\View\Account.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Trial;
        
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
            System.Uri resourceLocater = new System.Uri("/HandsFreeLeveler;component/hfl/view/account.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\HFL\View\Account.xaml"
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
            this.username = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.password = ((System.Windows.Controls.Label)(target));
            return;
            case 3:
            this.AccountType = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.upButton = ((System.Windows.Controls.Button)(target));
            
            #line 9 "..\..\..\..\HFL\View\Account.xaml"
            this.upButton.Click += new System.Windows.RoutedEventHandler(this.upButtonClick);
            
            #line default
            #line hidden
            return;
            case 5:
            this.singleButton = ((System.Windows.Controls.Button)(target));
            
            #line 10 "..\..\..\..\HFL\View\Account.xaml"
            this.singleButton.Click += new System.Windows.RoutedEventHandler(this.singleButton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.multiButton = ((System.Windows.Controls.Button)(target));
            
            #line 11 "..\..\..\..\HFL\View\Account.xaml"
            this.multiButton.Click += new System.Windows.RoutedEventHandler(this.multiButton_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.Trial = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}


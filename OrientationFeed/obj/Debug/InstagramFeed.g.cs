﻿

#pragma checksum "C:\Visual Studio 2013\Projects\InstaFeed\OrientationFeed\InstagramFeed.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B4C1A6982DE241A53DE8301A565ED6F3"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OrientationFeed
{
    partial class InstagramFeed : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 135 "..\..\InstagramFeed.xaml"
                ((global::Windows.UI.Xaml.Controls.WebView)(target)).NavigationCompleted += this.AuthBrowser_NavigationCompleted;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 99 "..\..\InstagramFeed.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).PointerPressed += this.Image_PointerPressed;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 97 "..\..\InstagramFeed.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Button_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}



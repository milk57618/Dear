﻿#pragma checksum "C:\Users\myc\Desktop\SW_TEST\POS_UWP\POS_UWP\Views\POS_AS.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "247BFAA629FDD9AB21DCA2CAE35705C1"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace POS_UWP.Views
{
    partial class POS_AS : 
        global::Windows.UI.Xaml.Controls.Page, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.btn_Send = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 13 "..\..\..\Views\POS_AS.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btn_Send).Click += this.btn_Send_Click;
                    #line default
                }
                break;
            case 2:
                {
                    this.btn_Close = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 14 "..\..\..\Views\POS_AS.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btn_Close).Click += this.btn_Close_Click;
                    #line default
                }
                break;
            case 3:
                {
                    this.txtbox_Content = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                    #line 19 "..\..\..\Views\POS_AS.xaml"
                    ((global::Windows.UI.Xaml.Controls.TextBox)this.txtbox_Content).GotFocus += this.txtbox_Content_GotFocus;
                    #line default
                }
                break;
            case 4:
                {
                    this.tb_QA = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}

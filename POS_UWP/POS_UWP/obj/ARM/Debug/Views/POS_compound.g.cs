﻿#pragma checksum "C:\Users\myc\Desktop\SW_TEST\POS_UWP\POS_UWP\Views\POS_compound.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A1A2B074E48D9DF4D60E15013CCE289D"
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
    partial class POS_compound : 
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
                    this.btn_Payment = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 307 "..\..\..\Views\POS_compound.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btn_Payment).Click += this.btn_Payment_Click;
                    #line default
                }
                break;
            case 2:
                {
                    this.btn_Cash = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 308 "..\..\..\Views\POS_compound.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btn_Cash).Click += this.btn_Cash_Click;
                    #line default
                }
                break;
            case 3:
                {
                    this.btn_Card = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 309 "..\..\..\Views\POS_compound.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btn_Card).Click += this.btn_Card_Click;
                    #line default
                }
                break;
            case 4:
                {
                    this.btn_Close = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 310 "..\..\..\Views\POS_compound.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btn_Close).Click += this.btn_Close_Click;
                    #line default
                }
                break;
            case 5:
                {
                    this.tb_Cash = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 6:
                {
                    this.tb_Card = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 7:
                {
                    this.tb_CashPrice = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 8:
                {
                    this.tb_CardPrice = (global::Windows.UI.Xaml.Controls.TextBlock)(target);
                }
                break;
            case 9:
                {
                    this.lv_Card = (global::Windows.UI.Xaml.Controls.ListView)(target);
                }
                break;
            case 10:
                {
                    this.lv_Cash = (global::Windows.UI.Xaml.Controls.ListView)(target);
                }
                break;
            case 11:
                {
                    this.lv_Sale = (global::Windows.UI.Xaml.Controls.ListView)(target);
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


﻿#pragma checksum "C:\Users\myc\Desktop\SW_TEST\POS_UWP\POS_UWP\Views\POS_adminsetting.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "EA5E6D47F8288AAB7F4CDD67B8351226"
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
    partial class POS_adminsetting : 
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
                    this.txtbox_PhoneNumber = (global::Windows.UI.Xaml.Controls.Grid)(target);
                }
                break;
            case 2:
                {
                    this.btn_Close = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 130 "..\..\..\Views\POS_adminsetting.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btn_Close).Click += this.btn_Close_Click;
                    #line default
                }
                break;
            case 3:
                {
                    this.btn_Add = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 135 "..\..\..\Views\POS_adminsetting.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btn_Add).Click += this.btn_Add_Click;
                    #line default
                }
                break;
            case 4:
                {
                    this.btn_Modify = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 136 "..\..\..\Views\POS_adminsetting.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btn_Modify).Click += this.btn_Modify_Click;
                    #line default
                }
                break;
            case 5:
                {
                    this.btn_Delete = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 137 "..\..\..\Views\POS_adminsetting.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btn_Delete).Click += this.btn_Delete_Click;
                    #line default
                }
                break;
            case 6:
                {
                    this.btn_Commute = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 138 "..\..\..\Views\POS_adminsetting.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.btn_Commute).Click += this.btn_Commute_Click;
                    #line default
                }
                break;
            case 7:
                {
                    this.txtbox_Name = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 8:
                {
                    this.txtbox_Phonenumber = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 9:
                {
                    this.txtbox_Pay = (global::Windows.UI.Xaml.Controls.TextBox)(target);
                }
                break;
            case 10:
                {
                    this.cb_Position = (global::Windows.UI.Xaml.Controls.ComboBox)(target);
                    #line 147 "..\..\..\Views\POS_adminsetting.xaml"
                    ((global::Windows.UI.Xaml.Controls.ComboBox)this.cb_Position).SelectionChanged += this.cb_Position_SelectionChanged;
                    #line default
                }
                break;
            case 11:
                {
                    this.lv_member = (global::Windows.UI.Xaml.Controls.ListView)(target);
                    #line 15 "..\..\..\Views\POS_adminsetting.xaml"
                    ((global::Windows.UI.Xaml.Controls.ListView)this.lv_member).SelectionChanged += this.lv_member_SelectionChanged;
                    #line default
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

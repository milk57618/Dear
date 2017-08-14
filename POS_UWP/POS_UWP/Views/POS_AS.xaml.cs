﻿using System;
using System.Dynamic;
using System.Net.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.Generic;

namespace POS_UWP.Views
{
    public sealed partial class POS_AS : Page
    {
        public POS_AS()
        {
            this.InitializeComponent();
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
        
        /* 처음에 문의내용 칸을 클릭하면 "문의할 내용을 적어주세요." 문자열이 없어지도록 함 */
        private void txtbox_Content_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtbox_Content.Text.Equals("문의할 내용을 적어주세요."))
                txtbox_Content.Text = "";
        }
        
        private void btn_Send_Click(object sender, RoutedEventArgs e)
        {
            Web.sendAS(DateTime.Now.ToString("yy/MM/dd HH:mm:ss"), POS_main.PosId, txtbox_Content.Text);
            txtbox_Content.Text = "";
        }
    }
}

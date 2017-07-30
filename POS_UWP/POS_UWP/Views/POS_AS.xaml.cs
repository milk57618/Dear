using System;
using System.Dynamic;
using System.Net.Http;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using POS_UWP.DBConn;
using POS_UWP.PosDB;
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
            DBConn_Product dbp = new DBConn_Product();
            List<Product> productList = dbp.GetAllProduct();

            Uri requestUri = new Uri("http://localhost/actionTest.php");
            dynamic dynamicJson = new ExpandoObject();
            Random r = new Random();

            dynamicJson.product = productList;

            string json = "";
            json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);
            var objClint = new HttpClient();
            //HttpResponseMessage respon = objClint.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json")).Result;
        }
    }
}

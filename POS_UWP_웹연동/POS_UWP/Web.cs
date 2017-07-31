using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using POS_UWP.DBConn;
using POS_UWP.PosDB;
using POS_UWP.Views;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace POS_UWP
{
    public class Web
    {
        private const string ip = "125.183.218.149";

        public HttpResponseMessage login(string id, string pw)
        {
            Uri requestUri = new Uri("http://" + ip + "/POS_action_login.php");
            dynamic dynamicJson = new ExpandoObject();

            dynamicJson.id = id;
            dynamicJson.pw = pw;

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);
            var objClint = new HttpClient();

            return objClint.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json")).Result;
        }

        public void sendAllData()
        {
            sendProduct();
            sendSaleSearch();
            test();
        }

        private async void test()
        {
            Uri requestUri = new Uri("http://" + ip + "/showProduct.php");
            dynamic dynamicJson = new ExpandoObject();

            dynamicJson.PosId = POS_main.PosId;

            string json = "";
            json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);

            var objClint = new HttpClient();
            HttpResponseMessage respon = objClint.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json")).Result;

            String strRespon = await respon.Content.ReadAsStringAsync();
            int b = 1;
        }

        private async void sendProduct()
        {
            DBConn_Product dbp = new DBConn_Product();
            List<Product> productList = dbp.GetAllProduct();

            Uri requestUri = new Uri("http://" + ip + "/action_refreshProduct.php");
            dynamic dynamicJson = new ExpandoObject();

            dynamicJson.Product = productList;

            string json = "";
            json = "{\"PosId\":\"" + POS_main.PosId + "\"," + Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson).Substring(1);
            
            var objClint = new HttpClient();
            HttpResponseMessage respon = objClint.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json")).Result;

            String strRespon = await respon.Content.ReadAsStringAsync();

            if (!strRespon.Equals("1"))
            {
                MessageDialog messageDialog = new MessageDialog("상품들의 정보가 정상적으로 전송되지 않았습니다.");
                await messageDialog.ShowAsync();
            }
        }

        private async void sendSaleSearch()
        {
            DBConn_SaleSearch dbs = new DBConn_SaleSearch();
            List<SaleSearch> saleSearchList = dbs.ReadAllSaleSearch();

            Uri requestUri = new Uri("http://" + ip + "/action_refreshSaleSearch.php");
            dynamic dynamicJson = new ExpandoObject();

            dynamicJson.SaleSearch = saleSearchList;

            string json = "";
            json = "{\"PosId\":\"" + POS_main.PosId + "\"," + Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson).Substring(1);

            var objClint = new HttpClient();
            HttpResponseMessage respon = objClint.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json")).Result;

            String strRespon = await respon.Content.ReadAsStringAsync();

            if (!strRespon.Equals("1"))
            {
                MessageDialog messageDialog = new MessageDialog("판매기록이 정상적으로 전송되지 않았습니다.");
                await messageDialog.ShowAsync();
            }
        }


        public async void sendAS(string Time, int PosId, string Text)
        {
            Uri requestUri = new Uri("http://" + ip + "/action_getAS.php");
            dynamic dynamicJson = new ExpandoObject();

            dynamicJson.Time = Time;
            dynamicJson.PosId = PosId;
            dynamicJson.Text = Text;

            string json = "";
            json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);

            var objClint = new HttpClient();
            HttpResponseMessage respon = objClint.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json")).Result;

            String strRespon = await respon.Content.ReadAsStringAsync();

            if (strRespon.Equals("1"))
            {
                MessageDialog messageDialog = new MessageDialog("문의내용이 정상적으로 전송되었습니다.");
                await messageDialog.ShowAsync();
            }
            else
            {
                MessageDialog messageDialog = new MessageDialog("문의내용이 정상적으로 전송되지 않았습니다.");
                await messageDialog.ShowAsync();
            }
        }
    }
}

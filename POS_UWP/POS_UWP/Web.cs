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
        private const string ip = "172.20.10.6";

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

        public void sendAll()
        {
            sendData();
            test();
        }

        private async void test()
        {
            Uri requestUri = new Uri("http://" + ip + "/showMember.php");
            dynamic dynamicJson = new ExpandoObject();

            dynamicJson.PosId = POS_main.PosId;
            dynamicJson.Date = "2017-7";

            string json = "";
            json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);
            int c = 1;

            var objClint = new HttpClient();
            HttpResponseMessage respon = objClint.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json")).Result;

            String strRespon = await respon.Content.ReadAsStringAsync();
            int b = 1;
        }

        private async void sendData()
        {
            DBConn_Product dbp = new DBConn_Product();
            DBConn_SaleSearch dbs = new DBConn_SaleSearch();
            DBConn_Member dbm = new DBConn_Member();
            DBConn_MemberTime dbmt = new DBConn_MemberTime();
            List<Product> productList = dbp.GetAllProduct();
            List<SaleSearch> saleSearchList = dbs.ReadAllSaleSearch();
            List<Member> memberList = dbm.GetAllMember();
            List<MemberTime> memberTimeList = dbmt.GetAllMemberTime();

            Uri requestUri = new Uri("http://" + ip + "/action_refreshAll.php");
            dynamic dynamicJson = new ExpandoObject();

            dynamicJson.Product = productList;
            dynamicJson.SaleSearch = saleSearchList;
            dynamicJson.Member = memberList;
            dynamicJson.MemberTime = memberTimeList;

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
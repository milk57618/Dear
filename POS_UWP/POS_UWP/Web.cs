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
using Newtonsoft.Json;

namespace POS_UWP
{
    public class Web
    {
        /* 웹서버 IP 주소 */
        private const string ip = "125.183.218.149";

        private static DBConn_Category dbc = new DBConn_Category();
        private static DBConn_Product dbp = new DBConn_Product();
        private static DBConn_Member dbm = new DBConn_Member();
        private static DBConn_MemberTime dbmt = new DBConn_MemberTime();
        private static DBConn_SaleSearch dbss = new DBConn_SaleSearch();
        private static DBConn_SaleHistory dbsh = new DBConn_SaleHistory();

        public static HttpResponseMessage login(string id, string pw)
        {
            Uri requestUri = new Uri("http://" + ip + "/POS_action_login.php");
            dynamic dynamicJson = new ExpandoObject();

            dynamicJson.id = id;
            dynamicJson.pw = pw;

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);
            var objClint = new HttpClient();

            return objClint.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json")).Result;
        }

        /// <summary>
        /// 웹서버로부터 받은 json 문자열을 가공하기 위한 클래스
        /// </summary>
        private class AllDB
        {
            [JsonProperty("Category")]
            public Categorys[] Category { get; set; }
            [JsonProperty("Product")]
            public Product[] Product { get; set; }
            [JsonProperty("Member")]
            public Member[] Member { get; set; }
            [JsonProperty("MemberTime")]
            public MemberTime[] MemberTime { get; set; }
            [JsonProperty("SaleSearch")]
            public SaleSearch[] SaleSearch { get; set; }
            [JsonProperty("SaleHistory")]
            public SaleHistory[] SaleHistory { get; set; }
        }

        /// <summary>
        /// 로그인된 포스기의 정보를 웹서버로부터 받아온 후 현재 sqlite내부의 모든 db정보를 웹서버의 정보로 갱신함
        /// </summary>
        public static async void receiveDataFromServer()
        {
            Uri requestUri = new Uri("http://" + ip + "/action_sendDataToPos.php");
            dynamic dynamicJson = new ExpandoObject();

            string json = "";
<<<<<<< HEAD
            json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);
            
=======
            json = "{\"PosId\":\"" + POS_main.PosId + "\"}";
>>>>>>> dea8bf4ef8e4db1269924dbf168cb49b27d53370

            var objClint = new HttpClient();
            HttpResponseMessage respon = objClint.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json")).Result;

            String strRespon = await respon.Content.ReadAsStringAsync();
<<<<<<< HEAD
            
=======

            if(strRespon.IndexOf("{\"Category\":") == 0) // 웹서버로부터 데이터를 정상적으로 불러왔을 경우
            {
                AllDB all = JsonConvert.DeserializeObject<AllDB>(strRespon); // json 문자열을 객체로 가공함

                dbc.DeleteAllCategory();
                dbc.InsertCategoryArray(all.Category);
                
                dbp.DeleteAllProduct();
                dbp.InsertProductArray(all.Product);

                dbm.DeleteAllMember();
                dbm.InsertMemberArray(all.Member);

                dbmt.DeleteAllMemberTime();
                dbmt.InsertMemberTimeArray(all.MemberTime);

                dbss.DeleteAllSaleSearch();
                dbss.InsertSaleSearchArray(all.SaleSearch);

                dbsh.DeleteAllSaleHistory();
                dbsh.InsertSaleHistoryArray(all.SaleHistory);
            }
>>>>>>> dea8bf4ef8e4db1269924dbf168cb49b27d53370
        }

        /// <summary>
        /// 현재 로그인된 포스기의 정보를 웹서버에 전송함
        /// </summary>
        public static async void sendDataToServer()
        {
            List<Categorys> categoryList = dbc.GetAllCategory();
            List<Product> productList = dbp.GetAllProduct();
            List<Member> memberList = dbm.GetAllMember();
            List<MemberTime> memberTimeList = dbmt.GetAllMemberTime();
            List<SaleSearch> saleSearchList = dbss.GetAllSaleSearch();
            List<SaleHistory> saleHistoryList = dbsh.GetAllSaleHistory();

            Uri requestUri = new Uri("http://" + ip + "/action_getDataFromPos.php");
            dynamic dynamicJson = new ExpandoObject();

            dynamicJson.Category = categoryList;
            dynamicJson.Product = productList;
            dynamicJson.Member = memberList;
            dynamicJson.MemberTime = memberTimeList;
            dynamicJson.SaleSearch = saleSearchList;
            dynamicJson.SaleHistory = saleHistoryList;

            string json = "";
            json = "{\"PosId\":\"" + POS_main.PosId + "\"," + Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson).Substring(1);

            var objClint = new HttpClient();
            HttpResponseMessage respon = objClint.PostAsync(requestUri, new StringContent(json, System.Text.Encoding.UTF8, "application/json")).Result;

            String strRespon = await respon.Content.ReadAsStringAsync();

            if (!strRespon.Equals("1"))
            {
                MessageDialog messageDialog = new MessageDialog("웹서버에 데이터를 전송하지 못하였습니다.");
                await messageDialog.ShowAsync();
            }
        }

        public static async void sendAS(string Time, int PosId, string Text)
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

            /*
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
            */
        }
    }
}
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using POS_UWP.DBConn;
using POS_UWP.PosDB;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Windows.UI.Xaml.Input;
using System.Threading.Tasks;

namespace POS_UWP.Views
{

    public sealed partial class POS_login : Page
    {
        public POS_login()
        {
            this.InitializeComponent();
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        // 로그인 응답값을 Json 형태로 받은 후 객체로 처리하기 위한 클래스
        private class Login_Response
        {
            public int PosId { get; set; }
            public string StoreName { get; set; }
        }

        private async void login()
        {
            if (txtbox_id.Text == "")
            {
                MessageDialog messageDialog = new MessageDialog("아이디를 입력해주세요.");
                await messageDialog.ShowAsync();
                return;
            }
            else if (pb_pw.Password == "")
            {
                MessageDialog messageDialog = new MessageDialog("비밀번호를 입력해주세요.");
                await messageDialog.ShowAsync();
                return;
            }
            HttpResponseMessage respon = Web.login(txtbox_id.Text, pb_pw.Password);

            /* 웹서버 컴퓨터의 mysql이 연결이 안되면 -2, 존재하지 않는 계정이면 -1, 존재하는 계정이면 그 계정의 PosId */
            String strRespon = await respon.Content.ReadAsStringAsync();
            Login_Response objRespon = JsonConvert.DeserializeObject<Login_Response>(strRespon);
            int PosId = objRespon.PosId;

            /* 아이디, 비밀번호 확인 */
            if (PosId == -1)
            {
                MessageDialog messageDialog = new MessageDialog("일치하는 아이디나 비밀번호가 존재하지 않습니다.");
                await messageDialog.ShowAsync();
            }
            else if (PosId == -2)
            {
                MessageDialog messageDialog = new MessageDialog("에러 발생!");
                await messageDialog.ShowAsync();
            }
            else
            {
                txtbox_id.Text = "";
                pb_pw.Password = "";
                POS_main.PosId = PosId;
                Task t = new Task(() => {
                    Web.receiveDataFromServer(); // 로그인 성공시 웹에서 DB정보들 받아옴
                });
                t.Start();

                Frame.Navigate(typeof(POS_starting));
            }
        }

        private void login_keyEvent(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                login();
            }
        }

        private void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            login();
        }
    }
}

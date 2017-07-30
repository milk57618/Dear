using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace POS_UWP.Views
{

    public sealed partial class POS_login : Page
    {
        /*임시 할당한 아이디, 비밀번호*/
        static string RootID = "test";
        static string RootPW = "1234";

        public POS_login()
        {
            this.InitializeComponent();
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            /*아이디, 비밀번호 확인*/
            if (txtbox_id.Text == RootID && pb_pw.Password == RootPW)
            {
                txtbox_id.Text = "";
                pb_pw.Password = "";

                Frame.Navigate(typeof(POS_starting));
            }
            else
            {
                MessageDialog messageDialog = new MessageDialog("일치하는 아이디나 비밀번호가 존재하지 않습니다.");
                await messageDialog.ShowAsync();
            }
        }

        private void pb_pw_PasswordChanged(System.Object sender, RoutedEventArgs e)
        {

        }
    }
}

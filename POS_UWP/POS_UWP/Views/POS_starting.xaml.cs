using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace POS_UWP.Views
{
    public sealed partial class POS_starting : Page
    {
        public static bool openCheck = false;
        public static string startPrice;

        public POS_starting()
        {
            this.InitializeComponent();

            txtbox_StartPrice.Text = "0";
            txtbox_YesterdayPrice.Text = "0";
        }

        private async void btn_OK_Click(object sender, RoutedEventArgs e)
        {
            //준비금 설정
            startPrice = txtbox_StartPrice.Text;

            if (txtbox_StartPrice.Text != null && txtbox_YesterdayPrice.Text != null)
            {
                // 스레드를 사용하여 5분마다 웹서버로 DB정보 전송
                Random r = new Random();
                var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(10) };
                timer.Start();
                timer.Tick += (sender2, args) =>
                {
                    Web.sendDataToServer();
                };

                openCheck = true;
                Frame.Navigate(typeof(POS_main));
            }
            else
            {
                MessageDialog md = new MessageDialog("준비금을 입력해주세요");
                await md.ShowAsync();
            }
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}

using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace POS_UWP.Views
{
    public sealed partial class POS_main : Page
    {
        public static string managerName = "";
        public static int managerID = 0;
        public static int PosId = -1;
        public static string strPosId = "";

        DispatcherTimer Timer = new DispatcherTimer();

        public POS_main()
        {
            this.InitializeComponent();
            DataContext = this;

            /*현재시간 나타내기*/
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 0);
            Timer.Start();

            tb_Manager1.Text = managerName;

            /*개점 시 버튼 활성화*/
            if (POS_starting.openCheck == false)
            {
                btn_ChangeManager.IsEnabled = false;
                btn_Manage.IsEnabled = false;
                btn_Setting.IsEnabled = false;
                btn_Sale.IsEnabled = false;
                btn_ChangeManager.IsEnabled = false;
                btn_Finishing.IsEnabled = false;
                btn_SaleState.IsEnabled = false;
            }
            else
            {
                btn_Login.IsEnabled = false;
            }

            if (PosId != -1)
            {
                strPosId = setFiveLength(PosId);
                tb_PosNum.Text = strPosId;
                int b = 1;
            }
        }
        private string setFiveLength(int number)
        {
            string str = number.ToString();
            while(str.Length < 5)
            {
                str = "0" + str;
            }
            return str;
        }
        private void Timer_Tick(object sender, object e)
        {
            tb_Time.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }
        private async void btn_Sale_Click(object sender, RoutedEventArgs e)
        {
            if (managerName != "")
            {
                Frame.Navigate(typeof(POS_sale));
            }
            else
            {
                MessageDialog md = new MessageDialog("관리자를 설정하세요");
                await md.ShowAsync();
            }
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(POS_login));
        }

        private void btn_Setting_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(POS_setting));
        }

        private void btn_ChangeManager_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(POS_adminsetting));
        }

        private void btn_Manage_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(POS_manager));
        }

        private void btn_Finishing_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(POS_finishing));
        }

        private void btn_AS_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(POS_AS));
        }

        private void btn_SaleState_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(POS_salestatus));
        }

        private async void btn_test_Click(object sender, RoutedEventArgs e)
        {
            if (POS_starting.openCheck == false)
            {
                MessageDialog md = new MessageDialog("개점 하고 누르세요");
                await md.ShowAsync();
                return;
            }
            Web web = new Web();
            web.sendAllData();
        }
    }
}

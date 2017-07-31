using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using POS_UWP.DBConn;
using Windows.System;
using System;

namespace POS_UWP.Views
{
    public sealed partial class POS_finishing : Page
    {
        public POS_finishing()
        {
            this.InitializeComponent();
            btn_finishing2.IsEnabled = false;
            SetText();
        }

        //TextBox 업데이트
        private void SetText()
        {
            txtbox_Card.Text = POS_salesearch.cardprice;  //신용카드금액
            txtbox_Client.Text = POS_salesearch.clientCount.ToString();  //고객수
            txtbox_Cash.Text = POS_salesearch.cashprice; //현금금액
            txtbox_totalPrice.Text = POS_salesearch.totalprice;  //총 판매금액
            txtbox_CancelPrice.Text = DBConn_SaleSearch.priceTemp.ToString();  //매출취소금액
            txtbox_InitPrice.Text = POS_starting.startPrice;  //준비금
            txtbox_Discount.Text = POS_sale.totalDiscount.ToString();  //할인금액
        }

        //close button
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        //계산원 마감 버튼
        private void btn_finishing1_Click(object sender, RoutedEventArgs e)
        {
            new DBConn_MemberTime().UpdateMemberTime(POS_main.managerName);
            btn_finishing2.IsEnabled = true;

            //영수증 출력 기능 등 조명 끄기, 노래끄기등등

            btn_finishing1.IsEnabled = false;
        }

        private void btn_finishing2_Click(object sender, RoutedEventArgs e)
        {
            new DBConn_SaleSearch().Finish();
            //영수증 출력 후 시스템 종료

            /*시스템 즉시 종료*/
            ShutdownManager.BeginShutdown(ShutdownKind.Shutdown, TimeSpan.Zero);
        }
    }
}

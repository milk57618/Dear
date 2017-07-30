using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using POS_UWP.PosDB;
using POS_UWP.DBConn;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;

namespace POS_UWP.Views
{
    public sealed partial class POS_compound : Page
    {
        DBConn_SaleSearch Db_salesearch = new DBConn_SaleSearch();
        DBConn_SaleHistory Db_salehistory = new DBConn_SaleHistory();
        List<Sale> AllSaleList; // 모든 판매 리스트
        List<Sale> CashSaleList = new List<Sale>(); // 현금 결제할 리스트 (현금 
        List<Sale> CardSaleList = new List<Sale>();
        int cash = 0, card = 0;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            AllSaleList = e.Parameter as List<Sale>;
            POS_compound_Refresh();
        }

        public POS_compound()
        {
            this.InitializeComponent();
        }

        private void POS_compound_Refresh()
        {
            lv_Sale.ItemsSource = null;
            lv_Cash.ItemsSource = null;
            lv_Card.ItemsSource = null;
            lv_Sale.ItemsSource = AllSaleList;
            lv_Cash.ItemsSource = CashSaleList;
            lv_Card.ItemsSource = CardSaleList;
            cash = 0;
            card = 0;
            foreach (Sale s in CashSaleList) cash += s.TotalPrice;
            foreach (Sale s in CardSaleList) card += s.TotalPrice;
            tb_CashPrice.Text = cash.ToString() + " \\";
            tb_CardPrice.Text = card.ToString() + " \\";
        }

        //close button
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void btn_Cash_Click(object sender, RoutedEventArgs e)
        {
            if (lv_Sale.SelectedItems.Count == 0)
            {
                MessageDialog md = new MessageDialog("현금결제할 상품을 선택해주세요.");
                await md.ShowAsync();
                return;
            }
            foreach (Object o in lv_Sale.SelectedItems)
            {
                Sale s = o as Sale;
                CashSaleList.Add(s);
                AllSaleList.Remove(s);
            }
            POS_compound_Refresh();
        }

        private async void btn_Card_Click(object sender, RoutedEventArgs e)
        {
            if (lv_Sale.SelectedItems.Count == 0)
            {
                MessageDialog md = new MessageDialog("카드결제할 상품을 선택해주세요.");
                await md.ShowAsync();
                return;
            }
            foreach (Object o in lv_Sale.SelectedItems)
            {
                Sale s = o as Sale;
                CardSaleList.Add(s);
                AllSaleList.Remove(s);
            }
            POS_compound_Refresh();
        }

        private async void btn_Payment_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog md;
            if (AllSaleList.Count > 0)
            {
                md = new MessageDialog("왼쪽 리스트 상품들의 결제 방식을 선택해주세요.");
                await md.ShowAsync();
                return;
            }

            string time = DateTime.Now.ToString("yy/MM/dd HH:mm:ss");
            Db_salesearch.InsertSaleSearch(new SaleSearch(cash + card, cash, card, POS_main.managerID, time));

            foreach (Sale sale in CashSaleList)
                Db_salehistory.InsertSaleHistory(new SaleHistory(sale, time));
            foreach (Sale sale in CardSaleList)
                Db_salehistory.InsertSaleHistory(new SaleHistory(sale, time));

            CashSaleList.Clear();
            CardSaleList.Clear();

            POS_compound_Refresh();
            md = new MessageDialog("결제가 완료되었습니다.");
            await md.ShowAsync();
        }
    }
}

using System;
using System.Collections.Generic;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using POS_UWP.PosDB;
using POS_UWP.DBConn;
using System.Collections.ObjectModel;

namespace POS_UWP.Views
{
    public sealed partial class POS_salesearch : Page
    {
        DBConn_SaleSearch Db_salesearch = new DBConn_SaleSearch();
        DBConn_SaleHistory Db_salehistory = new DBConn_SaleHistory();
        SaleSearch currentsalesearch = new SaleSearch();
        ObservableCollection<SaleSearchTemp> DB_SaleSearchList = new ObservableCollection<SaleSearchTemp>();
        List<SaleHistory> DB_SaleHistoryList = new List<SaleHistory>();
        private int select_sale = -1;
        public static int cancelCount = 0;
        public static string clientCount = "0";
        public static string totalprice = "0";
        public static string cardprice = "0";
        public static string cashprice = "0";

        public POS_salesearch()
        {
            this.InitializeComponent();
            POS_SaleSearch_Refresh();
        }
        private void POS_SaleSearch_Refresh()
        {
            DB_SaleSearchList = Db_salesearch.GetList();
            lv_SaleMore.ItemsSource = null;

            /*교환 환불 버튼 활성/비활성화*/
            if (DB_SaleSearchList.Count <= 0)
            {
                btn_Refund.IsEnabled = false;
            }
            else
            {
                btn_Refund.IsEnabled = true;
            }

            //리스트 아이템 오름차순으로 정렬
            lv_Sale.ItemsSource = DB_SaleSearchList;

            /*고객 수 count*/
            txtbox_Client.Text = DB_SaleSearchList.Count.ToString();
            clientCount = txtbox_Client.Text;

            /*총금액, 현금금액, 카드금액 계산*/
            int Sum = 0, Cash = 0, Card = 0;
            foreach (SaleSearchTemp s in DB_SaleSearchList)
            {
                Sum += s.Price;
                Cash += s.Cash;
                Card += s.Card;
            }

            /*총금액, 현금금액, 카드금액 표시*/
            txtbox_Sum.Text = Sum.ToString();
            txtbox_Cash.Text = Cash.ToString();
            txtbox_Card.Text = Card.ToString();

            /*환불 금액 표시*/
            txtbox_Cancel.Text = DBConn_SaleSearch.priceTemp.ToString();

            totalprice = txtbox_Sum.Text;
            cashprice = txtbox_Cash.Text;
            cardprice = txtbox_Card.Text;
        }


        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void lv_Sale_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lv_Sale.SelectedIndex != -1)
            {
                SaleSearchTemp listitem = lv_Sale.SelectedItem as SaleSearchTemp;
                select_Function(listitem.SaleCount);

                DB_SaleHistoryList.Clear();  //리스트 값을 비워주고
                //salesearch와 시간이 동일한 salehistory값을 불러옴
                DB_SaleHistoryList = Db_salehistory.ReadSaleHistoryList(currentsalesearch.SaleTime);
                DB_SaleHistoryList.Sort();
                lv_SaleMore.ItemsSource = DB_SaleHistoryList; //listview SaleMore에 리스트를 보여줌
            }
        }

        private void select_Function(int Id)
        {
            //선택된 값의 객체를 가리키는 함수
            select_sale = Id;
            currentsalesearch = Db_salesearch.ReadSaleSearch(select_sale);
        }

        private async void btn_Refund_Click(object sender, RoutedEventArgs e)
        {
            //해당하는 salesearch와 salehistory 값을 삭제함
            Db_salesearch.DeleteSaleSearch(currentsalesearch.SaleCount);
            Db_salehistory.DeleteSaleHistory(currentsalesearch.SaleTime);
            POS_SaleSearch_Refresh();
            cancelCount++;
            MessageDialog messageDialog = new MessageDialog("환불이 완료되었습니다.");
            await messageDialog.ShowAsync();
        }
    }
}

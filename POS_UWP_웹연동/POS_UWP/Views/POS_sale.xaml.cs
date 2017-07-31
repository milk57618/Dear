using System;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using POS_UWP.PosDB;
using POS_UWP.DBConn;

namespace POS_UWP.Views
{

    public sealed partial class POS_sale : Page
    {
        DispatcherTimer Timer = new DispatcherTimer();

        ObservableCollection<Categorys> DB_CateList = new ObservableCollection<Categorys>();
        ObservableCollection<Product> DB_ProList = new ObservableCollection<Product>();
        ObservableCollection<Sale> DB_SaleList = new ObservableCollection<Sale>();
        ObservableCollection<SaleSearch> DB_SaleSearchList = new ObservableCollection<SaleSearch>();
        DBConn_Product Db_Pro = new DBConn_Product();
        DBConn_Category Db_cate = new DBConn_Category();
        DBConn_Sale Db_sale = new DBConn_Sale();
        DBConn_SaleSearch Db_salesearch = new DBConn_SaleSearch();
        DBConn_SaleHistory Db_salehistory = new DBConn_SaleHistory();


        Sale currentsale = new Sale();

        string cb_select_cate;
        int count = 1;
        int number = 0;
        int discountRate = 0;
        int discountMoney = 0;
        int SaleSelected = 0;
        string sale_select;
        public static int totalDiscount = 0;

        public POS_sale()
        {
            this.InitializeComponent();
            POS_Sale_DeleteAllSale();

            btn_Cash.IsEnabled = false;
            btn_Card.IsEnabled = false;
            btn_Compound.IsEnabled = false;

            /*콤보박스 리스트 나타내기*/
            DBConn_Category dbCates = new DBConn_Category();
            DB_CateList = dbCates.ReadCategorys();
            cb_Category.ItemsSource = DB_CateList;

            /*전체취소, 선택취소, +, - 버튼 비활성화*/
            btn_CancelAll.IsEnabled = false;
            btn_CancelSelect.IsEnabled = false;
            btn_Plus.IsEnabled = false;
            btn_Minus.IsEnabled = false;


            /*현재시간 받아오기*/
            DataContext = this;
            Timer.Tick += Timer_Tick;
            Timer.Interval = new TimeSpan(0, 0, 0);
            Timer.Start();

            //관리자
            tb_manager.Text = POS_main.managerName;

        }

        private void POS_Sale_DeleteAllSale()
        {
            Db_sale.DeleteAllSale();  //테이블에 있는 정보 모두 초기화

            /*총금액, 할인금액, 받은/을 돈, 거스름돈 리셋*/
            tb_Total.Text = "0";
            tb_Discount.Text = "0";
            tb_Receive.Text = "0";
            tb_Received.Text = "0";
            tb_Change.Text = "0";
        }

        /*카테고리 별 상품들 업데이트하는 함수*/
        private void POS_SalePro_Refresh()
        {
            DBConn_Product dbPros = new DBConn_Product();
            //list초기화
            DB_ProList = dbPros.GetProductForAssessment(cb_select_cate);
            lb_Product.ItemsSource = DB_ProList.OrderBy(i => i.Id).ToList();  //리스트 값을 오름차순으로 정렬

        }

        private void Timer_Tick(object sender, object e)
        {
            tb_time.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void btn_SalesSearch_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(POS_salesearch));
        }

        private void btn_Manage_Click(object sender, RoutedEventArgs e)
        {

            Frame.Navigate(typeof(POS_manager));
        }

        private void btn_SalesHistory_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(POS_salestatus));
        }

        private void btn_Setting_Click(object sender, RoutedEventArgs e)
        {

            Frame.Navigate(typeof(POS_setting));
        }

        private void cb_Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //콤보박스 값을 선택 시 객체 할당
            if (cb_Category.SelectedIndex != -1)
            {
                Categorys cb_item = cb_Category.SelectedItem as Categorys;
                cb_select_cate = cb_item.category;
                POS_SalePro_Refresh();
            }
        }

        private void lb_Product_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lb_Product.SelectedIndex != -1)
            {
                Product pro_item = lb_Product.SelectedItem as Product;

                //수량과 총금액을 계산
                int tot = Int32.Parse(pro_item.Price) * count;

                //선택된 상품이 중복되지 않은 경우의 조건문
                if (!Db_sale.OverlapSearch(pro_item.Name))
                {
                    number = Db_sale.SetId(); //아이디 지정해주는 함수
                    Db_sale.InsertSale(new Sale(pro_item.Name, Int32.Parse(pro_item.Price), count, tot, number));

                    //Sale List Update
                    POS_Sale_LV_Refresh();
                }
            }
        }

        public void POS_Sale_LV_Refresh()  //선택한 물품 리스트 업데이트
        {
            DB_SaleList = Db_sale.ReadSales();
            lv_Sale.ItemsSource = DB_SaleList;

            if (DB_SaleList.Count == 0)
            {
                btn_Cash.IsEnabled = false;
                btn_Card.IsEnabled = false;
                btn_Compound.IsEnabled = false;
            }
            else
            {
                btn_Cash.IsEnabled = true;
                btn_Card.IsEnabled = true;
                btn_Compound.IsEnabled = true;
            }

            /*전체취소, 선택취소, +, - 버튼 활성/비활성화*/
            if (DB_SaleList.Count > 0)
            {
                btn_CancelAll.IsEnabled = true;
            }
            else
            {
                btn_CancelAll.IsEnabled = false;
                btn_CancelSelect.IsEnabled = false;
                btn_Plus.IsEnabled = false;
                btn_Minus.IsEnabled = false;
            }

            /*리스트에서 받아온 값 모두 더한 총금액 업데이트*/
            tb_Total.Text = Db_sale.CalculateAll();

            /*총금액 값을 받아와 할인금액을 업데이트해주기*/
            double sum = Convert.ToDouble(tb_Total.Text);
            double discount = 0;
            bool check = false;

            // 할인율을 먼저 계산하고 그 다음 할인금액을 적용함
            if (discountRate != 0)
            {
                discount = sum * ((double)discountRate * 0.01); //할인율 계산
                discount = discount - (discount % 10);  //10원이하는 버림

                /*txtbox 업데이트*/
                tb_Discount.Text = Convert.ToString(-discount);
                tb_Receive.Text = Convert.ToString(sum - discount);

                check = true;
                discountRate = 0;

            }
            else
            {
                tb_Discount.Text = "0";
                tb_Receive.Text = tb_Total.Text;
            }

            if (discountMoney != 0)
            {
                /*txtbox 업데이트*/
                tb_Discount.Text = Convert.ToString(-discountMoney);
                tb_Receive.Text = Convert.ToString(sum - discountMoney);

                discountMoney = 0; //값 초기화
            }
            else
            {
                if (check == false)
                {
                    tb_Discount.Text = "0";
                    tb_Receive.Text = tb_Total.Text;
                }
            }

        }
        private void lv_Sale_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lv_Sale.SelectedIndex != -1)  //Sale ListView 값 선택
            {
                SaleSelected = lv_Sale.SelectedIndex;
                Sale listitem = lv_Sale.SelectedItem as Sale;

                sale_select = listitem.Name;
                currentsale = Db_sale.ReadSales(sale_select);

                /*선택한 상태에서는 선택버튼, +, -버튼 활성화*/
                btn_CancelSelect.IsEnabled = true;
                btn_Plus.IsEnabled = true;
                btn_Minus.IsEnabled = true;
            }
        }

        private void btn_Plus_Click(object sender, RoutedEventArgs e)
        {
            if (sale_select != null)
            {
                Db_sale.PlusSale(currentsale); //DB에 수량 +1해주기
                tb_Total.Text = Db_sale.CalculateAll();  //총금액 바꾸기
                POS_Sale_LV_Refresh(); //list 리셋
            }
        }

        private void btn_Minus_Click(object sender, RoutedEventArgs e)
        {
            if (sale_select != null)
            {
                Db_sale.MinusSale(currentsale);  //DB에 수량 -1해주기
                tb_Total.Text = Db_sale.CalculateAll();  //총금액 바꾸기
                POS_Sale_LV_Refresh(); //list 리셋
            }
        }

        private void btn_CancelAll_Click(object sender, RoutedEventArgs e)
        {
            Db_sale.DeleteAllSale(); //테이블 값 모두 삭제
            discountRate = 0;
            discountMoney = 0;
            POS_Sale_LV_Refresh(); //list 리셋
        }

        private void btn_CancelSelect_Click(object sender, RoutedEventArgs e)
        {
            Db_sale.DeleteSale(SaleSelected + 1);
            POS_Sale_LV_Refresh();  //list 리셋
        }

        private void btn_Backspace_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Sum.Text.Equals("0"))
                return;
            tb_Sum.Text = tb_Sum.Text.Remove(tb_Sum.Text.Length - 1);
            if (tb_Sum.Text.Equals(""))
                tb_Sum.Text = "0";
        }

        private void btn_PercentDC_Click(object sender, RoutedEventArgs e)
        {
            discountRate = Convert.ToInt32(tb_Sum.Text);
            tb_Sum.Text = "0";
            POS_Sale_LV_Refresh();
        }

        private void btn_WonDC_Click(object sender, RoutedEventArgs e)
        {
            discountMoney = Convert.ToInt32(tb_Sum.Text);
            tb_Sum.Text = "0";
            POS_Sale_LV_Refresh();
        }

        private void btn_Enter_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Sum.Text.Equals("0"))
                return;
            String sum = tb_Sum.Text;
            tb_Sum.Text = "0";
            int tot = Int32.Parse(sum) * count;

            // 예외로 추가하는 아이템 추가
            number = Db_sale.SetId(); //아이디 지정해주는 함수
            Db_sale.InsertSale(new Sale("", Int32.Parse(sum), count, tot, number));
            POS_Sale_LV_Refresh();  //업데이트
        }

        private void btn_One_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Sum.Text.Equals("0"))
                tb_Sum.Text = "";
            tb_Sum.Text += "1";
        }

        private void btn_Two_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Sum.Text.Equals("0"))
                tb_Sum.Text = "";
            tb_Sum.Text += "2";

        }

        private void btn_Three_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Sum.Text.Equals("0"))
                tb_Sum.Text = "";
            tb_Sum.Text += "3";

        }

        private void btn_Four_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Sum.Text.Equals("0"))
                tb_Sum.Text = "";
            tb_Sum.Text += "4";

        }

        private void btn_Five_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Sum.Text.Equals("0"))
                tb_Sum.Text = "";
            tb_Sum.Text += "5";

        }

        private void btn_Six_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Sum.Text.Equals("0"))
                tb_Sum.Text = "";
            tb_Sum.Text += "6";

        }

        private void btn_Seven_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Sum.Text.Equals("0"))
                tb_Sum.Text = "";
            tb_Sum.Text += "7";

        }

        private void btn_Eight_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Sum.Text.Equals("0"))
                tb_Sum.Text = "";
            tb_Sum.Text += "8";

        }

        private void btn_Nine_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Sum.Text.Equals("0"))
                tb_Sum.Text = "";
            tb_Sum.Text += "9";

        }

        private void btn_Zero_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Sum.Text.Equals("0"))
                return;
            tb_Sum.Text += "0";

        }

        private void btn_TwoZero_Click(object sender, RoutedEventArgs e)
        {
            if (tb_Sum.Text.Equals("0"))
                return;
            tb_Sum.Text += "00";
        }

        private void btn_CL_Click(object sender, RoutedEventArgs e)
        {
            tb_Sum.Text = "0";
        }

        private async void btn_Cash_Click(object sender, RoutedEventArgs e)
        {
            string time = DateTime.Now.ToString("yy/MM/dd HH:mm:ss");
            Db_salesearch.InsertSaleSearch(new SaleSearch(Convert.ToInt32(tb_Receive.Text), Convert.ToInt32(tb_Receive.Text), 0, POS_main.managerID, time));
            foreach (Sale sale in DB_SaleList)
            {
                Db_salehistory.InsertSaleHistory(new SaleHistory(sale, time));
            }
            totalDiscount += Convert.ToInt32(tb_Discount.Text);
            POS_Sale_DeleteAllSale();
            POS_Sale_LV_Refresh();
            MessageDialog md = new MessageDialog("결제가 완료되었습니다.");
            await md.ShowAsync();
        }

        private async void btn_Card_Click(object sender, RoutedEventArgs e)
        {
            string time = DateTime.Now.ToString("yy/MM/dd HH:mm:ss");
            Db_salesearch.InsertSaleSearch(new SaleSearch(Convert.ToInt32(tb_Receive.Text), 0, Convert.ToInt32(tb_Receive.Text), POS_main.managerID, time));
            foreach (Sale sale in DB_SaleList)
            {
                Db_salehistory.InsertSaleHistory(new SaleHistory(sale, time));
            }
            totalDiscount += Convert.ToInt32(tb_Discount.Text);
            POS_Sale_DeleteAllSale();
            POS_Sale_LV_Refresh();
            MessageDialog md = new MessageDialog("결제가 완료되었습니다.");
            await md.ShowAsync();
        }

        private void btn_Compound_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(POS_compound), DB_SaleList.ToList());
        }

        private void TextBlock_SelectionChanged(System.Object sender, RoutedEventArgs e)
        {

        }

        
    }
}
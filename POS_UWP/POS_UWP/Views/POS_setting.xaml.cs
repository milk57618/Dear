using System;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using POS_UWP.DBConn;
using System.Collections.ObjectModel;
using POS_UWP.PosDB;

namespace POS_UWP.Views
{
    public sealed partial class POS_setting : Page
    {
        ObservableCollection<Categorys> DB_CateList = new ObservableCollection<Categorys>();
        ObservableCollection<Product> DB_ProList = new ObservableCollection<Product>();
        DBConn_Product Db_Pro = new DBConn_Product();
        DBConn_Category Db_cate = new DBConn_Category();

        Categorys currentcate = new Categorys();
        Product currentpro = new Product();

        int lv_select_pro = -1;
        string select_pro = "";
        string cb_select_cate;

        public POS_setting()
        {
            this.InitializeComponent();

            DB_CateList = Db_cate.ReadCategorys();

            //콤보박스 업데이트
            cb_Category.ItemsSource = DB_CateList;
        }

        private void POS_product_Refresh()
        {
            DBConn_Product dbPros = new DBConn_Product();

            //list초기화
            DB_ProList = dbPros.GetProductForAssessment(cb_select_cate);

            if (DB_ProList.Count > 0)
            {
                btn_ProductDelete.IsEnabled = true;
            }
            else
            {
                btn_ProductDelete.IsEnabled = false;
            }

            lv_Product.ItemsSource = DB_ProList.OrderBy(i => i.Id).ToList();  //리스트 값을 오름차순으로 정렬

        }

        private void btn_Category_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(POS_category));
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private async void btn_ProductAdd_Click(object sender, RoutedEventArgs e)
        {
            /*값이 중복되지 않으면 추가*/
            if (txtbox_Name.Text != "" && txtbox_Price.Text != "" && !Db_Pro.SearchProduct(txtbox_Name.Text))
            {
                int id = Db_Pro.SetId(); //번호 설정

                //값 추가
                Db_Pro.InsertProduct(new Product(id, cb_select_cate, txtbox_Name.Text, txtbox_Price.Text));

                txtbox_Name.Text = "";
                txtbox_Price.Text = "";

                //상품 list update
                POS_product_Refresh();
            }
            else
            {
                if (Db_Pro.SearchProduct(txtbox_Name.Text))
                {
                    MessageDialog messageDialog = new MessageDialog("중복되는 상품이 존재합니다.");//Text should not be empty 
                    await messageDialog.ShowAsync();
                }
                else
                {
                    MessageDialog messageDialog = new MessageDialog("이름과 가격을 입력하세요!!");//Text should not be empty 
                    await messageDialog.ShowAsync();
                }

            }
        }

        private async void btn_ProductChange_Click(object sender, RoutedEventArgs e)
        {
            /*값이 중복되지 않으면 수정*/
            if (txtbox_Name.Text != "" && txtbox_Price.Text != "")
            {
                currentpro.category = cb_select_cate;
                currentpro.Name = txtbox_Name.Text;
                currentpro.Price = txtbox_Price.Text;

                //값 수정
                Db_Pro.UpdateProduct(currentpro);

                txtbox_Name.Text = "";
                txtbox_Price.Text = "";

                //리스트 업데이트
                POS_product_Refresh();

                MessageDialog mess = new MessageDialog("수정이 완료되었습니다.");
                await mess.ShowAsync();
            }
            else
            {
                MessageDialog mess = new MessageDialog("항목을 선택하세요");
                await mess.ShowAsync();
            }
        }

        private async void btn_ProductDelete_Click(object sender, RoutedEventArgs e)
        {
            if (txtbox_Name.Text != "" && txtbox_Price.Text != "")
            {
                //값 삭제
                Db_Pro.DeleteProduct(lv_select_pro);

                txtbox_Name.Text = "";
                txtbox_Price.Text = "";

                POS_product_Refresh();

                MessageDialog mess = new MessageDialog("삭제되었습니다.");
                await mess.ShowAsync();
            }
            else
            {
                MessageDialog mess = new MessageDialog("항목을 선택하세요");
                await mess.ShowAsync();
            }
        }

        /*리스트 선택 값을 객체로 저장*/
        private void select_Function(int select_value)
        {
            currentpro = Db_Pro.ReadProducts(select_value);
            select_pro = currentpro.Name;
            txtbox_Name.Text = currentpro.Name;
            txtbox_Price.Text = currentpro.Price;
        }

        //listbox select change
        private void lv_Product_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lv_Product.SelectedIndex != -1)
            {
                Product listitem = lv_Product.SelectedItem as Product;
                lv_select_pro = listitem.Id;
                select_Function(listitem.Id);
            }
        }

        //combobox change event
        private void cb_Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_Category.SelectedIndex != -1)
            {
                Categorys cb_item = cb_Category.SelectedItem as Categorys;
                cb_select_cate = cb_item.category;
                txtbox_Name.Text = "";
                txtbox_Price.Text = "";
                POS_product_Refresh();
            }
        }
    }
}
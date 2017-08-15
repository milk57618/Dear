using System;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using POS_UWP.DBConn;
using System.Collections.ObjectModel;
using POS_UWP.PosDB;
using Windows.UI.Xaml.Input;

namespace POS_UWP.Views
{
    public sealed partial class POS_category : Page
    {
        int select_cate = -1;
        string select_cateS;
        DBConn_Category Db_Cate = new DBConn_Category();
        Categorys currentcate = new Categorys();

        ObservableCollection<Categorys> DB_CateList = new ObservableCollection<Categorys>();

        public POS_category()
        {
            this.InitializeComponent();
            POS_category_Refresh();
        }

        /*카테고리 리스트 값을 업데이트하는 함수*/
        private void POS_category_Refresh()
        {
            DBConn_Category dbCates = new DBConn_Category();
            DB_CateList = dbCates.ReadCategorys();

            /*삭제 버튼 활성/비활성화 */
            if (DB_CateList.Count > 0)
            {
                btn_Delete.IsEnabled = true;
            }
            else
            {
                btn_Delete.IsEnabled = false;
            }

            /*카테고리 리스트 값 오름차순으로 업데이트*/
            lv_Category.ItemsSource = DB_CateList.OrderBy(i => i.Id).ToList();
        }

        /*선택한 값의 객체 설정 및 txtbox 값 업데이튼*/
        private void select_Function(int select_value)
        {
            select_cate = select_value;
            currentcate = Db_Cate.ReadCategorys(select_cate);
            select_cateS = currentcate.Category;
            txtbox_Category.Text = currentcate.Category;
        }

        /*페이지 이동*/
        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }


        private async void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            //카테고리 값이 중복되지 않으면 값을 추가하는 조건문
            if (txtbox_Category.Text != "" && !Db_Cate.SearchCategory(select_cateS))
            {
                currentcate.Id = select_cate;

                int num = Db_Cate.SetId(); //번호를 정렬시킴

                //데이터베이스 값 삽입
                Db_Cate.InsertCategory(new Categorys(num, txtbox_Category.Text));
                txtbox_Category.Text = "";

                //카테고리 리스트 업데이트
                POS_category_Refresh();

            }
            else
            {
                //카테고리값이 중복되는 경우의 조건문
                if (Db_Cate.SearchCategory(select_cateS))
                {
                    MessageDialog messageDialog = new MessageDialog("중복된 카테고리가 존재합니다.");
                    await messageDialog.ShowAsync();
                }
                else
                {
                    MessageDialog messageDialog = new MessageDialog("카테고리를 입력하세요!!");//Text should not be empty 
                    await messageDialog.ShowAsync();
                }

            }
        }

        private async void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (txtbox_Category.Text != "")
            {
                //해당하는 카테고리 값 삭제
                Db_Cate.DeleteCategory(currentcate);
                txtbox_Category.Text = "";

                //카테고리 리스트 업데이트
                POS_category_Refresh();
            }
            else
            {
                MessageDialog mess = new MessageDialog("항목을 선택하세요");
                await mess.ShowAsync();
            }
        }

        private async void btn_Change_Click(object sender, RoutedEventArgs e)
        {
            //카테고리 값이 중복되지 않으면 값을 수정하는 조건문
            if (txtbox_Category.Text != "")
            {
                currentcate.Id = select_cate;
                currentcate.Category = txtbox_Category.Text;

                //데이터베이스 값을 업데이트함
                Db_Cate.UpdateCategory(currentcate);
                txtbox_Category.Text = "";

                //카테고리 리스트 업데이트
                POS_category_Refresh();
            }
            else
            {
                MessageDialog mess = new MessageDialog("항목을 선택하세요");
                await mess.ShowAsync();

            }
        }

        private void lv_Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //리스트 선택한 값을 객체에 할당
            if (lv_Category.SelectedIndex != -1)
            {
                Categorys listitem = lv_Category.SelectedItem as Categorys;
                select_Function(listitem.Id);
            }
        }

        private async void txtbox_Category_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            //Enter Key
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                if (txtbox_Category.Text != "" && !Db_Cate.SearchCategory(select_cateS))
                {
                    currentcate.Id = select_cate;
                    int num = Db_Cate.SetId(); //번호를 정렬시킴

                    //데이터베이스 값 삽입
                    Db_Cate.InsertCategory(new Categorys(num, txtbox_Category.Text));

                    txtbox_Category.Text = "";

                    //카테고리 리스트 업데이트
                    POS_category_Refresh();

                }
                else
                {
                    if (Db_Cate.SearchCategory(select_cateS))
                    {
                        MessageDialog messageDialog = new MessageDialog("중복된 카테고리가 존재합니다.");
                        await messageDialog.ShowAsync();
                    }
                    else
                    {
                        MessageDialog messageDialog = new MessageDialog("카테고리를 입력하세요!!");//Text should not be empty 
                        await messageDialog.ShowAsync();
                    }

                }
            }
        }
    }
}
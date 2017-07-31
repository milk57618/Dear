using POS_UWP.PosDB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using POS_UWP.DBConn;
using System.Collections.ObjectModel;
using SQLite;

namespace POS_UWP.Views
{

    public sealed partial class POS_adminsetting : Page
    {
        DBConn_Member dbhelper = new DBConn_Member();
        ObservableCollection<Member> DB_MemberList = new ObservableCollection<Member>();

        string selectedPosi;  /*콤보 박스에서 지정한 직위*/
        int memberLiSelected;
        string inputPay;  /*직원들의 임금*/

        public POS_adminsetting()
        {
            this.InitializeComponent();
            btn_Delete.IsEnabled = false;
            ReadAllMemberList();
        }

        private void ReadAllMemberList()
        {
            DBConn_Member dbConntact = new DBConn_Member();
            DB_MemberList = dbConntact.ReadMembers();

            /*삭제버튼 상태 조정*/

            if (DB_MemberList.Count > 0)
            {
                btn_Delete.IsEnabled = true;
            }
            else
            {
                btn_Delete.IsEnabled = false;
            }

            lv_member.ItemsSource = DB_MemberList.OrderBy(i => i.Id).ToList();
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        /*직원 추가 버튼*/
        private async void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            if (txtbox_Name.Text != "" & txtbox_Phonenumber.Text != "" & txtbox_Pay.Text != "")
            {
                if (dbhelper.SearchPhoneNumber(txtbox_Phonenumber.Text) && dbhelper.SearchName(txtbox_Name.Text))
                {  /*번호중복방지 ,이름 중복 방지*/

                    /*아이디 지정 함수*/
                    int Id = dbhelper.SetId();
                    inputPay = txtbox_Pay.Text;

                    dbhelper.InsertMember(new Member(Id, txtbox_Name.Text, txtbox_Phonenumber.Text, selectedPosi, inputPay));

                    ReadAllMemberList();
                }
                else
                {
                    MessageDialog messageDialog = new MessageDialog("이름 또는 번호가 중복됩니다.");
                    await messageDialog.ShowAsync();
                }
            }

            else
            {
                MessageDialog messageDialog = new MessageDialog("추가 할 이름과 전화번호,시급을 입력하세요");//Text should not be empty 
                await messageDialog.ShowAsync();
            }

            /*추가 시 텍스트박스들 초기화*/
            txtbox_Name.Text = "";
            txtbox_Phonenumber.Text = "";
            txtbox_Pay.Text = "";
        }

        /*직원 수정 버튼*/
        private async void btn_Modify_Click(object sender, RoutedEventArgs e)
        {
            string checkPhone = dbhelper.Searchselected(memberLiSelected + 1);
            string checkName = dbhelper.SearchName(memberLiSelected + 1);

            /*입력오류 잡아주기 (번호,이름 중복방지,빈칸방지) */
            if (txtbox_Name.Text != "" && txtbox_Phonenumber.Text != "" && txtbox_Pay.Text != "")
            {
                if (((txtbox_Phonenumber.Text == checkPhone) || dbhelper.SearchPhoneNumber(txtbox_Phonenumber.Text)))  // 번호중복 방지 && 현재 자신의 번호는 사용가능
                {
                    if ((dbhelper.SearchName(txtbox_Name.Text) || (txtbox_Name.Text == checkName)))
                    {
                        inputPay = txtbox_Pay.Text;
                        dbhelper.UpdateMember(txtbox_Name.Text, txtbox_Phonenumber.Text, selectedPosi, memberLiSelected + 1, inputPay);

                        ReadAllMemberList();
                    }
                }
                else
                {
                    MessageDialog messageDialog = new MessageDialog("이름 또는 번호가 중복됩니다.");//Text should not be empty 
                    await messageDialog.ShowAsync();
                }
            }

            else
            {
                MessageDialog messageDialog = new MessageDialog("수정 할 이름과 전화번호를 입력하세요");//Text should not be empty 
                await messageDialog.ShowAsync();
            }
        }

        /*직원 삭제 버튼*/
        private async void btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            if(POS_main.managerName.Equals((lv_member.SelectedItem as Member).Name))
            {
                MessageDialog messageDialog = new MessageDialog("삭제가 불가합니다."); //Text should not be empty 
                await messageDialog.ShowAsync();
                return;
            }
            if (memberLiSelected > -1)
            {
                dbhelper.DeleteMember(memberLiSelected + 1);
                ReadAllMemberList();
            }
        }

        /*선택한 직급에 따라 직원의 직급을 지정한다.*/
        private void cb_Position_SelectionChanged(object sender, SelectionChangedEventArgs e)  //직위 설정
        {
            if (cb_Position.SelectedItem == null) return;

            if (cb_Position.SelectedIndex == 0)
                selectedPosi = "사장";
            else if (cb_Position.SelectedIndex == 1)
                selectedPosi = "매니저";
            else if (cb_Position.SelectedIndex == 2)
                selectedPosi = "직원";


        }


        private void lv_member_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            memberLiSelected = lv_member.SelectedIndex;
            var item = e.AddedItems.FirstOrDefault();

            Member s = (Member)item;

            if (s != null)
            {
                txtbox_Name.Text = s.Name;
                txtbox_Phonenumber.Text = s.PhoneNumber;
                txtbox_Pay.Text = s.Price.ToString();
            }
        }

        /*직원임금 조회 창으로 간다*/
        private void btn_Commute_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(POS_commute));
        }
    }
}
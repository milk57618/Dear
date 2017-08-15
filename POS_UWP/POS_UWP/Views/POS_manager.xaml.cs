using POS_UWP.DBConn;
using POS_UWP.PosDB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace POS_UWP.Views
{

    public sealed partial class POS_manager : Page
    {
        string selectedPosi; //콤보박스에서 선택한 조회할 직급
        DBConn_Member dbhelper = new DBConn_Member();
        DBConn_MemberTime T_dbhelper = new DBConn_MemberTime();
        ObservableCollection<Member> DB_Member = new ObservableCollection<Member>();

        public POS_manager()
        {
            this.InitializeComponent();
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        /*선택한 직급 별로 보여줘야할 직급을 지정*/
        private void cb_Position_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cb_Position.SelectedItem == null) return;

            if (cb_Position.SelectedIndex == 0)
                selectedPosi = "사장";
            else if (cb_Position.SelectedIndex == 1)
                selectedPosi = "매니저";
            else if (cb_Position.SelectedIndex == 2)
                selectedPosi = "직원";


            DB_Member = dbhelper.GetMemberForPosi(selectedPosi);
            /*선택한 직급별로 직원들을 보여준다.*/
            cb_Name.ItemsSource = DB_Member.OrderBy(i => i.Id).ToList();

        }

        /*관리자 변경 버튼*/
        private void btn_Change_Click(object sender, RoutedEventArgs e)
        {
            if (cb_Name.SelectedItem != null)
            {
                Member cb_member = cb_Name.SelectedItem as Member;

                if (cb_member.Name != POS_main.managerName)
                {

                    if (POS_main.managerName != "")
                    {
                        T_dbhelper.UpdateMemberTime(POS_main.managerName);
                    }

                    /*현재시간,직원들정보를 얻어와서 직원임금테이블에 저장한다.*/
                    POS_main.managerName = cb_member.Name;
                    POS_main.managerID = cb_member.Id;
                    string date = DateTime.Now.ToString("yyyy/M");
                    string day = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
                    T_dbhelper.InsertMemberTime(new MemberTime(cb_member.Name, date, day)); //이전 멤버의 기록을 추가한다.
                    Web.sendDataToServer();

                    Frame.GoBack();
                }
            }
        }

        private void cb_Name_SelectionChanged(System.Object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
using POS_UWP.DBConn;
using POS_UWP.PosDB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace POS_UWP.Views
{
    public sealed partial class POS_commute : Page
    {
        int curYear;
        int curMonth;
        double memberTime = 0;
        double m_minute = 0;
        double m_hour = 0;
        string curDay;
        Member selectedMember;

        DBConn_Member dbMemHelper = new DBConn_Member();
        DBConn_MemberTime dbTimeHelper = new DBConn_MemberTime();
        List<MemberTime> DB_MemberTime = new List<MemberTime>();

        public POS_commute()
        {
            this.InitializeComponent();

            /*현재 날짜 저장*/
            curDay = DateTime.Now.ToString("yyyy/M");
            string a = curDay;
            curYear = int.Parse(DateTime.Now.Year.ToString());
            curMonth = int.Parse(DateTime.Now.Month.ToString());

            List<Member> DB_Member = dbMemHelper.GetAllMember();

            /*콤보박스에 직원들 이름을 추가한다.*/
            cb_Member.ItemsSource = DB_Member.OrderBy(i => i.Id).ToList();



            tx_Date.Text = curYear + "년 " + curMonth + "월";
        }

        /*달별로 -*/
        private void btn_month_down_Click(object sender, RoutedEventArgs e)
        {
            if (curMonth == 1)
            {
                curMonth = 12;
                curYear--;
            }
            else
                curMonth--;


            tx_Date.Text = curYear + "년 " + curMonth + "월";


            curDay = curYear + "-" + curMonth;

            /*선택한 직원이 있다면 그직원의 정보와 작업시간을 보여준다.*/
            if (selectedMember != null)
            {
                DB_MemberTime = dbTimeHelper.SearchMemberTime(selectedMember.Name, curDay);

                for (int i = 0; i < DB_MemberTime.Count; i++)
                {
                    memberTime += DB_MemberTime[i].workTime;
                }

                m_hour = memberTime / 60;
                m_minute = memberTime % 60;

                tx_WorkTome.Text = ((int)m_hour).ToString() + "시간 " + ((int)m_minute).ToString() + "분";
                tx_Pay.Text = (((int)m_hour) * int.Parse(selectedMember.Price)).ToString() + "원";
                memberTime = 0;
            }
        }

        /*달별로 + (다음달 조회)*/
        private void btn_month_up_Click(object sender, RoutedEventArgs e)
        {
            if (curMonth == 12)
            {
                curMonth = 1;
                curYear++;
            }
            else
                curMonth++;

            tx_Date.Text = curYear + "년 " + curMonth + "월";

            curDay = curYear + "-" + curMonth;

            /*선택한 직원이 있다면 그직원의 정보와 작업시간을 보여준다.*/
            if (selectedMember != null)
            {
                DB_MemberTime = dbTimeHelper.SearchMemberTime(selectedMember.Name, curDay);

                for (int i = 0; i < DB_MemberTime.Count; i++)
                {
                    memberTime += DB_MemberTime[i].workTime;
                }

                m_hour = memberTime / 60;
                m_minute = memberTime % 60;

                tx_WorkTome.Text = ((int)m_hour).ToString() + "시간 " + ((int)m_minute).ToString() + "분";
                tx_Pay.Text = (((int)m_hour) * int.Parse(selectedMember.Price)).ToString() + "원"; //임금 설정 해주는곳 알바 직원에 따라 달라지는거 해결하기
                memberTime = 0;
            }

        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        /*선택한 직원이 바뀌었을 경우의 이벤트.*/
        private void cb_Member_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedMember = cb_Member.SelectedItem as Member;

            /*선택한 직원의 정보와 작업시간을 보여준다.*/
            if (selectedMember != null)
            {
                tx_Phone.Text = selectedMember.PhoneNumber;
                tx_Wage.Text = selectedMember.Price.ToString() + " 원";


                DB_MemberTime = dbTimeHelper.SearchMemberTime(selectedMember.Name, curDay);

                for (int i = 0; i < DB_MemberTime.Count; i++)
                {
                    memberTime += DB_MemberTime[i].workTime;
                }

                m_hour = memberTime / 60;
                m_minute = memberTime % 60;
                tx_WorkTome.Text = ((int)m_hour).ToString() + "시간 " + ((int)m_minute).ToString() + "분";
                tx_Pay.Text = (((int)m_hour) * int.Parse(selectedMember.Price)).ToString() + "원";

                memberTime = 0;
            }

        }
    }
}
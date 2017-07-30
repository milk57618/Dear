using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using Windows.UI.Xaml.Controls;
using POS_UWP.PosDB;
using POS_UWP.DBConn;

namespace POS_UWP.Views
{
    public sealed partial class POS_salestatus : Page
    {
        DBConn_SaleHistory Db_salehistory = new DBConn_SaleHistory();
        DBConn_SaleSearch Db_salesearch = new DBConn_SaleSearch();
        char mode = 'W'; //  W(week), M(month), Y(year)

        public POS_salestatus()
        {
            this.InitializeComponent();
            this.Loaded += POS_salestatus_Loaded;
        }

        class ProductRate : IComparable
        {
            public string Name { get; set; } // 물품 명
            public int Count { get; set; } // 팔린 개수
            public ProductRate(string Name, int Count)
            {
                this.Name = Name;
                this.Count = Count;
            }

            public int CompareTo(object p)
            {
                return ((IComparable)Count).CompareTo(((ProductRate)p).Count);
            }
        }
        class SaleSum : IComparable
        {
            public string Date { get; set; } // 판매 날짜
            public int Sum { get; set; } // 매출액
            public SaleSum(string Date, int Sum)
            {
                this.Date = Date;
                this.Sum = Sum;
            }

            public int CompareTo(object s)
            {
                return ((IComparable)Sum).CompareTo(((SaleSum)s).Sum);
            }
        }

        /* month의 전 달에 며칠까지 있는지 계산 */
        private int CalcPreviousMonth(int month)
        {
            int DaysInPreviousMonth = -1;  // 초기화

            if (month == 1)
                DaysInPreviousMonth = 31; // 12월은 31일까지 있으므로
            else
                DaysInPreviousMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month - 1);

            return DaysInPreviousMonth;
        }

        /* 현재 날짜에서 n달만큼 뺀 yy-MM 계산 */
        private string DecreaseMonth(int n)
        {
            string date = DateTime.Now.ToString("yy/MM");
            int year = Convert.ToInt32(date.Substring(0, 2));
            int month = Convert.ToInt32(date.Substring(3, 2));
            string yearS, monthS;

            while (n > 0)
            {
                n--;
                month--;
                if (month == 0) // 1월의 전 달은 작년 12월
                {
                    month = 12;
                    year--;
                }
            }
            yearS = year.ToString();

            if (month < 10) monthS = "0" + month.ToString(); // MM 형식으로 맞추기 위해
            else monthS = month.ToString();

            date = yearS + "-" + monthS;

            return date;
        }

        // 현재 날짜에서 n일만큼 뺀 yy-MM-dd 계산
        private string DecreaseDate(int n)
        {
            string date = DateTime.Now.ToString("yy/MM/dd");
            int year = Convert.ToInt32(date.Substring(0, 2));
            int month = Convert.ToInt32(date.Substring(3, 2));
            int day = Convert.ToInt32(date.Substring(6, 2));
            string yearS, monthS, dayS;

            while (n > 0)
            {
                n--;
                day--;
                if (day == 0)
                {
                    day = CalcPreviousMonth(month); // 이전 달에 며칠까지 있는지 계산
                    month--;
                }
                if (month == 0)
                {
                    month = 12;
                    year--;
                }
            }
            yearS = year.ToString();
            if (month < 10) monthS = "0" + month.ToString(); // MM 형식으로 맞추기 위해
            else monthS = month.ToString();
            if (day < 10) dayS = "0" + day.ToString(); // dd 형식으로 맞추기 위해
            else dayS = day.ToString();

            date = yearS + "-" + monthS + "-" + dayS;

            return date;
        }

        /* ProductRateList에 h 추가, 이미 물품이 있으면 개수를 더함 */
        private void insertHistory(SaleHistory h, List<ProductRate> ProductRateList)
        {
            bool has = false;
            foreach (ProductRate p in ProductRateList)
            {
                if (h.Name.Equals(p.Name)) // 리스트에 물품이 있는 경우 -> 개수 더함
                {
                    p.Count += h.Count;
                    has = true;
                    break;
                }
            }
            if (!has) // 리스트에 물품이 없는 경우 -> 새로운 물품 추가
            {
                ProductRateList.Add(new ProductRate(h.Name, h.Count));
            }
        }

        /* 파이차트 생성 */
        private void LoadPieChart()
        {
            List<SaleHistory> HistoryList = Db_salehistory.ReadAllSaleHistory();
            List<ProductRate> ProductRateList = new List<ProductRate>();

            foreach (SaleHistory h in HistoryList)
            {
                switch (mode)
                {
                    case 'W':
                        string[] yymmdd = new string[7]; // 최근 7일동안의 내역만 보여줌

                        for (int i = 0; i < yymmdd.Length; i++)
                        {
                            yymmdd[i] = DecreaseDate(i);
                        }
                        foreach (String s in yymmdd)
                        {
                            if (s.Equals(h.SaleTime.Substring(0, 8)))
                            {
                                insertHistory(h, ProductRateList);
                                break;
                            }
                        }
                        break;
                    case 'M':
                        string yymm = DateTime.Now.ToString("yy/MM"); // 이번달의 내역만 보여줌

                        if (yymm.Equals(h.SaleTime.Substring(0, 5)))
                        {
                            insertHistory(h, ProductRateList);
                            break;
                        }
                        break;
                    case 'Y':
                        string yy = DateTime.Now.ToString("yy"); // 올해의 내역만 보여줌

                        if (yy.Equals(h.SaleTime.Substring(0, 2)))
                        {
                            insertHistory(h, ProductRateList);
                            break;
                        }
                        break;
                }
            }
            ProductRateList.Sort();

            while (ProductRateList.Count > 7) // 판매한 개수가 많은 순서대로 7개만 리스트에 남기고 나머지는 제거
            {
                ProductRateList.RemoveAt(0);
            }
            ProductRateList.Reverse(); // 0번 원소가 가장 판매율이 높은 물품이 되도록 정렬

            (PieChart.Series[0] as PieSeries).ItemsSource = ProductRateList;

            if (ProductRateList.Count > 0) txtbox_Top1.Text = ProductRateList.ElementAt(0).Name;
            if (ProductRateList.Count > 1) txtbox_Top2.Text = ProductRateList.ElementAt(1).Name;
            if (ProductRateList.Count > 2) txtbox_Top3.Text = ProductRateList.ElementAt(2).Name;
        }

        /* 라인차트 생성 */
        private void LoadLineChart()
        {
            List<SaleSearch> SaleList = Db_salesearch.ReadAllSaleSearch();
            List<SaleSum> SumList = new List<SaleSum>();

            switch (mode)
            {
                case 'W':
                    for (int i = 0; i < 7; i++) // 최근 7일동안의 내역만 보여줌
                    {
                        SumList.Add(new SaleSum(DecreaseDate(i).Substring(3, 5), 0)); // MM-dd
                    }
                    break;
                case 'M':
                    for (int i = 0; i < 7; i++) // 최근 7달동안의 내역만 보여줌
                    {
                        SumList.Add(new SaleSum(DecreaseMonth(i), 0)); // yy-MM
                    }
                    break;
                case 'Y':
                    for (int i = 0; i < 7; i++) // 최근 7년동안의 내역만 보여줌
                    {
                        SumList.Add(new SaleSum((Convert.ToInt32(DateTime.Now.ToString("yy")) - i).ToString(), 0));
                    }
                    break;
            }

            foreach (SaleSearch h in SaleList)
            {
                switch (mode)
                {
                    case 'W':
                        foreach (SaleSum s in SumList)
                        {
                            if (s.Date.Equals(h.SaleTime.Substring(3, 5))) // MM-dd
                            {
                                s.Sum += h.Price;
                                break;
                            }
                        }
                        break;
                    case 'M':
                        foreach (SaleSum s in SumList)
                        {
                            if (s.Date.Equals(h.SaleTime.Substring(0, 5))) // yy-MM
                            {
                                s.Sum += h.Price;
                                break;
                            }
                        }
                        break;
                    case 'Y':
                        foreach (SaleSum s in SumList)
                        {
                            if (s.Date.Equals(h.SaleTime.Substring(0, 2))) // yy
                            {
                                s.Sum += h.Price;
                                break;
                            }
                        }
                        break;
                }
            }
            SumList.Reverse(); // 가장 최근 년도의 매출액이 마지막(오른쪽)에 오도록 정렬
            (LineChart.Series[0] as LineSeries).ItemsSource = SumList;

            if (mode == 'W')
                tb_todayAvg_value.Text = SumList.ElementAt(SumList.Count - 1).Sum.ToString();

            switch (mode)
            {
                case 'W':
                    int Sum = 0;
                    foreach (SaleSum s in SumList)
                        Sum += s.Sum;

                    tb_nowAvg_value.Text = Sum.ToString();
                    break;
                case 'M':
                case 'Y':
                    tb_nowAvg_value.Text = SumList.ElementAt(SumList.Count - 1).Sum.ToString();
                    break;
            }
        }

        public void POS_salestatus_Loaded(object sender, RoutedEventArgs e)
        {
            POS_SaleStatus_Refresh();
        }

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void POS_SaleStatus_Refresh()
        {
            LoadPieChart();
            LoadLineChart();
        }

        private void btn_Week_Click(object sender, RoutedEventArgs e)
        {
            mode = 'W';
            tb_nowAvg.Text = "최근 7일 매출액 총합";
            tb_select.Text = "선택한 날 매출액";
            tb_select_value.Text = "";
            POS_SaleStatus_Refresh();
        }

        private void btn_Month_Click(object sender, RoutedEventArgs e)
        {
            mode = 'M';
            tb_nowAvg.Text = "이번 달 매출액 총합";
            tb_select.Text = "선택한 달 매출액";
            tb_select_value.Text = "";
            POS_SaleStatus_Refresh();
        }

        private void btn_Year_Click(object sender, RoutedEventArgs e)
        {
            mode = 'Y';
            tb_nowAvg.Text = "이번 년도 매출액 총합";
            tb_select.Text = "선택한 년도 매출액";
            tb_select_value.Text = "";
            POS_SaleStatus_Refresh();
        }

        private void LineSeries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LineSeries ls = sender as LineSeries;
            if (ls.SelectedItem != null)
            {
                tb_select_value.Text = ((SaleSum)ls.SelectedItem).Sum.ToString();
            }
        }
    }
}
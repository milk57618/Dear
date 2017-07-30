using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_UWP.PosDB
{
    class SaleSearch
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int SaleCount { get; set; }

        public int Price { get; set; }

        public int Cash { get; set; }

        public int Card { get; set; }

        public int Id { get; set; } // 관리자 번호

        public string SaleTime { get; set; }

        public int Finished { get; set; } // 마감된 내역이면 1, 당일 개점한 후의 내역이면 0

        public SaleSearch() { }

        public SaleSearch(int Price, int Cash, int Card, int Id, string SaleTime)
        {
            SaleCount = 0;
            this.Price = Price;
            this.Cash = Cash;
            this.Card = Card;
            this.Id = Id;
            this.SaleTime = SaleTime;
            this.Finished = 0;
        }
    }
}

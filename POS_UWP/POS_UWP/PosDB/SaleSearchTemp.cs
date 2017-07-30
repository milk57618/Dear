using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_UWP.PosDB
{
    class SaleSearchTemp
    {
        public int SaleCount { get; set; }

        public int Price { get; set; }

        public int Cash { get; set; }

        public int Card { get; set; }

        public string Name { get; set; } // 관리자 이름

        public string SaleTime { get; set; }

        public SaleSearchTemp() { }

        public SaleSearchTemp(int SaleCount, int Price, int Cash, int Card, string Name, string SaleTime)
        {
            this.SaleCount = SaleCount;
            this.Price = Price;
            this.Cash = Cash;
            this.Card = Card;
            this.Name = Name;
            this.SaleTime = SaleTime;
        }
    }
}

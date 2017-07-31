using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_UWP.PosDB
{
    class SaleHistory : IComparable
    {
        [SQLite.AutoIncrement, SQLite.PrimaryKey]
        public int HistoryNumber { get; set; }

        public int ProductCount { get; set; }

        public string Name { get; set; }

        public string SaleTime { get; set; }

        public int OnePrice { get; set; }

        public int Count { get; set; }

        public int TotalPrice { get; set; }

        public SaleHistory() { }

        public SaleHistory(Sale sale, string time)
        {
            HistoryNumber = 0;
            ProductCount = sale.ProductCount;
            Name = sale.Name;
            OnePrice = sale.OnePrice;
            Count = sale.Count;
            TotalPrice = sale.TotalPrice;
            SaleTime = time;
        }

        public int CompareTo(object p)
        {
            return ((IComparable)ProductCount).CompareTo(((SaleHistory)p).ProductCount);
        }
    }
}

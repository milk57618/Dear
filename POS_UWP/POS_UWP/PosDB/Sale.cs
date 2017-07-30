using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * 상품 임시 판매 테이블
*/

namespace POS_UWP.PosDB
{
    public class Sale
    {   
        
        [SQLite.Unique]
        public int ProductCount { get; set; }

        [SQLite.PrimaryKey]
        public string Name { get; set; } 

        public int OnePrice { get; set; }

        public int Count { get; set; }

        public int TotalPrice { get; set; }  

        public Sale() { }
        public Sale(string name, int one, int cnt, int total, int proCnt)
        {
            ProductCount = proCnt;
            Name = name;
            OnePrice = one;
            Count = cnt;
            TotalPrice = total;
        }
    }
}

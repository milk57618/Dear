using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * 상품 메뉴 테이블
*/

namespace POS_UWP.PosDB
{
    public class Product
    {
        [SQLite.PrimaryKey]
        public int Id { get; set; }

        public string Category { get; set; }

        [SQLite.Unique]
        public string Name { get; set; }

        public string Price { get; set; }

        public Product() { }
        public Product(int id, string category, string name, string price)
        {
            Id = id;
            Category = category;
            Name = name;
            Price = price;
        }
    }
}

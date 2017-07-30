using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * 상품 카테고리 테이블
*/

namespace POS_UWP.PosDB
{
    public class Categorys
    {       
        [SQLite.PrimaryKey]
        public int Id { get; set; }

        [SQLite.Unique]
        public string category { get; set; }

        public Categorys() { }
        public Categorys(int id,string cate) { Id = id; category = cate; }
    }
}

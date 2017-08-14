using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * 직원들 일한 시간 저장하는 테이블
 */

namespace POS_UWP.PosDB
{
    public class MemberTime
    {
        [SQLite.PrimaryKey,SQLite.AutoIncrement]
        public int Id { get; set; }
        
        public string Date { get; set; }
        
        public string Name { get; set; }

        public string workStart { get; set; }

        public string workFinish { get; set; }

        public double workTime { get; set; }

        public MemberTime() { }

        public MemberTime(string Name,string Date,string workStart){

            this.Name = Name;
            this.Date = Date;
            this.workStart = workStart;
            this.workFinish = "0";
            this.workTime = 0;

        }


        //월별, 주별, 계속 축적할 디비 만들기
    }
}

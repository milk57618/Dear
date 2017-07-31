using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_UWP.PosDB
{
    public class Member
    {
        [SQLite.PrimaryKey]
        public int Id { get; set; }

        public string Price { get; set; }

        public string Name { get; set; }

        [SQLite.Unique]
        public string PhoneNumber { get; set; }

        public string StartTime { get; set; }

        public string FinishTime { get; set; }

        public string Position { get; set; }
        public Member() { }

        public Member(int Id, string name, string phone, string position, string price)
        {
            this.Id = Id;
            this.Name = name;
            PhoneNumber = phone;
            StartTime = DateTime.Now.ToString();
            Position = position;
            Price = price;

        }
    }
}
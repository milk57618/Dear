using POS_UWP.PosDB;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_UWP.DBConn
{

    class DBConn_MemberTime
    {

        SQLiteConnection dbConn;

        public DBConn_MemberTime()
        {
            dbConn = new SQLiteConnection(App.DB_PATH);
            dbConn.CreateTable<MemberTime>();
        }

        /*직원들의 작업시간(시작)을  저장한다.*/
        public void InsertMemberTime(MemberTime memT)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(memT);
                });
            }
        }

        /*직원들이 일이 끝났을 경우 끝난시간을 저장하고 총 작업시간을 계산해 디비 업데이트*/
        public void UpdateMemberTime(string name)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existing = dbConn.Query<MemberTime>("select * from MemberTime where Name =" + "'" + name + "'" + "and workFinish=0").FirstOrDefault();
                if (existing != null)
                {
                    existing.workFinish = DateTime.Now.ToString("yyyy/MM/dd HH:mm");

                    DateTime st = Convert.ToDateTime(existing.workStart);
                    DateTime fi = Convert.ToDateTime(existing.workFinish);

                    long convert = fi.Ticks - st.Ticks;
                    TimeSpan WT = new TimeSpan(convert);

                    existing.workTime = WT.TotalMinutes;

                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Update(existing);  //디비 업데이트함
                    });
                }
            }
        }

        /*직원들이 해당하는 날짜에 일한 시간을 모두 보여준다.*/
        public List<MemberTime> SearchMemberTime(string Name, string Date)
        {

            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {

                List<MemberTime> searchMemberTime = dbConn.Query<MemberTime>("select * from MemberTime where Name =" + "'" + Name + "'" + "and Date='" + Date + "'");

                return searchMemberTime;

            }
        }

        /*모든 멤버타임의 리스트를 반환*/
        public List<MemberTime> GetAllMemberTime()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<MemberTime> myCollection = dbConn.Table<MemberTime>().ToList<MemberTime>();
                return myCollection;
            }
        }

        /* MemberTime 데이터베이스 값들을 모두 삭제하는 함수 */
        public void DeleteAllMemberTime()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.DropTable<MemberTime>();
                dbConn.CreateTable<MemberTime>();
                dbConn.Dispose();
                dbConn.Close();
            }
        }


        /* 배열을 받아서 DB에 모두 입력 */
        public void InsertMemberTimeArray(MemberTime[] memberTime)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    foreach (MemberTime mt in memberTime)
                    {
                        dbConn.Insert(mt);
                    }
                });
            }
        }
    }
}
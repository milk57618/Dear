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
    class DBConn_SaleSearch
    {
        SQLiteConnection dbConn;
        public static int priceTemp = 0;

        public DBConn_SaleSearch()
        {
            dbConn = new SQLiteConnection(App.DB_PATH);
            dbConn.CreateTable<SaleSearch>();
        }

        //Retrieve the specific Search from the database;
        public SaleSearch ReadSaleSearch(int SaleCnt)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingCategory = dbConn.Query<SaleSearch>("select * from SaleSearch where SaleCount=" + SaleCnt).FirstOrDefault();
                return existingCategory;
            }
        }

        public List<SaleSearch> GetTodaySaleSearchList()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingCategory = dbConn.Query<SaleSearch>("select * from SaleSearch where Finished = 0");
                return existingCategory;
            }
        }

        public void Finish()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.Query<SaleSearch>("update SaleSearch set Finished = 1 where Finished = 0");
            }
        }

        //Retrieve the all Search list from the database
        public ObservableCollection<SaleSearch> ReadSaleSearchs()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<SaleSearch> myCollection = dbConn.Table<SaleSearch>().ToList<SaleSearch>();
                ObservableCollection<SaleSearch> CategorysList = new ObservableCollection<SaleSearch>(myCollection);
                return CategorysList;
            }
        }

        public List<SaleSearch> ReadAllSaleSearch()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<SaleSearch> myCollection = dbConn.Table<SaleSearch>().ToList<SaleSearch>();
                return myCollection;
            }
        }

        public ObservableCollection<SaleSearchTemp> GetList()
        {
            List<SaleSearch> list = GetTodaySaleSearchList();
            ObservableCollection<SaleSearchTemp> result = new ObservableCollection<SaleSearchTemp>();
            DBConn_Member m = new DBConn_Member();
            foreach (SaleSearch s in list)
            {
                result.Add(new SaleSearchTemp(s.SaleCount, s.Price, s.Cash, s.Card, m.GetName(s.Id), s.SaleTime));
            }
            return result;
        }

        public void InsertSaleSearch(SaleSearch newsale)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(newsale);
                });
            }
        }

        //Delete specific contact
        public void DeleteSaleSearch(int SaleCnt)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existing = dbConn.Query<SaleSearch>("select * from SaleSearch where SaleCount= " + SaleCnt).FirstOrDefault();
                priceTemp += existing.Price;
                if (existing != null)
                {
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Delete(existing);  //해당하는 salecount 삭제
                    });
                }

            }
        }

        public void DeleteAllSaleSearch()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.DropTable<SaleSearch>();
                dbConn.CreateTable<SaleSearch>();
                dbConn.Dispose();
                dbConn.Close();
            }
        }
    }
}

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
    class DBConn_SaleHistory
    {
        SQLiteConnection dbConn;

        public DBConn_SaleHistory()
        {
            dbConn = new SQLiteConnection(App.DB_PATH);
            dbConn.CreateTable<SaleHistory>();
        }

        //Retrieve the specific Category from the database;
        public List<SaleHistory> ReadSaleHistoryList(string SaleTime)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingCategory = dbConn.Query<SaleHistory>("select * from SaleHistory where SaleTime = \'" + SaleTime + "\'");
                return existingCategory;
            }
        }

        //Retrieve the all contact list from the database
        public List<SaleHistory> ReadAllSaleHistory()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<SaleHistory> myCollection = dbConn.Table<SaleHistory>().ToList<SaleHistory>();
                return myCollection;
            }
        }

        public void InsertSaleHistory(SaleHistory newsale)
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
        public void DeleteSaleHistory(string SaleTime)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.Query<SaleHistory>("delete from SaleHistory where SaleTime = \'" + SaleTime + "\'");
            }
        }

        public void DeleteAllSaleHistory()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.DropTable<SaleHistory>();
                dbConn.CreateTable<SaleHistory>();
                dbConn.Dispose();
                dbConn.Close();
            }
        }
    }
}
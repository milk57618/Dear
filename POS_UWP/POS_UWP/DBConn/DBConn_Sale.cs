using SQLite;
using System;
using POS_UWP.PosDB;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;


namespace POS_UWP.DBConn
{
    class DBConn_Sale
    {
        /*Sale 테이블 리스트 목록을 불러오는 함수*/
        public Sale ReadSales(string SaleName)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingSale = dbConn.Query<Sale>("select * from Sale where Name = '" + SaleName + "'").FirstOrDefault();
                return existingSale;
            }
        }

        /*Sale 데이터베이스 값 삽입하는 함수*/
        public void InsertSale(Sale newSale)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(newSale);
                });
            }
        }

        /*Sale 데이터베이스 필드인 수량을 증가시키는 함수*/
        public void PlusSale(Sale sale)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existing = dbConn.Query<Sale>("select * from Sale where Name ='" + sale.Name + "'").FirstOrDefault();
                if (existing != null)
                {
                    existing.Count = sale.Count + 1;
                    existing.TotalPrice = sale.OnePrice * (sale.Count + 1);

                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Update(existing);  //디비 업데이트함
                    });
                }
            }
        }

        /*Sale 데이터베이스 필드인 수량을 감소시키는 함수*/
        public void MinusSale(Sale sale)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existing = dbConn.Query<Sale>("select * from Sale where Name ='" + sale.Name + "'").FirstOrDefault();
                int count = dbConn.Query<Sale>("select * from Sale").Count;
                if (existing != null)
                {
                    if (sale.Count - 1 > 0)
                    {
                        existing.Count = sale.Count - 1;
                        existing.TotalPrice = sale.OnePrice * (sale.Count - 1);

                        dbConn.RunInTransaction(() =>
                        {
                            dbConn.Update(existing);  //값 업데이트함
                        });
                    }
                    else
                    {
                        dbConn.RunInTransaction(() =>
                        {
                            dbConn.Delete(existing);  //값 삭제함
                        });
                        for (int i = 0; i < count; i++)
                        {
                            int j = sale.ProductCount + i;
                            int k = j + 1;
                            dbConn.Query<Sale>("update Sale set ProductCount= " + j + " WHERE ProductCount= " + k); //아이디 값 맞춰주는 코드
                        }
                    }
                }
            }
        }


        /*Sale 테이블 리스트 목록을 확인하는 함수*/
        public ObservableCollection<Sale> ReadSales()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<Sale> myCollection = dbConn.Table<Sale>().ToList<Sale>();
                ObservableCollection<Sale> SalesList = new ObservableCollection<Sale>(myCollection);
                return SalesList;
            }
        }

        /*Sale 데이터베이스 값을 삭제하는 함수*/
        public void DeleteSale(int salename)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {

                var existingMember = dbConn.Query<Sale>("select * from Sale where ProductCount=" + salename).FirstOrDefault();
                int count = dbConn.Query<Sale>("select * from Sale").Count;
                if (existingMember != null)
                {
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Delete(existingMember);  //값 삭제함
                    });

                    for (int i = 0; i < count; i++)
                    {
                        int j = salename + i;
                        int k = j + 1;
                        dbConn.Query<Sale>("update Sale set ProductCount= " + j + " WHERE ProductCount= " + k); //아이디 값 맞춰주는 코드
                    }
                }

            }
        }

        /*Sale 데이터베이스 필드인 Name이 중복되는지 확인하는 함수*/
        public bool OverlapSearch(string name)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var value = dbConn.Query<Sale>("select * from Sale where Name= '" + name + "'").FirstOrDefault();
                if (value != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /*Sale 데이터베이스 필드인 금액을 합하는 함수*/
        public string CalculateAll()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var tableValue = dbConn.Table<Sale>();
                int sum = 0;

                foreach (var Col in tableValue)
                {
                    sum += Col.TotalPrice;
                }

                return sum.ToString();
            }
        }

        /*Sale 데이터베이스 값들을 모두 삭제하는 함수*/
        public void DeleteAllSale()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.DropTable<Sale>();
                dbConn.CreateTable<Sale>();
                dbConn.Dispose();
                dbConn.Close();
            }
        }

        /*번호 순서대로 바꾸는 함수*/
        public int SetId()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                int count = dbConn.Query<Sale>("select * from Sale").Count;
                return count + 1;   //추가시 아이디 정해주는 함수

            }
        }
    }
}

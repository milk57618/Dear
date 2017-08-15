using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SQLite;
using POS_UWP.PosDB;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS_UWP;

namespace POS_UWP.DBConn
{
    class DBConn_Product
    {
        /*Product 테이블 리스트 목록을 불러오는 함수*/
        public Product ReadProducts(int proId)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingProduct = dbConn.Query<Product>("select * from Product where Id =" + proId).FirstOrDefault();
                return existingProduct;
            }
        }

        /*번호 순서대로 바꾸는 함수*/
        public int SetId()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                int count = dbConn.Query<Product>("select * from Product").Count;
                return count + 1;   //추가시 아이디 정해주는 함수
            }
        }

        /*Product 데이터베이스 값 삽입하는 함수*/
        public void InsertProduct(Product newpro)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(newpro);
                });
            }
        }

        /*Product 데이터베이스 값 업데이트하는 함수*/
        public void UpdateProduct(Product pro)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existing = dbConn.Query<Product>("select * from Product where Id=" + pro.Id).FirstOrDefault();
                if (existing != null)
                {
                    existing.Price = pro.Price; //바꿀 카테고리 값을 원래 있던 카테고리값에 넣어줌
                    existing.Name = pro.Name;
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Update(existing);  //디비 업데이트함
                    });
                }
            }
        }

        /*Product 데이터베이스의 값이 존재하는지 확인하는 함수*/
        public bool SearchProduct(string proname)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existing = dbConn.Query<Categorys>("select * from Product where name='" + proname + "'").FirstOrDefault();
                if (existing != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /*Product 테이블 리스트 목록을 불러오는 함수*/
        public ObservableCollection<Product> ReadProducts()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<Product> myCollection = dbConn.Table<Product>().ToList<Product>();
                ObservableCollection<Product> ProductsList = new ObservableCollection<Product>(myCollection);
                return ProductsList;
            }
        }

        /*카테고리 별 Product 테이블 리스트 목록을 불러오는 함수*/
        public ObservableCollection<Product> GetProductForAssessment(string cate)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<Product> myCollection = dbConn.Query<Product>("select * from Product where category='" + cate + "'");

                ObservableCollection<Product> ProductList = new ObservableCollection<Product>(myCollection);
                return ProductList;
            }
        }

        /*모든 Product 테이블 리스트 목록을 불러오는 함수*/
        public List<Product> GetAllProduct()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<Product> myCollection = dbConn.Query<Product>("select * from Product");

                return myCollection;
            }
        }

        /*Product 데이터베이스 값 삭제하는 함수*/
        public void DeleteProduct(int proId)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingproduct = dbConn.Query<Product>("select * from Product where Id= " + proId).FirstOrDefault();
                int count = dbConn.Query<Product>("select * from Product").Count;
                if (existingproduct != null)
                {
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Delete(existingproduct);  //카테고리가 존재하면 카테고리 삭제해줌
                    });
                }
                for (int i = 0; i < count; i++)
                {
                    int j = proId + i;
                    int k = j + 1;
                    dbConn.Query<Categorys>("update Product set Id= " + j + " WHERE Id= " + k); //아이디 값 맞춰주는 코드
                }

            }
        }

        /* Product 데이터베이스 값들을 모두 삭제하는 함수 */
        public void DeleteAllProduct()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.DropTable<Product>();
                dbConn.CreateTable<Product>();
                dbConn.Dispose();
                dbConn.Close();
            }
        }

        /* 배열을 받아서 DB에 모두 입력 */
        public void InsertProductArray(Product[] product)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    foreach (Product p in product)
                    {
                        dbConn.Insert(p);
                    }
                });
            }
        }
    }
}

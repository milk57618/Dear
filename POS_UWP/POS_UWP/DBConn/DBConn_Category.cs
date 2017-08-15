using SQLite;
using POS_UWP.PosDB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace POS_UWP.DBConn
{
    public class DBConn_Category
    {
        /*카테고리 테이블 리스트 목록을 불러오는 함수*/
        public Categorys ReadCategorys(int CateId)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingCategory = dbConn.Query<Categorys>("select * from Categorys where Id=" + CateId).FirstOrDefault();
                return existingCategory;
            }
        }

        /*번호 순서대로 바꾸는 함수*/
        public int SetId()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                int count = dbConn.Query<Categorys>("select * from Categorys").Count;
                return count + 1;   //추가시 아이디 정해주는 함수
            }
        }

        /*카테고리 테이블 리스트 목록을 불러오는 함수*/
        public ObservableCollection<Categorys> ReadCategorys()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<Categorys> myCollection = dbConn.Table<Categorys>().ToList<Categorys>();
                ObservableCollection<Categorys> CategorysList = new ObservableCollection<Categorys>(myCollection);
                return CategorysList;
            }
        }

        /*카테고리 테이블 리스트 목록을 불러오는 함수*/
        public List<Categorys> GetAllCategory()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<Categorys> myCollection = dbConn.Table<Categorys>().ToList<Categorys>();
                return myCollection;
            }
        }

        /*카테고리 데이터베이스 업데이트*/
        public void UpdateCategory(Categorys cate)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existing = dbConn.Query<Categorys>("select * from Categorys where Id =" + cate.Id).FirstOrDefault();
                if (existing != null)
                {
                    existing.Category = cate.Category;  //바꿀 카테고리 값을 원래 있던 카테고리값에 넣어줌
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Update(existing);  //디비 업데이트함
                    });
                }
            }
        }
        

        /*카테고리 데이터베이스 값 존재하는지 확인하는 함수*/
        public bool SearchCategory(string cate)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existing = dbConn.Query<Categorys>("select * from Categorys where category= '" + cate + "'").FirstOrDefault();
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

        /*카테고리 데이터베이스 값 삽입하는 함수*/
        public void InsertCategory(Categorys newcate)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    dbConn.Insert(newcate);
                });
            }
        }

        /*카테고리 데이터베이스 값 삭제하는 함수*/
        public void DeleteCategory(Categorys cate)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingCate = dbConn.Query<Categorys>("select * from Categorys where category= '" + cate.Category + "'").FirstOrDefault();
                int count = dbConn.Query<Categorys>("select * from Categorys").Count;

                if (existingCate != null)
                {
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Query<Categorys>("delete from Product where category= '" + cate.Category + "'");
                        dbConn.Delete(existingCate);  //카테고리가 존재하면 카테고리 삭제해줌
                    });
                }

                for (int i = 0; i < count; i++)
                {
                    int j = cate.Id + i;
                    int k = j + 1;
                    dbConn.Query<Categorys>("update Categorys set Id= " + j + " WHERE Id= " + k); //아이디 값 맞춰주는 코드
                }
            }
        }

        /* Category 데이터베이스 값들을 모두 삭제하는 함수 */
        public void DeleteAllCategory()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.DropTable<Categorys>();
                dbConn.CreateTable<Categorys>();
                dbConn.Dispose();
                dbConn.Close();
            }
        }

        /* 배열을 받아서 DB에 모두 입력 */
        public void InsertCategoryArray(Categorys[] category)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    foreach (Categorys c in category)
                    {
                        dbConn.Insert(c);
                    }
                });
            }
        }
    }
}
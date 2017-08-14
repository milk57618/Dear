using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SQLite;
using POS_UWP.PosDB;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_UWP.DBConn
{
    class DBConn_Member
    {
        SQLiteConnection dbConn;

        public DBConn_Member()
        {
            dbConn = new SQLiteConnection(App.DB_PATH);
            dbConn.CreateTable<Member>();
        }

        /*정보수정을 통해 디비를 업데이트 하는 함수*/
        public void UpdateMember(String Name, String Phone, string posi, int Id, string Pay)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existing = dbConn.Query<Member>("select * from Member where Id =" + Id).FirstOrDefault();
                if (existing != null)
                {

                    /*바꿀 카테고리 값을 원래 있던 카테고리값에 넣어줌*/

                    existing.Name = Name;
                    existing.PhoneNumber = Phone;
                    existing.Position = posi;
                    existing.Price = Pay;
                    dbConn.RunInTransaction(() =>
                    {
                        dbConn.Update(existing);
                    });
                }
            }
        }

        /*이름 중복 방지를 위해 이름을 검색하는 기능*/
        public string GetName(int id)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existing = dbConn.Query<Member>("select Name from Member where Id=" + id).FirstOrDefault();
                if (existing != null)
                {
                    return existing.Name;
                }
                else
                {
                    return "error";
                }
            }
        }

        /*멤버 추가 함수*/
        public void InsertMember(Member mem)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {

                    dbConn.Insert(mem);
                });
            }
        }

        /*멤버 검색 함수*/
        public bool SearchMember(Member mem)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existing = dbConn.Query<Member>("select * from Member where Id=" + mem.Id).FirstOrDefault();
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

        /*정보 수정시 핸드폰 번호 그대로 쓸 수 있도록 자신의 폰번호 찾아서 리턴*/
        public String Searchselected(int id)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existing = dbConn.Query<Member>("select * from Member where Id=" + id).FirstOrDefault();
                if (existing != null)
                {

                    return existing.PhoneNumber;
                }
                else
                {
                    return "error";
                }
            }

        }

        /*정보 수정시 이름 그대로 쓸 수 있도록 클릭한 지점의 이름을 찾아서 리턴*/
        public String SearchName(int id)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existing = dbConn.Query<Member>("select * from Member where Id=" + id).FirstOrDefault();
                if (existing != null)
                {

                    return existing.Name;
                }
                else
                {
                    return "error";
                }
            }

        }

        public int SetId()
        {
            int count = dbConn.Query<Member>("select * from Member").Count;
            return count + 1;   /*추가시 아이디 정해주는 함수*/
        }


        /*핸드폰 번호 중복 방지를 위해 검색*/
        public bool SearchPhoneNumber(String s)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existing = dbConn.Query<Member>("select * from Member where PhoneNumber='" + s + "'").FirstOrDefault();
                if (existing != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /*이름 중복방지를 위해 검색*/
        public bool SearchName(String name)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existing = dbConn.Query<Member>("select * from Member where Name='" + name + "'").FirstOrDefault();
                if (existing != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        /*리스트 뷰에 보여주기위해 디비에서 추출*/
        public ObservableCollection<Member> ReadMembers()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<Member> myCollection = dbConn.Table<Member>().ToList<Member>();
                ObservableCollection<Member> MembersList = new ObservableCollection<Member>(myCollection);
                return MembersList;
            }
        }
        
        /*모든 멤버의 리스트를 반환*/
        public List<Member> GetAllMember()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<Member> myCollection = dbConn.Table<Member>().ToList<Member>();
                return myCollection;
            }
        }

        /*멤버 삭제함수*/
        public void DeleteMember(int memId)

        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                var existingMember = dbConn.Query<Member>("select * from Member where Id=" + memId).FirstOrDefault();
                int count = dbConn.Query<Member>("select * from Member").Count;
                int a = count;

                if (existingMember != null)
                {
                    dbConn.RunInTransaction(() =>
                    {
                        string name = GetName(memId);
                        dbConn.Query<MemberTime>("delete from MemberTime where Name = '" + name + "'");
                        dbConn.Delete(existingMember);  //카테고리가 존재하면 카테고리 삭제해줌
                    });


                    for (int i = 0; i < count; i++)
                    {
                        int j = memId + i;
                        int k = j + 1;
                        dbConn.Query<Member>("update Member set Id = " + j + " WHERE Id = " + k); //아이디 값 맞춰주는 코드
                    }
                }
            }
        }

        /*직위 별로 멤버들을 뽑아서 추출한다.*/
        public ObservableCollection<Member> GetMemberForPosi(string cate)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                List<Member> myCollection = dbConn.Query<Member>("select * from Member where Position=" + "'" + cate + "'");

                ObservableCollection<Member> MemberList = new ObservableCollection<Member>(myCollection);

                return MemberList;
            }
        }

        /* Member 데이터베이스 값들을 모두 삭제하는 함수 */
        public void DeleteAllMember()
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.DropTable<Member>();
                dbConn.CreateTable<Member>();
                dbConn.Dispose();
                dbConn.Close();
            }
        }

        /* 배열을 받아서 DB에 모두 입력 */
        public void InsertMemberArray(Member[] member)
        {
            using (var dbConn = new SQLiteConnection(App.DB_PATH))
            {
                dbConn.RunInTransaction(() =>
                {
                    foreach (Member m in member)
                    {
                        dbConn.Insert(m);
                    }
                });
            }
        }
    }
}
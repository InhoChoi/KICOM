using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FaceAPI
{
    /*
     *  Database 접근 관련 클래스
     *  SQLite 데이터 접근을 위한 Reference 추가 필요
     *  기본적인 자료 구조체 - Person class [Name, ImgName, Relation, Etc, Face ID] 을 사용함
     *
     *  Private createDB - DB 파일 존재 유무 확인 및 생성, DB 테이블 생성
     *  Public delete(string) - name가 일치되는 DB 데이터 삭제
     *  Public deleteAll - 모든 DB 데이터 삭제
     *  Public insert(Person) - DB 데이터 객체 추가
     *  Public insert(string....) - DB 데이터 string 으로 추가
     *  Public select() - DB안에 있는 데이터 값 얻기 위한 메서드
     *  Private query() - qeury문 실행하기 위한 private 매서드
     */

    class DBManagement
    {
        //DB 저장될 경로
        private const string dbname = @"db.db";
        private string dbPath = null;
        private string strConn = null;

        //생성자 - 셋팅값이 없을 경우
        public DBManagement()
        {
            this.dbPath = @".\db.db";
            this.strConn = @"Data Source=.\db.db";
            createDB();
        }

        //생성자 dbPath를 매개변수를 작동될경우
        public DBManagement(string dbSavePath)
        {
            this.dbPath = Path.Combine(dbSavePath, dbname);
            this.strConn = string.Format(@"Data Source={0}", this.dbPath);
            createDB();
        }

        //DB FILE 생성 및 TABLE 생성
        private void createDB()
        {
            //DB FILE 존재 유무 확인
            if (!System.IO.File.Exists(this.dbPath))
            {
                //DB 파일 생성
                SQLiteConnection.CreateFile(this.dbPath);

                //테이블 생성 query 문
                string sql = "CREATE TABLE IF NOT EXISTS [Person](";
                sql += " [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, ";
                sql += " [Name] TEXT NOT NULL, ";
                sql += " [ImgName] TEXT NOT NULL, ";    
                sql += " [Relation] TEXT NOT NULL, ";
                sql += " [ETC] TEXT";
                sql += ");";

                this.query(sql);
            }
        }

        // DB 내용 삭제 - 이름으로 삭제
        public void delete(string name)
        {
            string sql = "DELETE FROM Person WHERE Name='";
            sql += name;
            sql += "';";
            this.query(sql);
        }

        // DB 안에 있는 내용 모두 삭제
        public void deleteAll()
        {
            string sql = "DELETE FROM Person";
            this.query(sql);
        }

        //Insert 오버로딩 - Person 클래스 객체 이용
        public void insert(Person person)
        {
            this.insert(person.name, person.imgname, person.relation, person.etc);
        }

        // DB 내용 추가
        public void insert(string name, string imgname, string relation, string etc)
        {
            string sql = "INSERT INTO Person (Name,ImgName,Relation,ETC) VALUES(";
            sql += "'" + name + "',";
            sql += "'" + imgname + "',";
            sql += "'" + relation + "',";
            sql += "'" + etc + "');";

            this.query(sql);
        }

        //DB의 값을 얻기 위한 매서드
        public Person[] select()
        {
            List<Person> list = new List<Person>();
            using (var conn = new SQLiteConnection(this.strConn))
            {
                conn.Open();
                string sql = "SELECT * FROM Person";

                //SQLiteDataReader를 이용하여 연결 모드로 데이타 읽기
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                SQLiteDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string name = rdr["Name"].ToString();
                    string imgname = rdr["ImgName"].ToString();
                    string relation = rdr["Relation"].ToString();
                    string etc = rdr["ETC"].ToString();

                    Person person = new Person(name,imgname,relation,etc);
                    list.Add(person);
                }
                rdr.Close();
            }

            return list.ToArray();
        }

        private void query(string sql)
        {
            try
            {
                //sql command가 null이 아닐경우
                if (sql != null)
                {
                    using (SQLiteConnection conn = new SQLiteConnection(this.strConn))
                    {
                        conn.Open();
                        SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }
    }
}

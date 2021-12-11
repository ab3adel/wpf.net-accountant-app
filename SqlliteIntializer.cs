using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace accountant
{
  public  class SqlliteIntializer
    {
        SQLiteConnection dbConnection;
        SQLiteCommand command;
        string sqlCommand;
        string dbPath = System.Environment.CurrentDirectory + "\\DB";
        string dbFilePath { get; set; }
        public void createDbFile()
        {
            if (!string.IsNullOrEmpty(dbPath) && !Directory.Exists(dbPath))
                Directory.CreateDirectory(dbPath);
            this.dbFilePath = dbPath + "\\accountatn.db";
            if (!System.IO.File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);
            }
        }
        public string ConnString ()
        {
            createDbFile();
            string strCon = string.Format("Data Source={0};", dbFilePath);
            return strCon;
        }
       
        public void delete (string tableName)
        {
            sqlCommand = $@"DROP TABLE IF EXISTS {tableName};";
           using (SQLiteConnection conn = new SQLiteConnection(ConnString()))
            {
                conn.Open();
                SQLiteCommand command = new SQLiteCommand(sqlCommand,conn);
               bool done = command.ExecuteNonQuery() > 0;
                conn.Close();
            }
            
        }
        public  void createTables(string tableName)
        {
            sqlCommand = @"CREATE TABLE IF NOT EXISTS  Authentication (
                                                Id INTEGER PRIMARY KEY 
                                                 ,UserName TEXT(25) NOT NULL UNIQUE
                                                  ,Password TEXT(25) NOT NULL
                                                  );
";
            using (SQLiteConnection conn = new SQLiteConnection(ConnString()))
            {
                conn.Open();
                SQLiteCommand command = new SQLiteCommand(sqlCommand, conn);
                bool done = command.ExecuteNonQuery() > 0;
                
                conn.Close();
            }

            sqlCommand = @"CREATE TABLE IF NOT EXISTS  Store (
                                                Id INTEGER PRIMARY KEY 
                                                 ,ProductName TEXT(25) NOT NULL UNIQUE
                                                  ,ProductId TEXT(25) NOT NULL UNIQUE
                                                  ,ProductNumber INTERGER NOT NULL DEFAULT 0
                                                   ,Price  INTERGER NOT NULL DEFAULT 0  
                                                   ,SellingPrice INTERGER NOT NULL DEFAULT 0
                                                    ,InsertedDate DATETIME NULL);
";
               using (SQLiteConnection conn = new SQLiteConnection(ConnString()))
            {
                conn.Open();
                SQLiteCommand command = new SQLiteCommand(sqlCommand, conn);
                bool done =  command.ExecuteNonQuery() > 0;
               
                conn.Close();
            }
            
           
                sqlCommand = @"CREATE TABLE IF NOT EXISTS  Selling(Id INTEGER PRIMARY KEY 
                                                 ,ProductName TEXT(25) NOT NULL 
                                                  ,ProductId TEXT(25) NOT NULL 
                                     
                                                   ,Number  INTERGER NOT NULL DEFAULT 0  
                                                   ,Gain  INTERGER NOT NULL DEFAULT 0
                                                   ,SellingPrice INTERGER NOT NULL DEFAULT 0
                                                    ,Inserted DATETIME NULL);";
            using (SQLiteConnection conn = new SQLiteConnection(ConnString()))
            {
                conn.Open();
                SQLiteCommand command = new SQLiteCommand(sqlCommand, conn);
                bool done = command.ExecuteNonQuery() > 0;
              
                conn.Close();
            }

            sqlCommand = @"CREATE TABLE IF NOT EXISTS  Operation(Id INTEGER PRIMARY KEY 
                                                 ,Name TEXT(25) NOT NULL  
                                                  ,Type VARCHAR NOT NULL 
                                                     ,ExpenseSource VARCHAR NOT NULL                                                
                                                    ,Hidden INTERGER NOT NULL 
                                                   ,MoneyAmmount  INTERGER NOT NULL DEFAULT 0  
                                                   ,CostPrice INTERGER NOT NULL DEFAULT 0
                                                    ,Inserted DATETIME NULL);";
            using (SQLiteConnection conn = new SQLiteConnection(ConnString()))
            {
                conn.Open();
                SQLiteCommand command = new SQLiteCommand(sqlCommand, conn);
                bool done = command.ExecuteNonQuery() > 0;
                
                conn.Close();
            }


            sqlCommand = @"CREATE TABLE IF NOT EXISTS  Box(Id INTEGER PRIMARY KEY 
                                                 ,BoxName TEXT(25) NOT NULL UNIQUE  
                                                   ,TotallAmmount  INTERGER NOT NULL DEFAULT 0  
                                                   ,TotallGain INTERGER NOT NULL DEFAULT 0
                                                    ,Created DATETIME NULL);";
            using (SQLiteConnection conn = new SQLiteConnection(ConnString()))
            {
                conn.Open();
                SQLiteCommand command = new SQLiteCommand(sqlCommand, conn);
                bool done = command.ExecuteNonQuery() > 0;
               if (!done)
                {
                    string txtcomman = "SELECT * FROM Box WHERE BoxName='Store';";
                     command = new SQLiteCommand(txtcomman, conn);
                    SQLiteDataReader reader = command.ExecuteReader() ;
                    if (!reader.Read())
                    {
                        txtcomman = @"INSERT INTO Box  (BoxName,TotallAmmount,TotallGain,Created)
                                                          VALUES ('Store',0,0,DATETIME('now'));";
                        command = new SQLiteCommand(txtcomman, conn);
                        bool ok = command.ExecuteNonQuery() > 0;
                        

                    }
                }
                conn.Close();
            }

        }

        public bool checkIfExist(string tableName)
        {
            
            command.CommandText = "SELECT name FROM sqlite_master WHERE name='" + tableName + "'";
            var result = command.ExecuteScalar();
            Trace.WriteLine($" tableName{tableName} {result}");
            return result != null && result.ToString() == tableName ? true : false;
        }
        
        public bool checkIfTableContainsData(string tableName)
        {
            command.CommandText = "SELECT count(*) FROM " + tableName;
            var result = command.ExecuteScalar();

            return Convert.ToInt32(result) > 0 ? true : false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace accountant
{
   public class Authentication
    {
        private string UserNameValue = string.Empty;
        private string PasswordValue = string.Empty;
        public string UserName {
            get
            {
                return this.UserNameValue;
            }
            set
            {
                this.UserNameValue = value;
                NotifyPropertyChanged("UserName");
            }
               
               }
        public string Password
        {
            get
            {
                return this.PasswordValue;
            }
            set
            {
                this.PasswordValue = value;
                NotifyPropertyChanged("Password");
            }
        }
        public int ID { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
       
        public static string CheckAny()

        {
            SqlliteIntializer w = new SqlliteIntializer();
            string user = "";
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(w.ConnString()))
                {
                    conn.Open();
                    string commandText = @"SELECT UserName  FROM Authentication LIMIT 1;";
                    SQLiteCommand command = new SQLiteCommand(commandText, conn);
                    SQLiteDataReader data = command.ExecuteReader();
                        while (data.Read())
                    {
                        user = data.GetString(0);
                    }
                    conn.Close();
                }

            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                return user;
            }

            return user;
        }
       
        public static Authentication CheckAuthByName(string userName)
        {
            SqlliteIntializer init = new SqlliteIntializer();
            Authentication auth = null;
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(init.ConnString()))
                {
                    conn.Open();
                    string commandText = @"SELECT UserName,Password,Id FROM Authentication WHERE UserName=@userName;";
                    SQLiteCommand command = new SQLiteCommand(commandText, conn);
                    command.Parameters.AddWithValue("@userName", userName);
               
                    SQLiteDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        auth = new Authentication()
                        {
                            UserName = dataReader.GetString(0),
                            Password = dataReader.GetString(1),
                            ID = dataReader.GetInt32(2)

                        };

                    }
                    dataReader.Close();
                    conn.Close();
                }

            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
                return null;
            }

            return auth;
        }
        public static int CheckAuth(Authentication auth)
        {

            SqlliteIntializer w = new SqlliteIntializer();
            int number = -1;
            
            if (!string.IsNullOrEmpty(auth.UserName) && !string.IsNullOrEmpty(auth.Password))
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(w.ConnString()))
                    {
                        conn.Open();
                        string commandText = @"SELECT Id  FROM Authentication 
                                                            WHERE UserName=@userName 
                                                             AND Password=@password;";
                        SQLiteCommand command = new SQLiteCommand(commandText, conn);
                        command.Parameters.AddWithValue("@userName", auth.UserName);
                        command.Parameters.AddWithValue("@password", auth.Password);
                        SQLiteDataReader data = command.ExecuteReader();
                        while (data.Read())
                        {
                            number = data.GetInt32(0);
                        }
                        data.Close();
                        conn.Close();


                    }
                   
                    

                }
                catch (Exception e)
                {
                    
                    return -1;
                }
            }

            return number;
        }
        public static bool UpdateAuth (Authentication auth)
        {
            SqlliteIntializer w = new SqlliteIntializer();
            bool isUpdated = false;
            using (SQLiteConnection conn = new SQLiteConnection(w.ConnString()))
            {
                conn.Open();

                string txtCommand = @"UPDATE Authentication SET 
                                           UserName=@userName,Password=@password             
                                         WHERE Id=@Id ;";


                SQLiteCommand command = new SQLiteCommand(txtCommand, conn);
                command.Parameters.AddWithValue("@userName", auth.UserName);
                command.Parameters.AddWithValue("@password", auth.Password);
                command.Parameters.AddWithValue("@Id", auth.ID);
                isUpdated = command.ExecuteNonQuery() > 0;
                conn.Close();
            }
         
           return isUpdated;
            
        }
        public static bool SetAuth(Authentication auth)
        {
            SqlliteIntializer w = new SqlliteIntializer();
            bool done = false;
            if (!string.IsNullOrEmpty(auth.UserName) && !string.IsNullOrEmpty(auth.Password))
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(w.ConnString()))
                    {
                        conn.Open();
                        string commandText = @"INSERT INTO Authentication (UserName,Password)
                                        VALUES(@userName,@password);";
                        SQLiteCommand command = new SQLiteCommand(commandText, conn);
                        command.Parameters.AddWithValue("@userName", auth.UserName);
                        command.Parameters.AddWithValue("@password", auth.Password);
                        done = command.ExecuteNonQuery() > 0;
                        conn.Close();
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e);
                    return false;

                }
            }

            return done;
        }
        public static bool DeleteAll()
        {
            SqlliteIntializer w = new SqlliteIntializer();
            bool done = false;
            using (SQLiteConnection conn = new SQLiteConnection(w.ConnString()))
            {
                conn.Open();
                string txtCommand = @"DELETE  FROM Authentication ; ";
                SQLiteCommand command = new SQLiteCommand(txtCommand, conn);
                done = command.ExecuteNonQuery() > 0;
                conn.Close();
            }
            return done;
        }
        public static List<Authentication> GetAllAuth()
        {
            SqlliteIntializer w = new SqlliteIntializer();
            List<Authentication> lst = new List<Authentication>();
            using (SQLiteConnection conn = new SQLiteConnection(w.ConnString()))
            {
                conn.Open();
                string commandText = @"SELECT *  FROM Authentication 
                                                           ;";
                SQLiteCommand command = new SQLiteCommand(commandText, conn);
                
                SQLiteDataReader data = command.ExecuteReader();
                while (data.Read())
                {
                    lst.Add(new Authentication()
                    {
                        UserName = data.GetString(1),
                        ID = data.GetInt32(0),
                        Password = data.GetString(2)
                    });
                }
                data.Close();
                conn.Close();


            }
            return lst;
        }
    }
}

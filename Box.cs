using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Data.SQLite;
namespace accountant
{
    public class Box :INotifyPropertyChanged
    {
        private string BoxNameValue = String.Empty;
        private int TotallAmmountValue = 0;
        private int TotallGainValue = 0;
        private int TotallCostValue = 0;

        public int ID { get; set; }
        public string BoxName {
            get
            {
                return this.BoxNameValue;
            }
            set
            {
                this.BoxNameValue = value;
                NotifyPropertyChanged("BoxName");
            }
        }
        public DateTime Created { set; get; }
        public int TotallAmmount
        {
            get
            {
                return this.TotallAmmountValue;
            }
            set
            {
                if (int.TryParse(value.ToString(),out int v))
                {
                    this.TotallAmmountValue = v;
                    NotifyPropertyChanged("TotallAmmount");
                }
                
            }
        }
        public int TotallGain { 
            get
            {
                return this.TotallGainValue;
            }
            set
            {
                if (int.TryParse(value.ToString(), out int v))
                {
                    this.TotallGainValue = v;
                    NotifyPropertyChanged("TotallGain");
                }
            }
        }
        public int TotallCost { 
            get
            {
                return this.TotallCostValue;
            }
            set
            {
                if ((int)value >= 0)
                {
                    this.TotallCostValue = value;
                    NotifyPropertyChanged("TotallCost");
                }
            }
        }
        public int ExtraField { get; set; }
       
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this,new PropertyChangedEventArgs(propertyName));
            }
        }
        public override string ToString ()
        {
            if (BoxName != null) return BoxName;
            return "";
           

        }
        private SqlliteIntializer sql;
         
        public Box ()
        {

        }
       
        public static bool SetBox(Box box)
        {
            SqlliteIntializer v = new SqlliteIntializer();
            bool done = false;
            

            if (box.BoxName != null)
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(v.ConnString()))
                    {
                        conn.Open();
                        string commandText = @"INSERT INTO Box (BoxName,Created,TotallAmmount)
                                        VALUES(@BoxName,@Created,@TotallAmmount);";
                        SQLiteCommand command = new SQLiteCommand(commandText, conn);
                        command.Parameters.AddWithValue("@BoxName",box.BoxName);
                        command.Parameters.AddWithValue("@Created", box.Created);
                        command.Parameters.AddWithValue("@TotallAmmount", box.TotallAmmount);
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
        public static ObservableCollection<Box> GetAllBox ()
        {
            SqlliteIntializer v = new SqlliteIntializer();
            ObservableCollection<Box> boxes = new ObservableCollection<Box>();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(v.ConnString()))
                {
                    conn.Open();
                    string commandText = @"SELECT BoxName,Created,TotallAmmount,TotallGain,Id FROM Box;";
                    SQLiteCommand command = new SQLiteCommand(commandText, conn);
                    SQLiteDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        boxes.Add(new Box()
                        {
                            BoxName = dataReader.GetString(0),
                            Created = dataReader.GetDateTime(1),
                            TotallAmmount = dataReader.GetInt32(2),
                            TotallGain=dataReader.GetInt32(3),
                            TotallCost= dataReader.GetInt32(2) - dataReader.GetInt32(3),
                            ID=dataReader.GetInt32(4)
                   
                        });
                      
                    }
                    dataReader.Close();
                    conn.Close();
                }
        
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
            
            return boxes;
        }
        public  static List<string> GetAllBoxNames ()
        {
            SqlliteIntializer v = new SqlliteIntializer();
            List<string> boxes = new List<string>();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(v.ConnString()))
                {
                    conn.Open();
                    string commandText = @"SELECT BoxName FROM Box;";
                    SQLiteCommand command = new SQLiteCommand(commandText, conn);
                    SQLiteDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        boxes.Add(dataReader.GetString(0));
                    }
                    dataReader.Close();
                    conn.Close();
                }

            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
            return boxes;
        }
        public static  Box GetBox (string boxName)
        {
            SqlliteIntializer v = new SqlliteIntializer();
            Box box = null;
            if (boxName!= null)
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(v.ConnString()))
                    {
                        conn.Open();
                        string commandText = @"SELECT BoxName,Created,TotallAmmount,TotallGain,Id FROM Box WHERE BoxName = @boxName;";
                        SQLiteCommand command = new SQLiteCommand(commandText, conn);
                        command.Parameters.AddWithValue("@boxName", boxName);
                        SQLiteDataReader dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            box=  new Box()
                            {
                                BoxName = dataReader.GetString(0),
                                Created = dataReader.GetDateTime(1),
                                TotallAmmount = dataReader.GetInt32(2),
                                TotallGain=dataReader.GetInt32(3),
                               ID= dataReader.GetInt32(4)

                            };
                        }
                        dataReader.Close();
                        conn.Close();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            
            }
            return box;
        }
        public static ObservableCollection<Box> GetBoxByBoxNaem(string boxName)
        {
            SqlliteIntializer v = new SqlliteIntializer();
            ObservableCollection<Box> box = new ObservableCollection<Box>();
            if (boxName != null)
            {
                try
                {
                    using (SQLiteConnection conn = new SQLiteConnection(v.ConnString()))
                    {
                        conn.Open();
                        string commandText = @"SELECT BoxName,Created,TotallAmmount,TotallGain,Id FROM dbo.Box WHERE BoxName = @boxName;";
                        SQLiteCommand command = new SQLiteCommand(commandText, conn);
                        command.Parameters.AddWithValue("@boxName", boxName);
                        SQLiteDataReader dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            box.Add(new Box()
                            {
                                BoxName = dataReader.GetString(0),
                                Created = dataReader.GetDateTime(1),
                                TotallAmmount = dataReader.GetInt32(2),
                                TotallGain = dataReader.GetInt32(3),
                                ID = dataReader.GetInt32(4)


                            });
                        }
                        dataReader.Close();
                        conn.Close();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }
            return box;
        }
        public static bool UpdateTotallAmmount (Operation operation)
        {
            SqlliteIntializer v = new SqlliteIntializer();
            bool isUpdated = false;

            if (operation.Name != null)
            {

                Box box = GetBox(operation.Name);
               
                if (operation.Type == "revenue")
                {
                   
                    box.TotallAmmount += operation.MoneyAmmount;
                    int Gain = operation.MoneyAmmount - operation.CostPrice;
                    box.TotallGain += Gain;
                    box.TotallCost += operation.CostPrice;
                    box.Created = operation.Inserted;
                    bool done = Box.UpdateBox(box);
                }
                else
                {
                    if (operation.MoneyAmmount <= box.TotallAmmount)
                    {
                        
                        box.TotallAmmount -= operation.MoneyAmmount;
                        if (operation.ExpenseSource.ToString() == "gain")
                        {
                            if (operation.MoneyAmmount <= box.TotallGain)
                            {
                                box.TotallGain -= operation.MoneyAmmount;
                               
                            }
                            else
                            {
                                box.TotallGain = 0;
                                box.TotallCost = box.TotallAmmount;
                            }
                            
                            bool done = Box.UpdateBox(box);
                            if (!done)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (box.TotallAmmount ==0)
                            {
                                box.TotallGain = box.TotallAmmount;
                            }
                           
                            
                            bool done = Box.UpdateBox(box);
                            if (!done)
                            {
                                return false;
                            }
                        }
                    }
                   
                }    
               
               
                
                using (SQLiteConnection conn = new SQLiteConnection(v.ConnString()))
                {
                    conn.Open();

                    string txtCommand = @"UPDATE Box SET TotallAmmount=@totallAmmount,TotallGain=@totallGain,
                                                                Created=@updated
                                         WHERE BoxName=@boxName ;";
                   
                    
                    SQLiteCommand command = new SQLiteCommand(txtCommand, conn);
                    command.Parameters.AddWithValue("@boxName", box.BoxName);
                    command.Parameters.AddWithValue("@totallAmmount", box.TotallAmmount);
                    command.Parameters.AddWithValue("@totallGain", box.TotallGain);
                    command.Parameters.AddWithValue("@updated", box.Created);
                    isUpdated = command.ExecuteNonQuery() > 0;
                    conn.Close();
                }
            }
            return isUpdated;
        }
        public static bool CheckCost (Box box)
        {
            bool ok = false;
            if (box.TotallCost == 0)
            {
                box.TotallCost = box.TotallAmmount - box.TotallGain;
                bool done = Box.UpdateBox(box);
               if (done)
                {
                    return done;
                }
            }
            return ok;
        }
        public static bool UpdateBox(Box box)
        {
            SqlliteIntializer v = new SqlliteIntializer();
            bool isUpdated = false;
            if (box !=null && box.BoxName != null)
            {
                
                using (SQLiteConnection conn = new SQLiteConnection(v.ConnString()))
                {
                    conn.Open();
                    string txtCommand = @"UPDATE Box SET 
                                                    TotallAmmount=@totallAmmount
                                          ,TotallGain=@totallGain
                                         
                                              WHERE BoxName=@boxName ;";
                    SQLiteCommand command = new SQLiteCommand(txtCommand, conn);
                   
                    command.Parameters.AddWithValue("@boxName", box.BoxName);
                    command.Parameters.AddWithValue("@totallAmmount", box.TotallAmmount);
                    command.Parameters.AddWithValue("@totallGain", box.TotallGain);
                    
                    isUpdated = command.ExecuteNonQuery() > 0;
                    conn.Close();
                }
            }
            return isUpdated;
        }
        public static bool DeleteBox(string boxName)
        {
            SqlliteIntializer v = new SqlliteIntializer();
            bool deleted = false;
            if (boxName != null && boxName != "Store")
            {
                using (SQLiteConnection conn = new SQLiteConnection(v.ConnString()))
                {
                    conn.Open();
                    string txtCommand = @"DELETE FROM Box WHERE BoxName=@boxName;";
                    SQLiteCommand command = new SQLiteCommand(txtCommand, conn);
                    command.Parameters.AddWithValue("@boxName", boxName);
                    deleted = command.ExecuteNonQuery() > 0;
                    conn.Close();
                }
            }
            return deleted;
        }
        public static bool DeleteAll()
        {
            SqlliteIntializer v = new SqlliteIntializer();
            bool done = false;

            UpdateBox(new Box()
            {
                BoxName="Store",
                TotallAmmount=0,
                TotallCost=0,
                TotallGain=0,
                Created=DateTime.Today.Date
            });
            using (SQLiteConnection conn = new SQLiteConnection(v.ConnString()))
            {
                conn.Open();
                string txtCommand = @"DROP TABLE IF EXISTS  Box ; ";
                SQLiteCommand command = new SQLiteCommand(txtCommand, conn);
                done = command.ExecuteNonQuery() > 0;
                conn.Close();

            }
            return done;
        }
        public static bool Drop()
        {
            SqlliteIntializer v = new SqlliteIntializer();
            bool done = false;
            using (SQLiteConnection conn = new SQLiteConnection(v.ConnString()))
            {
                conn.Open();
                string txtCommand = @"DROP TABLE IF EXISTS  Box ; ";
                SQLiteCommand command = new SQLiteCommand(txtCommand, conn);

                done = command.ExecuteNonQuery() > 0;
                conn.Close();
            }
            if (!done)
            {

                v.createTables("create");
            }
            return done;
        }
    }
   

}

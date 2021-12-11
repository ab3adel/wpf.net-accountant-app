using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;

using System.Diagnostics;
using System.Data.SQLite;
namespace accountant
{
   public class Operation
    {
        [ForeignKey("Box")]
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime Inserted { get; set; }
        public int MoneyAmmount { get; set; }
        public int CostPrice { get; set; }
        public string ExpenseSource { get; set; }

        public int Hidden { get; set; }
 


        public static bool  SetOperation(Operation operation)
        {
            SqlliteIntializer v = new SqlliteIntializer();
            bool done = false;
           
            DateTime inserted = DateTime.Today.Date;
                if (operation.Name != null)
                {
                bool updated = Box.UpdateTotallAmmount(operation);
                    if ( !updated )
                {
                    return done;
                }
                    using (SQLiteConnection conn = new SQLiteConnection(v.ConnString()))
                    {
                        conn.Open();
                    string commandText = @"INSERT INTO  Operation (Name,Type,Inserted,MoneyAmmount,CostPrice,ExpenseSource,Hidden)
                                        VALUES(@name,@type,@inserted,@moneyAmmount,@costPrice,@expenseSource,@hidden);";
                   
                        SQLiteCommand command = new SQLiteCommand(commandText, conn);
                    command.Parameters.AddWithValue("@name", operation.Name);
                    command.Parameters.AddWithValue("@type", operation.Type);
                        command.Parameters.AddWithValue("@inserted", inserted);
                        command.Parameters.AddWithValue("@moneyAmmount", operation.MoneyAmmount);
                        command.Parameters.AddWithValue("@costPrice", operation.CostPrice);
                    command.Parameters.AddWithValue("@hidden", operation.Hidden);
                    if (string.IsNullOrEmpty(operation.ExpenseSource)) operation.ExpenseSource = "__";

                    command.Parameters.AddWithValue("@expenseSource", operation.ExpenseSource);
                    done =  command.ExecuteNonQuery() > 0;
                    conn.Close();
                    }
                }
                return done;
          
        }
        public static ObservableCollection<Operation> GetLastOperations()
        {
            SqlliteIntializer v = new SqlliteIntializer();
            ObservableCollection<Operation> operations = new ObservableCollection<Operation>();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(v.ConnString()))
                {
                    conn.Open();
                    string commandText = @"SELECT  Name,Type,Inserted,MoneyAmmount,CostPrice,ExpenseSource,Hidden FROM Operation WHERE Hidden=0  ORDER BY Inserted DESC LIMIT 20;";
                    SQLiteCommand command = new SQLiteCommand(commandText, conn);
                
                    SQLiteDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                       
                        operations.Add(new Operation()
                        {
                            Name = dataReader.GetString(0),
                            Type = dataReader.GetString(1),
                            Inserted = dataReader.GetDateTime(2),
                            MoneyAmmount = dataReader.GetInt32(3),
                            CostPrice = dataReader.GetInt32(4),
                            ExpenseSource = dataReader.GetString(5),
                            


                        }); ;
                    }
                    dataReader.Close();
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
            
            return operations;
        }
        public static ObservableCollection<Operation> GetLastOperationsByName(string name)
        {
            SqlliteIntializer v = new SqlliteIntializer();
            ObservableCollection<Operation> operations = new ObservableCollection<Operation>();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(v.ConnString()))
                {
                    conn.Open();
                    string commandText = @"SELECT  Name,Type,Inserted,MoneyAmmount,CostPrice,ExpenseSource FROM Operation WHERE Name=@name AND Hidden =0 ORDER BY Inserted DESC LIMIT 100;";
                    SQLiteCommand command = new SQLiteCommand(commandText, conn);
                    command.Parameters.AddWithValue("@name", name);
                    SQLiteDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        operations.Add(new Operation()
                        {
                            Name = dataReader.GetString(0),
                            Type = dataReader.GetString(1),
                            Inserted = dataReader.GetDateTime(2),
                            MoneyAmmount = dataReader.GetInt32(3),
                            CostPrice = dataReader.GetInt32(4),
                            ExpenseSource=dataReader.GetString(5)


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

            return operations;
        }
        public static ObservableCollection<Operation> GetOperationsByDate(DateTime inserted)
        {
            SqlliteIntializer v = new SqlliteIntializer();
            ObservableCollection<Operation> operations = new ObservableCollection<Operation>();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(v.ConnString()))
                {
                    conn.Open();
                    string commandText = @"SELECT Name,Type,Inserted,MoneyAmmount,CostPrice,ExpenseSource FROM Operation WHERE Inserted=@inserted AND Hidden=0  ORDER BY Inserted DESC LIMIT 100;";
                    SQLiteCommand command = new SQLiteCommand(commandText, conn);
                    command.Parameters.AddWithValue("@inserted", inserted);
                    SQLiteDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        operations.Add(new Operation()
                        {
                            Name = dataReader.GetString(0),
                            Type = dataReader.GetString(1),
                            Inserted = dataReader.GetDateTime(2),
                            MoneyAmmount = dataReader.GetInt32(3),
                            CostPrice = dataReader.GetInt32(4),
                            ExpenseSource = dataReader.GetString(5)


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

            return operations;
        }
        public static ObservableCollection<Operation> GetOperationsByDateName(string name,DateTime inserted)
        {
            SqlliteIntializer v = new SqlliteIntializer();

            ObservableCollection<Operation> operations = new ObservableCollection<Operation>();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(v.ConnString()))
                {
                    conn.Open();
                    string commandText = @"SELECT  Name,Type,Inserted,MoneyAmmount,CostPrice,ExpenseSource
                                            FROM Operation WHERE Name=@name AND Inserted=@inserted;";
                    if (name == "Store") commandText = @"SELECT  Name,Type,Inserted,MoneyAmmount,CostPrice,ExpenseSource
                                            FROM Operation WHERE Name=@name AND Inserted=@inserted AND Type='expense';";
                    SQLiteCommand command = new SQLiteCommand(commandText, conn);
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@inserted", inserted);
                    SQLiteDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        operations.Add(new Operation()
                        {
                            Name = dataReader.GetString(0),
                            Type = dataReader.GetString(1),
                            Inserted = dataReader.GetDateTime(2),
                            MoneyAmmount = dataReader.GetInt32(3),
                            CostPrice = dataReader.GetInt32(4),
                            ExpenseSource = dataReader.GetString(5)


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

            return operations;
        }
        public static Box GetOperationsByMonthYear(int yearNumber, int monthNumber,string name)
        {

            SqlliteIntializer v = new SqlliteIntializer();
            int totallGain = 0;
           int totallExpense = 0;
                int totallCost = 0;
            string m = monthNumber.ToString();
            if (monthNumber < 10) m = "0" + monthNumber.ToString();
            Box box = new Box();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(v.ConnString()))
                {
                    conn.Open();
                    string commandText = @"SELECT  Name,MoneyAmmount,CostPrice,Type
                                          FROM Operation 
                                            WHERE Name=@name AND
                                            strftime('%m',Inserted)=@monthNumber AND strftime('%Y',Inserted)=@yearNumber  ;";
                    SQLiteCommand command = new SQLiteCommand(commandText, conn);
                    command.Parameters.AddWithValue("@name", name);
                   command.Parameters.AddWithValue("@monthNumber",m);
                    command.Parameters.AddWithValue("@yearNumber", yearNumber.ToString());
          

                    SQLiteDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        if (dataReader.GetString(3) == "revenue")
                        {
                            totallGain += dataReader.GetInt32(1) - dataReader.GetInt32(2);
                            totallCost += dataReader.GetInt32(2);
                        }
                        else
                        {
                            totallExpense += dataReader.GetInt32(1);
                        }
                       

                    }
                   
                    dataReader.Close();
                    box.BoxName = name;
                    box.TotallCost = totallCost;
                    box.TotallGain = totallGain;
                    box.TotallAmmount = totallExpense;
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
            
            return box;
        }
        public static ObservableCollection<Box> GetTotalGainCostPerDay(int year, int month,string name)
        {
            SqlliteIntializer v = new SqlliteIntializer();
            
            string m = month.ToString();
            string d = "";
            List<int> DaysNumbers =new List<int>() { 30, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            if (month< 10)  m = "0"+ month.ToString();
            

            ObservableCollection<Box> boxes = new ObservableCollection<Box>();
           try
            {
                using (SQLiteConnection conn = new SQLiteConnection(v.ConnString()))
                {
                    conn.Open();
                    for (int i=1; i<=DaysNumbers[month-1];i++)
                    {
                        int totallGain = 0;
                        int totallExpense = 0;
                        int totallCost = 0;
                        d = i.ToString();
                        if (i < 10) d =  "0"+i.ToString() ;
                    
                        string CommandString = $@"SELECT  Name,MoneyAmmount,CostPrice ,Type
                                          FROM Operation 
                                            WHERE Name=@name  AND
                                            strftime('%m',Inserted)=@monthNumber  AND
                                            strftime('%d',Inserted)='{d}'
                                            AND strftime('%Y',Inserted)=@yearNumber  ;";
                        SQLiteCommand command = new SQLiteCommand(CommandString, conn);
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@monthNumber", m);
                        command.Parameters.AddWithValue("@yearNumber", year.ToString());
                        SQLiteDataReader dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            if (dataReader.GetString(3) == "revenue")
                            {
                                totallGain += dataReader.GetInt32(1) - dataReader.GetInt32(2);
                                totallCost += dataReader.GetInt32(2);
                            }
                            else
                            {
                                totallExpense += dataReader.GetInt32(1);
                            }
                        }
                        dataReader.Close();
                        boxes.Add(new Box()
                        {
                            BoxName = name,
                            TotallAmmount = totallExpense,
                            TotallCost=totallCost,
                            TotallGain=totallGain,
                            ExtraField=i
                        });
                    }
                }
            }
            catch(Exception e)
            {
                Trace.WriteLine(e);
            }

            return boxes;
        }
        public static ObservableCollection<Box> GetTotalGainCostPerMonth (int year,int month)
        {
           
            ObservableCollection<Box> boxes = new ObservableCollection<Box>();
            List<string> boxNames = Box.GetAllBoxNames();
            foreach (string name in boxNames)
            {
                boxes.Add(Operation.GetOperationsByMonthYear(year, month, name));
            }
                 

            return boxes;
        }
        public static  bool DeleteAll()
        {
            SqlliteIntializer v = new SqlliteIntializer();
            bool done = false;
            using (SQLiteConnection conn = new SQLiteConnection(v.ConnString()))
            {
                conn.Open();
                string txtCommand = @"DELETE  FROM Operation  ; ";
                SQLiteCommand command = new SQLiteCommand(txtCommand, conn);
                command.CommandTimeout = 60;
                done =  command.ExecuteNonQuery() > 0;
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
                string txtCommand = @"DROP TABLE IF EXISTS  Operation ; ";
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
namespace accountant
{
   public class Product:INotifyPropertyChanged
    {
        private string ProductIdValue = String.Empty;
        private string ProductNameValue = String.Empty;
        private int ProdutNumberValue = 0;
        private int PriceValue = 0;
        private int SellingPriceValue = 0;
        public int ID { get; set; }
        public string ProductName
        {
            get { return this.ProductNameValue; }
            set
            {
                
                    this.ProductNameValue = value;
                    NotifyPropertyChanged("ProductName");
                
            }
        }
        public string ProductId { 
            get
            {
                return this.ProductIdValue;
            }
            set
            {
                
                    
                    this.ProductIdValue = value;
                    NotifyPropertyChanged("ProductId");
                
            }
        }
        public int ProductNumber {
            get
            {
                return this.ProdutNumberValue;
            }
            set
            {
                if ((int)value >= 0)
                {
                    this.ProdutNumberValue = value;
                    NotifyPropertyChanged("ProductNumber");
                }
            }
        }
        public int Price
        {
            get
            {
                return this.PriceValue;
            }
            set
            {
                if ((int)value >= 0)
                {
                    this.PriceValue = value;
                    NotifyPropertyChanged("Price");
                }
            }
        
        }
        public int SellingPrice {
            get
            {
                return this.SellingPriceValue;
            }
            set
            {
                if ((int)value >= 0)
                {
                    this.SellingPriceValue = value;
                    NotifyPropertyChanged("SellingPrice");
                }
            }
        }
        public DateTime InsertedDate { set; get; }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public override string ToString()
        {
            if (ProductName != null)
            {
                return ProductName.ToString();
            }
            return "";
        }
  
       
        public Product ()
        {
            
        }
        public static  ObservableCollection<Product>  showStore()
        {
            SqlliteIntializer v = new SqlliteIntializer();
            ObservableCollection<Product> product = new ObservableCollection<Product>();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(v.ConnString()))
                {
                    conn.Open();
                    string commandText = @"SELECT ProductName,ProductId,ProductNumber,Price,SellingPrice,Id FROM Store;";
                    SQLiteCommand command = new SQLiteCommand(commandText, conn);
                    SQLiteDataReader dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        
                 


                        product.Add(new Product()
                        {
                            ProductName = dataReader.GetString(0),
                            ProductId = dataReader.GetString(1),
                            ProductNumber = dataReader.GetInt32(2),
                            Price = dataReader.GetInt32(3),
                            SellingPrice = dataReader.GetInt32(4),
                            ID = dataReader.GetInt32(5)


                        });
                    }
                    dataReader.Close();
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
    
            return product;
        }
        public static bool InsertProduct(Product product)
        {
            bool isSaved = false;
            SqlliteIntializer v = new SqlliteIntializer();
        
            using (SQLiteConnection conn = new SQLiteConnection(v.ConnString()))
            {
                conn.Open();
                string commandText = @"INSERT INTO Store (ProductId,ProductName,ProductNumber,Price,SellingPrice)
                                        VALUES(@productId,@productName,@productNumber,@price,@sellingPrice);";
                SQLiteCommand command = new SQLiteCommand(commandText, conn);
                command.Parameters.AddWithValue("@Id", "");
                command.Parameters.AddWithValue("@ProductId", product.ProductId);
                command.Parameters.AddWithValue("@productName", product.ProductName);
                command.Parameters.AddWithValue("@productNumber", product.ProductNumber);
                command.Parameters.AddWithValue("@price", product.Price);
                command.Parameters.AddWithValue("@sellingPrice", product.SellingPrice);
                try
                {
                    isSaved = command.ExecuteNonQuery() > 0;
                    conn.Close();

                }
                catch (SQLiteException err)
                {
                    Trace.WriteLine(err);
                    isSaved = false;
                    return isSaved;
                }
            }
            
            return isSaved;
        }
        public static async  Task<Product> GetProduct(string productId=null,string productName=null)
        {
            SqlliteIntializer v = new SqlliteIntializer();
            
            Product product=null;
            if (productId != null || productName != null)
            {
                using (SQLiteConnection sqlConnection = new SQLiteConnection(v.ConnString()))
                {
                    sqlConnection.Open();
                    string txtCommand;
                    SQLiteCommand command;
                    if (productId != null)
                    {
                        txtCommand = @"SELECT * FROM Store 
                                   WHERE ProductId=@ProductId ;";
                        command = new SQLiteCommand(txtCommand, sqlConnection);
                        command.Parameters.AddWithValue("@ProductId", productId);
                    }
                  else {
                        txtCommand = @"SELECT * FROM Store 
                                   WHERE ProductName=@productName ;";
                        command = new SQLiteCommand(txtCommand, sqlConnection);
                        command.Parameters.AddWithValue("@productName", productName);
                    }
                    SQLiteDataReader data = await command.ExecuteReaderAsync() as SQLiteDataReader;
                  
                    while (data.Read())
                    {
                        
                        product = new Product()
                        {
                            ID=data.GetInt32(0),
                            ProductName = data.GetString(1),
                            ProductId = data.GetString(2),
                            ProductNumber = data.GetInt32(3),
                            Price = data.GetInt32(4),
                            SellingPrice = data.GetInt32(5)
                            

                        };
                    }
                    data.Close();
                }
            }
    
            return product;
        }
        public static bool  UpdateProduct (Product product)
        {
            SqlliteIntializer v = new SqlliteIntializer();
            bool isUpdated = false;
            if (product != null)
            {
                using (SQLiteConnection conn = new SQLiteConnection(v.ConnString()))
                {
                    conn.Open();
                    string txtCommand = @"UPDATE Store SET ProductId=@ProductId
                                                    ,ProductName=@ProductName
                                          ,Price=@Price,SellingPrice=@SellingPrice,
                                             ProductNumber=@ProductNumber WHERE Id=@Id ;";
                    SQLiteCommand command = new SQLiteCommand(txtCommand, conn);
                    command.Parameters.AddWithValue("@Id", product.ID);
                    command.Parameters.AddWithValue("@ProductId",product.ProductId);
                    command.Parameters.AddWithValue("@ProductName",product.ProductName);
                    command.Parameters.AddWithValue("@Price",product.Price);
                    command.Parameters.AddWithValue("@SellingPrice",product.SellingPrice);
                    command.Parameters.AddWithValue("@ProductNumber",product.ProductNumber);
                    isUpdated =  command.ExecuteNonQuery() > 0;
                    conn.Close();
                }
              
            }
           
            return isUpdated;
        }
        public static async Task<bool> DeleteProduct (string productId)
        {
            SqlliteIntializer v = new SqlliteIntializer();
            bool deleted;
            using (SQLiteConnection conn = new SQLiteConnection(v.ConnString()))
            {
                conn.Open();
                string txtCommand = @"DELETE FROM Store WHERE ProductId=@ProductId;";
                SQLiteCommand command = new SQLiteCommand(txtCommand, conn);
                command.Parameters.AddWithValue("@ProductId", productId);
                deleted = await command.ExecuteNonQueryAsync() > 0;
                conn.Close();
            }
            return deleted;
        }
        public static bool DeleteAll()
        {
            SqlliteIntializer v = new SqlliteIntializer();
            bool done = false;

            using (SQLiteConnection conn = new SQLiteConnection(v.ConnString()))
            {

                conn.Open();
                string txtCommand = @"DELETE  FROM Store ; ";
                SQLiteCommand command = new SQLiteCommand(txtCommand, conn);
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
                string txtCommand = @"DROP TABLE IF EXISTS  Store ; ";
                SQLiteCommand command = new SQLiteCommand(txtCommand, conn);

                done = command.ExecuteNonQuery() > 0;
                if (!done)
                {
                    
                    v.createTables("create");
                }
                conn.Close();
            }
            return done;
        }
    }
   
   
}

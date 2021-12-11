using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Collections.ObjectModel;
namespace accountant
{
    /// <summary>
    /// Interaction logic for addProduct.xaml
    /// </summary>
    public partial class addProduct : Window
    {
        ObservableCollection<Product> ProductsList = new ObservableCollection<Product>();
        public bool submit = true;

      
        public addProduct()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(WindowLoaded);
            myDataGrid.ItemsSource = ProductsList;
            Uri iconUri = new Uri("pack://application:,,,/Images/favicon.ico", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);
        }
       
        private void WindowLoaded (Object sender , RoutedEventArgs e)
        {
            productId.Focus();
            
            if (String.IsNullOrEmpty(productId.Text))
            {
                
                addBtn.IsEnabled = false;
                saveBtn.IsEnabled = false;
            }
        }
        private void BarcodeReader_keyup(Object sender, KeyEventArgs e)
        {
            if (!addBtn.IsEnabled || !saveBtn.IsEnabled)
            {
                if (!String.IsNullOrEmpty(productId.Text))
                {
                    addBtn.IsEnabled = true;
                    saveBtn.IsEnabled = true;
                }
            }
         
            
            if (e.Key == Key.Enter)
            {
                IfEnter();
            }
        }

        private void addtoList(object sender, RoutedEventArgs e)
        {
            Product obj =null;
            try
            {
                 obj = new Product()
                {
                    ProductId = productId.Text,
                    ProductName = productName.Text,
                    Price = Int32.Parse(price.Text),
                    SellingPrice = Int32.Parse(sellingPrice.Text),
                    ProductNumber = Int32.Parse(number.Text)
                };
            }
            catch (Exception )
            {
                MessageBox.Show("invalid inputs !", "Error");
            }
            if (obj != null)
            {
                foreach (Product i in ProductsList.ToList())
                {
                    if (i.ProductName == obj.ProductName)
                    {
                        ProductsList.Remove(i);
                      
                    }

                }
                ProductsList.Add(obj);
           
                price.Clear();
                number.Clear();
                productId.Clear();
                productName.Clear();
                sellingPrice.Clear();
            }
            productId.Focus();
        }

        private void HandleDeleteBtn(object sender, MouseButtonEventArgs e)
        {
            if (myDataGrid.SelectedItem != null)
            {
               ProductsList.Remove((Product) myDataGrid.SelectedItem);
                myDataGrid.UnselectAll();
            }
          

          
        }
        private void HandleEditBtn(object sender, RoutedEventArgs e)
        {
            if (myDataGrid.SelectedItem != null)
            {
                var  myItem = (Product) myDataGrid.SelectedItem;
                productId.Text = myItem.ProductId;
                productName.Text = myItem.ProductName;
                price.Text = myItem.Price.ToString() ;
                sellingPrice.Text = myItem.SellingPrice.ToString();
                number.Text = myItem.ProductNumber.ToString();
               ProductsList.Remove(myItem);
                myDataGrid.UnselectAll();

            }
           
        }

       

      

      
        private void IfEnter()
        {


            if (String.IsNullOrEmpty(productName.Text))
            {
                productName.Focus();
            }
            else
            {
                if (String.IsNullOrEmpty(price.Text))
                {
                    price.Focus();
                }
                else
                {
                    if (String.IsNullOrEmpty(sellingPrice.Text))
                    {
                        sellingPrice.Focus();
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(number.Text))
                        {
                            number.Focus();
                        }
                        else
                        {
                            
                            
                                ProductsList.Add(new Product()
                                {
                                    ProductId = productId.Text,
                                    ProductName = productName.Text,
                                    Price = Int32.Parse(price.Text),
                                    ProductNumber = Int32.Parse(number.Text),
                                    SellingPrice = Int32.Parse(sellingPrice.Text)
                                });
                                productId.Clear();
                                productName.Clear();
                                price.Clear();
                                number.Clear();
                                sellingPrice.Clear();
                            

                        }
                    }
                }

            }  
        }

        private   void SaveToStore(object sender, RoutedEventArgs e)
        {
            
            if (ProductsList.Count > 0)
            {
                foreach (Product item in ProductsList.ToList())
                {
                    
                    bool IsSaved =  Product.InsertProduct(item);
                    if (!IsSaved)
                    {
                        MessageBox.Show($"حدث خطأ ما مع العنصر {item.ProductName}");
                        DataGridRow row = myDataGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                        row.Background = Brushes.Red;
                        return;
                    }
                   else
                    {
                       ProductsList.Remove(item);
                    }
                }
                ProductsList.Clear();
               
            }
            else
            {
                MessageBox.Show("No data  to save", "Error");
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HandleDoublClick(object sender, MouseButtonEventArgs e)
        {
            if (myDataGrid.SelectedItem != null)
            {
                Product row = myDataGrid.SelectedItem as Product;
                productId.Text = row.ProductId;
                productName.Text = row.ProductName;
                price.Text = row.Price.ToString();
                number.Text = row.ProductNumber.ToString();
                sellingPrice.Text = row.SellingPrice.ToString();
            }
        }

        private void EditorDelete(object sender, SelectionChangedEventArgs e)
        {
            deleteAllBtn.Visibility = Visibility.Visible;
            deleteBtn.Visibility = Visibility.Visible;
        }
        private void Check(object sender, MouseButtonEventArgs e)
        {
            string[] test = e.Source.ToString().Split(" ");


            if (test[0] != typeof(DataGrid).ToString())
            {
                myDataGrid.UnselectAll();
                deleteBtn.Visibility = Visibility.Hidden;
                deleteAllBtn.Visibility = Visibility.Hidden;
            }
        }

        private void PreventSumbit(object sender, ValidationErrorEventArgs e)
        {
            submit = false;

        }
    }
}

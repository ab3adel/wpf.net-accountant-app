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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace accountant
{
    /// <summary>
    /// Interaction logic for Selling.xaml
    /// </summary>
    /// 
    public partial class Selling : UserControl
    {
        List<string> lst = new List<string>();
        Product insertedItem = null;
        ObservableCollection<Product> productsList = new ObservableCollection<Product>();
        Button deleteBtn = new Button();
        Button deleteAllBtn = new Button();
        List<Product> suggestedList = new List<Product>();
        List<Product> lastSuggestedList = new List<Product>();
        string input = "";
        string currInput = "";
        string prevInput = "";
        public Selling()
        {
            
         
            InitializeComponent();
    

          
            SellingList = new ObservableCollection<Sellings>();
            myDataGrid.ItemsSource = SellingList;
            
            productsList = Product.showStore();
            productNameCbx.Items.Clear();
 
            SellingList.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(UpdateSum);


            deleteBtn.Content = "Delete";
            deleteBtn.Click += new RoutedEventHandler(Delete);

        
            deleteAllBtn.Content = "Delete All";
            deleteAllBtn.Click += new RoutedEventHandler(DeleteAll);


        }

        public ObservableCollection<Sellings> SellingList { get; set; }
       
    
        

        private async void myDataGrid_KeyDown(object sender, KeyEventArgs e)
        {
           
            int index = -1;
            if (e.Key != Key.Return && e.Key.ToString().Length <3)
            {
                 
                var keyStroke = e.Key.ToString();
                if (keyStroke.Contains('D') && keyStroke.Length>1)
                {
                     keyStroke = e.Key.ToString().Substring(1);
                }
               
                lst.Add(keyStroke);
            }
            if (e.Key == Key.Return)
            {
           
                string id="";
                foreach (string i in lst )
                {
                    id = id + i;
                }
            
                if (id != null)
                {
                    
                    if (SellingList.Count > 0)
                    {
                        
                        
                        foreach (var i in SellingList)
                        {
                            if (i.ProductId == id && i.ProductName !=null)
                            {
                                
                                index = SellingList.IndexOf(i);
                              
                                int num = SellingList[index].Number + 1;
                                i.Number = num;
                                SellingList.RemoveAt(index);
                                SellingList.RemoveAt(myDataGrid.SelectedIndex -1);
                                SellingList.Insert(index, i);
                                lst.Clear();
                                return;
                            }
                        }
                    }
                    
                    if (index == -1)
                    {
                        insertedItem =await Product.GetProduct(id);
                    }
                    lst.Clear();
                }
               
                if (insertedItem != null)
                {
                    Sellings thing = new Sellings()
                    {
                        ProductId = id,
                        ProductName = insertedItem.ProductName,
                        Number = 1,
                        SellingPrice = insertedItem.SellingPrice,
                        Gain = insertedItem.Price - insertedItem.SellingPrice
                    };

                    index = myDataGrid.Items.IndexOf(myDataGrid.CurrentCell.Item);

                    if (index != -1)
                    {
                        
                        if (index-1 >= 0) { 
                            SellingList.Insert(index - 1, thing);
                            SellingList.RemoveAt(index);
                            
                        }
                        else if (index == 0)
                        {
                            SellingList.Add(thing);
                            
                        }
                        
                       

                    }

                }
                else if (lst.Count >0 && insertedItem == null)
                {
                    MessageBox.Show("item you have inserted is not in the store");
                    index = myDataGrid.Items.IndexOf(myDataGrid.CurrentCell.Item);
                    if (myDataGrid.Items.Count >= index + 1)
                    {
                        DataGridRow row = (DataGridRow)myDataGrid.ItemContainerGenerator.ContainerFromIndex(index - 1);
                        if (row != null)
                        {

                            row.Background = Brushes.Red;
                        }

                    }
                }
            }
            
        }
       

        private  void InsertSellings(object sender, RoutedEventArgs e)
        {
            List<Sellings> sellingsList = new List<Sellings>();
            List<Product> products = new List<Product>();
            if (SellingList != null && SellingList.Count > 0)
            {
                foreach (var item in SellingList.ToList<Sellings>())
                {
                    if (item != null && item.ProductName != null && item.SellingPrice != 0 && item.GetType().Equals(typeof(Sellings)))
                    {



                        Sellings things = new Sellings()
                        {
                            ProductId = item.ProductId,
                            ProductName = item.ProductName,
                            SellingPrice = item.SellingPrice,
                            Number = item.Number,
                            Gain = item.Gain * item.Number

                        };
                       Product product= productsList.First(i => i.ProductName == item.ProductName);
                   
                        if (product.ProductNumber - things.Number < 0)
                        {
                            
                            var row = myDataGrid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                            row.Background = Brushes.Red;
                            MessageBox.Show("العدد المدخل أكبر من العدد الموجود !");
                            return;
                        }
                        products.Add(product);
                        sellingsList.Add(things);
                        SellingList.Remove(item);
                    }

                }
                bool done = Sellings.InsertProduct(sellingsList,products);
                if (!done)
                {
                    MessageBox.Show("لا يمكن القيام بالعملية");
                }
                 

                }  }

        private void DeleteAll(object sender, RoutedEventArgs e)
        {
            if (SellingList.Count > 0)
            {
                SellingList = new ObservableCollection<Sellings>();
                Sellings thing = new Sellings() { };
                SellingList.Add(thing);
                SellingList.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(UpdateSum);
                sum.Clear();
                myDataGrid.ItemsSource=SellingList;
                myDataGrid.CurrentCell = new DataGridCellInfo(myDataGrid.Items[0], myDataGrid.Columns[0]);

            }
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if (myDataGrid.SelectedItem != null && myDataGrid.SelectedItem.GetType().Equals(typeof(Sellings)))
            {
                SellingList.Remove((Sellings) myDataGrid.SelectedItem);
            }
        }

        private void SelectionHappen(object sender, SelectionChangedEventArgs e)
        {
            deleteBtn1.Visibility = Visibility.Visible;
            editBtn1.Visibility = Visibility.Visible;
            Xbutton.Visibility = Visibility.Visible;
        }

        private void BackProductHandler(object sender, RoutedEventArgs e)
        {
            BackProduct backDialog = new BackProduct();
            backDialog.ShowDialog();
            
        }

        private void UpdateSum(object sender, EventArgs e)
        {
            
            int total = 0;
            var items = myDataGrid.Items;
            if (items != null && items.Count > 0)
            {
               
                foreach (var item in items)
                {
                    if (item != null && item.GetType().Equals(typeof(Sellings)))
                    {
                        total = total + (item as Sellings).Number * (item as Sellings).SellingPrice;
                        
                    }

                }

            }
            sum.Text = $" {total} ل.س";
            
        }

        private void SelectProductName(object sender, TextChangedEventArgs e)
        {
            var cbx = sender as ComboBox;
            suggestedList = new List<Product>();
            input = (e.OriginalSource as TextBox).Text;
            prevInput = currInput;
            currInput = input;
            
            if (String.IsNullOrEmpty(currInput) )
            {
               
                suggestedList = new List<Product> ();
                productNameCbx.ItemsSource = null;
            }
          if (cbx.Text.Length > 2)
            {
                lastSuggestedList = suggestedList;
                foreach (Product item in productsList)
                {
                    if (!String.IsNullOrEmpty(currInput)) prevInput = currInput;
                    if (item.ProductId.ToLower().Contains(prevInput.ToLower()) || item.ProductName.ToLower().Contains(prevInput.ToLower()))
                    {
                        
                        if (suggestedList.Count() == 5) break;
                        else
                        {
                            
                            if (!suggestedList.Contains(item))
                            {
                                
                                suggestedList.Add(item);
                               

                            }
                            
                        }
                    }
                    else
                    {
                        
                        if (suggestedList.Contains(item)) suggestedList.Remove(item);
                    }
                }
                
                cbx.ItemsSource = suggestedList;
                cbx.IsTextSearchEnabled = false;
                cbx.IsDropDownOpen = true;

            }
           
      

        }

        private void GetProductName(object sender, RoutedEventArgs e)
        {

           
            if (!string.IsNullOrEmpty(productNameCbx.Text) || productNameCbx.SelectedItem != null)
            {
                Product product = new Product() { };
                if (!string.IsNullOrEmpty(productNameCbx.Text))
                {
                    foreach (Product item in productsList)
                    {
                        if (item.ProductName == productNameCbx.Text)
                        {
                            product = item;
                        }
                    }
                }
                else
                {
                    product = productNameCbx.SelectedItem as Product;
                }
                foreach(Sellings i in SellingList)
                {
                    if (i.ProductName == product.ProductName && i.ProductId == product.ProductId)
                    {
                        i.Number += 1;
                        int index = SellingList.IndexOf(i);
                        SellingList.Remove(i);
                        SellingList.Insert(index, i);
                        Trace.WriteLine(SellingList[SellingList.IndexOf(i)].Number);
                        myDataGrid.ItemsSource = SellingList;
                        productNameCbx.ClearValue(ComboBox.TextProperty);
                        return;
                    }
                }

                SellingList.Add(new Sellings()
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    SellingPrice = product.SellingPrice,
                    Gain = product.SellingPrice - product.Price,
                    Number = 1,
                    Inserted = DateTime.Today.Date

                }) ;

                productNameCbx.ClearValue(ComboBox.TextProperty);

            }
        }

        private void ShowSellings(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            if (btn.Content.ToString() == "سجل المبيعات")
            {
                myTab.SelectedIndex = 1;
                btn.Content = "رجوع";
            }
            else
            {
                myTab.SelectedIndex = 0;
                btn.Content = "سجل المبيعات";
            }
        }

        private void Check(object sender, MouseButtonEventArgs e)
        {
            string[] test = e.Source.ToString().Split(" ");


            if (test[0] != typeof(DataGrid).ToString())
            {
                myDataGrid.UnselectAll();
                deleteBtn1.Visibility = Visibility.Hidden;
                editBtn1.Visibility = Visibility.Hidden;
            }
        }

        private void EnterPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
               
                suggestedList.Clear();
                productNameCbx.ItemsSource = suggestedList;
            }
            if (e.Key == Key.Enter)
                
             {
                List<Product> container = suggestedList;
               if (suggestedList.Count == 0)
                {
                    if (lastSuggestedList.Count ==1) {
                        container = lastSuggestedList;
                    }
                }
                if (!string.IsNullOrEmpty(prevInput)  || !string.IsNullOrEmpty(prevInput))
                {

                    
                    foreach (Product item in container.ToList())
                    {
                        if (!string.IsNullOrEmpty(currInput) )
                        {
                            
                            if (item.ProductId == currInput || item.ProductName == currInput)
                            {
                                
                                productNameCbx.SelectedItem = item;
                                productNameCbx.Text = item.ToString();
                                addBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));


                            }
                            else
                            {
                            
                                productNameCbx.SelectedItem = null;
                            }
                        }
                        else 
                        {
                           
                            if (item.ProductId == prevInput || item.ProductName == prevInput)
                            {
                                
                                productNameCbx.SelectedItem = item;
                                productNameCbx.Text = item.ToString();
                                addBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));


                            }
                            else
                            {
                              
                                productNameCbx.SelectedItem = null;
                            }
                        }
                      
                    }


                }
               
                
            }
        }

        private void SelectionHandler(object sender, SelectionChangedEventArgs e)
        {
            var combo = sender as ComboBox;
            if (combo.SelectedItem != null )
            {
                Product item = combo.SelectedItem as Product;
                
                if (combo.Text.Contains( item.ToString()) || item.ProductId.Contains(combo.Text))
                {
                    
                    suggestedList = new List<Product>();
                    suggestedList.Add(item);
                    combo.ItemsSource = suggestedList;
                   
                }
                else
                {
                    suggestedList = new List<Product>();
                    combo.ItemsSource = suggestedList;
                }
              
            }
            
            
        }

        private void HideSideButtons(object sender, RoutedEventArgs e)
        {
            editBtn1.Visibility = Visibility.Collapsed;
            deleteBtn1.Visibility = Visibility.Collapsed;
            (sender as Button).Visibility = Visibility.Collapsed;
        }

        private void RefreshSelling(object sender, RoutedEventArgs e)
        {
            productsList = Product.showStore();
        }

        private void CellChanged(object sender, DataGridCellEditEndingEventArgs e)
        {
           
           
        }
    }
}


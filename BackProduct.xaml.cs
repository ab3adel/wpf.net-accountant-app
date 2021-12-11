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

namespace accountant
{
    /// <summary>
    /// Interaction logic for BackProduct.xaml
    /// </summary>
    public partial class BackProduct : Window
    {

        ObservableCollection<Sellings> sellingList = new ObservableCollection<Sellings>();
        string date = null;
        bool byName = false;
        string lst ="";
        string currInput = "";
        string prevInput = "";
        List<string> dateList = new List<string>();
        public BackProduct()
        {
            InitializeComponent();
            sellingList = Sellings.GetSellings();
           
            dateList.Add("Any");
            dateCbx.Text = "Any";

        }

        private void LoadedWindow(Object sender, RoutedEventArgs e)
        {


            returnedProduct.ItemsSource = sellingList;
            returnedProduct.SelectedValuePath = "ProductName";
        }
        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            /*
            returnedProduct.ItemsSource= (from item in sellingList
                                            where item.ProductName.ToLower().Contains(returnedProduct.Text.ToLower())
                                            || item.ProductId.ToLower().Contains(returnedProduct.Text.ToLower())
                                            select item) ;
           */

            ObservableCollection<Sellings> lst = new ObservableCollection<Sellings>();
            prevInput = currInput;
            currInput = returnedProduct.Text;
            if (!string.IsNullOrEmpty(currInput)) prevInput = currInput;
           
            var i = (from item in sellingList
                     where item.ProductName.ToLower().Contains(prevInput.ToLower())
                     || item.ProductId.ToLower().Contains(prevInput.ToLower())
                     select item
                    ).FirstOrDefault();
            if (lst.Count > 5) lst.Clear();
            lst.Add(i);
            returnedProduct.ItemsSource = lst;


            returnedProduct.IsDropDownOpen = true;


        }



        private  async void Restore(object sender, RoutedEventArgs e)
        {
            bool done = false;



            DateTime? theDate = (DateTime?)null;
            if (!string.IsNullOrEmpty(date))
            {
                try
                {
                    theDate = DateTime.Parse(date);
                }
                catch (Exception)
                {
                    throw;
                }

            }
            if (!Object.ReferenceEquals(returnedProduct.ItemsSource, null))
            {
                foreach (Sellings i in returnedProduct.ItemsSource)
                {

                    if (i.ProductName == lst || i.ProductId == lst)
                    {
                        
                        done = await Sellings.RestoreSelling(i.ProductId, null, theDate);

                        if (done)
                        {
                            returnedProduct.Text = "";
                            lst = "";
                            return;
                        }
                    }
                }
            }
            
            lst = "";
            if (!Object.ReferenceEquals(returnedProduct.SelectedValue, null))
            {
                
                Sellings thing = (Sellings)returnedProduct.SelectedValue;
                if (thing.ProductId != null || thing.ProductName != null)
                {
                    done = await Sellings.RestoreSelling(thing.ProductId, thing.ProductName, theDate);
                    if (!done)
                    {
                        done = await Sellings.RestoreSelling(thing.ProductId, thing.ProductName, theDate);
                        if (!done)
                        {
                            MessageBox.Show("no Product was found");
                            return;
                        }
                        else
                        {
                            returnedProduct.Text = "";
                            return;
                        }
                    }
                    else
                    {
                        returnedProduct.Text = "";
                        return;
                    }
                }
              
            }
                
            
           
      
           else if (!String.IsNullOrEmpty(returnedProduct.Text))
            {
               

                done = await Sellings.RestoreSelling(null, returnedProduct.Text, theDate);
                if (!done)
                {
                    done =await Sellings.RestoreSelling(returnedProduct.Text,null, theDate);
                    if (!done)
                    {
                        MessageBox.Show("no Product was found");
                    }
                    else
                    {
                        returnedProduct.Text = "";
                    }
                }
                else
                {
                    returnedProduct.Text = "";
                }
            }
            else
            {
                MessageBox.Show("no valid inputs");
            }



        }

       

        private void ManualOrBarSearch(object sender, RoutedEventArgs e)
        {
            var radioBtn = sender as CheckBox;

            if (radioBtn.IsChecked == true)
            {

                byName = true;

            }
            else if (radioBtn.IsChecked == false)
            {
                byName = false;

            }
        }

        private void BarInputs(object sender, KeyEventArgs e)
        {
            
            if (e.Key == Key.Enter || e.Key== Key.Return)
            {
                returnedProduct.ItemsSource = (from item in sellingList
                                              where item.ProductName.ToLower().Contains(returnedProduct.Text.ToLower())
                                              || item.ProductId.ToLower().Contains(returnedProduct.Text.ToLower())
                                              select item).TakeLast(5);
                returnedProduct.IsDropDownOpen = true;
            }
           /* else
            { 
                if (e.Key == Key.Back || e.Key== Key.Delete)
                {
                    if (lst.Length>0) lst = lst.Remove(lst.Length - 1);

                }
                if (e.Key.ToString().Length < 3)
                {
                    var keyStroke = e.Key.ToString();
                    if (keyStroke.Contains('D') && keyStroke.Length > 1)
                    {
                        keyStroke = e.Key.ToString().Substring(1);
                    }

                    lst += keyStroke;
                }
                if (String.IsNullOrEmpty(returnedProduct.Text))
                {
                    lst = "";
                }
                   
                
            }
           */
        }
        
        private void EnterPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
              
                if (!string.IsNullOrEmpty(returnedProduct.Text))
                {
                    returnedProduct.SelectedIndex = 0;
                }
            }
        }

        private void SetNewDate(object sender, SelectionChangedEventArgs e)
        {

            date = dateCbx.Text;
            DateTime theDate;
            sellingList = new ObservableCollection<Sellings>();
            returnedProduct.ClearValue(ComboBox.TextProperty);
            if (!String.IsNullOrEmpty(date))
            {
                bool done = DateTime.TryParse(date, out theDate);
                if (done)
                {
                    sellingList = Sellings.GetSellingToday(theDate);

                }

            }
        }
    }
    }

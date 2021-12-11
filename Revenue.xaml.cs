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
    /// Interaction logic for Revenue.xaml
    /// </summary>
    public partial class Revenue : UserControl
    {
        public string typeName { get; set; }
        ObservableCollection<Operation> operations = new ObservableCollection<Operation>();
        public Revenue()
        {
            InitializeComponent();
            List<Object> lst = new List<object>();
            lst.Add(new { typeName = "sellings"});
            lst.Add( new { typeName = "boxes"});

            filterName.ItemsSource = lst;
            specifaction.ItemsSource = Box.GetAllBox();
           
        }

        private void SelectDate(object sender, SelectionChangedEventArgs e)
        {
            var ele = sender as DatePicker;
         
        }

        private void SelectType(object sender, SelectionChangedEventArgs e)
        {
            var cmbx = sender as ComboBox;
           
            var item = cmbx.SelectedItem.GetType().GetProperty("typeName");
            string name = (string)item.GetValue(cmbx.SelectedItem, null);
          
            if (name == "sellings")
            {
                specifaction.IsEnabled = false;
                
                specifaction.IsDropDownOpen = false;
                specifaction.IsEditable = false;
                Translator myTranslator = new Translator();
                Binding myBinding = new Binding("ProductId");
                productId.Header = "العنصر Id";
                productId.Binding = myBinding;

                Binding bind1 = new Binding("ProductName");
                productName.Header = " العنصر";
                productName.Binding = bind1;

                Binding bind2 = new Binding("SellingPrice");
                price.Header = " السعرالمبيع";
                price.Binding = bind2;

                Binding bind3 = new Binding("Number");
                Number.Header = "العدد";
                Number.Binding = bind3;

                expense.Visibility = Visibility.Visible;
                Binding myBinding4 = new Binding("Gain");
                myBinding4.Converter = myTranslator;
                expense.Binding = myBinding4;
                expense.Header = "الربح";
            }
            else
            {
                specifaction.IsEnabled = true;
                specifaction.IsReadOnly = false;
                specifaction.IsDropDownOpen = true;
                specifaction.IsEditable = true;
                Translator myTranslator = new Translator();
                Binding myBinding =new Binding("Name");
                productId.Header = "الخزينة";
                productId.Binding = myBinding;

                Binding bind1 = new Binding("Type");

                bind1.Converter = myTranslator;
                productName.Header = "النوع";
                productName.Binding = bind1;
                
                Binding bind2 = new Binding("MoneyAmmount");
                price.Header = "المبلغ";
                price.Binding = bind2;

                Binding bind3 = new Binding("CostPrice");
                Number.Header = " الكلفة";
                Number.Binding = bind3;

                expense.Visibility = Visibility.Visible;
                Binding myBinding4 = new Binding("ExpenseSource");
                myBinding4.Converter = myTranslator;
                expense.Binding = myBinding4;
                expense.Header = "مصدر السحوبات";

            }
        }

        private  void Search(object sender, RoutedEventArgs e)
        {
            if (filterName.SelectedItem != null)
            {
                var item = filterName.SelectedItem.GetType().GetProperty("typeName");
                string name = (string)item.GetValue(filterName.SelectedItem, null);
                if (name == "boxes")
                {
                    if (!string.IsNullOrEmpty(myDate.Text))
                    {
                        DateTime date = DateTime.Parse(myDate.Text);
                        
                        if (!string.IsNullOrEmpty(specifaction.Text))
                        {
                            string spec = specifaction.Text;
                            operations = Operation.GetOperationsByDateName(spec, date);
                        }
                        else
                        {
                            operations = Operation.GetOperationsByDate(date);
                        }
                      

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(specifaction.Text))
                        {
                            string spec = specifaction.Text;
                            operations = Operation.GetLastOperationsByName(spec);
                        }
                        else
                        {
                            operations = Operation.GetLastOperations();
                        }
                    }
                    myRecordGrid.ItemsSource = operations;
                }
                else
                {
                    if (!string.IsNullOrEmpty(myDate.Text))
                    {
                        DateTime date = DateTime.Parse(myDate.Text);
                        myRecordGrid.ItemsSource =  Sellings.GetSellingToday(date);
                    }
                    else
                    {
                        myRecordGrid.ItemsSource = Sellings.GetSellings();
                    }
                }
               
            }
        }

        private void ConvertRecordGain(object sender, RoutedEventArgs e)
        {
            if (myTab.SelectedIndex == 0)
            {
                myTab.SelectedIndex = 1;
            }
            else {
                myTab.SelectedIndex = 0;
            }
        }

        private void RefreshRecords(object sender, RoutedEventArgs e)
        {
            specifaction.ItemsSource = Box.GetAllBox();
        }
    }
}

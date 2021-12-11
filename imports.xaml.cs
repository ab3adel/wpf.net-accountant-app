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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ComponentModel;
namespace accountant
{
    /// <summary>
    /// Interaction logic for imports.xaml
    /// </summary>
    public partial class imports : UserControl
    {
        ObservableCollection<Box> boxes = new ObservableCollection<Box>();
        ObservableCollection<Operation> operations = new ObservableCollection<Operation>();
   
        public imports()
        {
            InitializeComponent();
            ObservableCollection<Box> boxes = new ObservableCollection<Box>();
            boxes = Box.GetAllBox();
            myDataGrid.ItemsSource = boxes;
         
      
           
        }

     
        private void SearchBox(object sender, TextChangedEventArgs e)
        {
            ObservableCollection<Box> boxesName = new ObservableCollection<Box>();
            searchBoxCmbx.ItemsSource = from item in Box.GetAllBox()
                                        where item.BoxName.ToLower().Contains(searchBoxCmbx.Text.ToLower())
                                        select item;
            searchBoxCmbx.IsDropDownOpen = true;

        }

        private void GotBox(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
               
                if (searchBoxCmbx.Text != "")
                {
                    boxes = Box.GetBoxByBoxNaem(searchBoxCmbx.Text);
                    myDataGrid.ItemsSource = boxes;
                }
                else
                {
                    boxes = Box.GetAllBox();
               
                    myDataGrid.ItemsSource = boxes;
                }
               
                
            }
            
        }

        private void OperationsHandler(object sender, RoutedEventArgs e)
        {
            
            BoxOperations w = new BoxOperations();
            
            w.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            w.ShowDialog();
            boxes = Box.GetAllBox();
            myDataGrid.ItemsSource = boxes;
    
        }

        private void ShowLastOperation(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
          
            if (myTab.SelectedIndex == 0)
            {
                myTab.SelectedIndex = 1;
                boxOperationsbtn.Visibility = Visibility.Hidden;
                cmbxStack.Visibility = Visibility.Hidden;
                btn.Content = "الرجوع ";
                operations = Operation.GetLastOperations();
                
                myDataGrid1.ItemsSource = operations;
            }
            else
            {
                myTab.SelectedIndex = 0;
                boxOperationsbtn.Visibility = Visibility.Visible;
                cmbxStack.Visibility = Visibility.Visible;
                btn.Content = "أخر العمليات ";
               
            }
        }

        private void ShowOperation(object sender, SelectedCellsChangedEventArgs e)
        {
            if (myDataGrid.SelectedItem != null)
            {
                Box item = myDataGrid.SelectedItem as Box;
                BoxOperations.boxName = item;
                BoxOperations w = new BoxOperations();
                w.ShowDialog();
                boxes = Box.GetAllBox();
                myDataGrid.ItemsSource =boxes;

            }


        }

        private void Search(object sender, RoutedEventArgs e)
        {

            if (!string.IsNullOrEmpty(searchBoxCmbx.Text) )
            {
                boxes = Box.GetBoxByBoxNaem(searchBoxCmbx.Text);
                myDataGrid.ItemsSource = boxes;
            }
            else
            {
                boxes = Box.GetAllBox();

                myDataGrid.ItemsSource = boxes;
            }

        }
    }
}

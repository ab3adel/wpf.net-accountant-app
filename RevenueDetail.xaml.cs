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
    /// Interaction logic for RevenueDetail.xaml
    /// </summary>
    public partial class RevenueDetail : UserControl
    {
       public  int monthNumber = 0;
        public int yearNumber = 0;
        ObservableCollection<Box> boxes = new ObservableCollection<Box>();

       
        public RevenueDetail()
        {
            InitializeComponent();
            List<Object> months = new List<Object>();
            List<Object> years = new List<Object>();
         
            for (int i=1; i < 13; i++)
            {
                months.Add(new { monthNumber = i });
                years.Add(new { yearNumber = 2019 + i });
            }
            cmbxMonth.ItemsSource = months;
            cmbxYear.ItemsSource = years;
        }

        private void Search(object sender, RoutedEventArgs e)
        {
            if (monthNumber != 0 && yearNumber != 0)
            {
               
                boxes = Operation.GetTotalGainCostPerMonth(yearNumber, monthNumber);
                myDataGrid.ItemsSource = boxes;
            }
        }

        private void ShowDetail(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
           
            if (myTab1.SelectedIndex == 0)
            {
                myTab1.SelectedIndex = 1;
                Box item = myDataGrid.SelectedItem as Box;
                ObservableCollection<Box> boxes = Operation.GetTotalGainCostPerDay(yearNumber, monthNumber, item.BoxName);
               
                detailDataGrid.ItemsSource = boxes;
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            myTab1.SelectedIndex = 0;
        }

        private void SetMonth(object sender, SelectionChangedEventArgs e)
        {

          var item = cmbxMonth.SelectedItem.GetType().GetProperty("monthNumber");
            monthNumber = (int)item.GetValue(cmbxMonth.SelectedItem, null);

        }

        private void SetYear(object sender, SelectionChangedEventArgs e)
        {
            var item = cmbxYear.SelectedItem.GetType().GetProperty("yearNumber");
             yearNumber = (int)item.GetValue(cmbxYear.SelectedItem, null);
        }
    }
}

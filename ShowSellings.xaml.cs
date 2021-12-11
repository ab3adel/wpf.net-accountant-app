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
using System.Collections.ObjectModel;
using System.Diagnostics;
namespace accountant
{
    /// <summary>
    /// Interaction logic for ShowSellings.xaml
    /// </summary>
    public partial class ShowSellings : UserControl
    {
        ObservableCollection<Sellings> sellingsList = new ObservableCollection<Sellings>();
        public  ShowSellings()
        {
            InitializeComponent();
            DateTime date = DateTime.Today.Date;
                
          
            myDataGrid.ItemsSource = sellingsList;
           
        }

        private  void RefreshSellings(object sender, RoutedEventArgs e)
        {
            DateTime date = DateTime.Today.Date;
            sellingsList = Sellings .GetSellingToday(date);
            myDataGrid.ItemsSource = sellingsList;
        }
    }
}

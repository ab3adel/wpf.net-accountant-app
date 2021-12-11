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
namespace accountant
{
    /// <summary>
    /// Interaction logic for BoxOperations.xaml
    /// </summary>
    public partial class BoxOperations : Window
    {
        public static Box boxName = new Box () ;
        public BoxOperations()
        {
            InitializeComponent();
            Uri iconUri = new Uri("pack://application:,,,/Images/favicon.ico", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);
            ObservableCollection<Box> lst = new ObservableCollection<Box>();
            lst = Box.GetAllBox();
            List<string> sources = new List <string>();
            sources.Add("cost");
            sources.Add("gain");
            boxNameCmbx.ItemsSource = lst;
            sourceCmbx.ItemsSource = sources;
            if (boxName != null)
            {
                boxNameCmbx.Text = boxName.BoxName;
                boxNameCmbx1.Text = boxName.BoxName;
                if (boxName.BoxName == "Store")
                {
                    boxNameCmbx.IsEnabled = false;
                    ammount.IsEnabled = false;
                    cost.IsEnabled = false;
                    addOperation.IsEnabled = false;
                    cancel.IsEnabled = false;
                }
            }
            else
            {
                ObservableCollection<Box> boxes = new ObservableCollection<Box>();
                boxes = Box.GetAllBox();
                boxNameCmbx.ItemsSource = boxes;
                boxNameCmbx1.ItemsSource = boxes;
            }

        }

        private  void InsertOperation(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Operation operation = new Operation();
            bool done = false;
           
            if (btn.Content.ToString() == "أدخل")
            {
                if ( !string.IsNullOrEmpty (boxNameCmbx.Text)  && !string.IsNullOrEmpty (ammount.Text)  && !string.IsNullOrEmpty (cost.Text) && ammount.Text != "0")
                {
                    

                    operation = new Operation()
                    {
                        Type = "revenue",
                        MoneyAmmount = Int32.Parse(ammount.Text),
                        CostPrice = Int32.Parse(cost.Text),
                        Name = boxNameCmbx.Text,
                        Inserted = DateTime.Today.Date

                    };

                }
            }
            else
            {
                if (!string.IsNullOrEmpty(boxNameCmbx1.Text)  && !string.IsNullOrEmpty(ammount1.Text) && !string.IsNullOrEmpty(sourceCmbx.Text) && ammount1.Text !="0") { 
                    operation = new Operation()
                    {
                        Type = "expense",
                        MoneyAmmount = Int32.Parse(ammount1.Text),
                        ExpenseSource = sourceCmbx.Text,
                        Name = boxNameCmbx.Text,
                        Inserted = DateTime.Today.Date

                    };
                }
            }
                done =  Operation.SetOperation(operation);
                if (!done)
                {
                    MessageBox.Show("invalid inputs");
                }
                else
                {
                if (ammount.Text != null)
                {
                    
                    cost.Clear();
                    ammount.Clear();
                }
                 if (ammount1.Text != null)
                {
                 
                    
                    sourceCmbx.Text="";
                    ammount1.Clear();
                }
                }
            }
        

        private void CancelHandler(object sender, RoutedEventArgs e)
        {
          
        }

        private void GetBoxeName(object sender, EventArgs e)
        {
            Binding myBinding = new Binding("BoxName");
            myBinding.Source = Box.GetAllBoxNames();
            boxNameCmbx.SetBinding(ComboBox.ItemsPanelProperty, myBinding);
            Binding myBinding1 = new Binding("BoxName");
            myBinding1.Source = Box.GetAllBoxNames();

            boxNameCmbx1.SetBinding(ComboBox.ItemsPanelProperty, myBinding1);
        }
    }
}

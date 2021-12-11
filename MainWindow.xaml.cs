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
using System.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Collections;
using System.Windows.Media.Animation;

namespace accountant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Storyboard myStoryBoard = new Storyboard();
        public static int  authId = -1;
     
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(WindowLoaded);
            Uri iconUri = new Uri("pack://application:,,,/Images/favicon.ico", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);
            
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {

            // Authentication.DeleteAll();
            // Product.DeleteAll();
            // Sellings.DeleteAll();
            // Operation.DeleteAll();
           //TestClass.DummyDataProduct();
            //TestClass.DummyDataSellings();
            //TestClass.DummyDataBoxOperation();
           
            SqlliteIntializer init = new SqlliteIntializer();
            init.createTables("create");

            authentication w = new authentication();
           
           if (!string.IsNullOrEmpty(Authentication.CheckAny()))
            {
                w.myTab.SelectedIndex = 1;
                w.ShowDialog();
                this.CheckAuthentication();
            }
           else
            {
                w.myTab.SelectedIndex = 0;
                w.ShowDialog();
            }
           
         
      
        }


        public  void CheckAuthentication ()
        {
           
            if (authId ==-1)
            {
                this.storebtn.IsEnabled = false;
                this.revenueBtn.IsEnabled = false;
                this.boxBtn.IsEnabled = false;
                this.adminBtn.IsEnabled = false;
                this.tabControl.SelectedIndex = 1;

            }
            else
            {
                
                this.storebtn.IsEnabled = true;
                this.revenueBtn.IsEnabled = true;
                this.boxBtn.IsEnabled = true;
                this.adminBtn.IsEnabled = true;
                
            }
        }
        private void Salary_Section(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 1;
        }

        private void Store_Section(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 0;
        }

        private void MouseUp_Handler(object sender, MouseButtonEventArgs e)
        {
            Store s = new Store();
          
        }

        private void Revenue_Section(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 3;
        }

       

        
        private void Box_Section(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 2;
        }

        private void Admin_Panel(object sender, RoutedEventArgs e)
        {
            tabControl.SelectedIndex = 4;
        }

        private void Security_Section(object sender, RoutedEventArgs e)
        {
            authentication w = new authentication();
            if (!String.IsNullOrEmpty(Authentication.CheckAny()) )
            {
               if (authId > -1)
                {
                   
                    if (!storebtn.IsEnabled)
                    {
                        
                        authId = -1;
                        w.myTab.SelectedIndex = 1;
                        w.ShowDialog();
                        this.CheckAuthentication();
                      

                    }
                    else
                    {
                        this.storebtn.IsEnabled = false;
                        this.revenueBtn.IsEnabled = false;
                        this.boxBtn.IsEnabled = false;
                        this.adminBtn.IsEnabled = false;
                        this.tabControl.SelectedIndex = 1;
                    }
               
                }
               else
                {
                    w.myTab.SelectedIndex = 1;
                    w.ShowDialog();
                    this.CheckAuthentication();
                }
                
            }
            else
            {
                
                w.myTab.SelectedIndex = 0;
                w.ShowDialog();
            }
        }
    }
   
}

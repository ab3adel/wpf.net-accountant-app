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

namespace accountant
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : UserControl
    {
        static bool Isadmin = false;
        public Admin()
        {
            InitializeComponent();
        
        }

        private void NewPassWord(object sender, RoutedEventArgs e)
        {
            var btn = sender as CheckBox;
            MainWindow.authId = -1;
            authentication w = new authentication();
            w.myTab.SelectedIndex = 1;
            w.ShowDialog();
            if (MainWindow.authId > -1)
            {
                w = new authentication();
                w.myTab.SelectedIndex = 0;
                w.ShowDialog();
                
               
            }
            else
            {
               
            }

        }

        private void DeleteBoxes(object sender, RoutedEventArgs e)
        {
            var btn = sender as CheckBox;
            MainWindow.authId = -1;
            authentication w = new authentication();
            w.myTab.SelectedIndex = 1;
            w.ShowDialog();
            if (MainWindow.authId > -1)
            {
               
               bool done = Box.Drop();
                if (done)
                {
                    MessageBox.Show("تم");
                }
            }
            else
            {
                btn.IsChecked = false;
            }


        }

        private void DeleteSellings(object sender, RoutedEventArgs e)
        {
            var btn = sender as CheckBox;
            MainWindow.authId = -1;
            authentication w = new authentication();
            w.myTab.SelectedIndex = 1;
            w.ShowDialog();
            if (MainWindow.authId > -1)
            {
               
                bool done =Sellings.Drop();
                if (done)
                {
                    MessageBox.Show("تم");
                }
            }
            else
            {
                btn.IsChecked = false;
            }
        }

        private void DeleteOperations(object sender, RoutedEventArgs e)
        {
            var btn = sender as CheckBox;
            MainWindow.authId = -1;
            authentication w = new authentication();
            w.myTab.SelectedIndex = 1;
            w.ShowDialog();
            if (MainWindow.authId > -1)
            {
               
              bool done=  Operation.Drop();
                if (done)
                {
                    MessageBox.Show("تم");
                }
            }
            else
            {
                btn.IsChecked = false;
            }
        }

        private void DeleteProducts(object sender, RoutedEventArgs e)
        {
            var btn = sender as CheckBox;
            MainWindow.authId = -1;
            authentication w = new authentication();
            w.myTab.SelectedIndex = 1;
            w.ShowDialog();
            if (MainWindow.authId > -1)
            {
                
              bool done=  Product.Drop();
                if (done)
                {
                    MessageBox.Show("تم");
                }
            }
            else
            {
                btn.IsChecked = false;
            }
        }
    }
}

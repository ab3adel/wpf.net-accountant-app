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

namespace accountant
{
    /// <summary>
    /// Interaction logic for AddBox.xaml
    /// </summary>
    public partial class AddBox : Window
    {
        public AddBox()
        {
            InitializeComponent();
            Uri iconUri = new Uri("pack://application:,,,/Images/favicon.ico", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);

        }

        private void Add(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine(boxName.Text);
           
            bool parsed = Int32.TryParse(totallAmmount.Text,out int totall);
            if (!parsed)
            {
                totall = 0;
            }
            if (!string.IsNullOrEmpty(boxName.Text))
            {
                Box box = new Box()
                {
                    BoxName = boxName.Text,
                    TotallAmmount = totall,
                    TotallGain = 0,
                    Created = DateTime.Today.Date

                };
                bool done = Box.SetBox(box);
                if (done)
                {
                    boxName.Clear();
                    totallAmmount.Clear();
                }
                else
                {
                    MessageBox.Show("البيانات المدخلة غير صالحة");

                }
            }
           
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
    }
}

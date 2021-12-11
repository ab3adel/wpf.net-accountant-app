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
    /// Interaction logic for authentication.xaml
    /// </summary>
    public partial class authentication : Window
    {
         
        public static string pass = "";
        public authentication()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(WindowLoaded);
            Uri iconUri = new Uri("pack://application:,,,/Images/favicon.ico", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);
        }
        private void WindowLoaded (object sender ,RoutedEventArgs e)
        {
           
            //Authentication.DeleteAll();
         
        }
        private void ConfirmPassword(object sender, RoutedEventArgs e)
        {

            
        }

        private void SetPassword(object sender, RoutedEventArgs e)

        {
            

            if ( !Validation.GetHasError(newPassword))
            {
                if ( string.IsNullOrEmpty(Authentication.CheckAny()) )

                {
                    
                    if (confirmPassowrd.Password != newPassword.Password)
                    {
                        
                        MessageBox.Show("كلمة المرور والتأكيد غير متطابقتين");
                        return;
                    }
                    bool done = false;
                    done = Authentication.SetAuth(new Authentication()
                    {
                        UserName = userName.Text,
                        Password = newPassword.Password
                    });

                    if (!done)
                    {
                        MessageBox.Show("حدث خطأ ما ");
                    }
                    else
                    {
                        userName.Clear();
                        confirmPassowrd.Clear();
                        newPassword.Clear();
                        this.Close();
                    }
                }
                else
                {
                    if (confirmPassowrd.Password != newPassword.Password)
                    {

                        MessageBox.Show("كلمة المرور والتأكيد غير متطابقتين");
                        return;
                    }
                    bool done = false;
                    done = Authentication.UpdateAuth(new Authentication()
                    {
                        UserName = userName.Text,
                        Password = newPassword.Password,
                        ID=MainWindow.authId
                    });

                    if (!done)
                    {
                        MessageBox.Show("حدث خطأ ما ");
                    }
                    else
                    {
                        userName.Clear();
                        confirmPassowrd.Clear();
                        newPassword.Clear();
                        this.Close();
                    }
                }
            }
        }

      
        

        private void Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();

        }

        private void CheckAuthentication(object sender, RoutedEventArgs e)
        {
            
            
            if (!string.IsNullOrEmpty(password.Password) && !string.IsNullOrEmpty(Name.Text))
            {
                int id = Authentication.CheckAuth(new Authentication()
                {
                    UserName = Name.Text,
                    Password = password.Password
                });
               
                if (id <0)
                {
                    MessageBox.Show("اسم المستخدم او كلمة المرور خاطئة");
                }
                else
                {
                    MainWindow.authId = id;
                  
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("لا يمكنك ترك احد الحقول فارغا");
            }
        }

       
    }
}

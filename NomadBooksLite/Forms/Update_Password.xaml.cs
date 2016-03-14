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

namespace NomadBooksLite
{
    /// <summary>
    /// Interaction logic for Update_Password.xaml
    /// </summary>
    public partial class Update_Password : Window
    {
        int user_id_x;

        public Update_Password(int user_id)
        {
            InitializeComponent();
            user_id_x = user_id;
        }

        private void Password_Submit(object sender, RoutedEventArgs e)
        {
            string password1 = passwordTextBox1.Password;
            string password2 = passwordTextBox2.Password;

            if (password1 == password2)
            {
                UserDatabase.UpdatePassword(user_id_x, password1);
                this.Close();

                MessageBox.Show("Please login again using your new password.");
            }
            else {
                MessageBox.Show("The passwords you have entered do not match, please try again.");
                passwordTextBox1.Password = "";
                passwordTextBox2.Password = "";
            }
        }
    }
}

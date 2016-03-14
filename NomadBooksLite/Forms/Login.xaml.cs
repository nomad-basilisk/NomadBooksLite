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

using System.IO;
using NomadBooksLite.Forms;

namespace NomadBooksLite
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        Tuple<int, int> userDetails;

        public Login()
        {
            InitializeComponent();

            OnlineStatus.status = false;

            //do online checks
            if (ConnectivityCheck.IsInternetConnection())
            {
                OnlineStatus.status = true;

                StatusLabel.Content = "Online.";
                var bc = new BrushConverter();
                StatusLabel.Background = (Brush)bc.ConvertFrom("#ccffcc");
            }
            else
            {
                OnlineStatus.status = false;

                StatusLabel.Content = "Offline.";
                var bc = new BrushConverter();
                StatusLabel.Background = (Brush)bc.ConvertFrom("#ffcccc");
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string username = username_textbox.Text;
            string password = password_textbox.Password;

            bool result = UserDatabase.AddUser(username, password);
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = username_textbox.Text;
            string password = password_textbox.Password;

            //Use wait cursor as this can take time.
            using (new WaitCursor())
            {
                //If Offline
                if (!OnlineStatus.status) //NOT CONNECTED TO INTERNET
                {
                    if (MessageBox.Show("You are not connected to the internet, would you like to continue in offline mode?",
                        "Continue Offline",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning) == MessageBoxResult.No)
                    {
                        this.Close();
                        Application.Current.Shutdown();
                    }
                    else
                    {
                        userDetails = UserDatabase.GetUserIdByUsernameAndPasswordOffline(username, password);
                    }
                }
                else //CONNECTED TO INTERNET
                {
                    userDetails = UserDatabase.GetUserIdByUsernameAndPassword(username, password);
                    //Tuple returns item 1: userId 
                    //              item 2: access_level
                }
                if (userDetails.Item1 > 0)
                {
                    //agent_id set to -1 as it is only used later on in application 
                    //if an Admin reviews agents stokvels. Might move this out of global variables.
                    if (userDetails.Item2 == 0)
                    {
                        //Stokvel level lite, user will never be admin.
                        //Redundant check.

                    }
                    else if (userDetails.Item2 == 1)
                    {
                        //User is Agent, user will never be agent.
                        //Redundant check.
                    }
                    else if (userDetails.Item2 == 2)
                    {
                        //User is stokvel user.
                        this.Close();

                        Stokvel_Dashboard sd = new Stokvel_Dashboard();
                        sd.Show();
                    }
                }
            }
        }

        private void username_textbox_MouseEnter(object sender, MouseEventArgs e)
        {
            username_textbox.Text = "";
        }
    }
}

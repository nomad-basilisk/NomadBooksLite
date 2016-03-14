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

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Threading;
using NomadBooksLite.Tools;

namespace NomadBooksLite.Forms.ConstitutionWizard
    
{
    /// <summary>
    /// Interaction logic for Constitution_Wizard_2.xaml
    /// </summary>
    public partial class Constitution_Wizard_2 : Window
    {
        string stokvel_id;
        string name;
        int purpose;
        string joining_fee;
        string contributions;
        string main_id;

        public Constitution_Wizard_2(string user_id, string stokvel_id_x, string name_x, int purpose_x, string joining_fee_x, string contributions_x)
        {
            InitializeComponent();

            //set user id
            main_id = user_id;

            //set stokvel id
            stokvel_id = stokvel_id_x;
            name = name_x;
            purpose = purpose_x;
            joining_fee = joining_fee_x;
            contributions = contributions_x;

            //do online checks
            if (ConnectivityCheck.IsInternetConnection())
            {
                FillDataGrid();
            }
            else
            {
                FillDataGrid_Offline();
            }
        }

        private void FillDataGrid()
        {

            var connectionString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;

            string CmdString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                CmdString = String.Format("select firstname, lastname, contact_number, email, id_number, address, dob, CASE WHEN mo.verified = 1 THEN 'YES' ELSE 'NO' END AS 'Verified' from member m join member_otp mo on mo.member_id = m.id where stokvel_id = {0}", stokvel_id);
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Members");
                sda.Fill(dt);

                //Write datatable to JSON
                JsonStringWriter jsw = new JsonStringWriter();
                jsw.DataTableToJSONWithJSONNet(dt, main_id, "Constitution_Wizard_2", "MemberGrid");

                membersGrd.ItemsSource = dt.DefaultView;
            }
        }

        public void FillDataGrid_Offline()
        {
            membersGrd.ItemsSource = null;

            JsonStringWriter jsw = new JsonStringWriter();
            DataTable dt1 = jsw.JSONtoDataTableWithJSONNet(main_id, "Constitution_Wizard_2", "MemberGrid");

            membersGrd.ItemsSource = dt1.DefaultView;
        }

        private void Refresh_Grid(object sender, RoutedEventArgs e)
        {
            FillDataGrid();
        }

        private void AddMember(object sender, RoutedEventArgs e) 
        {
            Add_Member am = new Add_Member(stokvel_id, main_id, true);
            am.Show();
        }

        private void NextStep(object sender, RoutedEventArgs e)
        {
            ConstitutionGenerator c = new ConstitutionGenerator();

            string purposeStr = "";
            //do online checks and get purpose
            if (ConnectivityCheck.IsInternetConnection())
            {
                purposeStr = return_purpose(purpose);
            }
            else
            {
                purposeStr = return_purpose_offline(purpose);
            }

            c.CreateSampleDocument(stokvel_id, name, purposeStr, joining_fee, contributions);

            this.Close();
        }

        private void CancelSetup(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private string return_purpose(int purpose_id)
        {
            string purp = "";
            var connectionString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;

            string CmdString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                CmdString = String.Format("select purpose from purpose where id = {0}", purpose_id);
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Purpose");
                sda.Fill(dt);

                //Write datatable to JSON
                JsonStringWriter jsw = new JsonStringWriter();
                jsw.DataTableToJSONWithJSONNet(dt, main_id, "Constitution_Wizard_2", "Purpose");

                foreach (DataRow dr in dt.Rows)
                {
                    purp = dr["purpose"].ToString();
                }
            }

            return purp;
        }

        private string return_purpose_offline(int purpose_id)
        {
            string purp = "";

            JsonStringWriter jsw = new JsonStringWriter();
            DataTable dt1 = jsw.JSONtoDataTableWithJSONNet(main_id, "Constitution_Wizard_2", "Purpose");

            foreach (DataRow dr in dt1.Rows)
            {
                purp = dr["purpose"].ToString();
            }

            return purp;
        }
    }
}

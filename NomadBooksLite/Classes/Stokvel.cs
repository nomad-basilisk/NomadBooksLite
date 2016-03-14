using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//2016/01/13 -- Added by Mark 
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Configuration;

namespace NomadBooksLite.Forms
{
    public class Stokvel
    {
        public int id { get; set; }
        public string user_id { get; set; }
        public string name { get; set; }
        public int purpose { get; set; }
        public decimal contribution_amount { get; set; }
        public decimal joining_fee { get; set; }
        public decimal opening_bal { get; set; }
        public DateTime inception_date { get; set; }
        public DateTime opening_balance_date { get; set; }
        public int chairman { get; set; }

        public String insert()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("stokvel_insert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter nameParam = new SqlParameter("@name", SqlDbType.VarChar, 250);
            SqlParameter contributionParam = new SqlParameter("@contribution_amount", SqlDbType.Money);
            SqlParameter joiningFeeParam = new SqlParameter("@joining_fee", SqlDbType.Money);
            SqlParameter openingBalParam = new SqlParameter("@opening_balance", SqlDbType.Money);
            SqlParameter purposeParam = new SqlParameter("@purpose", SqlDbType.Int);
            SqlParameter inceptionDateParam = new SqlParameter("@inception_date", SqlDbType.DateTime);
            SqlParameter user_idParam = new SqlParameter("@user_id", SqlDbType.BigInt);

            nameParam.Value = name;
            contributionParam.Value = contribution_amount;
            joiningFeeParam.Value = joining_fee;
            openingBalParam.Value = opening_bal;
            purposeParam.Value = purpose;
            inceptionDateParam.Value = inception_date;
            user_idParam.Value = user_id;

            cmd.Parameters.Add(nameParam);
            cmd.Parameters.Add(contributionParam);
            cmd.Parameters.Add(joiningFeeParam);
            cmd.Parameters.Add(openingBalParam);
            cmd.Parameters.Add(purposeParam);
            cmd.Parameters.Add(inceptionDateParam);
            cmd.Parameters.Add(user_idParam);

            try
            {
                con.Open();
                var rowCount = cmd.ExecuteScalar();
                MessageBox.Show(String.Format("Record {0} inserted", rowCount));

                return rowCount.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.ToString());
                return ex.ToString();
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        public bool remove() 
        {
            var connectionString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("stokvel_remove", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter stokvel_idParam = new SqlParameter("@stokvel_id", SqlDbType.BigInt);

            stokvel_idParam.Value = id;
            
            cmd.Parameters.Add(stokvel_idParam);

            try
            {
                con.Open();
                var rowCount = cmd.ExecuteScalar();
                MessageBox.Show("Stokvel succesfully removed");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.ToString());
                return false;
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }

        public void update_name()
        {
            Console.WriteLine(name);
            //Parameter and StoredProc for NAME update.
            SqlParameter param = new SqlParameter("@name", SqlDbType.VarChar);
            param.Value = name;
            string storedProc = "stokvel_name_update";

            update_variable(param, storedProc);
        }

        public void update_purpose()
        {
            //Parameter and StoredProc for PURPOSE update.
            SqlParameter param = new SqlParameter("@purpose", SqlDbType.Int);
            param.Value = purpose;
            string storedProc = "stokvel_purpose_update";

            update_variable(param, storedProc);
        }

        public void update_contribution()
        {
            //Parameter and StoredProc for CONTRIBUTION update.
            SqlParameter param = new SqlParameter("@contribution", SqlDbType.Money);
            param.Value = contribution_amount;
            string storedProc = "stokvel_contribution_update";

            update_variable(param, storedProc);
        }

        public void update_joiningFee()
        {
            //Parameter and StoredProc for JOINING FEE update.
            SqlParameter param = new SqlParameter("@joiningFee", SqlDbType.Money);
            param.Value = joining_fee;
            string storedProc = "stokvel_joiningFee_update";

            update_variable(param, storedProc);
        }

        public void update_openingBal()
        {
            //Parameter and StoredProc for OPENING BAL update.
            SqlParameter param = new SqlParameter("@openingBal", SqlDbType.Money);
            param.Value = opening_bal;
            string storedProc = "stokvel_openingBal_update";

            update_variable(param, storedProc);
        }

        public void update_openingBalDate()
        {
            //Parameter and StoredProc for OPENING BAL DATE update.
            SqlParameter param = new SqlParameter("@openingBalDate", SqlDbType.DateTime);
            param.Value = opening_balance_date;
            string storedProc = "stokvel_openingBalDate_update";

            update_variable(param, storedProc);
        }

        public void update_inceptionDate()
        {
            //Parameter and StoredProc for INCEPTION DATE update.
            SqlParameter param = new SqlParameter("@inceptionDate", SqlDbType.DateTime);
            param.Value = inception_date;
            string storedProc = "stokvel_inceptionDate_update";

            update_variable(param, storedProc);
        }

        public void update_chairman()
        {
            //Parameter and StoredProc for CHAIRMAN update.
            SqlParameter param = new SqlParameter("@chairman", SqlDbType.Int);
            param.Value = chairman;
            string storedProc = "stokvel_chairman_update";

            update_variable(param, storedProc);
        }

        public void update_offline(string attribute, string value)
        {
            //Parameter and StoredProc for ?????? update.
            SqlParameter param = new SqlParameter("@" + attribute, SqlDbType.Money);
            param.Value = value;
            string storedProc = "stokvel_" + attribute + "_update";
            Console.WriteLine("Running StoredProc : " + storedProc);
            Console.WriteLine("Attribute : @" + attribute);
            Console.WriteLine("Value : " + value);
            Console.WriteLine("ID : " + id);

            update_variable(param, storedProc);
        }

        private void update_variable(SqlParameter variableParam, string storedProc)
        {
            //Rest of method for updating parameters.

            var connectionString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(storedProc, con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter stokvel_idParam = new SqlParameter("@stokvel_id", SqlDbType.BigInt);
            stokvel_idParam.Value = id;

            cmd.Parameters.Add(stokvel_idParam);
            cmd.Parameters.Add(variableParam);

            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.ToString());
            }
            finally
            {
                cmd.Dispose();
                con.Close();
            }
        }
    }

}

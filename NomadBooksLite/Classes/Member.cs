using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using System.Configuration;

namespace NomadBooksLite.Forms
{
    class Member
    {
        public int id { get; set; }
        public string user_id { get; set; }
        public string stokvel_idStr { get; set; }
        public string firstNameStr { get; set; }
        public string lastNameStr { get; set; }
        public string idNumberStr { get; set; }
        public string cellPhoneStr { get; set; }
        public string emailStr { get; set; }
        public string addressStr { get; set; }
        public DateTime dobDt { get; set; }


        public void insert()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("member_insert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter firstName = new SqlParameter("@firstname", SqlDbType.VarChar, 250);
            SqlParameter lastName = new SqlParameter("@lastname", SqlDbType.VarChar, 250);
            SqlParameter cellPhone = new SqlParameter("@contact_number", SqlDbType.VarChar, 250);
            SqlParameter email = new SqlParameter("@email", SqlDbType.VarChar, 250);
            SqlParameter stokvel_id = new SqlParameter("@stokvel_id", SqlDbType.BigInt);
            SqlParameter idNumber = new SqlParameter("@id_number", SqlDbType.VarChar, 250);
            SqlParameter address = new SqlParameter("@address", SqlDbType.VarChar, 250);
            SqlParameter dob = new SqlParameter("@dob", SqlDbType.DateTime);

            firstName.Value = firstNameStr;
            lastName.Value = lastNameStr;
            idNumber.Value = idNumberStr;
            cellPhone.Value = cellPhoneStr;
            email.Value = emailStr;
            stokvel_id.Value = long.Parse(stokvel_idStr);
            address.Value = addressStr;
            dob.Value = dobDt;

            cmd.Parameters.Add(firstName);
            cmd.Parameters.Add(lastName);
            cmd.Parameters.Add(idNumber);
            cmd.Parameters.Add(cellPhone);
            cmd.Parameters.Add(email);
            cmd.Parameters.Add(stokvel_id);
            cmd.Parameters.Add(address);
            cmd.Parameters.Add(dob);

            try
            {
                con.Open();
                var rowCount = cmd.ExecuteScalar();
                StaticMemberOTP.member_id = rowCount.ToString();
                MessageBox.Show("Record inserted");
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

        public void insert_offline()
        {
            List<string> listAttribs = new List<string>();
            listAttribs.Add("entity");
            listAttribs.Add("firstName");
            listAttribs.Add("lastName");
            listAttribs.Add("idNumber");
            listAttribs.Add("cellPhone");
            listAttribs.Add("email");
            listAttribs.Add("stokvel_id");
            listAttribs.Add("address");
            listAttribs.Add("dob");

            List<string> listValues = new List<string>();
            listValues.Add("member");
            listValues.Add(firstNameStr);
            listValues.Add(lastNameStr);
            listValues.Add(idNumberStr);
            listValues.Add(cellPhoneStr);
            listValues.Add(emailStr);
            listValues.Add(stokvel_idStr);
            listValues.Add(addressStr);
            listValues.Add(dobDt.ToString());

            try
            {
                JsonStringWriter jsw = new JsonStringWriter();
                jsw.OfflineInserts("member", listAttribs, listValues);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.ToString());
            }

        }

        public bool remove()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("member_remove", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter stokvel_idParam = new SqlParameter("@member_id", SqlDbType.BigInt);

            stokvel_idParam.Value = id;

            cmd.Parameters.Add(stokvel_idParam);

            try
            {
                con.Open();
                var rowCount = cmd.ExecuteScalar();
                MessageBox.Show("Member succesfully removed");

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

        public void update_first_name()
        {
            //Parameter and StoredProc for NAME update.
            SqlParameter param = new SqlParameter("@firstname", SqlDbType.VarChar);
            param.Value = firstNameStr;
            string storedProc = "member_firstname_update";

            update_variable(param, storedProc);
        }

        public void update_last_name()
        {
            //Parameter and StoredProc for lastname update.
            SqlParameter param = new SqlParameter("@lastname", SqlDbType.VarChar);
            param.Value = lastNameStr;
            string storedProc = "member_lastname_update";

            update_variable(param, storedProc);
        }

        public void update_contact_number()
        {
            //Parameter and StoredProc for contact_number update.
            SqlParameter param = new SqlParameter("@contact_number", SqlDbType.VarChar);
            param.Value = cellPhoneStr;
            string storedProc = "member_contactNumber_update";

            update_variable(param, storedProc);
        }

        public void update_email_number()
        {
            //Parameter and StoredProc for email update.
            SqlParameter param = new SqlParameter("@email", SqlDbType.VarChar);
            param.Value = emailStr;
            string storedProc = "member_email_update";

            update_variable(param, storedProc);
        }

        public void update_idNumber()
        {
            //Parameter and StoredProc for id_number update.
            SqlParameter param = new SqlParameter("@id_number", SqlDbType.BigInt);
            param.Value = idNumberStr;
            string storedProc = "member_idNumber_update";

            update_variable(param, storedProc);
        }

        public void update_address()
        {
            //Parameter and StoredProc for id_number update.
            SqlParameter param = new SqlParameter("@address", SqlDbType.VarChar);
            param.Value = addressStr;
            string storedProc = "member_address_update";

            update_variable(param, storedProc);
        }

        public void update_dob()
        {
            //Parameter and StoredProc for DOB update.
            SqlParameter param = new SqlParameter("@dob", SqlDbType.DateTime);
            param.Value = dobDt;
            string storedProc = "member_dob_update";

            update_variable(param, storedProc);
        }

        private void update_variable(SqlParameter variableParam, string storedProc)
        {
            //Rest of method for updating parameters.

            var connectionString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(storedProc, con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter stokvel_idParam = new SqlParameter("@member_id", SqlDbType.BigInt);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Windows;

namespace NomadBooksLite.Forms
{
    class Record
    {
        public string id { get; set; }
        public string stokvel_id { get; set; }
        public string member_id { get; set; }
        public string type_id { get; set; }
        public string amount { get; set; }
        public DateTime date { get; set; }
        public string repayment_date { get; set; }
        public DateTime date_created { get; set; }
        public string agent_id { get; set; }
        public string edited { get; set; }
        public string deleted { get; set; }

        public void insert()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("record_insert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter stokvel_idParam = new SqlParameter("@stokvel_id", SqlDbType.BigInt);
            SqlParameter member_idParam = new SqlParameter("@member_id", SqlDbType.BigInt);
            SqlParameter type_idParam = new SqlParameter("@type_id", SqlDbType.BigInt);
            SqlParameter amountParam = new SqlParameter("@amount", SqlDbType.Money);
            SqlParameter dateParam = new SqlParameter("@date", SqlDbType.DateTime);
            SqlParameter agent_idParam = new SqlParameter("@agent_id", SqlDbType.BigInt);

            stokvel_idParam.Value = stokvel_id;
            member_idParam.Value = member_id;
            type_idParam.Value = type_id;
            amountParam.Value = amount;
            dateParam.Value = date;
            agent_idParam.Value = agent_id;

            cmd.Parameters.Add(stokvel_idParam);
            cmd.Parameters.Add(member_idParam);
            cmd.Parameters.Add(type_idParam);
            cmd.Parameters.Add(amountParam);
            cmd.Parameters.Add(dateParam);
            cmd.Parameters.Add(agent_idParam);

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

        public void insert_offline()
        {
            List<string> listAttribs = new List<string>();
            listAttribs.Add("entity");
            listAttribs.Add("stokvel_id");
            listAttribs.Add("member_id");
            listAttribs.Add("type_id");
            listAttribs.Add("amount");
            listAttribs.Add("date");
            listAttribs.Add("agent_id");

            List<string> listValues = new List<string>();
            listValues.Add("meeting");
            listValues.Add(stokvel_id);
            listValues.Add(member_id);
            listValues.Add(type_id);
            listValues.Add(amount);
            listValues.Add(date.ToString());
            listValues.Add(agent_id);

            try
            {
                JsonStringWriter jsw = new JsonStringWriter();
                jsw.OfflineInserts("record", listAttribs, listValues);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.ToString());
            }
            finally
            {

            }
        }

        public void update_variable(string attribute, string value, SqlDbType type)
        {
            //Parameter and StoredProc for ?????? update.
            SqlParameter param = new SqlParameter("@" + attribute, type);
            param.Value = value;
            string storedProc = "record_" + attribute + "_update";
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

            SqlParameter idParam = new SqlParameter("@record_id", SqlDbType.BigInt);
            idParam.Value = id;

            cmd.Parameters.Add(idParam);
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

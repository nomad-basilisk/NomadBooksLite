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
    class Meeting
    {
        public string notesX { get; set; } 
        public DateTime dateX { get; set; }
        public string stokvel_idStr { get; set; }

        public void insert()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("meeting_insert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlParameter stokvel_id = new SqlParameter("@stokvel_id", SqlDbType.BigInt);
            SqlParameter notes = new SqlParameter("@notes", SqlDbType.Text);
            SqlParameter date = new SqlParameter("@date", SqlDbType.DateTime);

            stokvel_id.Value = long.Parse(stokvel_idStr);
            notes.Value = notesX;
            date.Value = dateX;

            cmd.Parameters.Add(stokvel_id);
            cmd.Parameters.Add(notes);
            cmd.Parameters.Add(date);

            try
            {
                con.Open();
                var rowCount = cmd.ExecuteScalar();
                MessageBox.Show("Meeting created");
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
            listAttribs.Add("notes");
            listAttribs.Add("date");

            List<string> listValues = new List<string>();
            listValues.Add("meeting");
            listValues.Add(stokvel_idStr);
            listValues.Add(notesX);
            listValues.Add(dateX.ToString());

            try
            {
                JsonStringWriter jsw = new JsonStringWriter();
                jsw.OfflineInserts("agent", listAttribs, listValues);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error : " + ex.ToString());
            }
            finally
            {

            }

        }
    }
}

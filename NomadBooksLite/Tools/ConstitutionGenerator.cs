using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Novacode;
using System.Diagnostics;
using System.Windows;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Threading;

namespace NomadBooksLite.Tools
{
    class ConstitutionGenerator
    {
        public void CreateSampleDocument(string stokvel_id, string stokvel_name, string purpose, string joining_fee, string contrib)
        {
            string name_final = stokvel_name.Replace(" ", "_");
            string fileName = String.Format(@"{1}Constitutions\{0}_constitution.docx", name_final, System.AppDomain.CurrentDomain.BaseDirectory);
            string headlineText = String.Format("Constitution of {0} Stokvel", stokvel_name);

            string paraOne = "This constitution has been drafted and published by "
                                + "the Nomad Stokvel Constitution Wizard "
                                + "as a set of fundamental principles according to which a "
                                + "Stokvel or any community-based deposit-taking organisation "
                                + "is governed. Together, these principles define the "
                                + "Stokvel group premise. This document is a set of  "
                                + "guidelines that a Stokvel group may use to create  "
                                + "its own constitution.  ";

            string paraTwo = String.Format(" "
                                + "\nThe name of the Stokvel is: "
                                + " "
                                + "\n{0}", stokvel_name);

            string headingObjectives = "\n "
                                + "\n1. Aims and Objectives"
                                + " ";

            string paraObjectives = String.Format(" "
                                    + "\n •	To promote personal and group development. "
                                    + "\n •	To pool funds with a common purpose and outcome. "
                                    + "\nThe aim of this Stokvel is to pool funds for the following purpose: "
                                    + "\n •	{0}", purpose);

            string headingMembers = "\n "
                                + "\n2. Members";
            string paraMembers = " "
                                + "\n •	Members will supply the Stokvel with their personal details (ID number, date of birth and residential address)."
                                + "\n •	Members must abide by the Stokvel constitution. "
                                + "\n •	Should a member pass away, his or her family members will not automatically become members of the club. ";

            string headingExecCommit = "\n "
                                + "\n3. Stokvel Executive Committee";
            string paraExecCommit = "\n The executive committee will consist of the following positions: "

                                    + "\n a.	Chairperson, whose responsibilities are: "

                                    + "\n  •	To lead and prepare the agenda for meetings.  "
                                    + "\n  •	Make sure rules are followed.  "
                                    + "\n  •	Approve money withdrawal with other executive member.  "
                                    + "\n  •	Explore opportunities for enhancing the group’s practices.  "
                                    + "\n  "
                                    + "\n b.	Secretary, whose responsibilities are: "

                                    + "\n  •	Keep an accurate record of the group’s activities, namely minutes, correspondence and membership register.  "
                                    + "\n  •	Maintain communication to make sure all members are informed of all activities of the group.  "
                                    + "\n  •	Have signing powers with the chairperson and treasurer.  "

                                    + "\n c.	Treasurer, whose responsibilities are: "

                                    + "\n  •	Keep accurate account of all the group’s finances and present copies of all the deposit slips.  "
                                    + "\n  •	Collect money or deposit slips at every meeting.  "
                                    + "\n  •	Have signing powers with the chairperson and the secretary.  "

                                    + "\n Change of Leadership "

                                    + "\n  •	Members can change the leadership structure if there is a majority vote.  "
                                    + "\n  •	Changes in the leadership structure must be announced 60 days prior to the meeting.  ";

            string headingJoiningFee = "\n "
                                + "\n4. Joining Fee";
            string paraJoiningFee = " "
                                + String.Format("\n  •	  Each member must pay R{0} as a non-refundable joining fee. ", joining_fee);

            string headingContribs = "\n "
                                + "\n5. Contributions";
            string paraContribs = " "
                                + String.Format("\n  •	  Each member will contribute an amount of R{0} per meeting.  ", contrib);

            string headingDeclaration = "\n "
                                + "\n6. Declaration";
            string paraDeclaration = String.Format("\nBy affixing my name to this document, I hereby accept the constitution of {0}\n", stokvel_name);

            // A formatting object for our headline:
            var headLineFormat = new Formatting();
            headLineFormat.FontFamily = new System.Drawing.FontFamily("Calibri");
            headLineFormat.Size = 18D;
            headLineFormat.Position = 12;

            // A formatting object for our headline 2:
            var headLineFormat2 = new Formatting();
            headLineFormat2.FontFamily = new System.Drawing.FontFamily("Calibri");
            headLineFormat2.Size = 14D;
            headLineFormat2.Position = 6;

            // A formatting object for our headline 3:
            var headLineFormat3 = new Formatting();
            headLineFormat3.FontFamily = new System.Drawing.FontFamily("Calibri");
            headLineFormat3.Size = 12D;
            headLineFormat3.Bold = true;

            // A formatting object for our normal paragraph text:
            var paraFormat = new Formatting();
            paraFormat.FontFamily = new System.Drawing.FontFamily("Calibri");
            paraFormat.Size = 12D;

            // Create the document in memory:
            var doc = DocX.Create(fileName);

            // Insert the now text obejcts;
            doc.InsertParagraph(headlineText, false, headLineFormat);
            doc.InsertParagraph(paraOne, false, paraFormat);
            doc.InsertParagraph(paraTwo, false, headLineFormat2);
            doc.InsertParagraph(headingObjectives, false, headLineFormat3);
            doc.InsertParagraph(paraObjectives, false, paraFormat);
            doc.InsertParagraph(headingMembers, false, headLineFormat3);
            doc.InsertParagraph(paraMembers, false, paraFormat);
            doc.InsertParagraph(headingExecCommit, false, headLineFormat3);
            doc.InsertParagraph(paraExecCommit, false, paraFormat);
            doc.InsertParagraph(headingJoiningFee, false, headLineFormat3);
            doc.InsertParagraph(paraJoiningFee, false, paraFormat);
            doc.InsertParagraph(headingContribs, false, headLineFormat3);
            doc.InsertParagraph(paraContribs, false, paraFormat);
            doc.InsertParagraph(headingDeclaration, false, headLineFormat3);
            doc.InsertParagraph(paraDeclaration, false, paraFormat);


            //GET MEMBERS
            var connectionString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;

            string CmdString = String.Empty;
            DataTable dt = new DataTable("members_otp");
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                CmdString = String.Format("select CONCAT(m.firstname, ' ', m.lastname) As Name, mo.member_id, m.stokvel_id, mo.verified as Verified from member_otp mo join member m on m.id = mo.member_id where m.stokvel_id = {0}", stokvel_id);
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }


            string spaces = "                                                ";

            //Add member declarations:
            foreach (DataRow row in dt.Rows)
            {
                string nameMember = String.Concat(row["Name"], spaces.Substring(0, (48 - row["Name"].ToString().Length)));

                //set verification text
                string verifiedText = String.Empty;
                if (row["Verified"].ToString() == "1")
                {
                    verifiedText = "Verified via OTP.                               ";
                }
                else
                {
                    verifiedText = "..................................              ";
                }

                string dateDeclaration = DateTime.Today.ToShortDateString();

                string paraMemberDec = String.Format("\n{0}{1}{2}\nFull Name and Surname                           Signature                                        ", nameMember, verifiedText, dateDeclaration);

                doc.InsertParagraph(paraMemberDec, false, paraFormat);

            }

            // Save to the output directory:
            try
            {
                doc.Save();

                // Open in Word:
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.FileName = "WINWORD.EXE";
                startInfo.Arguments = "\"" + fileName + "\"";
                System.Diagnostics.Process.Start(startInfo);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error : " + e.Message);
            }
        }
    }
}

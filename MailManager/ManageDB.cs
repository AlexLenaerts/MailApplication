using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace MailManager
{
    class ManageDB
    {
        public static void ConnectDB(SqlConnection con)
        {
            try
            {
                //Ask user, mdp, type de compte
                con.Open();
                Console.WriteLine("Connected to the server");
                con.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("Serveur non disponible");
            }
            finally
            {
                Console.WriteLine("Version 1.0");
            }

        }

        public static void CloseDB(SqlConnection con)
        {
            con.Close();
            Console.WriteLine("Disconnected from the server");
        }

        public static void SearchMail(string param, string value, SqlConnection con)
        {
            con.Open();
            string queryStr = $"SELECT * from TBLMAIL where {param}= '{value}'";
            SqlCommand cmd = new SqlCommand(queryStr, con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Console.WriteLine(String.Format("{0} {1} {2} {3} {4}", dr[1], dr[2], dr[3], dr[4], dr[5]));
                }
            }
            else
            {
                Console.WriteLine("No found");
            }
            dr.Close();
            con.Close();
        }

        public static void AddMailToDB(Mail mail, SqlConnection con)
        {
            con.Open();
            string queryStr = $"INSERT INTO TBLMAIL (Dest, date, subject, content, refe) VALUES ('{mail.From}','{mail.Date}','{mail.Subject.Replace("\\", "'").Replace("'", "''")}','{mail.msg.Replace("'","''")}',{mail.Reference})";
            SqlCommand cmd = new SqlCommand(queryStr, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }


        public static void RemoveMailByRef(int reference, SqlConnection con)
        {
            con.Open();
            string queryStr = $"DELETE FROM TBLMAIL where refe= {reference}";
            SqlCommand cmd = new SqlCommand(queryStr, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void RemoveMail( SqlConnection con)
        {
            con.Open();
            string queryStr = $"DELETE FROM TBLMAIL";
            SqlCommand cmd = new SqlCommand(queryStr, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public static void ModifyDrawMail(Mail mail, SqlConnection con)
        {
            con.Open();
            string queryStr = $"Update TBLMAIL set from = '{mail.From}', date = '{mail.Date}', subject= '{mail.Subject}', content = '{mail.msg}' where refe= '{mail.Reference}'";
            SqlCommand cmd = new SqlCommand(queryStr, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }
        public static List<Mail> DBTOLIST(SqlConnection con)
        {
            con.Open();
            string queryStr = "SELECT * from TBLMAIL";
            SqlCommand cmd = new SqlCommand(queryStr, con);
            SqlDataReader dr = cmd.ExecuteReader();
            List<Mail> mails = new List<Mail>();
            while (dr.Read())
            {
                mails.Add(new Mail { From = dr[1].ToString(), Subject = dr[3].ToString(), Date = dr[4].ToString(), msg = dr[2].ToString(), Reference = Convert.ToInt32(dr[5]) });
            }

            dr.Close();
            con.Close();
            return mails;
        }
    }
}

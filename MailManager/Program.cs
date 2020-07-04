using System;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace MailManager
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoadApp());
            /*SqlConnection con = new SqlConnection(@"Data Source = C:\USERS\ALEXANDRE\SOURCE\REPOS\MAILAPPLICATION\MAILMANAGER\DB\DATABASE1.MDF; Integrated Security = True");

            try { 
                ManageDB.ConnectDB(con); 
                }
            catch
            { 
                Console.WriteLine("error");
            }
            */


        }

    }
}

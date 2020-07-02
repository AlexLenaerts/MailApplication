using System;
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
            LoadApp loadpage = new LoadApp();
            Application.Run(new LoadApp());

        }

    }
}

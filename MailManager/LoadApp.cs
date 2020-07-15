using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MailManager
{
    public partial class LoadApp : Form
    {
        public static List<Mail> mail;
        public bool successLoad { get; private set; }
        public bool firstkeer { get; set; }

        public LoadApp()
        {

            InitializeComponent();
            button1.Hide();
            //load mail from server and add new mail to DB
            load(60);
        }

        public async void load(int numberMail)
        {
            IProgress<int> progress = new Progress<int>(value => { progressBar1.Value = value; });
            progressBar1.Visible = true;
            progressBar1.Minimum = 0;
            progressBar1.Step = 1;
            Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var client = new Pop3Client();
            client.Connect("pop.gmail.com", 995, true);
            client.Authenticate("recent:alexandrelenaerts@gmail.com", "vnfkfkxlcgpnebra");
            List<string> uids = client.GetMessageUids();
            List<OpenPop.Mime.Message> AllMsgReceived = new List<OpenPop.Mime.Message>();
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Alexandre\Source\Repos\MailApplication\MailManager\DB\database1.mdf;Integrated Security=True");
            ManageDB.RemoveMail(con);

            List<string> seenUids = new List<string>();
            int messageCount = client.GetMessageCount();
            progressBar1.Maximum = numberMail;
            await Task.Run(() =>
            {
                    firstkeer = false;
                    for (int i = messageCount; i >= (messageCount - numberMail); i--)
                    {
                        progress.Report(messageCount - i);
                        OpenPop.Mime.Message unseenMessage = client.GetMessage(i);
                        AllMsgReceived.Add(unseenMessage);
                }

                var mails = new List<Mail>();
                    MessagePart plainTextPart = null, HTMLTextPart = null;
                    string pattern = @"[A-Za-z0-9]*[@]{1}[A-Za-z0-9]*[.\]{1}[A-Za-z]*";

                int a = 0;
                foreach (var msg in AllMsgReceived)
                {
                        //Check you message is not null
                        if (msg != null)
                        {
                            plainTextPart = msg.FindFirstPlainTextVersion();
                        //HTMLTextPart = msg.FindFirstHtmlVersion();
                        //mail.Html = (HTMLTextPart == null ? "" : HTMLTextPart.GetBodyAsText().Trim());

                        //ajouter au serveur 
                        //mails.Add(new Mail { From = Regex.Match(msg.Headers.From.ToString(), pattern).Value, Subject = msg.Headers.Subject, Date = msg.Headers.DateSent.ToString(), msg = (plainTextPart == null ? "" : plainTextPart.GetBodyAsText().Trim()), Attachment = msg.FindAllAttachments() });

                        ManageDB.AddMailToDB(
                            new Mail { From = Regex.Match(msg.Headers.From.ToString(), pattern).Value, 
                                Subject = msg.Headers.Subject, Date = msg.Headers.DateSent.ToString(), 
                                msg = (plainTextPart == null ? "" : plainTextPart.GetBodyAsText().Trim()), 
                                Attachment = msg.FindAllAttachments(),Reference= a+=1 },con);                 
                    }
                }
                ManageDB.AddNewContact(con);
                //LoadApp.mail = mails;
                successLoad = true;                
            });
            label1.Text = "";

            button1.Show();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Visible = false;
            Form1 f1 = new Form1();
            f1.Show();
        }
    }
}
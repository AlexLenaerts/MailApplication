using OpenPop.Mime;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace MailManager
{
    public partial class Form1 : Form
    {
        public static string SenTo { get; internal set; }
        public static string Msg { get; internal set; }
        public static string Subject { get; internal set; }
        SqlConnection con = new SqlConnection(@"Data Source = C:\USERS\ALEXANDRE\SOURCE\REPOS\MAILAPPLICATION\MAILMANAGER\DB\DATABASE1.MDF; Integrated Security = True");

        public Form1()
        {
            InitializeComponent();
            CreateMyMultilineTextBox();
            DisplayData(ManageDB.DBTOLIST(con), false);
            //DisplayData(LoadApp.mail,false);
        }
        public void CreateMyMultilineTextBox()
        {
            // Create an instance of a TextBox control.
            // Set the Multiline property to true.
            textBox3.Multiline = true;
            // Add vertical scroll bars to the TextBox control.
            textBox3.ScrollBars = ScrollBars.Vertical;
            // Allow the TAB key to be entered in the TextBox control.
            textBox3.AcceptsReturn = true;
            // Allow the TAB key to be entered in the TextBox control.
            textBox3.AcceptsTab = true;
            // Set WordWrap to true to allow text to wrap to the next line.
            textBox3.WordWrap = true;
        }
        private void DisplayData(List<Mail> receivedMail, bool isOwn)
        {
            listView1.Scrollable = true;
            listView1.View = View.Details;
            listView1.AllowColumnReorder = false;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.Clear();
            listView1.Columns.Add("From", 20);
            listView1.Columns.Add("Subject", 10);
            listView1.Columns.Add("Date", 10);
            listView1.Columns.Add("Content", 0);
            foreach (var element in receivedMail)
            {
                if (element.From != "alexandrelenaerts@gmail.com" && !isOwn)
                {
                    ListViewItem row = new ListViewItem(element.From);
                    row.SubItems.Add(new ListViewItem.ListViewSubItem(row, element.Subject));
                    row.SubItems.Add(new ListViewItem.ListViewSubItem(row, element.Date));
                    row.SubItems.Add(new ListViewItem.ListViewSubItem(row, element.msg));
                    listView1.Items.Add(row);
                }
                if ((element.From == "alexandrelenaerts@gmail.com") && isOwn)
                {
                    ListViewItem row = new ListViewItem(element.From);
                    row.SubItems.Add(new ListViewItem.ListViewSubItem(row, element.Subject));
                    row.SubItems.Add(new ListViewItem.ListViewSubItem(row, element.Date));
                    row.SubItems.Add(new ListViewItem.ListViewSubItem(row, element.msg));
                    listView1.Items.Add(row);
                }
            }
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            listView1.Columns[3].Width = 0;
            listView1.Columns[1].Width = 890 - listView1.Columns[0].Width - listView1.Columns[2].Width - SystemInformation.VerticalScrollBarWidth - 10;
            ;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*Refresh (count message and compare with database*/
            /*Save new mail to DB*/

                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);                
                var mails = new List<Mail>();
                MessagePart plainTextPart = null, HTMLTextPart = null;

                string pattern = @"[A-Za-z0-9]*[@]{1}[A-Za-z0-9]*[.\]{1}[A-Za-z]*";
                foreach (var msg in Manage.Receive())
                {
                    //Check you message is not null
                    if (msg != null)
                    {
                        plainTextPart = msg.FindFirstPlainTextVersion();
                        //HTMLTextPart = msg.FindFirstHtmlVersion();
                        //mail.Html = (HTMLTextPart == null ? "" : HTMLTextPart.GetBodyAsText().Trim());
                        mails.Add(new Mail { From = Regex.Match(msg.Headers.From.ToString(), pattern).Value, Subject = msg.Headers.Subject, Date = msg.Headers.DateSent.ToString(), msg = (plainTextPart == null ? "" : plainTextPart.GetBodyAsText().Trim()) });
                        //if mails ! DB ajouter 
                }
                }
            DisplayData(ManageDB.DBTOLIST(con), false);

        }
        private void button2_Click(object sender, EventArgs e)
        {
            Form1.SenTo = "Destinataires";
            Form1.Subject = "Object";
            Form2 f2 = new Form2();
            f2.Show();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //afficher messsage
            ListView.SelectedListViewItemCollection mails = listView1.SelectedItems;
            foreach (ListViewItem item in mails)
            {
                var SenTo = item.Text;
                textBox1.Text = SenTo;
                var client = Manage.Connect(new MailAddress("alexandrelenaerts@gmail.com", "From Name"));
                MailAddress tomail = new MailAddress(SenTo, "To Name");
                textBox3.Text = item.SubItems[3].Text;
                textBox2.Text = item.SubItems[1].Text;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //répondre
            ListView.SelectedListViewItemCollection mails = listView1.SelectedItems;

            foreach (ListViewItem item in mails)
            {
                 Form1.SenTo = item.Text;
                 Form1.Subject = "Re :" + item.SubItems[1].Text;
                 Form2 f2 = new Form2();
                 f2.Show();
            }                    
        }
        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        //Lire les msg envoyés 
        private void button4_Click(object sender, EventArgs e)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var mails = new List<Mail>();
            MessagePart plainTextPart = null, HTMLTextPart = null;

            string pattern = @"[A-Za-z0-9]*[@]{1}[A-Za-z0-9]*[.\]{1}[A-Za-z]*";
            foreach (var msg in Manage.Receive())
            {
                //Check you message is not null
                if (msg != null)
                {
                    plainTextPart = msg.FindFirstPlainTextVersion();
                    //HTMLTextPart = msg.FindFirstHtmlVersion();
                    //mail.Html = (HTMLTextPart == null ? "" : HTMLTextPart.GetBodyAsText().Trim());
                    mails.Add(new Mail { From = Regex.Match(msg.Headers.From.ToString(), pattern).Value, Subject = msg.Headers.Subject, Date = msg.Headers.DateSent.ToString(), msg = (plainTextPart == null ? "" : plainTextPart.GetBodyAsText().Trim()) });
                }
            }
            DisplayData(ManageDB.DBTOLIST(con), true);
            //DisplayData(LoadApp.mail, true);
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public void downloadFile(Mail receivedMail)
        {
            var mails = receivedMail.Attachment;
            try
            {
                if (mails[0] != null)
                {
                    byte[] content = mails[0].Body;
                    string[] stringParts = mails[0].FileName.Split(new char[] { '.' });
                    string strType = stringParts[1];
                    File.WriteAllBytes(@"C:\Users\Alexandre\Downloads\" + stringParts[0] + "." + strType, content);
                }
            }
            catch (Exception ex)
            {

            }
        }

        //download attachment 
        private void button7_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection mails = listView1.SelectedItems;
            var sento = mails[0].Text;

            var down = LoadApp.mail.Where(x => x.From == sento).ToList();

            downloadFile(down.First());
        }
    }

}
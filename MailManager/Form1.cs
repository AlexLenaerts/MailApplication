using OpenPop.Mime;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        public static string Subjec { get; internal set; }

        public Form1()
        {
            InitializeComponent();
            CreateMyMultilineTextBox();
            listView1.Columns.Add("From", 20);
            listView1.Columns.Add("Subject", 10);
            listView1.Columns.Add("Date", 10);
            listView1.Columns.Add("Content", 0);
            DisplayData(LoadApp.mail);
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
        private void DisplayData(List<Mail> receivedMail)
        {
            listView1.Scrollable = true;
            listView1.View = View.Details;
            listView1.AllowColumnReorder = false;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;

            foreach (var element in receivedMail)
            {
                if(element.From != "alexandrelenaerts@gmail.com")
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
                    }
                }
            DisplayData(mails);

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


    }

}
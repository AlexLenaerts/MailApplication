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
        private readonly string contextMenu;

        public static string SenTo { get; internal set; }
        public static string Msg { get; internal set; }
        public static string Subject { get; internal set; }
        public static string Subjec { get; internal set; }

        private bool isFirstClick = true;
        private bool isDoubleClick = false;
        private bool isRight = false;
        private int milliseconds = 0;
        private Timer doubleClickTimer = new Timer();

        public Form1()
        {
            InitializeComponent();
            doubleClickTimer.Interval = 100;
            doubleClickTimer.Tick +=
                new EventHandler(doubleClickTimer_Tick);
            listView1.Columns.Add("From", 20);
            listView1.Columns.Add("Subject", 10);
            listView1.Columns.Add("Date", 10);
            listView1.Columns.Add("Content", 0);
            DisplayData(LoadApp.mail);
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
                ListViewItem row = new ListViewItem(element.From);
                row.SubItems.Add(new ListViewItem.ListViewSubItem(row, element.Subject));
                row.SubItems.Add(new ListViewItem.ListViewSubItem(row, element.Date));
                row.SubItems.Add(new ListViewItem.ListViewSubItem(row, element.msg));
                listView1.Items.Add(row);
            }
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            listView1.Columns[3].Width = 0;
            listView1.Columns[1].Width = 1691 - listView1.Columns[0].Width - listView1.Columns[2].Width - SystemInformation.VerticalScrollBarWidth - 10;
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

        void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            // This is the first mouse click.
            if (isFirstClick)
            {
                isFirstClick = false;
                Invalidate();
                // Start the double click timer.
                doubleClickTimer.Start();
                if (e.Button.ToString() == "Right")
                {
                    isRight = true;
                }
            }
            // This is the second mouse click.
            else
            {
                if (milliseconds < SystemInformation.DoubleClickTime)
                {
                    isDoubleClick = true;
                }
            }
        }


        void doubleClickTimer_Tick(object sender, EventArgs e)
        {
            milliseconds += 100;

            // The timer has reached the double click time limit.
            if (milliseconds >= SystemInformation.DoubleClickTime)
            {
                doubleClickTimer.Stop();

                if (isDoubleClick)
                {
                    //répondre

                    ListView.SelectedListViewItemCollection mails =  listView1.SelectedItems;
                    foreach (ListViewItem item in mails)
                    {
                        var SenTo = item.Text;
                        Form1.SenTo = SenTo;
                        var client = Manage.Connect(new MailAddress("alexandrelenaerts@gmail.com", "From Name"));
                        MailAddress tomail = new MailAddress(SenTo, "To Name");
                        Form1.Msg = item.SubItems[3].Text;
                        Form1.Subject = item.SubItems[1].Text;
                        Form3 f3 = new Form3();
                        f3.Show();
                     }
                }
                if(!isDoubleClick && isRight)
                {
                    //afficher messsage
                    ListView.SelectedListViewItemCollection mails = listView1.SelectedItems;


                    foreach (ListViewItem item in mails)
                    {
                        var SenTo = item.Text;
                        Form1.SenTo = SenTo;
                        Form1.Subject = "Re :" + item.SubItems[1].Text;
                        Form2 f2 = new Form2();
                        f2.Show();
                    }
                }
                isFirstClick = true;
                isDoubleClick = false;
                isRight = false;
                milliseconds = 0;
            }
        }
    }

}
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
            dataGridView1.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;

            dataGridView1.AutoSize = true;
            doubleClickTimer.Interval = 100;
            doubleClickTimer.Tick +=
                new EventHandler(doubleClickTimer_Tick);
            dataGridView1.DataSource = LoadApp.mail;
            dataGridView1.Columns["msg"].Visible = false;

            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridView1.BorderStyle = BorderStyle.None;

            /*Extract mail from DB and write them in a dataGridView */

        }

        private void GetData()
        {
            throw new NotImplementedException();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*Refresh (count message and compare with database*/
            /*Save new mail to DB*/

                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                var AllMsgReceived = Manage.Receive();
                var maxLength1 = AllMsgReceived.Max(ot => (ot.Headers.From).ToString().Length);
                var maxLength2 = AllMsgReceived.Max(ot => ot.Headers.Subject.Length);
                var maxLength3 = AllMsgReceived.Max(ot => ot.Headers.Date.Length);
                var mails = new List<Mail>();
                MessagePart plainTextPart = null, HTMLTextPart = null;

                string pattern = @"[A-Za-z0-9]*[@]{1}[A-Za-z0-9]*[.\]{1}[A-Za-z]*";
                foreach (var msg in AllMsgReceived)
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
                dataGridView1.DataSource = mails;
                dataGridView1.Columns["msg"].Visible = false;
                dataGridView1.AutoResizeColumns();
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dataGridView1.ScrollBars = ScrollBars.Both;

            
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
                    Int32 selectedRowCount = dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);
                    if (selectedRowCount > 0)
                    {
                        var SenTo = dataGridView1.SelectedRows[0].Cells[0].EditedFormattedValue.ToString();
                        Form1.SenTo = SenTo;
                        var client = Manage.Connect(new MailAddress("alexandrelenaerts@gmail.com", "From Name"));
                        MailAddress tomail = new MailAddress(SenTo, "To Name");
                        Form1.Msg = dataGridView1.SelectedRows[0].Cells[3].EditedFormattedValue.ToString();
                        Form1.Subject = dataGridView1.SelectedRows[0].Cells[1].EditedFormattedValue.ToString();
                        Form3 f3 = new Form3();
                        f3.Show();
                    }

                }
                if(!isDoubleClick && isRight)
                {

                    Int32 selectedRowCount = dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);
                    if (selectedRowCount > 0)
                    {
                        var SenTo = dataGridView1.SelectedRows[0].Cells[0].EditedFormattedValue.ToString();
                        Form1.SenTo = SenTo;
                        Form1.Subject = "Re :" + dataGridView1.SelectedRows[0].Cells[1].EditedFormattedValue.ToString();
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
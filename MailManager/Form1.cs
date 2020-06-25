using MailKit.Net.Pop3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace MailManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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

            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                var AllMsgReceived = Manage.Receive();
                var maxLength1 = AllMsgReceived.Max(ot => (ot.Headers.From).ToString().Length);
                var maxLength2 = AllMsgReceived.Max(ot => ot.Headers.Subject.Length);
                var maxLength3 = AllMsgReceived.Max(ot => ot.Headers.Date.Length);
                var mails = new List<Mail>();

                string pattern = @"[A-Za-z0-9]*[@]{1}[A-Za-z0-9]*[.\]{1}[A-Za-z]*";
                foreach (var msg in AllMsgReceived)
                {
                    mails.Add(new Mail { From = Regex.Match(msg.Headers.From.ToString(), pattern), Subject = msg.Headers.Subject, Date = msg.Headers.DateSent.ToString() });
                }
                dataGridView1.DataSource = mails;
                dataGridView1.AutoResizeColumns();
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            }
            catch (SmtpException ex)
            {
                //Console.WriteLine(ex.ToString());
                //Console.ReadKey();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Int32 selectedRowCount = dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount > 0)
            {
                var SenTo = dataGridView1.SelectedRows[0].Cells[0].EditedFormattedValue.ToString();
                var client = Manage.Connect(new MailAddress("alexandrelenaerts@gmail.com", "From Name"));
                MailAddress tomail = new MailAddress(SenTo, "To Name");

                /*Window open to write a message before send it */
                //var message = Manage.Message(tomail, "helloooooooo", "test1");

                /*Confirm button message */
                // client.Send(message);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Windows.Forms;
namespace MailManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
            try
            {
                var client = Manage.Connect(new MailAddress("alexandrelenaerts@gmail.com", "From Name"));

                MailAddress tomail = new MailAddress("alexandrelenaerts@gmail.com", "To Name");

                var message = Manage.Message(tomail, "helloooooooo", "test1");

                // client.Send(message);
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);


                var AllMsgReceived = Manage.Receive();
                var maxLength1 = AllMsgReceived.Max(ot => (ot.Headers.From).ToString().Length);
                var maxLength2 = AllMsgReceived.Max(ot => ot.Headers.Subject.Length);
                var maxLength3 = AllMsgReceived.Max(ot => ot.Headers.Date.Length);
                var mails = new List<Mail>();

                foreach (var msg in AllMsgReceived)
                {
                    mails.Add(new Mail { From = msg.Headers.From.ToString(), Subject = msg.Headers.Subject, Date = msg.Headers.DateSent.ToString() });
                }
                dataGridView1.DataSource = mails;
                dataGridView1.AutoResizeColumns();
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            }
            catch (SmtpException ex)
            {
               // Console.WriteLine(ex.ToString());
                //Console.ReadKey();
            }
        }
    }
}

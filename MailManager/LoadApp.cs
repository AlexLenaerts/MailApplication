using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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

        public LoadApp()
        {
            InitializeComponent();

            load(60);
        }

        private async void load(object sender, EventArgs e)
        {
            IProgress<int> progress = new Progress<int>(value => { progressBar1.Value = value; });
            await Task.Run(() =>
            {
                for (int i = 0; i <= 100; i++)
                    progress.Report(i);
            });
        }

        public async void load(int numberMail)
        {
            
            IProgress<int> progress = new Progress<int>(value => { progressBar1.Value = value; });
            progressBar1.Visible = true;
            progressBar1.Minimum = 0;
            progressBar1.Step = 1;

           
                
            // Set Maximum to the total number of files to copy.
            Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var client = new Pop3Client();
            client.Connect("pop.gmail.com", 995, true);
            client.Authenticate("recent:alexandrelenaerts@gmail.com", "vnfkfkxlcgpnebra");
            List<string> uids = client.GetMessageUids();
            List<OpenPop.Mime.Message> AllMsgReceived = new List<OpenPop.Mime.Message>();
            List<string> seenUids = new List<string>();
            int messageCount = client.GetMessageCount();
            progressBar1.Maximum = numberMail;

            await Task.Run(() =>
            {
                for (int i = messageCount; i >= (messageCount - numberMail); i--)
            {
                //progressBar1.PerformStep();
                progress.Report(messageCount-i);
                OpenPop.Mime.Message unseenMessage = client.GetMessage(i);
                AllMsgReceived.Add(unseenMessage);
            }
                
         

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
            LoadApp.mail = mails;
            successLoad = true;
            });
        }
    }
}
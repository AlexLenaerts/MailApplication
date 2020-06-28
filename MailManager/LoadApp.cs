using OpenPop.Mime;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
            progressBar1.Minimum = 0;
            // Set Maximum to the total number of files to copy.
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var AllMsgReceived = Manage.Receive();
            progressBar1.Maximum = AllMsgReceived.Count();

            var maxLength1 = AllMsgReceived.Max(ot => (ot.Headers.From).ToString().Length);
            var maxLength2 = AllMsgReceived.Max(ot => ot.Headers.Subject.Length);
            var maxLength3 = AllMsgReceived.Max(ot => ot.Headers.Date.Length);
            var mails = new List<Mail>();
            MessagePart plainTextPart = null, HTMLTextPart = null;

            string pattern = @"[A-Za-z0-9]*[@]{1}[A-Za-z0-9]*[.\]{1}[A-Za-z]*";
            foreach (var msg in AllMsgReceived)
            {
                progressBar1.Value++;
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
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using OpenPop.Mime;
using OpenPop.Pop3;

namespace MailManager
{
    public class Mail
    {
        public string From { get; set; }
        public string Subject { get; set; }
        public string Date { get; set; }
        public string msg { get; set; }

        public string FileName;

        public List<MessagePart> Attachment;

        public int Reference { get; set; }




    }
}

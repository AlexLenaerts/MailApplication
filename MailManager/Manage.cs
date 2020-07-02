using OpenPop.Mime;
using OpenPop.Pop3;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Windows.Forms;

namespace MailManager
{
    public class Manage
    {

        public static SmtpClient Connect(MailAddress fromAddress)
        {
            SmtpClient client = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, "vnfkfkxlcgpnebra"),
                Timeout = 20000
            };
            return client;
        }
        public static MailMessage Message(MailAddress tomail, string messageBody, string messageSubject, bool att, string file)
        {
            var fromAddress = new MailAddress("alexandrelenaerts@gmail.com", "Alexandre Lenaerts");
            MailMessage message = new MailMessage(fromAddress, tomail);
            message.Subject = messageSubject;
            message.Body = messageBody;
            if (att)
            {
                Attachment data = new Attachment(file, MediaTypeNames.Application.Octet);
                message.Attachments.Add(data);
            }
            return message;
        }
        public static List<OpenPop.Mime.Message> Receive()
        {
            var client = new Pop3Client();
            client.Connect("pop.gmail.com", 995, true);
            client.Authenticate("recent:alexandrelenaerts@gmail.com", "vnfkfkxlcgpnebra");
            List<string> uids = client.GetMessageUids();
            List<OpenPop.Mime.Message> newMessages = new List<OpenPop.Mime.Message>();
            List<string> seenUids = new List<string>();
            int messageCount = client.GetMessageCount();

            for (int i = messageCount; i > (messageCount-50) ; i--)
            {
                OpenPop.Mime.Message unseenMessage = client.GetMessage(i);

                newMessages.Add(unseenMessage);
            }
            return newMessages;
        }
        
        public static void SendMessage(string to, string sub, string msg, string file, bool isAtt)
        {
            var fromAddress = new MailAddress("alexandrelenaerts@gmail.com", "Alexandre Lenaerts");
            var client = Manage.Connect(fromAddress);
            var senTo = new MailAddress(to);
            var subject = sub;
            var message = msg;
            client.Send(Manage.Message(senTo, message, subject, isAtt, file));
        }

    }
}

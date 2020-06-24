using OpenPop.Mime;
using OpenPop.Pop3;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

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
        public static MailMessage Message(MailAddress tomail, string messageBody, string messageSubject)
        {
            var fromAddress = new MailAddress("alexandrelenaerts@gmail.com", "From Name");
            MailMessage message = new MailMessage(fromAddress, tomail);
            message.Subject = messageSubject;
            message.Body = messageBody;
            return message;
        }
        public static List<Message> Receive()
        {
            var client = new Pop3Client();
            client.Connect("pop.gmail.com", 995, true);
            client.Authenticate("recent:alexandrelenaerts@gmail.com", "vnfkfkxlcgpnebra");
            List<string> uids = client.GetMessageUids();
            List<Message> newMessages = new List<Message>();
            List<string> seenUids = new List<string>();

           
            int messageCount = client.GetMessageCount();

            for (int i = messageCount; i > (messageCount-30) ; i--)
            {
                Message unseenMessage = client.GetMessage(i);
                newMessages.Add(unseenMessage);
            }
            return newMessages;
        }


    }
}

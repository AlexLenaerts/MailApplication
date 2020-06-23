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

            for (int i = messageCount; i > (messageCount-50) ; i--)
            {

                Message unseenMessage = client.GetMessage(i);

                // Add the message to the new messages
                newMessages.Add(unseenMessage);

            }
            /*
           // All the new messages not seen by the POP3 client
           for (int i = 0; i < client.GetMessageCount(); i++)
           {
               string currentUidOnServer = uids[i];
               if (!seenUids.Contains(currentUidOnServer))
               {
                   // We have not seen this message before.
                   // Download it and add this new uid to seen uids

                   // the uids list is in messageNumber order - meaning that the first
                   // uid in the list has messageNumber of 1, and the second has 
                   // messageNumber 2. Therefore we can fetch the message using
                   // i + 1 since messageNumber should be in range [1, messageCount]
                   Message unseenMessage = client.GetMessage(i + 1);

                   // Add the message to the new messages
                   newMessages.Add(unseenMessage);

                   // Add the uid to the seen uids, as it has now been seen
                   seenUids.Add(currentUidOnServer);
               }

           }
           */
            return newMessages;
        }
    }
}

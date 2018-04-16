using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Net.Mail;
using System.Net;

namespace FunctionApp3
{
    public static class QueueTrigger
    {
        [FunctionName("QueueTrigger")]
        public static void Run([QueueTrigger("myqueue-items", Connection = "azurestoragequeu")]string myQueueItem, TraceWriter log)
        {
            person p = new person();
            string[] ssize = myQueueItem.Split(' ');
            p.name = ssize[0];
            p.address = ssize[1];
            p.number = ssize[2];
            string toAddress = "vinay.prithiani@gmail.com";
            string subject = "New Merchant is Registered";
            string body = "Hi,\n\n\nThe Details of New Merchant is \n\n\n" +
                "Contact Person - "+ p.name+" \n\n\n" +
                "Business Address  - "+p.address+"\n\n"+
                "Contact Number - "+p.number+"\n\nThank you";
            string senderID = "chaitraparappa31@gmail.com";
            const string senderPassword = "8095595545c";
            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                    Timeout = 30000,

                };
                MailMessage message = new MailMessage(senderID, toAddress, subject, body);
                smtp.Send(message);

            }
            catch (Exception e)
            {

            }
        }

    }
    public class person
    {
        public string name
        {
            get; set;
        }
        public string number { get; set; }
        public string address { get; set; }

    }
}

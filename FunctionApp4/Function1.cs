using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System.Net.Mail;

namespace FunctionApp4
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static void Run([QueueTrigger("sendingemail", Connection = "sendmail")]string myQueueItem, TraceWriter log)
        {
           
            string toAddress = myQueueItem;
            string subject = "Details of your product";
            string body = "Hi,\n\n\nSorry, For Inconvience the product you ordered is out of stock. \n\n\n" +
                 "\n\nThank you";
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
}

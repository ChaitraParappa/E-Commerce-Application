using Microsoft.Azure;
using Microsoft.Azure.ServiceBus;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ProductTaskDay3.Models
{
    public class StorageInQueue
    {

        public void SendingToQueue(string Name,string Address,string Phno)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("Storageconnectionstring"));
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                CloudQueue queue = queueClient.GetQueueReference("myqueue-items");
                CloudQueueMessage message = new CloudQueueMessage(Name+" "+ Address+" "+ Phno);
                queue.AddMessage(message);
            }
            catch(Exception e)
            {

            }
        }

        public void SendingToQueue1(string email)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("Storageconnectionstring1"));
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                CloudQueue queue = queueClient.GetQueueReference("sendingemail");
                CloudQueueMessage message = new CloudQueueMessage(email);
                queue.AddMessage(message);
            }
            catch (Exception e)
            {

            }
        }


        public async Task SendingToServiceBusQueue(string Name, string Address, string Phno)
        {
            const string ServiceBusConnectionString = "Endpoint=sb://chaiservicebusqueue.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=6AjMZPK8ucFdJ38KxHTv1mpA9Qo0C83LKZm8M0A8F40=";
            const string QueueName = "servicestoragequeue";
            IQueueClient queueClient;
            var message = new Message(Encoding.UTF8.GetBytes(Name + " " + Address + " " + Phno));

            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            await queueClient.SendAsync(message);

        }

        public void SendingImageToQueue(string id,string Userid, HttpPostedFileBase ImageData,string orginalUrl)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("Storageconnectionstring"));
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("imagequeue");
            CloudQueueMessage message = new CloudQueueMessage(id + " " + Userid + " " + ImageData+" "+ orginalUrl);
            queue.AddMessage(message);
        }
       

    }
}
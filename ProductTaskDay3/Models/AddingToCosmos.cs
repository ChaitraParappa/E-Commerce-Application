using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ProductTaskDay3.Models
{
    public class AddingToCosmos
    {
        private const string EndpointUrl = "https://shoppingcosmos.documents.azure.com:443/";
        private const string PrimaryKey = "4qaVmRH3vzhoy2MEk4gee5HitvZIJfSu4HhTNl8BHFJ8cZIQflv0RU7xyWPcIQHjRIuArZJxzgRIKdT7b7tcgA==";
        private DocumentClient client;

        public void startingMethod()
        {
            try
            {
               
                GetStartedDemo().Wait();
            }
            catch (DocumentClientException de)
            {
                Exception baseException = de.GetBaseException();
                Console.WriteLine("{0} error occurred: {1}, Message: {2}", de.StatusCode, de.Message, baseException.Message);
            }
            catch (Exception e)
            {
                Exception baseException = e.GetBaseException();
                Console.WriteLine("Error: {0}, Message: {1}", e.Message, baseException.Message);
            }
            finally
            {
                Console.WriteLine("End of demo, press any key to exit.");
                Console.ReadKey();
            }

        }


        private async Task GetStartedDemo()
        {
            this.client = new DocumentClient(new Uri(EndpointUrl), PrimaryKey);
        }
    }
}
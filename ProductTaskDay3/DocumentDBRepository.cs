using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Newtonsoft.Json;
using ProductTaskDay3.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace ProductTaskDay3
{
    public static class DocumentDBRepository<T> where T : class
    {
        private static readonly string DatabaseId = ConfigurationManager.AppSettings["database"];
        private static readonly string CollectionId = ConfigurationManager.AppSettings["collection"];
        private static readonly string DatabaseId1 = ConfigurationManager.AppSettings["database1"];
        private static readonly string CollectionId1 = ConfigurationManager.AppSettings["collection1"];
        private static readonly string DatabaseId2 = ConfigurationManager.AppSettings["database2"];
        private static readonly string CollectionId2 = ConfigurationManager.AppSettings["collection2"];
        private static DocumentClient client;

        public static void Initialize()
        {
            client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["endpoint"]), ConfigurationManager.AppSettings["authKey"]);
            CreateDatabaseIfNotExistsAsync(DatabaseId).Wait();
            CreateCollectionIfNotExistsAsync(DatabaseId, CollectionId).Wait();
            CreateDatabaseIfNotExistsAsync(DatabaseId1).Wait();
            CreateCollectionIfNotExistsAsync(DatabaseId1, CollectionId1).Wait();
            CreateDatabaseIfNotExistsAsync(DatabaseId2).Wait();
            CreateCollectionIfNotExistsAsync(DatabaseId2, CollectionId2).Wait();
        }

        private static async Task CreateDatabaseIfNotExistsAsync(string DatabaseId)

        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task CreateCollectionIfNotExistsAsync(string DatabaseId,string CollectionId)
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {

                    DocumentCollection myCollection = new DocumentCollection();
                    myCollection.Id = CollectionId;
                   

                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        myCollection,
                        new RequestOptions { OfferThroughput = 1000 });


                    
                }
                else
                {
                    throw;
                }
            }
        }
        public static async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {

            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
            UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
             new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
              .Where(predicate)
               .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public static async Task<IEnumerable<T>> GetItemsAsyncFromCart(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId1, CollectionId1),
                new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }
        public static async Task<IEnumerable<T>> GetItemsImageAsync(Expression<Func<T, bool>> predicate)
        {
            try
            {
                DocumentClient client = new DocumentClient(new Uri(ConfigurationManager.AppSettings["endpoint"]), ConfigurationManager.AppSettings["authKey"]);
                IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                    UriFactory.CreateDocumentCollectionUri(DatabaseId2, CollectionId2),
                    new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true })
                    .Where(predicate)
                    .AsDocumentQuery();

                List<T> results = new List<T>();
                while (query.HasMoreResults)
                {
                    results.AddRange(await query.ExecuteNextAsync<T>());
                }

                return results;
            }catch(Exception e)
            {
                return null;
            }
        }

        public static async Task<Document> CreateItemAsync(T item)
        {
            try
            {
                return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);

            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static async Task<Document> CreateItemImages(T item)
        {
            try
            {
                DocumentClient client= new DocumentClient(new Uri(ConfigurationManager.AppSettings["endpoint"]), ConfigurationManager.AppSettings["authKey"]);
                return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId2, CollectionId2), item);

            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static async Task<Document> CreateItemAsyncInCart(T item)
        {
            return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId1, CollectionId1), item);
        }


        public static async Task<Product> UpdateItemAsync(string id, T item)
        {
            try
            {
                var document = await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), item);
                var data = document.Resource.ToString();
                var person = JsonConvert.DeserializeObject<Product>(data);
                return person;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public static async Task<T> GetItemAsync(string id)
        {
            try
            {
                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public static async Task DeleteDocumentFromCollectionAsync1(string id)
        {
            try
            {
                await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId1, CollectionId1, id));
            }catch(Exception e)
            {

            }
        }

        public static async Task DeleteDocumentFromCollectionAsync(string id)
        {
            try
            {
                await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
            }
            catch (Exception e)
            {

            }
        }

    }
}
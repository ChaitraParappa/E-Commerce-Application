using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Newtonsoft.Json;
using ProductTaskDay3.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProductTaskDay3.Controllers
{
    public class SearchController : Controller
    {
        static SearchServiceClient searchClient;
        [HttpGet]
        public ActionResult SearchingProduct(string search)
        {
            try
            {
                string servicename = "chaiazuresearch";
                string apikey = "40BC449132454F14F046A151E9C22533";
                string index = "documentdb-index";
                searchClient = new SearchServiceClient(servicename, new SearchCredentials(apikey));
                var indexclient = searchClient.Indexes.GetClient(index);

                SearchParameters parameters;
                DocumentSearchResult<Product> results;

                parameters =
                    new SearchParameters()
                    {
                        Select = new[] { "id", "p_id", "name", "description", "price", "image", "quantity", "select", "isdeleted" }
                    };

                results = indexclient.Documents.Search<Product>(search, parameters);
                List<Product> listofresult = new List<Product>();


                foreach (SearchResult<Product> r in results.Results)
                {
                    listofresult.Add(r.Document);
                }
                IEnumerable<Product> result = listofresult.ToList();



                return View("SearchView", result);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public ActionResult SearchView()
        {
            return View();
        }


        public byte[] CreatingThumbnal(HttpPostedFileBase image)
        {
            try
            {
                
                Image imgOriginal = Image.FromStream(image.InputStream);
           
                int ThumbnailWidth=150;
                int ThumbnailHeight=100;
               
                Bitmap ThumbnailBitmap = new Bitmap(ThumbnailWidth, ThumbnailHeight);
                Graphics ResizedImage = Graphics.FromImage(ThumbnailBitmap);
                ResizedImage.InterpolationMode = InterpolationMode.HighQualityBicubic;
                ResizedImage.CompositingQuality = CompositingQuality.HighQuality;
                ResizedImage.SmoothingMode = SmoothingMode.HighQuality;
                ResizedImage.DrawImage(imgOriginal, 0, 0, ThumbnailWidth, ThumbnailHeight);
                ImageConverter converter = new ImageConverter();
                return (byte[])converter.ConvertTo(ThumbnailBitmap, typeof(byte[]));
            }
            catch(Exception e)
            {
                return null;
            }

           // return imgThumbnail;
        }
        public bool CallbackFun()
        {
            return true;
        }


        //public async Task<bool> StoringToCache(string userid,string role)
        //{
        //    try
        //    {


        //        var configuration = "chaistoreecom.redis.cache.windows.net:6380,password=XgjxKmY/3ppiWrHlfJ97/DfI2paYZa5WMOCdWBeRtIE=,ssl=True,abortConnect=False";
        //        var configurationOptions = ConfigurationOptions.Parse(configuration);
        //        ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(configurationOptions);
        //        IDatabase redisDatabase = connection.GetDatabase();

        //        if (role == "Merchant")
        //        {
        //            IEnumerable<Product> valueq = await DocumentDBRepository<Product>.GetItemsAsync(d => d.UserId== userid);
        //            foreach (Product p in valueq)
        //            {
        //                redisDatabase.StringSet(p.UserId, JsonConvert.SerializeObject(valueq), TimeSpan.FromMinutes(15));
        //            }


        //        }else if(role == "Customer")
        //        {
        //            IEnumerable<Product> item1 = await DocumentDBRepository<Product>.GetItemsAsyncFromCart(d => d.UserId == userid);

        //                redisDatabase.StringSet(userid, JsonConvert.SerializeObject(item1), TimeSpan.FromMinutes(15));


        //            var product = JsonConvert.DeserializeObject<Product>(redisDatabase.StringGet(userid));
        //            List<Product> list = new List<Product>();
        //            list.Add(product);
        //        }

        //        return true;



        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }
        //}
    }
}
using ProductTaskDay3.Models;
using ProductTaskDay3.ProductTask.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProductTaskDay3.Controllers
{
    
    public class HomeController : Controller
    {
      //  Imagekey imagekey = new Imagekey();
        BlobStorage blobStorage = new BlobStorage();
        SearchController SearchController = new SearchController();
        OperationWithCosmos operationWithCosmos = new OperationWithCosmos();
        OperationWithDatabase operationWithDatabase = new OperationWithDatabase();
        StorageInQueue storageInQueue = new StorageInQueue();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddingProduct()
       {
            return View();
        }

        [HttpPost]
       // [OutputCache(Duration = 900)]
        public async Task<ActionResult> AddingProduct(Product product )
         {
            HttpPostedFileBase file = Request.Files["ImageData"];
            string originalUrl =await blobStorage.UploadImageAsync(file);
            var image = SearchController.CreatingThumbnal(file);
           string thumbnailUrl = blobStorage.UploadImageAsyncasByte(image, file.FileName);
          //  imagekey.ProductKey = imageUrl;
          //  imagekey.Id = product.Id;
         //   product.image = imageUrl;
           product.UserId= Session["User"].ToString();
                   
                  //  await DocumentDBRepository<Product>.CreateItemAsync(product);
          await operationWithDatabase.AddingProductToDatabase(product);
            IEnumerable<Product> items = operationWithDatabase.GetProductItems(product);
            //IEnumerable<Product> items = await operationWithDatabase.GetItemsAsync(d => d.UserId == product.UserId && d.Name==product.Name && d.Description==product.Description && d.Price==product.Price && d.Quantity==product.Quantity);
            foreach (Product p in items)
            {
               operationWithDatabase.AddingImageToDatabase(p, originalUrl, thumbnailUrl);
               // operationWithDatabase.AddingImageToDatabase(p, originalUrl, thumbnailUrl);
            }
                return RedirectToAction("Display");
        }

       // [OutputCache(Duration = 900)]
        public async Task<ActionResult> Display()
        {
            Product result = new Product();
            string userid = Session["User"].ToString();
            //IEnumerable<Product> result = productOperation.Displaying(userid);
            List<ProductImage> list = new List<ProductImage>();
            IEnumerable<Product> items = operationWithDatabase.GetProductItemsOfMerchant(userid);
            foreach (Product product in items)
            {
                IEnumerable<ProductImage> itemimages= operationWithDatabase.GetProductImages(product);
                foreach (ProductImage image in itemimages)
                {
                    list.Add(image);
                }
                product.imagelist = list;
                list = new List<ProductImage>();
            }
            return View(items);
        }

        //[OutputCache(Duration = 900)]
        public async Task<ActionResult> AddingCart()
        {
            try
            {
                string userid = Session["UserName"].ToString();
                IEnumerable<Product> items = await DocumentDBRepository<Product>.GetItemsAsync(d => d.UserId!=null);
                return View(items);
            }catch(Exception e)
            {
                return View();
            }
        }

       // [OutputCache(Duration = 900)]
        public async Task<ActionResult> AddingtoCart(string UserId, string Name, string Description, string Price, string image)
        {
            Product product = new Product();
            product.UserId = Session["UserName"].ToString();
            product.Name = Name;
            product.Description = Description;
            product.Price = Convert.ToDouble(Price);
            await DocumentDBRepository<Product>.CreateItemAsyncInCart(product);
            return RedirectToAction("DisplayingCart");
        }

      // [OutputCache(Duration =900)]
        public async Task<ActionResult> DisplayingCart()
        {
            string userid = Session["UserName"].ToString();
            IEnumerable<Product> items = await DocumentDBRepository<Product>.GetItemsAsyncFromCart(d => d.UserId == userid);
            return View(items);
        }

        //[OutputCache(Duration = 900)]
        public async Task<ActionResult> DeletingProduct(string Name,string Description,string Price)
        {
            string id;
            string email;
            string UserId = Session["User"].ToString();
            IEnumerable<Product> items = await DocumentDBRepository<Product>.GetItemsAsync(d => d.Name == Name && d.UserId == UserId && d.Description ==Description && d.Price ==Convert.ToDouble( Price));
            IEnumerable<Product> item1= await DocumentDBRepository<Product>.GetItemsAsyncFromCart(d => d.Name == Name && d.Description == Description && d.Price == Convert.ToDouble(Price));
            foreach (Product p in items)
            {
                id = p.id;
               await DocumentDBRepository<Product>.DeleteDocumentFromCollectionAsync(id);
            }

            foreach (Product p in item1)
            {
                id = p.id;
                email = p.UserId;
                StorageInQueue g = new StorageInQueue();
                
                await DocumentDBRepository<Product>.DeleteDocumentFromCollectionAsync1(id);
                g.SendingToQueue1(email);
            }

            return RedirectToAction("Display", "Home");
        }


        public async Task<ActionResult> AddImages(string id,string UserId, HttpPostedFileBase ImageData)
        {
            string originalUrl = await blobStorage.UploadImageAsync(ImageData);
            var image = SearchController.CreatingThumbnal(ImageData);
            storageInQueue.SendingImageToQueue(id, UserId, ImageData, originalUrl);
            //string thumbnailUrl = blobStorage.UploadImageAsyncasByte(image, ImageData.FileName);
            //ProductImage productImage = new ProductImage();
            //productImage.user_id = UserId;
            //productImage.product_key = id;
            //productImage.originalimage_url = originalUrl;
            //productImage.thumbnail_url = thumbnailUrl;
            //await DocumentDBRepository<ProductImage>.CreateItemImages(productImage);
            return RedirectToAction("Display");
        }





    }
}
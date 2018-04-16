using ProductTaskDay3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ProductTaskDay3.ProductTask.DAL
{
    public class OperationWithDatabase
    {
        ProductImage productImage = new ProductImage();

        public void AddingImageToDatabase(Product product,string OriginalUrl,string thumbnailImage)
        {
            productImage.product_key = product.id;
            productImage.user_id = product.UserId;
            productImage.originalimage_url = OriginalUrl;
            productImage.thumbnail_url = thumbnailImage;
            using (ProductDbContext ProductContext =new ProductDbContext())
            {
                try
                {
                    ProductContext.ProductImages.Add(productImage);
                    ProductContext.SaveChanges();
                }catch(Exception e)
                {

                }


            }


        }

        public async Task AddingProductToDatabase(Product product)
        {
            using (ProductDbContext productDbContext = new ProductDbContext())
            {
                try
                {
                   productDbContext.Products.Add(product);
                    productDbContext.SaveChanges();
                }
                catch(Exception e)
                {

                }
            }
        }

        public IEnumerable<Product> GetProductItems(Product product)
        {
            try
            {
                using (ProductDbContext productDbContext = new ProductDbContext())
                {
                    IEnumerable<Product> products = productDbContext.Products.Where(x => x.UserId == product.UserId && x.Name == product.Name && x.Description == product.Description && x.Price == product.Price).ToList();
                    return products;
                }
            }catch(Exception e)
            {
                return null;
            }
        }

        public IEnumerable<Product> GetProductItemsOfMerchant(string UserId)
        {
            try
            {
                using (ProductDbContext productDbContext = new ProductDbContext())
                {
                    IEnumerable<Product> products = productDbContext.Products.Where(x => x.UserId == UserId).ToList();
                    return products;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IEnumerable<ProductImage> GetProductImages(Product product)
        {
            try
            {
                using (ProductDbContext productDbContext = new ProductDbContext())
                {
                    IEnumerable<ProductImage> products = productDbContext.ProductImages.Where(x => x.product_key== product.id && x.user_id==product.UserId).ToList();
                    return products;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }





    }
}
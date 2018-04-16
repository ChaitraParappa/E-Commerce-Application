using ProductTaskDay3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ProductTaskDay3.ProductTask.DAL
{
    public class OperationWithCosmos
    {
        ProductImage productImage = new ProductImage();

        public async Task AddingImageToCosmos(Product product,string originalUrl,string thumbnailUrl) 
        {
            productImage.product_key= product.id;
            productImage.user_id = product.UserId;
            productImage.originalimage_url = originalUrl;
            productImage.thumbnail_url = thumbnailUrl;
            await DocumentDBRepository<ProductImage>.CreateItemImages(productImage);


        }
    }
}
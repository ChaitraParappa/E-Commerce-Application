using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductTaskDay3.Models
{
    public class Product
    {
      
        [Key]
        public string id { get; set; }

        public string p_id { get; set; }

        public string UserId {get;set;}

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

       public List<ProductImage> imagelist { get; set; }

        public int Quantity { get; set; }
       
    }
}
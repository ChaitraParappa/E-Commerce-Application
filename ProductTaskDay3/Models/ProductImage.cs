using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ProductTaskDay3.Models
{
    public class ProductImage
    {
        [key]
        public int id { get; set; }

        public string product_key { get; set; }

        public string user_id { get; set; }

        public string originalimage_url { get; set; }

        public string thumbnail_url { get; set; }

    }
}
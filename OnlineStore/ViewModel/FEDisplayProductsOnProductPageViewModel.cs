using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class FEDisplayProductsOnProductPageViewModel
    {
        [Key]
        public long ProductID { get; set; }
        public string ProductSideViewThumbnail { get; set; }
        public string ProductFrontViewThumbnail { get; set; }
        public string ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductLocation { get; set; }
        public string ProductName { get; set; }
        public string ProductSKU { get; set; }
        public bool? ProductIsNew { get; set; }
        public int? ProductQuantity { get; set; }
    }
}
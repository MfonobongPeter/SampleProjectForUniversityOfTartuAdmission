using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class FERelatedProductListViewModel
    {
        [Key]
        public Int64 ProductID { get; set; }
        public string ProductSideViewThumb { get; set; }
        public string ProductFrontViewThumb { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int? ProductQuantity { get; set; }           
        public string ProductSKU { get; set; }
        public bool? ProductIsNew { get; set; }
    }
}
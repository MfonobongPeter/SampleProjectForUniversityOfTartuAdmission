using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class FEOrderDetailsViewModel
    {
        [Key]
        public long OrderID {get;set;}
        public long ProductID { get; set; }
        public string TransactionID { get; set; }
       
        public string ProductName { get; set; }
        public string ProductSku { get; set; }
        public string ProductFrontViewThumb { get; set; }
        public int? ProductQuantity { get; set; }
        public decimal ProductUnitPrice { get; set; }
        public decimal ProductTotalPrice { get; set; }

    }
}
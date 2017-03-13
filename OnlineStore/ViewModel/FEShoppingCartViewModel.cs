using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class FEShoppingCartViewModel
    {
        [Key]

        public long ShoppingCartRowID { get; set; }
        public string CartSessionID { get; set; }
        public long ProductID { get; set; }
        public string ProductSku { get; set; }
        public string FrontViewThumbNail { get; set; }
        public string ProductName { get; set; }
        public decimal ProductTotalPrice { get; set; }
        public decimal ProductPrice { get; set; }

        [Range(1, 100, ErrorMessage = "Between 1 and 100 only")]        
        public int ProductQuantity { get; set; }
        public string PaymentOption { get; set; }
        public string DeliveryOption { get; set; }

        public DateTime? DateCreated { get; set; }
    }
}
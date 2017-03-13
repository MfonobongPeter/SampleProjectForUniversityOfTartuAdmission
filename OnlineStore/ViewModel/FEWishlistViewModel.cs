using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class FEWishlistViewModel
    {
        [Key]
        public long WhishListRowID { get; set; }
        public long ProductID { get; set; }
        public string ProductModelNumber { get; set; }
        public decimal? ProductPrice { get; set; }
        public int? ProductQuantity { get; set; }
        public string ProductName { get; set; }
        public decimal? ProductTotalPrice { get; set; }
        public string WishListSessionID { get; set; }

    }
}
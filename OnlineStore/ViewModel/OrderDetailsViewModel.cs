using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class OrderDetailsViewModel
    {      
            [Key]
            public long OrderDetailID { get; set; }
            public long OrderID { get; set; }
            public long ProductID { get; set; }
            public string ProductName { get; set; }
            public Nullable<decimal> ProductPrice { get; set; }
            public string ProductSKU { get; set; }
            public Nullable<System.DateTime> OrderDate { get; set; }
            public Nullable<int> Quantity { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class OrderViewDetailViewModel
    {       
            [Key]
            public long OrderID { get; set; }
            public string Ordername { get; set; }            
            public string ProductName { get; set; }
            public Nullable<decimal> Amount { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string Country { get; set; }
            public string State { get; set; }
            public string City { get; set; }
            public string TransactionNumber { get; set; }
            public string TransactionStatus { get; set; }
            public string DeliveryStatus { get; set; }
            public string OrderType { get; set; }
            public Nullable<System.DateTime> OrderDate { get; set; }      
    }
}
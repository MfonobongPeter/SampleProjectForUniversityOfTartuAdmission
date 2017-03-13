using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class OrdersViewModel
    {
        [Key]
        public long OrderID { get; set; }        
        public long UserID { get; set; }
        public long ProductID { get; set; }
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
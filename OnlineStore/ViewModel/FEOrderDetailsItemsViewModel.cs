using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class FEOrderDetailsItemsViewModel
    {
        [Key]
        public long OrderID { get; set; }
        public string TransactionID { get; set; }
        public string TransIDShort { get; set; }
        public DateTime? OrderDate { get; set; }
        public string OrderDeliveryStatus { get; set; }
        public string OrderPaymentMethod { get; set; }
        public string OrderDeliveryMethod { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserCountry { get; set; }
        public string UserState { get; set; }
        public string UserCity { get; set; }
        public string UserPhoneNumber { get; set; }
        public string UserAddress1 { get; set; }
        public string UserBillingFirstName { get; set; }
        public string UserBillingLastName { get; set; }
        public string UserBillingCountry { get; set; }
        public string UserBillingState { get; set; }
        public string UserBillingCity { get; set; }
        public string UserBillingAddress { get; set; }
        public string UserBillingPhone { get; set; }
    }
}
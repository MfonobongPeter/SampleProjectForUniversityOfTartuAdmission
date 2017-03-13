using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class OrderEditViewModel
    {
        [Key]
        public int OrderID { get; set; }
        [Required(ErrorMessage = "Select Delivery Status!")]
        [Display(Name = "Delivery Status:")]
        public int DeliveryStatus { get; set; }
    }
}
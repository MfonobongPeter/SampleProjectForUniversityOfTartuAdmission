using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class StateViewModel
    {
        [Key]
        public int StateID { get; set; }

        [Display(Name = "State Name:")]
        [Required(ErrorMessage = "Enter State Name!")]
        public string StateName { get; set; }

        [Display(Name = "Shipping fee:")]
        [Required(ErrorMessage = "Enter shipping fee!")]
        [Range(typeof(decimal), "100", "10000", ErrorMessage = "Enter Positive values only for Price (between 100 - 10000)")]
        public decimal ShippingFee { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class LgaViewModel
    {
        public int LgaID { get; set; }

        [Display(Name = "State ID:")]
        public int StateID { get; set; }

        [Required(ErrorMessage = "Enter Lga  name!")]
        [Display(Name = "Lga Name:")]
        public string LgaName { get; set; }

        [Required(ErrorMessage = "Select state name!")]
        [Display(Name = "State Name:")]
        public string StateName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class CountryViewModel
    {
        [Key]
        public int CountryID { get; set; }

        [Display(Name = "Country Name:")]
        [Required(ErrorMessage = "Enter CountryName!")]
        public string CountryName { get; set; }
    }
}
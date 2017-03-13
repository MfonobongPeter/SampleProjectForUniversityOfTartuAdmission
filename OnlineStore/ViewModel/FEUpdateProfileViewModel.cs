using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class FEUpdateProfileViewModel
    {
        [Key]
        public long UserID { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        public string BillingAddress { get; set; }
        [Required]
        public string ContactAddress { get; set; }

        public string OfficeAddress { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Display(Name = "Country:")]
        [Required(ErrorMessage = "Select country")]
        public string CountryID { get; set; }

        [Display(Name = "Gender:")]
        [Required(ErrorMessage = "Select gender")]
        public string GenderID { get; set; }

        [Display(Name = "State:")]
        [Required(ErrorMessage = "Select state")]
        public string UserState { get; set; }

        [Display(Name = "City:")]
        [Required(ErrorMessage = "Select city")]
        public string UserCity { get; set; }
    }
}
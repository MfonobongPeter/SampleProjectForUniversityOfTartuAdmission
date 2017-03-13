using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class ShowSignUpViewModel
    {
        [Key]
        public long UserID { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
       
        [Display(Name = "Address 1")]
        public string Address1 { get; set; }

        [Display(Name = "Country")]       
        public string Country { get; set; }

        [Display(Name = "State")]
        public string State { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }
        
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingCountry { get; set; }
        public string BillingAddress { get; set; }

        [Display(Name = "CreatedOn")]
        public DateTime? CreatedOn { get; set; }    
    }
}
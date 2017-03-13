using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class FEUserRegistrationViewModel
    {
        [Key]
        public long UserID { get; set; }

        [Display(Name = "Country:")]
        [Required(ErrorMessage = "Select country")]
        public int CountryID { get; set; }

        [Display(Name = "Gender:")]
        [Required(ErrorMessage = "Select gender")]
        public int GenderID { get; set; }

        public int SecurityQuestionID { get; set; }
        
        [Display(Name = "First Name:")]
        [Required(ErrorMessage = "Enter first name")]
        public string UserFirstName { get; set; }

        [Display(Name = "Last Name:")]
        [Required(ErrorMessage = "Enter last name")]
        public string UserLastName { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(1500)]
        [Display(Name = "Contact Address:")]
        [Required(ErrorMessage = "Enter contact address")]
        public string UserAddress1 { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(1500)]
        [Display(Name = "Billing Address:")]
        [Required(ErrorMessage = "Enter billing address")]
        public string BillingAddress { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(1500)]
        [Display(Name = "Office Address:")]        
        public string UserAddress2 { get; set; }

        [Display(Name = "State:")]
        [Required(ErrorMessage = "Select state")]
        public string UserState { get; set; }

        [Display(Name = "City:")]
        [Required(ErrorMessage = "Select city")]
        public string UserCity { get; set; }

        [Display(Name = "Phone:")]
        [Required(ErrorMessage = "Enter phone number")]
        public string UserPhone { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email:")]
        [Required(ErrorMessage = "Enter email")]
        public string UserEmail { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password:")]
        [Required(ErrorMessage = "Enter password")]
        public string UserPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password:")]
        [Required(ErrorMessage = "Enter Confirm password")]
        [CompareAttribute("UserPassword", ErrorMessage = "Passwords don't match.")]
        public string UserConfirmPassword { get; set; }
        public string UserRole { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ActivationID { get; set; }
        public bool IsActivated { get; set; }
        public bool IsDeleted { get; set; }
    }
}
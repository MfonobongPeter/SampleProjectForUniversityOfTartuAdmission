using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class SignUpViewModel
    {
        [Key]
        public int UserID { get; set; }
        [Display(Name = "First Name:")]
        [Required(ErrorMessage = "Enter first name")]
        public string UserFirstName { get; set; }


        [Display(Name = "Last Name:")]
        [Required(ErrorMessage = "Enter last name")]
        public string UserLastName { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(1500)]
        [Display(Name = "Address 1:")]
        [Required(ErrorMessage = "Enter address 1")]
        public string UserAddress1 { get; set; }

        [DataType(DataType.MultilineText)]
        [MaxLength(1500)]
        [Display(Name = "Address 2:")]
        //[Required(ErrorMessage = "Enter Address 2!")]
        public string UserAddress2 { get; set; }

        [Display(Name = "Country:")]
        [Required(ErrorMessage = "Select country")]
        public int CountryID { get; set; }

        [Display(Name = "State:")]
        [Required(ErrorMessage = "Select state")]
        public int State { get; set; }

        [Display(Name = "City:")]
        [Required(ErrorMessage = "Select city")]
        public string UserCity { get; set; }

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

        [Display(Name = "Security Question:")]
        [Required(ErrorMessage = "Select security question")]
        public Nullable<int> SecurityQuestionIDList { get; set; }

        [Display(Name = "Security Answer:")]
        [Required(ErrorMessage = "Enter security answer")]
        public string SecurityAnswer { get; set; }

        [Display(Name = "Phone:")]
        [Required(ErrorMessage = "Enter phone number")]
        public string UserPhone { get; set; }

        [Display(Name = "State:")]
        [Required(ErrorMessage = "Enter state")]
        public string notNigeriaState { get; set; }

        [Display(Name = "City:")]
        [Required(ErrorMessage = "Enter city")]
        public string notNigeriaCity { get; set; }

        [Display(Name = "Terms And Conditions:")]
        public string TermsAndConditions { get; set; }
    
        public Nullable<System.DateTime> CreatedOn { get; set; }

        [Display(Name = "Gender:")]
        [Required(ErrorMessage = "Select gender!")]
        public Nullable<int> Gender { get; set; }        
        public Nullable<bool> IsDeleted { get; set; }

        [Display(Name = "Role:")]
        [Required(ErrorMessage = "Select role")]
        public string UserRole { get; set; }
    }
}
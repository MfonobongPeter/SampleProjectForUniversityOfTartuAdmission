using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class FEContactViewModel
    {
        [Display(Name = "Firstname:")]
        [Required(ErrorMessage = "Enter firstname!")]
        public string Firstname { get; set; }

        [Display(Name = "Lastname:")]
        [Required(ErrorMessage = "Enter lastname!")]
        public string Lastname { get; set; }

        [Display(Name = "Email:")]
        [Required(ErrorMessage = "Enter email!")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Subject:")]
        [Required(ErrorMessage = "Enter subject!")]        
        public string Subject { get; set; }

        [Display(Name = "Message:")]
        [Required(ErrorMessage = "Enter message!")]
        [DataType (DataType.MultilineText)]
        public string Message { get; set; }
    }
}
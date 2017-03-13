using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class UserLoginViewModel
    {
        [Display(Name = "Username:")]
        [Required(ErrorMessage = "Enter User Name!")]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }

        [Display(Name = "Password:")]
        [Required(ErrorMessage = "Enter Password!")]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }
    }
}
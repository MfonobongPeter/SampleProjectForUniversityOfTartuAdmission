﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class FEChangeOldPasswordViewModel
    {
        [DataType(DataType.Password)]
        [Display(Name = "Old Password:")]
        [Required(ErrorMessage = "Enter Old password")]
        public string UserOldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New Password:")]
        [Required(ErrorMessage = "Enter New password")]
        public string UserPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password:")]
        [Required(ErrorMessage = "Confirm New password")]
        [CompareAttribute("UserPassword", ErrorMessage = "Passwords don't match.")]
        public string UserConfirmPassword { get; set; }
    }
}
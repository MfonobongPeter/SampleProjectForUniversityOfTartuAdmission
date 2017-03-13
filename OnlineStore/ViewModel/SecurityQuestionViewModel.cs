using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineStore
{
    public class SecurityQuestionViewModel
    {
        [Display(Name = "Security Question:")]
        [Required(ErrorMessage = "Select security question")]
        public Nullable<int> SecurityQuestion { get; set; }

        [Display(Name = "SecurityAnswer:")]
        [Required(ErrorMessage = "Enter Security Answer!")]
        [DataType(DataType.Password)]
        [AllowHtml] 
        public string SecurityAnswer { get; set; }
       
    }
}
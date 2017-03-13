using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class MainSecurityQuestionsViewModel
    {
        [Key]
        public int SecurityQuestionID { get; set; }

        [Required(ErrorMessage = "Enter Security Question")]
        [Display(Name = "Security Question")]
        public string Question { get; set; }
    }
}
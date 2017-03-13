using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class CategorySetUpViewModel
    {
        [Key]
        public int CategoryID { get; set; }

        [Required (ErrorMessage="Enter Category")]
        [Display(Name ="Category Name")]
        public string CategoryName { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }
    }
}
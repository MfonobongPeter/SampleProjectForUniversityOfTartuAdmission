using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class SubCategoryViewModel
    {
        public int SubCategoryID { get; set; }

        [Required(ErrorMessage = "Select Category name!")]
        [Display(Name = "Category Name")]
        public string CategoryID { get; set; }
        [Required(ErrorMessage = "Select Category name!")]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }

        [Required(ErrorMessage = "Enter Sub Category name!")]
        [Display(Name = "Sub Category Name")]
        public string SubCategoryName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class CategoryMetaData
    {
            public int CategoryID { get; set; }
            [Required(ErrorMessage = "Category name is required!")]
            [Display(Name="Category Name")]
            public string CategoryName { get; set; }
    }


    public class CountryMetaData
    {
        [Required(ErrorMessage = "Country name is required!")]
        [Display(Name = "Country Name:")]
        public string CountryName { get; set; }
    }

    public class StateMetaData
    {
        [Required (ErrorMessage="State name is required!")]
        [Display(Name = "State Name:")]
        public string StateName { get; set; }
    }

    public class SecurityQuestionMetaData
    {
        [Key]
        public int SecurityQuestionID { get; set; }

        [Required(ErrorMessage = "Enter Security Question")]
        [Display(Name = "Security Question")]
        public string Question { get; set; }
    }

    public class LgaMetaData
    {
        
        public string LgaID { get; set; }

        [Required(ErrorMessage = "State name is required!")]
        [Display(Name = "State Name:")]
        public string StateID { get; set; }

       // [Required(ErrorMessage = "Enter Lga name")]
        [Display(Name = "Lga Name:")]
        public string LgaName { get; set; }
    }

    public class RoleMetaData
    {
        [Key]
        public int RoleiD { get; set; }

        [Required(ErrorMessage = "Role name is required!")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        [Required(ErrorMessage = "Role desc is required!")]
        [Display(Name = "Role Desc")]
        public string RoleDesc { get; set; }
    }

    public class SubCategoryMetaData
    {
        [Required(ErrorMessage = "Category name is required!")]
        [Display(Name = "Category Name")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Sub Category name is required!")]
        [Display(Name = "Sub Category Name")]
        public string SubCategoryName { get; set; }
    }
}
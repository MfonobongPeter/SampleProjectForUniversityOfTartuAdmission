using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class ProductViewModel
    {
        public long ProductID { get; set; }

        [Display(Name = "Product SubCategory:")]
        [Required(ErrorMessage = "Select Product SubCategory")] 
        public int SubCategoryID { get; set; }

        [Display(Name = "Category:")]
        [Required(ErrorMessage = "Select Product Category")] 
        public int CategoryID { get; set; }

        [Display(Name = "Product Model Number:")]
        [Required(ErrorMessage = "Enter Product Model Number")] 
        public string ProductSKU { get; set; }

        [Display(Name = "Product Quantity:")]
        [Required(ErrorMessage = "Enter Product Quantity")]

        [Range(1, 200, ErrorMessage ="Enter positive number between 1 and 200")]
        public Nullable<int> ProductQuantity { get; set; }

        [Display(Name = "Product Unit Price:")]
        [Required(ErrorMessage = "Enter Product Unit Price")] 

        [Range(typeof(decimal),"100", "1000000000", ErrorMessage="Enter Positive values only for Price (between 100 - 1000000000)")]
        public decimal ProductUnitPrice { get; set; }

        [Display(Name = "Product Name:")]
        [Required(ErrorMessage = "Enter Product Name")] 
        public string ProductName { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Product Short Desc:")]
        [Required(ErrorMessage = "Enter Product Short Desc")] 
        public string ProductshortDesc { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Product Long Desc:")]
        [Required(ErrorMessage = "Enter Product Long Desc")] 
        public string ProductLongDesc { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Product Side View:")]
        [Required(ErrorMessage = "Please choose Product Side View file to upload.")]
        public string ProductSideView { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Side View Thumbnail:")]
        [Required(ErrorMessage = "Please choose Product Side View Thumbnail file to upload.")]
        public string ProductSideViewThumbnail { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Product Front View:")]
        [Required(ErrorMessage = "Please choose Product Front View file to upload.")]
        public string ProductFrontView { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Front View Thumbnail:")]
        [Required(ErrorMessage = "Please choose Product Front View Thumbnail file to upload.")]
        public string ProductFrontViewThumbnail { get; set; }
        
        [Display(Name = "Product Location:")]
        [Required(ErrorMessage = "Enter Product Location")] 
        public string ProductLocation { get; set; }

        [Display(Name = "Product Weight:")]
       // [Required(ErrorMessage = "Enter Product Weight")] 
        public Nullable<float> ProductWeight { get; set; }

        [Display(Name = "Product Colour:")]
        //[Required(ErrorMessage = "Enter Product Colour")] 
        public string ProductColour { get; set; }

        [Display(Name = "Product Discount:")]
        public Nullable<decimal> ProductDiscount { get; set; }

        [Display(Name = "Shipping Fee:")]
        [Required(ErrorMessage = "Enter shipping fee")]
        [Range(typeof(decimal), "100", "10000", ErrorMessage = "Enter Positive values only for Price (between 100 - 10000)")]
        public Nullable<decimal> ShippingFee { get; set; }

        //[Display(Name = "Product Discount:")]
        //public Nullable<decimal> ProductSEOUrlFriendlyPath { get; set; }
    }
}
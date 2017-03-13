using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class ProductViewEditViewModel
    {
        [Key]
        public long ProductID { get; set; }

        
        [Required(ErrorMessage = "Enter Product ProductSKU")] 
        public string ProductSKU { get; set; }

        
        [Required(ErrorMessage = "Enter Product Quantity")] 
        [Range(0, 200, ErrorMessage = "Enter positive number between 1 and 200")]
        public Nullable<int> Quantity { get; set; }

        
        [Required(ErrorMessage = "Enter Product Price")] 

        [Range(typeof(decimal), "100", "1000000000", ErrorMessage = "Enter Positive values only for Price (between 100 - 1000000000)")]
        public decimal Price { get; set; }

       
        [Required(ErrorMessage = "Enter Product Name")] 
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]

        
        [Required(ErrorMessage = "Enter Product Short Description")] 
        public string ShortDescription { get; set; }

        
        [Required(ErrorMessage = "Enter Product Long Description")] 
        [DataType(DataType.MultilineText)]
        public string LongDescription { get; set; }

        
        [Required(ErrorMessage = "Enter Product Location")] 
        public string Location { get; set; }

        
        [Required(ErrorMessage = "Enter Product Short Description")] 
        public Nullable<decimal> Weight { get; set; }

        
        public string CreatedBy { get; set; }

        
        [Required(ErrorMessage = "Enter Product Availability")] 
        public string Availability { get; set; }

       
        [Required(ErrorMessage = "Enter Product Colour")] 
        public string Colour { get; set; }

        [Display(Name = "Shipping Fee:")]
        [Required(ErrorMessage = "Enter shipping fee")]
        [Range(typeof(decimal), "100", "10000", ErrorMessage = "Enter Positive values only for Price (between 100 - 10000)")]
        public Nullable<decimal> ShippingFee { get; set; }

        public DateTime? ProductCreatedOn { get; set; }
    }
}
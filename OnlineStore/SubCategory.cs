//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineStore
{
    using System;
    using System.Collections.Generic;
    
    public partial class SubCategory
    {
        public SubCategory()
        {
            this.Products = new HashSet<Product>();
        }
    
        public int SubCategoryID { get; set; }
        public int CategoryID { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public string SubCategoryName { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
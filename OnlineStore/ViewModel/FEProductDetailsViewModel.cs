using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class FEProductDetailsViewModel
    {
        public long ProductID { get; set; }
        public string ProductSideView { get; set; }
        public string ProductSideViewThumb { get; set; }
        public string ProductDesc { get; set; }
        public decimal ProductPrice { get; set; }
        public string ProductLocation { get; set; }
        public string ProductName { get; set; }
        public int? ProductQuantity { get; set; }
        public string ProductFrontView { get; set; }
        public string ProductFrontViewThumb { get; set; }
        public string ProductAvailability { get; set; }
        public string ProductColour { get; set; }
        public string ProductLongDesc { get; set; }
        public string ProductSKU { get; set; }
        public decimal? ProductWeight { get; set; }
        public bool? ProductIsNew { get; set; }

    }
}
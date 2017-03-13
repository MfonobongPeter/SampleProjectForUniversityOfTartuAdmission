using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class FECombineViewModel
    {
        public IEnumerable<FERelatedProductListViewModel> List { get; set; }
        public FEProductDetailsViewModel Details { get; set; }
    }
}
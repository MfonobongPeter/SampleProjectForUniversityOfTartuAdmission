using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class FECombineOrderDetailsViewModel
    {
        public IEnumerable<FEOrderDetailsViewModel> List { get; set; }
        public FEOrderDetailsItemsViewModel OrderItems { get; set; }
    }
}
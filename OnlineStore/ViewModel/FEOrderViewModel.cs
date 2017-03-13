using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class FEOrderViewModel
    {
        [Key]
        public long OrderID { get; set; }
        public string TransID { get; set; }
        public string UrlTransID { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }
        public string Status { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlineStore
{
    public class FEPaymentApiViewModel
    {
        [Key]
        public string transaction_id { get; set; }
        public string Merchant_id { get; set; }
        public string email { get; set; }
        public string total { get; set; }
        public string merchant_ref { get; set; }
        public string memo { get; set; }
        public string status { get; set; }
        public string date { get; set; }
        public string method { get; set; }
        public string referrer { get; set; }
        public string total_credited_to_merchant { get; set; }
        public string extra_charges_by_merchant { get; set; }
        public string charges_paid_by_merchant { get; set; }
        public string fund_maturity { get; set; }
        public string total_paid_by_buyer { get; set; }
        public double process_duration { get; set; }
    }
}
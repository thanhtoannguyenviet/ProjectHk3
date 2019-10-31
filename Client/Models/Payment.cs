using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class Payment
    {
        public int id { get; set; }
        public string paymentId { get; set; }
        public Nullable<decimal> totalMoney { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<int> customerId { get; set; }
    }
}
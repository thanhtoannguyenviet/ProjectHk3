using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class Detail
    {
        public int id { get; set; }
        public Nullable<int> customerId { get; set; }
        public Nullable<int> staffId { get; set; }
        public Nullable<System.DateTime> startDate { get; set; }
        public Nullable<System.DateTime> endDate { get; set; }
        public Nullable<decimal> amountMoney { get; set; }
        public Nullable<int> statusOrder { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<int> paymentId { get; set; }
    }
}
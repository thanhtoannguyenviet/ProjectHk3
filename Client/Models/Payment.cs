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
        public decimal? totalMoney { get; set; }
    }
}
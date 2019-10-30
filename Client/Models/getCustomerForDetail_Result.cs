using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public partial class getCustomerForDetail_Result
    {
        public int id { get; set; }
        public string headEmail { get; set; }
        public string headPhone { get; set; }
        public string headName { get; set; }
        public Nullable<System.DateTime> headBirtday { get; set; }
        public string taxCode { get; set; }
        public string address_ { get; set; }
        public Nullable<bool> checkOTP { get; set; }
        public Nullable<int> active { get; set; }
        public int idDetail { get; set; }
    }
}
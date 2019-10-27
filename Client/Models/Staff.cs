using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class Staff
    {
        public int id { get; set; }
        public string staffEmail { get; set; }
        public string staffPhone { get; set; }
        public string staffName { get; set; }
        public DateTime? staffBirtday { get; set; }
        public string department { get; set; }
        public int? mistakeCount { get; set; }
        public string bankCard { get; set; }
        public int? status_ { get; set; }
    }
}
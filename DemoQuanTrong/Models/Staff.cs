//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DemoQuanTrong.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Staff
    {
        public int id { get; set; }
        public string staffEmail { get; set; }
        public string staffPhone { get; set; }
        public string staffName { get; set; }
        public Nullable<System.DateTime> staffBirtday { get; set; }
        public string department { get; set; }
        public Nullable<int> mistakeCount { get; set; }
        public string bankCard { get; set; }
        public Nullable<int> status_ { get; set; }
    }
}

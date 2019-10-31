using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class Account
    {
        public int id { get; set; }
        [Display(Name="Username")]
        public string userName { get; set; }
        [Display(Name = "Password")]
        public string pass_word { get; set; }
        [Display(Name = "Role")]
        public Nullable<int> role_ { get; set; }
    }
}
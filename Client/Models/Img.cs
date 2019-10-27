using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class Img
    {
        public int id { get; set; }
        public string path_ { get; set; }
        public string entryName { get; set; }
        public int? entryId { get; set; }
        public bool? active { get; set; }
    }
}
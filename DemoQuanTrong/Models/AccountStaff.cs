using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoQuanTrong.Models
{
    public class AccountStaff : Account
    {
        public Account account { get; set; }
        public Staff staff { get; set; }
        public List<Img> imgs { get; set; }
        public List<Service_> services { get; set; }
        public List<Detail> details { get; set; }
        public string messsage { get; set; }

        public AccountStaff()
        {
            account = new Account();
            staff = new Staff();
            imgs = new List<Img>();
            services = new List<Service_>();
            details = new List<Detail>();
            messsage = "";

        }
        public AccountStaff(Account account, Staff staff, List<Img> imgs, List<Service_> services, List<Detail> details)
        {
            this.account = account ?? throw new ArgumentNullException(nameof(account));
            this.staff = staff ?? throw new ArgumentNullException(nameof(staff));
            this.imgs = imgs;
            this.services = services;
            this.details = details;
        }
    }
}
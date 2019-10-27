using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoQuanTrong.Models
{
    public class DetailPayment
    {
        public List<Detail> details { get; set; }
        public Payment payment { get; set; }
        public Customer customer { get; set; }

        public DetailPayment()
        {
            details = new List<Detail>();
            payment = new Payment();
            customer = new Customer();
        }
        public DetailPayment(List<Detail> details, Payment payment, Customer customer)
        {
            this.details = details;
            this.payment = payment;
            this.customer = customer;
        }
    }
}
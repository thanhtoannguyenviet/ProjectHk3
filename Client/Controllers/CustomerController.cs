using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Client.Models;
using Newtonsoft.Json;
using static Client.Service.CustomerService;
namespace Client.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        // GET: Customer

        [Authorize(Roles = "Customer")]
        public ActionResult Index()
        {
            ViewBag.Count = CountStaffCustomer();
            return View(GetStaffCustomer(0));
        }


        [Authorize(Roles = "Customer")]
        [HttpPost]
        public ActionResult Index(int id)
        {
            return View(GetStaffCustomer(id));
        }

        [Authorize(Roles = "Customer")]
        public ActionResult Order()
        {
            var session = Session["Account"]  as AccountCustomer;
            var customer = session.customer;
            ViewBag.Count = CountAllOrder(customer);
            return View(GetAllOrder(customer, 0));
        }

        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Order(int id)
        {
            var customer = Session["Account"] as Customer;
            ViewBag.Count = CountAllOrder(customer);
            return View(GetAllOrder(customer, id));
        }

        [Authorize(Roles = "Customer")]
        public ActionResult Payment()
        {
            var customer = Session["Account"] as Customer;
            ViewBag.Count = CountPayment(customer);
            return View(GetPayment(customer,0));
        }

        [Authorize(Roles = "Customer")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Payment(int id)
        {
            var customer = Session["Account"] as Customer;
            ViewBag.Count = CountPayment(customer);
            return View(GetPayment(customer, id));
        }
        //detailStaff
    }
}
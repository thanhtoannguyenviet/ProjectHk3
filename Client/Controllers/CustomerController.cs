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
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            int pageNummber = CountStaffCustomer()/3;
            ViewBag.Count = pageNummber;
            return View(GetStaffCustomer(0));
        }

        public ActionResult StaffCanHire(int id)
        {
            return PartialView("_StaffCanHire", GetStaffCustomer(id));
        }

        public ActionResult Order()
        {
            var session = Session["Account"]  as AccountCustomer;
            var customer = session.customer;
            ViewBag.Count = CountAllOrder(customer)/3;
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
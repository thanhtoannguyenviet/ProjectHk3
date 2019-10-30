using System;
using System.Collections.Generic;
using System.IO;
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

        [Authorize(Roles = "Customer")]
        public ActionResult Index()
        {
            ViewBag.Count = CountStaffCustomer() / 3;
            return View(GetStaffCustomer(0));
        }

        public ActionResult StaffCanHire(int id)
        {
            return PartialView("_StaffCanHire", GetStaffCustomer(id));
        }
        [Authorize(Roles = "Customer")]
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

        public ActionResult SettingView()
        {
            AccountCustomer accountCustomer = Session["Account"] as AccountCustomer;
            
            return View(accountCustomer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SettingView(AccountCustomer accountCustomer)
        {
            var newAccountCustomer = UpdateCustomer(accountCustomer);
            if (newAccountCustomer != null)
            {
                return Index();
            }
            return View(accountCustomer);
        }
        //detailStaff
        [HttpPost]
        public ActionResult Img(HttpPostedFileBase ImageFile)
        {
            AccountCustomer accountCustomer = Session["Account"] as AccountCustomer;
            if (ImageFile.ContentLength > 0) {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                ImageFile.SaveAs(path);
            accountCustomer.img.path_ = "~/Images/" + fileName;
            var img = UpdateImg(accountCustomer.img);
            if (img != null)    
            {
                accountCustomer.img = img;
                Session["Account"] = accountCustomer;
                ViewBag.Count = CountStaffCustomer() / 3;
                ViewBag.UpdateSuccess = 1;
                return View("Index",GetStaffCustomer(0));
            }
            }
            return SettingView();
        }
    }
}
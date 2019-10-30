using Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Newtonsoft.Json;
using static Client.Service.CustomerService;
using static Client.Service.LoginService;
using  static Client.Common.Crypts;
namespace Client.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
          if(Session["Account"]==null&& FormsAuthentication.IsEnabled == true) { 
            FormsAuthentication.SignOut();
            return View(new Account());
          }
          else if (Session["Account"] as AccountStaff != null)
          {
              return Redirect("/Admin/");
          }
          else return Redirect("/Customer/");
        }
        [HttpPost]
        public ActionResult LogIn()
        {
            var usernames = Request["UserAccount"];
            var password = EnCrypt(Request["PasswordAccount"]);
            var person = CheckLogin(usernames, password);
            if (person != null)
            {

                if (person.role_ == 1)
                {
                    var customer = LoginCustomer(person);
                    Session["Account"] = customer;
                    ViewBag.Name = customer.customer.headName;
                    var username = Common.Role.GetValue(person.role_);
                    FormsAuthentication.SetAuthCookie(Common.Role.GetValue(person.role_), true);
                    return Redirect("/Customer/");
                }
                else if (person.role_ > 1)
                {
                    var staff = LoginStaff(person);
                    Session["Account"] = staff;
                    ViewBag.Name = staff.staff.staffName;
                    return Redirect("/Admin/");
                }
            }
            else
            {
                ViewBag.Error = "1";
                ModelState.AddModelError("","Invalid user and password");
            }
            return View("Index"); 
        }
        [HttpPost]
        public ActionResult Register()
        {
            var accountCus = new AccountCustomer();
            accountCus.customer.headName = Request["Fullname"].Trim();
            accountCus.customer.headEmail = Request["Email"].Trim();
            accountCus.account.pass_word = EnCrypt(Request["Password"]);
            accountCus.customer.headPhone = Request["Phone"].Trim();
            accountCus.account.userName = Request["Username"].Trim();
            //accountCus.customer.headBirtday =DateTime.Parse( Request["Birthday"]);
            accountCus.customer.taxCode = Request["taxCode"];
            accountCus.account.role_ = 1;
            var done = RegisterCustomer(accountCus);
            
            if (done != null)
            {
                Session["Account"] = done;
                return Redirect("/Admin/");
            }
            //Chua tra ve loi khong dang ky duoc
            else return Redirect("/Home/");
        }

        public ActionResult LogOut()
        {
            Session.Remove("Account");
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}
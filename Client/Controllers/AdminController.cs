using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Client.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Client.Common;
using static Client.Service.LoginService;
using static Client.Service.StaffService;
using static Client.Service.PaymentService;
using static Client.Common.Crypts;
namespace Client.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin

        [Authorize(Roles = "Admin,HR,Trainer,NhanVien")]
        public ActionResult Index()
        {
            var accountStaff = Session["Account"] as AccountStaff;
            // ngay đây đối tượng trả về là getCustomerForDetail_Result
            //chưa truyền vào id staff dc (id = 0)
            ViewBag.Model = GetDetailForStaff(accountStaff.staff.id);
            return View(accountStaff.details);
        }
        

        [Authorize(Roles = "Admin")]
        public ActionResult Account()
        {
            var accountStaff = Session["Account"] as AccountStaff;
            return View(FindWithRole(accountStaff.account.role_));
        }
        [HttpGet]
        public ActionResult AcceptReplyCustomer(int detail)
        {
            var accountSaff = Session["Account"] as AccountStaff;
            var update=accountSaff.details.Find(s => s.id == detail);
            update.statusOrder = 1;
            var updateDetail = UpdateDetail(update);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult BanAccount(int id)
        {
            var accountStaff = Session["Account"] as AccountStaff;
            var q=FindWithRole(accountStaff.account.role_).Where(t=>t.staff.id==id).FirstOrDefault();
            q.account.role_ = -1;
            UpdateStaff(q);
            return RedirectToAction("Account");
        }

        [Authorize(Roles = "Admin,Trainer")]
        public ActionResult Service()
        {
            var accountStaff = Session["Account"] as AccountStaff;
            return View(FindWithRole(accountStaff.account.role_));
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Trainer")]
        public ActionResult SettingServiceDetail(FormCollection form)
        {
            
            var inbound = false;
            var outbound = false;
            var telemarketing = false;
            if (!string.IsNullOrEmpty(form["inbound"]))
            {
                inbound = true;
            }

            if (!string.IsNullOrEmpty(form["outbound"]))
            {
                outbound = true;}
            if (!string.IsNullOrEmpty(form["telemarketing"]))
            {
                telemarketing = true;
            }
            var accountStaff = Session["Acc"] as AccountStaff;
            List<Service_> lsService = new List<Service_>(); 
            
            if (inbound == true)
            {
                var updateService = new Client.Models.Service_();
                updateService.serviceName = "In-bound";
                updateService.staffId = accountStaff.staff.id;
                lsService.Add(updateService);
            }

            if (outbound == true)
            {
                var updateService = new Client.Models.Service_();
                updateService.serviceName = "Out-bound";
                updateService.staffId = accountStaff.staff.id;
                lsService.Add(updateService);
            }

            if (telemarketing == true)
            {
                var updateService = new Client.Models.Service_();
                updateService.serviceName = "Tele Marketing";
                updateService.staffId = accountStaff.staff.id;
                lsService.Add(updateService);
            }
            string result = "";
            if (accountStaff.services.Count ==0) { 
            accountStaff.services = lsService;
            result = RegisterService(accountStaff);
            }
            else
            {
                foreach (var item in accountStaff.services)
                {
                    UpdateService(item);
                }
                accountStaff.services = lsService;
                result = RegisterService(accountStaff);
                var update = "f";
                if (update!= null)
                    result = "Its update";
            }
            if (result != "") { 
            return PartialView("_PartialService");
            }
            else
            {
                ViewBag.Error = 1;
                return PartialView("_PartialService");
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Trainer")]
        public ActionResult SettingService(int id)
        {
            var accountStaff = Session["Account"] as AccountStaff;
            var lsacc = FindWithRole(accountStaff.account.role_);
            var acc = lsacc.Where(a => a.staff.id == id).FirstOrDefault();
            Session["Acc"] = acc;
            return PartialView("_SettingService",acc);
        }

        [HttpGet]
        public ActionResult DenyReplyCustomer(int detail)
        {
            var accountSaff = Session["Account"] as AccountStaff;
            var update = accountSaff.details.Find(s => s.id == detail);
            update.statusOrder = -1;
            var updateDetail = UpdateDetail(update);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin,HR")]
        public ActionResult NewAccount()
        {
            return View(new AccountStaff());
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize(Roles = "Admin,HR")]
        public ActionResult NewAccount(AccountStaff accountStaff)
        {
            accountStaff.staff.staffBirtday = DateTime.Parse(Request["birthday"]);
            accountStaff.staff.department = Request["idCat"];
            accountStaff.staff.mistakeCount = 0;
            accountStaff.account.pass_word = EnCrypt(accountStaff.account.pass_word);
            accountStaff.staff.status_ = 1;
            accountStaff.account.role_ = Role.GetKey(accountStaff.staff.department);
            accountStaff.imgs.Add(new Img(accountStaff.staff.id));
            accountStaff = RegisterStaff(accountStaff);
            return Redirect("/Admin/");
        }

        [Authorize(Roles = "Admin,HR,Trainer,NhanVien")]
        public ActionResult SettingView()
        {
            AccountStaff accountStaff = Session["Account"] as AccountStaff;
            return View(accountStaff);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,HR,Trainer,NhanVien")]
        public ActionResult SettingView(AccountStaff accountStaff)
        {
            var newaccountStaff = UpdateStaff(accountStaff);
            if (newaccountStaff != null) {
                return RedirectToAction("Index");
            }
            return SettingView();
        }

        [HttpPost]
        public ActionResult ChangePassword(string password)
        {
            AccountStaff accountStaff = Session["Account"] as AccountStaff;
            if (accountStaff.account.pass_word == Request["Oldpassword"] && password == Request["ConfPassword"])
            {
                accountStaff.account.pass_word = password;
                var newaccountStaff = UpdateStaff(accountStaff);
                if (newaccountStaff != null)
                {
                    ViewBag.Success = 1;
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Error = 1;
            return SettingView();
        }
        [HttpPost]
        public ActionResult Img(HttpPostedFileBase ImageFile)
        {
            AccountStaff accountStaff = Session["Account"] as AccountStaff;
            if (ImageFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                ImageFile.SaveAs(path);
                accountStaff.imgs[0].path_ = "~/Images/" + fileName;
                var img = UpdateImg(accountStaff.imgs[0]);
                if (img != null)
                {
                    accountStaff.imgs[0] = img;
                    Session["Account"] = accountStaff;
                    ViewBag.UpdateSuccess = 1;
                    return View("Index");
                }
            }
            return SettingView();
        }
    }
}
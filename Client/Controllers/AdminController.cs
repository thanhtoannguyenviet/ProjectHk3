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

        public ActionResult AcceptReplyCustomer(int detail)
        {
            var accountSaff = Session["Account"] as AccountStaff;
            var update=accountSaff.details.Find(s => s.id == detail);
            update.statusOrder = 1;
            var updateDetail = UpdateDetail(update);
            return RedirectToAction("Index");
        }

        public ActionResult BanAccount(int id)
        {

        }
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
            accountStaff.account.pass_word = EnCrypt(accountStaff.account.pass_word);
            accountStaff.staff.status_ = 1;
            accountStaff.account.role_ = 20;
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
            ViewBag.Error=1
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
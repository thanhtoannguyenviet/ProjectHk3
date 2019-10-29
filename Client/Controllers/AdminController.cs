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
using static Client.Service.StaffService;
using static Client.Common.Crypts;
namespace Client.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        [Authorize(Roles = "Admin,HR,Trainer,NhanVien")]
        public ActionResult Index() => View();


        [Authorize(Roles = "Admin,HR")]
        public ActionResult Account()
        {
           return View();
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
                return SettingView();
            }
            return Index();

        }
        [HttpPost]
        public ActionResult Img()
        {
            AccountStaff accountStaff = Session["Account"] as AccountStaff;
           string imgpath= Request["ImageFile"];
            string fileName = Path.GetFileNameWithoutExtension(imgpath);
            string extension = Path.GetExtension(imgpath);
            fileName = fileName + DateTime.Now.ToString("yymmssffff") + extension;
            //Img img = new Img();
            //img.path_ ="~/Images/"+ fileName;
            //img.entryId = accountStaff.staff.id;
            accountStaff.imgs[0].path_ = "~/Images/" + fileName;
            var img =UpdateImg(accountStaff.imgs[0]);
            if (img != null)
            {
                accountStaff.imgs[0] = img;
                Session["Account"] = accountStaff;
                ViewBag.UpdateSuccess = 1;
                return Account();
            }
            return SettingView();
        }
    }

}
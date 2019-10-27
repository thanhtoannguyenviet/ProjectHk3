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
namespace Client.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: Admin
        [Authorize(Roles = "Admin,HR,Trainer,NhanVien")]
        public ActionResult Index() => View();


        [Authorize(Roles = "Admin,HR")]
        public ActionResult Account() => View();
        
        
        [Authorize(Roles = "Admin,HR")]
        public ActionResult NewAccount()
        {
            return View(new AccountStaff());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,HR")]
        public ActionResult NewAccount(AccountStaff accountStaff)
        {
            var PATHIMG = "~/Images/";
            var ENTRYNAME = "01.jpg";
            accountStaff.staff.status_ = 1;
            accountStaff.imgs[0].path_ = PATHIMG;
            accountStaff.imgs[0].entryName = ENTRYNAME;
            accountStaff.imgs[0].entryId = accountStaff.staff.id;
            accountStaff = RegisterStaff(accountStaff);
            return Redirect("/Admin/");
        }

        [Authorize(Roles = "Admin,HR,Trainer,NhanVien")]
        public ActionResult SettingView() => View();
        [HttpPost]
        [Authorize(Roles = "Admin,HR,Trainer,NhanVien")]
        public ActionResult SettingView(AccountStaff accountStaff)
        {
            accountStaff = UpdateStaff(accountStaff);
            if (accountStaff != null)
                return Redirect("/Admin/");
            return SettingView();

        }

    }

}
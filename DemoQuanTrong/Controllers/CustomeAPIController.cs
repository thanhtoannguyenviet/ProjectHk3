using DemoQuanTrong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DemoQuanTrong.Common;
using System.Web.Http.Description;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace DemoQuanTrong.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountAPIController : ApiController
    {
        private ExcellonEntities1 db = new ExcellonEntities1();
        [HttpGet]
        [Route("checkLogin/{username}/{password}")]
        public IHttpActionResult checkLogin(string userName, string password)
        {

            string query = CustomSQL.checkLogin(userName, password);
            using (var entities = new ExcellonEntities1())
            {
                Account account = entities.Accounts
                        .SqlQuery(query)
                        .ToList<Account>().DefaultIfEmpty(null).First();

                return Ok(account);
            }

        }
        [HttpPost]
        [Route("loginStaff/")]
        public IHttpActionResult loginStaff([FromBody] Account account)
        {

            string query = CustomSQL.checkRole(ConstantTable.STAFF, account.id + "");
            using (var entities = new ExcellonEntities1())
            {

                Staff staff = entities.Staffs
                   .SqlQuery(query)
                   .ToList<Staff>().DefaultIfEmpty(null).First();
                string queryImg = CustomSQL.getImg(ConstantTable.STAFF, account.id + "");
                var imgs = entities.Imgs
                        .SqlQuery(queryImg)
                        .ToList<Img>();
                AccountStaff accountStaff = new AccountStaff();
                accountStaff.account = account;
                accountStaff.staff = staff;
                if (imgs != null && imgs.Count > 0)
                {
                    foreach (Img item in imgs)
                        accountStaff.imgs.Add(item);
                }

                return Ok(accountStaff);
            }

        }

        [HttpPost]
        [Route("loginCustomer/")]
        public IHttpActionResult loginCustomer([FromBody] Account account)
        {

            string query = CustomSQL.checkRole(ConstantTable.CUSTOMER, account.id + "");
            using (var entities = new ExcellonEntities1())
            {

                Customer customer = entities.Customers
                   .SqlQuery(query)
                   .ToList<Customer>().DefaultIfEmpty(null).First();
                string queryImg = CustomSQL.getImg(ConstantTable.CUSTOMER, account.id + "");
                var img = entities.Imgs
                        .SqlQuery(queryImg)
                        .ToList<Img>().DefaultIfEmpty(null).First();
                AccountCustomer accountCustomer = new AccountCustomer();
                accountCustomer.account = account;
                accountCustomer.customer = customer;
                accountCustomer.img = img;
                return Ok(accountCustomer);
            }

        }

        [HttpGet]
        [Route("updateStatus")]
        public string updateStatus() //chạy để update status
        {
            using (var entity = new ExcellonEntities1())
            {
                var result = entity.updateStatus();
            }
            return "Success";
        }
    }
}

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
        private ExcellonEntities db = new ExcellonEntities();
        [HttpGet]
        [Route("checkLogin/{username}/{password}")]
        public IHttpActionResult checkLogin(string userName, string password)
        {

            string query = CustomSQL.checkLogin(userName, password);
            using (var entities = new ExcellonEntities())
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
            using (var entities = new ExcellonEntities())
            {

                Staff staff = entities.Staffs
                   .SqlQuery(query)
                   .ToList<Staff>().DefaultIfEmpty(null).First();
                AccountStaff accountStaff = new AccountStaff();
                string queryImg = CustomSQL.getImg(ConstantTable.STAFF, staff.id + "");
                var imgs = entities.Imgs.SqlQuery(queryImg).ToList<Img>();
                accountStaff.imgs.AddRange(imgs);
                accountStaff.staff = staff;
                string queryService = CustomSQL.getService(staff.id);
                var services = entities.Service_.SqlQuery(queryService).ToList<Service_>();
                accountStaff.services.AddRange(services);
                string queryDetail = CustomSQL.getDetailPendingStaff(staff.id + "");
                var details = entities.Details.SqlQuery(queryDetail).ToList<Detail>();
                accountStaff.details.AddRange(details);

                return Ok(accountStaff);
            }

        }

        [HttpPost]
        [Route("loginCustomer/")]
        public IHttpActionResult loginCustomer([FromBody] Account account)
        {

            string query = CustomSQL.checkRole(ConstantTable.CUSTOMER, account.id + "");
            using (var entities = new ExcellonEntities())
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

                List<DetailPayment> listDetailPayment = new List<DetailPayment>();
                string queryPayment = CustomSQL.getPaymentForCus(customer.id + "");
                var payments = entities.Payments.SqlQuery(queryPayment).ToList<Payment>();
                foreach (var item in payments)
                {
                    DetailPayment detailPayment = new DetailPayment();
                    detailPayment.payment = item;
                    string queryDetail = CustomSQL.getDetail(item.id + "");
                    var details = entities.Details.SqlQuery(queryDetail).ToList<Detail>();
                    detailPayment.details.AddRange(details);
                    listDetailPayment.Add(detailPayment);
                }
                return Ok(accountCustomer);
            }

        }

        [HttpGet]
        [Route("updateStatus")]
        public string updateStatus() //chạy để update status
        {
            using (var entity = new ExcellonEntities())
            {
                var result = entity.updateStatus();
            }
            return "Success";
        }

        [HttpGet]
        [Route("getDetailForStaff/{id}")]
        public IHttpActionResult updgetDetailForStaffateStatus(int id) //chạy để update status
        {
            using (var entity = new ExcellonEntities())
            {
                var result = entity.getCustomerForDetail(id).ToList();
                if (result.Count() > 0)
                {
                    return Ok(result);
                }
            }

            return null;
        }
    }
}

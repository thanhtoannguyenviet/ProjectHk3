using DemoQuanTrong.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DemoQuanTrong.Common;
using System.Web.Http.Description;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http.Results;
using System.Net.Http;

namespace DemoQuanTrong.Controllers
{
    [RoutePrefix("api/customer")]
    public class CustomerAPIController : ApiController
    {
        private ExcellonEntities1 db = new ExcellonEntities1();
        [HttpGet]
        [Route("getAllOrder/{id}/{page}")]
        public IHttpActionResult getAllOrder(string id, int page)
        {

            Filter filter = new Filter();
            filter.pageNumber = page;
            string query = CustomSQL.findForCustomer(ConstantTable.DETAIL, id, filter);
            using (var entities = new ExcellonEntities1())
            {
                var details = entities.Details
                        .SqlQuery(query)
                        .ToList<Detail>();
                if (details == null || details.Count == 0)
                {
                    return NotFound();
                }
                return Ok(details);
            }
        }

        [HttpGet]
        [Route("countAllOrder/{id}")]
        public IHttpActionResult countDetail(string id)
        {

            string query = CustomSQL.findForCustomer(ConstantTable.DETAIL, id, null, true);
            using (var entities = new ExcellonEntities1())
            {
                int countOrder = entities.Details
                        .SqlQuery(query).Count();
                return Ok(countOrder);
            }
        }

        [HttpGet]
        [Route("getPayment/{id}/{page}")]
        public IHttpActionResult getPayment(string id, int page)
        {

            Filter filter = new Filter();
            filter.pageNumber = page;
            string query = CustomSQL.findForCustomer(ConstantTable.PAYMENT, id, filter);
            using (var entities = new ExcellonEntities1())
            {
                var payments = entities.Payments
                        .SqlQuery(query)
                        .ToList<Payment>();
                if (payments == null || payments.Count == 0)
                {
                    return NotFound();
                }
                return Ok(payments);
            }
        }

        [HttpGet]
        [Route("countPayment/{id}")]
        public IHttpActionResult countPayment(string id)
        {

            string query = CustomSQL.findForCustomer(ConstantTable.PAYMENT, id, null, true);
            using (var entities = new ExcellonEntities1())
            {
                int countPayment = entities.Payments
                        .SqlQuery(query).Count();
                return Ok(countPayment);
            }
        }

        [HttpGet]
        [Route("getStaffCustomer/{page}")]
        public IHttpActionResult getStaff(int page)
        {
            Filter filter = new Filter();
            filter.pageNumber = page;
            string query = CustomSQL.getStaffForCustomer(filter);
            List<AccountStaff> staffListCus = new List<AccountStaff>();
            using (var entities = new ExcellonEntities1())
            {
                var staffList = entities.Staffs
                        .SqlQuery(query)
                        .ToList<Staff>();
                if (staffList == null || staffList.Count == 0)
                {
                    return NotFound();
                }

                foreach (var item in staffList)
                {
                    AccountStaff accountStaff = new AccountStaff();
                    string queryImg = CustomSQL.getImg(ConstantTable.STAFF, item.id + "");
                    var imgs = entities.Imgs.SqlQuery(queryImg).ToList<Img>();
                    accountStaff.imgs.AddRange(imgs);
                    accountStaff.staff = item;
                    string queryService = CustomSQL.getService(item.id);
                    var services = entities.Service_.SqlQuery(queryService).ToList<Service_>();
                    accountStaff.services.AddRange(services);
                    staffListCus.Add(accountStaff);
                }
                return Ok(staffListCus);
            }
        }

        [HttpGet]
        [Route("countStaffCustomer/")]
        public IHttpActionResult countStaff()
        {
            string query = CustomSQL.getStaffForCustomer(null, true);
            using (var entities = new ExcellonEntities1())
            {
                int count = entities.Staffs
                        .SqlQuery(query).Count();
                return Ok(count);
            }
        }

        [HttpGet]
        [Route("detailStaff/{id}")]
        public IHttpActionResult detailStaff(int id)
        {
            AccountStaff accountStaff = new AccountStaff();
            string query = CustomSQL.findStaff(null, id);
            using (var entities = new ExcellonEntities1())
            {
                var staff = entities.Staffs
                        .SqlQuery(query)
                        .ToList<Staff>().DefaultIfEmpty(null).First();
                if (staff == null)
                {
                    return NotFound();
                }
                string queryImg = CustomSQL.getImg(ConstantTable.STAFF, staff.id + "");
                var imgs = entities.Imgs.SqlQuery(queryImg).ToList<Img>();
                accountStaff.imgs.AddRange(imgs);
                accountStaff.staff = staff;
                string queryServices = CustomSQL.getService(staff.id);
                var services = entities.Service_.SqlQuery(queryServices).ToList<Service_>();
                accountStaff.services.AddRange(services);
                return Ok(accountStaff);
            }
        }

        [HttpPost]
        [Route("registerCustomer/")]
        public IHttpActionResult RegisterCustomer([FromBody] AccountCustomer accountCustomer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (accountCustomer.account != null && accountCustomer.customer != null)
            {
                try
                {
                    using (var entity = new ExcellonEntities1())
                    {
                        var result = entity.checkAccount(ConstantTable.CUSTOMER, accountCustomer.account.userName, accountCustomer.customer.headEmail);
                        if (result.Count() > 0)
                        {
                            accountCustomer.messsage = MessageError.EXIST;
                            return Ok(accountCustomer);
                        }
                    }

                    db.Accounts.Add(accountCustomer.account);
                    db.SaveChanges();
                    accountCustomer.customer.id = accountCustomer.account.id;
                    db.Customers.Add(accountCustomer.customer);
                    db.SaveChanges();
                    if (accountCustomer.img != null)
                    {
                        accountCustomer.img.entryId = accountCustomer.account.id;
                        accountCustomer.img.entryName = ConstantTable.CUSTOMER;
                        db.Imgs.Add(accountCustomer.img);
                        db.SaveChanges();
                    }


                }
                catch (Exception e)
                {
                    return Json(e.Message);
                }

            }
            else
            {
                throw new Exception();
            }
            return Ok(accountCustomer);
        }

        [HttpPut]
        [Route("updateCustomer/")]
        public IHttpActionResult UpdateStaff([FromBody] AccountCustomer accountCustomer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (accountCustomer.account != null && accountCustomer.customer != null)
                {
                    using (var entity = new ExcellonEntities1())
                    {
                        var result = entity.checkAccount(ConstantTable.CUSTOMER, accountCustomer.account.userName, accountCustomer.customer.headEmail).ToList();
                        if (result.Count() > 0)
                        {
                            accountCustomer.messsage = MessageError.EXIST;
                            return Ok(accountCustomer);
                        }
                    }

                    db.Entry(accountCustomer.account).State = EntityState.Modified;
                    db.SaveChanges();

                    accountCustomer.customer.id = accountCustomer.account.id;
                    db.Entry(accountCustomer.customer).State = EntityState.Modified;
                    db.SaveChanges();
                    if (accountCustomer.img != null)
                    {
                        accountCustomer.img.entryId = accountCustomer.account.id;
                        accountCustomer.img.entryName = ConstantTable.CUSTOMER;
                        db.Entry(accountCustomer.img).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }

            return Ok(accountCustomer);
        }

        //[HttpPost]
        //[Route("registerCustomer/")]
        //public IHttpActionResult RegisterCustomer([FromBody] AccountCustomer accountCustomer)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    if (accountCustomer.account != null && accountCustomer.customer != null)
        //    {
        //        try
        //        {
        //            db.Accounts.Add(accountCustomer.account);
        //            db.SaveChanges();
        //            accountCustomer.customer.id = accountCustomer.account.id;
        //            db.Customers.Add(accountCustomer.customer);
        //            db.SaveChanges();
        //            if (accountCustomer.img != null)
        //            {
        //                accountCustomer.img.entryId = accountCustomer.account.id;
        //                accountCustomer.img.entryName = ConstantTable.CUSTOMER;
        //                db.Imgs.Add(accountCustomer.img);
        //                db.SaveChanges();
        //            }


        //        }
        //        catch (Exception e)
        //        {
        //            return Json(e.Message);
        //        }

        //    }
        //    else
        //    {
        //        throw new Exception();
        //    }
        //    return Ok(accountCustomer);
        //}

    }
}
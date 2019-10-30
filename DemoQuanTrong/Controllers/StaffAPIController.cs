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
    [RoutePrefix("api/staff")]

    public class StaffAPIController : ApiController
    {
        private ExcellonEntities db = new ExcellonEntities();

        [HttpPost]
        [Route("registerStaff/")]
        public IHttpActionResult RegisterStaff([FromBody] AccountStaff accountStaff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (accountStaff.account != null && accountStaff.staff != null)
                {
                    using (var entity = new ExcellonEntities())
                    {
                        var result = entity.checkAccount(ConstantTable.STAFF, accountStaff.account.userName, accountStaff.staff.staffEmail, 0).ToList();
                        if (result.Count() > 0)
                        {
                            accountStaff.messsage = MessageError.EXIST;
                            return Ok(accountStaff);
                        }
                    }
                    db.Accounts.Add(accountStaff.account);
                    db.SaveChanges();

                    accountStaff.staff.id = accountStaff.account.id;
                    db.Staffs.Add(accountStaff.staff);
                    db.SaveChanges();
                    if (accountStaff.imgs != null && accountStaff.imgs.Count() > 0)
                    {
                        foreach (Img item in accountStaff.imgs)
                        {
                            item.entryName = ConstantTable.STAFF;
                            item.entryId = accountStaff.staff.id;
                            db.Imgs.Add(item);
                            db.SaveChanges();
                        }
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
            return Ok(accountStaff);
        }

        [HttpPut]
        [Route("updateStaff/")]
        public IHttpActionResult UpdateStaff([FromBody] AccountStaff accountStaff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (accountStaff.account != null && accountStaff.staff != null)
                {
                    using (var entity = new ExcellonEntities())
                    {
                        var result = entity.checkAccount(ConstantTable.STAFF, accountStaff.account.userName, accountStaff.staff.staffEmail, accountStaff.id).ToList();
                        if (result.Count() > 0)
                        {
                            accountStaff.messsage = MessageError.EXIST;
                            return Ok(accountStaff);
                        }
                    }
                    db.Entry(accountStaff.account).State = EntityState.Modified;
                    db.SaveChanges();
                    accountStaff.staff.id = accountStaff.account.id;
                    db.Entry(accountStaff.staff).State = EntityState.Modified;
                    db.SaveChanges();
                    //if (accountStaff.imgs != null && accountStaff.imgs.Count() > 0)
                    //{
                    //    foreach (var item in accountStaff.imgs)
                    //    {
                    //        item.entryName = ConstantTable.STAFF;
                    //        item.entryId = accountStaff.staff.id;
                    //        db.Imgs.Add(item);
                    //        db.SaveChanges();
                    //    }
                    //}

                }
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(accountStaff);
        }

        [HttpPut]
        [Route("updateImg/")]
        public IHttpActionResult updateImg([FromBody] Img img)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                db.Entry(img).State = EntityState.Modified;
                db.SaveChanges();

            }
            catch (Exception)
            {

                throw;
            }
            return Ok(img);
        }

        [HttpPut]
        [Route("addImg/")]
        public IHttpActionResult addImg([FromBody] AccountStaff accountStaff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (accountStaff.account != null && accountStaff.staff != null && accountStaff.imgs != null && accountStaff.imgs.Count() > 0)
                {
                    foreach (var item in accountStaff.imgs)
                    {
                        item.entryName = ConstantTable.STAFF;
                        item.entryId = accountStaff.staff.id;
                        db.Imgs.Add(item);
                        db.SaveChanges();
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }
            return Ok(accountStaff);
        }

        [HttpPost]
        [Route("registerService/")]
        public IHttpActionResult registerService([FromBody] AccountStaff accountStaff)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (accountStaff.account != null && accountStaff.staff != null && accountStaff.services != null && accountStaff.services.Count() > 0)
                {
                    foreach (var item in accountStaff.services)
                    {
                        db.Service_.Add(item);
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
            return Ok(accountStaff);
        }

        [HttpPut]
        [Route("updateService/")]
        public IHttpActionResult updateService([FromBody] Service_ service)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (service != null)
                {
                    db.Entry(service);
                    db.SaveChanges();

                }
            }
            catch (Exception)
            {

                throw;
            }
            return Ok(service);
        }

        [HttpGet]
        [Route("findWithRole/{role}")]
        public IHttpActionResult findWithRole(int role)
        {
            if (role < 20)
            {
                return BadRequest(ModelState);
            }
            else
            {
                try
                {
                    List<AccountStaff> listAccStaff = new List<AccountStaff>();
                    string query = CustomSQL.getStaffWithRole(role + "", null, false);
                    using (var entities = new ExcellonEntities())
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
                            listAccStaff.Add(accountStaff);
                        }
                        return Ok(listAccStaff);
                    }

                }
                catch (Exception e)
                {
                    return Json(e.Message);
                }

            }
        }

        [HttpGet]
        [Route("countWithRole/{role}")]
        public IHttpActionResult countWithRole(int role)
        {
            if (role < 20)
            {
                return BadRequest(ModelState);
            }
            else
            {
                try
                {
                    string query = CustomSQL.getStaffWithRole(role + "", null, true);
                    using (var entities = new ExcellonEntities())
                    {
                        var count = entities.Staffs
                                .SqlQuery(query)
                                .ToList<Staff>().Count;
                        return Ok(count);
                    }

                }
                catch (Exception e)
                {
                    return Json(e.Message);
                }

            }
        }

        [HttpGet]
        [Route("findWithKeyword/{role}/{keyword}")]
        public IHttpActionResult findWithKeyword(string role, string keyword)
        {
            if ("".Equals(keyword))
            {
                return BadRequest(ModelState);
            }
            else
            {
                try
                {
                    Filter filter = new Filter();
                    filter.keyword = keyword;
                    List<AccountStaff> listAccStaff = new List<AccountStaff>();
                    string query = CustomSQL.getStaffWithKeyword(role, filter, false);
                    using (var entities = new ExcellonEntities())
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
                            listAccStaff.Add(accountStaff);
                        }
                        return Ok(listAccStaff);
                    }

                }
                catch (Exception e)
                {
                    return Json(e.Message);
                }

            }
        }

        [HttpGet]
        [Route("countWithKeyword/{role}/{keyword}")]
        public IHttpActionResult countWithKeyword(string role, string keyword)
        {
            if ("".Equals(keyword))
            {
                return BadRequest(ModelState);
            }
            else
            {
                try
                {
                    Filter filter = new Filter();
                    filter.keyword = keyword;
                    List<AccountStaff> listAccStaff = new List<AccountStaff>();
                    string query = CustomSQL.getStaffWithKeyword(role, filter, false);
                    using (var entities = new ExcellonEntities())
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
                            listAccStaff.Add(accountStaff);
                        }
                        return Ok(listAccStaff);
                    }

                }
                catch (Exception e)
                {
                    return Json(e.Message);
                }

            }
        }


    }
}
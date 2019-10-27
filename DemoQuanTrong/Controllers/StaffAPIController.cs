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
        private ExcellonEntities1 db = new ExcellonEntities1();

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
                    using (var entity = new ExcellonEntities1())
                    {
                        var result = entity.checkAccount(ConstantTable.STAFF, accountStaff.account.userName, accountStaff.staff.staffEmail);
                        if (result != null)
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
                    using (var entity = new ExcellonEntities1())
                    {
                        var result = entity.checkAccount(ConstantTable.STAFF, accountStaff.account.userName, accountStaff.staff.staffEmail);
                        if (result != null)
                        {
                            accountStaff.messsage = MessageError.EXIST;
                            return Ok(accountStaff);
                        }
                    }
                    db.Entry(accountStaff.account);
                    db.SaveChanges();
                    accountStaff.staff.id = accountStaff.account.id;
                    db.Entry(accountStaff.staff);
                    db.SaveChanges();
                    if (accountStaff.imgs != null && accountStaff.imgs.Count() > 0)
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
                db.Imgs.Add(img);
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

                    db.Accounts.Add(accountStaff.account);
                    db.SaveChanges();
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

    }
}
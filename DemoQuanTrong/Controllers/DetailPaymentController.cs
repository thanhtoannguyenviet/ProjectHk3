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
    [RoutePrefix("api/payment")]
    public class DetailPaymentController : ApiController
    {
        private ExcellonEntities db = new ExcellonEntities();
        [HttpPost]
        [Route("createOrder/")]
        public IHttpActionResult createOrder([FromBody] DetailPayment detailPayment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (detailPayment.customer != null && detailPayment.payment != null && detailPayment.details != null && detailPayment.details.Count > 0)
            {
                try
                {
                    db.Payments.Add(detailPayment.payment);
                    db.SaveChanges();
                    foreach (var item in detailPayment.details)
                    {
                        item.paymentId = detailPayment.payment.id;
                        db.Details.Add(item);
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
            return Ok(detailPayment);
        }


        [HttpPut]
        [Route("updateDetail/")]
        public IHttpActionResult UpdateStaff([FromBody] Detail detail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (detail != null)
                {
                    db.Entry(detail).State = EntityState.Modified;
                    db.SaveChanges();

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

            return Ok(detail);
        }


    }
}
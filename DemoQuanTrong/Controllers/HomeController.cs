﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace DemoQuanTrong.Controllers
{
    public class HomeController : Controller
    {
        // GET api/values
        public ActionResult Index()
        {
            return View();
        }
    }
}

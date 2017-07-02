﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SamNetMvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Demo1()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Demo2()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
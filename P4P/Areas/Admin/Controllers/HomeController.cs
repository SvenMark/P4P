using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using P4P.Helpers;

namespace P4P.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            return View();
        }
    }
}
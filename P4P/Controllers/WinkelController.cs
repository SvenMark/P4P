using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace P4P.Controllers
{
    public class WinkelController : Controller
    {
        // GET: Winkel
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(string q)
        {
            return View();
        }

        public ActionResult Quickorder()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Quickorder(string q)
        {
            return View();
        }

        public ActionResult Artikelen()
        {
            return RedirectToAction("Quickorder");
        }

        public ActionResult Details(int id)
        {
            return View();
        }
    }
}
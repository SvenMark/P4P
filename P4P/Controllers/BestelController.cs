using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using P4P.Models;

namespace P4P.Controllers
{
    public class BestelController : Controller
    {
        // GET: Bestel
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Orderdetails()
        {
            return View();
        }

        public ActionResult Orderconfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Orderconfirmation(Bestelling bestelling)
        {
            //database shit hier

            return View();
        }

        //de rest van de stappen.
    }
}
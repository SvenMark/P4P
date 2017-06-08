using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace P4P.Areas.Admin.Controllers
{
    public class ArtikelController : Controller
    {
        // GET: Admin/Artikel
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        //edit, delete
    }
}
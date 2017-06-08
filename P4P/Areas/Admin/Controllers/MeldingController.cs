using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace P4P.Areas.Admin.Controllers
{
    public class MeldingController : Controller
    {
        // GET: Admin/Melding
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
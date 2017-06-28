using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using P4P.Models;

namespace P4P.Areas.Admin.Controllers
{
    public class MeldingController : Controller
    {
        // GET: Admin/Melding
        public ActionResult Index()
        {
            using (var ctx = new P4PContext())
            {
                var melding = ctx.Meldingen.ToList();
                return View(melding);
            }
        }

        public ActionResult New()
        {
                return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(Melding melding)
        {
            using (var ctx = new P4PContext())
            {
                ctx.Meldingen.Add(melding);
                ctx.SaveChanges();
                return RedirectToAction("Index", "Melding");
            }
        }

        //edit, delete
    }
}
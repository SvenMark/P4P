using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
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

        public ActionResult Edit(int id)
        {
            using (var ctx = new P4PContext())
            {
                var melding = ctx.Meldingen.Find(id);
                return View(melding);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Melding melding)
        {
            using (var ctx = new P4PContext())
            {
                var meldingInDb = ctx.Meldingen.Find(melding.Id);
                if (meldingInDb == null) return HttpNotFound();
                meldingInDb.Naam = melding.Naam;
                meldingInDb.Message = melding.Message;
                ctx.SaveChanges();
                return RedirectToAction("Edit", "Melding");
            }
        }

        public ActionResult Delete(int id)
        {
            using (var ctx = new P4PContext())
            {
                var melding = ctx.Meldingen.Find(id);
                if (melding == null) return HttpNotFound();
                ctx.Meldingen.Remove(melding);
                ctx.SaveChanges();
                return RedirectToAction("Index", "Melding");
            }
        }

        //edit, delete
    }
}
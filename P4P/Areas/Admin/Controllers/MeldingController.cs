using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using P4P.Helpers;
using P4P.Models;

namespace P4P.Areas.Admin.Controllers
{
    public class MeldingController : Controller
    {
        // GET: Admin/Melding
        public ActionResult Index()
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            if (!string.IsNullOrWhiteSpace(Request.QueryString["success"]))
            {
                ViewBag.Success = Request.QueryString["success"];
                ViewBag.Errormessage = Request.QueryString["errormessage"];
            }
            using (var ctx = new P4PContext())
            {
                var melding = ctx.Meldingen.ToList();
                return View(melding);
            }
        }

        public ActionResult New()
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(Melding melding)
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            using (var ctx = new P4PContext())
            {
                ctx.Meldingen.Add(melding);
                ctx.SaveChanges();
                return RedirectToAction("Index", "Melding", new { success="true" });
            }
        }

        public ActionResult Edit(int id)
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            if (!string.IsNullOrWhiteSpace(Request.QueryString["success"]))
            {
                ViewBag.Success = Request.QueryString["success"];
                ViewBag.Errormessage = Request.QueryString["errormessage"];
            }
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
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            using (var ctx = new P4PContext())
            {
                var meldingInDb = ctx.Meldingen.Find(melding.Id);
                if (meldingInDb == null) return HttpNotFound();
                meldingInDb.Naam = melding.Naam;
                meldingInDb.Message = melding.Message;
                ctx.SaveChanges();
                return RedirectToAction("Edit", "Melding", new { success="true"});
            }
        }

        public ActionResult Delete(int id)
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            using (var ctx = new P4PContext())
            {
                var melding = ctx.Meldingen.Find(id);
                if (melding == null) return HttpNotFound();
                ctx.Meldingen.Remove(melding);
                ctx.SaveChanges();
                return RedirectToAction("Index", "Melding", new { success="true" });
            }
        }
    }
}
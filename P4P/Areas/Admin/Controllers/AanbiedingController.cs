﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;
using P4P.Helpers;
using P4P.Models;

namespace P4P.Areas.Admin.Controllers
{
    public class AanbiedingController : Controller
    {
        // GET: Admin/Aanbieding
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
                var producten = ctx.Products.Include(c => c.Hoofdcategorie).Include(c => c.Subcategorie).ToList();

                return View(producten);
            }
        }

        public ActionResult New(int id)
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            using (var ctx = new P4PContext())
            {
                var producten = ctx.Products.Find(id);
                return View(producten);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult New(Product product, FormCollection collection)
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            using (var ctx = new P4PContext())
            {
                try
                {
                    var productInDb = ctx.Products.Find(product.Id);
                    if (productInDb == null) return HttpNotFound();
                    productInDb.Aanbiedingen = product.Aanbiedingen;
                    productInDb.Aanbiedingprijs = Convert.ToDouble(collection["Aanbiedingprijs"].Replace('.', ','));
                    ctx.SaveChanges();
                    return RedirectToAction("Index", "Aanbieding", new {success = "true"});
                }
                catch
                {
                    return RedirectToAction("Index", "Aanbieding", new { success="false", errormessage="Onverwachte fout" });
                }
            }
        }
    }
}
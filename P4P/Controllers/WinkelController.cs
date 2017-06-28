﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;
using P4P.Helpers;
using P4P.Models;
using P4P.ViewModel;
using System.Data.Entity;
using Microsoft.Ajax.Utilities;


namespace P4P.Controllers
{
    public class WinkelController : Controller
    {
        // GET: Winkel
        public ActionResult Index()
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int user_id = Convert.ToInt32(Session["Id"]);

            using (P4PContext ctx = new P4PContext())
            {
                var categorie = ctx.Hoofdcategories.ToList();
                var gebruiker = ctx.Gebruikers.Find(user_id);
                var meldingen = ctx.Meldingen.ToList();

                var getCategories = new IndexWinkel
                {
                    hoofdcategorie = categorie,
                    gebruiker = gebruiker,
                    meldingen = meldingen
                };

                return View(getCategories);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(FormCollection collection)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");

            string search = collection["search"];
            ViewBag.zoek = search;

            using (P4PContext ctx = new P4PContext())
            {
                var producten = ctx.Products.ToList().Where(c => c.Naam.Contains(search));
                return View(producten);
            }
        }

        public ActionResult Quickorder()
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Quickorder(FormCollection collection)
        {
            using (var ctx = new P4PContext())
            {
                string code = collection["search"];
                if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
                var product = ctx.Products.SingleOrDefault(c => c.Code == code);

                var viewModel = new NewWinkelmand
                {
                    product = product
                };
                return View(viewModel);
            }

        }

        public ActionResult Categorie(int id)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");

            using (var ctx = new P4PContext())
            {
                var producten = ctx.Products.Include(c => c.Hoofdcategorie).Include(c => c.Subcategorie).ToList().Where(c => c.Hoofdcategorie.Id == id && c.Subcategorie == null);
                var subcategories = ctx.Subcategories.Include(c => c.Hoofdcategorie).ToList().Where(c => c.Hoofdcategorie.Id == id);
                var hoofdCategorie = ctx.Hoofdcategories.Find(id);

                if (producten.Any())
                {
                    var viewProducts = new SubcategorieProducts
                    {
                        product = producten,
                        hoofdcategorie = hoofdCategorie
                    };

                    return View("ProductsInCategorie", viewProducts);
                }

                var viewSubcategories = new SubcategorieProducts
                {
                    subcategories = subcategories,
                    hoofdcategorie = hoofdCategorie
                };

                return View(viewSubcategories);
            }
        }

        public ActionResult SubCategorie(int id)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");

            using (var ctx = new P4PContext())
            {
                var subCategorie = ctx.Subcategories.Include(c => c.Hoofdcategorie).SingleOrDefault(c => c.Id == id);
                var hoofdCategorie = ctx.Hoofdcategories.SingleOrDefault(c => c.Id == subCategorie.Hoofdcategorie.Id);
                var producten = ctx.Products.Include(c => c.Subcategorie).Include(c => c.Hoofdcategorie).ToList().Where(c => c.Subcategorie != null && c.Subcategorie.Id == id);

                var viewModel = new SubcategorieProducts
                {
                    product = producten,
                    subcategorie = subCategorie,
                    hoofdcategorie = hoofdCategorie
                };

                return View(viewModel);
            }
        }

        public ActionResult Artikelpagina(int id)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int user_id = Convert.ToInt32(Session["Id"]);

            using (var ctx = new P4PContext())
            {
                var product = ctx.Products.Include(c => c.Hoofdcategorie).Include(c => c.Subcategorie).SingleOrDefault(c => c.Id == id);
                var favorietenlijsten = ctx.Favorietenlijsts.Include(c => c.Producten).Include(c => c.Gebruiker).ToList().Where(c => c.Gebruiker.Id == user_id && !c.Producten.Contains(product));

                if (product == null) return HttpNotFound();

                var viewModel = new NewWinkelmand
                {
                    favorietenlijsten = favorietenlijsten,
                    product = product
                };

                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Artikelpagina(Winkelwagen winkelwagen, Product product)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int user_id = Convert.ToInt32(Session["Id"]);

            try
            {
                using (var ctx = new P4PContext())
                {
                    var AddWinkelwagen = new Winkelwagen
                    {
                        Gebruiker_id = user_id,
                        Product_Id = product.Id,
                        Aantal = winkelwagen.Aantal
                    };

                    ctx.Winkelwagens.Add(AddWinkelwagen);
                    ctx.SaveChanges();

                    return RedirectToAction("Artikelpagina", new {product.Id});
                }
            }
            catch
            {
                return RedirectToAction("Artikelpagina", new {product.Id});
            }
        }
    }
}
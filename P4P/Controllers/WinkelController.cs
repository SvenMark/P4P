using System;
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

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Search(string search)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");

            return View();
        }

        public ActionResult Quickorder()
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Quickorder(string q)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");

            return View();
        }

        public ActionResult Categorie(int id)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");

            using (var ctx = new P4PContext())
            {
                var producten = ctx.Products.Include(c => c.Hoofdcategorie).ToList().Where(c => c.Hoofdcategorie.Id == id);
                var subcategories = ctx.Subcategories.Include(c => c.Hoofdcategorie).ToList().Where(c => c.Hoofdcategorie.Id == id);

                if (ctx.Products.Include(c => c.Hoofdcategorie).Any(c => c.Hoofdcategorie.Id == id))
                {
                    return View("ProductsInCategorie", producten);
                }

                return View("Categorie", subcategories);
            }
        }

        public ActionResult SubCategorie(int id)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");

            using (var ctx = new P4PContext())
            {
                var producten = ctx.Products.Include(c => c.Subcategorie).Include(c => c.Hoofdcategorie).ToList().Where(c => c.Subcategorie.Id == id);
                var subCategorie = ctx.Subcategories.Include(c => c.Hoofdcategorie).SingleOrDefault(c => c.Id == id);
                var hoofdCategorie = ctx.Hoofdcategories.SingleOrDefault(c => c.Id == subCategorie.Hoofdcategorie.Id);

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

            using (var ctx = new P4PContext())
            {
                var product = ctx.Products.Find(id);

                if (product == null) return HttpNotFound();

                var viewModel = new NewWinkelmand
                {
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
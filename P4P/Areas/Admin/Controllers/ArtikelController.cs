using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using P4P.Models;
using P4P.ViewModel;

namespace P4P.Areas.Admin.Controllers
{
    public class ArtikelController : Controller
    {
        // GET: Admin/Artikel
        public ActionResult Index()
        {
            using (var ctx = new P4PContext())
            {
                var producten = ctx.Products.Include(c => c.Hoofdcategorie).Include(c => c.Subcategorie).ToList();
                    
                return View(producten);
            } 
            
        }

        public ActionResult CreateArtikel()
        {
            using (var ctx = new P4PContext())
            {
                var hoofdCategorie = ctx.Hoofdcategories.ToList();
                var subCategorie = ctx.Subcategories.ToList();
                var viewModel = new NewArtikelViewModel
                {
                    Hoofdcategorie = hoofdCategorie,
                    Subcategorie = subCategorie
                };
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateArtikel(Product product, FormCollection collection)
        {
            using (var ctx = new P4PContext())
            {
                product.Prijs = Convert.ToDouble(collection["Product.Prijs"].Replace('.', ','));

                if (product.Hoofdcategorie.Id != 0)
                    product.Hoofdcategorie = ctx.Hoofdcategories.Find(product.Hoofdcategorie.Id);
                if (product.Subcategorie.Id != 0)
                {
                    product.Subcategorie = ctx.Subcategories.Find(product.Subcategorie.Id);
                }
                else
                {
                    product.Subcategorie.Hoofdcategorie = product.Hoofdcategorie;
                }
                ctx.Products.Add(product);
                ctx.SaveChanges();
                return RedirectToAction("Index", "Artikel");
            }
        }

        public ActionResult EditArtikel(int id)
        {
            using (var ctx = new P4PContext())
            {
                var product = ctx.Products.Find(id);
                if (product == null) return HttpNotFound();

                return View(product);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditArtikel(Product product, FormCollection collection)
        {
            using (var ctx = new P4PContext())
            {
                var productInDb = ctx.Products.Find(product.Id);
                if (productInDb == null) return HttpNotFound();

                productInDb.Naam = product.Naam;
                productInDb.Prijs = Convert.ToDouble(collection["Product.Prijs"].Replace('.', ','));
                productInDb.Verkoopeenheid = product.Verkoopeenheid;
                productInDb.Beschrijving = product.Beschrijving;
                productInDb.Code = product.Code;
                productInDb.Specificaties = product.Specificaties;

                if (product.Hoofdcategorie.Id != 0)
                {
                    productInDb.Hoofdcategorie = ctx.Hoofdcategories.Find(product.Hoofdcategorie.Id);
                }
                if (product.Subcategorie.Id != 0)
                {
                    product.Subcategorie = ctx.Subcategories.Find(product.Subcategorie.Id);
                }

                ctx.SaveChanges();
                return RedirectToAction("Index", "Categorie", new { success = "true" });
            }
        }

        //edit, delete
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using P4P.Helpers;
using P4P.Models;
using P4P.ViewModel;

namespace P4P.Areas.Admin.Controllers
{
    public class ArtikelController : Controller
    {

        // GET: Admin/Artikel
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
                try
                {
                    var producten = ctx.Products.Include(c => c.Hoofdcategorie).Include(c => c.Subcategorie).ToList();

                    return View(producten);
                }
                catch
                {
                    return RedirectToAction("Index", "Artikel", new { success = "false", errormessage = "Onverwachte fout" });
                }
            } 
            
        }

        public ActionResult CreateArtikel()
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            using (var ctx = new P4PContext())
            {
                try
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
                catch
                {
                    return RedirectToAction("Index", "Artikel", new { success = "false", errormessage = "Onverwachte fout" });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateArtikel(Product product, FormCollection collection)
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            using (var ctx = new P4PContext())
            {
                try
                {
                    product.Prijs = Convert.ToDouble(collection["Product.Prijs"].Replace('.', ','));
                    if (product.Hoofdcategorie != null)
                    {
                        if (product.Hoofdcategorie.Id != 0)
                            product.Hoofdcategorie = ctx.Hoofdcategories.Find(product.Hoofdcategorie.Id);
                    }
                    if (product.Subcategorie != null)
                    {
                        product.Subcategorie.Hoofdcategorie = product.Hoofdcategorie;
                        if (product.Subcategorie.Id != 0)
                            product.Subcategorie = ctx.Subcategories.Find(product.Subcategorie.Id);
                    }

                    ctx.Products.Add(product);
                    ctx.SaveChanges();
                    return RedirectToAction("SetSubcategorie", "Artikel", new {id = product.Id});
                }
                catch
                {
                    return RedirectToAction("Index", "Artikel", new { success = "false", errormessage = "Onverwachte fout" });
                }
            }
        }

        public ActionResult EditArtikel(int id)
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            using (var ctx = new P4PContext())
            {
                try
                {
                    var product = ctx.Products.Find(id);
                    if (product == null) return HttpNotFound();
                    var hoofdcategorieen = ctx.Hoofdcategories.ToList();
                    var viewModel = new NewArtikelViewModel
                    {
                        Product = product,
                        Hoofdcategorie = hoofdcategorieen
                    };

                    return View(viewModel);
                }
                catch
                {
                    return RedirectToAction("Index", "Artikel", new { success = "false", errormessage = "Onverwachte fout" });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditArtikel(Product product, FormCollection collection)
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            using (var ctx = new P4PContext())
            {
                try
                {
                    var productInDb = ctx.Products.Find(product.Id);
                    if (productInDb == null) return HttpNotFound();

                    productInDb.Naam = product.Naam;
                    productInDb.Prijs = Convert.ToDouble(collection["Product.Prijs"].Replace('.', ','));
                    productInDb.Verkoopeenheid = product.Verkoopeenheid;
                    productInDb.Beschrijving = product.Beschrijving;
                    productInDb.Code = product.Code;
                    productInDb.Specificaties = product.Specificaties;

                    if (product.Hoofdcategorie != null)
                    {
                        productInDb.Hoofdcategorie = ctx.Hoofdcategories.Find(product.Hoofdcategorie.Id);
                    }

                    ctx.SaveChanges();
                    return RedirectToAction("SetSubcategorie", "Artikel", new {id = product.Id});
                }
                catch
                {
                    return RedirectToAction("Index", "Artikel", new { success = "false", errormessage = "Onverwachte fout" });
                }
            }
        }

        public ActionResult SetSubcategorie(int id)
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            using (var ctx = new P4PContext())
            {
                try
                {
                    var product = ctx.Products.Find(id);
                    if (product == null) return HttpNotFound();
                    var subcategorieen = ctx.Subcategories.Include(c => c.Hoofdcategorie).ToList()
                        .Where(c => c.Hoofdcategorie == product.Hoofdcategorie);
                    var viewModel = new NewArtikelViewModel
                    {
                        Product = product,
                        Subcategorie = subcategorieen
                    };
                    return View(viewModel);
                }
                catch
                {
                    return RedirectToAction("Index", "Artikel", new { success = "false", errormessage = "Onverwachte fout" });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetSubcategorie(Product product)
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            using (var ctx = new P4PContext())
            {
                try
                {
                    var productInDb = ctx.Products.SingleOrDefault(c => c.Id == product.Id);
                    if (productInDb == null) return HttpNotFound();
                    var subcategorie = ctx.Subcategories.Find(product.Subcategorie.Id);
                    productInDb.Subcategorie = subcategorie;
                    ctx.SaveChanges();
                    return RedirectToAction("Index", "Artikel", new {success = "true"});
                }
                catch
                {
                    return RedirectToAction("Index", "Artikel", new { success = "false", errormessage="Onverwachte fout" });
                }
            }

        }

        public ActionResult Delete(int id)
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            using (var ctx = new P4PContext())
            {
                try
                {
                    var product = ctx.Products.Find(id);
                    if (product == null) return HttpNotFound();
                    product.Hoofdcategorie = null;
                    product.Subcategorie = null;
                    ctx.Products.Remove(product);
                    ctx.SaveChanges();
                    return RedirectToAction("Index", "Artikel", new {success = "true"});
                }
                catch
                {
                    return RedirectToAction("Index", "Artikel", new { success = "false", errormessage = "Onverwachte fout" });
                }
            }
        }
    }
}
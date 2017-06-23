using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using P4P.ViewModel;
using System.Data.Entity;
using P4P.Models;

namespace P4P.Areas.Admin.Controllers
{
    public class CategorieController : Controller
    {
        // GET: Admin/Categorie
        public ActionResult Index()
        {
            using (var ctx = new P4PContext())
            {
                var hoofdCategorie = ctx.Hoofdcategories.ToList();

                if (string.IsNullOrWhiteSpace(Request.QueryString["success"])) return View(hoofdCategorie);
                ViewBag.Success = Request.QueryString["success"];
                ViewBag.Errormessage = Request.QueryString["errormessage"];
                return View(hoofdCategorie);
            }
            
        }

        public ActionResult Details(int id)
        {
            using (var ctx = new P4PContext())
            {
                var subCategorie = ctx.Subcategories.Include(c => c.Hoofdcategorie).ToList().Where(c => c.Hoofdcategorie.Id == id);
                var hoofdCategorie = ctx.Hoofdcategories.Find(id);

                if (hoofdCategorie == null) return HttpNotFound();

                var viewModel = new DetailsSubcategorie()
                {
                    Subcategorie = subCategorie,
                    Hoofdcategorie = hoofdCategorie
                };
                if (string.IsNullOrWhiteSpace(Request.QueryString["success"])) return View(viewModel);
                ViewBag.Success = Request.QueryString["success"];
                ViewBag.Errormessage = Request.QueryString["errormessage"];
                return View(viewModel);
            }
        }

        public ActionResult CreateHoofdcategorie()
        {
            if (string.IsNullOrWhiteSpace(Request.QueryString["success"])) return View();
            ViewBag.Success = Request.QueryString["success"];
            ViewBag.Errormessage = Request.QueryString["errormessage"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateHoofdcategorie(Hoofdcategorie hoofdcategorie)
        {
            using (var ctx = new P4PContext())
            {
                ctx.Hoofdcategories.Add(hoofdcategorie);
                ctx.SaveChanges();
                return RedirectToAction("Index", new { success = "true"});
            }
        }

        public ActionResult EditHoofdcategorie(int id)
        {
            using (var ctx = new P4PContext())
            {
                var hoofdCategorie = ctx.Hoofdcategories.Find(id);
                if (hoofdCategorie == null) return HttpNotFound();

                return View(hoofdCategorie);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditHoofdCategorie(Hoofdcategorie hoofdcategorie)
        {
            using (var ctx = new P4PContext())
            {
                var hoofdCategorie = ctx.Hoofdcategories.Find(hoofdcategorie.Id);
                if (hoofdCategorie == null) return HttpNotFound();
                hoofdCategorie.Naam = hoofdcategorie.Naam;

                ctx.SaveChanges();
                return RedirectToAction("Index", "Categorie", new { success = "true" });
            }
        }

        public ActionResult DeleteHoofdCategorie(int id)
        {
            using (var ctx = new P4PContext())
            {
                var hoofdCategorie = ctx.Hoofdcategories.Find(id);

                if (hoofdCategorie == null) return HttpNotFound();
                if (ctx.Subcategories.Any(c => c.Hoofdcategorie.Id == id))
                {

                    var subCategorieen = ctx.Subcategories.Include(c => c.Hoofdcategorie).ToList().Where(c => c.Hoofdcategorie.Id == id);

                    foreach (var subCategorie in subCategorieen)
                    {
                        ctx.Subcategories.Remove(subCategorie);
                    }
                }

                ctx.Hoofdcategories.Remove(hoofdCategorie);
                ctx.SaveChanges();
                return RedirectToAction("Index", "Categorie", new { success = "true" });
            }
        }

        public ActionResult CreateSubCategorie(int id)
        {
            if (!string.IsNullOrWhiteSpace(Request.QueryString["success"]))
            {
                ViewBag.Success = Request.QueryString["success"];
                ViewBag.Errormessage = Request.QueryString["errormessage"];
            }

            using (var ctx = new P4PContext())
            {
                var hoofdCategorie = ctx.Hoofdcategories.Find(id);
                var viewModel = new NewSubcategorie()
                {
                    Hoofdcategorie = hoofdCategorie
                };
                return View(viewModel);
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSubCategorie(Subcategorie subcategorie, int id)
        {
            using (var ctx = new P4PContext())
            {
                var hoofdCategorie = ctx.Hoofdcategories.Find(id);
                subcategorie.Hoofdcategorie = hoofdCategorie;
                ctx.Subcategories.Add(subcategorie);
                ctx.SaveChanges();
            }
            return RedirectToAction("Details", new {id});
        }

        public ActionResult EditSubCategorie(int id)
        {
            using (var ctx = new P4PContext())
            {
                var subCategorie = ctx.Subcategories.Include(c => c.Hoofdcategorie).SingleOrDefault(c => c.Id == id);
                if (subCategorie == null) return HttpNotFound();

                return View(subCategorie);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSubCategorie(Subcategorie subcategorie)
        {
            using (var ctx = new P4PContext())
            {
                var subCategorie = ctx.Subcategories.Include(c => c.Hoofdcategorie).SingleOrDefault(c => c.Id == subcategorie.Id);
                if (subCategorie == null) return HttpNotFound();
                subCategorie.Naam = subcategorie.Naam;
                
                ctx.SaveChanges();
                return RedirectToAction("Details", "Categorie", new {id = subCategorie.Hoofdcategorie.Id, success = "true" });
            }
        }

        public ActionResult DeleteSubCategorie(int id)
        {
            using (var ctx = new P4PContext())
            {
                var subCategorie = ctx.Subcategories.Include(c => c.Hoofdcategorie).SingleOrDefault(c => c.Id == id);
                var hoofdCategorieId = subCategorie.Hoofdcategorie.Id;

                if (subCategorie == null) return HttpNotFound();
                ctx.Subcategories.Remove(subCategorie);
                ctx.SaveChanges();
                return RedirectToAction("Details", "Categorie", new { id = hoofdCategorieId, success = "true" });
            }
        }
    }
}
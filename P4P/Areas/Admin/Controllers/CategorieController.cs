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

                return View(hoofdCategorie);
            }
            
        }

        public ActionResult Details(int id)
        {
            using (var ctx = new P4PContext())
            {
                var subCategorie = ctx.Subcategories.Include(c => c.Hoofdcategorie).ToList().Where(c => c.Hoofdcategorie.Id == id);

                return View(subCategorie);
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
                return RedirectToAction("CreateHoofdcategorie", new { success = "true"});
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

        //edit, delete
    }
}
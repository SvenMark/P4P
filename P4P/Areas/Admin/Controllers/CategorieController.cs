﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using P4P.ViewModel;
using System.Data.Entity;
using P4P.Helpers;
using P4P.Models;

namespace P4P.Areas.Admin.Controllers
{
    public class CategorieController : Controller
    {
        // GET: Admin/Categorie
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
                    var hoofdCategorie = ctx.Hoofdcategories.ToList();

                    if (string.IsNullOrWhiteSpace(Request.QueryString["success"])) return View(hoofdCategorie);
                    ViewBag.Success = Request.QueryString["success"];
                    ViewBag.Errormessage = Request.QueryString["errormessage"];
                    return View(hoofdCategorie);
                }
                catch
                {
                    return RedirectToAction("Index", "Categorie", new {success = "false", errormessage = "Onbekende fout"});
                }
            }
            
        }

        public ActionResult Details(int id)
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            using (var ctx = new P4PContext())
            {
                try
                {
                    var subCategorie = ctx.Subcategories.Include(c => c.Hoofdcategorie).ToList()
                        .Where(c => c.Hoofdcategorie.Id == id);
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
                catch
                {
                    return RedirectToAction("Index", "Categorie", new { success = "false", errormessage = "Onbekende fout" });
                }
            }
        }

        public ActionResult CreateHoofdcategorie()
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            if (string.IsNullOrWhiteSpace(Request.QueryString["success"])) return View();
            ViewBag.Success = Request.QueryString["success"];
            ViewBag.Errormessage = Request.QueryString["errormessage"];
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateHoofdcategorie(Hoofdcategorie hoofdcategorie)
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            using (var ctx = new P4PContext())
            {
                try
                {
                    ctx.Hoofdcategories.Add(hoofdcategorie);
                    ctx.SaveChanges();
                    return RedirectToAction("Index", new {success = "true"});
                }
                catch
                {
                    return RedirectToAction("Index", "Categorie", new { success = "false", errormessage = "Onbekende fout" });
                }
            }
        }

        public ActionResult EditHoofdcategorie(int id)
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            using (var ctx = new P4PContext())
            {
                try
                {
                    var hoofdCategorie = ctx.Hoofdcategories.Find(id);
                    if (hoofdCategorie == null) return HttpNotFound();

                    return View(hoofdCategorie);
                }
                catch
                {
                    return RedirectToAction("Index", "Categorie", new { success = "false", errormessage = "Onbekende fout" });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditHoofdCategorie(Hoofdcategorie hoofdcategorie)
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            using (var ctx = new P4PContext())
            {
                try
                {
                    var hoofdCategorie = ctx.Hoofdcategories.Find(hoofdcategorie.Id);
                    if (hoofdCategorie == null) return HttpNotFound();
                    hoofdCategorie.Naam = hoofdcategorie.Naam;

                    ctx.SaveChanges();
                    return RedirectToAction("Index", "Categorie", new {success = "true"});
                }
                catch
                {
                    return RedirectToAction("Index", "Categorie", new { success = "false", errormessage = "Onbekende fout" });
                }
            }
        }

        public ActionResult DeleteHoofdCategorie(int id)
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
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

                try
                {
                    ctx.Hoofdcategories.Remove(hoofdCategorie);
                    ctx.SaveChanges();
                    return RedirectToAction("Index", "Categorie", new { success = "true" });
                }
                catch
                {
                    return RedirectToAction("Index", "Categorie", new { success = "false", errormessage="Er zijn nog producten met deze hoofdcategorie" });
                }


            }
        }

        public ActionResult CreateSubCategorie(int id)
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
                    var hoofdCategorie = ctx.Hoofdcategories.Find(id);
                    var viewModel = new NewSubcategorie()
                    {
                        Hoofdcategorie = hoofdCategorie
                    };
                    return View(viewModel);
                }
                catch
                {
                    return RedirectToAction("Index", "Categorie", new { success = "false", errormessage = "Onbekende fout" });
                }
            }
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSubCategorie(Subcategorie subcategorie, int id)
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            using (var ctx = new P4PContext())
            {
                try
                {
                    var hoofdCategorie = ctx.Hoofdcategories.Find(id);
                    subcategorie.Hoofdcategorie = hoofdCategorie;
                    ctx.Subcategories.Add(subcategorie);
                    ctx.SaveChanges();
                    return RedirectToAction("Details", new {id, success="true"});
                }
                catch
                {
                    return RedirectToAction("Index", "Categorie", new { success = "false", errormessage = "Onbekende fout" });
                }
            }
            
        }

        public ActionResult EditSubCategorie(int id)
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            using (var ctx = new P4PContext())
            {
                try
                {
                    var subCategorie = ctx.Subcategories.Include(c => c.Hoofdcategorie)
                        .SingleOrDefault(c => c.Id == id);
                    if (subCategorie == null) return HttpNotFound();

                    return View(subCategorie);
                }
                catch
                {
                    return RedirectToAction("Index", "Categorie", new { success = "false", errormessage = "Onbekende fout" });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSubCategorie(Subcategorie subcategorie)
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            using (var ctx = new P4PContext())
            {
                try
                {
                    var subCategorie = ctx.Subcategories.Include(c => c.Hoofdcategorie)
                        .SingleOrDefault(c => c.Id == subcategorie.Id);
                    if (subCategorie == null) return HttpNotFound();
                    subCategorie.Naam = subcategorie.Naam;

                    ctx.SaveChanges();
                    return RedirectToAction("Details", "Categorie",
                        new {id = subCategorie.Hoofdcategorie.Id, success = "true"});
                }
                catch
                {
                    return RedirectToAction("Index", "Categorie", new { success = "false", errormessage = "Onbekende fout" });
                }
            }
        }

        public ActionResult DeleteSubCategorie(int id)
        {
            if (Auth.getRole() != "Admin") return RedirectToAction("Index", "Profiel", new { area = "" });
            using (var ctx = new P4PContext())
            {
                var subCategorie = ctx.Subcategories.Include(c => c.Hoofdcategorie)
                    .SingleOrDefault(c => c.Id == id);
                if (subCategorie == null) return HttpNotFound();
                var hoofdCategorieId = subCategorie.Hoofdcategorie.Id;

                try
                {
                    ctx.Subcategories.Remove(subCategorie);
                    ctx.SaveChanges();
                    return RedirectToAction("Details", "Categorie", new {id = hoofdCategorieId, success = "true"});
                }
                catch
                {
                    return RedirectToAction("Details", "Categorie", new {id = hoofdCategorieId, success = "false", errormessage="Er zijn nog producten met deze subcategorie"});
                }
            }
        }
    }
}
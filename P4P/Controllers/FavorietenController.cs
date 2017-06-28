using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using P4P.Models;
using  P4P.Helpers;
using System.Data.Entity;
using System.Web.Management;

namespace P4P.Controllers
{
    public class FavorietenController : Controller
    {
        // GET: Favorieten
        public ActionResult Index()
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            if (!string.IsNullOrWhiteSpace(Request.QueryString["success"]))
            {
                ViewBag.Success = Request.QueryString["success"];
                ViewBag.Errormessage = Request.QueryString["errormessage"];
            }
            int user_id = Convert.ToInt32(Session["Id"]);

            using (P4PContext ctx = new P4PContext())
            {
                var favorietenlijsts = ctx.Favorietenlijsts.Include(c => c.Gebruiker).ToList().Where(c => c.Gebruiker.Id == user_id);

                return View(favorietenlijsts);
            }
        }

        public ActionResult Create()
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Favorietenlijst favorietenlijst)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int user_id = Convert.ToInt32(Session["Id"]);

            using (var ctx = new P4PContext())
            {
                try
                {
                    var gebruiker = ctx.Gebruikers.Find(user_id);
                    favorietenlijst.Gebruiker = gebruiker;

                    ctx.Favorietenlijsts.Add(favorietenlijst);
                    ctx.SaveChanges();
                    return RedirectToAction("Index", "Favorieten", new {success = "true"});
                }
                catch
                {
                    return RedirectToAction("Index", "Favorieten", new { success = "false", errormessage="Onbekende fout" });
                }
            }
        }

        [HttpPost]
        public ActionResult Toevoegen(FormCollection collection)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int user_id = Convert.ToInt32(Session["Id"]);
            int prod_id = Convert.ToInt32(collection["prod_id"]);

            using (var ctx = new P4PContext())
            {
                try
                {
                    var favorietenlijst = ctx.Favorietenlijsts.Find(Convert.ToInt32(collection["fav_id"]));
                    var product = ctx.Products.Find(prod_id);

                    if (favorietenlijst != null && product != null)
                    {
                        favorietenlijst.Producten.Add(product);
                        ctx.SaveChanges();
                    }
                    return RedirectToAction("Artikelpagina", "Winkel", new {id = prod_id, success="true"});
                }

                catch
                {
                    return RedirectToAction("Artikelpagina", "Winkel", new { success = "false", errormessage = "Onbekende fout" });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Nieuw(FormCollection collection, Favorietenlijst favorietenlijst)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int user_id = Convert.ToInt32(Session["Id"]);
            int prod_id = Convert.ToInt32(collection["prod_id"]);

            using (var ctx = new P4PContext())
            {
                try
                {
                    var product = ctx.Products.Find(prod_id);
                    var gebruiker = ctx.Gebruikers.Find(user_id);

                    if (favorietenlijst.Naam != null)
                    {
                        favorietenlijst.Gebruiker = gebruiker;
                    }

                    if (product != null)
                    {
                        favorietenlijst.Producten.Add(product);
                        ctx.Favorietenlijsts.Add(favorietenlijst);
                        ctx.SaveChanges();
                    }
                    return RedirectToAction("Artikelpagina", "Winkel", new {id = prod_id, success="true"});
                }
                catch
                {
                    return RedirectToAction("Artikelpagina", "Winkel", new {success = "false", errormessage = "Onbekende fout"});
                }
            }
            
        }

        public ActionResult Details(int id)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            using (P4PContext ctx = new P4PContext())
            {
                var favorietenlijst = ctx.Favorietenlijsts.Include(c => c.Producten).SingleOrDefault(c => c.Id == id);

                if (favorietenlijst == null)
                    return HttpNotFound();

                return View(favorietenlijst);
            }
        }

        public ActionResult Delete(int id)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int user_id = Convert.ToInt32(Session["Id"]);

            using (var ctx = new P4PContext())
            {
                try
                {
                    var favorietenlijst = ctx.Favorietenlijsts.Find(id);
                    if (favorietenlijst == null) return HttpNotFound();
                    ctx.Favorietenlijsts.Remove(favorietenlijst);
                    ctx.SaveChanges();
                    return RedirectToAction("Index", "Favorieten", new { success="true"});
                }
                catch
                {
                    return RedirectToAction("Index", "Favorieten", new { success = "false", errormessage="Onbekende fout" });
                }
            }
        }

        public ActionResult RemoveProd(int id, int id2)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int user_id = Convert.ToInt32(Session["Id"]);

            using (var ctx = new P4PContext())
            {
                var favorietenlijst = ctx.Favorietenlijsts.Find(id2);
                var product = ctx.Products.Find(id);
                if(product == null) return HttpNotFound();
                if (favorietenlijst == null) return HttpNotFound();
                favorietenlijst.Producten.Remove(product);
                ctx.SaveChanges();
                return RedirectToAction("Details", new {id = id2});
            }
        }
    }
}
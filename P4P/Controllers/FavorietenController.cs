using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using P4P.Models;
using  P4P.Helpers;

namespace P4P.Controllers
{
    public class FavorietenController : Controller
    {
        // GET: Favorieten
        public ActionResult Index()
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            return View();
        }

        [HttpPost]
        public ActionResult Toevoegen(FormCollection collection)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int user_id = Convert.ToInt32(Session["Id"]);
            int prod_id = Convert.ToInt32(collection["prod_id"]);

            using (var ctx = new P4PContext())
            {

                var favorietenlijst = ctx.Favorietenlijsts.Find(Convert.ToInt32(collection["fav_id"]));
                var product = ctx.Products.Find(prod_id);

                if (favorietenlijst != null && product != null)
                {
                    favorietenlijst.Producten.Add(product);
                    ctx.Favorietenlijsts.Add(favorietenlijst);
                    ctx.SaveChanges();
                }
            }
            return RedirectToAction("Artikelpagina", "Winkel", new { id = prod_id});
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
                var product = ctx.Products.Find(prod_id);
                var gebruiker = ctx.Gebruikers.Find(user_id);

                if (favorietenlijst.Naam != null)
                {
                    favorietenlijst.Gebruiker = gebruiker;

                    ctx.Favorietenlijsts.Add(favorietenlijst);
                    ctx.SaveChanges();
                }

                if (product != null)
                {
                    favorietenlijst.Producten.Add(product);
                    ctx.Favorietenlijsts.Add(favorietenlijst);
                    ctx.SaveChanges();
                }
            }
            return RedirectToAction("Artikelpagina", "Winkel", new { id = prod_id });
        }

        public ActionResult Details(int id)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            using (P4PContext ctx = new P4PContext())
            {
                var favorietenlijst = ctx.Favorietenlijsts.SingleOrDefault(m => m.Id == id);

                if (favorietenlijst == null)
                    return HttpNotFound();

                return View(favorietenlijst);
            }
        }
    }
}
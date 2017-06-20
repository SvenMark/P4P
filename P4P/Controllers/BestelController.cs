using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;
using P4P.Helpers;
using P4P.Models;

namespace P4P.Controllers
{
    public class BestelController : Controller
    {
        // GET: Bestel
        public ActionResult Index()
        {
            if (Session["Id"] == null) return RedirectToAction("Login", "Profiel");

            using (P4PContext ctx = new P4PContext())
            {
                var winkelmand = ctx.Winkelwagens.Include(c => c.Product).ToList();
                return View(winkelmand);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Winkelwagen winkelwagen)
        {
            if (Auth.IsAuth()) return RedirectToAction("Login", "Profiel");

            try
            {
                using (var ctx = new P4PContext())
                {
                    double totaalPrijs = 0;
                    foreach (var item in ctx.Winkelwagens.Include(c => c.Product).ToList())
                    {
                        totaalPrijs += (item.Aantal * item.Product.Prijs);
                    }

                    int Id = Convert.ToInt32(Session["Id"]);
                    var gebruikerInDb = ctx.Gebruikers.Include(c => c.Bedrijf).SingleOrDefault(c => c.Id == Id);

                    Bestelling bestelling = new Bestelling
                    {
                        Prijs = totaalPrijs,
                        Gebruiker = gebruikerInDb,
                        Bedrijf = gebruikerInDb.Bedrijf,
                        Afgerond = false
                    };

                    foreach (var item in ctx.Winkelwagens.Include(c => c.Product).ToList())
                    {
                        BestellingProduct bestellingProduct = new BestellingProduct
                        {
                            Bestelling_Id = bestelling.Id,
                            Aantal = item.Aantal,
                            Product_Id = item.Product_Id
                        };
                        ctx.BestellingProducts.Add(bestellingProduct);
                    }

                    ctx.Bestellingen.Add(bestelling);
                    ctx.SaveChanges();

                    return RedirectToAction("Orderdetails");
                }
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Orderdetails()
        {
            if (Session["Id"] == null) return RedirectToAction("Login", "Profiel");
            int Id = Convert.ToInt32(Session["Id"]);

            using (P4PContext ctx = new P4PContext())
            {
                var bestelling = ctx.BestellingProducts.Include(c => c.Bestelling.Gebruiker).Include(c => c.Product).Include(c => c.Bestelling).ToList().Where(c => c.Bestelling.Gebruiker.Id == Id);
                return View(bestelling);
            }
        }

        public ActionResult Orderconfirmation()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Orderconfirmation(Bestelling bestelling)
        {
            //database shit hier

            //de actie waar je heen wilt redirecten RedirectToAction("Action", "Controller");
            return RedirectToAction("idk", "Bestel");
        }

        public ActionResult Afgerond()
        {
            using (var ctx = new P4PContext())
            {
                List<Bestelling> bestellingen = ctx.Bestellingen.Where(x => x.Afgerond).ToList();
                return View(bestellingen);
            }
        }
        //de rest van de stappen.
    }
}
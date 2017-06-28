using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;
using System.Runtime.InteropServices.ComTypes;
using P4P.Helpers;
using P4P.Models;
using P4P.ViewModel;

namespace P4P.Controllers
{
    public class BestelController : Controller
    {
        // GET: Bestel
        public ActionResult Index()
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int user_id = Convert.ToInt32(Session["Id"]);

            using (P4PContext ctx = new P4PContext())
            {
                var winkelmand = ctx.Winkelwagens.Include(c => c.Product).Include(c => c.Gebruiker).ToList().Where(c => c.Gebruiker_id == user_id);
                return View(winkelmand);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Winkelwagen winkelwagen)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int user_id = Convert.ToInt32(Session["Id"]);

            try
            {
                using (var ctx = new P4PContext())
                {
                    double totaalPrijs = 0;
                    foreach (var item in ctx.Winkelwagens.Include(c => c.Product).Include(c => c.Gebruiker).ToList().Where(c => c.Gebruiker.Id == user_id))
                    {
                        totaalPrijs += (item.Aantal * item.Product.Prijs);
                    }

                    var gebruikerInDb = ctx.Gebruikers.Include(c => c.Bedrijf).SingleOrDefault(c => c.Id == user_id);
                    if (gebruikerInDb == null) return HttpNotFound();

                    Bestelling bestelling = new Bestelling
                    {
                        Prijs = totaalPrijs,
                        Gebruiker = gebruikerInDb,
                        Bedrijf = gebruikerInDb.Bedrijf,
                        Afgerond = false
                    };

                    ctx.Bestellingen.Add(bestelling);
                    ctx.SaveChanges();

                    foreach (var item in ctx.Winkelwagens.Include(c => c.Product).Include(c => c.Gebruiker).ToList().Where(c => c.Gebruiker.Id == user_id))
                    {
                        BestellingProduct bestellingProduct = new BestellingProduct
                        {
                            Bestelling_Id = bestelling.Id,
                            Aantal = item.Aantal,
                            Product_Id = item.Product_Id
                        };
                        if(bestellingProduct.Product_Id != null) ctx.BestellingProducts.Add(bestellingProduct);
                    }

                    ctx.SaveChanges();

                    return RedirectToAction("Orderdetails", new { id=bestelling.Id});
                }
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(int id)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int user_id = Convert.ToInt32(Session["Id"]);

            using (P4PContext ctx = new P4PContext())
            {
                var product = ctx.Winkelwagens.Include(c => c.Gebruiker).Include(c => c.Product).SingleOrDefault(c => c.Product_Id == id && c.Gebruiker_id == user_id);
                if (product != null)
                {
                    ctx.Winkelwagens.Remove(product);
                    ctx.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        public ActionResult Orderdetails(int id)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int user_id = Convert.ToInt32(Session["Id"]);

            using (P4PContext ctx = new P4PContext())
            {
                var gebruiker = ctx.Gebruikers.SingleOrDefault(c => c.Id == user_id);
                var bedrijf = ctx.Bedrijven.SingleOrDefault(c => c.Id == gebruiker.BedrijfId);
                var bestelling = ctx.Bestellingen.Include(c => c.Gebruiker).Include(c => c.Bedrijf).FirstOrDefault(x => x.Gebruiker.Id == user_id && x.Id == id);
                var producten = ctx.BestellingProducts.Include(c => c.Bestelling.Gebruiker).Include(c => c.Product).Include(c => c.Bestelling).ToList().Where(c => c.Bestelling.Gebruiker.Id == user_id && c.Bestelling_Id == id);
                var viewModel = new NewOrderDetails()
                {
                    BestellingProducts = producten,
                    Bedrijf = bedrijf,
                    Gebruiker = gebruiker,
                    Bestelling = bestelling
                };
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Orderdetails(Bestelling bestelling, Bedrijf bedrijf)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int user_Id = Convert.ToInt32(Session["Id"]);

            try
            {
                using (var ctx = new P4PContext())
                {
                    var getBestelling = ctx.Bestellingen.Include(c => c.Gebruiker).Include(c => c.Bedrijf).FirstOrDefault(x => x.Gebruiker.Id == user_Id && x.Id == bestelling.Id);

                    if (getBestelling == null) return HttpNotFound();

                    getBestelling.Opmerking = bestelling.Opmerking;
                    getBestelling.AfleverAdres = bedrijf.Adres;
                    getBestelling.Afleverdatum = bestelling.Afleverdatum;
                    getBestelling.AfleverPlaats = bedrijf.Plaats;
                    getBestelling.AfleverPostcode = bedrijf.Postcode;

                    ctx.SaveChanges();

                    return RedirectToAction("Orderconfirmation", new { id = bestelling.Id });
                }
            }
            catch
            {
                return RedirectToAction("Orderdetails");
            }
        }

        public ActionResult Orderconfirmation(int id)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int user_id = Convert.ToInt32(Session["Id"]);

            using (P4PContext ctx = new P4PContext())
            {
                var gebruiker = ctx.Gebruikers.SingleOrDefault(c => c.Id == user_id);
                var bedrijf = ctx.Bedrijven.SingleOrDefault(c => c.Id == gebruiker.BedrijfId);
                var bestelling = ctx.Bestellingen.Include(c => c.Gebruiker).Include(c => c.Bedrijf).FirstOrDefault(x => x.Gebruiker.Id == user_id && x.Id == id);
                var producten = ctx.BestellingProducts.Include(c => c.Bestelling.Gebruiker).Include(c => c.Product).Include(c => c.Bestelling).ToList().Where(c => c.Bestelling.Gebruiker.Id == user_id && c.Bestelling_Id == id);
                var viewModel = new NewOrderDetails()
                {
                    BestellingProducts = producten,
                    Bedrijf = bedrijf,
                    Gebruiker = gebruiker,
                    Bestelling = bestelling
                };
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Orderconfirmation(Bestelling bestelling)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int user_id = Convert.ToInt32(Session["Id"]);

            try
            {
                using (var ctx = new P4PContext())
                {
                    var getBestelling = ctx.Bestellingen.Include(c => c.Gebruiker).Include(c => c.Bedrijf).FirstOrDefault(x => x.Gebruiker.Id == user_id && x.Id == bestelling.Id);
                    if (getBestelling == null) return HttpNotFound();

                    getBestelling.Afgerond = true;
                    var producten = ctx.Winkelwagens.Include(c => c.Product).Include(c => c.Gebruiker).ToList().Where(c => c.Gebruiker.Id == user_id);
                    foreach (var x in producten)
                    {
                        ctx.Winkelwagens.Remove(x);
                    }

                    ctx.SaveChanges();

                    return RedirectToAction("Afgerond", "Bestel", new {id = bestelling.Id});
                }
            }
            catch
            {
                return RedirectToAction("Orderdetails");
            }
        }

        public ActionResult AfgerondBestellingen()
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int user_id = Convert.ToInt32(Session["Id"]);

            using (P4PContext ctx = new P4PContext())
            {
                var bestellingen = ctx.Bestellingen.Include(c => c.Gebruiker).ToList().Where(c => c.Gebruiker.Id == user_id && c.Afgerond);

                return View(bestellingen);
            }
        }

        public ActionResult Afgerond(int id)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int user_id = Convert.ToInt32(Session["Id"]);

            using (var ctx = new P4PContext())
            {
                var bestelling = ctx.Bestellingen.Include(c => c.Gebruiker.Bedrijf).Include(c => c.Gebruiker).SingleOrDefault(c => c.Gebruiker.Id == user_id && c.Id == id);
                var producten = ctx.BestellingProducts.Include(c => c.Bestelling.Gebruiker).Include(c => c.Product).Include(c => c.Bestelling).ToList().Where(c => c.Bestelling.Gebruiker.Id == user_id && c.Bestelling_Id == id);
                var viewModel = new NewOrderDetails()
                {
                    BestellingProducts = producten,
                    Bestelling = bestelling,
                    Gebruiker = bestelling.Gebruiker,
                    Bedrijf = bestelling.Gebruiker.Bedrijf
                };
                if (bestelling.Afgerond)
                {
                    return View(viewModel);
                }
                return RedirectToAction("Index", "Winkel");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Afgerond(Bestelling bestelling)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int user_id = Convert.ToInt32(Session["Id"]);
            int bestelling_id = bestelling.Id;

            try
            {
                using (var ctx = new P4PContext())
                {
                    var bestellingInDb = ctx.Bestellingen.Include(c => c.Gebruiker).SingleOrDefault(c => c.Id == bestelling_id);
                    if (bestellingInDb.Gebruiker.Id == user_id)
                    {
                        var producten = ctx.BestellingProducts.Include(c => c.Bestelling).ToList().Where(c => c.Bestelling.Id == bestelling_id);
                        foreach (var p in producten)
                        {
                            var winkelwagen = new Winkelwagen
                            {
                                Gebruiker_id = user_id,
                                Product_Id = p.Product_Id,
                                Aantal = p.Aantal
                            };
                            ctx.Winkelwagens.Add(winkelwagen);
                        }

                        ctx.SaveChanges();
                    }

                    return RedirectToAction("Index", "Bestel");
                }
            }
            catch
            {
                return RedirectToAction("Afgerond", new {bestelling_id});
            }
        }
    }
}
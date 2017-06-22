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
                    foreach (var item in ctx.Winkelwagens.Include(c => c.Product).ToList())
                    {
                        totaalPrijs += (item.Aantal * item.Product.Prijs);
                    }

                    var gebruikerInDb = ctx.Gebruikers.Include(c => c.Bedrijf).SingleOrDefault(c => c.Id == user_id);

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

                    return RedirectToAction("Orderdetails", new { id=bestelling.Id});
                }
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Orderdetails()
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int User_id = Convert.ToInt32(Session["Id"]);

            using (P4PContext ctx = new P4PContext())
            {
                var gebruiker = ctx.Gebruikers.SingleOrDefault(c => c.Id == User_id);
                var bedrijf = ctx.Bedrijven.SingleOrDefault(c => c.Id == gebruiker.BedrijfId);
                var bestelling = ctx.BestellingProducts.Include(c => c.Bestelling.Gebruiker).Include(c => c.Product).Include(c => c.Bestelling).ToList().Where(c => c.Bestelling.Gebruiker.Id == User_id);
                var viewModel = new NewOrderDetails()
                {
                    BestellingProducts = bestelling,
                    Bedrijf = bedrijf,
                    Gebruiker = gebruiker
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
                    var getBestelling = ctx.Bestellingen.Include(c => c.Gebruiker).Include(c => c.Bedrijf).FirstOrDefault(x => x.Gebruiker.Id == user_Id);

                    getBestelling.Opmerking = bestelling.Opmerking;
                    getBestelling.AfleverAdres = bedrijf.Adres;
                    getBestelling.Afleverdatum = bestelling.Afleverdatum;
                    getBestelling.AfleverPlaats = bedrijf.Plaats;
                    getBestelling.AfleverPostcode = bedrijf.Postcode;

                    ctx.SaveChanges();

                    return RedirectToAction("Orderconfirmation");
                }
            }
            catch
            {
                return RedirectToAction("Orderdetails");
            }
        }

        public ActionResult Orderconfirmation()
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int user_id = Convert.ToInt32(Session["Id"]);

            using (P4PContext ctx = new P4PContext())
            {
                var gebruiker = ctx.Gebruikers.SingleOrDefault(c => c.Id == user_id);
                var bedrijf = ctx.Bedrijven.SingleOrDefault(c => c.Id == gebruiker.BedrijfId);
                var bestelling = ctx.Bestellingen.Include(c => c.Gebruiker).Include(c => c.Bedrijf).FirstOrDefault(x => x.Gebruiker.Id == user_id);
                var producten = ctx.BestellingProducts.Include(c => c.Bestelling.Gebruiker).Include(c => c.Product).Include(c => c.Bestelling).ToList().Where(c => c.Bestelling.Gebruiker.Id == user_id);
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
                    var getBestelling = ctx.Bestellingen.Include(c => c.Gebruiker).Include(c => c.Bedrijf).FirstOrDefault(x => x.Gebruiker.Id == user_id);

                    getBestelling.Afgerond = true;
                    var producten = ctx.Winkelwagens.Include(c => c.Product).Include(c => c.Gebruiker).ToList().Where(c => c.Gebruiker.Id == user_id);
                    foreach (var x in producten)
                    {
                        ctx.Winkelwagens.Remove(x);
                    }

                    ctx.SaveChanges();

                    return RedirectToAction("Afgerond", "Bestel");
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
                var bestelling = ctx.Bestellingen.Include(c => c.Gebruiker).SingleOrDefault(c => c.Gebruiker.Id == user_id && c.Id == id);
                var producten = ctx.BestellingProducts.Include(c => c.Bestelling.Gebruiker).Include(c => c.Product).Include(c => c.Bestelling).ToList().Where(c => c.Bestelling.Gebruiker.Id == user_id);
                var viewModel = new NewOrderDetails()
                {
                    BestellingProducts = producten,
                    Bestelling = bestelling
                };
                return View(viewModel);
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
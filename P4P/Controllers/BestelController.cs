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
            if (!string.IsNullOrWhiteSpace(Request.QueryString["success"]))
            {
                ViewBag.Success = Request.QueryString["success"];
                ViewBag.Errormessage = Request.QueryString["errormessage"];
            }
            int user_id = Convert.ToInt32(Session["Id"]);

            using (P4PContext ctx = new P4PContext())
            {
                var winkelmand = ctx.Winkelwagens.Include(c => c.Product).Include(c => c.Gebruiker).ToList().Where(c => c.Gebruiker_id == user_id);
                return View(winkelmand);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection collection, string submit)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int user_id = Convert.ToInt32(Session["Id"]);
            switch (submit)
            {
                case "submit":


                    try
                    {
                        using (var ctx = new P4PContext())
                        {
                            double totaalPrijs = 0;
                            foreach (var item in ctx.Winkelwagens.Include(c => c.Product).Include(c => c.Gebruiker).ToList()
                                .Where(c => c.Gebruiker.Id == user_id))
                            {
                                totaalPrijs += (item.Aantal * item.Product.Prijs);
                            }

                            var gebruikerInDb = ctx.Gebruikers.Include(c => c.Bedrijf)
                                .SingleOrDefault(c => c.Id == user_id);
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

                            int i = 0;
                            var aantallen = collection["aantal[]"].Split(',');
                            foreach (var item in ctx.Winkelwagens.Include(c => c.Product).Include(c => c.Gebruiker).ToList()
                                .Where(c => c.Gebruiker.Id == user_id))
                            {
                                BestellingProduct bestellingProduct = new BestellingProduct
                                {
                                    Bestelling_Id = bestelling.Id,
                                    Aantal = item.Aantal,
                                    Product_Id = item.Product_Id
                                };
                                //winkelwagenupdate
                                item.Aantal = Convert.ToInt32(aantallen[i]);
                                //tabelupdate
                                bestellingProduct.Aantal = Convert.ToInt32(aantallen[i]);
                                i++;
                                if (bestellingProduct.Product_Id != 0) ctx.BestellingProducts.Add(bestellingProduct);

                            }

                            ctx.SaveChanges();

                            return RedirectToAction("Orderdetails", new {id = bestelling.Id, success="true"});
                        }
                    }
                    catch
                    {
                        return RedirectToAction("Index", "Bestel", new { success="false", errormessage="Onbekende fout"});
                    }
                case "update":
                    using (var ctx = new P4PContext())
                    {
                        var aantallen = collection["aantal[]"].Split(',');
                        int i = 0;
                        foreach (var item in ctx.Winkelwagens.Include(c => c.Product).Include(c => c.Gebruiker).ToList()
                            .Where(c => c.Gebruiker.Id == user_id))
                        {
                            item.Aantal = Convert.ToInt32(aantallen[i]);
                            i++;
                        }
                        ctx.SaveChanges();
                        return RedirectToAction("Index", "Bestel", new { success="true"});
                    }
                default:
                    return RedirectToAction("Index", "Bestel", new { success = "false", errormessage = "Onbekende fout" });
            }
        }

        public ActionResult Delete(int id)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            if (!string.IsNullOrWhiteSpace(Request.QueryString["success"]))
            {
                ViewBag.Success = Request.QueryString["success"];
                ViewBag.Errormessage = Request.QueryString["errormessage"];
            }
            int user_id = Convert.ToInt32(Session["Id"]);

            using (var ctx = new P4PContext())
            {
                var product = ctx.Winkelwagens.Include(c => c.Gebruiker).Include(c => c.Product).SingleOrDefault(c => c.Gebruiker.Id == user_id && c.Product.Id == id);
                if (product == null) return HttpNotFound();
                ctx.Winkelwagens.Remove(product);
                ctx.SaveChanges();
                return RedirectToAction("Index", "Bestel", new { success = "true" });
            }
        }

        public ActionResult Orderdetails(int id)
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

                    return RedirectToAction("Orderconfirmation", new { id = bestelling.Id, succes="true" });
                }
            }
            catch
            {
                return RedirectToAction("Orderdetails", "Bestel", new { success="false", errormessage="Onbekende fout"});
            }
        }

        public ActionResult Orderconfirmation(int id)
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

                    return RedirectToAction("Afgerond", "Bestel", new {id = bestelling.Id, success="true"});
                }
            }
            catch
            {
                return RedirectToAction("Orderdetails", "Bestel", new { success="false", errormessage="Onbekende fout"});
            }
        }

        public ActionResult AfgerondBestellingen()
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
                var bestellingen = ctx.Bestellingen.Include(c => c.Gebruiker).ToList().Where(c => c.Gebruiker.Id == user_id && c.Afgerond);

                return View(bestellingen);
            }
        }

        public ActionResult Afgerond(int id)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            if (!string.IsNullOrWhiteSpace(Request.QueryString["success"]))
            {
                ViewBag.Success = Request.QueryString["success"];
                ViewBag.Errormessage = Request.QueryString["errormessage"];
            }
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

                    return RedirectToAction("Index", "Bestel", new { success="true"});
                }
            }
            catch
            {
                return RedirectToAction("Afgerond", new {bestelling_id});
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BestelFavoriet(Favorietenlijst favorietenlijst)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int user_id = Convert.ToInt32(Session["Id"]);
            int favorietenlijst_id = favorietenlijst.Id;

            try
            {
                using (var ctx = new P4PContext())
                {
                    var favorietenlijstInDb = ctx.Favorietenlijsts.Include(c => c.Gebruiker).Include(c => c.Producten).SingleOrDefault(c => c.Id == favorietenlijst_id);
                    if (favorietenlijstInDb.Gebruiker.Id == user_id)
                    {
                        var producten = favorietenlijstInDb.Producten.ToList();
                        foreach (var p in producten)
                        {
                            var winkelwagen = new Winkelwagen
                            {
                                Gebruiker_id = user_id,
                                Product_Id = p.Id,
                                Aantal = 1
                            };
                            ctx.Winkelwagens.Add(winkelwagen);
                        }

                        ctx.SaveChanges();
                    }

                    return RedirectToAction("Index", "Bestel", new { success="true"});
                }
            }
            catch
            {
                return RedirectToAction("Details", "Favorieten", new { id=favorietenlijst_id });
            }
        }
    }
}
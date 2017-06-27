using System;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web.Mvc;
using P4P.Helpers;
using P4P.Models;
using P4P.ViewModel;
using System.Data.Entity;
using WebGrease.Css.Ast.Selectors;

namespace P4P.Controllers
{
    public class ProfielController : Controller
    {
        // GET: Profiel
        public ActionResult Index()
        {
            if (Session["Id"] == null) return RedirectToAction("Login");
            if (!string.IsNullOrWhiteSpace(Request.QueryString["success"]))
            {
                ViewBag.Success = Request.QueryString["success"];
                ViewBag.Errormessage = Request.QueryString["errormessage"];
            }

            using (P4PContext ctx = new P4PContext())
            {
                var gebruiker = ctx.Gebruikers.Find(Session["Id"]);
                return View(gebruiker);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Gebruiker gebruiker)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login");

            try
            {
                using (var ctx = new P4PContext())
                {
                    var gebruikerInDb = ctx.Gebruikers.Find(Session["Id"]);

                    if (gebruikerInDb == null) return HttpNotFound();

                    if (!Auth.VerifyHash(gebruiker.Wachtwoord, gebruikerInDb.Wachtwoord))
                        return RedirectToAction("Index", "Profiel", new { success="false", errormessage="Incorrect wachtwoord"});

                    gebruikerInDb.Voornaam = gebruiker.Voornaam;
                    gebruikerInDb.Achternaam = gebruiker.Achternaam;
                    gebruikerInDb.Telefoonnummer = gebruiker.Telefoonnummer;

                    ctx.SaveChanges();
                    return RedirectToAction("Index", "Profiel", new { success="true"});
                }
            }
            catch
            {
                return RedirectToAction("Index", "Profiel", new { success="false", errormessage="Onbekende fout"});
            }
        }

        public ActionResult Reset(Gebruiker gebruiker)
        {
            using (var ctx = new P4PContext())
            {
                var gebruikerInDb = ctx.Gebruikers.SingleOrDefault(c => c.Emailadres == gebruiker.Emailadres);

                if (gebruikerInDb == null) return HttpNotFound();

                gebruikerInDb.Wachtwoord = null;
                gebruikerInDb.Token = Auth.Getlogintoken();

                ctx.SaveChanges();

                GMailer.GmailUsername = "webshopjansma@gmail.com";
                GMailer.GmailPassword = "Lemmesmash";

                GMailer mailer = new GMailer();
                mailer.ToEmail = gebruiker.Emailadres;
                mailer.Subject = "ResetLink";
                mailer.Body =
                    "Hierbij de resetlink voor uw account<br> Reset uw wachtwoord met behulp van deze link: http://localhost:60565/Profiel/Login/" +
                    gebruikerInDb.Token;
                mailer.IsHtml = true;
                mailer.Send();
                return RedirectToAction("Login", "Profiel", new {success = "true"});
            }
        }

        public ActionResult Login(string token)
        {
            if (!string.IsNullOrWhiteSpace(Request.QueryString["success"]))
            {
                ViewBag.Success = Request.QueryString["success"];
                ViewBag.Errormessage = Request.QueryString["errormessage"];
            }
            if (token == null) return View();

            using (P4PContext ctx = new P4PContext())
            {
                try
                {
                    if (ctx.Gebruikers.Any(m => m.Token == token))
                    {
                        var gebruiker = ctx.Gebruikers.Single(m => m.Token == token);
                        Session["Id"] = gebruiker.Id;
                        ctx.SaveChanges();
                        ViewBag.Error = "";
                        return RedirectToAction("Gegevenscontrole");
                    }
                        ViewBag.Error = "Token is not valid";
                }
                catch
                {
                    ViewBag.Error = "An unexpected error occured, please try again later";
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Gebruiker gebruiker)
        {
            if (Session["Id"] != null) return RedirectToAction("Index");
            try
            {

                using (var ctx = new P4PContext())
                {
                    var gebruikerInDb = ctx.Gebruikers.SingleOrDefault(m => m.Emailadres == gebruiker.Emailadres);

                    if (gebruikerInDb == null)
                        return RedirectToAction("Login", new { success="false", errormessage="Deze combinatie is niet bij ons bekend!"});

                    if (!Auth.VerifyHash(gebruiker.Wachtwoord, gebruikerInDb.Wachtwoord))
                        return RedirectToAction("Login", new { success = "false", errormessage = "Deze combinatie is niet bij ons bekend!" });

                    Session["Id"] = gebruikerInDb.Id;
                    return RedirectToAction("Index", "Winkel");
                }
            }
            catch
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Gegevenscontrole()
        {
            if (Session["Id"] == null) return RedirectToAction("Login");

            using (P4PContext ctx = new P4PContext())
            {
                var gebruiker = ctx.Gebruikers.Find(Session["Id"]);
                return View(gebruiker);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Gegevenscontrole(Gebruiker gebruiker)
        {
            try
            {
                using (P4PContext ctx = new P4PContext())
                {
                    var gebruikerInDb = ctx.Gebruikers.Find(Session["Id"]);

                    if (gebruikerInDb == null) return HttpNotFound();

                    gebruikerInDb.Voornaam = gebruiker.Voornaam;
                    gebruikerInDb.Achternaam = gebruiker.Achternaam;

                    ctx.SaveChanges();

                    return RedirectToAction("Setpassword");
                }
            }
            catch
            {
                return RedirectToAction("Gegevenscontrole");
            }
        }

        public ActionResult Setpassword()
        {
            if (Session["Id"] == null) return RedirectToAction("Login");

            using (P4PContext ctx = new P4PContext())
            {
                var gebruiker = ctx.Gebruikers.Find(Session["Id"]);
                return View(gebruiker);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Setpassword(Gebruiker gebruiker)
        {
            try
            {
                using (P4PContext ctx = new P4PContext())
                {
                    var gebruikerInDb = ctx.Gebruikers.Find(Session["Id"]);

                    if (gebruikerInDb == null) return HttpNotFound();

                    gebruikerInDb.Wachtwoord = Auth.Hash(gebruiker.Wachtwoord);
                    gebruikerInDb.Token = "";

                    ctx.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Wachtwoord()
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login");
            if (!string.IsNullOrWhiteSpace(Request.QueryString["success"]))
            {
                ViewBag.Success = Request.QueryString["success"];
                ViewBag.Errormessage = Request.QueryString["errormessage"];
            }

            using (P4PContext ctx = new P4PContext())
            {
                var gebruiker = ctx.Gebruikers.Find(Session["Id"]);
                return View(gebruiker);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Wachtwoord(Gebruiker gebruiker)
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", new { success="false", errormessage="Uw sessie is verlopen"});

            try
            {
                using (var ctx = new P4PContext())
                {
                    var gebruikerInDb = ctx.Gebruikers.Find(Session["Id"]);

                    if (gebruikerInDb == null) return HttpNotFound();

                    if (!Auth.VerifyHash(gebruiker.Token, gebruikerInDb.Wachtwoord))
                        return RedirectToAction("Wachtwoord", new { success = "false", errormessage = "Foutief wachtwoord" });

                    if (gebruiker.Wachtwoord != gebruiker.ConfirmPassword) return RedirectToAction("Wachtwoord", new { success = "false", errormessage = "De wachtwoorden komen niet overeen" });

                    gebruikerInDb.Wachtwoord = Auth.Hash(gebruiker.Wachtwoord);
                    ctx.SaveChanges();
                    return RedirectToAction("Index", new { success = "true" });
                }
            }
            catch
            {
                return RedirectToAction("Wachtwoord", new { success = "false", errormessage = "Onbekende fout" });
            }
        }

        public ActionResult Bedrijf()
        {
            if (!Auth.IsAuth())
                return RedirectToAction("Login", new {success = "false", errormessage = "Uw sessie is verlopen"});
            if (Auth.getRole() == "Werknemer") return View("Bedrijf_read_only");
            if (!string.IsNullOrWhiteSpace(Request.QueryString["success"]))
            {
                ViewBag.Success = Request.QueryString["success"];
                ViewBag.Errormessage = Request.QueryString["errormessage"];
            }
            int user_id = Convert.ToInt32(Session["Id"]);
            using (var ctx = new P4PContext())
            {
                var gebruiker = ctx.Gebruikers.Include(c => c.Bedrijf).SingleOrDefault(c => c.Id == user_id);
                if (gebruiker == null) return HttpNotFound();
                var bedrijf = ctx.Bedrijven.Find(gebruiker.Bedrijf.Id);
                var viewModel = new BedrijfFormViewModel
                {
                    Bedrijf = bedrijf
                };
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Bedrijf(Bedrijf bedrijf)
        {
            using (var ctx = new P4PContext())
            {
                var bedrijfInDb = ctx.Bedrijven.Find(bedrijf.Id);
                if (bedrijfInDb == null) return HttpNotFound();
                bedrijfInDb.Naam = bedrijf.Naam;
                bedrijfInDb.Adres = bedrijf.Adres;
                bedrijfInDb.Postcode = bedrijf.Postcode;
                bedrijfInDb.Plaats = bedrijf.Plaats;
                bedrijfInDb.Telefoonnummer = bedrijf.Telefoonnummer;

                ctx.SaveChanges();
                return RedirectToAction("Bedrijf", "Profiel");
            }
        }

        public ActionResult CreateWerknemer()
        {
            if (!Auth.IsAuth())
                return RedirectToAction("Login", new { success = "false", errormessage = "Uw sessie is verlopen" });
            if (Auth.getRole() == "Werknemer") RedirectToAction("Bedrijf", "Profiel");
            if (!string.IsNullOrWhiteSpace(Request.QueryString["success"]))
            {
                ViewBag.Success = Request.QueryString["success"];
                ViewBag.Errormessage = Request.QueryString["errormessage"];
            }
            int user_id = Convert.ToInt32(Session["Id"]);
            using (var ctx = new P4PContext())
            {
                var gebruiker = ctx.Gebruikers.Include(c => c.Bedrijf).SingleOrDefault(c => c.Id == user_id);
                if (gebruiker == null) return HttpNotFound();
                var bedrijf = ctx.Bedrijven.Find(gebruiker.Bedrijf.Id);
                var viewModel = new BedrijfFormViewModel
                {
                    Bedrijf = bedrijf
                };
                return View(viewModel);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateWerknemer(Gebruiker gebruiker, Bedrijf bedrijf)
        {
            using (var ctx = new P4PContext())
            {
                //als de code hier ergens een error oplevert voert hij catch uit.
                if (ctx.Gebruikers.Any(m => m.Emailadres == gebruiker.Emailadres))
                    return RedirectToAction("Bedrijf", new {success = "false", errormessage = "Email already exists"});

                gebruiker.Token = Auth.Getlogintoken();
                gebruiker.BedrijfId = bedrijf.Id;
                gebruiker.Rol = "Werknemer";
                ctx.Gebruikers.Add(gebruiker);
                ctx.SaveChanges();

                GMailer.GmailUsername = "webshopjansma@gmail.com";
                GMailer.GmailPassword = "Lemmesmash";

                GMailer mailer = new GMailer();
                mailer.ToEmail = gebruiker.Emailadres;
                mailer.Subject = "Loginlink";
                mailer.Body =
                    "Hierbij de inloggegevens voor uw account<br> Login met behulp van deze link: http://localhost:60565/Profiel/Login/" +
                    gebruiker.Token;
                mailer.IsHtml = true;
                mailer.Send();
                return RedirectToAction("Bedrijf", new {success = "true"});
            }
        }

        public ActionResult Contactpersonen()
        {
            return View();
        }
    }
}
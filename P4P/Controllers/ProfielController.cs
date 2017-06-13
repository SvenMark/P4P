using System.Linq;
using System.Web.Mvc;
using P4P.Helpers;
using P4P.Models;
using WebGrease.Css.Ast.Selectors;

namespace P4P.Controllers
{
    public class ProfielController : Controller
    {
        // GET: Profiel
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login(string token)
        {
            if (string.IsNullOrWhiteSpace(Request.QueryString["success"])) return View();
            ViewBag.Success = Request.QueryString["success"];
            ViewBag.Errormessage = Request.QueryString["Errormessage"];
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
                    else
                    {
                        ViewBag.Error = "Token is not valid";
                    }
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

                    if (gebruikerInDb == null) return RedirectToAction("Login");

                    if (!Auth.VerifyHash(gebruiker.Wachtwoord, gebruikerInDb.Wachtwoord))
                        return RedirectToAction("Login");

                    Session["Id"] = gebruikerInDb.Id;
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return RedirectToAction("Index");
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

                    return RedirectToAction("Gegevens");
                }
            }
            catch
            {
                return RedirectToAction("Gegevens");
            }
        }

        public ActionResult Gegevens()
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
        public ActionResult Gegevens(Gebruiker gebruiker)
        {
            if(Auth.IsAuth()) return RedirectToAction("Login");

            try
            {
                using (var ctx = new P4PContext())
                {
                    var gebruikerInDb = ctx.Gebruikers.Find(Session["Id"]);

                    if (gebruikerInDb == null) return HttpNotFound();

                    if (!Auth.VerifyHash(gebruiker.Wachtwoord, gebruikerInDb.Wachtwoord))
                        return RedirectToAction("Gegevens");

                    gebruikerInDb.Voornaam = gebruiker.Voornaam;
                    gebruikerInDb.Achternaam = gebruiker.Achternaam;
                    gebruikerInDb.Telefoonnummer = gebruiker.Telefoonnummer;

                    ctx.SaveChanges();
                    return RedirectToAction("Gegevens");
                }
            }
            catch
            {
                return RedirectToAction("Gegevens");
            }
        }

        public ActionResult Wachtwoord()
        {
            if (Auth.IsAuth()) return RedirectToAction("Login");

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
            if (Auth.IsAuth()) return RedirectToAction("Login");

            try
            {
                using (var ctx = new P4PContext())
                {
                    var gebruikerInDb = ctx.Gebruikers.Find(Session["Id"]);

                    if (!Auth.VerifyHash(gebruiker.Token, gebruikerInDb.Wachtwoord))
                        return RedirectToAction("Wachtwoord");

                    if (gebruiker.Wachtwoord != gebruiker.ConfirmPassword) return RedirectToAction("Wachtwoord");

                    gebruikerInDb.Wachtwoord = Auth.Hash(gebruiker.Wachtwoord);
                    ctx.SaveChanges();
                    return RedirectToAction("Gegevens");
                }
            }
            catch
            {
                return RedirectToAction("Wachtwoord");
            }
        }

        public ActionResult Bedrijf()
        {
            return View();
        }

        public ActionResult Contactpersonen()
        {
            return View();
        }
    }
}
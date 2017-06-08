using System.Linq;
using System.Web.Mvc;
using P4P.Helpers;
using P4P.Models;

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
            if (token != null)
            {
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
                            return RedirectToAction("Gegevens");
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
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Gebruiker gebruiker)
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Gegevens()
        {
            if (Session["Id"] == null)
                return RedirectToAction("Login");
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
            try
            {
                using (P4PContext ctx = new P4PContext())
                {
                    var gebruikerInDb = ctx.Gebruikers.Find(Session["Id"]);

                    gebruikerInDb.Voornaam = gebruiker.Voornaam;
                    gebruikerInDb.Achternaam = gebruiker.Achternaam;
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
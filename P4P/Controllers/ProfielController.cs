using System.Web.Mvc;

namespace P4P.Controllers
{
    public class ProfielController : Controller
    {
        // GET: Profiel
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Gegevens()
        {
            using (P4PContext ctx = new P4PContext())
            {
                var gebruiker = ctx.Gebruikers.Find(25);
//                var gebruiker = ctx.Gebruikers.Find(Session["id"]);
                return View(gebruiker);
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
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
            //if (Session["Id"] == null) return RedirectToAction("Login", "Profiel");

            using (P4PContext ctx = new P4PContext())
            {
                var winkelmand = ctx.Winkelwagens.Include(c => c.Product).ToList();
                return View(winkelmand);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Bestelling bestelling)
        {
            //if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");

            try
            {
                using (var ctx = new P4PContext())
                {
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
            return View();
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

        public ActionResult Details(int id)
        {
            using (P4PContext ctx = new P4PContext())
            {
                var bestellijst = ctx.Bestellingen.SingleOrDefault(m => m.Id == id);

                if (bestellijst == null)
                    return HttpNotFound();

                return View(bestellijst);
            }
        }

        //de rest van de stappen.
    }
}
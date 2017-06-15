using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using P4P.Models;

namespace P4P.Controllers
{
    public class BestelController : Controller
    {
        // GET: Bestel
        public ActionResult Index()
        {
            return View();
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
                //hiersomekekcode
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
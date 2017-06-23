using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using P4P.Models;
using P4P.Helpers;

namespace P4P.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            if (!Auth.IsAuth()) return RedirectToAction("Login", "Profiel");
            int user_id = Convert.ToInt32(Session["Id"]);

            using (P4PContext ctx = new P4PContext())
            {
                var categorie = ctx.Hoofdcategories.ToList();
                return View(categorie);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Get in Touch!";

            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Contact(Contact contact)
        {
            using (var ctx = new P4PContext())
            {
                //als de code hier ergens een error oplevert voert hij catch uit.
                try
                {
                    ctx.Contacten.Add(contact);
                    ctx.SaveChanges();
                    ViewBag.Message = "Your message has been received!";
                }
                catch
                {
                    ViewBag.Message = "Something went wrong!";
                }
            }

            return View();
        }
    }
}
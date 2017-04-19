using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using P4P.Models;

namespace P4P.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
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
                    ViewBag.Succes = true;

                    return RedirectToAction("Index");
                }
                catch
                {
                    ViewBag.Succes = false;
                }
            }

            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using P4P.Models;

namespace P4P.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        // GET: Admin/Admin
        public ActionResult Index()
        {
            ViewBag.Registratie = null;
            if (Request.QueryString["reg_success"] == "True")
                ViewBag.Registratie = "True";
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public bool Register(Gebruiker gebruiker)
        {
            using (var ctx = new P4PContext())
            {
                //als de code hier ergens een error oplevert voert hij catch uit.
                try
                {
                    if (gebruiker.Wachtwoord == null)
                        gebruiker.LoginToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                    ctx.Gebruikers.Add(gebruiker);
                    ctx.SaveChanges();

                    //TODO: Send email
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
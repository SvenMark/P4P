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
using P4P.ViewModel;

namespace P4P.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        // GET: Admin/Admin
        public ActionResult Index()
        {
            using (var ctx = new P4PContext())
            {
                var gebruikers = ctx.Gebruikers.Include(c => c.Bedrijf).ToList();
                return View(gebruikers);
            }
            
        }

        public ActionResult Create()
        {
            if (string.IsNullOrWhiteSpace(Request.QueryString["success"])) return View();
            ViewBag.Success = Request.QueryString["success"];
            ViewBag.Errormessage = Request.QueryString["errormessage"];
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Gebruiker gebruiker, Bedrijf bedrijf)
        {
            using (var ctx = new P4PContext())
            {
                //als de code hier ergens een error oplevert voert hij catch uit.
//                try
//                {
                    if (ctx.Gebruikers.Any(m => m.Emailadres == gebruiker.Emailadres))
                        return RedirectToAction("Create", new { success="false", errormessage="Email already exists"});

                    gebruiker.Token = Auth.Getlogintoken();
                    ctx.Bedrijven.Add(bedrijf);
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
                    return RedirectToAction("Create", new { success="true"});
//                }
//                catch
//                {
//                    return RedirectToAction("Create", new { success="false", errormessage="Unexpected error"});
//                }
            }
        }

        //edit(rechten), delete gebruiker
    }
}
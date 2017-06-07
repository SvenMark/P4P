﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using P4P.Helpers;
using P4P.Models;

namespace P4P.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        // GET: Admin/Admin
        public ActionResult Index()
        {
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
                    //admin registratie
//                    gebruiker.Wachtwoord = BCrypt.Net.BCrypt.HashPassword("Lemmesmash", 13);

                    //normale registratie(bij admin creatie dit uitcommenten)
                    gebruiker.LoginToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());

                    ctx.Gebruikers.Add(gebruiker);
                    ctx.SaveChanges();

                    GMailer.GmailUsername = "webshopjansma@gmail.com";
                    GMailer.GmailPassword = "Lemmesmash";

                    GMailer mailer = new GMailer();
                    mailer.ToEmail = gebruiker.Emailadres;
                    mailer.Subject = "Loginlink";
                    mailer.Body =
                        "Hierbij de inloggegevens voor uw account<br> Login met behulp van deze link: <br> <a href=http://localhost:60565/account/login?token=" +
                        gebruiker.LoginToken + ">Verify</a>";
                    mailer.IsHtml = true;
                    mailer.Send();
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
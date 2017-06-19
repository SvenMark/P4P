using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.Mvc;

namespace P4P.Areas.Admin.Controllers
{
    public class ArtikelController : Controller
    {
        // GET: Admin/Artikel
        public ActionResult Index()
        {
            using (var ctx = new P4PContext())
            {
                var Producten = ctx.Products.ToList();
                    
                return View(Producten);
            } 
            
        }

        public ActionResult New()
        {
            return View();
        }

        //edit, delete
    }
}
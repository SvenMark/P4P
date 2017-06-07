using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace P4P.Controllers
{
    public class FavorietenController : Controller
    {
        // GET: Favorieten
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            using (P4PContext ctx = new P4PContext())
            {
                var favorietenlijst = ctx.Favorietenlijsts.SingleOrDefault(m => m.Id == id);

                if (favorietenlijst == null)
                    return HttpNotFound();

                return View(favorietenlijst);
            }
        }
        //TODO: New, Edit, Delete, Save(controller voor opslaan van voorgaande acties), Add_product, removeproduct, editproduct?
    }
}
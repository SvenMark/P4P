using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace P4P
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "WinkelQuery",
                "Winkel/{action}/{q}",
                new {controller = "Winkel", action = "Index", q = UrlParameter.Optional},
                new[] {"P4P.Controllers"}
            );

            routes.MapRoute(
                "ProfielToken",
                "Profiel/{action}/{token}",
                new { controller = "Profiel", action = "Index", token = UrlParameter.Optional },
                new[] { "P4P.Controllers" }
            );

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new {controller = "Home", action = "Index", id = UrlParameter.Optional},
                new[] {"P4P.Controllers"}
            );

            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MicroSocialPlatform
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AcceptRequestGroups", 
                url: "RequestGroup/Accept/{id}/{user}", 
                defaults: new { controller = "RequestGroup", action = "Accept" });

            routes.MapRoute(
                name: "RefuseRequestGroups",
                url: "RequestGroup/Refuse/{id}/{user}",
                defaults: new { controller = "RequestGroup", action = "Refuse" });
            routes.MapRoute(
                name: "KickRequestGroups",
                url: "RequestGroup/Kick/{id}/{user}",
                defaults: new { controller = "RequestGroup", action = "Kick" });
            routes.MapRoute(
                    name: "ShowPostGroups",
                    url: "Group/ShowPost/{id}/{idpost}",
                    defaults: new { controller = "Group", action = "ShowPost" });

            routes.MapRoute(
                    name: "EditPostGroups",
                    url: "Group/EditPost/{id}/{idpost}",
                    defaults: new { controller = "Group", action = "EditPost" });

            routes.MapRoute(
                    name: "DeletePostGroups",
                    url: "Group/DeletePost/{id}/{idpost}",
                    defaults: new { controller = "Group", action = "DeletePost" });

        }
    }
}

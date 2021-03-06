using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using MicroSocialPlatform.Models;
using Microsoft.AspNet.Identity;

namespace MicroSocialPlatform.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            ViewBag.User = db.Users.Find(userId);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
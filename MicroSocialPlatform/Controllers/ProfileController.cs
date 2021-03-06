using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MicroSocialPlatform.Models;
using Microsoft.AspNet.Identity;

namespace MicroSocialPlatform.Controllers
{
    public class ProfileController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Profile
        public ActionResult Index(String id)
        {
            var profil = db.Profile.Find(id);
            ViewBag.User = profil;

            var posts = db.Posts.Where(p => p.UserId == profil.Id).OrderByDescending(p => p.Date).ToList();
            ViewBag.Posts = posts;

            // Este prieten sau a primit request
            if (db.Friendship.Find(id, User.Identity.GetUserId()) != null)
                ViewBag.Friendship = "prieteni";
            else if (db.Request.Find(User.Identity.GetUserId(), id) != null)
                ViewBag.Friendship = "trimisa";
            else if (db.Request.Find(id, User.Identity.GetUserId()) != null)
                ViewBag.Friendship = "primita";
            else if (User.Identity.GetUserId() == null)
                ViewBag.Friendship = "neautentificat";
            else if (id != User.Identity.GetUserId())
                ViewBag.Friendship = "straini";


            var friendsid = db.Friendship.Where(p => p.Id == profil.Id).ToList();
            List<Profile> friends = new List<Profile>();
            foreach (var i in friendsid)
            {
                friends.Add(db.Profile.Find(i.Id2));
            }

            ViewBag.NoFriends = friends.Count();
            ViewBag.Friends = friends;

            return View();

        }
        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(String id)
        {
            if (id == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                ViewBag.profile = db.Profile.Find(id);
                return View(db.Profile.Find(id));
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari";
                return RedirectToAction("Index", "Profile");
            }
            return View();
        }
        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(String id, Profile requestProfile)
        {
            try
            {
                Profile prof = db.Profile.Find(id);
                if (TryUpdateModel(prof))
                {
                    prof.Username = requestProfile.Username;
                    prof.Name = requestProfile.Name;
                    prof.About = requestProfile.About;
                    prof.City = requestProfile.City;
                    prof.Age = requestProfile.Age;
                    prof.Job = requestProfile.Job;
                    prof.publicProfile = requestProfile.publicProfile;
                    db.SaveChanges();
                }
                return RedirectToAction("Index", "Profile", new { id = id });
            }
            catch (Exception e)
            {
                return View();
            }

        }
        public ActionResult Friends(String id)
        {
            var profil = db.Profile.Find(id);
            ViewBag.User = profil;

            var friendsid = db.Friendship.Where(p => p.Id == profil.Id).ToList();
            List<Profile> friends = new List<Profile>();
            foreach (var i in friendsid)
            {
                friends.Add(db.Profile.Find(i.Id2));
            }

            ViewBag.NoFriends = friends.Count();
            ViewBag.Friends = friends;


            var requests = db.Request.Where(p => p.Received == profil.Id).ToList();
            var requestsSenders = (from pair in requests select new List<string> { pair.Sent, db.Profile.Find(pair.Sent).Name }).ToList();

            ViewBag.RequestsSenders = requestsSenders;

            return View();
        }
        [HttpPost]
        public ActionResult Search(String name)
        {
            var results = db.Profile.Where(p => p.Name == name).ToList();
            ViewBag.Searched = name;
            return View(results);
        }
    }
}
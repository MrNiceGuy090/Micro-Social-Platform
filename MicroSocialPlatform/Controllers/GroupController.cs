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
    public class GroupController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Posts

        public ActionResult Index()
        {
            var groups = db.Groups.ToList();
            
            ViewBag.Groups = groups;
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"].ToString();
            }

            return View();
        }
        
        public ActionResult Show(int id)
        {
            Group group = db.Groups.Find(id);
            // Poate si p.idgroup
            var posts = db.Groupposts.Where(p => p.GroupId == id).ToList();
            ViewBag.Posts = posts;
            ViewBag.Group = group;

            var req = db.RequestGroup.Find(id, User.Identity.GetUserId());
            var user = db.Members.Find(id, User.Identity.GetUserId());

            if (req != null)
                ViewBag.Member = "pending";
            else if (user == null)
                ViewBag.Member = "false";
            else ViewBag.Member = "true";

            return View(group);
        }

        [Authorize(Roles = "User,Admin")]
        public ActionResult New()
        {
            Group group = new Group();

            return View(group);
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public ActionResult New(Group group)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                    db.Groups.Add(group);
                    TempData["message"] = "Grupul a fost creat!";

                    db.SaveChanges();

                    Member mbr = new Member();
                    mbr.GroupId = group.Id;
                    mbr.UserId = User.Identity.GetUserId();
                    mbr.Role = "Admin1";

                    db.Members.Add(mbr);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(group);
                }
            }
            catch (Exception e)
            {
                return View(group);
            };


        }
        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(int id)
        {
            var userid = User.Identity.GetUserId();
            var userrole = db.Members.Find(id, userid).Role;
            if (userrole == "Admin1" || userrole == "Admin2" || User.IsInRole("Admin"))
            {
                ViewBag.Group = db.Groups.Find(id);
                return View(db.Groups.Find(id));
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari";
                return RedirectToAction("Index", "Groups");
            }
        }

        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(int id, Group requestGroup)
        {
            try
            {
                Group group = db.Groups.Find(id);
                if (TryUpdateModel(group))
                {
                    group.Name = requestGroup.Name;
                    group.Description = requestGroup.Description;
                    db.SaveChanges();
                }
                return RedirectToAction("Show", "Group", new { id });
            }
            catch (Exception e)
            {
                return View();
            }

        }

        public ActionResult GroupMembers(int id)
        {
            var group = db.Groups.Find(id);
            ViewBag.Group = group;

            var membersid = db.Members.Where(p => p.GroupId == group.Id).ToList();
            List<Profile> members = new List<Profile>();
            foreach (var i in membersid)
            {
                members.Add(db.Profile.Find(i.UserId));
            }

            ViewBag.NoMembers = members.Count();
            ViewBag.Members = members;

            var user = db.Members.Find(id, User.Identity.GetUserId());
            if (user == null)
            {
                ViewBag.Admin = "No";
            }
            else if (user.Role == "Admin1" || user.Role == "Admin2")
            {
                ViewBag.Admin = "Yes";
            }
            else ViewBag.Admin = "No";

            var requestsid = db.RequestGroup.Where(p => p.GroupId == group.Id).ToList();
            List<Profile> requests = new List<Profile>();
            foreach (var i in requestsid)
            {
                requests.Add(db.Profile.Find(i.Sent));
            }
            ViewBag.RequestsSenders = requests;

            return View();
        }

        [Authorize(Roles = "User,Admin")]
        public ActionResult NewPost(int id)
        {
            var user = db.Members.Find(id, User.Identity.GetUserId());
            if (user != null)
            {
                Grouppost post = new Grouppost();
                post.UserId = User.Identity.GetUserId();
                post.GroupId = id;
                return View(post);
            }
            else return View();
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public ActionResult NewPost( Grouppost post)
        {
            Grouppost postare = new Grouppost();
            postare.GroupId = post.GroupId;
            postare.Content = post.Content;
            postare.Date = post.Date;
            postare.UserId = User.Identity.GetUserId();

            Random random = new Random();
            postare.Id = random.Next();
            while (postare.Id == null  )
            {
                postare.Id = random.Next();
            }

            if (postare.Id != null)
            {
                db.Groupposts.Add(postare);
                db.SaveChanges();
            }
            TempData["message"] = "Postarea a fost adaugata!";
            return RedirectToAction("Show", new { id = postare.GroupId});


        }


        public ActionResult ShowPost(int id, int idpost)
        {
            Grouppost post = db.Groupposts.Find(id, idpost);
            var user = db.Members.Find(id, User.Identity.GetUserId());
            if (!User.Identity.IsAuthenticated)
                ViewBag.Owner = false;
            else if (user == null)
                ViewBag.Owner = false;
            else if (post.UserId == User.Identity.GetUserId() | User.IsInRole("Admin") | user.Role == "Admin1")
                ViewBag.Owner = true;
            else ViewBag.Owner = false;

            ViewBag.Post = post;
            return View(post);
        }

        [Authorize(Roles = "User,Admin")]
        public ActionResult EditPost(int id, int idpost)
        {
            Grouppost post = db.Groupposts.Find(id, idpost);
            ViewBag.Post = post;
            var user = db.Members.Find(id, User.Identity.GetUserId());
            if (post.UserId == User.Identity.GetUserId() | User.IsInRole("Admin") | user.Role == "Admin1")
            {
                return View(post);
            }

            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari asupra unui postari care nu va apartine";
                return RedirectToAction("Index");
            }
        }
        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        public ActionResult EditPost( Grouppost requestPost)
        {
            var id = requestPost.GroupId;
            var idpost = requestPost.Id;
            try
            {
                if (ModelState.IsValid)
                {
                    Grouppost post = db.Groupposts.Find(id, idpost);

                    if (TryUpdateModel(post))
                    {
                        post.Content = requestPost.Content;
                        post.Date = DateTime.Now;
                        db.SaveChanges();
                    }
                    return RedirectToAction("ShowPost", new { id , idpost });
                }

                else
                {
                    return View(requestPost);
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }


        [HttpDelete]
        [Authorize(Roles = "User,Admin")]
        public ActionResult DeletePost(int id, int idpost)
        {
            Grouppost post = db.Groupposts.Find(id,idpost);
            var user = db.Members.Find(id, User.Identity.GetUserId());
            if (post.UserId == User.Identity.GetUserId() | User.IsInRole("Admin") | user.Role == "Admin1")
            {
                db.Groupposts.Remove(post);
                db.SaveChanges();
                TempData["message"] = "Postarea a fost sters!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti unei postari care nu va apartine";
                return RedirectToAction("Index");
            }
        }
    }

}
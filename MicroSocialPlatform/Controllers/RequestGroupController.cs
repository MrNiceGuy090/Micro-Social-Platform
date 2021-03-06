using MicroSocialPlatform.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MicroSocialPlatform.Controllers
{
    public class RequestGroupController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: RequestGroup
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "User,Admin")]
        public ActionResult Send(int id)
        {
            var mbr = db.Members.Find(id, User.Identity.GetUserId());
            var r = db.RequestGroup.Find(id, User.Identity.GetUserId());

            if (mbr == null && r == null)
            {
                RequestGroup req = new RequestGroup();
                req.GroupId = id;
                req.Sent = User.Identity.GetUserId();
                db.RequestGroup.Add(req);
                db.SaveChanges();
                return RedirectToAction("Index", "Group", new { id });
            }
            else
                return View();
        }


        [Authorize(Roles = "User,Admin")]
        public ActionResult Accept(int id, string user)
        {
            var mbr = db.Members.Find(id, User.Identity.GetUserId());

            if (mbr.Role == "Admin1" || mbr.Role == "Admin2")
            {
                var req = db.RequestGroup.Find(id, user);
                if (req != null)
                {
                    Member mbr2 = new Member();
                    mbr2.GroupId = id;
                    mbr2.UserId = user;
                    mbr2.Role = "User";

                    db.Members.Add(mbr2);
                    db.RequestGroup.Remove(req);
                    db.SaveChanges();
                    return RedirectToAction("GroupMembers", "Group", new { id });
                }
                else return View();
            }
            
            else return View();
        }


        [Authorize(Roles = "User,Admin")]
        public ActionResult Refuse(int id, string user)
        {
            var mbr = db.Members.Find(id, User.Identity.GetUserId());

            if (mbr.Role == "Admin1" || mbr.Role == "Admin2")
            {
                var req = db.RequestGroup.Find(id, user);
                if (req != null)
                {
                    db.RequestGroup.Remove(req);
                    db.SaveChanges();
                    return RedirectToAction("GroupMembers", "Group", new { id });
                }
                else return View();
            }

            else return View();
        }

        [Authorize(Roles = "User,Admin")]
        public ActionResult Kick(int id, string user)
        {
            var mbr = db.Members.Find(id, User.Identity.GetUserId());

            if (mbr.Role == "Admin1" || mbr.Role == "Admin2")
            {
                var mbr2 = db.Members.Find(id, user);
                if (mbr2 != null)
                {
                    db.Members.Remove(mbr2);
                    db.SaveChanges();
                    return RedirectToAction("GroupMembers", "Group", new { id });
                }
                else return View();
            }

            else return View();
        }

    }
}
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
    public class CommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Comments
        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public ActionResult New(Comment comment)
        {
            comment.UserId = User.Identity.GetUserId();
            var postId = comment.PostId;
            db.Comments.Add(comment);
            db.SaveChanges();
            return RedirectToAction("Show", "Posts", new { id = postId });
        }

        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(int id)
        {
            Comment com = db.Comments.Find(id);
            ViewBag.Comment = com;
            if (com.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                return View(com);
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari";
                return RedirectToAction("Index", "Posts");
            }
        }
        [HttpPut]
        [Authorize(Roles = "User,Admin")]
        public ActionResult Edit(int id, Post requestCom)
        {
            try
            {
                Comment com = db.Comments.Find(id);
                if (TryUpdateModel(com))
                {
                    com.Content = requestCom.Content;
                    com.Date = DateTime.Now;
                    db.SaveChanges();
                }
                return RedirectToAction("Show", "Posts", new { id = com.PostId });
            }
            catch (Exception e)
            {
                return View();
            }
        }

        [Authorize(Roles = "User,Admin")]
        public ActionResult Delete(int id)
        {
            Comment com = db.Comments.Find(id);

            if (com.UserId == User.Identity.GetUserId() || User.IsInRole("Admin"))
            {
                var postId = com.PostId;
                db.Comments.Remove(com);
                db.SaveChanges();
                return RedirectToAction("Show", "Posts", new { id = postId });
            }
            else
            {
                TempData["message"] = "Nu aveti dreptul sa faceti modificari";
                return RedirectToAction("Index", "Posts");
            }
        }
    }
}
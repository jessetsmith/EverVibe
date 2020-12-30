using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VibeSpace.DATA;
using VibeSpace.MODELS;
using VibeSpace.Services;

namespace VibeSpace.Controllers
{
    public class CommentController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        private CommentService CreateCommentService()
        {
            var userId = User.Identity.GetUserId();
            var commentService = new CommentService(userId);
            return commentService;
        }

        private VibeService CreateVibeService()
        {
            var userId = User.Identity.GetUserId();
            var vibeService = new VibeService(userId);
            return vibeService;
        }

        // GET: Vibes
        public ActionResult Index()
        {
            var comments = CreateCommentService().GetComments();

            return View(comments);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        public ActionResult Create(CommentCreate comment, int id)
        {
            var vibe = CreateVibeService().GetVibesByID(id).VibeID;

            if (ModelState.IsValid)
            {
                var service = CreateCommentService().CreateComment(comment, vibe);
                if (!service.Equals(1))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                return RedirectToAction("Index");
            }
            return View(vibe);
        }

        //GET : Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentDetail comment = CreateCommentService().GetCommentsByID(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var service = CreateCommentService().DeleteComments();
            if (!service.Equals(1))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return RedirectToAction("Index");
        }

        //GET : Edit
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentDetail comment = CreateCommentService().GetCommentsByID(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CommentEdit comment)
        {
            if (ModelState.IsValid)
            {
                var service = CreateCommentService().UpdateComment(comment);
                if (!service.Equals(1))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                return RedirectToAction("Index");
            }
            return View(comment);
        }

        //GET : Details

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CommentDetail comment = CreateCommentService().GetCommentsByID(id);

            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }
    }
}
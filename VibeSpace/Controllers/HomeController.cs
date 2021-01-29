using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VibeSpace.MODELS;
using VibeSpace.Services;

namespace VibeSpace.Controllers
{
    public class HomeController : Controller
    {

        private VibeService CreateVibeService()
        {
            var userId = User.Identity.GetUserId();
            var username = User.Identity.Name;

            var vibeService = new VibeService(userId, username);
            return vibeService;
        }

        public UserInfoService CreateUserInfoService()
        {
            var userId = User.Identity.GetUserId();
            var userService = new UserInfoService(userId);
            return userService;
        }

        private CommentService CreateCommentService()
        {
            var userId = User.Identity.GetUserId();
            var commentService = new CommentService(userId);
            return commentService;
        }

        //public string GetUserId()
        //{
        //    var userId = User.Identity.GetUserId();
        //    var userService = new UserInfoService(userId);
        //    var userInfo = userService.GetUsersByUserId(userId);
        //    var user = userInfo.UserID;
        //    return user;
        //}



        public ActionResult Index()
        {
         
            IEnumerable<VibeListItem> vibe = CreateVibeService().GetVibes();

            return View("Index", vibe);
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

        //public ActionResult _UsersVibesPartial(int? id)
        //{
        //    var vibe = CreateVibeService().GetVibesByID(id);
        //    var userID = vibe.Select(e => e.UserID).ToString();
        //    var userVibes = CreateVibeService().GetVibesByUserID(userID);


        //    return View(userVibes);
        //}

        public ActionResult PublicDetails(string id)
        {

            //var vibe = CreateVibeService().GetVibeDetailsById(id);
            UserInfoDetail user = null;

            var userID = id;

            if(userID != null)
            {
                user = CreateUserInfoService().GetUsersByUserId(userID);
                if(user != null)
                {
                    return View(user);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            //ICollection<Vibespace.DATA.Vibe> vibes = user.Vibes;

            //if (user == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}

        }

        public ActionResult CommentModal(int? vibeId)
        {
            var comments = CreateCommentService().GetCommentsByVibeID(vibeId);

            return View("_CommentModal", comments);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CommentCreate(CommentCreate comment, int id)
        {
            var vibe = CreateVibeService().GetVibeDetailsByVibeId(id);

            if (ModelState.IsValid)
            {
                var service = CreateCommentService().CreateComment(comment, id);
                if (service)
                {
                    return RedirectToAction("Index");

                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                }
            }
            return View(vibe);
        }
    }
}
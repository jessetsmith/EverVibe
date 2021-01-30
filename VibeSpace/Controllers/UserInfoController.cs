using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Vibespace.DATA;
using VibeSpace.DATA;
using VibeSpace.MODELS;
using VibeSpace.Services;
using ActionNameAttribute = System.Web.Mvc.ActionNameAttribute;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;

namespace VibeSpace.Controllers
{
    public class UserInfoController : Controller
    {

        private ApplicationDbContext _db = new ApplicationDbContext();

        private UserInfoService CreateUserInfoService()
        {
            var userId = User.Identity.GetUserId();
            var userService = new UserInfoService(userId);
            return userService;
        }

        private VibeService CreateVibeService()
        {
            var userId = User.Identity.GetUserId();
            var username = User.Identity.Name;

            var vibeService = new VibeService(userId, username);
            return vibeService;
        }

        
        // GET: UserInfo
        public ActionResult Index()
        {
            var service = CreateUserInfoService().GetUsers();

            return View(service);
        }

        public ActionResult Create()
        {
            var ctx = new ApplicationDbContext();
            var userId = User.Identity.GetUserId();

            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserInfoCreate user)
        {
            
                if (ModelState.IsValid)
                {
                    HttpPostedFileBase file = Request.Files["Image"];

                    var service = CreateUserInfoService().CreateUserInfo(user, file);
                    if (service)
                    {
                        return RedirectToAction("Details");

                    }
                    else
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                    }
                    //if (!service.Equals(1))
                    //{
                    //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    //}
                }
            return View(user);
        }

        //GET : Delete
        public ActionResult Delete(string id)
        {
            id = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInfoDetail user = CreateUserInfoService().GetUsersByUserId(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete()
        {
            var service = CreateUserInfoService().DeleteUserInfo();
            if (service)
            {
                return RedirectToAction("Index");

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }
        }

        //GET : Edit
        public ActionResult Edit(string id)
        {
            id = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = CreateUserInfoService().GetUsersByUserIdEdit(id);
           
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserInfoEdit user)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["Image"];

                var service = CreateUserInfoService().UpdateUserInfo(user, file);
                if (service)
                {
                    return RedirectToAction("Index");

                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

                }
            }
            return View(user);
        }

        //GET : Details

        //public ActionResult _UsersVibesPartial(int? id)
        //{
        //    var vibe = CreateVibeService().GetVibesByID(id);
        //    var userID = vibe.Select(e => e.UserID).ToString();
        //    var userVibes = CreateVibeService().GetVibesByUserID(userID);


        //    return View(userVibes);
        //}

        public ActionResult Details(string id)
        {
            id = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //var vibes = CreateVibeService().get
            var user = CreateUserInfoService().GetUsersByUserId(id);
            var vibes = CreateVibeService().GetVibesByUserID(id);

            ViewBag.vibes = vibes;

            if (user == null)
            {
                JavaScript("<script language='javascript'>window.alert('Please create a user profile')</script>");
                return RedirectToAction("Create", "UserInfo");
            }
            return View("Details", user);
        }

        //public ActionResult PublicDetails(int? id)
        //{
        //    var vibe = CreateVibeService().GetVibesByID(id);
        //    var userID = vibe.Select(e => e.Username).ToString();

        //    var user = CreateUserInfoService().GetUsersByUsername(userID);
        //    ICollection<Vibespace.DATA.Vibe> vibes = user.Vibes;

        //    if (user == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    return View(user);
        //}


    }
}
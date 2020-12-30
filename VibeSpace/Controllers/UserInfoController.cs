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

        // GET: UserInfo
        public ActionResult Index()
        {
            var service = CreateUserInfoService().GetUsers();

            return View(service);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        public ActionResult Create(UserInfoCreate user)
        {
            if (ModelState.IsValid)
            {
                var service = CreateUserInfoService().CreateUserInfo(user);
                if (!service.Equals(1))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                return RedirectToAction("Index");
            }
            return View(user);
        }

        //GET : Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserInfo user = _db.UsersInfo.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var service = CreateUserInfoService().DeleteUserInfo();
            if (!service.Equals(1))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return RedirectToAction("Index");
        }

        //GET : Edit
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = CreateUserInfoService().GetUsersByID(id);
           
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
                var service = CreateUserInfoService().UpdateUserInfo(user);
                if (!service.Equals(1))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                return RedirectToAction("Index");
            }
            return View(user);
        }

        //GET : Details

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = CreateUserInfoService().GetUsersByID(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
    }
}
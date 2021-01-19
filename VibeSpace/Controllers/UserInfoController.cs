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
            var userId = User.Identity.GetUserId();

            if (CreateUserInfoService().GetUsersByUserId(userId) == null)
            {
                return View();
            }
            else
            {
                return JavaScript("alert('User Info Already Exists. If you'd like to make changes, please use the Edit Option')");

            }
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
                        return RedirectToAction("Index");

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
                var service = CreateUserInfoService().UpdateUserInfo(user);
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

        public ActionResult Details(string id)
        {
            id = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = CreateUserInfoService().GetUsersByUserId(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
    }
}
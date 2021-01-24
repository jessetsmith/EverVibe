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
            var vibeService = new VibeService(userId);
            return vibeService;
        }

        public UserInfoService CreateUserInfoService()
        {
            var userId = User.Identity.GetUserId();
            var userService = new UserInfoService(userId);
            return userService;
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

        public ActionResult PublicDetails(int? id)
        {
            var vibe = CreateVibeService().GetVibesByID(id);
            var user = CreateUserInfoService().GetUsersByUsername(vibe.Username);

            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(user);
        }
    }
}
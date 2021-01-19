using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private UserInfoService CreateUserInfoService()
        {
            var userId = User.Identity.GetUserId();
            var userService = new UserInfoService(userId);
            return userService;
        }

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var user = CreateUserInfoService().GetUsersByUserId(userId);
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
    }
}
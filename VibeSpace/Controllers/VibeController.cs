using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Vibespace.DATA;
using VibeSpace.DATA;
using VibeSpace.MODELS;
using VibeSpace.Services;

namespace VibeSpace.Controllers
{
    public class VibeController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

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
        //    return userId;
        //}

        //public ContentResult GetLocation()
        //{
        //    var location = CreateVibeService().ResolveAddressSync();
        //    HtmlString _location = Convert.toht
        //    return location;

        //}


        // GET: Vibes
        public ActionResult Index()
        {
            IEnumerable<VibeListItem> vibe = CreateVibeService().GetVibes();

            return View(vibe);
        }

        public ActionResult List()
        {
            var vibe = CreateVibeService().GetVibeDetails();

            return View(vibe);
        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost, ActionName("Create")]
        public ActionResult Create(VibeCreate vibe)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["VibeImage"];

                var service = CreateVibeService().CreateVibe(vibe, file);
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

        //GET : Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VibeDetail vibe = CreateVibeService().GetVibesByID(id);
            if (vibe == null)
            {
                return HttpNotFound();
            }
            return View(vibe);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var service = CreateVibeService().DeleteVibe(id);
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
            VibeDetail vibe = CreateVibeService().GetVibesByID(id);
            if (vibe == null)
            {
                return HttpNotFound();
            }
            return View(vibe);
        }


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VibeEdit vibe)
        {
            if (ModelState.IsValid)
            {
                var service = CreateVibeService().UpdateVibe(vibe);
                if (!service.Equals(1))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                return RedirectToAction("Index");
            }
            return View(vibe);
        }

        //GET : Details

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VibeDetail vibe = CreateVibeService().GetVibesByID(id);

            if (vibe == null)
            {
                return HttpNotFound();
            }
            return View(vibe);
        }
    }
}
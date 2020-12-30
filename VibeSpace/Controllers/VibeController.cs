﻿using Microsoft.AspNet.Identity;
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

        // GET: Vibes
        public ActionResult Index()
        {
            var vibe = CreateVibeService().GetVibes();

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
                var service = CreateVibeService().CreateVibe(vibe);
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
            var service = CreateVibeService().DeleteVibe();
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
﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEverNote.BusinessLayer;
using MyEverNote.BusinessLayer.Results;
using MyEverNote.Entities;
using MyEverNote.WebApp.Filters;

namespace MyEverNote.WebApp.Controllers
{
    [Auth]
    [AuthAdmin]
    [Exc]
    public class EverNoteUserController : Controller
    {
        private EverNoteUserManager evernoteusermanager = new EverNoteUserManager();

        // GET: EverNoteUser
        public ActionResult Index()
        {
            return View(evernoteusermanager.List());
        }

        // GET: EverNoteUser/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EverNoteUser everNoteUser = evernoteusermanager.Find(x => x.Id == id.Value);
            if (everNoteUser == null)
            {
                return HttpNotFound();
            }
            return View(everNoteUser);
        }

        // GET: EverNoteUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EverNoteUser/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EverNoteUser everNoteUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<EverNoteUser> res = evernoteusermanager.Insert(everNoteUser);
                if (res.Errors.Count>0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(everNoteUser);
                }
                return RedirectToAction("Index");
            }

            return View(everNoteUser);
        }

        // GET: EverNoteUser/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EverNoteUser everNoteUser = evernoteusermanager.Find(x => x.Id == id.Value);
            if (everNoteUser == null)
            {
                return HttpNotFound();
            }
            return View(everNoteUser);
        }

        // POST: EverNoteUser/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EverNoteUser everNoteUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<EverNoteUser> res = evernoteusermanager.Update(everNoteUser);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(everNoteUser);
                }
                return RedirectToAction("Index");
            }
            return View(everNoteUser);
        }

        // GET: EverNoteUser/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EverNoteUser everNoteUser = evernoteusermanager.Find(x => x.Id == id.Value);
            if (everNoteUser == null)
            {
                return HttpNotFound();
            }
            return View(everNoteUser);
        }

        // POST: EverNoteUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EverNoteUser everNoteUser = evernoteusermanager.Find(x => x.Id == id);
            evernoteusermanager.Delete(everNoteUser);
            return RedirectToAction("Index");
        }
    }
}

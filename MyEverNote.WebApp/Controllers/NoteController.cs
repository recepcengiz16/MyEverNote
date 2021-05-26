using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEverNote.BusinessLayer;
using MyEverNote.Entities;
using MyEverNote.WebApp.Filters;
using MyEverNote.WebApp.Models;

namespace MyEverNote.WebApp.Controllers
{
    [Exc]
    public class NoteController : Controller
    {
        private NoteManager notemanager = new NoteManager();
        private CategoryManager categorymanager = new CategoryManager();
        private LikedManager likemanager = new LikedManager();
        // GET: Note
        [Auth]
        public ActionResult Index()
        {
            //owner evernoteuser ile ilişkili olan propertynin adı onu yazmamız gerekli tablonun adı değil propun adı. Category de aynı. note clasındaki propertynin adı
            var notes = notemanager.ListQueryable().Include("Category").Include("Owner").
                Where(x=>x.Owner.Id==CurrentSession.User.Id).
                OrderByDescending(x=>x.ModifiedOn); //sorguya dahil etsin notun categorilerini de
            
            return View(notes.ToList());
        }

        [Auth]
        public ActionResult MyLikedNotes()
        {
           var notes= likemanager.ListQueryable().Include("LıkedUser").Include("Note").Where(x => x.LikedUser.Id == CurrentSession.User.Id).
                Select(x=>x.Note).Include("Category").Include("Owner").OrderByDescending(x=>x.ModifiedOn);

            return View("Index",notes.ToList());
        }

        [HttpPost]
        public ActionResult GetLiked(int[] ids)
        {
            if (CurrentSession.User != null)
            {
                int userid = CurrentSession.User.Id;
                List<int> likedNoteIds = new List<int>();

                if (ids != null)
                {
                    likedNoteIds = likemanager.List(
                        x => x.LikedUser.Id == userid && ids.Contains(x.Note.Id)).Select(
                        x => x.Note.Id).ToList();
                }
                else
                {
                    likedNoteIds = likemanager.List(
                        x => x.LikedUser.Id == userid).Select(
                        x => x.Note.Id).ToList();
                }

                return Json(new { result = likedNoteIds });
            }
            else
            {
                return Json(new { result = new List<int>() });
            }
        }

        [HttpPost]
        public ActionResult SetLikeState(int noteid, bool liked)
        {
            int res = 0;

            if (CurrentSession.User == null)
                return Json(new { hasError = true, errorMessage = "Beğenme işlemi için giriş yapmalısınız.", result = 0 });

            Liked like =
                likemanager.Find(x => x.Note.Id == noteid && x.LikedUser.Id == CurrentSession.User.Id);

            Note note = notemanager.Find(x => x.Id == noteid);

            if (like != null && liked == false) //liked false ise yani false yapılmaya çalışılıyor manasında
            {
                res = likemanager.Delete(like);
            }
            else if (like == null && liked == true)
            {
                res = likemanager.Insert(new Liked()
                {
                    LikedUser = CurrentSession.User,
                    Note = note
                });
            }

            if (res > 0)
            {
                if (liked)
                {
                    note.LikeCount++;
                }
                else
                {
                    note.LikeCount--;
                }

                res = notemanager.Update(note);

                return Json(new { hasError = false, errorMessage = string.Empty, result = note.LikeCount });//işlem başarılı olduysa
            }

            return Json(new { hasError = true, errorMessage = "Beğenme işlemi gerçekleştirilemedi.", result = note.LikeCount });
        }


        // GET: Note/Details/5
        [Auth]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = notemanager.Find(x=>x.Id==id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // GET: Note/Create
        [Auth]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title");
            return View();
        }

        // POST: Note/Create
        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                note.Owner = CurrentSession.User;
                notemanager.Insert(note);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        // GET: Note/Edit/5
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = notemanager.Find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        // POST: Note/Edit/5

        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
               Note db_note= notemanager.Find(x => x.Id == note.Id);
                db_note.IsDraft = note.IsDraft;
                db_note.CategoryId = note.CategoryId;
                db_note.Text = note.Text;
                db_note.Title = note.Title;

                notemanager.Update(db_note);
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        // GET: Note/Delete/5
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = notemanager.Find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Note/Delete/5
        [Auth]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = notemanager.Find(x => x.Id == id);

            return RedirectToAction("Index");
        }

        public ActionResult GetNoteText(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = notemanager.Find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialNoteText",note);
        }
    }
}

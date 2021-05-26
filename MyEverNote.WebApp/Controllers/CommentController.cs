using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using MyEverNote.BusinessLayer;
using MyEverNote.Entities;
using System.Data.Entity;
using MyEverNote.WebApp.Models;
using MyEverNote.WebApp.Filters;

namespace MyEverNote.WebApp.Controllers
{
    [Exc]
    public class CommentController : Controller
    {
        private NoteManager notemanager = new NoteManager();
        private CommentManager commentmanager = new CommentManager();

        // GET: Comment
        public ActionResult ShowNoteComments(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Note not = notemanager.Find(x => x.Id == id); alttaki ile aynı
            Note not = notemanager.ListQueryable().Include("Comments").FirstOrDefault(x => x.Id == id);

            if (not==null)
            {
                return HttpNotFound();
            }

            return PartialView("_PartialComments",not.Comments);
        }

        [Auth]
        [HttpPost]
        public ActionResult Edit(int? id,String text)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = commentmanager.Find(x => x.Id == id);
            if (comment==null)
            {
                return new HttpNotFoundResult();
            }

            comment.Text = text;
            if(commentmanager.Update(comment)>0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = commentmanager.Find(x => x.Id == id);
            if (comment == null)
            {
                return new HttpNotFoundResult();
            }

            if (commentmanager.Delete(comment) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        [Auth]
        [HttpPost]
        public ActionResult Create(Comment comment,int? noteid)
        {
            //gelen text özelliği commentin içindeki textin içine yazılacak

            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                if (noteid == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Note note = notemanager.Find(x => x.Id == noteid);
                if (note == null)
                {
                    return new HttpNotFoundResult();
                }
                comment.Note = note;
                comment.Owner = CurrentSession.User;

                if (commentmanager.Insert(comment) > 0)
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }
    }
}
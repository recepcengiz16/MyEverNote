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
    [Auth]
    [AuthAdmin]
    [Exc]
    public class CategoryController : Controller
    {
        //categoryde de repository deki ekle listele güncelle gibi şeylere erişmek istiyorum.
        //zaten bu metotlar data access içindeki abstract olan Irepository de var ama Dataaccess layera ulaşımımız yok bunun için de bir katman da oluşturup.
        //Bu katmana Irepository yi ekliyim erişime açık olsun da gidip businessdaki manager katmanında yeniden teker teker yazmak zorunda kalmayayım. Bu katmana da myevernote.core ismin verdik.

        private CategoryManager categorymanager = new CategoryManager();
        // GET: Category
        public ActionResult Index()
        {
            return View(categorymanager.List());
        }

        // GET: Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categorymanager.Find(x=>x.Id==id.Value); //nulable olduğu için .value ile veriyoruz.
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                categorymanager.Insert(category);
                CacheHelper.RemoveCategoriesFromCache(); //insertten sonra yeni bir kategori eklediysem o cache i silsin zaten silince cache güncelleyecek.
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categorymanager.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");


            if (ModelState.IsValid)
            {
                Category cat = categorymanager.Find(x=>x.Id==category.Id);
                cat.Title = category.Title;
                cat.Description = category.Description;

                categorymanager.Update(cat);
                CacheHelper.RemoveCategoriesFromCache();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categorymanager.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = categorymanager.Find(x => x.Id == id);
            categorymanager.Delete(category);
            CacheHelper.RemoveCategoriesFromCache();

            return RedirectToAction("Index");
        }

    }
}

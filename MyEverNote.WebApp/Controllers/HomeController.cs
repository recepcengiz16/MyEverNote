using MyEverNote.BusinessLayer;
using MyEverNote.Entities;
using MyEverNote.Entities.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEverNote.Entities.Messages;
using MyEverNote.WebApp.ViewModels;
using MyEverNote.BusinessLayer.Results;
using MyEverNote.WebApp.Models;
using MyEverNote.WebApp.Filters;

namespace MyEverNote.WebApp.Controllers
{
    [Exc]
    public class HomeController : Controller
    {
        private NoteManager notemanager = new NoteManager();
        private CategoryManager categorymanager = new CategoryManager();
        private EverNoteUserManager evernoteusermanager = new EverNoteUserManager();
        // GET: Home
        public ActionResult Index()
        {

            //CategoryController üzerinden gelen view talebi
            //if (TempData["notlar"]!=null)//category controllerından geliyor bu tempdata
            //{
            //    return View(TempData["notlar"] as List<Note>);
            //}

            
            return View(notemanager.ListQueryable().Where(x=>x.IsDraft==false).OrderByDescending(x=>x.ModifiedOn).ToList());
            //return View(nm.GetAllNoteQueryable().OrderByDescending(x => x.ModifiedOn).ToList()); bu da Iqueryable ile sorguladık sql tarafında ne zaman to list dersek gidip verileri çeksin gelsin
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            /*Category kat = categorymanager.Find(x=>x.Id==id.Value); *///burada nullable category manager da nullable olmayan olduğu için value değerini yazarız

            /* List<Note> notes=kat.Notes.Where(x => x.IsDraft == false).OrderByDescending(x=>x.ModifiedOn).ToList();*/

            //if (kat == null)
            //{
            //    return HttpNotFound();
            //}

            List<Note> notes=notemanager.ListQueryable().Where(x => x.IsDraft == false && x.CategoryId == id).OrderByDescending(x=>x.ModifiedOn).ToList();

            return View("Index",notes);
        }

        public ActionResult MostLiked()
        {
            
            return View("Index",notemanager.ListQueryable().OrderByDescending(x=>x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        [Auth]
        public ActionResult ShowProfile()
        {
               

            BusinessLayerResult<EverNoteUser> res = evernoteusermanager.GetUserById(CurrentSession.User.Id);

            if (res.Errors.Count>0)
            {
                ErrorViewModel hatanesnesi = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", hatanesnesi);
            }
            return View(res.Result);
        }

        [Auth]
        public ActionResult EditProfile()
        {

            BusinessLayerResult<EverNoteUser> res = evernoteusermanager.GetUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel hatanesnesi = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", hatanesnesi);
            }
            return View(res.Result);
        }

        [Auth]
        [HttpPost]
        public ActionResult EditProfile(EverNoteUser user,HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUserName"); // bu alanı zorunlu yapmıştık. Bunun için bir kontrol yapmasını istemiyorum 
            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                (ProfileImage.ContentType == "image/jpeg" || ProfileImage.ContentType == "image/jpg" || ProfileImage.ContentType == "image/png"))
                {
                    String filename = $"user_{user.Id}.{ProfileImage.ContentType.Split('/')[1]}"; //image/jpeg diyor ya / dan ayırıp 1. indisi yani jpegi al
                                                                                                  //user_12.jpg gibi oluyor.

                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    user.ProfileImageFileName = filename;
                }

                BusinessLayerResult<EverNoteUser> res = evernoteusermanager.UpdateProfile(user);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel message = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellenmedi",
                        RedirectUrl = "/Home/EditProfile"
                    };

                    return View("Error", message);
                }

                CurrentSession.Set<EverNoteUser>("login",res.Result); //profil güncellendiği için session güncellendi

                return RedirectToAction("ShowProfile");
            }
            return View(user);
        }

        [Auth]
        public ActionResult DeleteProfile()
        {

            BusinessLayerResult<EverNoteUser> res = evernoteusermanager.RemoveUserById(CurrentSession.User.Id);


            if (res.Errors.Count>0)
            {
                ErrorViewModel message = new ErrorViewModel()
                {
                    Items=res.Errors,
                    Title="Profil Silinemedi",
                    RedirectUrl="/Home/ShowProfile"
                };

                return View("Error", message);
            }

            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult Login()
        {
           
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            //yönlendirme
            //Sessiona kullanıcı adını ekleme
            if (ModelState.IsValid)
            {
                BusinessLayerResult<EverNoteUser> res= evernoteusermanager.LoginUser(model);

                 if (res.Errors.Count > 0) //hata var demek
                 {
                    if (res.Errors.Find(x=>x.Code==ErrorMessageCode.UserIsNotActive)!=null)
                    {
                        ViewBag.Setlink = "http://Home/Activate/123456-123";
                    }

                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message)); // tüm error listesinde foreach ile dön her biri için ilgili hatayı modelstate e hata olarak ver
                    return View(model);
                 }

                CurrentSession.Set<EverNoteUser>("login",res.Result);
                return RedirectToAction("Index");
            }
            

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
           
            if (ModelState.IsValid)
            {           
                BusinessLayerResult<EverNoteUser> res = evernoteusermanager.RegisterUser(model);

                if (res.Errors.Count>0) //hata var demek
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message)); // tüm error listesinde foreach ile dön her biri için ilgili hatayı modelstate e hata olarak ver
                    return View(model);
                }


                //try
                //{
                //    user=eum.RegisterUser(model);
                //}
                //catch (Exception hata)
                //{

                //    ModelState.AddModelError("", hata.Message);
                //}
                //if (user==null)
                //{
                //    return View(model);
                //}

                OkViewModel bilginesnesi = new OkViewModel() 
                {
                    Title="Kayıt Başarılı",
                    RedirectUrl="/Home/Login",
                };
                bilginesnesi.Items.Add("Lütfen E-posta adresinize gönderdiğimiz aktivasyon linkine tıklayarak hesabınızı aktive ediniz.<br />"
                            +"Hesabınızı aktive etmeden not ekleyemez ve beğeni yapamazsınız.");
                return View("Ok",bilginesnesi);
            }
            return View(model);
        }

       
        public ActionResult UserActivate(Guid id)
        {
            BusinessLayerResult<EverNoteUser> res = evernoteusermanager.ActivateUser(id);

            if (res.Errors.Count>0)
            {
                ErrorViewModel hatanesnesi = new ErrorViewModel() 
                {
                    Title="Geçersiz İşlem",
                    Items=res.Errors
                };

                return View("Error",hatanesnesi);
            }

            OkViewModel islemtamam = new OkViewModel() 
            {
                Title="Hesap Aktifleştirildi",
                RedirectUrl="/Home/Login"
            };
            islemtamam.Items.Add("Hesabınız aktifleştirildi. Arık not paylaşabilir ve beğeni yapabilirsiniz.");

            return View("Ok",islemtamam);
        }

       
        public ActionResult LogOut()
        {
            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult HasError()
        {
            return View();
        }
    }
}
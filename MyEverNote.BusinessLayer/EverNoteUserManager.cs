using MyEverNote.DataAccessLayer.EntityFramework;
using MyEverNote.Entities;
using MyEverNote.Entities.ValueObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEverNote.Entities.Messages;
using MyEverNote.Common.Helpers;
using MyEverNote.BusinessLayer.Results;
using MyEverNote.BusinessLayer.Abstract;

namespace MyEverNote.BusinessLayer
{
    public class EverNoteUserManager:ManagerBase<EverNoteUser>
    {
        //kayıt işlemlei ve böyle bir kullaıcı var mı onu görmek için bu sınıf var. Businesslayer da yazılan kodlar her UI için çalışan kodlar olması lazım.
        //metotlar manager baseden geliyor. abstract olduğu için direk biz de bu metotlara new lemeden erişebiliyoruz.
        public BusinessLayerResult<EverNoteUser> RegisterUser(RegisterViewModel data)
        {
            //Kullanıcı username kontrolü
            //Kullanıcı e posta kontrolü
            //Kayıt işlemi
            //Aktivasyon e posta gönderimi

            EverNoteUser u1 = Find(x => x.UserName == data.UserName || x.Email==data.Email);
            BusinessLayerResult<EverNoteUser> layerResult = new BusinessLayerResult<EverNoteUser>();
            
            if (u1!=null)
            {
                if (u1.UserName==data.UserName)
                {
                    layerResult.AddError(ErrorMessageCode.UserNameAlreadyExists,"Kullanıcı adı kayıtlı"); //hata mesajlarına ekledik.
                }
                if (u1.Email==data.Email)
                {
                    layerResult.AddError(ErrorMessageCode.EmailAlreadyExists,"Bu email adresi kayıtlı");
                }
            }
            else
            {
               int dbresult= base.Insert(new EverNoteUser() //aşağıda da insert var ya base classdan geleni kullan dedik.
                {
                    UserName=data.UserName,
                    Email=data.Email,
                    Password=data.Password,
                    ProfileImageFileName="profil.png",
                    ActivateGuid=Guid.NewGuid(),
                    IsActive=false,
                    IsAdmin=false
                });

                if (dbresult>0)//kullanıcı insert olmuş
                {
                    layerResult.Result =Find(x => x.Email == data.Email && x.UserName == data.UserName);
                    //TODO:Aktivasyon maili atılacak.
                    String siteUrl = ConfigHelper.Get<String>("SiteRootUrl");
                    String activateUrl = $"{siteUrl}/Home/UserActivate/{layerResult.Result.ActivateGuid}";
                    String body = $"Merhaba {layerResult.Result.UserName};<br><br>.Hesabınızı aktifleştirmek için <a href='{activateUrl}' target='_blank'>tıklayınız<a/>";
                    
                        
                    MailHelper.SendMail(body,layerResult.Result.Email,"MyEverNote hesap aktifleştirme");
                }
            }

            return layerResult;
        }

        public BusinessLayerResult<EverNoteUser> GetUserById(int id)
        {
            BusinessLayerResult<EverNoteUser> res = new BusinessLayerResult<EverNoteUser>();
            res.Result = Find(x => x.Id == id);

            if (res.Result==null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı bulunamadı");
            }

            return res;
        }

        public BusinessLayerResult<EverNoteUser> LoginUser(LoginViewModel data)
        {
            //Giriş kontrolü
            //Hesap aktif edildi mi

            
            BusinessLayerResult<EverNoteUser> res = new BusinessLayerResult<EverNoteUser>();
            res.Result = Find(x => x.UserName == data.UserName || x.Email == data.Password);


            if (res.Result!=null)
            {
                if (!res.Result.IsActive)//aktif değilse 
                {
                    res.AddError(ErrorMessageCode.UserIsNotActive,"Kullanıcı aktif edilmemiştir.");
                    res.AddError(ErrorMessageCode.CheckYourEmail,"E mail adresinizi kontrol ediniz.");
                }                
            }
            else
            {
                res.AddError(ErrorMessageCode.UserNameOrPassWrong, "Kullanıcı adı ya da şifre uyuşmuyor.");
            }
            return res;
        }

        public BusinessLayerResult<EverNoteUser> UpdateProfile(EverNoteUser data)
        {
            EverNoteUser dbuser = Find(x => x.UserName == data.UserName || x.Email == data.Email);
            BusinessLayerResult<EverNoteUser> res = new BusinessLayerResult<EverNoteUser>();

            if (dbuser !=null && dbuser.Id!=data.Id) //yani bunu güncelleyen ben miyim 
            {
                if (dbuser.UserName==data.UserName)
                {
                    res.AddError(ErrorMessageCode.UserNameAlreadyExists, "Kullanıcı Adı kayıtlı");

                }
                if (dbuser.Email==data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E posta adresi kayıtlı");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Password = data.Password;
            res.Result.UserName = data.UserName;

            if (String.IsNullOrEmpty(data.ProfileImageFileName)==false)
            {
                res.Result.ProfileImageFileName = data.ProfileImageFileName;
            }

            if (base.Update(res.Result)==0)
            {
                res.AddError(ErrorMessageCode.ProfilCouldNotUpdate, "Profil güncellenemedi");
            }

            return res;
        }

        public BusinessLayerResult<EverNoteUser> RemoveUserById(int id)
        {
            BusinessLayerResult<EverNoteUser> res = new BusinessLayerResult<EverNoteUser>();
            EverNoteUser user = Find(x => x.Id == id);


            if (user!=null)
            {
                if (Delete(user)==0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı silinemedi");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı bulunamadı");
            }
            return res;
        }

        public BusinessLayerResult<EverNoteUser> ActivateUser(Guid acivateId)
        {
            BusinessLayerResult<EverNoteUser> res = new BusinessLayerResult<EverNoteUser>();
            res.Result = Find(x => x.ActivateGuid == acivateId);

            if (res.Result!=null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir.");
                    return res;
                }
                res.Result.IsActive = true;
                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoseNotExist, "Aktifleştirilecek kullanıcı bulunamadı");
            }

            return res;
        }

        public new BusinessLayerResult<EverNoteUser> Insert(EverNoteUser data)
        {
            //üstten gelen insert metotu var ve override ederek ezebiliyorduk. Fakat üstten gelenin dönüşü int biz busineslayerresult istiyoruz.
            //New parametresi ile de ezmenin bir türü gibi düşünebilirsin.  Metot hiding deniliyor. Yani base classdan gelen insert ü gizle bu insert ü kullan
            EverNoteUser u1 = Find(x => x.UserName == data.UserName || x.Email == data.Email);
            BusinessLayerResult<EverNoteUser> layerResult = new BusinessLayerResult<EverNoteUser>();

            layerResult.Result = data;

            if (u1 != null)
            {
                if (u1.UserName == data.UserName)
                {
                    layerResult.AddError(ErrorMessageCode.UserNameAlreadyExists, "Kullanıcı adı kayıtlı"); //hata mesajlarına ekledik.
                }
                if (u1.Email == data.Email)
                {
                    layerResult.AddError(ErrorMessageCode.EmailAlreadyExists, "Bu email adresi kayıtlı");
                }
            }
            else
            {
                layerResult.Result.ProfileImageFileName = "profil.png";
                layerResult.Result.ActivateGuid = Guid.NewGuid();

                if (base.Insert(layerResult.Result) == 0) //basedeki dedik çünkü sadece insert deseydik sürekli kendini çağıracaktı.
                {
                    layerResult.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı kayıtı başarısız");
                }

            }
            return layerResult;
        }

        public new BusinessLayerResult<EverNoteUser> Update(EverNoteUser data)
        {
            EverNoteUser dbuser = Find(x => x.UserName == data.UserName || x.Email == data.Email);
            BusinessLayerResult<EverNoteUser> res = new BusinessLayerResult<EverNoteUser>();
            res.Result = data;


            if (dbuser != null && dbuser.Id != data.Id) //yani bunu güncelleyen ben miyim 
            {
                if (dbuser.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UserNameAlreadyExists, "Kullanıcı Adı kayıtlı");

                }
                if (dbuser.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E posta adresi kayıtlı");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Password = data.Password;
            res.Result.UserName = data.UserName;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;
            

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı güncellenemedi.");
            }

            return res;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEverNote.Entities.ValueObject
{
    //Bunlar normalde UI katmanındaydı fakat business içinden UI ya erişmemesi lazım. Çünkü zaten UI katmanı business a erişiyor. Business da erişirse UI a hata olmuş olur. Bu yüzden Entity içine koyduk
    public class RegisterViewModel
    {
        [DisplayName("Kullanıcı Adı"),
         Required(ErrorMessage ="{0} alanı boş geçilemez"), 
         StringLength(25,ErrorMessage ="{0} max {1} karakter olamalı")]
        public String UserName { get; set; }

        [DisplayName("E mail"), 
         Required(ErrorMessage = "{0} alanı boş geçilemez"), 
         StringLength(70, ErrorMessage = "{0} max {1} karakter olamalı"),
         EmailAddress(ErrorMessage ="{0} alanı için geçerli bir e-posta alanı giriniz")]
        public String Email { get; set; }

        [DisplayName("Şifre"), 
         Required(ErrorMessage = "{0} alanı boş geçilemez"), 
         DataType(DataType.Password),
         StringLength(50, ErrorMessage = "{0} max {1} karakter olamalı")]
        public String Password { get; set; }

        [DisplayName("Şifre Tekrar"), 
         DataType(DataType.Password),
         Required(ErrorMessage = "{0} alanı boş geçilemez"), 
         StringLength(50, ErrorMessage = "{0} max {1} karakter olamalı"),
         Compare("Password",ErrorMessage ="{0} ile {1} uyuşmuyor.")]
        public String RePassword { get; set; }
    }
}
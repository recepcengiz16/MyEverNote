using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyEverNote.Entities.ValueObject
{
    public class LoginViewModel
    {
        [DisplayName("Kullanıcı Adı"), Required(ErrorMessage = "{0} alanı boş geçilemez"), StringLength(25, ErrorMessage = "{0} max {1} karakter olamalı")]
        //modellerde de dataanotationsu kullanabiliriz.
        public String UserName { get; set; }

        [DisplayName("Şifre"),Required(ErrorMessage = "{0} alanı boş geçilemez"),DataType(DataType.Password),StringLength(50, ErrorMessage = "{0} max {1} karakter olamalı")]
        public String Password { get; set; }
    }
}
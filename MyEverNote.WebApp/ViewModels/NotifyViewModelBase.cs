using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEverNote.WebApp.ViewModels
{
    public class NotifyViewModelBase<T>
    {
        //Bildirim ekranları için oluşturduğumuz temel sınıf. aktif oldunuz , böyle biri yok gibi uyarı sayfaları buradan türeyecek
        public List<T> Items { get; set; } //içindeki mesajları tutan liste
        public String Header { get; set; } // o sayfadaki başlık(Yönlendiriliyorsunuz..)
        public String Title { get; set; }// o sayfanın başlığı(Geçersiz işlem..,İşlem başarılı)
        public Boolean IsRedirecting { get; set; } //yönlendiriliyor mu
        public String RedirectUrl { get; set; }//yönlendrime urlsi
        public int RedirectingTimeOut { get; set; }// şu kadar süre sonra başka sayfaya git

        public NotifyViewModelBase() //varsayılan değerler ile oluşsun
        {
            Header = "Yönlendiriliyorsunuz";
            Title = "Geçersiz İşlem";
            IsRedirecting = true;
            RedirectUrl = "/Home/Index";
            RedirectingTimeOut = 5000;
            Items = new List<T>();
        }

    }
}
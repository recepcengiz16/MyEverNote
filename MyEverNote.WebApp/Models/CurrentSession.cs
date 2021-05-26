using MyEverNote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEverNote.WebApp.Models
{
    public class CurrentSession
    {
        //Sessiondaki veriyi her defasında yazmak yerine böyle biri var mı diye burada sadece get ile alıp gerekli yerde kullanabiliriz
        public static EverNoteUser User
        {
            get 
            {
                return Get<EverNoteUser>("login");
            }
        }

        public static void Set<T>(String key,T obj)
        {
            //verdiğimiz session anahtar ismine verdiğimiz tipte bir objeyi set edelim
            HttpContext.Current.Session[key] = obj;
        }

        public static T Get<T>(String key)
        {
            //Session obje döner biz tip döndürdüğümüz için bu metotu yazdık
            if (HttpContext.Current.Session[key]!=null)
            {
                return (T)HttpContext.Current.Session[key]; 
            }

            return default(T); //eğer int verdiysek 0 döner class verdiysek null döner boolean da false
        }

        public static void Remove(String key)
        {
            if (HttpContext.Current.Session[key] != null)
            {
                HttpContext.Current.Session.Remove(key);
            }
        }

        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }

    }
}
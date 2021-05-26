using MyEverNote.BusinessLayer;
using MyEverNote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace MyEverNote.WebApp.Models
{
    public class CacheHelper
    {
        //cache den okuma yapan metot
        public static List<Category> GetCategoriesFromCache()
        {
            //kategorileri sürekli veritabanından çekip yormak yerine cacheden aldım. Web cache ikullanmak için nughetten yükledim.

            var result = WebCache.Get("category-cache");
            if (result==null)
            {
                CategoryManager categorymanager = new CategoryManager();
                result = categorymanager.List();
                WebCache.Set("category-cache",result,20,true);
            }
            return result;
        }

        public static void RemoveCategoriesFromCache()
        {
            Remove("category-cache");
        }

        public static void Remove(String key)
        {
            WebCache.Remove(key);
        }
    }
}
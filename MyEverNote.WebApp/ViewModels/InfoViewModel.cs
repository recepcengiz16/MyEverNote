using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEverNote.WebApp.ViewModels
{
    public class InfoViewModel:NotifyViewModelBase<String>
    {
        public InfoViewModel()
        {
            Title = "Bilgilendirme";
        }
    }
}
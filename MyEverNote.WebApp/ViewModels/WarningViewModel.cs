using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEverNote.WebApp.ViewModels
{
    public class WarningViewModel:NotifyViewModelBase<String>
    {
        public WarningViewModel()
        {
            Title = "Uyarı!";
        }
    }
}
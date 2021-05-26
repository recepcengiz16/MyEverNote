using MyEverNote.Common;
using MyEverNote.Entities;
using MyEverNote.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyEverNote.WebApp.Init
{
    public class WebCommon : ICommon
    {
        //Data accesslayerın web tarafından veri çekeceği zaman ulaştığı metot.
        public string GetCurrentUserName()
        {
            EverNoteUser user = CurrentSession.User;
            if (user!=null)
            {
                return user.UserName;
            }
            else return "System";
        }

        
    }
}
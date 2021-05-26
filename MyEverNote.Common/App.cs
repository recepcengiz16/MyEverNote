using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEverNote.Common
{
    public static class App
    {
        //Globalasax a bak..
        //dışarıdan erişilen sınıf bu sınıf
        public static ICommon Common = new DefaultCommon();
    }
}

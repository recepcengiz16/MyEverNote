using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEverNote.Common.Helpers
{
    public class ConfigHelper
    {
        //Web congig deki verileri okumak için bu sınıf var.
        //Web setting içindeki anahtar ve değer değerlerinden anahtar değerini vererek value yu elde edebiliyoruz. System.configurationu common katmanında referance olarak ekledim

        public static T Get<T>(String key)
        {
            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key],typeof(T)); //appsetting bize string veri döndürür.Biz belki int isticez o yüzden generic yaptık. T cinsine dönüşsün dedik
        }
    }
}

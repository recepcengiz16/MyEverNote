using MyEverNote.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEverNote.DataAccessLayer.EntityFramework
{
    public class RepositoryBase
    {
        //Birbiriyle ilişkili tablolarda her defasında Databasecontext newlenmesin diye bu sınıfı oluşturduk ve bu sınıf Singletone pattern yapısı oluyor.

        protected static DatabaseContext context;//miras alınan yere de gider
        private static Object _lock = new Object();
        protected RepositoryBase()
        {
            //artık newlenemez. Miras alan bunu newleyebilir demek
            CreateContext();
        }

        private static void CreateContext()
        {
            if(context==null)
            {
                lock (_lock)//herhangi bir tane oluşturduğmuz new lenmiş bir objeyi baz alarak yaar.
                {
                    context = new DatabaseContext();//lock ile aynı anda sadece bir iş yapsın burada tek bir new leme var.
                }
            }
        }
    }
}

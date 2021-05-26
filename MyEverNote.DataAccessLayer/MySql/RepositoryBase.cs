using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEverNote.DataAccessLayer.MySql
{
    public class RepositoryBase
    {
        protected static Object context;//miras alınan yere de gider
        private static Object _lock = new Object();
        protected RepositoryBase()
        {
            //artık newlenemez. Miras alan bunu newleyebilir demek
            CreateContext();
        }

        private static void CreateContext()
        {
            if (context == null)
            {
                lock (_lock)//herhangi bir tane oluşturduğmuz new lenmiş bir objeyi baz alarak yaar.
                {
                    context = new object();//lock ile aynı anda sadece bir iş yapsın burada tek bir new leme var.
                }
            }
        }
    }
}

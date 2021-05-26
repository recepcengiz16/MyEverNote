using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEverNote.Core.DataAccess
{
    public interface IDataAccess<T> //interface i mesela ben mysql de çalışacağım ya da oracle da bu metotlar aynı olsun içindeki kodlar mysql e göre ya da oracle a göre değişebilir. Bu yüzden var.
    {
        List<T> List();//tüm tabloyu listeleyen listemiz
        List<T> List(Expression<Func<T, bool>> where);//ktitere göre getiren
        IQueryable<T> ListQueryable();
        int Insert(T obj);
        int Update(T obj);
        int Delete(T obj);
        int Save();
        T Find(Expression<Func<T, bool>> where);
        
    }
}

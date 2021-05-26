using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using MyEverNote.Entities;
using MyEverNote.Common;
using MyEverNote.Core.DataAccess;

namespace MyEverNote.DataAccessLayer.EntityFramework
{
    public class Repository<T>:RepositoryBase,IDataAccess<T> where T:class // T tipi class olmalı yani int felan girmemeli kısıtladık bu şekilde
    {
        private DbSet<T> _objectset;

        public Repository()//db REpositorybaseden geldi
        {
            _objectset = context.Set<T>();//hangi tabloya olduğunu dbset ile belirleyip ondan sonra ekliyoruz. categories.add() diyorduk ya tipi bilmediğimizden işlemleri set ile yapıyoruz.
                                     //dbseti her metotta yazacağımız için bir kere bu şekilde tanımlıyoruz. Sonra hep bunu çağırıyoruz.
        }

        public List<T> List()//tüm tabloyu listeleyen listemiz
        {
            return _objectset.ToList(); //sana ne tip gelirse o tabloya git ve onu listeye çevirip bana getir. Dbsetin çalışması için new lenen bir nesne olması gerekli. Bu yüzden kısıtladık
        }

        public IQueryable<T> ListQueryable()
        {
            return _objectset.AsQueryable<T>(); //orderbyla ne zaman tolist metotunu çağırdığınızda o zaman sqle gidiyor olacak
        }

        public List<T> List(Expression<Func<T,bool>> where)//ktitere göre getiren
        {
            //eğer bana sıralı getir. ya da ilk 5kayıtı getir diyebilsin dersen List<T> değil de IQueryable<T> olmalı. O zaman da whereden sonra tolist yazmayız
           return _objectset.Where(where).ToList();//where in içinde yazıyorduk ya kriteri x=>x.category diye yukarıda parametrede bu şekilde genel bir sorgulama yapılıyor. Bana bu tip ver bool geri döndereyim.
        }

        public int Insert(T obj)
        {
            _objectset.Add(obj); //hangi tabloya olduğunu dbset ile belirleyip ondan sonra ekliyoruz. categories.add() diyorduk ya tipi bilmediğimizden işlemleri set ile yapıyoruz.

            if (obj is MyEntityBase) //oluşturulma tarhi modifiye tarihi ne zaman onları ayarladık. Liked mesela entitybaseden türemediği için ife girmicek
            {
                MyEntityBase o = obj as MyEntityBase;
                DateTime now = DateTime.Now;

                o.CreatedOn = now;
                o.ModifiedOn = now;
                o.ModifiedUserName = App.Common.GetCurrentUserName(); // o anki kullanıc adını dönecek
            }
            return Save();
        }

        public int Update(T obj)
        {
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;
                o.ModifiedOn = DateTime.Now;
                o.ModifiedUserName = "System";
            }
            return Save();
        }

        public int Delete(T obj)
        {
            _objectset.Remove(obj);
            return Save();
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        public T Find(Expression<Func<T, bool>> where)
        {
            return _objectset.FirstOrDefault(where);
        }
    }
}

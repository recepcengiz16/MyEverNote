using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEverNote.Entities;
using MyEverNote.BusinessLayer.Abstract;

namespace MyEverNote.BusinessLayer
{
    public class CategoryManager: ManagerBase<Category>
    {
        //Bu sadece web olmayabilir ileride windows da olabilir android de. O yüzden delete i override işlemini orada değil de burada yapıyoruz.
        //2 . yöntemde veritabanında diagramı açıp ilişkili olan yerlerde on delete cascade on update cascade i açmak lazım
       //public override int Delete(Category category)
       //{
       //     NoteManager nm = new NoteManager();
       //     LikedManager likedmanager = new LikedManager();
       //     CommentManager cm = new CommentManager();

       //     //Kategori ile ilişkili notların silinmesi
       //     foreach (Note not in category.Notes.ToList())
       //     {
       //         //Note ile ilişkili likeların silinmesi
       //         foreach (Liked like in not.Likes.ToList())
       //         {
       //             likedmanager.Delete(like);
       //         }

       //         //Note ile ilişkili commentlerin silinmesi
       //         foreach (Comment comment in not.Comments.ToList())
       //         {
       //             cm.Delete(comment);
       //         }

       //         nm.Delete(not);
       //     }

       //     return base.Delete(category);
       //}
    }
}

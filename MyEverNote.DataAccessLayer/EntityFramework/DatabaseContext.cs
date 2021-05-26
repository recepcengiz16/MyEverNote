using MyEverNote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEverNote.DataAccessLayer.EntityFramework
{
    public class DatabaseContext:DbContext
    {
        public DbSet<EverNoteUser> EverNoteUsers { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Liked> Likes { get; set; }

        public DatabaseContext()
        {
            Database.SetInitializer(new MyInitializer());
        }

        //Veritabanındaki on delete cascade on update cascade işlemlerini codefirst tarafında yapımı. Override ile onmodelcreatingi eziyoruz.

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelle ilgili işlemleri yapmak için. Dataannotation lar vardı ya boş geçilemez 50 karakter olmalı hepsini burada tanımlayabilyoruz.
            //Fluent Api: Dataannotaionda yaptıklarımızı burada buununla yapabiliriz. Biz orada kısıtlamaları yaptığımız içibn burada sadece cascade işlemlerini yapacağız. 
            //Adımlar:
            //1. İlk önce işlem yapılacak entity elde edilir. Not tablosu
            //2. ilişkili olduğulu property belirlenir. bire çok ilişki var.

            modelBuilder.Entity<Note>()
                .HasMany(n => n.Comments) //note bire çok şeklinde comment ile ilişkili
                .WithRequired(c => c.Note)
                .WillCascadeOnDelete(true); //comment de note ile ilişkili yani bir commentin notu olmalı ihtiyaç duyuyor yani

            modelBuilder.Entity<Note>()
                .HasMany(n => n.Likes) //note bire çok şeklinde like ile ilişkili
                .WithRequired(c => c.Note) //bir likeın mutlaka notu olmalı. eğer boş geçilebilir bir alan olsaydı withoptional diyecektik.
                .WillCascadeOnDelete(true);

        }
    }
}

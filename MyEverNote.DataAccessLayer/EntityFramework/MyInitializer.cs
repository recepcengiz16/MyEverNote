using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MyEverNote.Entities;

namespace MyEverNote.DataAccessLayer.EntityFramework
{
    public class MyInitializer:CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            EverNoteUser admin = new EverNoteUser()
            {
                Name = "Recep",
                Surname = "CENGİZ",
                Email = "recepcengiz_96@outlook.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive=true,
                IsAdmin=true,
                UserName="Mellon_16",
                Password="Orjinaladam_96",
                ProfileImageFileName="profil.png",
                CreatedOn=DateTime.Now,
                ModifiedOn=DateTime.Now.AddMinutes(5),
                ModifiedUserName="Mellon_16"
            };

            EverNoteUser standartUser = new EverNoteUser()
            {
                Name = "Emin",
                Surname = "CENGİZ",
                Email = "emincengiz1998@outlook.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                UserName = "emincengiz",
                Password = "123456",
                ProfileImageFileName="profil.png",
                CreatedOn = DateTime.Now.AddHours(1),
                ModifiedOn = DateTime.Now.AddMinutes(65),
                ModifiedUserName = "Mellon_16"
            };

            context.EverNoteUsers.Add(admin);
            context.EverNoteUsers.Add(standartUser);

            //Fake kullanıcı ekleme
            for (int s = 0; s < 8; s++)
            {
                EverNoteUser user = new EverNoteUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    UserName = $"user{s}",
                    Password = "123",
                    ProfileImageFileName = "profil.png",
                    CreatedOn = DateTime.Now.AddHours(1),
                    ModifiedOn = DateTime.Now.AddMinutes(65),
                    ModifiedUserName = $"user{s}"
                    
                };

                context.EverNoteUsers.Add(user);
            }

            context.SaveChanges();

            List<EverNoteUser> userlist = context.EverNoteUsers.ToList();
            //Fake kategori ekleme
            for (int i = 0; i < 10; i++)
            {
                Category kategori = new Category()
                {
                    Title=FakeData.PlaceData.GetStreetName(),
                    Description=FakeData.PlaceData.GetAddress(),
                    CreatedOn= FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn= FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUserName="Mellon_16"    
                };
                
                context.Categories.Add(kategori);
                // Fake notlar ekleme
                for (int j = 0; j < FakeData.NumberData.GetNumber(5,9); j++)
                {
                    EverNoteUser owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];
                    Note not = new Note()
                    {
                        Title=FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5,25)),
                        Text=FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1,3)),
                        IsDraft=false,
                        LikeCount=FakeData.NumberData.GetNumber(1,9),
                        Owner=owner,
                        CreatedOn=FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1),DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUserName=owner.UserName
                    };
                   
                    kategori.Notes.Add(not);

                    for (int k = 0; k < FakeData.NumberData.GetNumber(3,5); k++)
                    {
                        EverNoteUser comment_owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];
                        Comment yorum = new Comment()
                        {
                            Text=FakeData.TextData.GetSentence(),
                            Owner= comment_owner,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUserName = comment_owner.UserName
                        };

                        not.Comments.Add(yorum);
                    }

                    //Fake like ekleme                  
                    for (int t = 0; t < not.LikeCount; t++)
                    {
                        Liked like = new Liked()
                        { 
                            LikedUser=userlist[t]
                        };

                        not.Likes.Add(like);
                    }
                }
            }

            context.SaveChanges();
        }//seed bitişi
    }
}

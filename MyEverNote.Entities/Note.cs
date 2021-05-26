using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEverNote.Entities
{
    [Table("Notes")]
    public class Note:MyEntityBase
    {
        [DisplayName("Not Başlığı"),Required,StringLength(60)]
        public String Title { get; set; }
        
        [DisplayName("Not Metni"),Required,StringLength(2000)]
        public String Text { get; set; }

        [DisplayName("Taslak")]
        public Boolean IsDraft { get; set; }//Daha taslak mı bu 

        [DisplayName("Beğenilme")]
        public int LikeCount { get; set; }//Like sayısı

        [DisplayName("Kategori")]
        public int CategoryId { get; set; }//ilişkili olduğu nesnenin adı sonra o propun adı. Bu şekilde notlar üzerinden direk kategoriid ye ulaşırız. Öteki türlü notlar üzerinden categoriler den ulaşacaktık extra sql sorgusu olurdu


        public virtual EverNoteUser Owner { get; set; }//Bir notun bir userı olur. Kim yazdı
        public virtual List<Comment> Comments { get; set; }//Birden çok yorumu vardır.
        public virtual Category Category { get; set; }
        public virtual List<Liked> Likes { get; set; }//Bir notun birden çok like ı vardır

        public Note()
        {
            Comments = new List<Comment>();// fakedata eklerken otomatik oluşsun diye yaptık.
            Likes = new List<Liked>();
        }
    }
}

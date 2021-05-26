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
    [Table("EverNoteUsers")]
    public class EverNoteUser:MyEntityBase
    {
        [DisplayName("Ad"),StringLength(25,ErrorMessage ="{0} alanı max. {1} karakter olmalıdır.")]
        public String Name { get; set; }

        [DisplayName("Soyad"),StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public String Surname { get; set; }

        [DisplayName("Kullanıcı Adı"),Required(ErrorMessage ="{0} alanı gereklidir."),StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public String UserName { get; set; }

        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı gereklidir."), StringLength(50, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public String Password { get; set; }

        [DisplayName("E-posta"), Required(ErrorMessage = "{0} alanı gereklidir."), StringLength(70, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public String Email { get; set; }

        [StringLength(30),ScaffoldColumn(false)] //user_12.jpg. Otomatik crud yaparken scaffoldcolumn false ise bu propertyi göstermez hani form tablosunda felan otomatik oluşuyor ya oluşmasın.
        public String ProfileImageFileName { get; set; }

        [DisplayName("Is Active")]
        public Boolean IsActive { get; set; }//Mail ile kullanıcıyı aktif etmek için

        [Required,ScaffoldColumn(false)]
        public Guid ActivateGuid { get; set; }// www.abc.com/user/activate/ndjhf-jnfn-ansa gibi olsun id ler olmasın ki yanlışlıkla millet birbirini aktif edebilir.
        
        [DisplayName("Is Admin")]
        public Boolean IsAdmin { get; set; }

        public virtual List<Note> Notes { get; set; }//Bir kullanıcının birden çok notu olabilir
        public virtual List<Comment> Comments { get; set; }//Bir kullanıcının birden çok yorumu olabilir
        public virtual List<Liked> Likes { get; set; }
    }
}

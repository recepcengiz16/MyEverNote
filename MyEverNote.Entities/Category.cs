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
    [Table("Categories")]
    public class Category:MyEntityBase
    {
        [DisplayName("Kategori"),Required(ErrorMessage ="{0} alanı gereklidir."),StringLength(50,ErrorMessage ="{0} alanı max. {1} karakter olmalıdır.")]
        public String Title { get; set; }

        [DisplayName("Açıklama"),StringLength(150, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public String Description { get; set; }
        public virtual List<Note> Notes { get; set; }//Bir kategoride birden çok not olabilir

        public Category()
        {
            //fakedata eklerken otomatik not listesi de eklensin de hata almayalım.
            Notes = new List<Note>();
        }
    }
}

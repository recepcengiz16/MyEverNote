using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEverNote.Entities
{
    [Table("Comments")]
    public class Comment:MyEntityBase
    {
        [Required,StringLength(300)]
        public String Text { get; set; }

        public virtual Note Note { get; set; }//hangi nota bu yorum eklendi
        public virtual EverNoteUser Owner { get; set; }//Hangi kullanıcı yaptı yorumu
    }
}

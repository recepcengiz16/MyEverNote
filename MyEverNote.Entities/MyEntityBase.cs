using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEverNote.Entities
{
    public class MyEntityBase
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }//Bu üçü ne zaman oluşturuldu ne zaman güncellendi kim tarafından bu işlemler yapıldı bunun için var. 
        
        [Required]
        public DateTime ModifiedOn { get; set; }

        [Required,StringLength(30)]
        public String ModifiedUserName { get; set; }
    }
}

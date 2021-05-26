using MyEverNote.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEverNote.BusinessLayer.Results
{
    public class BusinessLayerResult<T> where T:class
    {
        public List<ErrorMesageObject> Errors { get; set; } //hata mesajları için
        public T Result { get; set; } //hata yoksa sonucu bu property şeklinde dön demiş olduk

        public BusinessLayerResult()
        {
            Errors = new List<ErrorMesageObject>(); // hata mesajları yoksa hata alırız. Bunun için birliste oluşturmamız gerekli 
        }

        public void AddError(ErrorMessageCode code,String message)
        {
            Errors.Add(new ErrorMesageObject() { Code = code, Message = message });
        }
    }
}

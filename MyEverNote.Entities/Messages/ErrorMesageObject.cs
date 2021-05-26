using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEverNote.Entities.Messages
{
    public class ErrorMesageObject
    {
        public ErrorMessageCode Code { get; set; }
        public String Message { get; set; }
    }
}

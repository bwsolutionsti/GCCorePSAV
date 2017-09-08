using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GCCorePSAV.Models
{
    public class ContactTypeModel
    {
[Display(Prompt ="ID Tipo Contacto")]        
        public string ID { get; set; }
        [Display(Prompt ="Tipo Contacto")]
        public string ContactTypeD { get; set; }
        [Display(Prompt ="Descripción")]
        public string ContactTypeDesc { get; set; }
    }
}

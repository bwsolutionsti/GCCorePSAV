using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GCCorePSAV.Models
{
    public class PersonModel
    {
        [Display(Prompt = "Nombre(s)")]
        public string Names { get; set; }
        [Display(Prompt = "Primer Apellido")]
        public string FirstLastName { get; set; }
        [Display(Prompt = "Segundo Apellido")]
        public string SecondLastName { get; set; }
        [Display(Prompt ="Fecha Nacimiento")]
        [DataType(DataType.Date)]
        public string DOB { get; set; }
    }
}

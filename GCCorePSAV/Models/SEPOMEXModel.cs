using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GCCorePSAV.Models
{
    public class SEPOMEXModel
    {
        [Display(Prompt ="Código Postal")]
        [DataType(DataType.PostalCode)]
        public string Zipcode { get; set; }
        [Display(Prompt ="Colonia")]
        public string Suburb { get; set; }
        [Display(Prompt ="Tipo Colonia")]
        public string SuburbType { get; set; }
        [Display(Prompt ="Estado")]
        public string State { get; set; }
        [Display(Prompt ="Municipio")]
        public string Town { get; set; }
        [Display(Prompt ="Ciudad")]
        public string City { get; set; }

    }
}

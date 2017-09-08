using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GCCorePSAV.Models.PSAVCrud
{
    public class UsuariosConsulta
    {
        public class UsuarioBuscar
        {
            [Display(Prompt = "Consulta usuario...")]
            public string UserFind { get; set; }
        }
    }
}

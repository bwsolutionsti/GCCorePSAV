using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCCorePSAV.Models.PSAVCrud.Returning
{
    public class UsuariosReturnModel
    {
        public string Usuario { get; set; }
        public string NombreCompleto { get; set; }
        public string Expira { get; set; }
        public string Activo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCCorePSAV.Models.PSAVCrud
{
    public class EmployeesModel
    {
        public class SearchEmployee
        {
            public string EmployeeSearch { get; set; }
        }
        public class EmployeeList
        {
            public string NombreCompleto { get; set; }
            public string Puesto { get; set; }
            public string Activo { get; set; }
            public string LOB { get; set; }
        }
        public class NewEmployee
        {
            public string PrimerNombre { get; set; }
            public string SegundoNombre { get; set; }
            public string PrimerApellido { get; set; }
            public string SegundoApellido { get; set; }
            public string DOB { get; set; }
            public string RFC { get; set; }
            public string CURP { get; set; }
            public string LOB { get; set; }
            public string EmployeeType { get; set; }
        }
    }
}

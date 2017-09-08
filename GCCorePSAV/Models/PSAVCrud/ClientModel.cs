using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GCCorePSAV.Models.PSAVCrud
{
    public class ClientModel
    {
        public class ClientQuickSearch
        {
            [Display(Prompt = "Cliente")]
            public string FullName { get; set; }
        }
        public class ClientViewData
        {
            public string RazonSocial { get; set; }
            public string TaxingAddress { get; set; }
            public string RFC { get; set; }
        }
        public class ClientDetails
        {

        }
        public class ClientPerson
        {

        }
        public class ClientContact
        {

        }
        public class ClientAutoComplete
        {
            public string FullName { get; set; }
            public string IDClient { get; set; }
        }
        public class ClientSearch
        {
            public string IDClient { get; set; }
            public string Razon { get; set; }
            public string Domicilio { get; set; }
            public string Fiscal { get; set; }
            public string RFC { get; set; }
            public string Tel { get; set; }
            public string Cel { get; set; }
            public string Email { get; set; }
            public string Ext { get; set; }
            public string NombreContacto { get; set; }
            public string IDEmpresa { get; set; }
        }
        public class ClientNewAll
        {
            public string PrimerNombre { get; set; }
            public string SegundoNombre { get; set; }
            public string PrimerApellido { get; set; }
            public string SegundoApellido { get; set; }
            public string RazonSocial { get; set; }
            public string DOB { get; set; }
            public string RFC { get; set; }
            public string NombreComercial { get; set; }
            public string Telefono { get; set; }
            public string Extension { get; set; }
            public string Celular { get; set; }
            public string EMail { get; set; }
            public string NombreContacto { get; set; }
            public string Domicilio { get; set; }
            public string DomicilioFiscal { get; set; }
        }
    }
}

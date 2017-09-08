using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GCCorePSAV.Models
{
    
    public class EPTModel
    {
        [Display(Prompt ="# EPT")]
        [Required]
        public string EPTNumber { get; set; }
        [Display(Prompt ="Cliente")]
        public string IDClient { get; set; }
        [Display(Prompt ="Razón Social")]
        [DataType(DataType.MultilineText)]
        public string RazonSocial { get; set; }
        [Display(Prompt ="Domicilio")]
        [DataType(DataType.MultilineText)]
        public string DomicilioComercial { get; set; }
        [Display(Prompt ="Domicilio Fiscal")]
        [DataType(DataType.MultilineText)]
        public string DomicilioFiscal { get; set; }
        [Display(Prompt ="R.F.C.")]
        public string RFC { get; set; }
        [Display(Prompt ="Nombre de contacto")]
        public string ContactClientName { get; set; }
        [Display(Prompt = "Puesto")]
        public string Job { get; set; }
        [Display(Prompt = "Teléfono")]
        public string Phone { get; set; }
        [Display(Prompt = "Ext.")]
        public string Ext { get; set; }
        [Display(Prompt = "Móvil")]
        public string Mobile { get; set; }
        [Display(Prompt = "Email")]
        public string Email { get; set; }
        [Display(Prompt = "Email Alterno")]
        public string AEMail { get; set; }
        [Display(Prompt = "Fax")]
        public string Fax { get; set; }
        [Display(Prompt = "Nombre del evento")]
        
        public string EventName { get; set; }
        [Display(Prompt = "Fecha de Montaje")]
        [DataType(DataType.Date)]
        public string MountDate { get; set; }
        [Display(Prompt = "Hora de Montaje")]
        [DataType(DataType.Time)]
        public string MountHour { get; set; }
        [Display(Prompt = "Contacto en sitio")]
        public string PlaceContact { get; set; }
        [Display(Prompt = "Celular")]
        public string PlaceMobile { get; set; }
        [Display(Prompt = "Fecha de inicio")]
        [DataType(DataType.Date)]
        public string StartDate { get; set; }
        [Display(Prompt = "Hora de inicio")]
        [DataType(DataType.Time)]
        public string StartHour { get; set; }
        [Display(Prompt = "Fecha de termino")]
        [DataType(DataType.Date)]
        public string FinishDate { get; set; }
        [Display(Prompt = "Hora de termino")]
        [DataType(DataType.Time)]
        public string FinishHour { get; set; }
        [Display(Prompt = "Lugar")]
        public string Place { get; set; }
        [Display(Prompt = "Dirección")]
        public string Address { get; set; }
        [Display(Prompt = "Nombre")]
        public string SMName { get; set; }
        [Display(Prompt = "Puesto")]
        public string SMJob { get; set; }
        [Display(Prompt = "Email")]
        public string SMEmail { get; set; }
        [Display(Prompt = "Telefono")]
        public string SMPhone { get; set; }
        [Display(Prompt = "Nombre")]
        public string PMName { get; set; }
        [Display(Prompt = "Celular")]
        public string PMMobile { get; set; }
        [Display(Prompt = "Email")]
        public string PMEmail { get; set; }
        [Display(Prompt = "Ubicación")]
        public string PMLocation { get; set; }
        public string IDEmpresa { get; set; }
        public List<Models.PSAVCrud.ClientModel.ClientAutoComplete> Clients { get; set; }
        public string IDEvent { get; set; }
        public EPTModel()
        {
            Data.clsQuery con = new Data.clsQuery();
             Clients = con.GetAutoClients();
        }
        public class EPTConsult
        {
            public string FolioEPT { get; set; }
            public string EventName { get; set; }
            public string Client { get; set; }
            public string StartDate { get; set; }
        }
    }
}

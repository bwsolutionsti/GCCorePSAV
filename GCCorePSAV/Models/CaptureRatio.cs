using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GCCorePSAV.Models
{
    public class CaptureRatio
    {
        public class CRVResumenModel
        {
            public string IDCratio { get; set; }
            public string HotelName { get; set; }
            public string LocationHotel { get; set; }
            public string CityLoc { get; set; }
            public string DET { get; set; }
            public string Contact { get; set; }
            public string FillFormName { get; set; }
        }
        public class CRVModel
        {
            [Display(Prompt ="Mes / Año MM/YY")]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString ="mm/yy")]
            public string MesOp { get; set; }
            [Display(Prompt ="Hotel")]
            public string HotelName { get; set; }
            [Display(Prompt = "Ciudad")]
            public string City { get; set; }
            [Display(Prompt = "Location Fecha")]
            public string FechaLoc { get; set; }
            [Display(Prompt = "DET")]
            public string DET { get; set; }
            [Display(Prompt = "Contacto DET")]
            public string ContactDET { get; set; }
            [Display(Prompt = "Quien llena")]
            public string NameFillForm { get; set; }
            [Display(Prompt ="Fecha Inicio")]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "mm/yy")]
            public string Finicio { get; set; }
            [Display(Prompt = "Fecha Fin")]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "mm/yy")]
            public string FFin { get; set; }
            [Display(Prompt ="Nombre del Evento")]
            public string EventName { get; set; }
            [Display(Prompt = "Cliente Final")]
            public string FinalClient { get; set; }
            [Display(Prompt = "Nombre del Contacto")]
            public string ContactName { get; set; }
            [Display(Prompt = "Contacto")]
            public string ContactContact { get; set; }
            [Display(Prompt = "Agencia")]
            public string Agency { get; set; }
            [Display(Prompt = "Tipo de Evento")]
            public string EventType { get; set; }
            [Display(Prompt = "Empresa AV")]
            public string EmpresaAV { get; set; }
            [Display(Prompt = "Responsable PSAV")]
            public string RsponsablePSAV { get; set; }
            [Display(Prompt = "Venta PSAV")]
            public string VentasPSAV { get; set; }
            [Display(Prompt = "Lost Business")]
            public string LB { get; set; }
            [Display(Prompt = "Suma")]
            public string Suma { get; set; }
            [Display(Prompt = "Capture Ratio")]
            public string CRatio { get; set; }
            [Display(Prompt = "Comisión Hotel")]
            public string HotelFee { get; set; }
            [Display(Prompt = "Causa Lost Business")]
            public string LBCause { get; set; }
            [Display(Prompt = "Fecha Siguiente Evento")]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "mm/yy")]
            public string NextEventDate { get; set; }
            [Display(Prompt = "Lugar Siguiente Evento")]
            public string NextEventPlace { get; set; }
            public List<CaptureRatio.CRatioList> CRLit { get; set; }
            public string IDCRAtio { get; set; }
        }
        public class CRatioList
        {
            [Display(Prompt = "Fecha Inicio")]
            [DataType(DataType.Date)]
            public string Finicio { get; set; }
            [Display(Prompt = "Fecha Fin")]
            [DataType(DataType.Date)]
            public string FFin { get; set; }
            [Display(Prompt = "Nombre del Evento")]
            public string EventName { get; set; }
            [Display(Prompt = "Cliente Final")]
            public string FinalClient { get; set; }
            [Display(Prompt = "Nombre del Contacto")]
            public string ContactName { get; set; }
            [Display(Prompt = "Contacto")]
            public string ContactContact { get; set; }
            [Display(Prompt = "Agencia")]
            public string Agency { get; set; }
            [Display(Prompt = "Tipo de Evento")]
            public string EventType { get; set; }
            [Display(Prompt = "Empresa AV")]
            public string EmpresaAV { get; set; }
            [Display(Prompt = "Responsable PSAV")]
            public string RsponsablePSAV { get; set; }
            [Display(Prompt = "Venta PSAV")]
            public string VentasPSAV { get; set; }
            [Display(Prompt = "Lost Business")]
            public string LB { get; set; }
            [Display(Prompt = "Suma")]
            public string Suma { get; set; }
            [Display(Prompt = "Capture Ratio")]
            public string CRatio { get; set; }
            [Display(Prompt = "Comisión Hotel")]
            public string HotelFee { get; set; }
            [Display(Prompt = "Causa Lost Business")]
            public string LBCause { get; set; }
            [Display(Prompt = "Fecha Siguiente Evento")]
            public string NextEventDate { get; set; }
            [Display(Prompt = "Lugar Siguiente Evento")]
            public string NextEventPlace { get; set; }
            public string MesFiltro { get; set; }
        }
        public class GetFormatCRL
        {
            public List<CRatioList> CRLL { get; set; }
        }
    }
}

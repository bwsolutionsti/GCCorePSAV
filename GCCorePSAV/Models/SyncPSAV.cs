using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GCCorePSAV.Models
{
    public class SyncPSAV
    {
        public class CratioDets
        {
            public string MonthOp { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string EventName { get; set; }
            public string IDClient { get; set; }
            public string ContactName { get; set; }
            public string ContactContact { get; set; }
            public string Agency { get; set; }
            public string IDEventType { get; set; }
            public string EmpresaAV { get; set; }
            public string ResponsableVtas { get; set; }
            public string VtasPSAV { get; set; }
            public string LB { get; set; }
            public string Suma { get; set; }
            public string CRatio { get; set; }
            public string HotelFee { get; set; }
            public string LBCause { get; set; }
            public string NextEventDate { get; set; }
            public string NextEventPlace { get; set; }
            public string IDCratio { get; set; }
            public string IDCratioDets { get; set; }
            public string ID { get; set; }
        }
        public class ItemListServices
        {
            public int IDEvento { get; set; }
            [Key]
            public string ID { get; set; }
            public string IDITL { get; set; }
            public string Clave { get; set; }
            public string Cantidad { get; set; }
            public string Dias { get; set; }
            public string Descripcion { get; set; }
            public string PrecioUnit { get; set; }
            public string Categoria { get; set; }
        }
        public class ItemListWorkForce
        {
            public int IDEvento { get; set; }
            public string ID { get; set; }
            public string IDITL { get; set; }
            public string Seccion { get; set; }
            public string Clave { get; set; }
            public string Cantidad { get; set; }
            public string Dias { get; set; }
            public string Descripcion { get; set; }
            public string PrecioUnit { get; set; }
            public string Categoria { get; set; }
        }
        public class VentaDes
        {
            public string Category { get; set; }
            public string VentaEqui { get; set; }
            public string VentaEquEx { get; set; }
            public string TotalVenta { get; set; }
            public string DesPorEq { get; set; }
            public string TotalDescEPS { get; set; }
            public string DescExt { get; set; }
            public string TotalExt { get; set; }
            public string TotalDesc { get; set; }
            public string PorcTotalDesc { get; set; }
            public string AplicaAut { get; set; }
        }
        public class VentaFee
        {
            public string Category { get; set; }
            public string BaseFee { get; set; }
            public string PorcFee { get; set; }
            public string SubFee { get; set; }
            public string ImporteFee { get; set; }
        }
        public class SubRenta
        {
            public string Category { get; set; }
            public string Invoice { get; set; }
            public string Supplier { get; set; }
            public string Descripcion { get; set; }
            public string Gastosub { get; set; }
            public string Ventasub { get; set; }
        }
        public class FreelanceOL
        {
            public string Nombres { get; set; }
            public string Puesto { get; set; }
            public string Dias { get; set; }
            public string Sueldo { get; set; }
            public string Condiciones { get; set; }
            public string CostoCarga { get; set; }
            public string CostoTotal { get; set; }
        }
        public class Viaticos
        {
            public string Nombres { get; set; }
            public string Puesto { get; set; }
            public string Observaciones { get; set; }
            public string TotalSol { get; set; }
        }
        public class VentasFeeTot
        {
            public string Nombres { get; set; }
            public string Puesto { get; set; }
            public string Comision { get; set; }
            public string VentaNeta { get; set; }
            public string Comisiontot { get; set; }
        }
        public class GastosFinancieros
        {
            public string Comision { get; set; }
            public string Importe { get; set; }
            public string PorcCom { get; set; }
            public string ImporteCom { get; set; }
        }
        public class Consumibles
        {
            public string Cotizacion { get; set; }
            public string Supplier { get; set; }
            public string Description { get; set; }
            public string Costo { get; set; }
        }
        public class CargosInternos
        {
            public string Equipo { get; set; }
            public string Categoria { get; set; }
            public string PrecioLista { get; set; }
            public string TipoOper { get; set; }
            public string PorcCargo { get; set; }
            public string MontoCargo { get; set; }
        }
        public class SalonIL
        {
            public string ID { get; set; }
            public string IDEvt { get; set; }
            public string Salon { get; set; }
            public string Asistentes { get; set; }
            public string Montaje { get; set; }
            public string Horario { get; set; }
            public string IDITL { get; set; }
            public string EventoName { get; set; }
        }
        public class SalonILWF
        {
            public string ID { get; set; }
            public string IDITL { get; set; }
            public string IDEvt { get; set; }
            public string Salon { get; set; }
            public string Asistentes { get; set; }
            public string Montaje { get; set; }
            public string Horario { get; set; }
        }
    }
}

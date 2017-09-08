using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GCCorePSAV.Models
{
    public class ItemListModel
    {
        public class ItemListEventModel
        {
            public EPTModel EPTLoad { get; set; }
            [Display(Prompt ="Nombre del evento")]
            [Required]
            public string EventoName { get; set; }
            [Display(Prompt ="Salón/Area")]
            public string Salon { get; set; }
            [Display(Prompt ="# Asistentes")]
            public string Asistentes { get; set; }
            [Display(Prompt ="Montaje")]
            public string Montaje { get; set; }
            [Display(Prompt ="Horario")]
            public string Horario { get; set; }
            public int IDEvento { get; set; }
            [Display(Prompt ="Clave Servicio")]
            public string Clave { get; set; }
            [Display(Prompt ="Cantidad")]
            public string Cantidad { get; set; }
            [Display(Prompt ="Dias")]
            public string Dias { get; set; }
            [Display(Prompt ="Descripcion")]
            public string Descripcion { get; set; }
            [Display(Prompt ="Precio Unitario")]
            public string PrecioUnit { get; set; }
            public List<Models.ItemListModel.ItemListServices> ServList { get; set; }
            public string Seccion { get; set; }
            public string Categoria { get; set; }
        }
        public class ItemListServices
        {
            public int IDEvento { get; set; }
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
            public string Seccion { get; set; }
            public string Clave { get; set; }
            public string Cantidad { get; set; }
            public string Dias { get; set; }
            public string Descripcion { get; set; }
            public string PrecioUnit { get; set; }
            public string Categoria { get; set; }
        }
        public class CategoryItemList
        {
            public string ID { get; set; }
            public string Catego { get; set; }
        }
    }
}

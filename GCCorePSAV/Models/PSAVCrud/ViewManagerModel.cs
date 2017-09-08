using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GCCorePSAV.Models.PSAVCrud
{
    public class ViewManagerModel
    {
        public class ViewManagerList
        {
            public string VMName { get; set; }
        }
        public class ViewManagerSave
        {
            [Display(Prompt ="Nombre de Control")]
            [Required]
            public string VMName { get; set; }
            [Display(Prompt ="Descripción de control")]
            [Required]
            public string VMDesc { get; set; }
            [Display(Prompt ="Controlador")]
            [Required]
            public string VMController { get; set; }
            [Display(Prompt ="Vista")]
            [Required]
            public string VMAction { get; set; }
        }
        public class ViewManagerSearch
        {
            [Display(Prompt ="Vista")]
            public string FullName { get; set; }
        }
    }
}

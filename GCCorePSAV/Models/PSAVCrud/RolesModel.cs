using jQWidgets.AspNetCore.Mvc.TagHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace GCCorePSAV.Models.PSAVCrud
{
    public class RolesModel
    {
        [Key]
        public string ID
        {
            get;
            set;
        }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
    public class RolesListView
    {
        public List<RolesModel> LRM { get; set; }
    }
}

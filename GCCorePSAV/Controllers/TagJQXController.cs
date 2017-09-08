using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using jQWidgets.AspNetCore.Mvc.TagHelpers;
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GCCorePSAV.Controllers
{
    public class TagJQXController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
#region RolesCrud
        public class JsonDataRoles
        {
            public List<Models.PSAVCrud.RolesModel> RolesCrud { get; set; }
        }
        [HttpPost]
        public IActionResult EditRoles(string jsondata)
        {
            Models.PSAVCrud.RolesModel modRoles = (Models.PSAVCrud.RolesModel)JsonConvert.DeserializeObject(jsondata, typeof(Models.PSAVCrud.RolesModel));

            return View();   
        }
#endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.JavaScript;
using GCCorePSAV.Models;

namespace GCCorePSAV.Controllers
{
    public class PSAVCrudController : Controller
    {
        Data.clsQuery ConSQL = new Data.clsQuery();
        // GET: /<controller>/
        //public IActionResult Usuarios()
        //{
        //    List<Models.PSAVCrud.Returning.UsuariosReturnModel> URMList = ConSQL.SearchUsers(string.Empty);
        //    ViewBag.URMList = URMList;
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> Usuarios(Models.PSAVCrud.UsuariosConsulta.UsuarioBuscar model)
        //{

        //    List<Models.PSAVCrud.Returning.UsuariosReturnModel> URMList = ConSQL.SearchUsers(model.UserFind);
        //    ViewBag.URMList = URMList;
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult EditUser()
        //{
        //    return View();
        //}
        //public IActionResult Coins()
        //{
        //    List<Models.PSAVCrud.CoinsModel> CoinsList = ConSQL.GetCoin(string.Empty);
        //    ViewBag.CoinsList = CoinsList;
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> Coins(Models.PSAVCrud.CoinsModel model)
        //{
        //    List<Models.PSAVCrud.CoinsModel> CoinsList = ConSQL.GetCoin(model.CoinName);
        //    ViewBag.CoinsList = CoinsList;
        //    return View();
        //}
        //[HttpGet]
        //public IActionResult NewCoin()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public async Task<IActionResult> NewCoin(Models.PSAVCrud.CoinsModel model)
        //{
        //    return View();
        //}
        public IActionResult ViewsManager()
        {
            List<Models.PSAVCrud.ViewManagerModel.ViewManagerList> VMList = ConSQL.GetVMList(string.Empty);
            ViewBag.VMList = VMList;
            return View();
        }
        [HttpPost]
        public IActionResult ViewsManager(Models.PSAVCrud.ViewManagerModel.ViewManagerSearch model)
        {
            List<Models.PSAVCrud.ViewManagerModel.ViewManagerList> VMList = ConSQL.GetVMList(model.FullName);
            ViewBag.VMList = VMList;
            return View();
        }
        public IActionResult NewVM()
        {
            return View();
        }
        public IActionResult Clients(Models.PSAVCrud.ClientModel.ClientQuickSearch model)
        {
            List<Models.PSAVCrud.ClientModel.ClientSearch> ClientList = ConSQL.GetClients(model.FullName);
            ViewBag.CoinsList = ClientList;
            return View();
        }
        public IActionResult ContactType(Models.ContactTypeModel model)
        {
            return View();
        }
        //public IActionResult Roles()
        //{
        //    List<Models.PSAVCrud.RolesModel> RM = ConSQL.GetRoles();
        //    ViewData["Theme"] = "light";
        //    return View(RM.ToList());
        //}
        public IActionResult NewClient()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> NewClientPost(Models.PSAVCrud.ClientModel.ClientNewAll model)
        {
            if (ModelState.IsValid)
            {
                ConSQL.SaveClient(model);
                return View("Clients");
            }
            else
            {
                return View();
            }
        }
        #region NewSyncFCrud
        #region Users
        public static List<Models.PSAVCrud.SyncModels.UsersModel> UsersList = new List<Models.PSAVCrud.SyncModels.UsersModel>();
        public ActionResult Usuarios()
        {
            if (UsersList.Count.Equals(0))
                BindDataSourceUsers();
            ViewBag.datasource = UsersList;
            return View();
        }
        public void BindDataSourceUsers() { UsersList = ConSQL.GetUsers(); }
        public ActionResult UserNormalUpdate([FromBody]CRUDModel<Models.PSAVCrud.SyncModels.UsersModel> myObject)
        {
            var ord = myObject.Value;
            Models.PSAVCrud.SyncModels.UsersModel val = UsersList.Where(or => or.ID == ord.ID).FirstOrDefault();
            val.ID = ord.ID; val.Active = ord.Active; val.Username = ord.Username; val.Expira = ord.Expira;
            ConSQL.UpdateUsers(val, 1);
            return Json(myObject.Value);
        }
        public ActionResult UserNormalInsert([FromBody]CRUDModel<Models.PSAVCrud.SyncModels.UsersModel> value)
        {
            Models.PSAVCrud.SyncModels.UsersModel val = value.Value;
            val.ID = ConSQL.UpdateUsers(value.Value, 0);
            UsersList.Insert(CoinsList.Count, val);
            return Json(CoinsList);
        }
        public ActionResult UserNormalDelete([FromBody]CRUDModel<Models.PSAVCrud.SyncModels.UsersModel> value)
        {
            UsersList.Remove(UsersList.Where(or => or.ID == value.Key.ToString()).FirstOrDefault());
            ConSQL.UpdateUsers(value.Value, 2);
            return Json(value);
        }
        #endregion
        #region Coins
        public static List<Models.PSAVCrud.SyncModels.CoinModel> CoinsList = new List<Models.PSAVCrud.SyncModels.CoinModel>();
        public ActionResult Coins()
        {
            if (CoinsList.Count.Equals(0))
                BindDataSourceCoins();
            ViewBag.datasource = CoinsList;
            return View();
        }
        public void BindDataSourceCoins() { CoinsList= ConSQL.GetCoins(); }
        public ActionResult CoinNormalUpdate([FromBody]CRUDModel<Models.PSAVCrud.SyncModels.CoinModel> myObject)
        {
            var ord = myObject.Value;
            Models.PSAVCrud.SyncModels.CoinModel val = CoinsList.Where(or => or.ID == ord.ID).FirstOrDefault();
            val.ID = ord.ID; val.Name = ord.Name; val.Change = ord.Change; val.Active=ord.Active;
            ConSQL.UpdateCoins(val, 1);
            return Json(myObject.Value);
        }
        public ActionResult CoinNormalInsert([FromBody]CRUDModel<Models.PSAVCrud.SyncModels.CoinModel> value)
        {
            Models.PSAVCrud.SyncModels.CoinModel val = value.Value;
            val.ID = ConSQL.UpdateCoins(value.Value, 0);
            CoinsList.Insert(CoinsList.Count, val);
            return Json(CoinsList);
        }
        public ActionResult CoinNormalDelete([FromBody]CRUDModel<Models.PSAVCrud.SyncModels.CoinModel> value)
        {
            Models.PSAVCrud.SyncModels.CoinModel val = new Models.PSAVCrud.SyncModels.CoinModel();
            val.ID = value.Key.ToString();
            ConSQL.UpdateCoins(val, 2);
            CoinsList.Remove(CoinsList.Where(or => or.ID == value.Key.ToString()).FirstOrDefault());
            return Json(value);
        }
        #endregion
        #region Roles
        public static List<Models.PSAVCrud.RolesModel> RolesList = new List<Models.PSAVCrud.RolesModel>();
        public ActionResult Roles()
        {
            if(RolesList.Count.Equals(0))
                BindDataSourceRoles();
            ViewBag.datasource = RolesList;
            return View();
        }
        public ActionResult RoleNormalUpdate([FromBody]CRUDModel<Models.PSAVCrud.RolesModel> myObject)
        {
            var ord = myObject.Value;
            Models.PSAVCrud.RolesModel val = RolesList.Where(or => or.ID == ord.ID).FirstOrDefault();
            val.ID = ord.ID;val.Nombre = ord.Nombre;val.Descripcion = ord.Descripcion;
            ConSQL.UpdateRole(val,1);
            return Json(myObject.Value);
        }
        public ActionResult RoleNormalInsert([FromBody]CRUDModel<Models.PSAVCrud.RolesModel> value)
        {
            Models.PSAVCrud.RolesModel val = value.Value;
            val.ID= ConSQL.UpdateRole(value.Value, 0);
            RolesList.Insert(RolesList.Count, val);            
            return Json(RolesList);
        }
        public ActionResult RoleNormalDelete([FromBody]CRUDModel<Models.PSAVCrud.RolesModel> value)
        {
            RolesList.Remove(RolesList.Where(or => or.ID == value.Key.ToString()).FirstOrDefault());
            ConSQL.UpdateRole(value.Value, 2);
            return Json(value);
        }
        public void BindDataSourceRoles()
        {
            RolesList = ConSQL.GetRoles();
        }
#endregion
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.JavaScript;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.IO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GCCorePSAV.Controllers
{

    public class PSAVController : Controller
    {
        // GET: /<controller>/
        Data.clsQuery ConSQL = new Data.clsQuery();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult EPT()
        {
            ViewBag.CRList = ConSQL.EPTSearch();
            return View();
        }
        [HttpPost]
        public IActionResult EPT(string ac1)
        {
            ViewBag.CRList = ConSQL.EPTSearchOne(ac1);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EPT1(object model)
        {
            return View();
        }
        public IActionResult NewEPT()
        {
            List<Models.PSAVCrud.CoinsModel> CoinsList = ConSQL.GetCoin(string.Empty);
            List<Models.PSAVCrud.ClientModel.ClientAutoComplete> ClientList = ConSQL.GetAutoClients();
            ViewBag.ClientList = ClientList;
            ViewBag.CoinsList = CoinsList;
            return View(new Models.EPTModel());
        }
        [HttpPost]
        public IActionResult NewEPT(string ac1, Models.EPTModel model, string NewValue, string folio)
        {
            if (NewValue.Equals("0"))
            {
                Models.EPTModel eptM = ConSQL.GetEPTToEdit(folio);
                List<Models.PSAVCrud.CoinsModel> CoinsList = ConSQL.GetCoin(string.Empty);
                List<Models.PSAVCrud.ClientModel.ClientAutoComplete> ClientList = ConSQL.GetAutoClients();
                ViewBag.ClientList = ClientList;
                ViewBag.CoinsList = CoinsList;
                List<Models.PSAVCrud.ClientModel.ClientSearch> Clients = ConSQL.GetClientID(eptM.IDClient);
                for (int i = 0; i < Clients.Count; i++)
                {
                    eptM.DomicilioComercial = Clients[i].Domicilio;
                    eptM.DomicilioFiscal = Clients[i].Fiscal;
                    eptM.IDClient = Clients[i].IDClient;
                    eptM.RazonSocial = Clients[i].Razon;
                    eptM.Phone = Clients[i].Tel;
                    eptM.Mobile = Clients[i].Cel;
                    eptM.Email = Clients[i].Email;
                    eptM.RFC = Clients[i].RFC;
                    eptM.Ext = Clients[i].Ext;
                    eptM.ContactClientName = Clients[i].NombreContacto;
                }
                return View(eptM);
            }
            else
            {
                if (!string.IsNullOrEmpty(ac1))
                {
                    List<Models.PSAVCrud.CoinsModel> CoinsList = ConSQL.GetCoin(string.Empty);
                    List<Models.PSAVCrud.ClientModel.ClientAutoComplete> ClientList = ConSQL.GetAutoClients();
                    ViewBag.ClientList = ClientList;
                    ViewBag.CoinsList = CoinsList;
                    List<Models.PSAVCrud.ClientModel.ClientSearch> Clients = ConSQL.GetClient(ac1);
                    Models.EPTModel ept = new Models.EPTModel();
                    for (int i = 0; i < Clients.Count; i++)
                    {
                        ept.DomicilioComercial = Clients[i].Domicilio;
                        ept.DomicilioFiscal = Clients[i].Fiscal;
                        ept.IDClient = Clients[i].IDClient;
                        ept.RazonSocial = Clients[i].Razon;
                        ept.Phone = Clients[i].Tel;
                        ept.Mobile = Clients[i].Cel;
                        ept.Email = Clients[i].Email;
                        ept.RFC = Clients[i].RFC;
                        ept.Ext = Clients[i].Ext;
                        ept.ContactClientName = Clients[i].NombreContacto;
                        ept.IDEmpresa = Clients[i].IDEmpresa;
                    }
                    Response.Cookies.Append("IDC", ept.IDClient, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                    Response.Cookies.Append("IDCE", ept.IDEmpresa, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                    return View(ept);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        //save model
                        model.IDClient = Request.Cookies["IDC"].ToString();
                        model.IDEmpresa = Request.Cookies["IDCE"].ToString();
                        string IDEvent = ConSQL.InsertEPT(model);
                        ViewBag.EPTM = model;
                        Models.ItemListModel.ItemListEventModel Ilem = new Models.ItemListModel.ItemListEventModel();
                        Ilem.EPTLoad = ViewBag.EPTM;
                        Response.Cookies.Append("IDE", IDEvent, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                        Response.Cookies.Append("EVN", model.EventName, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                        ViewBag.CategoriaList = ConSQL.GetCategoryItemList(1);
                        return RedirectToAction("ItemList");
                    }
                    else
                    {
                        return View();
                    }
                }
            }
        }
        [HttpPost]
        public IActionResult NewEPTs(Models.EPTModel model)
        {
            if (ModelState.IsValid)
            {
                return View("ItemList");
            }
            else
            {
                return View();
            }
        }
        public List<Models.ItemListModel.ItemListServices> serv { get; set; }
        #region itemlists
        #region Itemlist
        #region editItemlist
        [HttpPost]
        public IActionResult EditItemList(string IDIL, string Advance, Models.SyncPSAV.SalonIL model, string EVT)
        {
            if (string.IsNullOrEmpty(Advance))
            {
                if (string.IsNullOrEmpty(IDIL))
                {
                    Models.SyncPSAV.SalonIL ILS = new Models.SyncPSAV.SalonIL();
                    ServList = new List<Models.SyncPSAV.ItemListServices>();
                    ViewBag.datasource = ServList;
                    Response.Cookies.Append("IDEVNN", ConSQL.GetEPTToEdit(EVT).IDEvent, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                    return View(ILS);
                }
                else
                {
                    Models.SyncPSAV.SalonIL ILS = ConSQL.GetOneSalonIL(IDIL);
                    ServList = ConSQL.LILS(IDIL);
                    ViewBag.datasource = ServList;
                    Response.Cookies.Append("IDEVNN", ILS.IDEvt, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                    return View(ILS);
                }
            }
            else
            {
                string folio = ConSQL.GetFolioByITL(Request.Cookies["IDEVNN"].ToString());
                model.IDEvt = Request.Cookies["IDEVNN"].ToString();

                ConSQL.UpdateITL(model, ServList, ServList[0].IDITL);
                Response.Cookies.Append("folio", folio, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                ServList = new List<Models.SyncPSAV.ItemListServices>();
                return RedirectToAction("ResumeEPT");
            }
        }
        #endregion
        public IActionResult ItemList()
        {
            Models.ItemListModel.ItemListEventModel mod = new Models.ItemListModel.ItemListEventModel();
            mod.EventoName = Request.Cookies["EVN"].ToString();
            mod.IDEvento = Convert.ToInt32(Request.Cookies["IDE"].ToString());
            if (ServList.Count.Equals(0))
            {
                BindServList();
                ViewBag.datasource = ServList;
            }
            return View(mod);
        }
        //[HttpPost]
        //public IActionResult ItemList(string Advance)
        //{           
        //        return View("ItemListWorkForce");            
        //}
        [HttpPost]
        public IActionResult ItemList(Models.ItemListModel.ItemListEventModel mod, string Advance)
        {
            if (Advance.Equals("0"))
            {
                string IDITL = ConSQL.InsertTMItemList(mod, Request.Cookies["IDE"].ToString());
                mod = new Models.ItemListModel.ItemListEventModel();
                mod.EventoName = Request.Cookies["EVN"].ToString();
                mod.IDEvento = Convert.ToInt32(Request.Cookies["IDE"].ToString());
                ConSQL.SaveItemListDetail(ServList, IDITL, Request.Cookies["IDE"].ToString());
                return View(mod);
            }
            else
            {
                ServList = new List<Models.SyncPSAV.ItemListServices>();
                return RedirectToAction("ItemListWorkForce");
            }
        }
        public static List<Models.SyncPSAV.ItemListServices> ServList = new List<Models.SyncPSAV.ItemListServices>();
        public void BindServList() { ServList = new List<Models.SyncPSAV.ItemListServices>(); }
        public ActionResult ItemListNormalUpdate([FromBody]CRUDModel<Models.SyncPSAV.ItemListServices> myObject)
        {
            var ord = myObject.Value;
            Models.SyncPSAV.ItemListServices val = ServList.Where(or => or.ID == ord.ID).FirstOrDefault();
            val.ID = ord.ID; val.Cantidad = ord.Cantidad; val.Categoria = ord.Categoria; val.Clave = ord.Clave; val.Descripcion = ord.Descripcion;
            val.Dias = ord.Dias; val.PrecioUnit = ord.PrecioUnit;
            return Json(myObject.Value);
        }
        public ActionResult ItemListNormalInsert([FromBody]CRUDModel<Models.SyncPSAV.ItemListServices> value)
        {
            Models.SyncPSAV.ItemListServices val = value.Value;
            ServList.Insert(ServList.Count, val);
            return Json(ServList);
        }
        public ActionResult ItemListNormalDelete([FromBody]CRUDModel<Models.SyncPSAV.ItemListServices> value)
        {
            ServList.Remove(ServList.Where(or => or.ID == value.Key.ToString()).FirstOrDefault());
            return Json(value);
        }
        #endregion
        #region ItemListWorkForce
        #region EditItemListWF
        [HttpPost]
        public IActionResult EditItemListWF(string IDIL, string Advance, Models.SyncPSAV.SalonILWF model, string EVT)
        {
            if (string.IsNullOrEmpty(Advance))
            {
                if (!string.IsNullOrEmpty(IDIL))
                {
                    Models.SyncPSAV.SalonILWF ILS = ConSQL.GetOneSalonILWF(IDIL);
                    WFList = ConSQL.LILSWF(IDIL);
                    Response.Cookies.Append("IDEVNN", ILS.IDEvt, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                    ViewBag.datasource = WFList;
                    return View(ILS);
                }
                else
                {
                    Models.SyncPSAV.SalonILWF ILS = new Models.SyncPSAV.SalonILWF();
                    WFList = new List<Models.SyncPSAV.ItemListWorkForce>();
                    Response.Cookies.Append("IDEVNN", ConSQL.GetEPTToEdit(EVT).IDEvent, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                    ViewBag.datasource = WFList;
                    return View(ILS);
                }
            }
            else
            {
                string folio = ConSQL.GetFolioByITL(Request.Cookies["IDEVNN"].ToString());
                model.IDEvt = Request.Cookies["IDEVNN"].ToString();
                ConSQL.UpdateITLWF(model, WFList, WFList[0].IDITL);
                Response.Cookies.Append("folio", folio, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                WFList = new List<Models.SyncPSAV.ItemListWorkForce>();
                return RedirectToAction("ResumeEPT");
            }
        }
        #endregion
        [HttpPost]
        public IActionResult ItemListWorkForce(Models.ItemListModel.ItemListEventModel mod, string Advance)
        {
            if (Advance.Equals("0"))
            {
                string IDITL = ConSQL.InsertTMItemListWF(mod, Request.Cookies["IDE"].ToString());
                mod = new Models.ItemListModel.ItemListEventModel();
                mod.EventoName = Request.Cookies["EVN"].ToString();
                mod.IDEvento = Convert.ToInt32(Request.Cookies["IDE"].ToString());
                ConSQL.SaveItemListWFDetail(WFList, IDITL, Request.Cookies["IDE"].ToString());
                return View(mod);
            }
            else
            {
                WFList = new List<Models.SyncPSAV.ItemListWorkForce>();
                return RedirectToAction("VtaDesc");
            }
        }
        public IActionResult ItemListWorkForce()
        {
            Models.ItemListModel.ItemListEventModel mod = new Models.ItemListModel.ItemListEventModel();
            mod.EventoName = Request.Cookies["EVN"].ToString();
            mod.IDEvento = Convert.ToInt32(Request.Cookies["IDE"].ToString());
            if (ServList.Count.Equals(0))
            {
                BindServListWF();
                ViewBag.datasource = WFList;
            }
            return View(mod);
        }
        public static List<Models.SyncPSAV.ItemListWorkForce> WFList = new List<Models.SyncPSAV.ItemListWorkForce>();
        public void BindServListWF() { WFList = new List<Models.SyncPSAV.ItemListWorkForce>(); }
        public ActionResult ItemListWFNormalUpdate([FromBody]CRUDModel<Models.SyncPSAV.ItemListWorkForce> myObject)
        {
            var ord = myObject.Value;
            Models.SyncPSAV.ItemListWorkForce val = WFList.Where(or => or.ID == ord.ID).FirstOrDefault();
            val.ID = ord.ID; val.Cantidad = ord.Cantidad; val.Categoria = ord.Categoria; val.Clave = ord.Clave; val.Descripcion = ord.Descripcion;
            val.Dias = ord.Dias; val.PrecioUnit = ord.PrecioUnit;
            return Json(myObject.Value);
        }
        public ActionResult ItemListWFNormalInsert([FromBody]CRUDModel<Models.SyncPSAV.ItemListWorkForce> value)
        {
            Models.SyncPSAV.ItemListWorkForce val = value.Value;
            WFList.Insert(WFList.Count, val);
            return Json(WFList);
        }
        public ActionResult ItemListWFNormalDelete([FromBody]CRUDModel<Models.SyncPSAV.ItemListWorkForce> value)
        {
            WFList.Remove(WFList.Where(or => or.ID == value.Key.ToString()).FirstOrDefault());
            return Json(value);
        }
        #endregion
        #region VentaDes
        #region edit
        [HttpPost]
        public IActionResult EditVtaDesc(string Advance)
        {
            if (!string.IsNullOrEmpty(Advance))
            {
                VDescList = ConSQL.GetVtaDesc(Request.Cookies["IDEVT"].ToString());
                ViewBag.datasource = VDescList;
            }
            else
            {
                ConSQL.UpdateVtaDesc(VDescList, Request.Cookies["IDEVT"].ToString());
                return RedirectToAction("ResumeEPT");
            }
            return View();
        }
        [HttpPost]
        public IActionResult EditVtaFee(string Advance)
        {
            if (!string.IsNullOrEmpty(Advance))
            {
                VFeeList = ConSQL.GetVFee(Request.Cookies["IDEVT"].ToString());
                ViewBag.datasource = VFeeList;
            }
            else
            {
                ConSQL.UpdateVFee(VFeeList, Request.Cookies["IDEVT"].ToString());
                return RedirectToAction("ResumeEPT");
            }
            return View();
        }
        [HttpPost]
        public IActionResult EditSubrenta(string Advance)
        {
            if (!string.IsNullOrEmpty(Advance))
            {
                SubRentaList = ConSQL.GetSubRenta(Request.Cookies["IDEVT"].ToString());
                ViewBag.datasource = SubRentaList;
            }
            else
            {
                ConSQL.UpdateSRenta(SubRentaList, Request.Cookies["IDEVT"].ToString());
                return RedirectToAction("ResumeEPT");
            }
            return View();
        }
        [HttpPost]
        public IActionResult EditOL(string Advance)
        {
            if (!string.IsNullOrEmpty(Advance))
            {
                ViewBag.datasource1 = FOList;
                ViewBag.datasource2 = ViaticosL;
                ViewBag.datasource3 = ComVenL;
                ViewBag.datasource4 = GasFinL;
                ViewBag.datasource5 = ConsuL;
                ViewBag.datasource6 = CIntL;
            }
            else
            {
                ConSQL.UpdateOL(FOList, ViaticosL, ComVenL, GasFinL, ConsuL, CIntL, Request.Cookies["IDEVT"].ToString());
                return RedirectToAction("ResumeEPT");
            }
            return View();
        }
        #endregion
        public static List<Models.SyncPSAV.VentaDes> VDescList = new List<Models.SyncPSAV.VentaDes>();
        public ActionResult VtaDctoNormalUpdate([FromBody]CRUDModel<Models.SyncPSAV.VentaDes> myObject)
        {
            var ord = myObject.Value;
            Models.SyncPSAV.VentaDes val = VDescList.Where(or => or.Category == ord.Category).FirstOrDefault();
            val.Category = ord.Category; val.VentaEqui = ord.VentaEqui; val.VentaEquEx = ord.VentaEquEx; val.TotalVenta = ord.TotalVenta;
            val.DesPorEq = ord.DesPorEq; val.TotalDescEPS = ord.TotalDescEPS; val.DescExt = ord.DescExt; val.TotalExt = ord.TotalExt;
            val.TotalDesc = ord.TotalDesc; val.PorcTotalDesc = ord.PorcTotalDesc; val.AplicaAut = ord.AplicaAut;
            return Json(myObject.Value);
        }
        public ActionResult VtaDctoNormalInsert([FromBody]CRUDModel<Models.SyncPSAV.VentaDes> value)
        {
            Models.SyncPSAV.VentaDes val = value.Value;
            VDescList.Insert(VDescList.Count, val);
            return Json(VDescList);
        }
        public ActionResult VtaDctoNormalDelete([FromBody]CRUDModel<Models.SyncPSAV.VentaDes> value)
        {
            VDescList.Remove(VDescList.Where(or => or.Category == value.Key.ToString()).FirstOrDefault());
            return Json(value);
        }
        public IActionResult VtaDesc()
        {
            VDescList = ConSQL.GetCategoryEvent(Request.Cookies["IDE"].ToString());
            ViewBag.datasource = VDescList;
            return View();
        }
        [HttpPost]
        public IActionResult VtaDesc(string Advance)
        {
            ConSQL.SaveVDesc(VDescList, Request.Cookies["IDE"].ToString());
            return RedirectToAction("VentaComision");
        }
        #endregion
        #region VentaFee
        public static List<Models.SyncPSAV.VentaFee> VFeeList = new List<Models.SyncPSAV.VentaFee>();
        public ActionResult VtaFeeNormalUpdate([FromBody]CRUDModel<Models.SyncPSAV.VentaFee> myObject)
        {
            var ord = myObject.Value;
            Models.SyncPSAV.VentaFee val = VFeeList.Where(or => or.Category == ord.Category).FirstOrDefault();
            val.BaseFee = ord.BaseFee; val.ImporteFee = ord.ImporteFee; val.PorcFee = ord.PorcFee; val.SubFee = ord.SubFee;
            return Json(myObject.Value);
        }
        public ActionResult VtaFeeNormalInsert([FromBody]CRUDModel<Models.SyncPSAV.VentaFee> value)
        {
            Models.SyncPSAV.VentaFee val = value.Value;
            VFeeList.Insert(VFeeList.Count, val);
            ViewBag.datasource = VFeeList;

            return Json(VFeeList);
        }
        public ActionResult VtaFeeNormalDelete([FromBody]CRUDModel<Models.SyncPSAV.VentaFee> value)
        {
            VFeeList.Remove(VFeeList.Where(or => or.Category == value.Key.ToString()).FirstOrDefault());
            return Json(value);
        }
        public IActionResult VentaComision()
        {
            VFeeList = ConSQL.GetCategoryEvtFee(Request.Cookies["IDE"].ToString());
            ViewBag.datasource = VFeeList;
            return View();
        }
        [HttpPost]
        public IActionResult VentaComision(string Advance)
        {
            ConSQL.SaveVFee(VFeeList, Request.Cookies["IDE"].ToString());
            return RedirectToAction("Subrenta");
        }
        #endregion
        #region Subrenta
        public static List<Models.SyncPSAV.SubRenta> SubRentaList = new List<Models.SyncPSAV.SubRenta>();
        public ActionResult SubRentaNormalUpdate([FromBody]CRUDModel<Models.SyncPSAV.SubRenta> myObject)
        {
            var ord = myObject.Value;
            Models.SyncPSAV.SubRenta val = SubRentaList.Where(or => or.Category == ord.Category).FirstOrDefault();
            val.Descripcion = ord.Descripcion; val.Gastosub = ord.Gastosub; val.Invoice = ord.Invoice; val.Supplier = ord.Supplier; val.Ventasub = ord.Ventasub;
            return Json(myObject.Value);
        }
        public ActionResult SubRentaNormalInsert([FromBody]CRUDModel<Models.SyncPSAV.SubRenta> value)
        {
            Models.SyncPSAV.SubRenta val = value.Value;
            SubRentaList.Insert(SubRentaList.Count, val);
            return Json(VFeeList);
        }
        public ActionResult SubRentaNormalDelete([FromBody]CRUDModel<Models.SyncPSAV.SubRenta> value)
        {
            SubRentaList.Remove(SubRentaList.Where(or => or.Category == value.Key.ToString()).FirstOrDefault());
            return Json(value);
        }
        public IActionResult Subrenta()
        {
            SubRentaList = new List<Models.SyncPSAV.SubRenta>();
            ViewBag.datasource = SubRentaList;
            return View();
        }
        [HttpPost]
        public IActionResult Subrenta(string Advance)
        {
            ConSQL.SaveSubRent(SubRentaList, Request.Cookies["IDE"].ToString());
            return RedirectToAction("OL");
        }
        #endregion
        #region OL
        public IActionResult OL()
        {
            BindResultsOL();
            ViewBag.datasource1 = FOList;
            ViewBag.datasource2 = ViaticosL;
            ViewBag.datasource3 = ComVenL;
            ViewBag.datasource4 = GasFinL;
            ViewBag.datasource5 = ConsuL;
            ViewBag.datasource6 = CIntL;
            return View();
        }
        [HttpPost]
        public IActionResult OL(string Advance)
        {
            string folio = ConSQL.GetFolioByITL(Request.Cookies["IDE"].ToString());
            ConSQL.SaveOL(FOList, ViaticosL, ComVenL, GasFinL, ConsuL, CIntL, Request.Cookies["IDE"].ToString());
            Response.Cookies.Append("folio", folio, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
            return RedirectToAction("ResumenEPT");
        }
        public static List<Models.SyncPSAV.FreelanceOL> FOList = new List<Models.SyncPSAV.FreelanceOL>();
        public static List<Models.SyncPSAV.Viaticos> ViaticosL = new List<Models.SyncPSAV.Viaticos>();
        public static List<Models.SyncPSAV.VentasFeeTot> ComVenL = new List<Models.SyncPSAV.VentasFeeTot>();
        public static List<Models.SyncPSAV.GastosFinancieros> GasFinL = new List<Models.SyncPSAV.GastosFinancieros>();
        public static List<Models.SyncPSAV.Consumibles> ConsuL = new List<Models.SyncPSAV.Consumibles>();
        public static List<Models.SyncPSAV.CargosInternos> CIntL = new List<Models.SyncPSAV.CargosInternos>();
        public void BindResultsOL()
        {
            FOList = new List<Models.SyncPSAV.FreelanceOL>(); ViaticosL = new List<Models.SyncPSAV.Viaticos>(); ComVenL = new List<Models.SyncPSAV.VentasFeeTot>();
            GasFinL = new List<Models.SyncPSAV.GastosFinancieros>(); ConsuL = new List<Models.SyncPSAV.Consumibles>(); CIntL = new List<Models.SyncPSAV.CargosInternos>();
        }
        #endregion
        #region editEPT
        #region resumeEPT

        public IActionResult ResumenEPT(string folio)
        {
            if (!string.IsNullOrEmpty(Request.Cookies["folio"].ToString())) { folio = Request.Cookies["folio"].ToString(); }
            Models.EPTModel eptM = ConSQL.GetEPTToEdit(folio);
            string IDEvent = eptM.IDEvent;
            //itemlist
            ViewBag.ILList = ConSQL.GetSalons(IDEvent);
            //itemlistworkforce
            ViewBag.ILListWF = ConSQL.GetSalonsWF(IDEvent);
            Response.Cookies.Append("IDEVT", IDEvent, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
            return View(eptM);
        }
        public IActionResult ResumeEPT()
        {
            string folio = "";
            if (!string.IsNullOrEmpty(Request.Cookies["folio"].ToString())) { folio = Request.Cookies["folio"].ToString(); }
            Models.EPTModel eptM = ConSQL.GetEPTToEdit(folio);
            string IDEvent = eptM.IDEvent;
            //itemlist
            ViewBag.ILList = ConSQL.GetSalons(IDEvent);
            //itemlistworkforce
            ViewBag.ILListWF = ConSQL.GetSalonsWF(IDEvent);
            Response.Cookies.Append("IDEVT", IDEvent, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
            return View(eptM);
        }
        [HttpPost]
        public IActionResult ResumeEPT(string folio)
        {
            //if (!string.IsNullOrEmpty(Request.Cookies["folio"].ToString())){ folio = Request.Cookies["folio"].ToString(); }
            Models.EPTModel eptM = ConSQL.GetEPTToEdit(folio);
            string IDEvent = eptM.IDEvent;
            //itemlist
            ViewBag.ILList = ConSQL.GetSalons(IDEvent);
            //itemlistworkforce
            ViewBag.ILListWF = ConSQL.GetSalonsWF(IDEvent);
            Response.Cookies.Append("IDEVT", IDEvent, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
            return View(eptM);
        }
        [HttpGet]
        public IActionResult EditEPT(Models.EPTModel eptt, string folio)
        {
            Models.EPTModel eptM = ConSQL.GetEPTToEdit(folio);
            List<Models.PSAVCrud.CoinsModel> CoinsList = ConSQL.GetCoin(string.Empty);
            ViewBag.CoinsList = CoinsList;
            return View(eptM);
        }
        [HttpPost]
        public IActionResult EditEPT(Models.EPTModel eptM, string NewValue, string folio)
        {
            if (!string.IsNullOrEmpty(NewValue))
            {
                List<Models.PSAVCrud.ClientModel.ClientSearch> Clients = ConSQL.GetClientOne(eptM.IDClient);
                for (int i = 0; i < Clients.Count; i++)
                {
                    eptM.DomicilioComercial = Clients[i].Domicilio;
                    eptM.DomicilioFiscal = Clients[i].Fiscal;
                    eptM.IDClient = Clients[i].IDClient;
                    eptM.RazonSocial = Clients[i].Razon;
                    eptM.Phone = Clients[i].Tel;
                    eptM.Mobile = Clients[i].Cel;
                    eptM.Email = Clients[i].Email;
                    eptM.RFC = Clients[i].RFC;
                    eptM.Ext = Clients[i].Ext;
                    eptM.ContactClientName = Clients[i].NombreContacto;
                    eptM.IDEmpresa = Clients[i].IDEmpresa;
                }
                ConSQL.EditInsertEPT(eptM);
                return RedirectToAction("EPT");
            }
            else
            {
                return View();
            }
        }
        #endregion
        #endregion
        #endregion
        //[HttpPost]
        //public IActionResult ItemList(Models.ItemListModel.ItemListEventModel model,List<Models.ItemListModel.ItemListServices> Servi,string Siguiente)
        //{
        //    Models.ItemListModel.ItemListEventModel Ilem = new Models.ItemListModel.ItemListEventModel();
        //    Ilem.ServList = new List<Models.ItemListModel.ItemListServices>();
        //    if(TempData["ServiciosList"] != null)
        //    {
        //        Ilem.ServList = (List<Models.ItemListModel.ItemListServices>)TempData["ServiciosList"];
        //    }
        //    Ilem.EventoName = Request.Cookies["EVN"].ToString();
        //    Ilem.Salon = model.Salon;
        //    Ilem.Asistentes = model.Asistentes;
        //    Ilem.Montaje = model.Montaje;
        //    Ilem.Horario = model.Horario;
        //    Ilem.Cantidad = "";
        //    Ilem.Clave = "";
        //    Ilem.Descripcion = "";
        //    Ilem.Dias = "";
        //    Ilem.PrecioUnit = "";            
        //    Ilem.ServList.Add(new Models.ItemListModel.ItemListServices() { Cantidad = model.Cantidad, Clave = model.Clave, Descripcion = model.Descripcion, Dias = model.Dias, PrecioUnit = model.PrecioUnit });
        //    ViewBag.ServiciosList = Ilem.ServList;
        //    ViewBag.CategoriaList = ConSQL.GetCategoryItemList(1);
        //    Ilem.EventoName = Request.Cookies["EVN"].ToString();
        //    //TempData["ServiciosList"] = Ilem.ServList;
        //    if (!string.IsNullOrEmpty(Siguiente))
        //    {
        //        if (Siguiente.Equals("Si"))
        //        {
        //            Response.Cookies.Append("IDE", Request.Cookies["IDE"].ToString(), new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
        //            Response.Cookies.Append("EVN", Request.Cookies["EVN"].ToString(), new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
        //            Response.Cookies.Append("SAL", model.Salon, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
        //            Response.Cookies.Append("ASI", model.Asistentes, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
        //            Response.Cookies.Append("MON", model.Montaje, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
        //            Response.Cookies.Append("HRO", model.Horario, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
        //            return View("ItemListWorkForce");
        //        }
        //        else
        //        {
        //            return View(Ilem);
        //        }
        //    }
        //    else
        //    {
        //        if(ModelState.IsValid)
        //        {
        //            string IDITL=ConSQL.InsertTMItemList(model, Request.Cookies["IDE"].ToString());
        //            Response.Cookies.Append("IDITL", IDITL, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
        //            ConSQL.SaveItemListDetail(model, IDITL);
        //        }
        //        return View(Ilem);
        //    }

        //}
        //[HttpPost]
        //public IActionResult ItemList2(Models.ItemListModel.ItemListEventModel model, List<Models.ItemListModel.ItemListServices> Servi, string Siguiente)
        //{
        //    Models.ItemListModel.ItemListEventModel Ilem = new Models.ItemListModel.ItemListEventModel();
        //    Ilem.ServList = new List<Models.ItemListModel.ItemListServices>();
        //    if (TempData["ServiciosList"] != null)
        //    {
        //        Ilem.ServList = (List<Models.ItemListModel.ItemListServices>)TempData["ServiciosList"];
        //    }
        //    Ilem.EventoName = Request.Cookies["EVN"].ToString();
        //    Ilem.Salon = model.Salon;
        //    Ilem.Asistentes = model.Asistentes;
        //    Ilem.Montaje = model.Montaje;
        //    Ilem.Horario = model.Horario;
        //    Ilem.Cantidad = "";
        //    Ilem.Clave = "";
        //    Ilem.Descripcion = "";
        //    Ilem.Dias = "";
        //    Ilem.PrecioUnit = "";
        //    Ilem.ServList.Add(new Models.ItemListModel.ItemListServices() { Cantidad = model.Cantidad, Clave = model.Clave, Descripcion = model.Descripcion, Dias = model.Dias, PrecioUnit = model.PrecioUnit });
        //    ViewBag.ServiciosList = Ilem.ServList;
        //    ViewBag.CategoriaList = ConSQL.GetCategoryItemList(1);
        //    //TempData["ServiciosList"] = Ilem.ServList;
        //    if (Siguiente.Equals("Si"))
        //    {
        //        return View("ItemListWorkForce");
        //    }
        //    else
        //    {
        //        return View(Ilem);
        //    }

        //}
        //[HttpPost]
        //public IActionResult ItemListWorkForce(Models.ItemListModel.ItemListEventModel model, List<Models.ItemListModel.ItemListServices> Servi, string Siguiente)
        //{
        //    Models.ItemListModel.ItemListEventModel Ilem = new Models.ItemListModel.ItemListEventModel();
        //    Ilem.ServList = new List<Models.ItemListModel.ItemListServices>();
        //    if (TempData["ServiciosList"] != null)
        //    {
        //        Ilem.ServList = (List<Models.ItemListModel.ItemListServices>)TempData["ServiciosList"];
        //    }
        //    Ilem.EventoName = Request.Cookies["EVN"].ToString();
        //    Ilem.Salon = Request.Cookies["SLN"].ToString();
        //    Ilem.Asistentes = Request.Cookies["ASI"].ToString();
        //    Ilem.Montaje = Request.Cookies["MON"].ToString();
        //    Ilem.Horario = Request.Cookies["HRO"].ToString();
        //    Ilem.Cantidad = "";
        //    Ilem.Clave = "";
        //    Ilem.Descripcion = "";
        //    Ilem.Dias = "";
        //    Ilem.PrecioUnit = "";
        //    Ilem.ServList.Add(new Models.ItemListModel.ItemListServices() { Cantidad = model.Cantidad, Clave = model.Clave, Descripcion = model.Descripcion, Dias = model.Dias, PrecioUnit = model.PrecioUnit });
        //    ViewBag.ServiciosList = Ilem.ServList;
        //    ViewBag.CategoriaList = ConSQL.GetCategoryItemList(2);
        //    //TempData["ServiciosList"] = Ilem.ServList;
        //    if (!string.IsNullOrEmpty(Siguiente))
        //    {
        //        if (Siguiente.Equals("Si"))
        //        {
        //            return View("ItemListWorkForce");
        //        }
        //        else
        //        {
        //            return View(Ilem);
        //        }
        //    }
        //    else
        //    {
        //        if(ModelState.IsValid)
        //        {
        //            ConSQL.SaveItemListWFDetail(model, Request.Cookies["IDITL"].ToString());
        //        }
        //        return View(Ilem);
        //    }

        //}
        //[HttpPost]
        //public IActionResult ItemListWorkForce2(Models.ItemListModel.ItemListEventModel model, List<Models.ItemListModel.ItemListServices> Servi, string Siguiente)
        //{
        //    Models.ItemListModel.ItemListEventModel Ilem = new Models.ItemListModel.ItemListEventModel();
        //    Ilem.ServList = new List<Models.ItemListModel.ItemListServices>();
        //    if (TempData["ServiciosList"] != null)
        //    {
        //        Ilem.ServList = (List<Models.ItemListModel.ItemListServices>)TempData["ServiciosList"];
        //    }
        //    Ilem.EventoName = Request.Cookies["EVN"].ToString();
        //    Ilem.Salon = model.Salon;
        //    Ilem.Asistentes = model.Asistentes;
        //    Ilem.Montaje = model.Montaje;
        //    Ilem.Horario = model.Horario;
        //    Ilem.Cantidad = "";
        //    Ilem.Clave = "";
        //    Ilem.Descripcion = "";
        //    Ilem.Dias = "";
        //    Ilem.PrecioUnit = "";
        //    Ilem.ServList.Add(new Models.ItemListModel.ItemListServices() { Cantidad = model.Cantidad, Clave = model.Clave, Descripcion = model.Descripcion, Dias = model.Dias, PrecioUnit = model.PrecioUnit });
        //    ViewBag.ServiciosList = Ilem.ServList;
        //    ViewBag.CategoriaList = ConSQL.GetCategoryItemList(2);
        //    //TempData["ServiciosList"] = Ilem.ServList;
        //    if (Siguiente.Equals("Si"))
        //    {
        //        return View("ItemListWorkForce");
        //    }
        //    else
        //    {
        //        return View(Ilem);
        //    }

        //}
        [HttpPost]
        public async Task<IActionResult> ShowEPT(string folio)
        {
            Models.EPTModel eptM = ConSQL.GetEPTToEdit(folio);
            return View(eptM);
        }
        #region CaptureRatio
        [HttpPost]
        public IActionResult CRatio(string ac1)
        {
            List<Models.CaptureRatio.CRVResumenModel> CRList = ConSQL.GetCRatio(string.Empty);
            ViewBag.CRList = CRList;
            return View();
        }
        public IActionResult CRatio()
        {
            List<Models.CaptureRatio.CRVResumenModel> CRList = ConSQL.GetCRatio(string.Empty);
            ViewBag.CRList = CRList;
            return View();
        }
        public IActionResult NewCratio()
        {
            return View();
        }
        public static List<Models.SyncPSAV.CratioDets> CDets = new List<Models.SyncPSAV.CratioDets>();
        public IActionResult NewCRatio2(Models.CaptureRatio.CRVModel model)
        {

            ViewBag.datasource = CDets;
            return View();
        }
        public ActionResult CratioNormalUpdate([FromBody]CRUDModel<Models.SyncPSAV.CratioDets> myObject)
        {
            var ord = myObject.Value;
            Models.SyncPSAV.CratioDets val = CDets.Where(or => or.IDCratioDets == ord.IDCratioDets).FirstOrDefault();

            val.IDCratio = Request.Cookies["IDCR"].ToString();
            //val.ID = ord.ID; val.Name = ord.Name; val.Change = ord.Change; val.Active = ord.Active;
            ConSQL.SaveCRDet(val, 1);
            return Json(myObject.Value);
        }
        public ActionResult CratioNormalInsert([FromBody]CRUDModel<Models.SyncPSAV.CratioDets> value)
        {
            Models.SyncPSAV.CratioDets val = value.Value;
            val.IDCratio = Request.Cookies["IDCR"].ToString();
            val.IDCratioDets = ConSQL.SaveCRDet(value.Value, 0);
            CDets.Insert(CDets.Count, val);
            ConSQL.SaveCRDet(val, 0);
            return Json(CDets);
        }
        public ActionResult CratioNormalDelete([FromBody]CRUDModel<Models.SyncPSAV.CratioDets> value)
        {
            CDets.Remove(CDets.Where(or => or.IDCratioDets == value.Key.ToString()).FirstOrDefault());
            ConSQL.SaveCRDet(value.Value, 2);
            return Json(value);
        }
        [HttpPost]
        public async Task<IActionResult> NewCratio(Models.CaptureRatio.CRVModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request.Cookies["IDCR"] != null) { Response.Cookies.Delete("IDCR"); }
                string IDCRatioL = ConSQL.GetCRatio(model);
                model.IDCRAtio = IDCRatioL;
                Response.Cookies.Append("IDCR", IDCRatioL, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
                return RedirectToAction("NewCRatio2", model);

            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> NewCR()
        {
            return View("CRatio");
        }
        public static List<Models.SyncPSAV.CratioDets> CRL = new List<Models.SyncPSAV.CratioDets>();
        [HttpPost]
        public async Task<IActionResult> EditCRatio(string IDC)
        {
            Models.CaptureRatio.CRVResumenModel CRVM = new Models.CaptureRatio.CRVResumenModel();
            List<Models.CaptureRatio.CRVResumenModel> CVR = ConSQL.GetCratioConsult(IDC);
            CDets = ConSQL.GetCD(IDC);
            if (Request.Cookies["IDCOne"] != null) { Response.Cookies.Delete("IDCOne"); }
            Response.Cookies.Append("IDCOne", IDC, new Microsoft.AspNetCore.Http.CookieOptions { Path = "/", HttpOnly = true });
            ViewBag.CRLIt = CDets;
            for (int i = 0; i < CVR.Count; i++)
            {
                CRVM.HotelName = CVR[i].HotelName;
                CRVM.DET = CVR[i].DET;
                CRVM.CityLoc = CVR[i].CityLoc;
                CRVM.Contact = CVR[i].Contact;
                CRVM.FillFormName = CVR[i].FillFormName;
                CRVM.LocationHotel = CVR[i].LocationHotel;
            }
            ViewBag.datasource = CDets;
            return View(CRVM);
        }
        #endregion
        #region EPTExcel
        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private readonly IHostingEnvironment _hostingEnvironment;
        public PSAVController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        public IActionResult ExportEPT(string folio)
        {
            try
            {
                var fileDownloadName = "EPTTem.xlsx";
                var reportsFolder = "reports";
                var fileInfo = new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, reportsFolder, fileDownloadName));
                byte[] ReportArray;
                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    
                    //start opening workbook Cotizacion
                    ExcelWorksheet wkCotizacion = package.Workbook.Worksheets[2];
                    //fill cotizacion info from DB
                    Models.EPTModel ModEptFill = ConSQL.GetEPTToEdit(folio);
                    //fill Client Info
                    wkCotizacion.Cells["C10"].Value = ModEptFill.RazonSocial;//Razon
                    wkCotizacion.Cells["C11"].Value = ModEptFill.DomicilioComercial;//Razon
                    wkCotizacion.Cells["C12"].Value = ModEptFill.DomicilioFiscal;//Razon
                    wkCotizacion.Cells["C13"].Value = ModEptFill.RFC;//Razon
                                                                     //fill contact client
                    wkCotizacion.Cells["C15"].Value = ModEptFill.ContactClientName;//Razon
                    wkCotizacion.Cells["C16"].Value = ModEptFill.Job;//Razon
                    wkCotizacion.Cells["C17"].Value = ModEptFill.Phone;//Razon
                    wkCotizacion.Cells["C18"].Value = ModEptFill.Ext;//Razon
                    wkCotizacion.Cells["C19"].Value = ModEptFill.Mobile;//Razon
                    wkCotizacion.Cells["C20"].Value = ModEptFill.Fax;//Razon
                    wkCotizacion.Cells["C21"].Value = ModEptFill.Email;//Razon
                                                                       //fill sales Represent
                    wkCotizacion.Cells["C10"].Value = "";//Razon
                    wkCotizacion.Cells["C10"].Value = "";//Razon
                    wkCotizacion.Cells["C10"].Value = "";//Razon
                    wkCotizacion.Cells["C10"].Value = "";//Razon
                                                         //fill event info
                    wkCotizacion.Cells["G10"].Value = ModEptFill.EventName;//Razon
                    wkCotizacion.Cells["G11"].Value = ModEptFill.MountDate;//Razon
                    wkCotizacion.Cells["G12"].Value = ModEptFill.MountHour;//Razon
                    wkCotizacion.Cells["G13"].Value = ModEptFill.PlaceContact;//Razon
                    wkCotizacion.Cells["G14"].Value = ModEptFill.PlaceMobile;//Razon
                    wkCotizacion.Cells["G15"].Value = ModEptFill.StartDate;//Razon
                    wkCotizacion.Cells["G16"].Value = ModEptFill.StartHour;//Razon
                    wkCotizacion.Cells["G17"].Value = ModEptFill.FinishDate;//Razon
                    wkCotizacion.Cells["G18"].Value = ModEptFill.FinishHour;//Razon
                    wkCotizacion.Cells["G19"].Value = ModEptFill.Place;//Razon
                    wkCotizacion.Cells["G20"].Value = ModEptFill.Address;//Razon
                    wkCotizacion.Cells["G22"].Value = "MXN Mexican Peso";//Razon
                                                                         //fill psav manager
                    wkCotizacion.Cells["F10"].Value = "";//Razon
                    wkCotizacion.Cells["F10"].Value = "";//Razon
                    wkCotizacion.Cells["F10"].Value = "";//Razon
                    wkCotizacion.Cells["F10"].Value = "";//Razon
                                                         ///////////Get Item List Sheet
                    ExcelWorksheet wkItemList = package.Workbook.Worksheets[3];
                    ///get item list salons
                    List<Models.SyncPSAV.SalonIL> SILList = ConSQL.GetSalons(ModEptFill.IDEvent);
                    for (int i = 0; i < SILList.Count; i++)
                    {
                        wkItemList.Cells["E16"].Value = ModEptFill.EventName;//Event
                        wkItemList.Cells["E17"].Value = SILList[i].Salon;//Event
                        wkItemList.Cells["E18"].Value = SILList[i].Asistentes;//Event
                        wkItemList.Cells["E19"].Value = SILList[i].Montaje;//Event
                        wkItemList.Cells["E20"].Value = SILList[i].Horario;//Event
                                                                           ///get itemlist services by salon
                        List<Models.SyncPSAV.ItemListServices> LILS = ConSQL.GetOneILIL(SILList[i].IDITL);
                        //fill items
                        for (int a = 0; a < LILS.Count; a++)
                        {
                            wkItemList.Cells["B" + (21 + a).ToString()].Value = LILS[a].Clave;//Event
                            wkItemList.Cells["C" + (21 + a).ToString()].Value = LILS[a].Cantidad;//Event
                            wkItemList.Cells["D" + (21 + a).ToString()].Value = LILS[a].Dias;//Event
                            wkItemList.Cells["E" + (21 + a).ToString()].Value = LILS[a].Descripcion;//Event
                            wkItemList.Cells["F" + (21 + a).ToString()].Value = LILS[a].PrecioUnit;//Event
                            wkItemList.Cells["I" + (21 + a).ToString()].Value = LILS[a].Categoria;//Event
                        }
                        //fill workforce
                        List<Models.SyncPSAV.ItemListWorkForce> LIWF = ConSQL.GetOneILWF(SILList[i].IDITL);
                        for (int o = 0; o < LIWF.Count; o++)
                        {
                            wkItemList.Cells["B" + (55 + o).ToString()].Value = LIWF[o].Clave;//Event
                            wkItemList.Cells["C" + (55 + o).ToString()].Value = LIWF[o].Cantidad;//Event
                            wkItemList.Cells["D" + (55 + o).ToString()].Value = LIWF[o].Dias;//Event
                            wkItemList.Cells["E" + (55 + o).ToString()].Value = LIWF[o].Descripcion;//Event
                            wkItemList.Cells["F" + (55 + o).ToString()].Value = LIWF[o].PrecioUnit;//Event
                            wkItemList.Cells["I" + (55 + o).ToString()].Value = LIWF[o].Categoria;//Event
                        }
                    }
                    //finally save the EPT document
                    ////package.Save();
                    ReportArray = package.GetAsByteArray();
                }
                return File(ReportArray, XlsxContentType, "EPT" + System.DateTime.Now.ToString("ddMMyyyy") + ".xlsx");
            }
            catch (Exception ex){ ViewBag.datashow = ex.Message; return View(); }
        }
        #endregion
        #region CRATIOOne
        public IActionResult ExportCratioOne()
        {
            string IDC = Request.Cookies["IDCOne"].ToString();
            var fileDownloadName = "CROne.xlsx";
            var reportsFolder = "reports";
            var fileInfo = new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, reportsFolder, fileDownloadName));
            byte[] ReportArray=null;
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                
                ExcelWorksheet exlWork = package.Workbook.Worksheets[5];
                List<Models.CaptureRatio.CRVResumenModel> CRVRM = ConSQL.GetCRatioExport(IDC);
                //fill header
                exlWork.Cells["B2"].Value = CRVRM[0].HotelName;
                exlWork.Cells["B3"].Value = CRVRM[0].CityLoc;
                exlWork.Cells["B4"].Value = CRVRM[0].LocationHotel;                                                                  
                exlWork.Cells["B5"].Value = CRVRM[0].DET;
                exlWork.Cells["B6"].Value = CRVRM[0].Contact;
                exlWork.Cells["B7"].Value = CRVRM[0].FillFormName;
                //fill months
                List<Models.CaptureRatio.CRatioList> CRL = ConSQL.GCRLExport(IDC);
                int cellHeader1 = 12;
                int cellHeader2 = 13;
                int cellBody = 14;
                int cellCount = 0;
                for(int a = 1; a <= 12; a++)
                {                    
                    var varCRLF = CRL.Where(p => p.MesFiltro.Contains(a.ToString("00")));
                    List<Models.CaptureRatio.CRatioList> CRLF = new List<Models.CaptureRatio.CRatioList>();
                    foreach(Models.CaptureRatio.CRatioList inListCR in varCRLF)
                    {
                        CRLF.Add(inListCR);
                    }
                    if (!CRLF.Count.Equals(0)) {
                        exlWork.Cells["C" + cellHeader1.ToString()].Value = GetMonth(a.ToString("00"));
                        exlWork.Cells["C" + cellHeader1.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["C" + cellHeader1.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        //titles
                        exlWork.Cells["A" + cellHeader2.ToString()].Value = "Fecha Inicio";
                        exlWork.Cells["B" + cellHeader2.ToString()].Value = "Fecha Fin";
                        exlWork.Cells["C" + cellHeader2.ToString()].Value = "Nombre del Evento";
                        exlWork.Cells["D" + cellHeader2.ToString()].Value = "Cliente Final";
                        exlWork.Cells["E" + cellHeader2.ToString()].Value = "Nombre del contacto";
                        exlWork.Cells["F" + cellHeader2.ToString()].Value = "Contacto Cliente";
                        exlWork.Cells["G" + cellHeader2.ToString()].Value = "Agencia";
                        exlWork.Cells["H" + cellHeader2.ToString()].Value = "Tipo de evento";
                        exlWork.Cells["I" + cellHeader2.ToString()].Value = "Empresa A/V";
                        exlWork.Cells["J" + cellHeader2.ToString()].Value = "Responsable PSAV";
                        exlWork.Cells["K" + cellHeader2.ToString()].Value = "Monto Ventas PSAV";
                        exlWork.Cells["L" + cellHeader2.ToString()].Value = "Lost Business";
                        exlWork.Cells["M" + cellHeader2.ToString()].Value = "Suma";
                        exlWork.Cells["N" + cellHeader2.ToString()].Value = "Capture Ratio";
                        exlWork.Cells["O" + cellHeader2.ToString()].Value = "Monto de Comisión al Hotel";
                        exlWork.Cells["P" + cellHeader2.ToString()].Value = "Motivo de LB";
                        exlWork.Cells["Q" + cellHeader2.ToString()].Value = "Cuando y donde será el próximo evento";
                        //background
                        exlWork.Cells["A" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["B" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["C" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["D" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["E" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["F" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["G" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["H" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["I" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["J" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["K" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["L" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["M" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["N" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["O" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["P" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        exlWork.Cells["Q" + cellHeader2.ToString()].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                        //font color
                        exlWork.Cells["A" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["B" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["C" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["D" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["E" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["F" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["G" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["H" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["I" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["J" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["K" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["L" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["M" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["N" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["O" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["P" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                        exlWork.Cells["Q" + cellHeader2.ToString()].Style.Font.Color.SetColor(System.Drawing.Color.Black);
                    }
                    for (int i = 0; i < CRLF.Count; i++)
                    {
                        exlWork.Cells["A" + cellBody.ToString()].Value = CRL[i].Finicio;
                        exlWork.Cells["B" + cellBody.ToString()].Value = CRL[i].FFin;
                        exlWork.Cells["C" + cellBody.ToString()].Value = CRL[i].EventName;
                        exlWork.Cells["D" + cellBody.ToString()].Value = CRL[i].FinalClient;
                        exlWork.Cells["E" + cellBody.ToString()].Value = CRL[i].ContactName;
                        exlWork.Cells["F" + cellBody.ToString()].Value = CRL[i].ContactContact;
                        exlWork.Cells["G" + cellBody.ToString()].Value = CRL[i].Agency;
                        exlWork.Cells["H" + cellBody.ToString()].Value = CRL[i].EventType;
                        exlWork.Cells["I" + cellBody.ToString()].Value = CRL[i].EmpresaAV;
                        exlWork.Cells["J" + cellBody.ToString()].Value = CRL[i].RsponsablePSAV;
                        exlWork.Cells["K" + cellBody.ToString()].Value = CRL[i].VentasPSAV;
                        exlWork.Cells["L" + cellBody.ToString()].Value = CRL[i].LB;
                        exlWork.Cells["M" + cellBody.ToString()].Value = CRL[i].Suma;
                        exlWork.Cells["N" + cellBody.ToString()].Formula = "(" + "K" + cellBody.ToString() + "*1)/" + ("M" + cellBody.ToString()) + "";
                        exlWork.Cells["N" + cellBody.ToString()].Calculate();
                        exlWork.Cells["O" + cellBody.ToString()].Value = CRL[i].HotelFee;
                        exlWork.Cells["P" + cellBody.ToString()].Value = CRL[i].LBCause;
                        exlWork.Cells["Q" + cellBody.ToString()].Value = CRL[i].NextEventDate+" " +CRL[i].NextEventPlace;
                        cellCount++;
                        cellHeader1 = cellHeader1 + cellCount;
                        cellHeader2 = cellHeader2 + cellCount;
                        cellBody = cellBody + cellCount;
                    }                    
                }                
                ReportArray = package.GetAsByteArray();
            }
            return File(ReportArray, XlsxContentType, "CRatioOne.xlsx");
        }
        public string GetMonth(string MonthsStr)
        {
            string ValReturn = "";
            switch (MonthsStr)
            {
                case "01":ValReturn = "ENERO"; break;
                case "02": ValReturn = "FEBRERO"; break;
                case "03": ValReturn = "MARZO"; break;
                case "04": ValReturn = "ABRIL"; break;
                case "05": ValReturn = "MAYO"; break;
                case "06": ValReturn = "JUNIO"; break;
                case "07": ValReturn = "JULIO"; break;
                case "08": ValReturn = "AGOSTO"; break;
                case "09": ValReturn = "SEPTIEMBRE"; break;
                case "10": ValReturn = "OCTUBRE"; break;
                case "11": ValReturn = "NOVIEMBRE"; break;
                case "12": ValReturn = "DICIEMBRE"; break;
            }
            return ValReturn;
        }
        #endregion
    }
}

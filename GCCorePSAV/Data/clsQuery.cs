using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace GCCorePSAV.Data
{
    public class clsQuery
    {
        private const string con = "Uid=root;Database=psav_dev;Pwd=(Conexi0npsavdatabasedev)1605;Host=35.188.2.70;";// CertificateFile=client.pfx;CertificatePassword=Mak3bbee;";/*SSL Mode=Required*/
        #region Security - Account - Manage
        public bool _loginSession(string _username, string pwd)
        {
            //string con = "Server = 104.154.153.106; Database = psav_dev; Uid = root; Pwd = (Conexi0npsavdatabasedev)1605;";
            MySqlConnection conexion_login = new MySqlConnection(con);
            conexion_login.Open();
            MySqlCommand cmd = new MySqlCommand("select count(1) from tm_users where concat(tmu_username,tmu_pwd)='" + _username + pwd + "'", conexion_login);
            int validacion = Convert.ToInt32(cmd.ExecuteScalar());
            conexion_login.Close();
            if (validacion > 0)
            {
                return true;
            }
            else { return false; }
        }
        public List<Models.MenuModel.MIMOdel> GetDataMenu(string UID)
        {
            string QuerySearch = "SELECT tmmp.tmmp_displaytext,tmmp.tmmp_id FROM psav_dev.tr_userroles trur inner join psav_dev.tr_rolesdetail trrd on trrd.tmr_id = trur.tmr_id  inner join psav_dev.tm_menudetail tmmd on tmmd.tmmd_id = trrd.tmmd_id " +
" inner join psav_dev.tm_menuparent tmmp on tmmp.tmmp_id = tmmd.tmmp_id inner join psav_dev.tm_viewmanage tmvm on tmvm.tmvm_id = tmmd.tmvm_id inner join psav_dev.tm_users tmu on tmu.tmu_id = trur.tmu_id " +
" where tmu.tmu_username = '" + UID + "' group by tmmp.tmmp_id";
            MySqlConnection conexion_login = new MySqlConnection(con);
            conexion_login.Open();
            MySqlCommand cmd = new MySqlCommand(QuerySearch, conexion_login);
            MySqlDataReader sdr = cmd.ExecuteReader();
            List<Models.MenuModel.DDLModel> DDLMList = new List<Models.MenuModel.DDLModel>();
            while (sdr.Read())
            {
                Models.MenuModel.DDLModel DDLMod = new Models.MenuModel.DDLModel();
                DDLMod.NameDDL = sdr.GetValue(0).ToString();
                DDLMod.IDDDL = sdr.GetValue(1).ToString();
                DDLMList.Add(DDLMod);
            }
            sdr.Close();
            conexion_login.Close();
            List<Models.MenuModel.MIMOdel> MIMOL = new List<Models.MenuModel.MIMOdel>();
            for (int i = 0; i < DDLMList.Count; i++)
            {
                string QuerySearch2 = "SELECT tmmd.tmmd_displaytext,tmvm.tmvm_name,tmvm.tmvm_controller,tmvm.tmvm_action FROM psav_dev.tr_userroles trur inner join psav_dev.tr_rolesdetail trrd on trrd.tmr_id = trur.tmr_id  inner join psav_dev.tm_menudetail tmmd on tmmd.tmmd_id = trrd.tmmd_id " +
" inner join psav_dev.tm_menuparent tmmp on tmmp.tmmp_id = tmmd.tmmp_id inner join psav_dev.tm_viewmanage tmvm on tmvm.tmvm_id = tmmd.tmvm_id inner join psav_dev.tm_users tmu on tmu.tmu_id = trur.tmu_id " +
" where tmu.tmu_username = '" + UID + "' and tmmp.tmmp_id=" + DDLMList[i].IDDDL;
                conexion_login.Open();
                MySqlCommand cmd2 = new MySqlCommand(QuerySearch2, conexion_login);
                MySqlDataReader sdr2 = cmd2.ExecuteReader();
                List<Models.MenuModel.LIDDL> LIDL = new List<Models.MenuModel.LIDDL>();
                while (sdr2.Read())
                {
                    Models.MenuModel.LIDDL LIDI = new Models.MenuModel.LIDDL();
                    LIDI.LIText = sdr2.GetValue(0).ToString();
                    LIDI.LIAction = sdr2.GetValue(3).ToString();
                    LIDI.LIController = sdr2.GetValue(2).ToString();
                    LIDL.Add(LIDI);
                }
                sdr2.Close();
                conexion_login.Close();
                MIMOL.Add(new Models.MenuModel.MIMOdel() { MenuD = LIDL, NameDDL = DDLMList[i].NameDDL });
            }
            return MIMOL;
        }
        public List<Models.PSAVCrud.ViewManagerModel.ViewManagerList> GetVMList(string vista)
        {
            List<Models.PSAVCrud.ViewManagerModel.ViewManagerList> _ReturnList = new List<Models.PSAVCrud.ViewManagerModel.ViewManagerList>();
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand("SELECT tmvm_name FROM psav_dev.tm_viewmanage where tmvm_name like '" + vista + "%'", conn);
            conn.Open();
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Models.PSAVCrud.ViewManagerModel.ViewManagerList _ListVM = new Models.PSAVCrud.ViewManagerModel.ViewManagerList();
                _ListVM.VMName = sdr.GetValue(0).ToString();
                _ReturnList.Add(_ListVM);
            }
            conn.Close();
            return _ReturnList;
        }
        #endregion
        #region CRUD
        #region usuarios
        public List<Models.PSAVCrud.SyncModels.UsersModel> GetUsers()
        {
            string QuerySearchAll = "SELECT tmu_id,concat(tmp_firstname,' ',tmp_secondname,' ',tmp_lastname,' ',tmp_seclastname) as Nombre,tmu.tmu_username,tmu.tmu_expire,tmu.tmu_active " +
                                        "FROM psav_dev.tm_person tmp inner join psav_dev.tm_users tmu on tmu.tmp_id = tmp.tmp_id order by tmu.tmu_active";
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QuerySearchAll, conn);
            conn.Open();
            MySqlDataReader sdr = cmd.ExecuteReader();
            List<Models.PSAVCrud.SyncModels.UsersModel> UM = new List<Models.PSAVCrud.SyncModels.UsersModel>();
            while (sdr.Read())
            {
                Models.PSAVCrud.SyncModels.UsersModel UMO = new Models.PSAVCrud.SyncModels.UsersModel();
                UMO.ID = sdr.GetValue(0).ToString();
                UMO.Persona = sdr.GetValue(1).ToString();
                UMO.Username = sdr.GetValue(2).ToString();
                UMO.Expira = sdr.GetValue(3).ToString();
                UMO.Active = sdr.GetValue(4).ToString();
                UM.Add(UMO);
            }
            return UM;
        }
        public List<Models.PSAVCrud.Returning.UsuariosReturnModel> SearchUsers(string UserName)
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                string QuerySearchOne = "SELECT concat(tmp_firstname,' ',tmp_secondname,' ',tmp_lastname,' ',tmp_seclastname) as Nombre,tmu.tmu_username,tmu.tmu_expire,tmu.tmu_active " +
                                        "FROM psav_dev.tm_person tmp inner join psav_dev.tm_users tmu on tmu.tmp_id = tmp.tmp_id where tmu.tmu_username like '" + UserName + "%'order by tmu.tmu_active";
                MySqlConnection conn = new MySqlConnection(con);
                MySqlCommand cmd = new MySqlCommand(QuerySearchOne, conn);
                conn.Open();
                MySqlDataReader sdr = cmd.ExecuteReader();
                List<Models.PSAVCrud.Returning.UsuariosReturnModel> ReturnList = new List<Models.PSAVCrud.Returning.UsuariosReturnModel>();
                while (sdr.Read())
                {
                    Models.PSAVCrud.Returning.UsuariosReturnModel URM = new Models.PSAVCrud.Returning.UsuariosReturnModel();
                    URM.NombreCompleto = sdr.GetValue(0).ToString();
                    URM.Usuario = sdr.GetValue(1).ToString();
                    URM.Expira = sdr.GetValue(2).ToString();
                    URM.Activo = Convert.ToBoolean(sdr.GetValue(3)) == true ? "Activo" : "Inactivo";
                    ReturnList.Add(URM);
                }
                conn.Close();
                return ReturnList;
            }
            else
            {
                string QuerySearchAll = "SELECT concat(tmp_firstname,' ',tmp_secondname,' ',tmp_lastname,' ',tmp_seclastname) as Nombre,tmu.tmu_username,tmu.tmu_expire,tmu.tmu_active " +
                                        "FROM psav_dev.tm_person tmp inner join psav_dev.tm_users tmu on tmu.tmp_id = tmp.tmp_id order by tmu.tmu_active";
                MySqlConnection conn = new MySqlConnection(con);
                MySqlCommand cmd = new MySqlCommand(QuerySearchAll, conn);
                conn.Open();
                MySqlDataReader sdr = cmd.ExecuteReader();
                List<Models.PSAVCrud.Returning.UsuariosReturnModel> ReturnList = new List<Models.PSAVCrud.Returning.UsuariosReturnModel>();
                while (sdr.Read())
                {
                    Models.PSAVCrud.Returning.UsuariosReturnModel URM = new Models.PSAVCrud.Returning.UsuariosReturnModel();
                    URM.NombreCompleto = sdr.GetValue(0).ToString();
                    URM.Usuario = sdr.GetValue(1).ToString();
                    URM.Expira = sdr.GetValue(2).ToString();
                    URM.Activo = Convert.ToBoolean(sdr.GetValue(3)) == true ? "Activo" : "Inactivo";
                    ReturnList.Add(URM);
                }
                conn.Close();
                return ReturnList;
            }
            return new List<Models.PSAVCrud.Returning.UsuariosReturnModel>();
        }
        public string UpdateUsers(Models.PSAVCrud.SyncModels.UsersModel mod, int oper)
        {
            string Retorno = "";
            string QueryCoin = "";
            switch (oper)
            {
                case 0:
                    QueryCoin = "insert into psav_dev.tm_users (tmu_username,tmu_pwd,tmu_active,tmu_expire) values('" + mod.Username + "'," + mod.Password + "," + mod.Active + ",'"+mod.Expira+"')";
                    Retorno = SaveWithIDReturn(QueryCoin);
                    break;
                case 1:
                    QueryCoin = "update psav_dev.tm_users set tmu_username='" + mod.Username + "', tmu_pwd='" + mod.Password + "', tmu_active=" + mod.Active + " where tmu_id=" + mod.ID;
                    SaveWithoutValidation(QueryCoin);
                    break;
                case 2:
                    QueryCoin = "Delete from psav_dev.tm_users where tmu_id=" + mod.ID;
                    SaveWithoutValidation(QueryCoin);
                    break;
            }
            return Retorno;
        }
        #endregion
        #region Coins
        public List<Models.PSAVCrud.SyncModels.CoinModel> GetCoins()
        {
            List<Models.PSAVCrud.SyncModels.CoinModel> CoinsReturn = new List<Models.PSAVCrud.SyncModels.CoinModel>();
            string QuerySearchAll = "SELECT tcm_id,tcm_name,tcm_change,tcm_activo FROM psav_dev.tc_moneda";
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QuerySearchAll, conn);
            conn.Open();
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Models.PSAVCrud.SyncModels.CoinModel OneCoin = new Models.PSAVCrud.SyncModels.CoinModel();
                OneCoin.ID = sdr.GetValue(0).ToString();
                OneCoin.Name = sdr.GetValue(1).ToString();
                OneCoin.Change = Convert.ToDecimal(sdr.GetValue(2).ToString());
                OneCoin.Active = Convert.ToInt32(sdr.GetValue(3).ToString());
                CoinsReturn.Add(OneCoin);
            }
            sdr.Close();
            conn.Close();
            return CoinsReturn;
        }
        public string UpdateCoins(Models.PSAVCrud.SyncModels.CoinModel mod,int oper)
        {
            string Retorno = "";
            string QueryCoin = "";
            switch (oper)
            {
                case 0:
                    QueryCoin = "insert into psav_dev.tc_moneda (tcm_name,tcm_change,tcm_activo) values('" + mod.Name + "'," + mod.Change.ToString() + "," + mod.Active + ")";
                    Retorno=SaveWithIDReturn(QueryCoin);
                    break;
                case 1:
                    QueryCoin = "update psav_dev.tc_moneda set tcm_name='"+mod.Name+"', tcm_change="+mod.Change.ToString()+", tcm_activo="+mod.Active.ToString()+" where tcm_id=" + mod.ID;
                    SaveWithoutValidation(QueryCoin);
                    break;
                case 2:
                    QueryCoin = "Delete from psav_dev.tc_moneda where tcm_id=" + mod.ID;
                    SaveWithoutValidation(QueryCoin);
                    break;
            }
            return Retorno;
        }
        public List<Models.PSAVCrud.CoinsModel> GetCoin(string coin)
        {
            List<Models.PSAVCrud.CoinsModel> CoinsReturn = new List<Models.PSAVCrud.CoinsModel>();
            if (!string.IsNullOrEmpty(coin))
            {
                string QuerySearchOne = "SELECT * FROM psav_dev.tc_moneda where tcm_name like '" + coin + "%'";
                MySqlConnection conn = new MySqlConnection(con);
                MySqlCommand cmd = new MySqlCommand(QuerySearchOne, conn);
                conn.Open();
                MySqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    Models.PSAVCrud.CoinsModel OneCoin = new Models.PSAVCrud.CoinsModel();
                    OneCoin.CoinName = sdr.GetValue(1).ToString();
                    OneCoin.CoinValue = sdr.GetValue(2).ToString();
                    OneCoin.Activo = Convert.ToBoolean(sdr.GetValue(6)) == true ? "Activo" : "Inactivo";
                    OneCoin.LastChange = sdr.GetValue(5).ToString();
                    OneCoin.IDCoin = sdr.GetValue(0).ToString();
                    CoinsReturn.Add(OneCoin);
                }
                sdr.Close();
                conn.Close();
                return CoinsReturn;
            }
            else
            {
                string QuerySearchAll = "SELECT * FROM psav_dev.tc_moneda";
                MySqlConnection conn = new MySqlConnection(con);
                MySqlCommand cmd = new MySqlCommand(QuerySearchAll, conn);
                conn.Open();
                MySqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    Models.PSAVCrud.CoinsModel OneCoin = new Models.PSAVCrud.CoinsModel();
                    OneCoin.CoinName = sdr.GetValue(1).ToString();
                    OneCoin.CoinValue = sdr.GetValue(2).ToString();
                    OneCoin.Activo = Convert.ToBoolean(sdr.GetValue(6)) == true ? "Activo" : "Inactivo";
                    OneCoin.LastChange = sdr.GetValue(5).ToString();
                    OneCoin.IDCoin = sdr.GetValue(0).ToString();
                    CoinsReturn.Add(OneCoin);
                }
                sdr.Close();
                conn.Close();
                return CoinsReturn;
            }
            return new List<Models.PSAVCrud.CoinsModel>();
        }
        #endregion
        #region Clients
        public void SaveClient(Models.PSAVCrud.ClientModel.ClientNewAll model)
        {
            //inserta persona
            string QueryToInsert = "insert into psav_dev.tm_person(tmp_firstname,tmp_secondname,tmp_lastname,tmp_seclastname,tmp_dob,tmp_razonsocial,tmp_rfc,tmp_commercial)" +
                "values('" + model.PrimerNombre + "','" + model.SegundoNombre + "','" + model.PrimerApellido + "','" + model.SegundoApellido + "',null,'" + model.RazonSocial + "','" + model.RFC + "','" + model.NombreComercial + "')";
            string IDClient = SaveWithIDReturn(QueryToInsert);
            //inserta contacto
            QueryToInsert = "insert into psav_dev.tr_contactperson(tcct_id,trcp_data,tmp_id) values(1,'" + model.Telefono + "'," + IDClient + ")";
            SaveWithoutValidation(QueryToInsert);
            QueryToInsert = "insert into psav_dev.tr_contactperson(tcct_id,trcp_data,tmp_id) values(2,'" + model.Celular + "'," + IDClient + ")";
            SaveWithoutValidation(QueryToInsert);
            QueryToInsert = "insert into psav_dev.tr_contactperson(tcct_id,trcp_data,tmp_id) values(3,'" + model.EMail + "'," + IDClient + ")";
            SaveWithoutValidation(QueryToInsert);
            QueryToInsert = "insert into psav_dev.tr_contactperson(tcct_id,trcp_data,tmp_id) values(4,'" + model.Extension + "'," + IDClient + ")";
            SaveWithoutValidation(QueryToInsert);
            QueryToInsert = "insert into psav_dev.tr_contactperson(tcct_id,trcp_data,tmp_id) values(5,'" + model.NombreContacto + "'," + IDClient + ")";
            SaveWithoutValidation(QueryToInsert);
            //inserta domicilio
            QueryToInsert = "insert into psav_dev.tm_personaddress(tmp_id,tmpa_domicilio,tmpa_domiciliofiscal) values(" + IDClient + ",'" + model.Domicilio + "','" + model.DomicilioFiscal + "')";
            SaveWithoutValidation(QueryToInsert);
            //INSERTA CLIENTE
            QueryToInsert = "insert into psav_dev.tm_client(tmp_id,tmc_userins,tmc_dateins) values(" + IDClient + ",1,now())";
            SaveWithoutValidation(QueryToInsert);
        }
        public List<Models.PSAVCrud.ClientModel.ClientSearch> GetClients(string ClientData)
        {
            string QueryToSearch = "SELECT * FROM psav_dev.tm_person where tmp_razonsocial like '%' and tmp_id in (select tmp_id from tm_client)";
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QueryToSearch, conn);
            List<Models.PSAVCrud.ClientModel.ClientSearch> ClientReturn = new List<Models.PSAVCrud.ClientModel.ClientSearch>();
            conn.Open();
            MySqlDataReader msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                Models.PSAVCrud.ClientModel.ClientSearch CS = new Models.PSAVCrud.ClientModel.ClientSearch();
                CS.IDClient = msdr.GetValue(0).ToString();
                CS.Razon = msdr.GetValue(6).ToString();
                CS.RFC = msdr.GetValue(11).ToString();
                ClientReturn.Add(CS);
            }
            msdr.Close();
            conn.Close();
            for (int i = 0; i < ClientReturn.Count; i++)
            {
                string QueryToSearchAddress = "SELECT tmpa_domicilio,tmpa_domiciliofiscal FROM psav_dev.tm_personaddress where tmp_id=" + ClientReturn[i].IDClient;
                MySqlCommand cmdAdd = new MySqlCommand(QueryToSearchAddress, conn);
                conn.Open();
                MySqlDataReader msdrAddress = cmdAdd.ExecuteReader();
                while (msdrAddress.Read())
                {
                    ClientReturn[i].Domicilio = msdrAddress.GetValue(0).ToString();
                    ClientReturn[i].Fiscal = msdrAddress.GetValue(1).ToString();
                }
                msdrAddress.Close();
                conn.Close();
                string QueryToSearchFiscal = "SELECT * FROM psav_dev.tr_contactperson where tmp_id=" + ClientReturn[i].IDClient;
                MySqlCommand cmdFis = new MySqlCommand(QueryToSearchFiscal, conn);
                conn.Open();
                MySqlDataReader msdrContact = cmdFis.ExecuteReader();

                while (msdrContact.Read())
                {
                    switch (msdrContact.GetValue(1).ToString())
                    {
                        case "1"://telefono
                            ClientReturn[i].Tel = msdrContact.GetValue(2).ToString();
                            break;
                        case "2"://movil
                            ClientReturn[i].Cel = msdrContact.GetValue(2).ToString();
                            break;
                        case "3"://correo
                            ClientReturn[i].Email = msdrContact.GetValue(2).ToString();
                            break;
                        case "4"://extension
                            ClientReturn[i].Ext = msdrContact.GetValue(2).ToString();
                            break;
                        case "5"://nombre
                            ClientReturn[i].NombreContacto = msdrContact.GetValue(2).ToString();
                            break;
                    }
                }
                conn.Close();
            }
            
            return ClientReturn;
        }
        #endregion
        #region Roles
        public List<Models.PSAVCrud.RolesModel> GetRoles()
        {
            List<Models.PSAVCrud.RolesModel> LRM = new List<Models.PSAVCrud.RolesModel>();
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM psav_dev.tm_roles", conn);
            conn.Open();
            MySqlDataReader msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                Models.PSAVCrud.RolesModel rm = new Models.PSAVCrud.RolesModel();
                rm.ID = msdr.GetValue(0).ToString();
                rm.Nombre = msdr.GetValue(1).ToString();
                rm.Descripcion = msdr.GetValue(2).ToString();
                LRM.Add(rm);
            }
            conn.Close();
            return LRM;            
        }
        public string UpdateRole(Models.PSAVCrud.RolesModel mod, int oper)
        {
            string query = "update psav_dev.tm_roles set tmr_name='"+mod.Nombre+"', tmr_description='"+mod.Descripcion+"' where tmr_id=" + mod.ID;
            if (oper.Equals(0))
            {
                query = "insert into psav_dev.tm_roles(tmr_name,tmr_description) values('" + mod.Nombre + "','" + mod.Descripcion + "')";
                return SaveWithIDReturn(query);
            }
            if (oper.Equals(2))
                query = "delete from psav_dev.tm_roles where tmr_id=" + mod.ID;
            SaveWithoutValidation(query);
            return "";
        }
#endregion
        #endregion
        #region EPT
        
        public void UpdateOL(List<Models.SyncPSAV.FreelanceOL> FOList, List<Models.SyncPSAV.Viaticos> ViaticosL, List<Models.SyncPSAV.VentasFeeTot> ComVenL,
            List<Models.SyncPSAV.GastosFinancieros> GasFinL, List<Models.SyncPSAV.Consumibles> ConsuL, List<Models.SyncPSAV.CargosInternos> CIntL, string idevt)
        {
            //freelance
            SaveWithoutValidation("delete from td_eptfreelance where tme_id=" + idevt);
            for (int i = 0; i < FOList.Count; i++)
            {
                string QueryInsert = "insert into td_eptfreelance(tdef_nombre,tdef_puesto,tdef_dias,tdef_sueldo,tdef_condiciones,tdef_costo,tdef_total,tme_id) values "
                     + "('" + FOList[i].Nombres + "','" + FOList[i].Puesto + "','" + FOList[i].Dias + "','" + FOList[i].Sueldo + "','" + FOList[i].Condiciones + "','" + FOList[i].CostoCarga + "','" + FOList[i].CostoTotal + "'," + idevt + ")";
                SaveWithoutValidation(QueryInsert);
            }
            //viaticos
            SaveWithoutValidation("delete from td_eptviaticos where tme_id="+idevt);
            for (int i = 0; i < ViaticosL.Count; i++)
            {
                string QueryInsert = "insert into td_eptviaticos(tdev_nombre,tdev_puesto,tdev_observaciones,tdev_totalsol,tme_id) values('" + ViaticosL[i].Nombres + "','" + ViaticosL[i].Puesto + "','" + ViaticosL[i].Observaciones + "','" + ViaticosL[i].TotalSol + "'," + idevt + ")";
                SaveWithoutValidation(QueryInsert);
            }
            //com venta total
            SaveWithoutValidation("delete from td_eptvfeetot where tme_id=" + idevt);
            for (int i = 0; i < ComVenL.Count; i++)
            {
                string QueryInsert = "insert into td_eptvfeetot(tdevf_nombre,tdevf_puesto,tdevf_comision,tdevf_ventaneta,tdevf_comisiontot,tme_id) values('" + ComVenL[i].Nombres + "','" + ComVenL[i].Puesto + "','" + ComVenL[i].Comision + "','" + ComVenL[i].VentaNeta + "','" + ComVenL[i].Comisiontot + "'," + idevt + ")";
                SaveWithoutValidation(QueryInsert);
            }
            //gastos financieros
            SaveWithoutValidation("delete from td_eptgfin where tme_id="+idevt);
            for (int i = 0; i < GasFinL.Count; i++)
            {
                string QueryInsert = "insert into td_eptgfin(tdegf_comision,tdegf_importe,tdegf_porccom,tegf_imporcom,tme_id) values ('" + GasFinL[i].Comision + "','" + GasFinL[i].Importe + "','" + GasFinL[i].PorcCom + "','" + GasFinL[i].ImporteCom + "'," + idevt + ")";
                SaveWithoutValidation(QueryInsert);
            }
            //consul
            SaveWithoutValidation("delete from td_eptconsumible where tme_id="+idevt);
            for (int i = 0; i < ConsuL.Count; i++)
            {
                string QueryInsert = "insert into td_eptconsumible(tdec_cotizacion,tdec_supplier,tdec_description,tec_costo,tme_id) values ('" + ConsuL[i].Cotizacion + "','" + ConsuL[i].Supplier + "','" + ConsuL[i].Description + "','" + ConsuL[i].Costo + "'," + idevt + ")";
                SaveWithoutValidation(QueryInsert);
            }
            //cargos internos
            SaveWithoutValidation("delete from td_eptcinternos where tme_id="+idevt);
            for (int i = 0; i < CIntL.Count; i++)
            {
                string QueryInsert = "insert into td_eptcinternos(tdeci_equipo,tdeci_categoria,tdeci_preciolista,tdeci_tipooper,tdeci_porccargo,tdeci_montocargo,tme_id) value('" + CIntL[i].Equipo + "','" + CIntL[i].Categoria + "','" + CIntL[i].PrecioLista + "','" + CIntL[i].TipoOper + "','" + CIntL[i].PorcCargo + "','" + CIntL[i].MontoCargo + "'," + idevt + ")";
                SaveWithoutValidation(QueryInsert);
            }
        }
        public void SaveOL(List<Models.SyncPSAV.FreelanceOL> FOList, List<Models.SyncPSAV.Viaticos> ViaticosL, List<Models.SyncPSAV.VentasFeeTot> ComVenL,
            List<Models.SyncPSAV.GastosFinancieros> GasFinL, List<Models.SyncPSAV.Consumibles> ConsuL, List<Models.SyncPSAV.CargosInternos> CIntL,string idevt)
        {
            //freelance
            for(int i = 0; i < FOList.Count; i++)
            {
                string QueryInsert = "insert into td_eptfreelance(tdef_nombre,tdef_puesto,tdef_dias,tdef_sueldo,tdef_condiciones,tdef_costo,tdef_total,tme_id) values " 
                     + "('"+FOList[i].Nombres+"','"+FOList[i].Puesto+"','"+FOList[i].Dias+"','"+FOList[i].Sueldo+"','"+FOList[i].Condiciones+"','"+FOList[i].CostoCarga+"','"+FOList[i].CostoTotal+"',"+idevt+")";
                SaveWithoutValidation(QueryInsert);
            }
            //viaticos
            for(int i = 0; i < ViaticosL.Count; i++)
            {
                string QueryInsert = "insert into td_eptviaticos(tdev_nombre,tdev_puesto,tdev_observaciones,tdev_totalsol,tme_id) values('"+ViaticosL[i].Nombres+"','"+ViaticosL[i].Puesto+"','"+ViaticosL[i].Observaciones+"','"+ViaticosL[i].TotalSol+"',"+idevt+")";
                SaveWithoutValidation(QueryInsert);
            }
            //com venta total
            for (int i = 0; i < ComVenL.Count; i++)
            {
                string QueryInsert = "insert into td_eptvfeetot(tdevf_nombre,tdevf_puesto,tdevf_comision,tdevf_ventaneta,tdevf_comisiontot,tme_id) values('" + ComVenL[i].Nombres + "','" + ComVenL[i].Puesto + "','" + ComVenL[i].Comision +"','"+ComVenL[i].VentaNeta+"','"+ComVenL[i].Comisiontot+"',"+idevt+")";
                SaveWithoutValidation(QueryInsert);
            }
            //gastos financieros
            for(int i = 0; i<GasFinL.Count; i++)
            {
                string QueryInsert = "insert into td_eptgfin(tdegf_comision,tdegf_importe,tdegf_porccom,tegf_imporcom,tme_id) values ('" + GasFinL[i].Comision+"','"+GasFinL[i].Importe+"','"+GasFinL[i].PorcCom+"','"+GasFinL[i].ImporteCom+"',"+idevt+")";
                SaveWithoutValidation(QueryInsert);
            }
            //consul
            for(int i = 0; i < ConsuL.Count; i++)
            {
                string QueryInsert = "insert into td_eptconsumible(tdec_cotizacion,tdec_supplier,tdec_description,tec_costo,tme_id) values ('"+ConsuL[i].Cotizacion+"','"+ConsuL[i].Supplier+"','"+ConsuL[i].Description+"','"+ConsuL[i].Costo+"',"+idevt+")";
                SaveWithoutValidation(QueryInsert);
            }
            //cargos internos
            for(int i = 0; i < CIntL.Count; i++)
            {
                string QueryInsert = "insert into td_eptcinternos(tdeci_equipo,tdeci_categoria,tdeci_preciolista,tdeci_tipooper,tdeci_porccargo,tdeci_montocargo,tme_id) value('"+CIntL[i].Equipo+"','"+CIntL[i].Categoria+"','"+CIntL[i].PrecioLista+"','"+CIntL[i].TipoOper+"','"+CIntL[i].PorcCargo+"','"+CIntL[i].MontoCargo+"',"+idevt+")";
                SaveWithoutValidation(QueryInsert);
            }
        }
        public void UpdateSRenta(List<Models.SyncPSAV.SubRenta> lmod,string idevt)
        {
            SaveWithoutValidation("delete from td_eptsrent where tme_id=" + idevt);
            for (int i = 0; i < lmod.Count; i++)
            {
                string QueryVFee = "insert into td_eptsrent(Category,Invoice,Supplier,Descripcion,Gastosub,Ventasub,tme_id)" +
                    "values('" + lmod[i].Category + "','" + lmod[i].Invoice + "','" + lmod[i].Supplier + "','" + lmod[i].Descripcion + "','" + lmod[i].Gastosub + "','" + lmod[i].Ventasub + "'," + idevt + ")";
                SaveWithoutValidation(QueryVFee);
            }
        }
        public List<Models.SyncPSAV.SubRenta> GetSubRenta(string idevt)
        {
            List<Models.SyncPSAV.SubRenta> SRL = new List<Models.SyncPSAV.SubRenta>();
            List<Models.SyncPSAV.VentaFee> VFEe = new List<Models.SyncPSAV.VentaFee>();
            string QuerySearch = "SELECT * FROM psav_dev.td_eptsrent where tme_id=" + idevt;
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QuerySearch, conn);
            conn.Open();
            MySqlDataReader msdr = cmd.ExecuteReader();
            while (msdr.Read()) {
                Models.SyncPSAV.SubRenta SR = new Models.SyncPSAV.SubRenta();
                SR.Category = msdr.GetValue(1).ToString();
                SR.Invoice = msdr.GetValue(2).ToString();
                SR.Supplier = msdr.GetValue(3).ToString();
                SR.Descripcion = msdr.GetValue(4).ToString();
                SR.Gastosub = msdr.GetValue(5).ToString();
                SR.Ventasub = msdr.GetValue(6).ToString();
                SRL.Add(SR);
            }
            conn.Close();
            return SRL;
        }
        public void UpdateVFee(List<Models.SyncPSAV.VentaFee> mod,string idevt)
        {
            SaveWithoutValidation("delete from td_eptvfee where tme_id="+ idevt);
            for(int i = 0; i < mod.Count; i++)
            {
                string QueryVFee = "insert into td_eptvfee(Category,BaseFee,PorcFee,SubFee,ImporteFee,tme_id)" +
                    "values('" + mod[i].Category + "','" + mod[i].BaseFee + "','" + mod[i].PorcFee + "','" + mod[i].SubFee + "','" + mod[i].ImporteFee + "'," + idevt + ")";
                SaveWithoutValidation(QueryVFee);
            }
        }
        public List<Models.SyncPSAV.VentaFee> GetVFee(string idevt)
        {
            List<Models.SyncPSAV.VentaFee> VFEe = new List<Models.SyncPSAV.VentaFee>();
            string QuerySearch = "SELECT * FROM psav_dev.td_eptvfee where tme_id=" + idevt;
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QuerySearch, conn);
            conn.Open();
            MySqlDataReader msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                Models.SyncPSAV.VentaFee VF = new Models.SyncPSAV.VentaFee();
                VF.Category = msdr.GetValue(1).ToString();
                VF.BaseFee = msdr.GetValue(2).ToString();
                VF.PorcFee = msdr.GetValue(3).ToString();
                VF.SubFee = msdr.GetValue(4).ToString();
                VF.ImporteFee = msdr.GetValue(5).ToString();
                VFEe.Add(VF);
            }
            conn.Close();
            return VFEe;
        }
        public void UpdateVtaDesc(List<Models.SyncPSAV.VentaDes> lvdes,string idvt)
        {
            SaveWithoutValidation("delete from td_eptvdesc where tme_id=" + idvt);
            for(int i = 0; i < lvdes.Count; i++)
            {
                string QueryVDesc = "insert into psav_dev.td_eptvdesc(tme_id,Category,VentaEqui,VentaEquEx,TotalVenta,DesPorEq,TotalDescEPS,DescExt,TotalExt,TotalDesc,PorcTotalDesc,AplicaAut)"
                     + "values(" + idvt + ",'" + lvdes[i].Category + "','" + lvdes[i].VentaEqui + "','" + lvdes[i].VentaEquEx + "','" + lvdes[i].TotalVenta + "','" + lvdes[i].DesPorEq + "','" + lvdes[i].TotalDescEPS + "','" + lvdes[i].DescExt + "','" + lvdes[i].TotalExt + "','" + lvdes[i].TotalDesc + "','" + lvdes[i].PorcTotalDesc + "','" + lvdes[i].AplicaAut + "')";
                SaveWithoutValidation(QueryVDesc);
            }
        }
        public List<Models.SyncPSAV.VentaDes> GetVtaDesc(string idevt)
        {
            string QuerySearch = "SELECT * FROM psav_dev.td_eptvdesc where tme_id="+idevt;
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QuerySearch, conn);
            conn.Open();
            MySqlDataReader msdr = cmd.ExecuteReader();
            List<Models.SyncPSAV.VentaDes> Vdes = new List<Models.SyncPSAV.VentaDes>();
            while (msdr.Read())
            {
                Models.SyncPSAV.VentaDes VD = new Models.SyncPSAV.VentaDes();
                VD.Category = msdr.GetValue(1).ToString();
                VD.VentaEqui = msdr.GetValue(2).ToString();
                VD.VentaEquEx = msdr.GetValue(3).ToString();
                VD.TotalVenta = msdr.GetValue(4).ToString();
                VD.DesPorEq = msdr.GetValue(5).ToString();
                VD.TotalDescEPS = msdr.GetValue(6).ToString();
                VD.DescExt = msdr.GetValue(7).ToString();
                VD.TotalExt = msdr.GetValue(8).ToString();
                VD.TotalDesc = msdr.GetValue(9).ToString();
                VD.PorcTotalDesc = msdr.GetValue(10).ToString();
                VD.AplicaAut = msdr.GetValue(11).ToString();
                Vdes.Add(VD);
            }
            conn.Close();
            return Vdes;
        }
        public void UpdateITL(Models.SyncPSAV.SalonIL Salon,List<Models.SyncPSAV.ItemListServices> Ilserv,string idtl)
        {
            string iltt = "";
            try
            {
                string QuerySalon = "Update tm_itemlistevt set tmitl_salon='" + Salon.Salon + "', tmitl_asistentes='" + Salon.Asistentes + "',tmitl_montaje='" + Salon.Montaje + "',tmitl_horario='" + Salon.Horario + "' where tmitl_id=" + idtl;
                SaveWithoutValidation(QuerySalon);
            }
            catch {
                String QuerySalon = "insert into tm_itemlistevt(tme_id,tmitl_salon,tmitl_asistentes,tmitl_montaje,tmitl_horario) values(" + Salon.IDEvt + ",'" + Salon.Salon + "','" + Salon.Asistentes + "','" + Salon.Montaje + "','" + Salon.Horario + "')";
                iltt = SaveWithIDReturn(QuerySalon);
            }
            try
            {
                SaveWithoutValidation("delete from td_itemlist where tmilt_id=" + idtl);
            }
            catch { }
            if (string.IsNullOrEmpty(idtl)) { idtl = iltt; }
            for(int i = 0; i < Ilserv.Count; i++)
            {
                string ConsSQL = "insert into td_itemlist(tmilt_id,tdil_clave,tdil_cantidad,tdil_des,tdil_preciounit,tcct_id,tme_id) values(" + idtl + ",'" + Ilserv[i].Clave + "','" + Ilserv[i].Cantidad + "','" + Ilserv[i].Descripcion + "','" + Ilserv[i].PrecioUnit + "','" + Ilserv[i].Categoria + "'," + Salon.IDEvt + ")";
                SaveWithoutValidation(ConsSQL);
            }
        }
        public void UpdateITLWF(Models.SyncPSAV.SalonILWF Salon, List<Models.SyncPSAV.ItemListWorkForce> Ilserv, string idtl)
        {
            string ILTT = "";
            try
            {
                string QuerySalon = "Update tm_itemlistevtwf set tmitl_salon='" + Salon.Salon + "', tmitl_asistentes='" + Salon.Asistentes + "',tmitl_montaje='" + Salon.Montaje + "',tmitl_horario='" + Salon.Horario + "' where tmitlwf_id=" + idtl;
                SaveWithoutValidation(QuerySalon);
            }
            catch {
                string QuerySalon= "insert into tm_itemlistevtwf(tme_id, tmitl_salon, tmitl_asistentes, tmitl_montaje, tmitl_horario) values(" + Salon.IDEvt + ", '" + Salon.Salon + "', '" + Salon.Asistentes + "', '" + Salon.Montaje + "', '" + Salon.Horario + "')";
                ILTT= SaveWithIDReturn(QuerySalon);
            }
            try
            {
                SaveWithoutValidation("delete from td_itemlistwf where tmilt_id=" + idtl);
            }
            catch { }
            try
            {
                if (string.IsNullOrEmpty(idtl)) { idtl = ILTT; }
                for (int i = 0; i < Ilserv.Count; i++)
                {
                    string ConsSQL = "insert into td_itemlistwf(tmilt_id,tdil_clave,tdil_cantidad,tdil_des,tdil_preciounit,tcct_id,tme_id) values(" + idtl + ",'" + Ilserv[i].Clave + "','" + Ilserv[i].Cantidad + "','" + Ilserv[i].Descripcion + "','" + Ilserv[i].PrecioUnit + "','" + Ilserv[i].Categoria + "'," + Salon.IDEvt + ")";
                    SaveWithoutValidation(ConsSQL);
                }
            }
            catch { }
        }
        public string GetFolioByITL(string idtl)
        {
            string QueryGetFolio = "SELECT tme_folio FROM psav_dev.tm_ept where tme_id=" + idtl;
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QueryGetFolio, conn);
            conn.Open();
            string Folio = cmd.ExecuteScalar().ToString();
            conn.Close();
            return Folio;
        }
        public List<Models.SyncPSAV.ItemListWorkForce> LILSWF(string idtl)
        {
            string QueryILIL = "SELECT * FROM psav_dev.td_itemlistwf where tmilt_id=" + idtl;
            List<Models.SyncPSAV.ItemListWorkForce> ILIL = new List<Models.SyncPSAV.ItemListWorkForce>();
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QueryILIL, conn);
            conn.Open();
            MySqlDataReader msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                Models.SyncPSAV.ItemListWorkForce Ils = new Models.SyncPSAV.ItemListWorkForce();
                Ils.ID = msdr.GetValue(0).ToString();
                Ils.IDITL = msdr.GetValue(1).ToString();
                Ils.Clave = msdr.GetValue(2).ToString();
                Ils.Cantidad = msdr.GetValue(3).ToString();
                Ils.Descripcion = msdr.GetValue(4).ToString();
                Ils.PrecioUnit = msdr.GetValue(5).ToString();
                Ils.Categoria = msdr.GetValue(6).ToString();
                Ils.IDEvento = Convert.ToInt32(msdr.GetValue(7).ToString());
                ILIL.Add(Ils);
            }
            conn.Close();
            return ILIL;
        }
        public List<Models.SyncPSAV.ItemListServices> LILS(string idtl)
        {
            string QueryILIL = "SELECT * FROM psav_dev.td_itemlist where tmilt_id=" + idtl;
            List<Models.SyncPSAV.ItemListServices> ILIL = new List<Models.SyncPSAV.ItemListServices>();
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QueryILIL, conn);
            conn.Open();
            MySqlDataReader msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                Models.SyncPSAV.ItemListServices Ils = new Models.SyncPSAV.ItemListServices();
                Ils.ID = msdr.GetValue(0).ToString();
                Ils.IDITL = msdr.GetValue(1).ToString();
                Ils.Clave = msdr.GetValue(2).ToString();
                Ils.Cantidad = msdr.GetValue(3).ToString();
                Ils.Descripcion = msdr.GetValue(4).ToString();
                Ils.PrecioUnit = msdr.GetValue(5).ToString();
                Ils.Categoria = msdr.GetValue(6).ToString();
                Ils.IDEvento = Convert.ToInt32(msdr.GetValue(7).ToString());
                ILIL.Add(Ils);
            }
            conn.Close();
            return ILIL;
        }
        public Models.SyncPSAV.SalonILWF GetOneSalonILWF(string idtl)
        {
            string QueryILIL = "SELECT * FROM psav_dev.tm_itemlistevtwf where tmitlwf_id=" + idtl;
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QueryILIL, conn);
            conn.Open();
            MySqlDataReader msdr = cmd.ExecuteReader();
            Models.SyncPSAV.SalonILWF SIL = new Models.SyncPSAV.SalonILWF();
            while (msdr.Read())
            {
                SIL.IDEvt = msdr.GetValue(1).ToString();
                SIL.ID = msdr.GetValue(0).ToString();
                SIL.Salon = msdr.GetValue(2).ToString();
                SIL.Asistentes = msdr.GetValue(3).ToString();
                SIL.Montaje = msdr.GetValue(4).ToString();
                SIL.Horario = msdr.GetValue(5).ToString();
            }
            conn.Close();
            return SIL;
        }
        public Models.SyncPSAV.SalonIL GetOneSalonIL(string idtl)
        {
            string QueryILIL = "SELECT * FROM psav_dev.tm_itemlistevt where tmitl_id=" + idtl;
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QueryILIL, conn);
            conn.Open();
            MySqlDataReader msdr = cmd.ExecuteReader();
            Models.SyncPSAV.SalonIL SIL = new Models.SyncPSAV.SalonIL();
            while (msdr.Read())
            {                
                SIL.IDEvt = msdr.GetValue(1).ToString();
                SIL.ID = msdr.GetValue(0).ToString();
                SIL.Salon = msdr.GetValue(2).ToString();
                SIL.Asistentes = msdr.GetValue(3).ToString();
                SIL.Montaje = msdr.GetValue(4).ToString();
                SIL.Horario = msdr.GetValue(5).ToString();                
            }
            conn.Close();
            return SIL;
        }
        public List<Models.SyncPSAV.SalonIL> GetSalons(string idevt)
        {
            string QueryILIL = "SELECT * FROM psav_dev.tm_itemlistevt where tme_id=" + idevt;
            List<Models.SyncPSAV.SalonIL> LSIL = new List<Models.SyncPSAV.SalonIL>();
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QueryILIL, conn);
            conn.Open();
            MySqlDataReader msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                Models.SyncPSAV.SalonIL SIL = new Models.SyncPSAV.SalonIL();
                SIL.IDEvt = idevt;
                SIL.ID = msdr.GetValue(0).ToString();
                SIL.IDITL = msdr.GetValue(0).ToString();
                SIL.Salon = msdr.GetValue(2).ToString();
                SIL.Asistentes = msdr.GetValue(3).ToString();
                SIL.Montaje = msdr.GetValue(4).ToString();
                SIL.Horario = msdr.GetValue(5).ToString();
                LSIL.Add(SIL);
            }
            conn.Close();
            return LSIL;
            }
        public List<Models.SyncPSAV.SalonILWF> GetSalonsWF(string idevt)
        {
            string QueryILIL = "SELECT * FROM psav_dev.tm_itemlistevtwf where tme_id=" + idevt;
            List<Models.SyncPSAV.SalonILWF> LSIL = new List<Models.SyncPSAV.SalonILWF>();
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QueryILIL, conn);
            conn.Open();
            MySqlDataReader msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                Models.SyncPSAV.SalonILWF SIL = new Models.SyncPSAV.SalonILWF();
                SIL.IDEvt = idevt;
                SIL.ID = msdr.GetValue(0).ToString();
                SIL.IDITL= msdr.GetValue(0).ToString();
                SIL.Salon = msdr.GetValue(2).ToString();
                SIL.Asistentes = msdr.GetValue(3).ToString();
                SIL.Montaje = msdr.GetValue(4).ToString();
                SIL.Horario = msdr.GetValue(5).ToString();
                LSIL.Add(SIL);
            }
            conn.Close();
            return LSIL;
        }
        public List<Models.SyncPSAV.ItemListServices> GetOneILIL(string idevt)
        {
            string QueryILIL = "SELECT * FROM psav_dev.td_itemlist where tme_id="+idevt;
            List<Models.SyncPSAV.ItemListServices> ILIL = new List<Models.SyncPSAV.ItemListServices>();
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QueryILIL, conn);
            conn.Open();
            MySqlDataReader msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                Models.SyncPSAV.ItemListServices Ils = new Models.SyncPSAV.ItemListServices();
                Ils.ID = msdr.GetValue(0).ToString();
                Ils.IDITL = msdr.GetValue(1).ToString();
                Ils.Clave = msdr.GetValue(2).ToString();
                Ils.Cantidad = msdr.GetValue(3).ToString();
                Ils.Descripcion = msdr.GetValue(4).ToString();
                Ils.PrecioUnit = msdr.GetValue(5).ToString();
                Ils.Categoria = msdr.GetValue(6).ToString();
                Ils.IDEvento = Convert.ToInt32(msdr.GetValue(7).ToString());
                ILIL.Add(Ils);
            }
            conn.Close();
            return ILIL;
        }
        public List<Models.SyncPSAV.ItemListWorkForce> GetOneILWF(string idevt)
        {
            List<Models.SyncPSAV.ItemListWorkForce> ILWF = new List<Models.SyncPSAV.ItemListWorkForce>();
            string QueryILIL = "SELECT * FROM psav_dev.td_itemlist where tme_id=" + idevt;
            return ILWF;
        }
        public List<Models.PSAVCrud.ClientModel.ClientAutoComplete> GetAutoClients()
        {
            List<Models.PSAVCrud.ClientModel.ClientAutoComplete> Cients = new List<Models.PSAVCrud.ClientModel.ClientAutoComplete>();
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand("SELECT concat(tmp.tmp_razonsocial), tmc_id FROM psav_dev.tm_client tmc inner join tm_person tmp on tmp.tmp_id=tmc.tmp_id", conn);
            conn.Open();
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Models.PSAVCrud.ClientModel.ClientAutoComplete _ListVM = new Models.PSAVCrud.ClientModel.ClientAutoComplete();
                _ListVM.FullName = sdr.GetValue(0).ToString();
                _ListVM.IDClient = sdr.GetValue(1).ToString();
                Cients.Add(_ListVM);
            }
            conn.Close();
            return Cients;
        }
        public List<Models.PSAVCrud.ClientModel.ClientSearch> GetClientID(string razonsocial)
        {
            List<Models.PSAVCrud.ClientModel.ClientSearch> Cients = new List<Models.PSAVCrud.ClientModel.ClientSearch>();
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand("SELECT tmp.tmp_id,tmp.tmp_razonsocial,tmpa.tmpa_domicilio,tmpa.tmpa_domiciliofiscal,tmp.tmp_rfc,(select trcp_data from tr_contactperson where tmp_id=tmp.tmp_id and tcct_id=1) as tel,(select trcp_data from tr_contactperson where tmp_id=tmp.tmp_id and tcct_id=2) as cel,(select trcp_data from tr_contactperson where tmp_id=tmp.tmp_id and tcct_id=3) as email,(select trcp_data from tr_contactperson where tmp_id=tmp.tmp_id and tcct_id=4) as ext,(select trcp_data from tr_contactperson where tmp_id=tmp.tmp_id and tcct_id=5) as nombrecontacto FROM psav_dev.tm_client tmc inner join tm_person tmp on tmp.tmp_id=tmc.tmp_id inner join tm_personaddress tmpa on tmpa.tmp_id=tmp.tmp_id where tmc.tmc_id=" + razonsocial, conn);
            conn.Open();
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Models.PSAVCrud.ClientModel.ClientSearch _ListVM = new Models.PSAVCrud.ClientModel.ClientSearch();
                _ListVM.IDClient = sdr.GetValue(0).ToString();
                _ListVM.Razon = sdr.GetValue(1).ToString();
                _ListVM.Domicilio = sdr.GetValue(2).ToString();
                _ListVM.Fiscal = sdr.GetValue(3).ToString();
                _ListVM.RFC = sdr.GetValue(4).ToString();
                _ListVM.Tel = sdr.GetValue(5).ToString();
                _ListVM.Cel = sdr.GetValue(6).ToString();
                _ListVM.Email = sdr.GetValue(7).ToString();
                _ListVM.Ext = sdr.GetValue(8).ToString();
                _ListVM.NombreContacto = sdr.GetValue(9).ToString();
                Cients.Add(_ListVM);
            }
            conn.Close();
            return Cients;
        }
        public List<Models.PSAVCrud.ClientModel.ClientSearch> GetClientOne(string IDC)
        {
            List<Models.PSAVCrud.ClientModel.ClientSearch> Cients = new List<Models.PSAVCrud.ClientModel.ClientSearch>();
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand("SELECT tmp.tmp_id,tmp.tmp_razonsocial,tmpa.tmpa_domicilio,tmpa.tmpa_domiciliofiscal,tmp.tmp_rfc,(select trcp_data from tr_contactperson where tmp_id=tmp.tmp_id and tcct_id=1) as tel,(select trcp_data from tr_contactperson where tmp_id=tmp.tmp_id and tcct_id=2) as cel,(select trcp_data from tr_contactperson where tmp_id=tmp.tmp_id and tcct_id=3) as email,(select trcp_data from tr_contactperson where tmp_id=tmp.tmp_id and tcct_id=4) as ext,(select trcp_data from tr_contactperson where tmp_id=tmp.tmp_id and tcct_id=5) as nombrecontacto,tmc.tmc_id FROM psav_dev.tm_client tmc inner join tm_person tmp on tmp.tmp_id=tmc.tmp_id inner join tm_personaddress tmpa on tmpa.tmp_id=tmp.tmp_id where tmc.tmc_id="+IDC, conn);
            conn.Open();
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Models.PSAVCrud.ClientModel.ClientSearch _ListVM = new Models.PSAVCrud.ClientModel.ClientSearch();
                _ListVM.IDClient = sdr.GetValue(10).ToString();
                _ListVM.Razon = sdr.GetValue(1).ToString();
                _ListVM.Domicilio = sdr.GetValue(2).ToString();
                _ListVM.Fiscal = sdr.GetValue(3).ToString();
                _ListVM.RFC = sdr.GetValue(4).ToString();
                _ListVM.Tel = sdr.GetValue(5).ToString();
                _ListVM.Cel = sdr.GetValue(6).ToString();
                _ListVM.Email = sdr.GetValue(7).ToString();
                _ListVM.Ext = sdr.GetValue(8).ToString();
                _ListVM.NombreContacto = sdr.GetValue(9).ToString();
                _ListVM.IDEmpresa = sdr.GetValue(0).ToString();
                Cients.Add(_ListVM);
            }
            conn.Close();
            return Cients;
        }
        public List<Models.PSAVCrud.ClientModel.ClientSearch> GetClient(string razonsocial)
        {
            List<Models.PSAVCrud.ClientModel.ClientSearch> Cients = new List<Models.PSAVCrud.ClientModel.ClientSearch>();
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand("SELECT tmp.tmp_id,tmp.tmp_razonsocial,tmpa.tmpa_domicilio,tmpa.tmpa_domiciliofiscal,tmp.tmp_rfc,(select trcp_data from tr_contactperson where tmp_id=tmp.tmp_id and tcct_id=1) as tel,(select trcp_data from tr_contactperson where tmp_id=tmp.tmp_id and tcct_id=2) as cel,(select trcp_data from tr_contactperson where tmp_id=tmp.tmp_id and tcct_id=3) as email,(select trcp_data from tr_contactperson where tmp_id=tmp.tmp_id and tcct_id=4) as ext,(select trcp_data from tr_contactperson where tmp_id=tmp.tmp_id and tcct_id=5) as nombrecontacto,tmc.tmc_id FROM psav_dev.tm_client tmc inner join tm_person tmp on tmp.tmp_id=tmc.tmp_id inner join tm_personaddress tmpa on tmpa.tmp_id=tmp.tmp_id where tmp.tmp_razonsocial like '" + razonsocial + "%'", conn);
            conn.Open();
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Models.PSAVCrud.ClientModel.ClientSearch _ListVM = new Models.PSAVCrud.ClientModel.ClientSearch();
                _ListVM.IDClient = sdr.GetValue(10).ToString();
                _ListVM.Razon = sdr.GetValue(1).ToString();
                _ListVM.Domicilio = sdr.GetValue(2).ToString();
                _ListVM.Fiscal = sdr.GetValue(3).ToString();
                _ListVM.RFC = sdr.GetValue(4).ToString();
                _ListVM.Tel = sdr.GetValue(5).ToString();
                _ListVM.Cel = sdr.GetValue(6).ToString();
                _ListVM.Email = sdr.GetValue(7).ToString();
                _ListVM.Ext = sdr.GetValue(8).ToString();
                _ListVM.NombreContacto = sdr.GetValue(9).ToString();
                _ListVM.IDEmpresa = sdr.GetValue(0).ToString();
                Cients.Add(_ListVM);
            }
            conn.Close();
            return Cients;
        }
        public List<Models.ItemListModel.CategoryItemList> GetCategoryItemList(int TypeCat)
        {
            string ConsSQLSearch = "SELECT * FROM psav_dev.tc_category where tcc_type=" + TypeCat.ToString();
            List<Models.ItemListModel.CategoryItemList> Catego = new List<Models.ItemListModel.CategoryItemList>();
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(ConsSQLSearch, conn);
            conn.Open();
            MySqlDataReader sdr = cmd.ExecuteReader();
            while (sdr.Read())
            {
                Models.ItemListModel.CategoryItemList CVM = new Models.ItemListModel.CategoryItemList();
                CVM.ID = sdr.GetValue(0).ToString();
                CVM.Catego = sdr.GetValue(1).ToString();
                Catego.Add(CVM);
            }
            conn.Close();
            return Catego;
        }
        public List<Models.EPTModel.EPTConsult> EPTSearchOne(string ParamStr)
        {
            List<Models.EPTModel.EPTConsult> EPTList = new List<Models.EPTModel.EPTConsult>();
            string ConsSQLSearch = "select ept.tme_folio,eptevt.tdee_nombre,per.tmp_razonsocial,eptevt.tdee_fecmontaje from tm_ept ept inner join td_eptevt eptevt on eptevt.tme_id = ept.tme_id inner join tm_client cli on cli.tmc_id = ept.tmc_id inner join tm_person per on per.tmp_id = cli.tmp_id where ept.tme_folio like '"+ParamStr+"%' order by eptevt.tdee_fecmontaje desc ";
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(ConsSQLSearch, conn);
            conn.Open();
            MySqlDataReader msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                Models.EPTModel.EPTConsult EPTC = new Models.EPTModel.EPTConsult();
                EPTC.FolioEPT = msdr.GetValue(0).ToString();
                EPTC.EventName = msdr.GetValue(1).ToString();
                EPTC.Client = msdr.GetValue(2).ToString();
                EPTC.StartDate = msdr.GetValue(3).ToString();
                EPTList.Add(EPTC);
            }
            conn.Close();
            return EPTList;
        }
        public List<Models.EPTModel.EPTConsult> EPTSearch()
        {
            List<Models.EPTModel.EPTConsult> EPTList = new List<Models.EPTModel.EPTConsult>();
            string ConsSQLSearch = "select ept.tme_folio,eptevt.tdee_nombre,per.tmp_razonsocial,eptevt.tdee_fecmontaje from tm_ept ept inner join td_eptevt eptevt on eptevt.tme_id = ept.tme_id inner join tm_client cli on cli.tmc_id = ept.tmc_id inner join tm_person per on per.tmp_id = cli.tmp_id order by eptevt.tdee_fecmontaje desc";
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(ConsSQLSearch, conn);
            conn.Open();
            MySqlDataReader msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                Models.EPTModel.EPTConsult EPTC = new Models.EPTModel.EPTConsult();
                EPTC.FolioEPT = msdr.GetValue(0).ToString();
                EPTC.EventName = msdr.GetValue(1).ToString();
                EPTC.Client = msdr.GetValue(2).ToString();
                EPTC.StartDate = msdr.GetValue(3).ToString();
                EPTList.Add(EPTC);
            }
            conn.Close();
            return EPTList;
        }
        public string EditInsertEPT(Models.EPTModel model)
        {
            //SaveWithoutValidation("delete from tm_ept where tme_folio='" + model.EPTNumber + "'");
            //string ConsSQLSearch = "insert into tm_ept(tme_id,tme_folio,tmc_id,tmem_id,tme_idpm,tme_language,tme_userins,tme_dateins) values("+model.IDEvent+",'" + model.EPTNumber + "'," + model.IDClient + "," + model.IDEmpresa + ",1,1,1,now())";
            //string ConsSQLSearch = "update tm_ept set tme_folio='';
            //MySqlConnection conn = new MySqlConnection(con);
            //MySqlCommand cmd = new MySqlCommand(ConsSQLSearch, conn);
            //conn.Open();
            //cmd.ExecuteNonQuery();
            //string lastID = cmd.LastInsertedId.ToString();
            //conn.Close();
            string lastID = GetEPTToEdit(model.EPTNumber).IDEvent;
            EditInsertEPTDetail(model, lastID);
            return lastID;
        }

        public string InsertEPT(Models.EPTModel model)
        {
            string ConsSQLSearch = "insert into tm_ept(tme_folio,tmc_id,tmem_id,tme_idpm,tme_language,tme_userins,tme_dateins) values('" + model.EPTNumber + "',"+model.IDClient+"," + model.IDEmpresa + ",1,1,1,now())";
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(ConsSQLSearch, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            string lastID = cmd.LastInsertedId.ToString();
            conn.Close();
            InsertEPTDetail(model, lastID);
            return lastID;
        }
        public void EditInsertEPTDetail(Models.EPTModel model, string EventId)
        {
            //string ConsSQLSearch = "insert into td_eptevt(tme_id,tdee_nombre,tdee_fecmontaje,tdee_horamontaje,tdee_contactositio,tdee_celular,tdee_fecinicio,tdee_horainicio,tdee_fectermino,tdee_horatermino,tdee_lugar,tdee_direccion)" +
            //  "values(" + EventId + ",'" + model.EventName + "','" + model.MountDate + "','" + model.MountHour + "','" + model.ContactClientName + "','" + model.PlaceMobile + "','" + model.StartDate + "','" + model.StartHour + "','" + model.FinishDate + "','" + model.FinishHour + "','" + model.Place + "','" + model.Address + "')";
            string ConsSQLSearch = "update td_eptevt set tdee_nombre='"+model.EventName+"',tdee_fecmontaje='"+model.MountDate+ "',tdee_horamontaje='" + model.MountHour + "',tdee_contactositio='" + model.ContactClientName + "',tdee_celular='" + model.PlaceMobile + "',tdee_fecinicio='" + model.StartDate + "',tdee_horainicio='" + model.StartHour + "',tdee_fectermino='" + model.FinishDate + "',tdee_horatermino='" + model.FinishHour + "',tdee_lugar='" + model.Place + "',tdee_direccion='" + model.Address + "' where tme_id="+EventId;
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(ConsSQLSearch, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            //string lastID = cmd.LastInsertedId.ToString();
            conn.Close();
        }
        public void InsertEPTDetail(Models.EPTModel model, string EventId)
        {
            string ConsSQLSearch = "insert into td_eptevt(tme_id,tdee_nombre,tdee_fecmontaje,tdee_horamontaje,tdee_contactositio,tdee_celular,tdee_fecinicio,tdee_horainicio,tdee_fectermino,tdee_horatermino,tdee_lugar,tdee_direccion)" +
                "values(" + EventId + ",'" + model.EventName + "','" + model.MountDate + "','" + model.MountHour + "','" + model.ContactClientName + "','" + model.PlaceMobile + "','" + model.StartDate + "','" + model.StartHour + "','" + model.FinishDate + "','" + model.FinishHour + "','" + model.Place + "','" + model.Address + "')";
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(ConsSQLSearch, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            string lastID = cmd.LastInsertedId.ToString();
            conn.Close();
        }
        public string InsertTMItemList(Models.ItemListModel.ItemListEventModel model, string IDEvento)
        {
            string ConsSQL = "insert into tm_itemlistevt(tme_id,tmitl_salon,tmitl_asistentes,tmitl_montaje,tmitl_horario) values(" + IDEvento + ",'" + model.Salon + "','" + model.Asistentes + "','" + model.Montaje + "','" + model.Horario + "')";
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(ConsSQL, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            string LastID = cmd.LastInsertedId.ToString();
            conn.Close();
            return LastID;
        }
        public string InsertTMItemListWF(Models.ItemListModel.ItemListEventModel model, string IDEvento)
        {
            string ConsSQL = "insert into tm_itemlistevtwf(tme_id,tmitl_salon,tmitl_asistentes,tmitl_montaje,tmitl_horario) values(" + IDEvento + ",'" + model.Salon + "','" + model.Asistentes + "','" + model.Montaje + "','" + model.Horario + "')";
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(ConsSQL, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            string LastID = cmd.LastInsertedId.ToString();
            conn.Close();
            return LastID;
        }
        public void SaveItemListDetail(List<Models.SyncPSAV.ItemListServices> lmo, string IDITLE,string IDEVN)
        {
            for (int i = 0; i < lmo.Count; i++)
            {
                string ConsSQL = "insert into td_itemlist(tmilt_id,tdil_clave,tdil_cantidad,tdil_des,tdil_preciounit,tcct_id,tme_id) values(" + IDITLE + ",'" + lmo[i].Clave + "','" + lmo[i].Cantidad + "','" + lmo[i].Descripcion + "','" + lmo[i].PrecioUnit + "','" + lmo[i].Categoria + "',"+IDEVN+")";
                MySqlConnection conn = new MySqlConnection(con);
                MySqlCommand cmd = new MySqlCommand(ConsSQL, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void SaveItemListWFDetail(List<Models.SyncPSAV.ItemListWorkForce> model, string IDITL, string IDEVN)
        {
            for (int i = 0; i < model.Count; i++)
            {
                string ConsSQL = "insert into td_itemlistwf(tmilt_id,tdil_clave,tdil_cantidad,tdil_des,tdil_preciounit,tcct_id,tdil_seccion,tme_id) values(" + IDITL + ",'" + model[i].Clave + "','" + model[i].Cantidad + "','" + model[i].Descripcion + "','" + model[i].PrecioUnit + "','" + model[i].Categoria + "','" + model[i].Seccion + "',"+IDEVN+")";
                MySqlConnection conn = new MySqlConnection(con);
                MySqlCommand cmd = new MySqlCommand(ConsSQL, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public List<Models.SyncPSAV.VentaDes> GetCategoryEvent(string IDEvent)
        {
            string query = "SELECT distinct itl.tcct_id FROM psav_dev.td_itemlist itl where itl.tme_id="+IDEvent+" union select distinct itlwf.tcct_id from psav_dev.td_itemlistwf itlwf where itlwf.tme_id="+IDEvent;
            List<Models.SyncPSAV.VentaDes> lVD = new List<Models.SyncPSAV.VentaDes>();
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(query, conn);
            conn.Open();
            MySqlDataReader msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                Models.SyncPSAV.VentaDes mod = new Models.SyncPSAV.VentaDes();
                mod.Category = msdr.GetValue(0).ToString();
                lVD.Add(mod);
            }
            conn.Close();
            return lVD;
        }
        public List<Models.SyncPSAV.VentaFee> GetCategoryEvtFee(string IDEvent)
        {
            string query = "SELECT distinct itl.tcct_id FROM psav_dev.td_itemlist itl where itl.tme_id=" + IDEvent + " union select distinct itlwf.tcct_id from psav_dev.td_itemlistwf itlwf where itlwf.tme_id=" + IDEvent;
            List<Models.SyncPSAV.VentaFee> lVD = new List<Models.SyncPSAV.VentaFee>();
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(query, conn);
            conn.Open();
            MySqlDataReader msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                Models.SyncPSAV.VentaFee mod = new Models.SyncPSAV.VentaFee();
                mod.Category = msdr.GetValue(0).ToString();
                lVD.Add(mod);
            }
            conn.Close();
            return lVD;
        }
        public void SaveVFee(List<Models.SyncPSAV.VentaFee>lmod,string IDEvn)
        {
            for(int i = 0; i < lmod.Count; i++)
            {
                string QueryVFee = "insert into td_eptvfee(Category,BaseFee,PorcFee,SubFee,ImporteFee,tme_id)"+
                    "values('"+lmod[i].Category+"','"+lmod[i].BaseFee+"','"+lmod[i].PorcFee+"','"+lmod[i].SubFee+"','"+lmod[i].ImporteFee+"',"+IDEvn+")";
                SaveWithoutValidation(QueryVFee);
            }
        }
        public void SaveSubRent(List<Models.SyncPSAV.SubRenta>lmod,string IdEvn)
        {
            for (int i = 0; i < lmod.Count; i++)
            {
                string QueryVFee = "insert into td_eptsrent(Category,Invoice,Supplier,Descripcion,Gastosub,Ventasub,tme_id)" +
                    "values('" + lmod[i].Category + "','" + lmod[i].Invoice + "','" + lmod[i].Supplier + "','" + lmod[i].Descripcion + "','" + lmod[i].Gastosub + "','"+lmod[i].Ventasub+"'," + IdEvn + ")";
                SaveWithoutValidation(QueryVFee);
            }
        }
        public void SaveVDesc(List<Models.SyncPSAV.VentaDes> lmod,string IdEvn)
        {
            for(int i = 0; i <lmod.Count; i++)
            {
                string QueryVDesc = "insert into psav_dev.td_eptvdesc(tme_id,Category,VentaEqui,VentaEquEx,TotalVenta,DesPorEq,TotalDescEPS,DescExt,TotalExt,TotalDesc,PorcTotalDesc,AplicaAut)" 
                    + "values("+IdEvn+",'"+lmod[i].Category+"','"+lmod[i].VentaEqui+"','"+lmod[i].VentaEquEx+"','"+lmod[i].TotalVenta+"','"+lmod[i].DesPorEq+"','"+lmod[i].TotalDescEPS+"','"+lmod[i].DescExt+"','"+lmod[i].TotalExt+"','"+lmod[i].TotalDesc+"','"+lmod[i].PorcTotalDesc+"','"+lmod[i].AplicaAut+"')";
                SaveWithoutValidation(QueryVDesc);
            }
        }

        public Models.EPTModel GetEPTToEdit(string folio)
        {
            string ConsSQL = "SELECT ept.tme_folio,ept.tmc_id,per.tmp_razonsocial,tmpa.tmpa_domicilio,tmpa.tmpa_domiciliofiscal," +
" per.tmp_rfc,'nombre contacto','puesto','tel','ext','movil','mail','alterno','fax'," +
" evt.tdee_nombre,evt.tdee_fecmontaje,evt.tdee_horamontaje,evt.tdee_contactositio,evt.tdee_celular," +
" evt.tdee_fecinicio,evt.tdee_horainicio,evt.tdee_fectermino,evt.tdee_horatermino,evt.tdee_lugar," +
" evt.tdee_direccion,evt.tme_id,ept.tmem_id FROM psav_dev.tm_ept ept" +
" inner join td_eptevt evt on evt.tme_id = ept.tme_id" +
" inner join tm_client cli on cli.tmc_id = ept.tmc_id" +
" inner join tm_person per on per.tmp_id = cli.tmp_id" +
" inner join tm_personaddress tmpa on tmpa.tmp_id = per.tmp_id" +
" where ept.tme_folio = '"+folio.Trim()+"'";
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(ConsSQL, conn);
            conn.Open();
            MySqlDataReader msdr = cmd.ExecuteReader();
            Models.EPTModel eptm = new Models.EPTModel();
            while (msdr.Read())
            {
                eptm.EPTNumber = msdr.GetValue(0).ToString();
                eptm.IDClient = msdr.GetValue(1).ToString();
                eptm.RazonSocial = msdr.GetValue(2).ToString();
                eptm.DomicilioComercial = msdr.GetValue(3).ToString();
                eptm.DomicilioFiscal = msdr.GetValue(4).ToString();
                eptm.RFC = msdr.GetValue(5).ToString();
                eptm.ContactClientName = msdr.GetValue(6).ToString();
                eptm.Job = msdr.GetValue(7).ToString();
                eptm.Phone = msdr.GetValue(8).ToString();
                eptm.Ext = msdr.GetValue(9).ToString();
                eptm.Mobile = msdr.GetValue(10).ToString();
                eptm.Email = msdr.GetValue(11).ToString();
                eptm.AEMail = msdr.GetValue(12).ToString();
                eptm.Fax = msdr.GetValue(13).ToString();
                eptm.EventName = msdr.GetValue(14).ToString();
                eptm.MountDate = msdr.GetValue(15).ToString();
                eptm.MountHour = msdr.GetValue(16).ToString();
                eptm.PlaceContact = msdr.GetValue(17).ToString();
                eptm.PlaceMobile = msdr.GetValue(18).ToString();
                eptm.StartDate = msdr.GetValue(19).ToString();
                eptm.StartHour = msdr.GetValue(20).ToString();
                eptm.FinishDate = msdr.GetValue(21).ToString();
                eptm.FinishHour = msdr.GetValue(22).ToString();
                eptm.Place = msdr.GetValue(23).ToString();
                eptm.Address = msdr.GetValue(24).ToString();
                eptm.IDEvent = msdr.GetValue(msdr.FieldCount - 2).ToString();
                eptm.IDEmpresa= msdr.GetValue(msdr.FieldCount - 1).ToString();
            }
            conn.Close();
            return eptm;
        }
        #endregion
        #region CaptureRatio
        public List<Models.SyncPSAV.CratioDets> GetCD(string IDEvent)
        {
            MySqlConnection Conn = new MySqlConnection(con);
            Conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM psav_dev.td_cratio where tmcr_id=" + IDEvent, Conn);
            MySqlDataReader msda = cmd.ExecuteReader();
            List<Models.SyncPSAV.CratioDets> CRVM = new List<Models.SyncPSAV.CratioDets>();
            while (msda.Read())
            {
                Models.SyncPSAV.CratioDets CRD = new Models.SyncPSAV.CratioDets();
                CRD.EventName = msda.GetValue(4).ToString();
                CRD.IDClient = msda.GetValue(5).ToString();
                CRD.StartDate = msda.GetValue(2).ToString();
                CRD.EndDate = msda.GetValue(3).ToString();
                CRD.LB = msda.GetValue(13).ToString();
                CRD.CRatio = msda.GetValue(15).ToString();
                CRD.LBCause = msda.GetValue(17).ToString();
                CRD.VtasPSAV = msda.GetValue(14).ToString();
                CRD.NextEventPlace = msda.GetValue(18).ToString() + " - " + msda.GetValue(19).ToString();
                CRD.IDCratioDets = msda.GetValue(0).ToString();
                CRD.ID = msda.GetValue(0).ToString();
                CRVM.Add(CRD);
            }
            Conn.Close();
            return CRVM;
        }
        public List<Models.CaptureRatio.CRatioList> GetCRL(string IDEvent)
        {
            MySqlConnection Conn = new MySqlConnection(con);
            Conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM psav_dev.td_cratio where tmcr_id=" + IDEvent, Conn);

            MySqlDataReader msda = cmd.ExecuteReader();
            List<Models.CaptureRatio.CRatioList> CRVM = new List<Models.CaptureRatio.CRatioList>();
            while (msda.Read())
            {
                Models.CaptureRatio.CRatioList CRV = new Models.CaptureRatio.CRatioList();
                CRV.EventName = msda.GetValue(4).ToString();
                CRV.FinalClient = msda.GetValue(5).ToString();
                CRV.Finicio = msda.GetValue(2).ToString();
                CRV.FFin = msda.GetValue(3).ToString();
                CRV.LB = msda.GetValue(13).ToString();
                CRV.CRatio = msda.GetValue(15).ToString();
                CRV.LBCause = msda.GetValue(17).ToString();
                CRV.VentasPSAV = msda.GetValue(14).ToString();
                CRV.NextEventPlace = msda.GetValue(18).ToString() + " - " + msda.GetValue(19).ToString();
                CRVM.Add(CRV);
            }
            Conn.Close();
            return CRVM;
        }
        public List<Models.CaptureRatio.CRVResumenModel> GetCratioConsult(string idEvent)
        {
            MySqlConnection Conn = new MySqlConnection(con);
            Conn.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM psav_dev.tm_cratio where tmcr_id=" + idEvent, Conn);

            MySqlDataReader msda = cmd.ExecuteReader();
            List<Models.CaptureRatio.CRVResumenModel> CRVM = new List<Models.CaptureRatio.CRVResumenModel>();
            while (msda.Read())
            {
                Models.CaptureRatio.CRVResumenModel CRV = new Models.CaptureRatio.CRVResumenModel();
                CRV.HotelName = msda.GetValue(1).ToString();
                CRV.LocationHotel = msda.GetValue(2).ToString();
                CRV.CityLoc = msda.GetValue(3).ToString();
                CRV.DET = msda.GetValue(4).ToString();
                CRV.Contact = msda.GetValue(5).ToString();
                CRV.FillFormName = msda.GetValue(6).ToString();
                CRV.IDCratio = msda.GetValue(0).ToString();
                CRVM.Add(CRV);
            }
            Conn.Close();
            return CRVM;
        }
        public bool ValidCRatio(Models.CaptureRatio.CRVModel model)
        {
            string ValidFields = model.HotelName + model.FechaLoc + model.City + model.DET + model.ContactDET + model.NameFillForm;
            string QueryValidate = "select count(*) from tm_cratio where concat(tmcr_hotel,tmcr_location,tmcr_city,tmcr_det,tmcr_contactdet,tmcr_fillform)='" + ValidFields + "'";
            MySqlConnection Conn = new MySqlConnection(con);
            Conn.Open();
            MySqlCommand cmd = new MySqlCommand(QueryValidate, Conn);
            int validacion = Convert.ToInt32(cmd.ExecuteScalar());
            Conn.Close();
            if (validacion > 0) { return true; } else { return false; }
        }
        public string GetCRatio(Models.CaptureRatio.CRVModel model)
        {
            string ValidFields = model.HotelName + model.FechaLoc + model.City + model.DET + model.ContactDET + model.NameFillForm;
            string QueryValidate = "select tmcr_id from tm_cratio where concat(tmcr_hotel,tmcr_location,tmcr_city,tmcr_det,tmcr_contactdet,tmcr_fillform)='" + ValidFields + "'";
            MySqlConnection Conn = new MySqlConnection(con);
            Conn.Open();
            MySqlCommand cmd = new MySqlCommand(QueryValidate, Conn);
            int validacion = Convert.ToInt32(cmd.ExecuteScalar());
            Conn.Close();
            if (validacion.Equals(0)) {validacion=Convert.ToInt32( SaveCRatio(model)); }
            return validacion.ToString();
        }
        public string SaveCRatio(Models.CaptureRatio.CRVModel model)
        {
            MySqlConnection Conn = new MySqlConnection(con);
            Conn.Open();
            MySqlCommand cmd = new MySqlCommand("insert into tm_cratio(tmcr_hotel,tmcr_location,tmcr_city,tmcr_det,tmcr_contactdet,tmcr_fillform) values (" +
            "'" + model.HotelName + "','" + model.FechaLoc + "','" + model.City + "','" + model.DET + "','" + model.ContactDET + "','" + model.NameFillForm + "')", Conn);
            cmd.ExecuteNonQuery();
            long lastId = cmd.LastInsertedId;
            Conn.Close();
            return lastId.ToString();
        }
        public List<Models.CaptureRatio.CRatioList> GCRLExport(string idCR)
        {
            string QueryAllByID = "SELECT * FROM psav_dev.td_cratio where tmcr_id="+idCR;
           
            List<Models.CaptureRatio.CRatioList> CRLDec = new List<Models.CaptureRatio.CRatioList>();
            MySqlConnection Conn = new MySqlConnection(con);
            Conn.Open();
            MySqlCommand cmd = new MySqlCommand(QueryAllByID, Conn);
            MySqlDataReader msdr = cmd.ExecuteReader();
            while (msdr.Read())
            {
                Models.CaptureRatio.CRatioList CRL = new Models.CaptureRatio.CRatioList();
                CRL.Finicio = msdr.GetValue(2).ToString();
                CRL.FFin = msdr.GetValue(3).ToString();
                CRL.EventName = msdr.GetValue(4).ToString();
                CRL.FinalClient = msdr.GetValue(5).ToString();
                CRL.ContactName = msdr.GetValue(6).ToString();
                CRL.ContactContact = msdr.GetValue(7).ToString();
                CRL.Agency = msdr.GetValue(8).ToString();
                CRL.EventType = msdr.GetValue(9).ToString();
                CRL.EmpresaAV = msdr.GetValue(10).ToString();
                CRL.RsponsablePSAV = msdr.GetValue(11).ToString();
                CRL.VentasPSAV = msdr.GetValue(12).ToString();
                CRL.LB = msdr.GetValue(13).ToString();
                CRL.Suma = msdr.GetValue(14).ToString();
                CRL.CRatio = msdr.GetValue(15).ToString();
                CRL.HotelFee = msdr.GetValue(16).ToString();
                CRL.LBCause = msdr.GetValue(17).ToString();
                CRL.NextEventDate = msdr.GetValue(18).ToString();
                CRL.NextEventPlace = msdr.GetValue(19).ToString();
                CRL.MesFiltro = msdr.GetValue(2).ToString().Split('-')[1];
                CRLDec.Add(CRL);
            }
            Conn.Close();
            return CRLDec;
        }
        public List<Models.CaptureRatio.CRVResumenModel> GetCRatioExport(string idEvent)
        {
            string searchQuery = "";
            if (string.IsNullOrEmpty(idEvent)) { searchQuery = "SELECT * FROM psav_dev.tm_cratio"; } else { searchQuery = "select * from psav_dev.tm_cratio where tmcr_id =" + idEvent; }
            MySqlConnection Conn = new MySqlConnection(con);
            Conn.Open();
            MySqlCommand cmd = new MySqlCommand(searchQuery, Conn);

            MySqlDataReader msda = cmd.ExecuteReader();
            List<Models.CaptureRatio.CRVResumenModel> CRVM = new List<Models.CaptureRatio.CRVResumenModel>();
            while (msda.Read())
            {
                Models.CaptureRatio.CRVResumenModel CRV = new Models.CaptureRatio.CRVResumenModel();
                CRV.HotelName = msda.GetValue(1).ToString();
                CRV.LocationHotel = msda.GetValue(2).ToString();
                CRV.CityLoc = msda.GetValue(3).ToString();
                CRV.DET = msda.GetValue(4).ToString();
                CRV.Contact = msda.GetValue(5).ToString();
                CRV.FillFormName = msda.GetValue(6).ToString();
                CRV.IDCratio = msda.GetValue(0).ToString();
                CRVM.Add(CRV);
            }
            Conn.Close();
            return CRVM;
        }
        public List<Models.CaptureRatio.CRVResumenModel> GetCRatio(string idEvent)
        {
            string searchQuery = "";
            if (string.IsNullOrEmpty(idEvent)) { searchQuery = "SELECT * FROM psav_dev.tm_cratio"; } else { searchQuery = "select * from psav_dev.tm_cratio where tmcr_hotel like '%"+idEvent+"%'"; }
            MySqlConnection Conn = new MySqlConnection(con);
            Conn.Open();
            MySqlCommand cmd = new MySqlCommand(searchQuery, Conn);

            MySqlDataReader msda = cmd.ExecuteReader();
            List<Models.CaptureRatio.CRVResumenModel> CRVM = new List<Models.CaptureRatio.CRVResumenModel>();
            while (msda.Read())
            {
                Models.CaptureRatio.CRVResumenModel CRV = new Models.CaptureRatio.CRVResumenModel();
                CRV.HotelName = msda.GetValue(1).ToString();
                CRV.LocationHotel = msda.GetValue(2).ToString();
                CRV.CityLoc = msda.GetValue(3).ToString();
                CRV.DET = msda.GetValue(4).ToString();
                CRV.Contact = msda.GetValue(5).ToString();
                CRV.FillFormName = msda.GetValue(6).ToString();
                CRV.IDCratio = msda.GetValue(0).ToString();
                CRVM.Add(CRV);
            }
            Conn.Close();
            return CRVM;
        }
        public string SaveCRDet(Models.SyncPSAV.CratioDets model,int oper)
        {
            string QueryCRDet = "";string Retorno = "";
            switch (oper)
            {
                case 0://insert
                    QueryCRDet = "insert into td_cratio (tmcr_id,tdcr_finicio,tdcr_ffin,tdcr_evento,tdcr_finalclient,tdcr_contactname,tdcr_contactcontact,tdcr_agency,tdcr_eventtype,tdcr_empresaav," +
                "tdcr_resppsav,tdcr_ventapsav,tdcr_lob,tdcr_suma,tdcr_captureratio,tdcr_hotelfee,tdcr_causacr,tdcr_nexteventdate,tdcr_nexteventplace) values (" +
                "'" + model.IDCratio + "','" + model.StartDate + "','" + model.EndDate + "','" + model.EventName + "','" + model.IDClient + "','" + model.ContactName + "','" + model.ContactContact + "','" + model.Agency + "','" + model.IDEventType + "','" + model.EmpresaAV + "','" + model.ResponsableVtas + "','" + model.VtasPSAV + "','" + model.LB + "'," +
                "'" + model.Suma + "','" + model.CRatio + "','" + model.HotelFee + "','" + model.LBCause + "','" + model.NextEventDate + "','" + model.NextEventPlace + "')";
                    Retorno = SaveWithIDReturn(QueryCRDet);
                    break;
                case 1://update
                    break;
                case 2://delete
                    QueryCRDet = "delete from td_cratio where tdcr_id=" + model.IDCratioDets;
                    SaveWithoutValidation(QueryCRDet);
                    break;
            }
            return Retorno;
        }
        public List<Models.CaptureRatio.CRatioList> SaveCRatioDetail(Models.CaptureRatio.CRVModel model, string idEvent)
        {
            MySqlConnection Conn = new MySqlConnection(con);
            Conn.Open();
            MySqlCommand cmd = new MySqlCommand("insert into td_cratio (tmcr_id,tdcr_finicio,tdcr_ffin,tdcr_evento,tdcr_finalclient,tdcr_contactname,tdcr_contactcontact,tdcr_agency,tdcr_eventtype,tdcr_empresaav," +
                "tdcr_resppsav,tdcr_ventapsav,tdcr_lob,tdcr_suma,tdcr_captureratio,tdcr_hotelfee,tdcr_causacr,tdcr_nexteventdate,tdcr_nexteventplace) values (" +
                "'" + idEvent + "','" + model.Finicio + "','" + model.FFin + "','" + model.EventName + "','" + model.FinalClient + "','" + model.ContactName + "','" + model.ContactContact + "','" + model.Agency + "','" + model.EventType + "','" + model.EmpresaAV + "','" + model.RsponsablePSAV + "','" + model.VentasPSAV + "','" + model.LB + "'," +
                "'" + model.Suma + "','" + model.CRatio + "','" + model.HotelFee + "','" + model.LBCause + "','" + model.NextEventDate + "','" + model.NextEventPlace + "')", Conn);
            cmd.ExecuteNonQuery();
            Conn.Close();
            cmd = new MySqlCommand("select * from td_cratio where tmcr_id=" + idEvent, Conn);
            Conn.Open();
            MySqlDataReader msda = cmd.ExecuteReader();
            List<Models.CaptureRatio.CRatioList> CRVM = new List<Models.CaptureRatio.CRatioList>();
            while (msda.Read())
            {
                Models.CaptureRatio.CRatioList CRV = new Models.CaptureRatio.CRatioList();
                CRV.Finicio = msda.GetValue(2).ToString();
                CRV.FFin = msda.GetValue(3).ToString();
                CRV.EventName = msda.GetValue(4).ToString();
                CRV.FinalClient = msda.GetValue(5).ToString();
                CRV.ContactName = msda.GetValue(6).ToString();
                CRV.ContactContact = msda.GetValue(7).ToString();
                CRV.Agency = msda.GetValue(8).ToString();
                CRV.EventType = msda.GetValue(9).ToString();
                CRV.EmpresaAV = msda.GetValue(10).ToString();
                CRV.RsponsablePSAV = msda.GetValue(11).ToString();
                CRV.VentasPSAV = msda.GetValue(12).ToString();
                CRV.LB = msda.GetValue(13).ToString();
                CRV.Suma = msda.GetValue(14).ToString();
                CRV.CRatio = msda.GetValue(15).ToString();
                CRV.HotelFee = msda.GetValue(16).ToString();
                CRV.LBCause = msda.GetValue(17).ToString();
                CRV.NextEventDate = msda.GetValue(18).ToString();
                CRV.NextEventPlace = msda.GetValue(19).ToString();
                CRVM.Add(CRV);
            }
            Conn.Close();
            return CRVM;
        }
        #endregion
        #region general
        public void SaveWithoutValidation(string QuerySend)
        {
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QuerySend, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public string SaveWithIDReturn(string QuerySend)
        {
            MySqlConnection conn = new MySqlConnection(con);
            MySqlCommand cmd = new MySqlCommand(QuerySend, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            string lastID = cmd.LastInsertedId.ToString();
            conn.Close();
            return lastID;
        }
        #endregion
    }
}

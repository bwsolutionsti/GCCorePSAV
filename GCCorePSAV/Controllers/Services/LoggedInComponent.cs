using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using GCCorePSAV.Models;

namespace GCCorePSAV.Controllers.Services
{
    public class LoggedInComponent
    {
        public bool isAuth;
        public string UsuarioDraw;
        public List<Models.MenuModel.MIMOdel> MIMO;
        public async Task<string> IsAuthS(string Bearer,string PandoraRules)
        {
            try
            {
                Sec.secjwt ValidToken = new Sec.secjwt();
                bool validate = await ValidToken.ValidateTokens(Bearer, PandoraRules);
                isAuth = validate;
                UsuarioDraw = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(PandoraRules));
                if (validate)
                {
                    GetMenu();
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                isAuth = false;
                return string.Empty;
            }
        }
        public async Task<bool> IsAuth(string Bearer, string PandoraRules)
        {
            try
            {
                Sec.secjwt ValidToken = new Sec.secjwt();
                bool validate = await ValidToken.ValidateTokens(Bearer, PandoraRules);
                isAuth = validate;
                UsuarioDraw = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(PandoraRules));
                if (validate)
                {
                    GetMenu();
                }
                return validate;
            }
            catch (Exception ex)
            {
                isAuth = false;
                return false;
            }
        }
        private void GetMenu()
        {
            Data.clsQuery Consulta = new Data.clsQuery();
            MIMO = Consulta.GetDataMenu(UsuarioDraw);
        }
    }
}

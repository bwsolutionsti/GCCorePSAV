using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jose;
using Newtonsoft.Json;
using GCCorePSAV.Models;

namespace GCCorePSAV.Sec
{
    public class secjwt
    {
        private byte[] Base64UrlDecode(string arg)
        {
            string s = arg;
            s = s.Replace('-', '+'); // 62nd char of encoding
            s = s.Replace('_', '/'); // 63rd char of encoding
            //switch (s.Length % 4) // Pad with trailing '='s
            //{
            //    case 0: break; // No pad chars in this case
            //    case 2: s += "=="; break; // Two pad chars
            //    case 3: s += "="; break; // One pad char
            //    default:
            //        throw new System.Exception(
            //    "Illegal base64url string!");
            //}
            return Convert.FromBase64String(s); // Standard base64 decoder
        }
        private long ToUnixTime(DateTime dateTime)
        {
            return (int)(dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
        public string GetToken(LoginViewModel model)
        {
            byte[] secretKey = System.Text.Encoding.UTF8.GetBytes("(Veamos51Pu3d3nRomp3rl4$$pandorabox)(Rul35)");
            DateTime issued = DateTime.Now;
            DateTime expire = DateTime.Now.AddHours(10);
            string UserName = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(model.Email));
            var payload = new Dictionary<string, object>()
{
    {"iss", "http://pandorabox-2605.appspot.com"},
    {"aud", "pandorabox-2605"},
    {"sub", UserName},
    {"iat", ToUnixTime(issued).ToString()},
    {"exp", ToUnixTime(expire).ToString()}
};

            string token = JWT.Encode(payload, secretKey, JwsAlgorithm.HS256);
            return token;
        }
        public bool ValidateToken(string Bearer, string Usuario)
        {
            JwtModel model = new JwtModel();

            string bear = JWT.Payload(Bearer);
            model = JsonConvert.DeserializeObject<JwtModel>(bear);
            string usuario = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(model.sub));
            string userName = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(Usuario));
            if (usuario == userName)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<bool> ValidateTokens(string Bearer, string UserName)
        {
            JwtModel model = new JwtModel();

            string bear = JWT.Payload(Bearer);
            model = JsonConvert.DeserializeObject<JwtModel>(bear);
            string usuario = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(model.sub));
            string userNam2e = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(UserName));
            if (usuario == userNam2e)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCCorePSAV.Models
{
    public class JwtModel
    {
        public string iss { get; set; }
        public string aud { get; set; }
        public string sub { get; set; }
        public string iat { get; set; }
        public string exp { get; set; }
        public string ip { get; set; }
        public string ua { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCCorePSAV.Models.PSAVCrud
{
    public class SyncModels
    {
        public class CoinModel
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public decimal Change { get; set; }
            public int Active { get; set; }
        }
        public class UsersModel
        {
            public string ID { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string Active { get; set; }
            public string Persona { get; set; }
            public string Expira { get; set; }
        }
    }
}

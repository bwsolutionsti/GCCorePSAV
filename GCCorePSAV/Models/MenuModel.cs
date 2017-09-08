using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GCCorePSAV.Models
{
    public class MenuModel
    {
        public class DDLModel
        {
            public string NameDDL { get; set; }
            public string IDDDL { get; set; }
        }
        public class LIDDL
        {
            public string LIController { get; set; }
            public string LIAction { get; set; }
            public string LIText { get; set; }
        }
        public class MIMOdel
        {
            public string NameDDL { get; set; }
            public List<LIDDL> MenuD { get; set; }
        }
    }
}

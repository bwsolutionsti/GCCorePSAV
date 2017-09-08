using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GCCorePSAV.Models
{
    public class UsersModel
    {
        [Display(Prompt ="Usuario")]
        [EmailAddress]
        public string Username { get; set; }
        [Display(Prompt = "Password")]
        public string Password { get; set; }        
        [Display(Prompt ="# Nómina")]
        public string PayrollNumber { get; set; }
        [Display(Prompt ="# Cliente")]
        public string ClientNumber { get; set; }
    }
}

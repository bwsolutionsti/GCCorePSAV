using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GCCorePSAV.Models.PSAVCrud
{
    public class CoinsModel
    {
        [Display(Prompt = "Moneda")]
        public string CoinName { get; set; }
        [Display(Prompt = "Tipo Cambio")]
        public string CoinValue { get; set; }
        [Display(Prompt = "Último Cambio")]
        public string LastChange { get; set; }
        public string Activo { get; set; }
        public string IDCoin { get; set; }
    }
}

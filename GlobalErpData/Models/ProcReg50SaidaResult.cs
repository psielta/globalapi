using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Models
{
    public class ProcReg50SaidaResult
    {
        public string scnpj { get; set; }
        public string? snr_insc_esta { get; set; }
        public DateTime sdata { get; set; }
        public string suf { get; set; }
        public string snr_nota_fiscal { get; set; }
        public int snr_lanc { get; set; }
        public string scd_cfop { get; set; }
        public string scst { get; set; }
        public decimal sbase { get; set; }
        public decimal svl_icms { get; set; }
        public decimal svl_total { get; set; }
        public decimal spor_icms { get; set; }
        public string smodelo { get; set; }
        public string scd_situacao { get; set; }
        public string sserie_nf { get; set; }
    }

}

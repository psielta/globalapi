using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Models
{
    public class ProcReg54EntradaResult
    {
        public string scnpj { get; set; }
        public string suf { get; set; }
        public string snr_nf { get; set; }
        public string smodelo_nf { get; set; }
        public string sserie_nf { get; set; }
        public string scd_cfop { get; set; }
        public int snr_item { get; set; }
        public int scd_produto { get; set; }
        public decimal squant { get; set; }
        public string scst { get; set; }
        public decimal sb_icms { get; set; }
        public decimal svl_total { get; set; }
        public decimal sporc_icms { get; set; }
        public decimal svl_ipi { get; set; }
        public string snm_produto { get; set; }
        public decimal svl_outras { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Models
{
    public class ProcReg50EntradaResult
    {
        public string scnpj { get; set; }
        public string snr_inscr_estadual { get; set; }
        public DateTime sdata { get; set; }
        public string suf { get; set; }
        public string snr_nf { get; set; }
        public string smodelo_nf { get; set; }
        public string sserie_nf { get; set; }
        public string scd_cfop { get; set; }
        public decimal sb_icms { get; set; }
        public decimal svl_icms { get; set; }
        public decimal svl_total { get; set; }
        public decimal sporc_icms { get; set; }
        public string semitende_nf { get; set; }
        public string scst { get; set; }
    }

}

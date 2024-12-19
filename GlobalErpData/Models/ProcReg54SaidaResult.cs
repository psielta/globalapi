using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Models
{
    public class ProcReg54SaidaResult
    {
        public string scnpj { get; set; }
        public string suf { get; set; }
        public string snr_nota_fiscal { get; set; }
        public string scfop { get; set; }
        public int? snr_item { get; set; }
        public int scd_produto { get; set; }
        public decimal squant { get; set; }
        public string scst { get; set; }
        public decimal svl_base_icms { get; set; }
        public decimal svl_total { get; set; }
        public decimal spoc_icms { get; set; }
        public decimal svl_ipi { get; set; }
        public string snm_produto { get; set; }
        public string scpf { get; set; }
        public decimal sdesconto { get; set; }
        public decimal svl_base_st { get; set; }
        public decimal sporc_st { get; set; }
        public string ssmodelo { get; set; }
        public string sserie_nf { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class LivroCaixaGetDto
    {
        public long NrLanc { get; set; }
        public DateTime DtLanc { get; set; }
        public int CdEmpresa { get; set; }
        public string CdHistorico { get; set; } = null!;
        public decimal VlLancamento { get; set; }
        public int? NrCp { get; set; }
        public int? NrCr { get; set; }
        public string? TxtObs { get; set; }
        public int NrConta { get; set; }
        public string CdPlano { get; set; } = null!;
        public DateTime? LastUpdate { get; set; }
        public int? Integrated { get; set; }
        public int Unity { get; set; }
        public string? NmEmpresa { get; set; }
        public string? Tipo { get; set; }
        public string? NmConta { get; set; }
        public string? Fornecedor { get; set; }
        public string? Cliente { get; set; }
    }
}

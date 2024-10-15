using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class FormaPagtDto
    {
        public int CdEmpresa { get; set; }
        public string NmForma { get; set; } = null!;
        public short Prz01 { get; set; }
        public short? Prz02 { get; set; }
        public short? Prz03 { get; set; }
        public short? Prz04 { get; set; }
        public short? Prz05 { get; set; }
        public short? Prz06 { get; set; }
        public short? Prz07 { get; set; }
        public short? Prz08 { get; set; }
        public short? Prz09 { get; set; }
        public short? Prz10 { get; set; }
        public string? TxtObs { get; set; }
        public string? CdPlanoCaixa { get; set; }
        public string? CdHistoricoCaixa { get; set; }
        public string? CdPlanoCaixaD { get; set; }
        public string? CdHistoricoCaixaD { get; set; }
        public string? TipoPrazo { get; set; }
        public string? Integrado { get; set; }
    }
}

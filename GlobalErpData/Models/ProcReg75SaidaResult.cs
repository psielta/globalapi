using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Models
{
    public class ProcReg75SaidaResult
    {
        [Column("scd_produto")]
        public int CodigoProduto { get; set; }
        [Column("sncm")]
        public string NCM { get; set; }
        [Column("snm_produto")]
        public string NomeProduto { get; set; }
        [Column("sun")]
        public string UnidadeMedida { get; set; }
        [Column("sporc_ipi")]
        public decimal PorcentagemIPI { get; set; }
        [Column("spoc_icms")]
        public decimal PorcentagemICMS { get; set; }
        [Column("svl_base_st")]
        public decimal ValorBaseST { get; set; }
        [Column("spoc_reducao")]
        public decimal PorcentagemReducao { get; set; }
        [Column("scd_barra")]
        public string CodigoBarra { get; set; }
        [Column("sdata")]
        public DateTime Data { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Models
{
    public class ProcReg75EntradaResult
    {
        [Column("scd_produto")]
        public int CodigoProduto { get; set; }

        [Column("scd_class_fiscal")]
        public string Ncm { get; set; } = string.Empty;

        [Column("snm_produto")]
        public string NomeProduto { get; set; } = string.Empty;

        [Column("scd_uni")]
        public string Un { get; set; } = string.Empty;

        [Column("sporc_ipi")]
        public decimal? PorcentagemIPI { get; set; }

        [Column("sporc_icms")]
        public decimal? PorcentagemICMS { get; set; }

        [Column("sdata")]
        public DateTime Data { get; set; }
    }
}

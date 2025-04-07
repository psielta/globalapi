using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Models
{
    public class TotalDiaResult
    {
        [Column("vl_total")]
        public decimal VlTotal { get; set; }

        [Column("data")]
        public DateOnly Data { get; set; }
    }

    public class TotalPeriodoResult
    {
        [Column("vl_total")]
        public decimal VlTotal { get; set; }

        [Column("data")]
        public DateOnly Data { get; set; }

        [Column("vl_desc_global")]
        public decimal VlDescGlobal { get; set; }

        [Column("frete")]
        public decimal Frete { get; set; }
    }

    public class FormaPagamentoResult
    {
        [Column("forma")]
        public string Forma { get; set; }

        [Column("total")]
        public decimal Total { get; set; }
    }
}

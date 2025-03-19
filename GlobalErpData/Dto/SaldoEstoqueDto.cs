using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class SaldoEstoqueDto
    {

        public int CdProduto { get; set; }

        public int CdEmpresa { get; set; }

        public int CdPlano { get; set; }

        public decimal? QuantE { get; set; }

        public decimal? QuantV { get; set; }

        public decimal? QuantF { get; set; }

        public int? Unity { get; set; }

    }
}

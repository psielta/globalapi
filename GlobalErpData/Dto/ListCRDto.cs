using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class ListCRDto
    {
        public List<int> Contas { get; set; } 
        public int CdEmpresa { get; set; }

        public DateOnly DataPagamento { get; set; }
        public int NrContaCaixa { get; set; }
        public decimal ValorDesconto { get; set; }
        public decimal ValorAcrescimo { get; set; }
        public decimal ValorPago { get; set; }
        public decimal ValorRestante { get; set; }
    }
}

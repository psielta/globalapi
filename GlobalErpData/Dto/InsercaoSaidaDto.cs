using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class InsercaoProdutoSaidumDto
    {
        public int NrSaida { get; set; }

        public int CdEmpresa { get; set; }

        public int Unity { get; set; }

        public int CdProduto { get; set; }

        public decimal Quant { get; set; }

        public int CdPlano { get; set; }
    }

    public class InsercaoProdutoSaidumEanDto
    {
        public int NrSaida { get; set; }

        public int CdEmpresa { get; set; }
        public int Unity { get; set; }

        public string Ean { get; set; }

        public decimal Quant { get; set; }

        public int CdPlano { get; set; }
    }
}

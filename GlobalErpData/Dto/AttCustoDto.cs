using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class AttCustoDto
    {
        public int cdProduto { get; set; }
        public int idEmpresa { get; set; }
        public decimal custo { get; set; }
        public ProdutoEntradaDto Item { get; set; }
    }

    public class AttCustoDtoList
    {
        public List<AttCustoDto> Itens { get; set; }
    }
}

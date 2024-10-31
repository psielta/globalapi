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
        public ProdutoEntradaDtoComId Item { get; set; }
    }

    public class AttCustoDtoList
    {
        public List<AttCustoDto> Itens { get; set; }
    } 
    
    public class AttPrecoDto
    {
        public int cdProduto { get; set; }
        public int idEmpresa { get; set; }
        public decimal lucroPor { get; set; }
        public ProdutoEntradaDtoComId Item { get; set; }
    }

    public class AttPrecoDtoList
    {
        public List<AttPrecoDto> Itens { get; set; }
    }
    
    public class AttPrecoMarkupDto
    {
        public int cdProduto { get; set; }
        public int idEmpresa { get; set; }
        public decimal percentualLiquido { get; set; }
        public decimal percentualImpostos { get; set; }
        public decimal percentualComissao { get; set; }
        public decimal percentualCustoFixo { get; set; }
        public ProdutoEntradaDtoComId Item { get; set; }
    }

    public class AttPrecoMarkupDtoList
    {
        public List<AttPrecoMarkupDto> Itens { get; set; }
    }
}

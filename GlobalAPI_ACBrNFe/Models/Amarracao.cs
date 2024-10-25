using GlobalErpData.Dto;
using GlobalErpData.Models;

namespace GlobalAPI_ACBrNFe.Models
{
    public class Amarracao
    {
        public string NrItem { get; set; } = null!;
        public string CdProduto { get; set; } = "";
        public string CdBarra { get; set; } = "";
        public string NmProduto { get; set; } = "";
        public decimal? FatorConversao { get; set; } = 1;
        public int CdForn { get; set; }
        public ProdutoEstoque? produto { get; set; }

    }

    public class Amarracao2
    {
        public string NrItem { get; set; } = null!;
        public string CdProduto { get; set; } = "";
        public string CdBarra { get; set; } = "";
        public string NmProduto { get; set; } = "";
        public decimal? FatorConversao { get; set; } = 1;
        public int CdForn { get; set; }
        public ProdutoEstoqueDto2? produto { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class NfceProdutoSaidaDto
    {
        public long IdSaida { get; set; }
        public int CdEmpresa { get; set; }
        public int CdPlano { get; set; }
        public int CdProduto { get; set; }
        public decimal Quant { get; set; }
        public decimal VlVenda { get; set; }
        public decimal Desconto { get; set; }
        public decimal VlTotal { get; set; }
        public string Lote { get; set; } = null!;
        public DateOnly DtValidade { get; set; }
        public string NmProduto { get; set; } = null!;
        public string? Pagou { get; set; }
        public decimal? VlCusto { get; set; }
        public decimal? QuantEstorno { get; set; }
        public string? Cst { get; set; }
        public string? Cfop { get; set; }
        public string? Un { get; set; }
        public string? Ncm { get; set; }
        public decimal? VlComissao { get; set; }
        public decimal? PocIcms { get; set; }
        public decimal? VlBaseIcms { get; set; }
        public decimal? VlIcms { get; set; }
        public decimal? VlBaseIpi { get; set; }
        public decimal? PorcIpi { get; set; }
        public decimal? VlIpi { get; set; }
        public decimal? VlBaseSt { get; set; }
        public decimal? PorcSt { get; set; }
        public decimal? VlSt { get; set; }
        public decimal? PocReducao { get; set; }
        public decimal? MvaSt { get; set; }
        public short? NrItem { get; set; }
        public string? Cancelou { get; set; }
        public string? CdCsosn { get; set; }
        public decimal? VlAproxImposto { get; set; }
        public decimal? PorcIbpt { get; set; }
        public decimal? QtTotal { get; set; }
        public string? Cest { get; set; }
        public string? CdInterno { get; set; }
        public int? SequenciaItem { get; set; }
        public decimal? IcmsSubstituto { get; set; }
        public decimal? St { get; set; }
        public string? Cor { get; set; }
        public string? Tamanho { get; set; }
        public string? Genero { get; set; }
        public int? CdProdutoOriginal { get; set; }
        public int? CdPlanoSecundario { get; set; }
    }
}

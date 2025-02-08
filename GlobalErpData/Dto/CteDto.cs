using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class CteDto
    {
        public int Id { get; set; }
        public string NrCte { get; set; } = null!;
        public string Serie { get; set; } = null!;
        public DateTime? DtHrEmissao { get; set; }
        public string Modelo { get; set; } = null!;
        public string? Status { get; set; }
        public string? CdNumerico { get; set; }
        public string Cfop { get; set; } = null!;
        public string Modal { get; set; } = null!;
        public string TpServico { get; set; } = null!;
        public string? FormaPagto { get; set; }
        public string? FinalidadeEmissao { get; set; }
        public string? FormaEmissao { get; set; }
        public string? ChaveAcesso { get; set; }
        public string? ChaveAcessoReferenc { get; set; }
        public string? MunicipioEmissao { get; set; }
        public string? MunicipioInicioPrestacao { get; set; }
        public string? MunicipioFimPrestacao { get; set; }
        public string? DadosRetirada { get; set; }
        public string? Detalhe { get; set; }
        public string? CaractAdTransp { get; set; }
        public string? CaractAdServico { get; set; }
        public string? FuncEmissorCte { get; set; }
        public string? MunicipioOrigemCalcFrete { get; set; }
        public string? MunicipioDestinoCalcFrete { get; set; }
        public string? CdRotaEntrega { get; set; }
        public string? OrigemFluxoCaixa { get; set; }
        public string? DestinoFluxoCaixa { get; set; }
        public string? PrevisaoData { get; set; }
        public string? PrevisaoHora { get; set; }
        public DateOnly? DtInicioPrevisao { get; set; }
        public DateOnly? DtFimPrevisao { get; set; }
        public TimeOnly? HrInicioPrevisao { get; set; }
        public TimeOnly? HrFimPrevisao { get; set; }
        public string? TpTomadorServico { get; set; }
        public int? CdTomadorServico { get; set; }
        public string? TpRemetente { get; set; }
        public int? CdRemetente { get; set; }
        public string? TpExpedidor { get; set; }
        public int? CdExpedidor { get; set; }
        public string? TpRecebedor { get; set; }
        public int? CdRecebedor { get; set; }
        public string? TpDestinatario { get; set; }
        public int? CdDestinatario { get; set; }
        public decimal? VlPrestServico { get; set; }
        public decimal? VlReceberPrestServico { get; set; }
        public decimal? VlTribtPrestServico { get; set; }
        public string? CdStIcms { get; set; }
        public decimal? PorcRedBcIcms { get; set; }
        public decimal? VlBcIcms { get; set; }
        public decimal? AliqIcms { get; set; }
        public decimal? VlIcms { get; set; }
        public decimal? VlCredPresumido { get; set; }
        public string? InfFiscoIcms { get; set; }
        public string? IcmsUfTermino { get; set; }
        public decimal? VlBcIcmsUft { get; set; }
        public decimal? AliqInternaUft { get; set; }
        public decimal? AliqInterestUft { get; set; }
        public string? CdPartUft { get; set; }
        public decimal? PorcPartUft { get; set; }
        public decimal? VlIcmsPartUfi { get; set; }
        public decimal? VlIcmsPartUft { get; set; }
        public decimal? PorcIcmsFcpUft { get; set; }
        public decimal? VlIcmsFcpUf { get; set; }
        public decimal? VlCarga { get; set; }
        public string? ProdPredominante { get; set; }
        public string? OutrasCaractProd { get; set; }
        public string? ChaveCteSubst { get; set; }
        public string? TomadorCteSubst { get; set; }
        public string? TomadorNc { get; set; }
        public string? ChaveCteTomador { get; set; }
        public string? ChaveNfeTomador { get; set; }
        public string? TpDocTomador { get; set; }
        public string? CpfTomador { get; set; }
        public string? CnpjTomador { get; set; }
        public string? CdModeloTomador { get; set; }
        public string? SerieTomador { get; set; }
        public string? Subserie { get; set; }
        public string? NumeroTomador { get; set; }
        public decimal? VlTomador { get; set; }
        public DateOnly? DtTomador { get; set; }
        public string? NrFatura { get; set; }
        public decimal? VlOriginalFatura { get; set; }
        public decimal? VlDescFatura { get; set; }
        public decimal? VlLiqFatura { get; set; }
        public string? Rntcr { get; set; }
        public DateOnly? DtPrevEntrega { get; set; }
        public string? IndicadorLot { get; set; }
        public string? Ciot { get; set; }
        public string? ChaveCteAnulacao { get; set; }
        public string? Obs { get; set; }
        public string? NrAutorizacaoCte { get; set; }
        public string? CdSituacaoCte { get; set; }
        public string? XmlCte { get; set; }
        public string? TxtJustificativaCancelamento { get; set; }
        public string? NrProtoCancelamento { get; set; }
        public string? TxtdescServicoPrestado { get; set; }
        public int? QtPassageiro { get; set; }
        public string? NrTaf { get; set; }
        public string? NrRegEstadual { get; set; }
        public DateOnly? DataViagem { get; set; }
        public TimeOnly? HoraViagem { get; set; }
        public string? TipoViagem { get; set; }
        public int? NrCnf { get; set; }
        public decimal? Inss { get; set; }
        public string? NrCteReferenciado { get; set; }
        public string? RetemInss { get; set; }
        public int IdEmpresa { get; set; }
    }
}

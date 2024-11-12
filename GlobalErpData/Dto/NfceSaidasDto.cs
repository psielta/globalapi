using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class NfceSaidaDto
    {
        public DateOnly? Data { get; set; }
        public int CdEmpresa { get; set; }
        public int Cliente { get; set; }
        public string? Requisicao { get; set; }
        public string? Observacao { get; set; }
        public string TpSaida { get; set; } = null!;
        public string TpPagt { get; set; } = null!;
        public DateOnly DtSaida { get; set; }
        public DateOnly? DtPedido { get; set; }
        public string? TabelaVenda { get; set; }
        public string? Pagou { get; set; }
        public string? Cfop { get; set; }
        public string? NrNotaFiscal { get; set; }
        public string? ChaveAcessoNfe { get; set; }
        public string? CdSituacao { get; set; }
        public decimal? VlOutro { get; set; }
        public decimal? VlDescGlobal { get; set; }
        public string? NrAutorizacaoNfe { get; set; }
        public TimeOnly HrSaida { get; set; }
        public int? NrCnf { get; set; }
        public int? Serie { get; set; }
        public decimal? Frete { get; set; }
        public int? Caixa { get; set; }
        public decimal? VlDescontoClassificacao { get; set; }
        public bool IsNfce { get; set; }
        public long NrAberturaCaixa { get; set; }
        public int IdUsuario { get; set; }
    }
}

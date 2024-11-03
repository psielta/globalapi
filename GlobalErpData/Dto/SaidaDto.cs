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
    public class SaidaDto
    {
        public DateOnly? Data { get; set; }

        public int Empresa { get; set; }

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

        public int? CdCarga { get; set; }

        public string? TxtObsNf { get; set; }

        public int? CdVendedor { get; set; }

        public string? PagaComissao { get; set; }

        public string? NrNotaFiscal { get; set; }

        public string? ChaveAcessoNfe { get; set; }

        public string? CdUf { get; set; }

        public string? CdSituacao { get; set; }

        public string? TxtJustificativaCancelamento { get; set; }

        public string? NrProtoCancelamento { get; set; }

        public DateOnly? DtPagouComis { get; set; }

        public string? XmNf { get; set; }

        public decimal? VlOutro { get; set; }

        public decimal? VlDescGlobal { get; set; }

        public string? NrAutorizacaoNfe { get; set; }

        public int? IdPontoVenda { get; set; }

        public string? ChaveNfeSaida { get; set; }

        public int? IdConvenio { get; set; }

        public int? IdMedico { get; set; }

        public int? IdPaciente { get; set; }

        public DateOnly? DtCirugia { get; set; }

        public TimeOnly? HrSaida { get; set; }

        public int? IdEndEntrega { get; set; }

        public int? IdEndRetirada { get; set; }

        public int? NrCnf { get; set; }

        public int? IdTabelaPrecos { get; set; }

        public int? IdTipoOperacaoIntermediador { get; set; }

        public int? IdTipoIndicador { get; set; }

        public string? SerieNf { get; set; }

        public string? CnpjMarket { get; set; }

        public string? NmMarket { get; set; }

        public string? LocalSalvoNota { get; set; }

        public string? TpOperacao { get; set; }

        public string? XmNfCnc { get; set; }
    }
}

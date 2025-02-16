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
    public class ContasAPagarDto
    {
        public DateOnly DtLancamento { get; set; }
        public DateOnly DtVencimento { get; set; }
        public DateOnly? DtPagou { get; set; }
        public int CdFornecedor { get; set; }
        public int? NrEntrada { get; set; }
        public string? NrDuplicata { get; set; }
        public decimal VlCp { get; set; }
        public decimal VlDesconto { get; set; }
        public decimal VlTotal { get; set; }
        public decimal? VlAcrescimo { get; set; }
        public decimal? VlPagoFinal { get; set; }
        public string Pagou { get; set; } = null!;
        public string? TxtObs { get; set; }
        public string? TpFormaPagt { get; set; }
        public int CdEmpresa { get; set; }
        public string? NrCheque { get; set; }
        public int? NrConta { get; set; }
        public string? PagoA { get; set; }
        public decimal? VlCheque { get; set; }
        public string? NrNf { get; set; }
        public string CdPlanoCaixa { get; set; } = null!;
        public string CdHistoricoCaixa { get; set; } = null!;
        public int? NrFormaPagt { get; set; }
        public decimal? VlDinheiro { get; set; }
        public string? IdLancPrinc { get; set; }
        public int? NrEntradaOutraDesp { get; set; }
        public decimal? RefQuantEntrega { get; set; }
        public string? IdExtrato { get; set; }
        public decimal Rate { get; set; }
        public int NumberOfPayments { get; set; }
        public int Type { get; set; }
        public int TypeRegister { get; set; }

        public decimal VlPago { get; set; }
        public bool PossuiPagamentoParcial { get; set; }
            
        public decimal ValorRestante { get; set; }
    }
}

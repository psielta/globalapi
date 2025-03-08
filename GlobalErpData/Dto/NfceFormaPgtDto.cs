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
    public class NfceFormaPgtDto
    {
        public int? IdSaida { get; set; }

        public int CdEmpresa { get; set; }

        public string? Forma { get; set; }

        public decimal? Valor { get; set; }

        public decimal? Troco { get; set; }

        public string? Bandeira { get; set; }

        public string? Cnpj { get; set; }

        public string? Nsu { get; set; }

        public int? NrAberturaCaixa { get; set; }

        public int? Caixa { get; set; }

        public int? TpIntegra { get; set; }

        public string? NrAutorizacaoOperacao { get; set; }

        public string? PwinfoReqnum { get; set; }

        public string? Rede { get; set; }

        public string? PrimeiraViaTef { get; set; }

        public string? SegundaViaTef { get; set; }

        public DateTime? LastUpdate { get; set; }

        public int? Integrated { get; set; }
    }
}

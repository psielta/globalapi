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
    public class EntradaOutrasDespDto
    {
        public DateOnly DtEntrada { get; set; }

        public int CdFornecedor { get; set; }

        public string NrNotaFiscal { get; set; } = null!;

        public string? ModeloNf { get; set; }

        public string? Serie { get; set; }

        public string? Subserie { get; set; }

        public string? NrNotaFiscalEntrada { get; set; }

        public string Cfop { get; set; } = null!;

        public decimal ValorTotal { get; set; }

        public decimal BaseIcms { get; set; }

        public decimal PorcIcms { get; set; }

        public decimal ValorIcms { get; set; }

        public decimal VlIsentaNTrib { get; set; }

        public decimal VlOutras { get; set; }

        public int TpFrete { get; set; }

        public string? CdTipoNf { get; set; }

        public int CdEmpresa { get; set; }

        public string? ChaveCte { get; set; }

        public string? CdCidadeOrigem { get; set; }

        public string? CdCidadeDestino { get; set; }
    }
}

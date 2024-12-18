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
    public class DistribuicaoDfeDto
    {
        public string Serie { get; set; } = null!;

        public string NrNotaFiscal { get; set; } = null!;

        public string ChaveAcessoNfe { get; set; } = null!;

        public string Cnpj { get; set; } = null!;

        public string Nome { get; set; } = null!;

        public string? Ie { get; set; }

        public string? TpNfe { get; set; }

        public string? Nsu { get; set; }

        public string? Emissao { get; set; }

        public decimal? Valor { get; set; }

        public string? Impresso { get; set; }

        public char? TpResposta { get; set; }

        public char? Manifesto { get; set; }

        public char? Transferiu { get; set; }

        public DateOnly? DtRecebimento { get; set; }

        public string? Xml { get; set; }

        public DateOnly DtInclusao { get; set; }

        public int IdEmpresa { get; set; }
    }
}

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
    public class ContaCaixaDto
    {
        public string NmConta { get; set; } = null!;

        public string? NrContaBanco { get; set; }

        public string? NrAgencia { get; set; }

        public string? NmBanco { get; set; }

        public decimal? SaldoInicial { get; set; }

        public string? NrChequeInicial { get; set; }

        public int CdEmpresa { get; set; }
        public int Unity { get; set; }

        public string? NrDigitoAg { get; set; }

        public string? NrDigitoConta { get; set; }

        public decimal? SaldoAtual { get; set; }

        public decimal? LimiteEspecial { get; set; }

        public string? MostrarDadosImpressao { get; set; }
    }
}

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
    public class VendedorDto
    {
        public int CdFuncionario { get; set; }

        public int CdEmpresa { get; set; }

        public decimal? ComissaoAPrazo { get; set; }

        public decimal? ComissaoAVista { get; set; }

        public string? TipoPagamento { get; set; }

        public DateTime? LastUpdate { get; set; }

        public int? Integrated { get; set; }

        public int? Unity { get; set; }

    }
}

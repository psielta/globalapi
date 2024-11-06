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
    public class IbptDto
    {
        public string Codigo { get; set; } = null!;

        public string? Ex { get; set; }

        public string? Tabela { get; set; }

        public decimal? Aliqnac { get; set; }

        public decimal? Aliqimp { get; set; }

        public string? Descricao { get; set; }

        public decimal? Aliqestadual { get; set; }

        public decimal? Aliqmunicipal { get; set; }
    }
}

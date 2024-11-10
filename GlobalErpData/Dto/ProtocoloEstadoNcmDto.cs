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
    public class ProtocoloEstadoNcmDto
    {
        public int IdEmpresa { get; set; }
        public string Ativo { get; set; } = null!;
        public string Nome { get; set; } = null!;
        public string Uf { get; set; } = null!;
        public decimal? Iva { get; set; }
        public decimal? St { get; set; }
        public decimal? RedSt { get; set; }
        public decimal? RedIcms { get; set; }
        public string? TxtObs { get; set; }
    }
}

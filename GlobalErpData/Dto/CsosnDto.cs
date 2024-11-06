using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class CsosnDto
    {
        public string Codigo { get; set; } = null!;

        public string? Descricao { get; set; }
    }
}

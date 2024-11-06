using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class CfopDto
    {
        public string CdCfop { get; set; } = null!;

        public string? Descricao { get; set; }

        public string? TipoCfop { get; set; }

        public string? DescNfe { get; set; }
    }
}

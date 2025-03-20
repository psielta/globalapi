using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class CfopImportacaoDto
    {
        public string CdCfopS { get; set; } = null!;

        public string CdCfopE { get; set; } = null!;

        public string? CfopDentro { get; set; }

        public string? CfopFora { get; set; }

        public string? Csosn { get; set; }

        public int Unity { get; set; }
    }
}

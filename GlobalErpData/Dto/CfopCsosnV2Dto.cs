using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class CfopCsosnV2Dto
    {
        public int IdEmpresa { get; set; }

        public string Cfop { get; set; } = null!;

        public string Csosn { get; set; } = null!;
    }
}

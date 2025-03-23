using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class ObsNfDto
    {
        public int Unity { get; set; }

        public string NmObs { get; set; } = null!;

        public string? TxtObs { get; set; }
    }
}

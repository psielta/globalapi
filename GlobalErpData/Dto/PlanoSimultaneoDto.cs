using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class PlanoSimultaneoDto
    {
        public int Unity { get; set; }

        public int CdPlanoPrinc { get; set; }

        public int CdPlanoReplica { get; set; }
    }
}

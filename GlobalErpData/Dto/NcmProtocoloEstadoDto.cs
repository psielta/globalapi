using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class NcmProtocoloEstadoDto
    {
        public int Unity { get; set; }

        public int IdCabProtocolo { get; set; }

        public string IdNcm { get; set; } = null!;
    }
}

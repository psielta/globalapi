using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class NcmDto
    {
        public string? Ncm1 { get; set; }

        public DateOnly? DtVigencia { get; set; }

        public string? Unidade { get; set; }

        public string? Descricao { get; set; }
    }
}

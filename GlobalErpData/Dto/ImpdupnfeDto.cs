using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class ImpdupnfeDto
    {
        public string ChNfe { get; set; } = null!;

        public string? NrDup { get; set; }

        public DateTime? DtVenc { get; set; }

        public string? Valor { get; set; }
    }
}

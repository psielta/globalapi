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
    public class SaidasVolumeDto
    {
        public int NrSaida { get; set; }
        public int? QVol { get; set; }
        public string? Esp { get; set; }
        public string? Marca { get; set; }
        public string? NVol { get; set; }
        public decimal? PesoL { get; set; }
        public decimal? PesoB { get; set; }
    }
}

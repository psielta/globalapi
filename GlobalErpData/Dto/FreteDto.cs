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
    public class FreteDto
    {
        public int CdEmpresa { get; set; }
        public int NrSaida { get; set; }
        public int FretePorConta { get; set; }
        public decimal VlFrete { get; set; }
        public int? CdTransp { get; set; }
        public decimal? Quant { get; set; }
        public string? Especie { get; set; }
        public string? Marca { get; set; }
        public string? Numeracao { get; set; }
        public decimal? Pbruto { get; set; }
        public decimal? Pliq { get; set; }
    }
}

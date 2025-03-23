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
    public class LivroCaixaDto
    {
        public DateTime DtLanc { get; set; }

        public int CdEmpresa { get; set; }
        public int Unity { get; set; }

        public string CdHistorico { get; set; } = null!;

        public decimal VlLancamento { get; set; }

        public int? NrCp { get; set; }

        public int? NrCr { get; set; }

        public string? TxtObs { get; set; }

        public int NrConta { get; set; }

        public string CdPlano { get; set; } = null!;
    }
}

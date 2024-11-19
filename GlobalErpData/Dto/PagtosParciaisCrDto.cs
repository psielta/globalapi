using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class PagtosParciaisCrDto
    {
        public int? NrConta { get; set; }

        public DateOnly? DtPagto { get; set; }

        public decimal? ValorPago { get; set; }

        public decimal? ValorRestante { get; set; }

        public decimal? Acrescimo { get; set; }

        public decimal? Desconto { get; set; }

        public int CdEmpresa { get; set; }

        public byte[]? PdfRecebimento { get; set; }
    }
}

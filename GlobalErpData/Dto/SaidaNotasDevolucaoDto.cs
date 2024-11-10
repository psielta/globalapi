using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class SaidaNotasDevolucaoDto
    {
        public int NrSaida { get; set; }

        public string? ChaveAcesso { get; set; }
    }
}

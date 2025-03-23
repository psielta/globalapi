using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class ControleNumeracaoNfeDto
    {
        public int IdEmpresa { get; set; }

        public int Unity { get; set; }
        public int Serie { get; set; }

        public long ProximoNumero { get; set; }

        public bool Padrao { get; set; }
    }
}

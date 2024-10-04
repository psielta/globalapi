using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class PlanoEstoqueDto
    {
        public string NmPlano { get; set; } = null!;

        public int CdEmpresa { get; set; }

        public bool Ativo { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class GetXmlDto
    {
        public List<int>? Empresas { get; set; }
        public List<string>? Situacoes { get; set; }
        public PeriodoDto? Periodo { get; set; }
    }
}

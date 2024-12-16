using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class SintegraDto
    {
        public string SessionId { get; set; }
        public int IdEmpresa { get; set; }
        public int Mes { get; set; }
        public int Ano { get; set; }
        public int CodFinalidadeArquivo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class PostGerarDto
    {
        public string sessionId { get; set; }
        public bool Gerar { get; set; } = false;
    }
    
    public class PostConsultaDto
    {
        public string sessionId { get; set; }
    }
}

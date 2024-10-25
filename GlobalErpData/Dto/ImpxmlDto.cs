using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class ImpxmlDto
    {
        public int IdEmpresa { get; set; }

        public string ChaveAcesso { get; set; } = null!;

        public int Type { get; set; }

        public string Xml { get; set; } = null!;
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class ServicoDto
    {
        public int Unity { get; set; }

        public int IdDepartamento { get; set; }

        public bool PagaComissao { get; set; }

        public decimal ValorUnitario { get; set; }
        public string NmServico { get; set; } = null!;
         
    }
}

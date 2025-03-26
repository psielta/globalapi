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
    public class OsTabelaPrecoDto
    {
        public long CdServico { get; set; }

        public int IdTabelaPreco { get; set; }

        public decimal PrecoVenda { get; set; }

        public DateTime DtUltAlteracao { get; set; }

        public string? Descricao { get; set; }
    }
}

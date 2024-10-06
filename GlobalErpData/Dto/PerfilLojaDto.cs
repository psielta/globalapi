using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
public    class PerfilLojaDto
    {
        public int IdEmpresa { get; set; }

        public string Nome { get; set; } = null!;

        public string Descricao { get; set; } = null!;

    }
}

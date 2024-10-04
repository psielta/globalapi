using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class UsuarioPermissaoDto
    {
        public string IdUsuario { get; set; } = null!;

        public int IdPermissao { get; set; }
    }
}

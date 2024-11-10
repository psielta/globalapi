using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalPdvData.Dto
{
    public class UsuarioPermissaoGetDto
    {
        public int IdPermissao { get; set; }
        public string Descricao { get; set; }
        public bool Possui { get; set; }
        public int IdUsuarioPermissao { get; set; } 
    }
}

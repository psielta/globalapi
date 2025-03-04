using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class UsuarioFuncionarioDto
    {
        public int CdEmpresa { get; set; }

        public int CdFuncionario { get; set; }

        public string NmUsuario { get; set; } = null!;

        public DateTime? LastUpdate { get; set; }

        public int? Integrated { get; set; }
    }
}

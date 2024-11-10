using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalPdvData.Dto
{
    public class UsuarioDto
    {
        public string NmUsuario { get; set; } = null!;

        public string CdSenha { get; set; } = null!;

        public string NmPessoa { get; set; } = null!;

        public int CdEmpresa { get; set; }

        public string? Ativo { get; set; }

        public bool? Admin { get; set; }

        public string Email { get; set; } = null!;

        public string NmUsuarioNormalized { get; set; } = null!;

        public string EmailNormalized { get; set; } = null!;

        public bool EmailConfirmed { get; set; }

        public string SecurityStamp { get; set; } = null!;
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalErpData.Dto
{
    public class UsuarioDto
    {
        [Required]
        [StringLength(62)]
        public string NmUsuario { get; set; } = null!;

        [Required]
        [StringLength(10)]
        public string CdSenha { get; set; } = null!;

        [Required]
        [StringLength(62)]
        public string NmPessoa { get; set; } = null!;

        [Required]
        public int CdEmpresa { get; set; }

        [StringLength(1)]
        public string? Ativo { get; set; }

        public bool? Admin { get; set; }
    }

}

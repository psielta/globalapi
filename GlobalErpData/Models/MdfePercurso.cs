using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Keyless]
[Table("mdfe_percurso", Schema = "mdfe")]
public partial class MdfePercurso
{
    [Column("id")]
    public int Id { get; set; }

    [Column("id_mdfe")]
    [StringLength(10)]
    public string IdMdfe { get; set; } = null!;

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("estado")]
    [StringLength(2)]
    public string? Estado { get; set; }

    [ForeignKey("IdEmpresa")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}

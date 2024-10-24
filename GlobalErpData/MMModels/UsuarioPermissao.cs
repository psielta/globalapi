using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("usuario_permissao")]
public partial class UsuarioPermissao
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_usuario")]
    [StringLength(255)]
    public string IdUsuario { get; set; } = null!;

    [Column("id_permissao")]
    public int IdPermissao { get; set; }

    [ForeignKey("IdPermissao")]
    [InverseProperty("UsuarioPermissaos")]
    public virtual Permissao IdPermissaoNavigation { get; set; } = null!;

    [ForeignKey("IdUsuario")]
    [InverseProperty("UsuarioPermissaos")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[PrimaryKey("CdUsuario", "CdEmpresa")]
[Table("usuario_empresa")]
[Index("Id", Name = "usuario_empresa_id_key", IsUnique = true)]
public partial class UsuarioEmpresa
{
    [Key]
    [Column("cd_usuario")]
    [StringLength(62)]
    public string CdUsuario { get; set; } = null!;

    [Key]
    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("id")]
    public int Id { get; set; }

    [ForeignKey("CdEmpresa")]
    [InverseProperty("UsuarioEmpresas")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [ForeignKey("CdUsuario")]
    [InverseProperty("UsuarioEmpresas")]
    public virtual Usuario CdUsuarioNavigation { get; set; } = null!;
}

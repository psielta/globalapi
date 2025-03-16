using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[PrimaryKey("CdFuncionario", "NmUsuario")]
[Table("usuario_funcionario")]
[Index("Id", Name = "usuario_funcionario_id_key", IsUnique = true)]
public partial class UsuarioFuncionario
{
    [Column("id")]
    public int Id { get; set; }

    [Key]
    [Column("cd_funcionario")]
    public int CdFuncionario { get; set; }

    [Key]
    [Column("nm_usuario")]
    [StringLength(62)]
    public string NmUsuario { get; set; } = null!;

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [ForeignKey("CdFuncionario, CdEmpresa")]
    [InverseProperty("UsuarioFuncionarios")]
    public virtual Funcionario Funcionario { get; set; } = null!;

    [ForeignKey("NmUsuario")]
    [InverseProperty("UsuarioFuncionarios")]
    public virtual Usuario NmUsuarioNavigation { get; set; } = null!;
}

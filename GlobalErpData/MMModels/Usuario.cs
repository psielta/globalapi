using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("usuario")]
[Index("Id", Name = "usuario_id_key", IsUnique = true)]
[Index("Email", Name = "usuario_idx", IsUnique = true)]
public partial class Usuario
{
    [Column("id")]
    public int Id { get; set; }

    [Key]
    [Column("nm_usuario")]
    [StringLength(62)]
    public string NmUsuario { get; set; } = null!;

    [Column("cd_senha")]
    [StringLength(16384)]
    public string CdSenha { get; set; } = null!;

    [Column("nm_pessoa")]
    [StringLength(62)]
    public string NmPessoa { get; set; } = null!;

    [Column("ativo")]
    [StringLength(1)]
    public string? Ativo { get; set; }

    [Column("admin")]
    public bool? Admin { get; set; }

    [Column("email")]
    [StringLength(512)]
    public string Email { get; set; } = null!;

    [Column("nm_usuario_normalized")]
    [StringLength(62)]
    public string NmUsuarioNormalized { get; set; } = null!;

    [Column("email_normalized")]
    [StringLength(512)]
    public string EmailNormalized { get; set; } = null!;

    [Column("email_confirmed")]
    public bool EmailConfirmed { get; set; }

    [Column("security_stamp")]
    [StringLength(1024)]
    public string SecurityStamp { get; set; } = null!;

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [InverseProperty("IdUsuarioCadNavigation")]
    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<ConfiguracoesUsuario> ConfiguracoesUsuarios { get; set; } = new List<ConfiguracoesUsuario>();

    [ForeignKey("Unity")]
    [InverseProperty("Usuarios")]
    public virtual Unity UnityNavigation { get; set; } = null!;

    [InverseProperty("CdUsuarioNavigation")]
    public virtual ICollection<UsuarioEmpresa> UsuarioEmpresas { get; set; } = new List<UsuarioEmpresa>();

    [InverseProperty("NmUsuarioNavigation")]
    public virtual ICollection<UsuarioFuncionario> UsuarioFuncionarios { get; set; } = new List<UsuarioFuncionario>();

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<UsuarioPermissao> UsuarioPermissaos { get; set; } = new List<UsuarioPermissao>();
}

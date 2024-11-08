using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("usuario")]
[Index("Id", Name = "usuario_id_key", IsUnique = true)]
public partial class Usuario : IIdentifiable<string>
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

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("ativo")]
    [StringLength(1)]
    public string? Ativo { get; set; }

    [Column("admin")]
    public bool? Admin { get; set; }

    [Column("email")]
    [StringLength(512)]
    public string? Email { get; set; }

    [Column("nm_usuario_normalized")]
    [StringLength(62)]
    public string NmUsuarioNormalized { get; set; }

    [Column("email_normalized")]
    [StringLength(512)]
    public string EmailNormalized { get; set; }

    [Column("email_confirmed")]
    public bool EmailConfirmed { get; set; }

    [Column("security_stamp")]
    public string SecurityStamp { get; set; }

    [NotMapped]
    public bool NeedPasswordHashUpdate { get; set; } = false;

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("Usuarios")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [InverseProperty("IdUsuarioCadNavigation")]
    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
    
    [JsonIgnore]
    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<UsuarioPermissao> UsuarioPermissaos { get; set; } = new List<UsuarioPermissao>();

    [JsonIgnore]
    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<ConfiguracoesUsuario> ConfiguracoesUsuarios { get; set; } = new List<ConfiguracoesUsuario>();
    
    [GraphQLIgnore]
    public string GetId()
    {
        return this.NmUsuario;
    }

    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "NmUsuario";
    }
}

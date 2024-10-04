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
    [StringLength(10)]
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

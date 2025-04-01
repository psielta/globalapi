using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[PrimaryKey("CdUsuario", "CdEmpresa")]
[Table("usuario_empresa")]
[Index("Id", Name = "usuario_empresa_id_key", IsUnique = true)]
public partial class UsuarioEmpresa : IIdentifiable<int>
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

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("UsuarioEmpresas")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("CdUsuario")]
    [InverseProperty("UsuarioEmpresas")]
    public virtual Usuario CdUsuarioNavigation { get; set; } = null!;

    [GraphQLIgnore]
    public int GetId()
    {
        return Id;
    }

    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "Id";
    }

    [Column("last_update", TypeName = "timestamp(0) without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }
}

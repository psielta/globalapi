using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[PrimaryKey("CdFuncionario", "NmUsuario")]
[Table("usuario_funcionario")]
[Index("Id", Name = "usuario_funcionario_id_key", IsUnique = true)]
public partial class UsuarioFuncionario : IIdentifiableMultiKey<int, string>
{
    [Column("id")]
    public int Id { get; set; }

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

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

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("UsuarioFuncionarios")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("CdFuncionario, CdEmpresa")]
    [InverseProperty("UsuarioFuncionarios")]
    public virtual Funcionario Funcionario { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("NmUsuario")]
    [InverseProperty("UsuarioFuncionarios")]
    public virtual Usuario NmUsuarioNavigation { get; set; } = null!;
    [GraphQLIgnore]
    public (int, string) GetId()
    {
        return (CdFuncionario, NmUsuario);
    }

    [GraphQLIgnore]
    public string GetKeyName1()
    {
        return "CdFuncionario";
    }

    [GraphQLIgnore]
    public string GetKeyName2()
    {
        return "NmUsuario";
    }
}

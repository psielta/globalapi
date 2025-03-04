using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[PrimaryKey("IdEmpresa", "ChaveAcesso")]
[Table("impxml")]
public partial class Impxml : IIdentifiableMultiKey<int, string>
{
    [Key]
    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Key]
    [Column("chave_acesso")]
    [StringLength(44)]
    public string ChaveAcesso { get; set; } = null!;

    [Column("type")]
    public int Type { get; set; }

    [Column("xml", TypeName = "xml")]
    public string Xml { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdEmpresa")]
    [InverseProperty("Impxmls")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [GraphQLIgnore]
    public (int, string) GetId()
    {
        return (IdEmpresa, ChaveAcesso);
    }

    [GraphQLIgnore]
    public string GetKeyName1()
    {
        return "IdEmpresa";
    }

    [GraphQLIgnore]
    public string GetKeyName2()
    {
        return "ChaveAcesso";
    }
}

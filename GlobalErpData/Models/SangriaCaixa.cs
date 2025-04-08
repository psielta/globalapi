using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[PrimaryKey("Id", "CdEmpresa")]
[Table("sangria_caixa")]
[Index("Sequence", Name = "sangria_caixa_u_idx1", IsUnique = true)]
public partial class SangriaCaixa : IIdentifiableMultiKey<int, int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Key]
    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("data")]
    public DateOnly? Data { get; set; }

    [Column("hora")]
    public TimeOnly? Hora { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("id_abertura_caixa")]
    public int IdAberturaCaixa { get; set; }

    [Column("id_caixa")]
    public int IdCaixa { get; set; }

    [Column("valor")]
    [Precision(15, 2)]
    public decimal Valor { get; set; }

    [Column("tipo")]
    [StringLength(4)]
    public string? Tipo { get; set; }

    [Column("obs")]
    [StringLength(256)]
    public string? Obs { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("SangriaCaixas")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [GraphQLIgnore]
    public (int, int) GetId()
    {
        return (Id, CdEmpresa);
    }

    [GraphQLIgnore]
    public string GetKeyName1()
    {
        return "Id";
    }
    [GraphQLIgnore]
    public string GetKeyName2()
    {
        return "CdEmpresa";
    }

    [Column("unity")]
    public int Unity { get; set; }

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("SangriaCaixas")]
    public virtual Unity UnityNavigation { get; set; } = null!;

    [Column("sequence")]
    public long Sequence { get; set; }

    [JsonIgnore]
    [ForeignKey("IdAberturaCaixa, CdEmpresa")]
    [InverseProperty("SangriaCaixas")]
    public virtual NfceAberturaCaixa NfceAberturaCaixa { get; set; } = null!;
}

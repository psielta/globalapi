using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("saldo_estoque")]
[Index("CdProduto", "CdEmpresa", "CdPlano", Name = "saldo_estoque_unique", IsUnique = true)]
public partial class SaldoEstoque : IIdentifiable<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("cd_produto")]
    public int CdProduto { get; set; }

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("cd_plano")]
    public int CdPlano { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [Column("quant_e")]
    [Precision(18, 4)]
    public decimal? QuantE { get; set; }

    [Column("quant_v")]
    [Precision(18, 4)]
    public decimal? QuantV { get; set; }

    [Column("quant_f")]
    [Precision(18, 4)]
    public decimal? QuantF { get; set; }

    [Column("unity")]
    public int? Unity { get; set; }

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("SaldoEstoques")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("CdPlano")]
    [InverseProperty("SaldoEstoques")]
    public virtual PlanoEstoque CdPlanoNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("CdProduto, Unity")]
    [InverseProperty("SaldoEstoques")]
    public virtual ProdutoEstoque? ProdutoEstoque { get; set; }

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("SaldoEstoques")]
    public virtual Unity? UnityNavigation { get; set; }

    [JsonPropertyName("nmPlano")]
    [NotMapped]
    public string NmPlano => CdPlanoNavigation?.NmPlano ?? string.Empty;

    [JsonPropertyName("nmProduto")]
    [NotMapped]
    public string NmProduto => ProdutoEstoque?.NmProduto ?? "";

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
}
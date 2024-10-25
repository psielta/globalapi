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

    [Column("quant_e")]
    [Precision(18, 4)]
    public decimal? QuantE { get; set; }

    [Column("quant_v")]
    [Precision(18, 4)]
    public decimal? QuantV { get; set; }

    [Column("quant_f")]
    [Precision(18, 4)]
    public decimal? QuantF { get; set; }

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("SaldoEstoques")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("CdPlano")]
    [InverseProperty("SaldoEstoques")]
    public virtual PlanoEstoque CdPlanoNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("CdProduto, CdEmpresa")]
    [InverseProperty("SaldoEstoques")]
    public virtual ProdutoEstoque ProdutoEstoque { get; set; } = null!;

    [JsonPropertyName("nmPlano")]
    [NotMapped]
    public string NmPlano => CdPlanoNavigation?.NmPlano ?? string.Empty;

    [JsonPropertyName("nmProduto")]
    [NotMapped]
    public string NmProduto => ProdutoEstoque?.NmProduto ?? "";

    public int GetId()
    {
        return Id;
    }

    public string GetKeyName()
    {
        return "Id";
    }
}
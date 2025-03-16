using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("produtos_forn")]
[Index("CdForn", "CdProduto", "IdEmpresa", "IdProdutoExterno", "CdBarra", Name = "pk_pedido_item", IsUnique = true)]
public partial class ProdutosForn : IIdentifiable<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("cd_produto")]
    public int CdProduto { get; set; }

    [Column("cd_forn")]
    public int CdForn { get; set; }

    [Column("id_produto_externo")]
    [StringLength(62)]
    public string IdProdutoExterno { get; set; } = null!;

    [Column("cd_barra")]
    [StringLength(14)]
    public string CdBarra { get; set; } = null!;

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [JsonIgnore]
    [ForeignKey("CdForn, Unity")]
    [InverseProperty("ProdutosForns")]
    public virtual Fornecedor Fornecedor { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdEmpresa")]
    [InverseProperty("ProdutosForns")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("CdProduto, IdEmpresa")]
    [InverseProperty("ProdutosForns")]
    public virtual ProdutoEstoque ProdutoEstoque { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("ProdutosForns")]
    public virtual Unity UnityNavigation { get; set; } = null!;

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

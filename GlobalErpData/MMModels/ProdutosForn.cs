using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("produtos_forn")]
[Index("CdProduto", "CdForn", "IdProdutoExterno", "CdBarra", "Unity", Name = "produtos_forn_idx")]
public partial class ProdutosForn
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

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [ForeignKey("CdForn, Unity")]
    [InverseProperty("ProdutosForns")]
    public virtual Fornecedor Fornecedor { get; set; } = null!;

    [ForeignKey("CdProduto, Unity")]
    [InverseProperty("ProdutosForns")]
    public virtual ProdutoEstoque ProdutoEstoque { get; set; } = null!;

    [ForeignKey("Unity")]
    [InverseProperty("ProdutosForns")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}

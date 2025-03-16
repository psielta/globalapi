using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("product_details")]
public partial class ProductDetail
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("id_produto")]
    public int IdProduto { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("ProductDetails")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    [InverseProperty("IdProductDetailsNavigation")]
    public virtual ICollection<ItemDetail> ItemDetails { get; set; } = new List<ItemDetail>();

    [ForeignKey("IdProduto, IdEmpresa")]
    [InverseProperty("ProductDetails")]
    public virtual ProdutoEstoque ProdutoEstoque { get; set; } = null!;
}

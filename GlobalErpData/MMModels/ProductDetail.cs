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

    [Column("unity")]
    public int Unity { get; set; }

    [Column("id_produto")]
    public int IdProduto { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [InverseProperty("IdProductDetailsNavigation")]
    public virtual ICollection<ItemDetail> ItemDetails { get; set; } = new List<ItemDetail>();

    [ForeignKey("IdProduto, Unity")]
    [InverseProperty("ProductDetails")]
    public virtual ProdutoEstoque ProdutoEstoque { get; set; } = null!;

    [ForeignKey("Unity")]
    [InverseProperty("ProductDetails")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}

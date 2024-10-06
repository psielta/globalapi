using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("product_details")]
public partial class ProductDetail : IIdentifiable<int>
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

    [JsonIgnore]
    [ForeignKey("IdEmpresa")]
    [InverseProperty("ProductDetails")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdEmpresa, IdProduto")]
    public virtual ProdutoEstoque ProdutoEstoque { get; set; } = null!;

    [JsonIgnore]
    [InverseProperty("IdProductDetailsNavigation")]
    public virtual ICollection<ItemDetail> ItemDetails { get; set; } = new List<ItemDetail>();

    public int GetId()
    {
        return Id;
    }

    public string GetKeyName()
    {
        return "Id";
    }
}

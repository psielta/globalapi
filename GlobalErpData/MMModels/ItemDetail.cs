using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("item_details")]
public partial class ItemDetail
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("id_product_details")]
    public int IdProductDetails { get; set; }

    [Column("value")]
    [StringLength(255)]
    public string Value { get; set; } = null!;

    [ForeignKey("IdEmpresa")]
    [InverseProperty("ItemDetails")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    [ForeignKey("IdProductDetails")]
    [InverseProperty("ItemDetails")]
    public virtual ProductDetail IdProductDetailsNavigation { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("category")]
public partial class Category
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [InverseProperty("Category")]
    public virtual ICollection<Featured> Featureds { get; set; } = new List<Featured>();

    [ForeignKey("IdEmpresa")]
    [InverseProperty("Categories")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    [InverseProperty("CategoryNavigation")]
    public virtual ICollection<ProdutoEstoque> ProdutoEstoques { get; set; } = new List<ProdutoEstoque>();

    [InverseProperty("Category")]
    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();
}

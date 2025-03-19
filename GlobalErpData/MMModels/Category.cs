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

    [Column("unity")]
    public int Unity { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Featured> Featureds { get; set; } = new List<Featured>();

    [InverseProperty("CategoryNavigation")]
    public virtual ICollection<ProdutoEstoque> ProdutoEstoques { get; set; } = new List<ProdutoEstoque>();

    [InverseProperty("Category")]
    public virtual ICollection<Section> Sections { get; set; } = new List<Section>();

    [ForeignKey("Unity")]
    [InverseProperty("Categories")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}

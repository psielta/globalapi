using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("section_items")]
public partial class SectionItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("section_id")]
    public int? SectionId { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string? Name { get; set; }

    [Column("href")]
    [StringLength(255)]
    public string? Href { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [InverseProperty("SectionItem")]
    public virtual ICollection<ProdutoEstoque> ProdutoEstoques { get; set; } = new List<ProdutoEstoque>();

    [ForeignKey("SectionId")]
    [InverseProperty("SectionItems")]
    public virtual Section? Section { get; set; }

    [ForeignKey("Unity")]
    [InverseProperty("SectionItems")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}

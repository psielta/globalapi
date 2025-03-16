using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("sections")]
public partial class Section
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("category_id")]
    public int? CategoryId { get; set; }

    [Column("section_id")]
    [StringLength(50)]
    public string? SectionId { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string? Name { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Sections")]
    public virtual Category? Category { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("Sections")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    [InverseProperty("Section")]
    public virtual ICollection<ProdutoEstoque> ProdutoEstoques { get; set; } = new List<ProdutoEstoque>();

    [InverseProperty("Section")]
    public virtual ICollection<SectionItem> SectionItems { get; set; } = new List<SectionItem>();
}

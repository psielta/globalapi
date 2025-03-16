using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[PrimaryKey("Id", "IdEmpresa")]
[Table("featured")]
public partial class Featured
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("category_id")]
    public int CategoryId { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string? Name { get; set; }

    [Column("href")]
    [StringLength(255)]
    public string? Href { get; set; }

    [Column("image_src")]
    [StringLength(255)]
    public string? ImageSrc { get; set; }

    [Column("image_alt")]
    [StringLength(255)]
    public string? ImageAlt { get; set; }

    [Key]
    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("excluiu")]
    public bool? Excluiu { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Featureds")]
    public virtual Category Category { get; set; } = null!;

    [ForeignKey("IdEmpresa")]
    [InverseProperty("Featureds")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    [InverseProperty("Featured")]
    public virtual ICollection<ProdutoEstoque> ProdutoEstoques { get; set; } = new List<ProdutoEstoque>();
}

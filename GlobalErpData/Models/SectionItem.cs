using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("section_items")]
public partial class SectionItem : IIdentifiable<int>
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

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [JsonIgnore]
    [ForeignKey("IdEmpresa")]
    [InverseProperty("SectionItems")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [InverseProperty("SectionItem")]
    public virtual ICollection<ProdutoEstoque> ProdutoEstoques { get; set; } = new List<ProdutoEstoque>();

    [JsonIgnore]
    [ForeignKey("SectionId")]
    [InverseProperty("SectionItems")]
    public virtual Section? Section { get; set; }

    [GraphQLIgnore]
    public int GetId()
    {
        return Id;
    }

    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "Id";
    }
}

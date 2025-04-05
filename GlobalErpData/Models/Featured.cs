using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[PrimaryKey("Id", "Unity")]
[Table("featured")]
public partial class Featured : IIdentifiableMultiKey<int, int>
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
    [Column("unity")]
    public int Unity { get; set; }

    [Column("excluiu")]
    public bool? Excluiu { get; set; }
    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }
    [JsonIgnore]
    [ForeignKey("CategoryId")]
    [InverseProperty("Featureds")]
    public virtual Category Category { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("Featureds")]
    public virtual Unity UnityNavigation { get; set; } = null!;

    [GraphQLIgnore]
    public (int, int) GetId()
    {
        return (Unity, Id);
    }

    [GraphQLIgnore]
    public string GetKeyName1()
    {
        return "Unity";
    }

    [GraphQLIgnore]
    public string GetKeyName2()
    {
        return "Id";
    }

    [Column("image")]
    public byte[]? Image { get; set; }
}
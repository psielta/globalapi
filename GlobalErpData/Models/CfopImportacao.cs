using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("cfop_importacao")]
[Index("CdCfopS", "CdCfopE", "Unity", Name = "cfop_importacao_unique", IsUnique = true)]
public partial class CfopImportacao : IIdentifiable<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("cd_cfop_s")]
    [StringLength(4)]
    public string CdCfopS { get; set; } = null!;

    [Column("cd_cfop_e")]
    [StringLength(4)]
    public string CdCfopE { get; set; } = null!;

    [Column("cfop_dentro")]
    [StringLength(4)]
    public string? CfopDentro { get; set; }

    [Column("cfop_fora")]
    [StringLength(4)]
    public string? CfopFora { get; set; }

    [Column("csosn")]
    [StringLength(4)]
    public string? Csosn { get; set; }

    [Column("unity")]
    public int Unity { get; set; }
    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }
    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("CfopImportacaos")]
    public virtual Unity UnityNavigation { get; set; } = null!;

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

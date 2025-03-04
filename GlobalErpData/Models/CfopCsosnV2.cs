using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("cfop_csosn_v2")]
[Index("IdEmpresa", "Cfop", "Csosn", Name = "cfop_csosn_v2_idx", IsUnique = true)]
public partial class CfopCsosnV2 : IIdentifiable<int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("cfop")]
    [StringLength(5)]
    public string Cfop { get; set; } = null!;

    [Column("csosn")]
    [StringLength(10)]
    public string Csosn { get; set; } = null!;
    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }
    [GraphQLIgnore]
    [ForeignKey("IdEmpresa")]
    [InverseProperty("CfopCsosnV2s")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

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

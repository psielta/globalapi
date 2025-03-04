using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("tipo_nf")]
public partial class TipoNf : IIdentifiable<string>
{
    [Key]
    [Column("cd_tipo_nf")]
    [StringLength(2)]
    public string CdTipoNf { get; set; } = null!;

    [Column("nm_tipo_nf")]
    [StringLength(62)]
    public string NmTipoNf { get; set; } = null!;

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [GraphQLIgnore]
    public string GetId()
    {
        return CdTipoNf;
    }

    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "CdTipoNf";
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("cfop")]
[Index("Descricao", Name = "cfop_idx")]
public partial class Cfop
{
    [Key]
    [Column("cd_cfop")]
    [StringLength(5)]
    public string CdCfop { get; set; } = null!;

    [Column("descricao")]
    [StringLength(155)]
    public string? Descricao { get; set; }

    [Column("tipo_cfop")]
    [StringLength(1)]
    public string? TipoCfop { get; set; }

    [Column("desc_nfe")]
    [StringLength(62)]
    public string? DescNfe { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }
}

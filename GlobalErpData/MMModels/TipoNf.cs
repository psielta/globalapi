using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("tipo_nf")]
public partial class TipoNf
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
}

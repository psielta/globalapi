using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[PrimaryKey("ChNfe", "NrDup")]
[Table("impdupnfe")]
public partial class Impdupnfe
{
    [Key]
    [Column("ch_nfe")]
    [StringLength(150)]
    public string ChNfe { get; set; } = null!;

    [Key]
    [Column("nr_dup")]
    [StringLength(50)]
    public string NrDup { get; set; } = null!;

    [Column("dt_venc", TypeName = "timestamp without time zone")]
    public DateTime? DtVenc { get; set; }

    [Column("valor")]
    [StringLength(50)]
    public string? Valor { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }
}

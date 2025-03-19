using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("csosn")]
public partial class Csosn
{
    [Key]
    [Column("codigo")]
    [StringLength(5)]
    public string Codigo { get; set; } = null!;

    [Column("descricao")]
    [StringLength(512)]
    public string? Descricao { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }
}

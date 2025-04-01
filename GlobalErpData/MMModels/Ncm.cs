using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("ncm")]
[Index("Ncm1", Name = "idx_ncm_codigo")]
public partial class Ncm
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("ncm")]
    [StringLength(8)]
    public string? Ncm1 { get; set; }

    [Column("dt_vigencia")]
    public DateOnly? DtVigencia { get; set; }

    [Column("unidade")]
    [StringLength(10)]
    public string? Unidade { get; set; }

    [Column("descricao")]
    [StringLength(255)]
    public string? Descricao { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }
}

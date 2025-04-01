using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("cest_ncm")]
[Index("NrCest", Name = "idx_cest_ncm_cest")]
[Index("NrNcm", Name = "idx_cest_ncm_ncm")]
public partial class CestNcm
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nr_cest")]
    [StringLength(7)]
    public string NrCest { get; set; } = null!;

    [Column("nr_ncm")]
    [StringLength(8)]
    public string NrNcm { get; set; } = null!;

    [Column("descricao")]
    [StringLength(16384)]
    public string? Descricao { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }
}

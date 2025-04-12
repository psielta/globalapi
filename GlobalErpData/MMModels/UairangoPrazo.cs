using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("uairango_prazos")]
public partial class UairangoPrazo
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_tempo")]
    public int? IdTempo { get; set; }

    [Column("min")]
    public int? Min { get; set; }

    [Column("max")]
    public int? Max { get; set; }

    [Column("status")]
    public int? Status { get; set; }

    [Column("last_update", TypeName = "timestamp(6) without time zone")]
    public DateTime? LastUpdate { get; set; }
}

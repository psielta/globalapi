using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("obs_nf")]
public partial class ObsNf
{
    [Key]
    [Column("nr_lanc")]
    public int NrLanc { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [Column("nm_obs")]
    [StringLength(150)]
    public string NmObs { get; set; } = null!;

    [Column("txt_obs")]
    [StringLength(16384)]
    public string? TxtObs { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [ForeignKey("Unity")]
    [InverseProperty("ObsNfs")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}

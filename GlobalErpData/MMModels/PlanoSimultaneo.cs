using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("plano_simultaneos")]
public partial class PlanoSimultaneo
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [Column("cd_plano_princ")]
    public int CdPlanoPrinc { get; set; }

    [Column("cd_plano_replica")]
    public int CdPlanoReplica { get; set; }

    [ForeignKey("CdPlanoPrinc")]
    [InverseProperty("PlanoSimultaneoCdPlanoPrincNavigations")]
    public virtual PlanoEstoque CdPlanoPrincNavigation { get; set; } = null!;

    [ForeignKey("CdPlanoReplica")]
    [InverseProperty("PlanoSimultaneoCdPlanoReplicaNavigations")]
    public virtual PlanoEstoque CdPlanoReplicaNavigation { get; set; } = null!;

    [ForeignKey("Unity")]
    [InverseProperty("PlanoSimultaneos")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}

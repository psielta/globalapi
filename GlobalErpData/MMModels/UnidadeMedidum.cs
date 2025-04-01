using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("unidade_medida")]
[Index("CdUnidade", "Unity", Name = "unidade_medida_idx", IsUnique = true)]
public partial class UnidadeMedidum
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("cd_unidade")]
    [StringLength(6)]
    public string CdUnidade { get; set; } = null!;

    [Column("descricao")]
    [StringLength(62)]
    public string Descricao { get; set; } = null!;

    [Column("unity")]
    public int Unity { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [ForeignKey("Unity")]
    [InverseProperty("UnidadeMedida")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}

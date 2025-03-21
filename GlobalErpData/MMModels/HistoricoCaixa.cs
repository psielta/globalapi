using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("historico_caixa")]
[Index("Unity", "CdSubPlano", "CdPlano", Name = "historico_caixa_idx1", IsUnique = true)]
public partial class HistoricoCaixa
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [Column("cd_sub_plano")]
    [StringLength(25)]
    public string CdSubPlano { get; set; } = null!;

    [Column("cd_plano")]
    [StringLength(25)]
    public string CdPlano { get; set; } = null!;

    [Column("tipo")]
    [StringLength(1)]
    public string Tipo { get; set; } = null!;

    [Column("descricao")]
    [StringLength(62)]
    public string Descricao { get; set; } = null!;

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [InverseProperty("HistoricoCaixa")]
    public virtual ICollection<ContasAPagar> ContasAPagars { get; set; } = new List<ContasAPagar>();

    [InverseProperty("HistoricoCaixa")]
    public virtual ICollection<ContasAReceber> ContasARecebers { get; set; } = new List<ContasAReceber>();

    [InverseProperty("HistoricoCaixa")]
    public virtual ICollection<LivroCaixa> LivroCaixas { get; set; } = new List<LivroCaixa>();

    [ForeignKey("CdPlano, Unity")]
    [InverseProperty("HistoricoCaixas")]
    public virtual PlanoDeCaixa PlanoDeCaixa { get; set; } = null!;

    [ForeignKey("Unity")]
    [InverseProperty("HistoricoCaixas")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}

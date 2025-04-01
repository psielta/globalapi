using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("plano_de_caixa")]
[Index("CdClassificacao", "Unity", Name = "plano_de_caixa_idx", IsUnique = true)]
public partial class PlanoDeCaixa
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [Column("cd_classificacao")]
    [StringLength(25)]
    public string CdClassificacao { get; set; } = null!;

    [Column("descricao")]
    [StringLength(62)]
    public string Descricao { get; set; } = null!;

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [InverseProperty("PlanoDeCaixa")]
    public virtual ICollection<HistoricoCaixa> HistoricoCaixas { get; set; } = new List<HistoricoCaixa>();

    [ForeignKey("Unity")]
    [InverseProperty("PlanoDeCaixas")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}

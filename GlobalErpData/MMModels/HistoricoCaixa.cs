using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("historico_caixa")]
[Index("CdEmpresa", "CdSubPlano", "CdPlano", Name = "historico_caixa_idx1", IsUnique = true)]
public partial class HistoricoCaixa
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

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

    [ForeignKey("CdEmpresa")]
    [InverseProperty("HistoricoCaixas")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [InverseProperty("HistoricoCaixa")]
    public virtual ICollection<ContasAPagar> ContasAPagars { get; set; } = new List<ContasAPagar>();

    [ForeignKey("CdPlano, CdEmpresa")]
    [InverseProperty("HistoricoCaixas")]
    public virtual PlanoDeCaixa PlanoDeCaixa { get; set; } = null!;
}

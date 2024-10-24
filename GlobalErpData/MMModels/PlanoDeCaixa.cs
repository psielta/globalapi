using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("plano_de_caixa")]
[Index("CdClassificacao", "CdEmpresa", Name = "plano_de_caixa_idx", IsUnique = true)]
public partial class PlanoDeCaixa
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("cd_classificacao")]
    [StringLength(25)]
    public string CdClassificacao { get; set; } = null!;

    [Column("descricao")]
    [StringLength(62)]
    public string Descricao { get; set; } = null!;

    [ForeignKey("CdEmpresa")]
    [InverseProperty("PlanoDeCaixas")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [InverseProperty("PlanoDeCaixa")]
    public virtual ICollection<HistoricoCaixa> HistoricoCaixas { get; set; } = new List<HistoricoCaixa>();
}

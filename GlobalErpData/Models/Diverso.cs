using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("diversos")]
[Index("CdTipo", Name = "diversos_idx")]
[Index("CdChave", Name = "diversos_idx1")]
public partial class Diverso
{
    [Key]
    [Column("cd_div")]
    public int CdDiv { get; set; }

    [Column("nm_div")]
    [StringLength(62)]
    public string NmDiv { get; set; } = null!;

    [Column("cd_tipo")]
    [StringLength(6)]
    public string CdTipo { get; set; } = null!;

    [Column("cd_chave")]
    [StringLength(15)]
    public string CdChave { get; set; } = null!;

    [Column("cd_historico")]
    [StringLength(25)]
    public string? CdHistorico { get; set; }

    [Column("nr_conta")]
    public int? NrConta { get; set; }

    [Column("cd_plano_caixa")]
    [StringLength(25)]
    public string? CdPlanoCaixa { get; set; }

    [Column("cd_plano_estoque")]
    public int? CdPlanoEstoque { get; set; }

    [Column("observacao")]
    [StringLength(16384)]
    public string? Observacao { get; set; }
    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }
}

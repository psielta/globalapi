using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("livro_caixa")]
public partial class LivroCaixa
{
    [Key]
    [Column("nr_lanc")]
    public long NrLanc { get; set; }

    [Column("dt_lanc")]
    public DateTime DtLanc { get; set; }

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("cd_historico")]
    [StringLength(25)]
    public string CdHistorico { get; set; } = null!;

    [Column("vl_lancamento")]
    [Precision(18, 4)]
    public decimal VlLancamento { get; set; }

    [Column("nr_cp")]
    public int? NrCp { get; set; }

    [Column("nr_cr")]
    public int? NrCr { get; set; }

    [Column("txt_obs")]
    [StringLength(16384)]
    public string? TxtObs { get; set; }

    [Column("nr_conta")]
    public int NrConta { get; set; }

    [Column("cd_plano")]
    [StringLength(25)]
    public string CdPlano { get; set; } = null!;

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [ForeignKey("CdEmpresa")]
    [InverseProperty("LivroCaixas")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [ForeignKey("CdEmpresa, CdHistorico, CdPlano")]
    [InverseProperty("LivroCaixas")]
    public virtual HistoricoCaixa HistoricoCaixa { get; set; } = null!;

    [ForeignKey("NrConta")]
    [InverseProperty("LivroCaixas")]
    public virtual ContaDoCaixa NrContaNavigation { get; set; } = null!;

    [ForeignKey("NrCp")]
    [InverseProperty("LivroCaixas")]
    public virtual ContasAPagar? NrCpNavigation { get; set; }

    [ForeignKey("NrCr")]
    [InverseProperty("LivroCaixas")]
    public virtual ContasAReceber? NrCrNavigation { get; set; }
}

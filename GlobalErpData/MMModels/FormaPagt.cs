using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("forma_pagt")]
public partial class FormaPagt
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("nm_forma")]
    [StringLength(150)]
    public string NmForma { get; set; } = null!;

    [Column("prz01")]
    public short Prz01 { get; set; }

    [Column("prz02")]
    public short? Prz02 { get; set; }

    [Column("prz03")]
    public short? Prz03 { get; set; }

    [Column("prz04")]
    public short? Prz04 { get; set; }

    [Column("prz05")]
    public short? Prz05 { get; set; }

    [Column("prz06")]
    public short? Prz06 { get; set; }

    [Column("prz07")]
    public short? Prz07 { get; set; }

    [Column("prz08")]
    public short? Prz08 { get; set; }

    [Column("prz09")]
    public short? Prz09 { get; set; }

    [Column("prz10")]
    public short? Prz10 { get; set; }

    [Column("txt_obs")]
    [StringLength(16384)]
    public string? TxtObs { get; set; }

    [Column("cd_plano_caixa")]
    [StringLength(25)]
    public string? CdPlanoCaixa { get; set; }

    [Column("cd_historico_caixa")]
    [StringLength(25)]
    public string? CdHistoricoCaixa { get; set; }

    [Column("cd_plano_caixa_d")]
    [StringLength(25)]
    public string? CdPlanoCaixaD { get; set; }

    [Column("cd_historico_caixa_d")]
    [StringLength(25)]
    public string? CdHistoricoCaixaD { get; set; }

    [Column("tipo_prazo")]
    [StringLength(1)]
    public string? TipoPrazo { get; set; }

    [Column("integrado")]
    [StringLength(1)]
    public string? Integrado { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [ForeignKey("CdEmpresa")]
    [InverseProperty("FormaPagts")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;
}

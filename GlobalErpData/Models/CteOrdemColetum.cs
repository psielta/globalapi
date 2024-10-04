using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("cte_ordem_coleta", Schema = "cte")]
public partial class CteOrdemColetum
{
    [Key]
    [Column("codigo")]
    public int Codigo { get; set; }

    [Column("serie_occ")]
    [StringLength(3)]
    public string? SerieOcc { get; set; }

    [Column("nr_occ")]
    public int NrOcc { get; set; }

    [Column("dt_emissao")]
    public DateOnly DtEmissao { get; set; }

    [Column("cnpj")]
    [StringLength(14)]
    public string? Cnpj { get; set; }

    [Column("insc_estadual")]
    [StringLength(64)]
    public string? InscEstadual { get; set; }

    [Column("uf")]
    [StringLength(2)]
    public string? Uf { get; set; }

    [Column("telefone")]
    [StringLength(11)]
    public string? Telefone { get; set; }

    [Column("cd_interno")]
    [StringLength(10)]
    public string? CdInterno { get; set; }

    [Column("nr_cte")]
    public int NrCte { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("CteOrdemColeta")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}

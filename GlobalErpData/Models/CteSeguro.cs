using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("cte_seguro", Schema = "cte")]
public partial class CteSeguro
{
    [Key]
    [Column("id_seguro")]
    public int IdSeguro { get; set; }

    [Column("responsavel")]
    [StringLength(2)]
    public string Responsavel { get; set; } = null!;

    [Column("nm_seguradoura")]
    [StringLength(30)]
    public string? NmSeguradoura { get; set; }

    [Column("nr_apolice")]
    [StringLength(20)]
    public string? NrApolice { get; set; }

    [Column("nr_averbacao")]
    [StringLength(20)]
    public string? NrAverbacao { get; set; }

    [Column("vl_mercadoria")]
    [Precision(18, 2)]
    public decimal? VlMercadoria { get; set; }

    [Column("nr_cte")]
    [StringLength(10)]
    public string NrCte { get; set; } = null!;

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("CteSeguros")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}

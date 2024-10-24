using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("cte_nf", Schema = "cte")]
public partial class CteNf
{
    [Key]
    [Column("id_nf")]
    public int IdNf { get; set; }

    [Column("serie")]
    [StringLength(3)]
    public string Serie { get; set; } = null!;

    [Column("numero")]
    [StringLength(20)]
    public string Numero { get; set; } = null!;

    [Column("dt_emissao")]
    public DateOnly DtEmissao { get; set; }

    [Column("cfop")]
    [StringLength(4)]
    public string Cfop { get; set; } = null!;

    [Column("modelo")]
    [StringLength(2)]
    public string Modelo { get; set; } = null!;

    [Column("nr_romaneio_nf")]
    [StringLength(20)]
    public string? NrRomaneioNf { get; set; }

    [Column("nr_pedido_nf")]
    [StringLength(20)]
    public string? NrPedidoNf { get; set; }

    [Column("dt_prevista")]
    public DateOnly? DtPrevista { get; set; }

    [Column("bc_icms")]
    [Precision(18, 2)]
    public decimal BcIcms { get; set; }

    [Column("vl_icms")]
    [Precision(18, 2)]
    public decimal VlIcms { get; set; }

    [Column("bc_icms_st")]
    [Precision(18, 2)]
    public decimal BcIcmsSt { get; set; }

    [Column("vl_icms_st")]
    [Precision(18, 2)]
    public decimal VlIcmsSt { get; set; }

    [Column("peso")]
    [Precision(18, 3)]
    public decimal? Peso { get; set; }

    [Column("pin")]
    public int? Pin { get; set; }

    [Column("vl_prod")]
    [Precision(18, 2)]
    public decimal VlProd { get; set; }

    [Column("vl_nota")]
    [Precision(18, 2)]
    public decimal VlNota { get; set; }

    [Column("tp_unidade")]
    [StringLength(2)]
    public string TpUnidade { get; set; } = null!;

    [Column("nr_cte")]
    [StringLength(10)]
    public string NrCte { get; set; } = null!;

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("CteNfs")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}

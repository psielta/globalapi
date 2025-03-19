using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("mdfe_reboque", Schema = "mdfe")]
public partial class MdfeReboque
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_mdfe")]
    [StringLength(10)]
    public string IdMdfe { get; set; } = null!;

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("cint")]
    [StringLength(10)]
    public string? Cint { get; set; }

    [Column("placa")]
    [StringLength(7)]
    public string? Placa { get; set; }

    [Column("renavam")]
    [StringLength(11)]
    public string? Renavam { get; set; }

    [Column("tara")]
    [Precision(15, 2)]
    public decimal? Tara { get; set; }

    [Column("capkg")]
    [Precision(15, 2)]
    public decimal? Capkg { get; set; }

    [Column("capm3")]
    [Precision(15, 2)]
    public decimal? Capm3 { get; set; }

    [Column("tprod")]
    [StringLength(2)]
    public string? Tprod { get; set; }

    [Column("tpcar")]
    [StringLength(2)]
    public string? Tpcar { get; set; }

    [Column("uf")]
    [StringLength(2)]
    public string? Uf { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("MdfeReboques")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}

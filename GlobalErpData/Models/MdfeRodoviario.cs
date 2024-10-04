using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("mdfe_rodoviario", Schema = "mdfe")]
public partial class MdfeRodoviario
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_mdfe")]
    [StringLength(10)]
    public string IdMdfe { get; set; } = null!;

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("rntrc")]
    [StringLength(8)]
    public string? Rntrc { get; set; }

    [Column("ciot")]
    [StringLength(12)]
    public string? Ciot { get; set; }

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
    public int? Tara { get; set; }

    [Column("capkg")]
    public int? Capkg { get; set; }

    [Column("capm3")]
    public int? Capm3 { get; set; }

    [Column("tprod")]
    [StringLength(2)]
    public string Tprod { get; set; } = null!;

    [Column("tpcar")]
    [StringLength(2)]
    public string? Tpcar { get; set; }

    [Column("uf")]
    [StringLength(2)]
    public string? Uf { get; set; }

    [Column("peso_total")]
    [Precision(18, 2)]
    public decimal? PesoTotal { get; set; }

    [Column("valor_carga")]
    [Precision(18, 2)]
    public decimal? ValorCarga { get; set; }

    [Column("prop_cpfcnpj")]
    [StringLength(14)]
    public string? PropCpfcnpj { get; set; }

    [Column("prop_rntrc")]
    [StringLength(8)]
    public string? PropRntrc { get; set; }

    [Column("prop_nome")]
    [StringLength(40)]
    public string? PropNome { get; set; }

    [Column("prop_uf")]
    [StringLength(2)]
    public string? PropUf { get; set; }

    [Column("prop_ie")]
    [StringLength(14)]
    public string? PropIe { get; set; }

    [Column("prop_tipo")]
    [StringLength(2)]
    public string? PropTipo { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("MdfeRodoviarios")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}

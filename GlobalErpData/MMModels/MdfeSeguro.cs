using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Keyless]
[Table("mdfe_seguro", Schema = "mdfe")]
public partial class MdfeSeguro
{
    [Column("id")]
    public int Id { get; set; }

    [Column("id_mdfe")]
    [StringLength(10)]
    public string IdMdfe { get; set; } = null!;

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("responsavel")]
    [StringLength(64)]
    public string? Responsavel { get; set; }

    [Column("nm_seguradora")]
    [StringLength(64)]
    public string? NmSeguradora { get; set; }

    [Column("nr_apolice")]
    [StringLength(32)]
    public string? NrApolice { get; set; }

    [Column("nr_averbacao")]
    [StringLength(32)]
    public string? NrAverbacao { get; set; }

    [Column("vl_mercadoria")]
    [Precision(18, 2)]
    public decimal? VlMercadoria { get; set; }

    [Column("cnpj")]
    [StringLength(20)]
    public string? Cnpj { get; set; }

    [ForeignKey("IdEmpresa")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}

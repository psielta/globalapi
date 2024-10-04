using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("cte_outros_doc", Schema = "cte")]
public partial class CteOutrosDoc
{
    [Key]
    [Column("codigo")]
    public int Codigo { get; set; }

    [Column("tp_documento")]
    [StringLength(2)]
    public string TpDocumento { get; set; } = null!;

    [Column("descricao")]
    [StringLength(100)]
    public string? Descricao { get; set; }

    [Column("numero")]
    [StringLength(20)]
    public string? Numero { get; set; }

    [Column("dt_emissao")]
    public DateOnly DtEmissao { get; set; }

    [Column("dt_prevista")]
    public DateOnly? DtPrevista { get; set; }

    [Column("vl_documento")]
    [Precision(18, 2)]
    public decimal? VlDocumento { get; set; }

    [Column("tp_unidade")]
    [StringLength(2)]
    public string? TpUnidade { get; set; }

    [Column("nr_cte")]
    public int NrCte { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("CteOutrosDocs")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}

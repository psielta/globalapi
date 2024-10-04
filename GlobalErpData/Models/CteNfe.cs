using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("cte_nfe", Schema = "cte")]
public partial class CteNfe
{
    [Key]
    [Column("id_nfe")]
    public int IdNfe { get; set; }

    [Column("chave")]
    [StringLength(44)]
    public string Chave { get; set; } = null!;

    [Column("pin")]
    public int? Pin { get; set; }

    [Column("dt_prevista")]
    public DateOnly? DtPrevista { get; set; }

    [Column("tp_unidade")]
    [StringLength(2)]
    public string? TpUnidade { get; set; }

    [Column("nr_cte")]
    [StringLength(10)]
    public string NrCte { get; set; } = null!;

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("CteNves")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}

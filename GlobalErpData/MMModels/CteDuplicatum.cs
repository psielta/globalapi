using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("cte_duplicata", Schema = "cte")]
public partial class CteDuplicatum
{
    [Key]
    [Column("codigo")]
    public int Codigo { get; set; }

    [Column("numero")]
    [StringLength(60)]
    public string? Numero { get; set; }

    [Column("dt_venc")]
    public DateOnly? DtVenc { get; set; }

    [Column("valor")]
    [Precision(18, 2)]
    public decimal? Valor { get; set; }

    [Column("nr_cte")]
    public int NrCte { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("CteDuplicata")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}

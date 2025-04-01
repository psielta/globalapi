using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("cte_obs", Schema = "cte")]
public partial class CteOb
{
    [Key]
    [Column("codigo")]
    public int Codigo { get; set; }

    [Column("tp_obs")]
    [StringLength(2)]
    public string TpObs { get; set; } = null!;

    [Column("identificador")]
    [StringLength(20)]
    public string Identificador { get; set; } = null!;

    [Column("obs")]
    [StringLength(16384)]
    public string Obs { get; set; } = null!;

    [Column("nr_cte")]
    public int NrCte { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("CteObs")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}

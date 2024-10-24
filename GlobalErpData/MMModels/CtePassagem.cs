using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("cte_passagem", Schema = "cte")]
public partial class CtePassagem
{
    [Key]
    [Column("codigo")]
    public int Codigo { get; set; }

    [Column("nr_passagem")]
    [StringLength(15)]
    public string NrPassagem { get; set; } = null!;

    [Column("nr_cte")]
    public int NrCte { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("CtePassagems")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}

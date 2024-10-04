using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("cte_qtd_carga", Schema = "cte")]
public partial class CteQtdCarga
{
    [Key]
    [Column("codigo")]
    public int Codigo { get; set; }

    [Column("un_medida")]
    [StringLength(2)]
    public string UnMedida { get; set; } = null!;

    [Column("tp_medida")]
    [StringLength(20)]
    public string TpMedida { get; set; } = null!;

    [Column("qtd")]
    [Precision(18, 3)]
    public decimal Qtd { get; set; }

    [Column("nr_cte")]
    public int NrCte { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("CteQtdCargas")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}

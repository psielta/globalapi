using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("cte_comp_prestacao", Schema = "cte")]
public partial class CteCompPrestacao
{
    [Key]
    [Column("id_comp_prestacao")]
    public int IdCompPrestacao { get; set; }

    [Column("nome")]
    [StringLength(15)]
    public string Nome { get; set; } = null!;

    [Column("valor")]
    [Precision(18, 2)]
    public decimal Valor { get; set; }

    [Column("nr_cte")]
    [StringLength(10)]
    public string NrCte { get; set; } = null!;

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("CteCompPrestacaos")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}

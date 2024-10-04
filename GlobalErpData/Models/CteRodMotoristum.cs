using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("cte_rod_motorista", Schema = "cte")]
public partial class CteRodMotoristum
{
    [Key]
    [Column("codigo")]
    public int Codigo { get; set; }

    [Column("cpf")]
    [StringLength(11)]
    public string Cpf { get; set; } = null!;

    [Column("nome")]
    [StringLength(60)]
    public string Nome { get; set; } = null!;

    [Column("nr_cte")]
    public int NrCte { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("CteRodMotorista")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}

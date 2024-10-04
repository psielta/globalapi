using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("cte_prod_perigoso", Schema = "cte")]
public partial class CteProdPerigoso
{
    [Key]
    [Column("codigo")]
    public int Codigo { get; set; }

    [Column("classe_risco")]
    public int? ClasseRisco { get; set; }

    [Column("nr_onu")]
    public int NrOnu { get; set; }

    [Column("grupo_embalagem")]
    [StringLength(6)]
    public string? GrupoEmbalagem { get; set; }

    [Column("ponto_fulgor")]
    [StringLength(6)]
    public string? PontoFulgor { get; set; }

    [Column("nm_embarque_prod")]
    [StringLength(16384)]
    public string NmEmbarqueProd { get; set; } = null!;

    [Column("qtd_total_prod")]
    [Precision(18, 3)]
    public decimal QtdTotalProd { get; set; }

    [Column("qtd_tipo_volume")]
    [StringLength(60)]
    public string? QtdTipoVolume { get; set; }

    [Column("nr_cte")]
    public int NrCte { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("CteProdPerigosos")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}

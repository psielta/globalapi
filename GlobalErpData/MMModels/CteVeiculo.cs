using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("cte_veiculo", Schema = "cte")]
public partial class CteVeiculo
{
    [Key]
    [Column("codigo")]
    public int Codigo { get; set; }

    [Column("cor")]
    [StringLength(3)]
    public string Cor { get; set; } = null!;

    [Column("nm_cor")]
    [StringLength(40)]
    public string NmCor { get; set; } = null!;

    [Column("cd_marca_modelo")]
    [StringLength(6)]
    public string CdMarcaModelo { get; set; } = null!;

    [Column("chassi")]
    [StringLength(17)]
    public string Chassi { get; set; } = null!;

    [Column("vl_veiculo")]
    [Precision(18, 2)]
    public decimal VlVeiculo { get; set; }

    [Column("frete")]
    [Precision(18, 2)]
    public decimal Frete { get; set; }

    [Column("nr_cte")]
    public int NrCte { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("CteVeiculos")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}

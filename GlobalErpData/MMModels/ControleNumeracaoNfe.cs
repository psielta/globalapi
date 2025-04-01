using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[Table("controle_numeracao_nfe")]
[Index("IdEmpresa", "Serie", Name = "controle_numeracao_nfe_idx", IsUnique = true)]
public partial class ControleNumeracaoNfe
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [Column("serie")]
    public int Serie { get; set; }

    [Column("proximo_numero")]
    public long ProximoNumero { get; set; }

    [Column("padrao")]
    public bool Padrao { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("ControleNumeracaoNfe")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;

    [ForeignKey("Unity")]
    [InverseProperty("ControleNumeracaoNves")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}

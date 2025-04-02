using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[PrimaryKey("Id", "CdEmpresa")]
[Table("nfce_forma_pgt")]
[Index("Sequence", Name = "nfce_forma_pgt_u_idx1", IsUnique = true)]
public partial class NfceFormaPgt
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_saida")]
    public int? IdSaida { get; set; }

    [Key]
    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("forma")]
    [StringLength(30)]
    public string? Forma { get; set; }

    [Column("valor")]
    [Precision(18, 2)]
    public decimal? Valor { get; set; }

    [Column("troco")]
    [Precision(18, 2)]
    public decimal? Troco { get; set; }

    [Column("bandeira")]
    [StringLength(30)]
    public string? Bandeira { get; set; }

    [Column("cnpj")]
    [StringLength(20)]
    public string? Cnpj { get; set; }

    [Column("nsu")]
    [StringLength(20)]
    public string? Nsu { get; set; }

    [Column("nr_abertura_caixa")]
    public int? NrAberturaCaixa { get; set; }

    [Column("caixa")]
    public int? Caixa { get; set; }

    [Column("tp_integra")]
    public int? TpIntegra { get; set; }

    [Column("nr_autorizacao_operacao")]
    [StringLength(100)]
    public string? NrAutorizacaoOperacao { get; set; }

    [Column("pwinfo_reqnum")]
    public string? PwinfoReqnum { get; set; }

    [Column("rede")]
    [StringLength(50)]
    public string? Rede { get; set; }

    [Column("primeira_via_tef")]
    public string? PrimeiraViaTef { get; set; }

    [Column("segunda_via_tef")]
    public string? SegundaViaTef { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [Column("sequence")]
    public long Sequence { get; set; }

    [ForeignKey("CdEmpresa")]
    [InverseProperty("NfceFormaPgts")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [ForeignKey("IdSaida, CdEmpresa")]
    [InverseProperty("NfceFormaPgts")]
    public virtual NfceSaida? NfceSaida { get; set; }

    [ForeignKey("Unity")]
    [InverseProperty("NfceFormaPgts")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}

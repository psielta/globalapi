using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[PrimaryKey("Id", "CdEmpresa")]
[Table("nfce_forma_pgt")]
public partial class NfceFormaPgt : IIdentifiableMultiKey<int, int>
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

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("NfceFormaPgts")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdSaida, CdEmpresa")]
    [InverseProperty("NfceFormaPgts")]
    public virtual NfceSaida? NfceSaida { get; set; }

    [GraphQLIgnore]
    public (int, int) GetId()
    {
        return (Id, CdEmpresa);
    }

    [GraphQLIgnore]
    public string GetKeyName1()
    {
        return "Id";
    }

    [GraphQLIgnore]
    public string GetKeyName2()
    {
        return "CdEmpresa";
    }
}

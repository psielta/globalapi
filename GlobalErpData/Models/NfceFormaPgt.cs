using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Keyless]
[Table("nfce_forma_pgt")]
public partial class NfceFormaPgt : IIdentifiable<long>
{
    [Column("id")]
    public long Id { get; set; }

    [Column("id_saida")]
    public long IdSaida { get; set; }

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
    public long? NrAberturaCaixa { get; set; }

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

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdSaida")]
    public virtual NfceSaida IdSaidaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("NrAberturaCaixa")]
    public virtual NfceAberturaCaixa? NrAberturaCaixaNavigation { get; set; }

    [GraphQLIgnore]
    public long GetId()
    {
        return Id;
    }

    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "Id";
    }
}

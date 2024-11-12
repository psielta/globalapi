using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("nfce_saidas")]
[Index("CdEmpresa", "NrNotaFiscal", "Serie", "IsNfce", Name = "nfce_saidas_idx")]
public partial class NfceSaida : IIdentifiable<long>
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("data")]
    public DateOnly? Data { get; set; }

    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("cliente")]
    public int Cliente { get; set; }

    [Column("requisicao")]
    [StringLength(10)]
    public string? Requisicao { get; set; }

    [Column("observacao")]
    [StringLength(16384)]
    public string? Observacao { get; set; }

    [Column("tp_saida")]
    [StringLength(2)]
    public string TpSaida { get; set; } = null!;

    [Column("tp_pagt")]
    [StringLength(1)]
    public string TpPagt { get; set; } = null!;

    [Column("dt_saida")]
    public DateOnly DtSaida { get; set; }

    [Column("dt_pedido")]
    public DateOnly? DtPedido { get; set; }

    [Column("tabela_venda")]
    [StringLength(4)]
    public string? TabelaVenda { get; set; }

    [Column("pagou")]
    [StringLength(1)]
    public string? Pagou { get; set; }

    [Column("cfop")]
    [StringLength(4)]
    public string? Cfop { get; set; }

    [Column("cd_carga")]
    public int? CdCarga { get; set; }

    [Column("txt_obs_nf")]
    [StringLength(16384)]
    public string? TxtObsNf { get; set; }

    [Column("paga_comissao")]
    [StringLength(1)]
    public string? PagaComissao { get; set; }

    [Column("nr_nota_fiscal")]
    [StringLength(25)]
    public string? NrNotaFiscal { get; set; }

    [Column("chave_acesso_nfe")]
    [StringLength(62)]
    public string? ChaveAcessoNfe { get; set; }

    [Column("cd_uf")]
    [StringLength(2)]
    public string? CdUf { get; set; }

    [Column("cd_situacao")]
    [StringLength(2)]
    public string? CdSituacao { get; set; }

    [Column("txt_justificativa_cancelamento")]
    [StringLength(16384)]
    public string? TxtJustificativaCancelamento { get; set; }

    [Column("nr_proto_cancelamento")]
    [StringLength(62)]
    public string? NrProtoCancelamento { get; set; }

    [Column("dt_pagou_comis")]
    public DateOnly? DtPagouComis { get; set; }

    [Column("xm_nf")]
    public string? XmNf { get; set; }

    [Column("vl_outro")]
    [Precision(18, 4)]
    public decimal? VlOutro { get; set; }

    [Column("vl_desc_global")]
    [Precision(18, 4)]
    public decimal? VlDescGlobal { get; set; }

    [Column("nr_autorizacao_nfe")]
    [StringLength(62)]
    public string? NrAutorizacaoNfe { get; set; }

    [Column("id_ponto_venda")]
    public int? IdPontoVenda { get; set; }

    [Column("chave_nfe_saida")]
    [StringLength(62)]
    public string? ChaveNfeSaida { get; set; }

    [Column("hr_saida")]
    [Precision(0, 0)]
    public TimeOnly HrSaida { get; set; }

    [Column("nr_cnf")]
    public int? NrCnf { get; set; }

    [Column("serie")]
    public int? Serie { get; set; }

    [Column("delivery")]
    [StringLength(1)]
    public string? Delivery { get; set; }

    [Column("frete")]
    [Precision(8, 2)]
    public decimal? Frete { get; set; }

    [Column("nr_protocolo_inutilizacao")]
    [StringLength(62)]
    public string? NrProtocoloInutilizacao { get; set; }

    [Column("local_salvo_nota")]
    [StringLength(256)]
    public string? LocalSalvoNota { get; set; }

    [Column("caixa")]
    public int? Caixa { get; set; }

    [Column("alterado")]
    [StringLength(1)]
    public string? Alterado { get; set; }

    [Column("desconto_classificacao")]
    [StringLength(1)]
    public string? DescontoClassificacao { get; set; }

    [Column("vl_desconto_classificacao")]
    [Precision(18, 2)]
    public decimal? VlDescontoClassificacao { get; set; }

    [Column("status_scanntech")]
    public int? StatusScanntech { get; set; }

    [Column("ret_scanntech")]
    public string? RetScanntech { get; set; }

    [Column("enviou_nao_cancelada")]
    [StringLength(1)]
    public string? EnviouNaoCancelada { get; set; }

    [Column("funcionario_ficha")]
    public int? FuncionarioFicha { get; set; }

    [Column("xm_nf_cnc")]
    public string? XmNfCnc { get; set; }

    [Column("chave_inc")]
    [StringLength(100)]
    public string? ChaveInc { get; set; }

    [Column("cd_animal")]
    public int? CdAnimal { get; set; }

    [Column("is_nfce")]
    public bool IsNfce { get; set; }

    [Column("nr_abertura_caixa")]
    public long NrAberturaCaixa { get; set; }

    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("NfceSaida")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("Cliente")]
    [InverseProperty("NfceSaida")]
    public virtual Cliente ClienteNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdUsuario")]
    [InverseProperty("NfceSaida")]
    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    [JsonIgnore]
    [InverseProperty("IdSaidaNavigation")]
    public virtual ICollection<NfceProdutoSaidum> NfceProdutoSaida { get; set; } = new List<NfceProdutoSaidum>();

    [JsonIgnore]
    [ForeignKey("NrAberturaCaixa")]
    [InverseProperty("NfceSaida")]
    public virtual NfceAberturaCaixa NrAberturaCaixaNavigation { get; set; } = null!;

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

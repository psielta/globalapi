﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[PrimaryKey("Id", "Empresa")]
[Table("nfce_saidas")]
[Index("Sequence", Name = "nfce_saidas_u_idx1", IsUnique = true)]
public partial class NfceSaida
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("data")]
    public DateOnly? Data { get; set; }

    [Key]
    [Column("empresa")]
    public int Empresa { get; set; }

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

    [Column("cd_vendedor")]
    public int? CdVendedor { get; set; }

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

    [Column("id_convenio")]
    public int? IdConvenio { get; set; }

    [Column("id_medico")]
    public int? IdMedico { get; set; }

    [Column("id_paciente")]
    public int? IdPaciente { get; set; }

    [Column("dt_cirugia")]
    public DateOnly? DtCirugia { get; set; }

    [Column("hr_saida")]
    [Precision(0, 0)]
    public TimeOnly? HrSaida { get; set; }

    [Column("nm_vendedor")]
    [StringLength(64)]
    public string? NmVendedor { get; set; }

    [Column("nr_abertura_caixa")]
    [StringLength(16)]
    public string? NrAberturaCaixa { get; set; }

    [Column("nr_cnf")]
    public int? NrCnf { get; set; }

    [Column("nm_operador")]
    [StringLength(40)]
    public string? NmOperador { get; set; }

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

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [Column("sequence")]
    public long Sequence { get; set; }

    [ForeignKey("Empresa")]
    [InverseProperty("NfceSaida")]
    public virtual Empresa EmpresaNavigation { get; set; } = null!;

    [InverseProperty("NfceSaida")]
    public virtual ICollection<NfceFormaPgt> NfceFormaPgts { get; set; } = new List<NfceFormaPgt>();

    [InverseProperty("NfceSaida")]
    public virtual ICollection<NfceProdutoSaidum> NfceProdutoSaida { get; set; } = new List<NfceProdutoSaidum>();

    [ForeignKey("Unity")]
    [InverseProperty("NfceSaida")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}

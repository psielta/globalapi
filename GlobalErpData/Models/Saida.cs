using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("saidas")]
public partial class Saida : IIdentifiable<int>
{
    [Key]
    [Column("nr_lanc")]
    public int NrLanc { get; set; }

    [Column("data")]
    public DateOnly? Data { get; set; }

    [Column("empresa")]
    public int Empresa { get; set; }

    [Column("cliente")]
    public int Cliente { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

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

    [Column("id_end_entrega")]
    public int? IdEndEntrega { get; set; }

    [Column("id_end_retirada")]
    public int? IdEndRetirada { get; set; }

    [Column("nr_cnf")]
    public int? NrCnf { get; set; }

    [Column("id_tabela_precos")]
    public int? IdTabelaPrecos { get; set; }

    [Column("id_tipo_operacao_intermediador")]
    public int? IdTipoOperacaoIntermediador { get; set; }

    [Column("id_tipo_indicador")]
    public int? IdTipoIndicador { get; set; }

    [Column("serie_nf")]
    [StringLength(4)]
    public string? SerieNf { get; set; }

    [Column("cnpj_market")]
    [StringLength(18)]
    public string? CnpjMarket { get; set; }

    [Column("nm_market")]
    [StringLength(162)]
    public string? NmMarket { get; set; }

    [Column("local_salvo_nota")]
    [StringLength(256)]
    public string? LocalSalvoNota { get; set; }

    [Column("tp_operacao")]
    [StringLength(1)]
    public string? TpOperacao { get; set; }

    [Column("xm_nf_cnc")]
    public string? XmNfCnc { get; set; }

    [Column("cd_grupo_estoque")]
    public int CdGrupoEstoque { get; set; }

    [Column("vl_seguro")]
    [Precision(18, 4)]
    public decimal? VlSeguro { get; set; }

    [JsonPropertyName("valorTotalNfe")]
    [NotMapped]
    public double? ValorTotalNfe { get; set; }

    [JsonPropertyName("subTotal")]
    [NotMapped]
    public double? SubTotal { get; set; }

    [JsonPropertyName("valorTotalDesconto")]
    [NotMapped]
    public double? ValorTotalDesconto { get; set; }

    [JsonPropertyName("valorTotalProdutos")]
    [NotMapped]
    public double? ValorTotalProdutos { get; set; }

    [Column("pdf")]
    public byte[]? Pdf { get; set; }

    [Column("pdf_cnc")]
    public byte[]? PdfCnc { get; set; }

    [Column("pdf_inu")]
    public byte[]? PdfInu { get; set; }

    [Column("xm_nf_inu")]
    public string? XmNfInu { get; set; }

    [Column("nr_proto_inu")]
    [StringLength(62)]
    public string? NrProtoInu { get; set; }

    [Column("unity")]
    public int Unity { get; set; }

    [JsonIgnore]
    [ForeignKey("CdGrupoEstoque")]
    [InverseProperty("Saida")]
    public virtual PlanoEstoque CdGrupoEstoqueNavigation { get; set; } = null!;

    [JsonPropertyName("nmPlano")]
    [NotMapped]
    public string NmPlano => CdGrupoEstoqueNavigation?.NmPlano ?? "";

    [JsonIgnore]
    [ForeignKey("Cliente")]
    [InverseProperty("Saida")]
    public virtual Cliente ClienteNavigation { get; set; } = null!;

    [JsonPropertyName("nmCliente")]
    [NotMapped]
    public string NmCliente => ClienteNavigation?.NmCliente ?? "";

    [JsonIgnore]
    [ForeignKey("Empresa")]
    [InverseProperty("Saida")]
    public virtual Empresa EmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [InverseProperty("NrSaidaNavigation")]
    public virtual ICollection<ProdutoSaidum> ProdutoSaida { get; set; } = new List<ProdutoSaidum>();

    [GraphQLIgnore]
    public int GetId()
    {
        return NrLanc;
    }

    [GraphQLIgnore]
    public string GetKeyName()
    {
        return "NrLanc";
    }

    [JsonIgnore]
    [InverseProperty("NrSaidaNavigation")]
    public virtual ICollection<SaidasVolume> SaidasVolumes { get; set; } = new List<SaidasVolume>();

    [JsonIgnore]
    [InverseProperty("NrSaidaNavigation")]
    public virtual ICollection<Frete> Fretes { get; set; } = new List<Frete>();

    [JsonIgnore]
    [InverseProperty("NrSaidaNavigation")]
    public virtual ICollection<SaidaNotasDevolucao> SaidaNotasDevolucaos { get; set; } = new List<SaidaNotasDevolucao>();

    [JsonIgnore]
    [ForeignKey("Unity")]
    [InverseProperty("Saida")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}

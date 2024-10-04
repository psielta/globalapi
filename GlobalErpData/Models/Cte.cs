using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("cte", Schema = "cte")]
[Index("NrCte", "IdEmpresa", "Modelo", "Serie", Name = "cte_idx", IsUnique = true)]
public partial class Cte
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nr_cte")]
    [StringLength(10)]
    public string NrCte { get; set; } = null!;

    [Column("serie")]
    [StringLength(3)]
    public string Serie { get; set; } = null!;

    [Column("dt_hr_emissao", TypeName = "timestamp(0) without time zone")]
    public DateTime? DtHrEmissao { get; set; }

    [Column("modelo")]
    [StringLength(2)]
    public string Modelo { get; set; } = null!;

    [Column("status")]
    [StringLength(2)]
    public string? Status { get; set; }

    [Column("cd_numerico")]
    [StringLength(9)]
    public string? CdNumerico { get; set; }

    [Column("cfop")]
    [StringLength(4)]
    public string Cfop { get; set; } = null!;

    [Column("modal")]
    [StringLength(2)]
    public string Modal { get; set; } = null!;

    [Column("tp_servico")]
    [StringLength(2)]
    public string TpServico { get; set; } = null!;

    [Column("forma_pagto")]
    [StringLength(2)]
    public string? FormaPagto { get; set; }

    [Column("finalidade_emissao")]
    [StringLength(2)]
    public string? FinalidadeEmissao { get; set; }

    [Column("forma_emissao")]
    [StringLength(2)]
    public string? FormaEmissao { get; set; }

    [Column("chave_acesso")]
    [StringLength(64)]
    public string? ChaveAcesso { get; set; }

    [Column("chave_acesso_referenc")]
    [StringLength(64)]
    public string? ChaveAcessoReferenc { get; set; }

    [Column("municipio_emissao")]
    [StringLength(10)]
    public string? MunicipioEmissao { get; set; }

    [Column("municipio_inicio_prestacao")]
    [StringLength(10)]
    public string? MunicipioInicioPrestacao { get; set; }

    [Column("municipio_fim_prestacao")]
    [StringLength(10)]
    public string? MunicipioFimPrestacao { get; set; }

    [Column("dados_retirada")]
    [StringLength(2)]
    public string? DadosRetirada { get; set; }

    [Column("detalhe")]
    [StringLength(16384)]
    public string? Detalhe { get; set; }

    [Column("caract_ad_transp")]
    [StringLength(15)]
    public string? CaractAdTransp { get; set; }

    [Column("caract_ad_servico")]
    [StringLength(30)]
    public string? CaractAdServico { get; set; }

    [Column("func_emissor_cte")]
    [StringLength(20)]
    public string? FuncEmissorCte { get; set; }

    [Column("municipio_origem_calc_frete")]
    [StringLength(10)]
    public string? MunicipioOrigemCalcFrete { get; set; }

    [Column("municipio_destino_calc_frete")]
    [StringLength(10)]
    public string? MunicipioDestinoCalcFrete { get; set; }

    [Column("cd_rota_entrega")]
    [StringLength(10)]
    public string? CdRotaEntrega { get; set; }

    [Column("origem_fluxo_caixa")]
    [StringLength(15)]
    public string? OrigemFluxoCaixa { get; set; }

    [Column("destino_fluxo_caixa")]
    [StringLength(15)]
    public string? DestinoFluxoCaixa { get; set; }

    [Column("previsao_data")]
    [StringLength(2)]
    public string? PrevisaoData { get; set; }

    [Column("previsao_hora")]
    [StringLength(2)]
    public string? PrevisaoHora { get; set; }

    [Column("dt_inicio_previsao")]
    public DateOnly? DtInicioPrevisao { get; set; }

    [Column("dt_fim_previsao")]
    public DateOnly? DtFimPrevisao { get; set; }

    [Column("hr_inicio_previsao")]
    [Precision(0, 0)]
    public TimeOnly? HrInicioPrevisao { get; set; }

    [Column("hr_fim_previsao")]
    [Precision(0, 0)]
    public TimeOnly? HrFimPrevisao { get; set; }

    [Column("tp_tomador_servico")]
    [StringLength(2)]
    public string? TpTomadorServico { get; set; }

    [Column("cd_tomador_servico")]
    public int? CdTomadorServico { get; set; }

    [Column("tp_remetente")]
    [StringLength(2)]
    public string? TpRemetente { get; set; }

    [Column("cd_remetente")]
    public int? CdRemetente { get; set; }

    [Column("tp_expedidor")]
    [StringLength(2)]
    public string? TpExpedidor { get; set; }

    [Column("cd_expedidor")]
    public int? CdExpedidor { get; set; }

    [Column("tp_recebedor")]
    [StringLength(2)]
    public string? TpRecebedor { get; set; }

    [Column("cd_recebedor")]
    public int? CdRecebedor { get; set; }

    [Column("tp_destinatario")]
    [StringLength(2)]
    public string? TpDestinatario { get; set; }

    [Column("cd_destinatario")]
    public int? CdDestinatario { get; set; }

    [Column("vl_prest_servico")]
    [Precision(18, 2)]
    public decimal? VlPrestServico { get; set; }

    [Column("vl_receber_prest_servico")]
    [Precision(18, 2)]
    public decimal? VlReceberPrestServico { get; set; }

    [Column("vl_tribt_prest_servico")]
    [Precision(18, 2)]
    public decimal? VlTribtPrestServico { get; set; }

    [Column("cd_st_icms")]
    [StringLength(2)]
    public string? CdStIcms { get; set; }

    [Column("porc_red_bc_icms")]
    [Precision(6, 2)]
    public decimal? PorcRedBcIcms { get; set; }

    [Column("vl_bc_icms")]
    [Precision(18, 2)]
    public decimal? VlBcIcms { get; set; }

    [Column("aliq_icms")]
    [Precision(6, 2)]
    public decimal? AliqIcms { get; set; }

    [Column("vl_icms")]
    [Precision(18, 2)]
    public decimal? VlIcms { get; set; }

    [Column("vl_cred_presumido")]
    [Precision(18, 2)]
    public decimal? VlCredPresumido { get; set; }

    [Column("inf_fisco_icms")]
    [StringLength(16384)]
    public string? InfFiscoIcms { get; set; }

    [Column("icms_uf_termino")]
    [StringLength(1)]
    public string? IcmsUfTermino { get; set; }

    [Column("vl_bc_icms_uft")]
    [Precision(18, 2)]
    public decimal? VlBcIcmsUft { get; set; }

    [Column("aliq_interna_uft")]
    [Precision(6, 2)]
    public decimal? AliqInternaUft { get; set; }

    [Column("aliq_interest_uft")]
    [Precision(6, 2)]
    public decimal? AliqInterestUft { get; set; }

    [Column("cd_part_uft")]
    [StringLength(3)]
    public string? CdPartUft { get; set; }

    [Column("porc_part_uft")]
    [Precision(6, 2)]
    public decimal? PorcPartUft { get; set; }

    [Column("vl_icms_part_ufi")]
    [Precision(18, 2)]
    public decimal? VlIcmsPartUfi { get; set; }

    [Column("vl_icms_part_uft")]
    [Precision(18, 2)]
    public decimal? VlIcmsPartUft { get; set; }

    [Column("porc_icms_fcp_uft")]
    [Precision(6, 2)]
    public decimal? PorcIcmsFcpUft { get; set; }

    [Column("vl_icms_fcp_uf")]
    [Precision(18, 2)]
    public decimal? VlIcmsFcpUf { get; set; }

    [Column("vl_carga")]
    [Precision(18, 2)]
    public decimal? VlCarga { get; set; }

    [Column("prod_predominante")]
    [StringLength(60)]
    public string? ProdPredominante { get; set; }

    [Column("outras_caract_prod")]
    [StringLength(30)]
    public string? OutrasCaractProd { get; set; }

    [Column("chave_cte_subst")]
    [StringLength(44)]
    public string? ChaveCteSubst { get; set; }

    [Column("tomador_cte_subst")]
    [StringLength(2)]
    public string? TomadorCteSubst { get; set; }

    [Column("tomador_nc")]
    [StringLength(2)]
    public string? TomadorNc { get; set; }

    [Column("chave_cte_tomador")]
    [StringLength(44)]
    public string? ChaveCteTomador { get; set; }

    [Column("chave_nfe_tomador")]
    [StringLength(44)]
    public string? ChaveNfeTomador { get; set; }

    [Column("tp_doc_tomador")]
    [StringLength(4)]
    public string? TpDocTomador { get; set; }

    [Column("cpf_tomador")]
    [StringLength(11)]
    public string? CpfTomador { get; set; }

    [Column("cnpj_tomador")]
    [StringLength(14)]
    public string? CnpjTomador { get; set; }

    [Column("cd_modelo_tomador")]
    [StringLength(2)]
    public string? CdModeloTomador { get; set; }

    [Column("serie_tomador")]
    [StringLength(1)]
    public string? SerieTomador { get; set; }

    [Column("subserie")]
    [StringLength(1)]
    public string? Subserie { get; set; }

    [Column("numero_tomador")]
    [StringLength(6)]
    public string? NumeroTomador { get; set; }

    [Column("vl_tomador")]
    [Precision(18, 2)]
    public decimal? VlTomador { get; set; }

    [Column("dt_tomador")]
    public DateOnly? DtTomador { get; set; }

    [Column("nr_fatura")]
    [StringLength(60)]
    public string? NrFatura { get; set; }

    [Column("vl_original_fatura")]
    [Precision(18, 2)]
    public decimal? VlOriginalFatura { get; set; }

    [Column("vl_desc_fatura")]
    [Precision(18, 2)]
    public decimal? VlDescFatura { get; set; }

    [Column("vl_liq_fatura")]
    [Precision(18, 2)]
    public decimal? VlLiqFatura { get; set; }

    [Column("rntcr")]
    [StringLength(8)]
    public string? Rntcr { get; set; }

    [Column("dt_prev_entrega")]
    public DateOnly? DtPrevEntrega { get; set; }

    [Column("indicador_lot")]
    [StringLength(1)]
    public string? IndicadorLot { get; set; }

    [Column("ciot")]
    [StringLength(12)]
    public string? Ciot { get; set; }

    [Column("chave_cte_anulacao")]
    [StringLength(44)]
    public string? ChaveCteAnulacao { get; set; }

    [Column("obs")]
    [StringLength(16384)]
    public string? Obs { get; set; }

    [Column("nr_autorizacao_cte")]
    [StringLength(62)]
    public string? NrAutorizacaoCte { get; set; }

    [Column("cd_situacao_cte")]
    [StringLength(2)]
    public string? CdSituacaoCte { get; set; }

    [Column("xml_cte")]
    public string? XmlCte { get; set; }

    [Column("txt_justificativa_cancelamento")]
    [StringLength(16384)]
    public string? TxtJustificativaCancelamento { get; set; }

    [Column("nr_proto_cancelamento")]
    [StringLength(62)]
    public string? NrProtoCancelamento { get; set; }

    [Column("txtdesc_servico_prestado")]
    [StringLength(16384)]
    public string? TxtdescServicoPrestado { get; set; }

    [Column("qt_passageiro")]
    public int? QtPassageiro { get; set; }

    [Column("nr_taf")]
    [StringLength(12)]
    public string? NrTaf { get; set; }

    [Column("nr_reg_estadual")]
    [StringLength(25)]
    public string? NrRegEstadual { get; set; }

    [Column("data_viagem")]
    public DateOnly? DataViagem { get; set; }

    [Column("hora_viagem")]
    [Precision(0, 0)]
    public TimeOnly? HoraViagem { get; set; }

    [Column("tipo_viagem")]
    [StringLength(1)]
    public string? TipoViagem { get; set; }

    [Column("nr_cnf")]
    public int? NrCnf { get; set; }

    [Column("inss")]
    [Precision(18, 4)]
    public decimal? Inss { get; set; }

    [Column("nr_cte_referenciado")]
    [StringLength(162)]
    public string? NrCteReferenciado { get; set; }

    [Column("retem_inss")]
    [StringLength(1)]
    public string? RetemInss { get; set; }

    [Column("id_empresa")]
    public int IdEmpresa { get; set; }

    [ForeignKey("IdEmpresa")]
    [InverseProperty("Ctes")]
    public virtual Empresa IdEmpresaNavigation { get; set; } = null!;
}

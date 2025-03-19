using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.MMModels;

[PrimaryKey("CdProduto", "Unity")]
[Table("produto_estoque")]
public partial class ProdutoEstoque
{
    [Key]
    [Column("cd_produto")]
    public int CdProduto { get; set; }

    [Column("nm_produto")]
    [StringLength(256)]
    public string NmProduto { get; set; } = null!;

    [Column("quant_minima")]
    [Precision(18, 4)]
    public decimal QuantMinima { get; set; }

    [Column("cd_barra")]
    [StringLength(14)]
    public string CdBarra { get; set; } = null!;

    [Column("cod_margem")]
    [StringLength(4)]
    public string? CodMargem { get; set; }

    [Column("cod_especie")]
    [StringLength(2)]
    public string? CodEspecie { get; set; }

    [Column("lanc_livro")]
    [StringLength(1)]
    public string LancLivro { get; set; } = null!;

    [Column("porc_vendedor")]
    [Precision(18, 2)]
    public decimal? PorcVendedor { get; set; }

    [Column("cd_class_fiscal")]
    [StringLength(8)]
    public string? CdClassFiscal { get; set; }

    [Column("cd_tribt")]
    public int CdTribt { get; set; }

    [Column("peso_bruto")]
    [Precision(18, 3)]
    public decimal? PesoBruto { get; set; }

    [Column("peso_liquido")]
    [Precision(18, 3)]
    public decimal? PesoLiquido { get; set; }

    [Column("qt_unitario")]
    [Precision(18, 4)]
    public decimal QtUnitario { get; set; }

    [Column("cd_grupo")]
    public int CdGrupo { get; set; }

    [Column("cd_ref")]
    public int CdRef { get; set; }

    [Column("qt_total")]
    [Precision(18, 4)]
    public decimal? QtTotal { get; set; }

    [Column("cd_seq")]
    public int? CdSeq { get; set; }

    [Column("suspenso")]
    [StringLength(1)]
    public string? Suspenso { get; set; }

    [Column("cd_plano_est")]
    public int? CdPlanoEst { get; set; }

    [Column("cd_uni")]
    [StringLength(4)]
    public string? CdUni { get; set; }

    [Column("porc_saida")]
    [Precision(18, 2)]
    public decimal? PorcSaida { get; set; }

    [Column("dt_suspenso")]
    public DateOnly? DtSuspenso { get; set; }

    [Column("hr_suspenso")]
    public TimeOnly? HrSuspenso { get; set; }

    [Column("dt_ativacao")]
    public DateOnly? DtAtivacao { get; set; }

    [Column("hr_ativacao")]
    public TimeOnly? HrAtivacao { get; set; }

    [Column("dt_ativo_livro")]
    public DateOnly? DtAtivoLivro { get; set; }

    [Column("dt_susp_livro")]
    public DateOnly? DtSuspLivro { get; set; }

    [Column("hr_ativo_livro")]
    public TimeOnly? HrAtivoLivro { get; set; }

    [Column("hr_susp_livro")]
    public TimeOnly? HrSuspLivro { get; set; }

    [Column("ativo")]
    [StringLength(1)]
    public string? Ativo { get; set; }

    [Column("desc_ref")]
    [StringLength(62)]
    public string? DescRef { get; set; }

    [Column("cd_tribt_fe")]
    public int? CdTribtFe { get; set; }

    [Column("mva")]
    [Precision(18, 2)]
    public decimal? Mva { get; set; }

    [Column("mvaajustado")]
    [Precision(18, 2)]
    public decimal? Mvaajustado { get; set; }

    [Column("foto")]
    public string? Foto { get; set; }

    [Column("cst_dentro1")]
    [StringLength(4)]
    public string? CstDentro1 { get; set; }

    [Column("cst_dentro2")]
    [StringLength(4)]
    public string? CstDentro2 { get; set; }

    [Column("cst_fora1")]
    [StringLength(4)]
    public string? CstFora1 { get; set; }

    [Column("cst_fora2")]
    [StringLength(4)]
    public string? CstFora2 { get; set; }

    [Column("cst_ipi")]
    [StringLength(4)]
    public string? CstIpi { get; set; }

    [Column("cst_pis")]
    [StringLength(4)]
    public string? CstPis { get; set; }

    [Column("cfo_dentro")]
    [StringLength(6)]
    public string? CfoDentro { get; set; }

    [Column("cfo_fora")]
    [StringLength(6)]
    public string? CfoFora { get; set; }

    [Column("porc_subst")]
    [Precision(18, 2)]
    public decimal? PorcSubst { get; set; }

    [Column("porc_ipi")]
    [Precision(18, 2)]
    public decimal? PorcIpi { get; set; }

    [Column("icms_dentro")]
    [Precision(18, 2)]
    public decimal? IcmsDentro { get; set; }

    [Column("icms_fora")]
    [Precision(18, 2)]
    public decimal? IcmsFora { get; set; }

    [Column("ex_tipi")]
    [StringLength(3)]
    public string? ExTipi { get; set; }

    [Column("cd_genero")]
    [StringLength(4)]
    public string? CdGenero { get; set; }

    [Column("tp_item")]
    [StringLength(2)]
    public string? TpItem { get; set; }

    [Column("letra_curvaabc")]
    [StringLength(1)]
    public string? LetraCurvaabc { get; set; }

    [Column("porc_venda_a_prazo")]
    [Precision(18, 2)]
    public decimal? PorcVendaAPrazo { get; set; }

    [Column("porc_venda_cd")]
    [Precision(18, 2)]
    public decimal? PorcVendaCd { get; set; }

    [Column("porc_venda_cc")]
    [Precision(18, 2)]
    public decimal? PorcVendaCc { get; set; }

    [Column("qtde_max")]
    [Precision(18, 4)]
    public decimal? QtdeMax { get; set; }

    [Column("corredor")]
    [StringLength(4)]
    public string? Corredor { get; set; }

    [Column("prateleira")]
    [StringLength(4)]
    public string? Prateleira { get; set; }

    [Column("txt_obs")]
    [StringLength(16384)]
    public string? TxtObs { get; set; }

    [Column("cd_interno")]
    [StringLength(62)]
    public string? CdInterno { get; set; }

    [Column("st_ecf")]
    [StringLength(4)]
    public string? StEcf { get; set; }

    [Column("cd_csosn")]
    [StringLength(10)]
    public string? CdCsosn { get; set; }

    [Column("codigo_balanca")]
    [StringLength(10)]
    public string? CodigoBalanca { get; set; }

    [Column("iat")]
    [StringLength(1)]
    public string? Iat { get; set; }

    [Column("ippt")]
    [StringLength(1)]
    public string? Ippt { get; set; }

    [Column("tipo_item_sped")]
    [StringLength(2)]
    public string? TipoItemSped { get; set; }

    [Column("taxa_pis")]
    [Precision(18, 2)]
    public decimal? TaxaPis { get; set; }

    [Column("taxa_issqn")]
    [Precision(18, 2)]
    public decimal? TaxaIssqn { get; set; }

    [Column("taxa_cofins")]
    [Precision(18, 2)]
    public decimal? TaxaCofins { get; set; }

    [Column("totalizador_parcial")]
    [StringLength(10)]
    public string? TotalizadorParcial { get; set; }

    [Column("vl_a_vista")]
    [Precision(18, 4)]
    public decimal? VlAVista { get; set; }

    [Column("vl_prazo")]
    [Precision(18, 4)]
    public decimal? VlPrazo { get; set; }

    [Column("vl_cc")]
    [Precision(18, 4)]
    public decimal? VlCc { get; set; }

    [Column("vl_deb")]
    [Precision(18, 4)]
    public decimal? VlDeb { get; set; }

    [Column("ecf_icm_st")]
    [StringLength(4)]
    public string? EcfIcmSt { get; set; }

    [Column("tp_cd_balanca")]
    public short? TpCdBalanca { get; set; }

    [Column("cst_cofins")]
    [StringLength(4)]
    public string? CstCofins { get; set; }

    [Column("balanca")]
    [StringLength(1)]
    public string? Balanca { get; set; }

    [Column("qt_dias_venc")]
    public short? QtDiasVenc { get; set; }

    [Column("vl_tabela_gov")]
    [Precision(18, 4)]
    public decimal? VlTabelaGov { get; set; }

    [Column("porc_aliq_interna")]
    [Precision(18, 2)]
    public decimal? PorcAliqInterna { get; set; }

    [Column("vl_comanda")]
    [Precision(18, 4)]
    public decimal? VlComanda { get; set; }

    [Column("classe_terapeutica")]
    [StringLength(4)]
    public string? ClasseTerapeutica { get; set; }

    [Column("reg_ms")]
    [StringLength(13)]
    public string? RegMs { get; set; }

    [Column("codigo_dcb")]
    [StringLength(5)]
    public string? CodigoDcb { get; set; }

    [Column("descricao_produto")]
    [StringLength(16384)]
    public string? DescricaoProduto { get; set; }

    [Column("cd_anp")]
    public int? CdAnp { get; set; }

    [Column("vl_media")]
    [Precision(18, 4)]
    public decimal? VlMedia { get; set; }

    [Column("vl_pequena")]
    [Precision(18, 4)]
    public decimal? VlPequena { get; set; }

    [Column("transferiu")]
    [StringLength(1)]
    public string? Transferiu { get; set; }

    [Column("cest")]
    [StringLength(7)]
    public string? Cest { get; set; }

    [Column("vl_custo")]
    [Precision(18, 4)]
    public decimal? VlCusto { get; set; }

    [Column("vl_atacado")]
    [Precision(18, 4)]
    public decimal? VlAtacado { get; set; }

    [Column("id_marca")]
    public int? IdMarca { get; set; }

    [Column("local")]
    [StringLength(128)]
    public string? Local { get; set; }

    [Column("bandeja_gaveta")]
    [StringLength(128)]
    public string? BandejaGaveta { get; set; }

    [Column("ent_mva")]
    [Precision(18, 2)]
    public decimal? EntMva { get; set; }

    [Column("ent_porc_st")]
    [Precision(18, 2)]
    public decimal? EntPorcSt { get; set; }

    [Column("ent_reducao_bc")]
    [Precision(18, 2)]
    public decimal? EntReducaoBc { get; set; }

    [Column("ent_bc_st")]
    [Precision(18, 2)]
    public decimal? EntBcSt { get; set; }

    [Column("ent_icms_st")]
    [Precision(18, 2)]
    public decimal? EntIcmsSt { get; set; }

    [Column("icms_subs_aliq")]
    [Precision(18, 2)]
    public decimal? IcmsSubsAliq { get; set; }

    [Column("icms_subs_reducao")]
    [Precision(18, 2)]
    public decimal? IcmsSubsReducao { get; set; }

    [Column("icms_subs_reducao_aliq")]
    [Precision(18, 2)]
    public decimal? IcmsSubsReducaoAliq { get; set; }

    [Column("lucro_por")]
    [Precision(18, 4)]
    public decimal? LucroPor { get; set; }

    [Column("operacional_por")]
    [Precision(5, 2)]
    public decimal? OperacionalPor { get; set; }

    [Column("frete")]
    [Precision(5, 2)]
    public decimal? Frete { get; set; }

    [Column("nome_imagem")]
    [StringLength(162)]
    public string? NomeImagem { get; set; }

    [Column("controla_estoque")]
    [StringLength(1)]
    public string? ControlaEstoque { get; set; }

    [Column("vl_custo_variavel")]
    [Precision(18, 2)]
    public decimal? VlCustoVariavel { get; set; }

    [Column("principal")]
    [StringLength(1)]
    public string? Principal { get; set; }

    [Column("embalagem")]
    [StringLength(62)]
    public string? Embalagem { get; set; }

    [Column("capacidade")]
    [StringLength(62)]
    public string? Capacidade { get; set; }

    [Column("vl_cheque")]
    [Precision(18, 4)]
    public decimal? VlCheque { get; set; }

    [Column("vl_credito_parcelado")]
    [Precision(18, 4)]
    public decimal? VlCreditoParcelado { get; set; }

    [Column("vl_boleto")]
    [Precision(18, 4)]
    public decimal? VlBoleto { get; set; }

    [Column("porc_limite_avista")]
    [Precision(18, 2)]
    public decimal? PorcLimiteAvista { get; set; }

    [Column("porc_limite_aprazo")]
    [Precision(18, 2)]
    public decimal? PorcLimiteAprazo { get; set; }

    [Column("porc_limite_credito")]
    [Precision(18, 2)]
    public decimal? PorcLimiteCredito { get; set; }

    [Column("porc_limite_debito")]
    [Precision(18, 2)]
    public decimal? PorcLimiteDebito { get; set; }

    [Column("porc_limite_creditoparc")]
    [Precision(18, 2)]
    public decimal? PorcLimiteCreditoparc { get; set; }

    [Column("porc_limite_boleto")]
    [Precision(18, 2)]
    public decimal? PorcLimiteBoleto { get; set; }

    [Column("porc_limite_cheque")]
    [Precision(18, 2)]
    public decimal? PorcLimiteCheque { get; set; }

    [Column("porc_desgaste_equipamento")]
    [Precision(18, 4)]
    public decimal? PorcDesgasteEquipamento { get; set; }

    [Column("custo_adicional")]
    [Precision(18, 4)]
    public decimal? CustoAdicional { get; set; }

    [Column("porc_mao_obra")]
    [Precision(18, 4)]
    public decimal? PorcMaoObra { get; set; }

    [Column("margem_lucro_atacado")]
    [Precision(18, 4)]
    public decimal? MargemLucroAtacado { get; set; }

    [Column("vl_n_fiscal")]
    [Precision(18, 4)]
    public decimal? VlNFiscal { get; set; }

    [Column("operacional_n_fiscal_por")]
    [Precision(18, 4)]
    public decimal? OperacionalNFiscalPor { get; set; }

    [Column("lucro_por_n_fiscal")]
    [Precision(18, 4)]
    public decimal? LucroPorNFiscal { get; set; }

    [Column("percentual_impostos")]
    [Precision(18, 4)]
    public decimal? PercentualImpostos { get; set; }

    [Column("percentual_comissao")]
    [Precision(18, 4)]
    public decimal? PercentualComissao { get; set; }

    [Column("percentual_custo_fixo")]
    [Precision(18, 4)]
    public decimal? PercentualCustoFixo { get; set; }

    [Column("percentual_lucro_liquido_fiscal")]
    [Precision(18, 4)]
    public decimal? PercentualLucroLiquidoFiscal { get; set; }

    [Column("indice_markup_fiscal")]
    [Precision(18, 4)]
    public decimal? IndiceMarkupFiscal { get; set; }

    [Column("dt_alt_preco", TypeName = "timestamp(0) without time zone")]
    public DateTime? DtAltPreco { get; set; }

    [Column("section_id")]
    public int? SectionId { get; set; }

    [Column("section_item_id")]
    public int? SectionItemId { get; set; }

    [Column("featured_id")]
    public int? FeaturedId { get; set; }

    [Column("category")]
    public int? Category { get; set; }

    [Column("cd_produto_erp")]
    public int? CdProdutoErp { get; set; }

    [Column("dt_cadastro", TypeName = "timestamp(0) without time zone")]
    public DateTime DtCadastro { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [Key]
    [Column("unity")]
    public int Unity { get; set; }

    [ForeignKey("Category")]
    [InverseProperty("ProdutoEstoques")]
    public virtual Category? CategoryNavigation { get; set; }

    [ForeignKey("CdGrupo")]
    [InverseProperty("ProdutoEstoques")]
    public virtual GrupoEstoque CdGrupoNavigation { get; set; } = null!;

    [ForeignKey("CdRef")]
    [InverseProperty("ProdutoEstoques")]
    public virtual ReferenciaEstoque CdRefNavigation { get; set; } = null!;

    [InverseProperty("ProdutoEstoque")]
    public virtual ICollection<FotosProduto> FotosProdutos { get; set; } = new List<FotosProduto>();

    [InverseProperty("ProdutoEstoque")]
    public virtual ICollection<NfceProdutoSaidum> NfceProdutoSaida { get; set; } = new List<NfceProdutoSaidum>();

    [InverseProperty("ProdutoEstoque")]
    public virtual ICollection<OlderItem> OlderItems { get; set; } = new List<OlderItem>();

    [InverseProperty("ProdutoEstoque")]
    public virtual ICollection<ProductDetail> ProductDetails { get; set; } = new List<ProductDetail>();

    [InverseProperty("ProdutoEstoque")]
    public virtual ICollection<ProdutoEntradum> ProdutoEntrada { get; set; } = new List<ProdutoEntradum>();

    [InverseProperty("ProdutoEstoque")]
    public virtual ICollection<ProdutoSaidum> ProdutoSaida { get; set; } = new List<ProdutoSaidum>();

    [InverseProperty("ProdutoEstoque")]
    public virtual ICollection<ProdutosForn> ProdutosForns { get; set; } = new List<ProdutosForn>();

    [InverseProperty("ProdutoEstoque")]
    public virtual ICollection<SaldoEstoque> SaldoEstoques { get; set; } = new List<SaldoEstoque>();

    [ForeignKey("SectionId")]
    [InverseProperty("ProdutoEstoques")]
    public virtual Section? Section { get; set; }

    [ForeignKey("SectionItemId")]
    [InverseProperty("ProdutoEstoques")]
    public virtual SectionItem? SectionItem { get; set; }

    [ForeignKey("Unity")]
    [InverseProperty("ProdutoEstoques")]
    public virtual Unity UnityNavigation { get; set; } = null!;
}

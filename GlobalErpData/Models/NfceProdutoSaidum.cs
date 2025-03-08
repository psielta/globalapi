using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[PrimaryKey("Id", "CdEmpresa")]
[Table("nfce_produto_saida")]
public partial class NfceProdutoSaidum : IIdentifiableMultiKey<int, int>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("id_saida")]
    public int IdSaida { get; set; }

    [Column("cd_produto")]
    public int CdProduto { get; set; }

    [Column("cd_plano")]
    public int CdPlano { get; set; }

    [Column("quant")]
    [Precision(18, 4)]
    public decimal Quant { get; set; }

    [Column("vl_venda")]
    [Precision(18, 4)]
    public decimal VlVenda { get; set; }

    [Column("desconto")]
    [Precision(18, 4)]
    public decimal Desconto { get; set; }

    [Column("vl_total")]
    [Precision(18, 4)]
    public decimal VlTotal { get; set; }

    [Column("lote")]
    [StringLength(16635)]
    public string Lote { get; set; } = null!;

    [Column("dt_validade")]
    public DateOnly DtValidade { get; set; }

    [Key]
    [Column("cd_empresa")]
    public int CdEmpresa { get; set; }

    [Column("nm_produto")]
    [StringLength(256)]
    public string NmProduto { get; set; } = null!;

    [Column("pagou")]
    [StringLength(1)]
    public string? Pagou { get; set; }

    [Column("vl_custo")]
    [Precision(18, 4)]
    public decimal? VlCusto { get; set; }

    [Column("quant_estorno")]
    [Precision(18, 4)]
    public decimal? QuantEstorno { get; set; }

    [Column("cst")]
    [StringLength(6)]
    public string? Cst { get; set; }

    [Column("cfop")]
    [StringLength(6)]
    public string? Cfop { get; set; }

    [Column("un")]
    [StringLength(4)]
    public string? Un { get; set; }

    [Column("ncm")]
    [StringLength(8)]
    public string? Ncm { get; set; }

    [Column("vl_comissao")]
    [Precision(18, 4)]
    public decimal? VlComissao { get; set; }

    [Column("poc_icms")]
    [Precision(18, 2)]
    public decimal? PocIcms { get; set; }

    [Column("vl_base_icms")]
    [Precision(18, 4)]
    public decimal? VlBaseIcms { get; set; }

    [Column("vl_icms")]
    [Precision(18, 4)]
    public decimal? VlIcms { get; set; }

    [Column("ipi_si_tribut")]
    [StringLength(4)]
    public string? IpiSiTribut { get; set; }

    [Column("ipi_clas_enqua")]
    [StringLength(4)]
    public string? IpiClasEnqua { get; set; }

    [Column("cd_selo_ipi")]
    [StringLength(4)]
    public string? CdSeloIpi { get; set; }

    [Column("qtde_selo_ipi")]
    public short? QtdeSeloIpi { get; set; }

    [Column("cd_enquadra_ipi")]
    [StringLength(4)]
    public string? CdEnquadraIpi { get; set; }

    [Column("vl_unid")]
    [Precision(18, 4)]
    public decimal? VlUnid { get; set; }

    [Column("cnpj_prod")]
    [StringLength(18)]
    public string? CnpjProd { get; set; }

    [Column("vl_base_ipi")]
    [Precision(18, 4)]
    public decimal? VlBaseIpi { get; set; }

    [Column("porc_ipi")]
    [Precision(18, 2)]
    public decimal? PorcIpi { get; set; }

    [Column("vl_ipi")]
    [Precision(18, 4)]
    public decimal? VlIpi { get; set; }

    [Column("vl_base_st")]
    [Precision(18, 4)]
    public decimal? VlBaseSt { get; set; }

    [Column("porc_st")]
    [Precision(18, 2)]
    public decimal? PorcSt { get; set; }

    [Column("vl_st")]
    [Precision(18, 4)]
    public decimal? VlSt { get; set; }

    [Column("poc_reducao")]
    [Precision(18, 2)]
    public decimal? PocReducao { get; set; }

    [Column("mva_st")]
    [Precision(18, 2)]
    public decimal? MvaSt { get; set; }

    [Column("nr_item")]
    public short? NrItem { get; set; }

    [Column("cancelou")]
    [StringLength(1)]
    public string? Cancelou { get; set; }

    [Column("e_or_os")]
    [StringLength(1)]
    public string? EOrOs { get; set; }

    [Column("vl_base_retido")]
    [Precision(18, 4)]
    public decimal? VlBaseRetido { get; set; }

    [Column("vl_icms_ret")]
    [Precision(18, 4)]
    public decimal? VlIcmsRet { get; set; }

    [Column("cd_csosn")]
    [StringLength(10)]
    public string? CdCsosn { get; set; }

    [Column("pcredito_icms")]
    [Precision(18, 2)]
    public decimal? PcreditoIcms { get; set; }

    [Column("vl_credito_icms")]
    [Precision(18, 4)]
    public decimal? VlCreditoIcms { get; set; }

    [Column("vl_aprox_imposto")]
    [Precision(18, 4)]
    public decimal? VlAproxImposto { get; set; }

    [Column("porc_ibpt")]
    [Precision(18, 2)]
    public decimal? PorcIbpt { get; set; }

    [Column("qt_total")]
    [Precision(18, 4)]
    public decimal? QtTotal { get; set; }

    [Column("cd_tip_st")]
    [StringLength(2)]
    public string? CdTipSt { get; set; }

    [Column("vl_base_pis")]
    [Precision(18, 4)]
    public decimal? VlBasePis { get; set; }

    [Column("vl_pis")]
    [Precision(18, 4)]
    public decimal? VlPis { get; set; }

    [Column("porc_pis")]
    [Precision(18, 2)]
    public decimal? PorcPis { get; set; }

    [Column("cst_pis")]
    [StringLength(6)]
    public string? CstPis { get; set; }

    [Column("cd_cont_social_ap_pis")]
    [StringLength(4)]
    public string? CdContSocialApPis { get; set; }

    [Column("vl_base_cofins")]
    [Precision(18, 4)]
    public decimal? VlBaseCofins { get; set; }

    [Column("vl_cofins")]
    [Precision(18, 4)]
    public decimal? VlCofins { get; set; }

    [Column("porc_cofins")]
    [Precision(18, 2)]
    public decimal? PorcCofins { get; set; }

    [Column("cst_cofins")]
    [StringLength(6)]
    public string? CstCofins { get; set; }

    [Column("cd_cont_social_ap_cofins")]
    [StringLength(4)]
    public string? CdContSocialApCofins { get; set; }

    [Column("vl_base_pis_st")]
    [Precision(18, 4)]
    public decimal? VlBasePisSt { get; set; }

    [Column("vl_pis_st")]
    [Precision(18, 4)]
    public decimal? VlPisSt { get; set; }

    [Column("porc_pis_st")]
    [Precision(18, 2)]
    public decimal? PorcPisSt { get; set; }

    [Column("vl_base_cofins_st")]
    [Precision(18, 4)]
    public decimal? VlBaseCofinsSt { get; set; }

    [Column("vl_cofins_st")]
    [Precision(18, 4)]
    public decimal? VlCofinsSt { get; set; }

    [Column("porc_cofins_st")]
    [Precision(18, 2)]
    public decimal? PorcCofinsSt { get; set; }

    [Column("cest")]
    [StringLength(7)]
    public string? Cest { get; set; }

    [Column("vbcufdest")]
    [Precision(18, 4)]
    public decimal? Vbcufdest { get; set; }

    [Column("picmsufdest")]
    [Precision(18, 2)]
    public decimal? Picmsufdest { get; set; }

    [Column("vicmsufdest")]
    [Precision(18, 4)]
    public decimal? Vicmsufdest { get; set; }

    [Column("vicmsufremt")]
    [Precision(18, 4)]
    public decimal? Vicmsufremt { get; set; }

    [Column("vbcfcpufdest")]
    [Precision(18, 4)]
    public decimal? Vbcfcpufdest { get; set; }

    [Column("pfcpufdest")]
    [Precision(18, 2)]
    public decimal? Pfcpufdest { get; set; }

    [Column("vfcpufdest")]
    [Precision(18, 4)]
    public decimal? Vfcpufdest { get; set; }

    [Column("picmsinter")]
    [Precision(18, 2)]
    public decimal? Picmsinter { get; set; }

    [Column("picmsinterpart")]
    [Precision(18, 2)]
    public decimal? Picmsinterpart { get; set; }

    [Column("cd_interno")]
    [StringLength(62)]
    public string? CdInterno { get; set; }

    [Column("sequencia_item")]
    public int? SequenciaItem { get; set; }

    [Column("icms_substituto")]
    [Precision(18, 4)]
    public decimal? IcmsSubstituto { get; set; }

    [Column("st")]
    [Precision(18, 4)]
    public decimal? St { get; set; }

    [Column("id_promo_scanntech")]
    [StringLength(50)]
    public string? IdPromoScanntech { get; set; }

    [Column("id_prevenda")]
    [StringLength(50)]
    public string? IdPrevenda { get; set; }

    [Column("id_os")]
    [StringLength(50)]
    public string? IdOs { get; set; }

    [Column("cor")]
    [StringLength(50)]
    public string? Cor { get; set; }

    [Column("tamanho")]
    [StringLength(50)]
    public string? Tamanho { get; set; }

    [Column("genero")]
    [StringLength(50)]
    public string? Genero { get; set; }

    [Column("cd_produto_original")]
    public int? CdProdutoOriginal { get; set; }

    [Column("cd_plano_secundario")]
    public int? CdPlanoSecundario { get; set; }

    [Column("last_update", TypeName = "timestamp without time zone")]
    public DateTime? LastUpdate { get; set; }

    [Column("integrated")]
    public int? Integrated { get; set; }

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("NfceProdutoSaida")]
    public virtual Empresa CdEmpresaNavigation { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("IdSaida, CdEmpresa")]
    [InverseProperty("NfceProdutoSaida")]
    public virtual NfceSaida NfceSaida { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("CdProduto, CdEmpresa")]
    [InverseProperty("NfceProdutoSaida")]
    public virtual ProdutoEstoque ProdutoEstoque { get; set; } = null!;

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

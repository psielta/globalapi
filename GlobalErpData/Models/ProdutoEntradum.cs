using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using GlobalLib.Database;
using Microsoft.EntityFrameworkCore;

namespace GlobalErpData.Models;

[Table("produto_entrada")]
public partial class ProdutoEntradum : IIdentifiable<int>
{
    [Key]
    [Column("nr")]
    public int Nr { get; set; }

    [Column("nr_entrada")]
    public int NrEntrada { get; set; }

    [Column("cd_empresa")]
    public int? CdEmpresa { get; set; }

    [Column("cd_produto")]
    public int CdProduto { get; set; }

    [Column("cd_barra")]
    [StringLength(16)]
    public string? CdBarra { get; set; }

    [Column("quant")]
    [Precision(18, 4)]
    public decimal Quant { get; set; }

    [Column("vl_unitario")]
    [Precision(18, 4)]
    public decimal VlUnitario { get; set; }

    [Column("b_icms")]
    [Precision(18, 4)]
    public decimal? BIcms { get; set; }

    [Column("porc_icms")]
    [Precision(18, 2)]
    public decimal? PorcIcms { get; set; }

    [Column("vl_icms")]
    [Precision(18, 4)]
    public decimal VlIcms { get; set; }

    [Column("cd_plano")]
    public int CdPlano { get; set; }

    [Column("lote")]
    [StringLength(26)]
    public string? Lote { get; set; }

    [Column("dt_validade")]
    public DateOnly? DtValidade { get; set; }

    [Column("base_ipi")]
    [Precision(18, 4)]
    public decimal? BaseIpi { get; set; }

    [Column("porc_ipi")]
    [Precision(18, 2)]
    public decimal? PorcIpi { get; set; }

    [Column("vl_ipi")]
    [Precision(18, 4)]
    public decimal VlIpi { get; set; }

    [Column("vl_outras")]
    [Precision(18, 4)]
    public decimal? VlOutras { get; set; }

    [Column("transferiu")]
    [StringLength(1)]
    public string? Transferiu { get; set; }

    [Column("unidade")]
    [StringLength(4)]
    public string? Unidade { get; set; }

    [Column("cd_class_fiscal")]
    [StringLength(8)]
    public string? CdClassFiscal { get; set; }

    [Column("cod_sub_trib")]
    [StringLength(6)]
    public string? CodSubTrib { get; set; }

    [Column("cd_cfop")]
    [StringLength(4)]
    public string? CdCfop { get; set; }

    [Column("nr_item")]
    public short? NrItem { get; set; }

    [Column("vl_pis")]
    [Precision(18, 4)]
    public decimal? VlPis { get; set; }

    [Column("vl_confins")]
    [Precision(18, 4)]
    public decimal? VlConfins { get; set; }

    [Column("cst")]
    [StringLength(6)]
    public string? Cst { get; set; }

    [Column("vl_icms_st")]
    [Precision(18, 4)]
    public decimal? VlIcmsSt { get; set; }

    [Column("qt_total")]
    [Precision(18, 4)]
    public decimal? QtTotal { get; set; }

    [Column("vl_base_st")]
    [Precision(18, 4)]
    public decimal? VlBaseSt { get; set; }

    [Column("porc_st")]
    [Precision(18, 2)]
    public decimal? PorcSt { get; set; }

    [Column("porc_pis")]
    [Precision(18, 2)]
    public decimal? PorcPis { get; set; }

    [Column("porc_confins")]
    [Precision(18, 2)]
    public decimal? PorcConfins { get; set; }

    [Column("vl_base_pis")]
    [Precision(18, 4)]
    public decimal? VlBasePis { get; set; }

    [Column("vl_base_confins")]
    [Precision(18, 4)]
    public decimal? VlBaseConfins { get; set; }

    [Column("cst_pis")]
    [StringLength(6)]
    public string? CstPis { get; set; }

    [Column("cst_confins")]
    [StringLength(6)]
    public string? CstConfins { get; set; }

    [Column("cd_tip_st")]
    [StringLength(2)]
    public string? CdTipSt { get; set; }

    [Column("cd_cont_social_ap_pis")]
    [StringLength(4)]
    public string? CdContSocialApPis { get; set; }

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

    [Column("cst_ipi")]
    [StringLength(6)]
    public string? CstIpi { get; set; }

    [Column("cd_enquadra_ipi")]
    [StringLength(2)]
    public string? CdEnquadraIpi { get; set; }

    [Column("frete_produto")]
    [Precision(18, 4)]
    public decimal? FreteProduto { get; set; }

    [Column("mov_estoque")]
    [StringLength(1)]
    public string? MovEstoque { get; set; }

    [Column("vl_desp_acess")]
    [Precision(18, 2)]
    public decimal? VlDespAcess { get; set; }

    [Column("fcp_base")]
    [Precision(18, 2)]
    public decimal? FcpBase { get; set; }

    [Column("fcp_porc")]
    [Precision(18, 2)]
    public decimal? FcpPorc { get; set; }

    [Column("fcp_valor")]
    [Precision(18, 2)]
    public decimal? FcpValor { get; set; }

    [Column("imp_base_st_ret")]
    [Precision(18, 4)]
    public decimal? ImpBaseStRet { get; set; }

    [Column("imp_base_icms_st_ret")]
    [Precision(18, 4)]
    public decimal? ImpBaseIcmsStRet { get; set; }

    [Column("imp_pst")]
    [Precision(18, 4)]
    public decimal? ImpPst { get; set; }

    [Column("imp_icms_prop_subs")]
    [Precision(18, 4)]
    public decimal? ImpIcmsPropSubs { get; set; }

    [Column("frete_rateio")]
    [Precision(18, 2)]
    public decimal? FreteRateio { get; set; }

    [Column("tp_operacao_veic")]
    public int? TpOperacaoVeic { get; set; }

    [Column("chasi_veic")]
    [StringLength(17)]
    public string? ChasiVeic { get; set; }

    [Column("cor_veic")]
    [StringLength(4)]
    public string? CorVeic { get; set; }

    [Column("desc_cor_veic")]
    [StringLength(40)]
    public string? DescCorVeic { get; set; }

    [Column("potencia_motor_veic")]
    [StringLength(4)]
    public string? PotenciaMotorVeic { get; set; }

    [Column("cilindradas_veic")]
    [StringLength(4)]
    public string? CilindradasVeic { get; set; }

    [Column("peso_liquido_veic")]
    [Precision(12, 4)]
    public decimal? PesoLiquidoVeic { get; set; }

    [Column("peso_bruto_veic")]
    [Precision(12, 4)]
    public decimal? PesoBrutoVeic { get; set; }

    [Column("serial_veic")]
    [StringLength(9)]
    public string? SerialVeic { get; set; }

    [Column("tp_combust_veic")]
    [StringLength(2)]
    public string? TpCombustVeic { get; set; }

    [Column("nr_motor_veic")]
    [StringLength(21)]
    public string? NrMotorVeic { get; set; }

    [Column("capc_max_trac_veic")]
    [Precision(12, 4)]
    public decimal? CapcMaxTracVeic { get; set; }

    [Column("dist_eixos_veic")]
    [StringLength(4)]
    public string? DistEixosVeic { get; set; }

    [Column("ano_veic")]
    [StringLength(4)]
    public string? AnoVeic { get; set; }

    [Column("ano_fab_veic")]
    [StringLength(4)]
    public string? AnoFabVeic { get; set; }

    [Column("tp_pintura_veic")]
    [StringLength(1)]
    public string? TpPinturaVeic { get; set; }

    [Column("tp_veic")]
    [StringLength(2)]
    public string? TpVeic { get; set; }

    [Column("espec_veic")]
    [StringLength(1)]
    public string? EspecVeic { get; set; }

    [Column("id_vin_veic")]
    [StringLength(1)]
    public string? IdVinVeic { get; set; }

    [Column("cond_veic")]
    [StringLength(1)]
    public string? CondVeic { get; set; }

    [Column("id_marca_veic")]
    [StringLength(6)]
    public string? IdMarcaVeic { get; set; }

    [Column("id_cor_veic")]
    [StringLength(2)]
    public string? IdCorVeic { get; set; }

    [Column("capc_max_lot_veic")]
    [StringLength(3)]
    public string? CapcMaxLotVeic { get; set; }

    [Column("restricao_veic")]
    [StringLength(1)]
    public string? RestricaoVeic { get; set; }

    [Column("v_icms_deson")]
    [Precision(18, 4)]
    public decimal? VIcmsDeson { get; set; }

    [Column("tamanho")]
    [StringLength(50)]
    public string? Tamanho { get; set; }

    [Column("cor")]
    [StringLength(50)]
    public string? Cor { get; set; }

    [Column("genero")]
    [StringLength(50)]
    public string? Genero { get; set; }

    [JsonPropertyName("nmProduto")]
    [NotMapped]
    public string NmProduto => ProdutoEstoque?.NmProduto ?? "";

    [JsonIgnore]
    [ForeignKey("CdEmpresa")]
    [InverseProperty("ProdutoEntrada")]
    public virtual Empresa? CdEmpresaNavigation { get; set; }

    [JsonIgnore]
    [ForeignKey("NrEntrada, CdEmpresa")]
    [InverseProperty("ProdutoEntrada")]
    public virtual Entrada? Entrada { get; set; }

    [JsonIgnore]
    [ForeignKey("CdProduto, CdEmpresa")]
    [InverseProperty("ProdutoEntrada")]
    public virtual ProdutoEstoque? ProdutoEstoque { get; set; }

    public int GetId()
    {
        return this.Nr;
    }

    public string GetKeyName()
    {
        return "Nr";
    }
}
